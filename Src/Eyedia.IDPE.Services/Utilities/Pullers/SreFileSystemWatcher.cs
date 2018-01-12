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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// SreFileSystemWatcher is used when datasource wants to watch a different folder for files instead of the SRE global path
    /// </summary>
    public class SreFileSystemWatcher : Watchers
    {
        /// <summary>
        /// Returns data source id for which the listener was set
        /// </summary>
        public int DataSourceId { get; private set; }

        /// <summary>
        /// Returns data source keys which was used to create the listener
        /// </summary>
        public List<SreKey> Keys { get; private set; }


        public string LocalFileSystemFolderPullFolder { get; private set; }
        public string LocalFileSystemFolderArchive { get; private set; }

        /// <summary>
        /// Initializes SreFileSystemWatcher with a file watcher to keep an eye on specific pull folder(taken from keys)
        /// </summary>
        /// <param name="dataSourceId">DataSource Id</param>
        /// <param name="keys">The keys of data source</param>
        public SreFileSystemWatcher(int dataSourceId, List<SreKey> keys)
        {
            this.DataSourceId = dataSourceId;
            this.Keys = keys;
            this.LocalFileSystemFolderPullFolder = DataSource.GetPullFolder(dataSourceId, keys);
            this.LocalFileSystemFolderArchive = DataSource.GetArchiveFolder(dataSourceId, keys);
        }

        /// <summary>
        /// Starts watching the configured pull folder
        /// </summary>
        public void StartWatching()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            if (!Directory.Exists(LocalFileSystemFolderPullFolder))
                Directory.CreateDirectory(LocalFileSystemFolderPullFolder);

            watcher.Path = LocalFileSystemFolderPullFolder;
            
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                        
            FileSystemEventHandler handler = new FileSystemEventHandler(OnCreated);
            watcher.Created -= handler;
            watcher.Created += handler;
            watcher.EnableRaisingEvents = true;
            HandleExistingFiles();
        }

        internal void HandleExistingFiles()
        {
            
            foreach (string file in Directory.GetFiles(LocalFileSystemFolderPullFolder))
            {
                lock (_lock)
                {
                    AddToLastFewRecentFiles(file);
                }
                this.Process(file);
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
                    TimeSpan ts = new FileInfo(fullFileName).LastAccessTime - fileInfo.ProcessedAt;
                    if (ts.TotalMilliseconds < 500)
                        return true;
                }
            }
            return false;
        }

        string CurrentFile;
        static Queue<SreLocalFileInfo> LastFewFiles = new Queue<SreLocalFileInfo>();
        const int MaxLastFewFiles = 3;

        private static readonly object _lock = new object();
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            if (e.Name == "New Folder") return;
            Registry.Instance.LocalFileWatcher.EnableRaisingEvents = false;
            bool fileCopied = false;
            lock (_lock)
            {
                if (IsItLastFewRecentFile(e.FullPath))
                    return;

                DateTime fileReceived = DateTime.Now;
                CurrentFile = e.FullPath;
                while (true)
                {
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
                        if (timeElapsed.TotalMinutes > SreConfigurationSection.CurrentConfig.LocalFileWatcher.RetryTimeOut)
                        {
                            Registry.Instance.LocalFileWatcher.EnableRaisingEvents = true;
                            ExtensionMethods.TraceError("The file \"{0}\" could not be processed. Time elapsed = '{1}', LocalFileWatcherMaximumRetryPeriod = '{2}'",
                                CurrentFile, timeElapsed.TotalMinutes, SreConfigurationSection.CurrentConfig.LocalFileWatcher.RetryTimeOut);
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
                Registry.Instance.LocalFileWatcher.EnableRaisingEvents = true;
                this.Process(CurrentFile);
                lock (_lock)
                {
                    CurrentFile = string.Empty;
                }
            }

            HandleExistingFiles();  //this is important (when more than 1 file is dropped)
            Trace.Flush();
        }


        public void Process(string fileFullName)
        {
            string fileName = Path.GetFileName(fileFullName);

            //move file
            string moveTo = Path.Combine(LocalFileSystemFolderArchive, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(moveTo));
            string renamedToIdentifier = Guid.NewGuid().ToString();
            if (File.Exists(moveTo))
            {
                string moveToBUName = Path.Combine(LocalFileSystemFolderArchive, string.Format("{0}_{1}", renamedToIdentifier, fileName));
                new FileUtility().FileCopy(moveTo, moveToBUName, true); //backup existing
            }
            new FileUtility().FileCopy(fileFullName, moveTo, true);   //move file


            if (this.DataSourceParameters == null)
                this.DataSourceParameters = new Dictionary<string, object>();
            DataSourceParameters.Clear();
            DataSourceParameters.Add("DataSourceId", this.DataSourceId);
            FileSystemWatcherEventArgs e = new FileSystemWatcherEventArgs(DataSourceParameters, moveTo, renamedToIdentifier);
            try
            {
                //if (File.Exists(moveTo))
                //{
                //    using (StreamReader sr = new StreamReader(moveTo))
                //    {
                //        e.FileContent = sr.ReadToEnd();
                //        sr.Close();
                //    }
                //    InvokeFileDownloaded(e);
                //}
                InvokeFileDownloaded(e);
            }
            catch (BusinessException ex)
            {
                ExtensionMethods.TraceError(ex.ToString());
                Trace.Flush();
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError("An unknown error occurred!. {0}. {1} This error needs immediate attention", 
                    ex.ToString(), Environment.NewLine + Environment.NewLine);
                Trace.Flush();
            }

        }
        bool FileDownloadCompleted(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}



