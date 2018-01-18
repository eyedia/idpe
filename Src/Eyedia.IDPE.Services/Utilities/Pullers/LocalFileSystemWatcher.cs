#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Deb'jyoti Das
Created - 3/19/2013 11:14:16 AM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice




using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Web.Hosting;
using System.Threading;
using System.Globalization;
using Eyedia.IDPE.Common;
using Eyedia.Core;


namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Local file watcher
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class LocalFileSystemWatcher : Watchers
    {
        #region Properties

        string CurrentFile;
        static Queue<SreLocalFileInfo> LastFewFiles = new Queue<SreLocalFileInfo>();
        const int MaxLastFewFiles = 3;
        private static readonly object _lock = new object();
        public string ArchiveLocation { get; set; }
        
        public override bool IsRunning
        {
            get
            {
                return Registry.Instance.LocalFileWatcher.EnableRaisingEvents;
            }

        }

        #endregion Properties

        /// <summary>
        /// Initializes a local file watcher with a archive path and standard (or global) watch path
        /// </summary>
        /// <param name="archivePath"></param>
        public LocalFileSystemWatcher(string archivePath)
        {
            this.ArchiveLocation = archivePath;
        }

        /// <summary>
        /// Initializes a local file watcher with a specific watch & archive path
        /// </summary>
        /// <param name="watchPath">The watch path</param>
        /// <param name="archivePath">The archive path</param>
        public LocalFileSystemWatcher(string watchPath, string archivePath)
        {
            this.ArchiveLocation = archivePath;
            Directory.CreateDirectory(watchPath);
            Registry.Instance.LocalFileWatcher = new FileSystemWatcher();
            Registry.Instance.LocalFileWatcher.Path = watchPath;

        }

        /// <summary>
        /// Starts the local file watcher
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run()
        {
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            Registry.Instance.LocalFileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            Registry.Instance.LocalFileWatcher.IncludeSubdirectories = true;
            FileSystemEventHandler handler = new FileSystemEventHandler(OnCreated);
            Registry.Instance.LocalFileWatcher.Created -= handler;
            Registry.Instance.LocalFileWatcher.Created += handler;

            Registry.Instance.LocalFileWatcher.EnableRaisingEvents = true;

        }

        /// <summary>
        /// Stops local file watcher events to be fired
        /// </summary>
        public void Stop()
        {
            Registry.Instance.LocalFileWatcher.EnableRaisingEvents = false;
            Registry.Instance.LocalFileWatcher.Created -= new FileSystemEventHandler(OnCreated);
        }

        void OnCreated(object source, FileSystemEventArgs e)
        {

            if (e.Name == "New Folder") return;
            Registry.Instance.LocalFileWatcher.EnableRaisingEvents = false;
            bool fileCopied = false;
            lock (_lock)
            {
                if (IsItLastFewRecentFile(e.FullPath))
                {
                    Registry.Instance.LocalFileWatcher.EnableRaisingEvents = true;
                    return;
                }

                DateTime fileReceived = DateTime.Now;
                CurrentFile = e.FullPath;
                while (true)
                {
                    if (!File.Exists(CurrentFile))
                        break;   //multi-instances - Must have been taken care by other instance

                    if (FileDownloadCompleted(CurrentFile))
                    {
                        fileCopied = true;
                        AddToLastFewRecentFiles(e.FullPath);
                        break;
                    }
                    else
                    {
                        // Calculate the elapsed time and stop if the maximum retry        
                        // period has been reached.        
                        TimeSpan timeElapsed = DateTime.Now - fileReceived;
                        if (timeElapsed.TotalMinutes > IdpeConfigurationSection.CurrentConfig.LocalFileWatcher.RetryTimeOut)
                        {
                            Registry.Instance.LocalFileWatcher.EnableRaisingEvents = true;
                            ExtensionMethods.TraceError("The file \"{0}\" could not be processed. Time elapsed = '{1}', LocalFileWatcherMaximumRetryPeriod = '{2}'", 
                                CurrentFile, timeElapsed.TotalMinutes, IdpeConfigurationSection.CurrentConfig.LocalFileWatcher.RetryTimeOut);
                            break;
                        }
                        Thread.Sleep(300);
                        if (string.IsNullOrEmpty(CurrentFile))
                            break;
                    }
                    Trace.Flush();
                }
            }

            if (fileCopied)
            {
                ExtensionMethods.TraceInformation("LFSW1 - Sending '{0}' to process.", e.Name);
                this.Process(CurrentFile, e.Name, 0);
                CurrentFile = string.Empty;

            }
            HandleExistingFiles();  //this is important (when more than 1 file is dropped)            
            Trace.Flush();
            Registry.Instance.LocalFileWatcher.EnableRaisingEvents = true;  //moved here-22OCT14
        }


        /// <summary>
        /// Processes a file by (1) moving file to archive folder, and then (2) invoking FileDownloaded event
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="fileName"></param>
        /// <param name="dataSourceId"></param>
        /// <param name="handleArchive"></param>
        public void Process(string fileFullName, string fileName, int dataSourceId, bool handleArchive = true)
        {
            if (fileName.Contains("\\"))
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);

            if (dataSourceId == 0)
            {
                int tempInt = 0;
                int.TryParse(Directory.GetParent(fileFullName).Name, out tempInt);
                if (tempInt == 0)
                    throw new Exception(string.Format("File '{0}' dropped on wrong location!", fileFullName));
                dataSourceId = tempInt;
            }

            lock (_lock)
            {
                string moveTo = fileFullName;
                string renamedToIdentifier = Guid.NewGuid().ToString();
                if (handleArchive)
                {
                    string archiveLoc = ArchiveLocation + "\\" + dataSourceId + "\\" + DateTime.Now.ToString("yyyyMMdd");

                    //move file
                    moveTo = Path.Combine(archiveLoc, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(moveTo));

                    if (File.Exists(moveTo))
                    {
                        string moveToBUName = Path.Combine(archiveLoc, string.Format("{0}_{1}", renamedToIdentifier, fileName));
                        FileCopy(moveTo, moveToBUName, true); //backup existing
                    }

                    FileCopy(fileFullName, moveTo, true);   //move file
                }
               
                if (this.DataSourceParameters == null)
                    this.DataSourceParameters = new Dictionary<string, object>();
                DataSourceParameters.Clear();
                DataSourceParameters.Add("DataSourceId", dataSourceId);
                FileSystemWatcherEventArgs e = new FileSystemWatcherEventArgs(DataSourceParameters, moveTo, renamedToIdentifier);
                try
                {
                    InvokeFileDownloaded(e);
                }
                catch (BusinessException ex)
                {
                    ExtensionMethods.TraceError(ex.ToString());
                    Trace.Flush();
                }
                catch (Exception ex)
                {
                    ExtensionMethods.TraceError("An unknown error occurred!. {0}. {1} This error needs immediate attention", ex.ToString(), Environment.NewLine + Environment.NewLine);
                    Trace.Flush();
                }

            }

        }

        /// <summary>
        /// Dispose this object (this will never be called as long as IDPE instance is active)
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        #region Helpers

        internal void HandleExistingFiles()
        {
            List<string> files = DirSearch(Registry.Instance.LocalFileWatcher.Path);
            foreach (string file in files)
            {
                lock (_lock)
                {
                    if (File.Exists(file))
                    {
                        AddToLastFewRecentFiles(file);
                        ExtensionMethods.TraceInformation("LFSW2 - Sending '{0}' to process.", file);
                        Trace.TraceInformation("3 - " + file);
                        Trace.Flush();
                        this.Process(file, Path.GetFileName(file), 0);
                    }
                }
              
            }
        }

        void AddToLastFewRecentFiles(string fullFileName)
        {
            if (LastFewFiles.Count == MaxLastFewFiles)
                LastFewFiles.Dequeue();

            LastFewFiles.Enqueue(new SreLocalFileInfo(fullFileName, DateTime.Now));
        }

        bool IsItLastFewRecentFile(string fullFileName)
        {
            foreach (SreLocalFileInfo fileInfo in LastFewFiles)
            {
                if (fileInfo.FileName == fullFileName)
                {
                    TimeSpan ts = DateTime.Now - fileInfo.ProcessedAt;

                    if (ts.TotalMilliseconds < 500)
                        return true;
                }
            }
            return false;
        }

        List<string> DirSearch(string sDir)
        {
            List<string> existingFiles = new List<string>();
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        existingFiles.Add(f);
                    }
                    DirSearch(d);
                }
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
            }

            return existingFiles;
        }

        static bool FileDownloadCompleted(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                    return false;   //multi-instances - Must have been taken care by other instance

                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.Flush();
                return false;
            }
        }

        public void FileCopy(string fromFileName, string toFileName, bool move)
        {
            try
            {                
                DateTime fileReceived = DateTime.Now;
                if (File.Exists(fromFileName))
                {
                    File.Copy(fromFileName, toFileName);
                    while (true)
                    {
                        if ((CurrentFile != null) && (!File.Exists(CurrentFile)))
                        {
                            if(File.Exists(fromFileName))
                                new FileUtility().Delete(fromFileName);
                            break;   //multi-instances - Must have been taken care by other instance
                        }

                        if (FileDownloadCompleted(toFileName))
                        {
                            if (move)
                                new FileUtility().Delete(fromFileName);
                            return;
                        }
                        // Calculate the elapsed time and stop if the maximum retry        
                        // period has been reached.        
                        TimeSpan timeElapsed = DateTime.Now - fileReceived;
                        if (timeElapsed.TotalMinutes > IdpeConfigurationSection.CurrentConfig.LocalFileWatcher.RetryTimeOut)
                        {
                            ExtensionMethods.TraceError("The file \"{0}\" could not be copied.", fromFileName);
                            return;
                        }
                        Thread.Sleep(300);
                    }
                }    
            }
            catch (IOException ioe)
            {
                //we can eat this exception, it was observed that mostly in multi instance scenario, access violation occurrs as 
                // the file is already taken/processed by an instance.
                Trace.TraceError(ioe.Message);
                Trace.Flush();
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
                Trace.Flush();
            }

        }

        #endregion Helpers

    }

    #region Struct SreLocalFileInfo

    public struct SreLocalFileInfo
    {
        public string FileName { get; private set; }
        public DateTime ProcessedAt { get; private set; }

        public SreLocalFileInfo(string fileName, DateTime processedAt)
            : this()
        {
            FileName = fileName;
            ProcessedAt = processedAt;
        }

    }

    #endregion Struct SreLocalFileInfo
}





