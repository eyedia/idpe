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
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.Core.Windows;
using Eyedia.IDPE.Common;
using System.IO;
using Eyedia.IDPE.DataManager;
using System.Threading;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Handles zip/rar file. Internal method 'Handle(int,string,string)' will get called once a file is found. 
    /// Data source owners to implement Handle(int,string)
    /// </summary>
    public class ZipFileWatcher
    {
        #region Constructors
        public ZipFileWatcher() { }

        /// <summary>
        /// This constructor get called internally by SRE
        /// </summary>
        public ZipFileWatcher(string zipFileName, int dataSourceId, string zipInterfaceName, string processingBy, string outputFolder, string renamedToIdentifier)
        {
            this.ZipUniqueId = ShortGuid.NewGuid().Value;
            this.ZipFileName = zipFileName;
            this.DataSourceId = dataSourceId;
            this.ProcessingBy = processingBy;            
            this.OutputFolder = outputFolder;
            this.RenamedToIdentifier = renamedToIdentifier;
            Registry.Instance.ZipFiles.Add(this.ZipUniqueId, new ZipFileInformation(this.ZipUniqueId, this.ZipFileName));

            if ((!string.IsNullOrEmpty(zipInterfaceName))
               && (Type.GetType(zipInterfaceName) != null))
            {
                object objZipFileWatcher = Activator.CreateInstance(Type.GetType(zipInterfaceName));
                DataSourceSpecificZipFileWatcher = (ZipFileWatcher)objZipFileWatcher;
                DataSourceSpecificZipFileWatcher.DataSourceId = this.DataSourceId;
                DataSourceSpecificZipFileWatcher.ProcessingBy = this.ProcessingBy;
                DataSourceSpecificZipFileWatcher.OutputFolder = this.OutputFolder;
                DataSourceSpecificZipFileWatcher.RenamedToIdentifier = this.RenamedToIdentifier;
            }

        }

        #endregion Constructors

        #region Properties

        public ZipFileWatcher DataSourceSpecificZipFileWatcher { get; internal set; }

        public string PullFolder { get; set; }

        public string OutputFolder { get; set; }

        public string RenamedToIdentifier { get; set; }

        /// <summary>
        /// The data source, under which the file has been received
        /// </summary>
        public int DataSourceId { get; set; }

        /// <summary>
        /// Processing by
        /// </summary>
        public string ProcessingBy { get; set; }

        /// <summary>
        /// The name of the zip file provided through the constructor
        /// </summary>
        public string ZipFileName { get; internal set; }

        /// <summary>
        /// List of all unzipped files found within the zip file
        /// </summary>
        public string[] UnZippedFileNames { get; protected set; }

        public string ZipUniqueId { get; private set; }

        List<SreKey> _datasourceKeys;
        public List<SreKey> DataSourceKeys
        {
            get
            {
                if (_datasourceKeys == null)
                {
                    object obj = Cache.Instance.Bag[this.DataSourceId + ".keys"];
                    if (obj != null)
                        _datasourceKeys = obj as List<SreKey>;
                    else
                        _datasourceKeys = new Manager().GetApplicationKeys(this.DataSourceId, true);
                }

                return _datasourceKeys;
            }

        }

        #endregion Properties

        internal string Handle()
        {
            ExtensionMethods.TraceInformation("Handling a zip file '{0}', processing by '{1}' of data source '{2}'.", ZipFileName, ProcessingBy, DataSourceId);            
            
            string unzipLocation = Path.Combine(EyediaCoreConfigurationSection.CurrentConfig.TempDirectory, Constants.SREBaseFolderName);
            unzipLocation = Path.Combine(unzipLocation, "TempZip");
            unzipLocation = Path.Combine(unzipLocation, ZipUniqueId);
            UnZippedFileNames = ZipFileHandler.UnZip(ZipFileName, unzipLocation).ToArray();
            Array.Sort(UnZippedFileNames, (f1, f2) => Path.GetExtension(f1).CompareTo(Path.GetExtension(f2)));  //todo
            
            if (UnZippedFileNames.Length == 0)
            {
                //2nd attempt. Reason - Sometimes unzip returns 0 files.
                Thread.Sleep(500);
                UnZippedFileNames = ZipFileHandler.UnZip(ZipFileName, unzipLocation).ToArray();
            }
            string validationErrorMessage = string.Format("The zip file '{0}' processing has beeen aborted as validation process failed", ZipFileName);
            if (DataSourceSpecificZipFileWatcher != null)
            {
                DataSourceSpecificZipFileWatcher.UnZippedFileNames = UnZippedFileNames;
                DataSourceSpecificZipFileWatcher.ZipUniqueId = ZipUniqueId;
                DataSourceSpecificZipFileWatcher.ZipFileName = ZipFileName;
                if (!DataSourceSpecificZipFileWatcher.Prepare(ref validationErrorMessage))
                    throw new BusinessException(validationErrorMessage);
            }
            else
            {
                if (!Prepare(ref validationErrorMessage))
                    throw new BusinessException(validationErrorMessage);
            }

            UnZippedFileNames = AddUniqueIdToFiles(UnZippedFileNames, ZipUniqueId);
            SetZipFileInfo(this.ZipUniqueId, UnZippedFileNames.Length);
            
            ExtensionMethods.TraceInformation("There are {0} files.", UnZippedFileNames.Length);
            foreach (string unZippedFileName in UnZippedFileNames)
            {
                string onlyFileName = Path.GetFileName(unZippedFileName);
                if (DataSourceSpecificZipFileWatcher != null)
                {                    
                    ExtensionMethods.TraceInformation("'{0}' will be processed with specific handler '{1}'", onlyFileName, DataSourceSpecificZipFileWatcher.ToString());
                    DataSourceSpecificZipFileWatcher.Handle(unZippedFileName, onlyFileName, Path.GetExtension(unZippedFileName), FileStatus.Process);
                }
                else
                {
                    ExtensionMethods.TraceInformation("'{0}' will be processed with default handler", onlyFileName);
                    string fileExt = Path.GetExtension(unZippedFileName).ToLower();
                    //if (fileExt == ".txt")
                    //    Debugger.Break();
                    FileStatus fileProposedStatus = FileStatus.Process;
                    if (IsInIgnoreList(fileExt))
                        fileProposedStatus = FileStatus.Ignore;
                    if (IsInIgnoreListButCopy(fileExt))
                        fileProposedStatus = FileStatus.IgnoreMoveToOutput;
                    Handle(unZippedFileName, Path.GetFileName(unZippedFileName), fileExt, fileProposedStatus);
                }
                ExtensionMethods.TraceInformation("'{0}' handled.",onlyFileName);
            }            
            ExtensionMethods.TraceInformation("All files handled.");
            Trace.Flush();
            return ZipUniqueId;
        }

        private bool IsInIgnoreList(string fileExtension)
        {
            List<string> filters = new List<string>();
            string appWatchFilter = this.DataSourceKeys.GetKeyValue(SreKeyTypes.ZipIgnoreFileList);
            if (appWatchFilter.Contains("|"))
                filters.AddRange(appWatchFilter.ToLower().Split("|".ToCharArray()));
            else
                filters.Add(appWatchFilter.ToLower());

            var filterOrNot = (from f in filters
                               where f == fileExtension.ToLower()
                               select f).SingleOrDefault();

            return filterOrNot == null ? false : true;
            
        }

        private bool IsInIgnoreListButCopy(string fileExtension)
        {
            List<string> filters = new List<string>();
            string appWatchFilter = this.DataSourceKeys.GetKeyValue(SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder);
            if (appWatchFilter.Contains("|"))
                filters.AddRange(appWatchFilter.ToLower().Split("|".ToCharArray()));
            else
                filters.Add(appWatchFilter.ToLower());

            var filterOrNot = (from f in filters
                               where f == fileExtension.ToLower()
                               select f).SingleOrDefault();

            return filterOrNot == null ? false : true;

        }

        /// <summary>
        /// This method gets called after unzipping files.
        /// Can be useful to validate the zip file, (sort)group unzipped files for sequential process
        /// </summary>
        /// <param name="errorMessage">override default error message</param>
        /// <returns>'false' to abort processing file</returns>
        public virtual bool Prepare(ref string errorMessage)
        {
            return true;
        }

        /// <summary>
        /// This method will get called for each file found within the zip file.
        /// </summary>        
        /// <param name="unzippedFileName">Un zipped full file name</param>
        /// <param name="onlyFileName">Un zipped file name</param>
        /// <param name="extension">file extension</param>
        /// <param name="fileStatus">file status</param>
        public virtual void Handle(string unzippedFileName, string onlyFileName, string extension, FileStatus fileStatus)
        {           
            string destFileName = string.Empty;
            string zipId = string.Empty;

            unzippedFileName = ExtractActualFileName(unzippedFileName);
            zipId = ExtractUniqueId(onlyFileName);

            ExtensionMethods.TraceInformation("Actual name = '{0}', zip id = '{1}', extension = '{2}', and status = '{3}'",
                Path.GetFileName(unzippedFileName), zipId, extension, fileStatus);

          
            switch (fileStatus)
            {
                case FileStatus.Process:

                    destFileName = Path.Combine(DataSource.GetArchiveFolder(DataSourceId, DataSourceKeys), onlyFileName);                    
                    Registry.Instance.Pullers._LocalFileSystemWatcher.Process(unzippedFileName, onlyFileName, this.DataSourceId);                 
                    break;

                case FileStatus.IgnoreMoveToOutput:
                   
                    AddIgnoredFileCountToZipFileInfo(zipId);
                    //moving to output
                    string myOutputFolder = Path.Combine(DataSource.GetOutputFolder(DataSourceId, DataSourceKeys), zipId);
                    if (!Directory.Exists(myOutputFolder))
                        Directory.CreateDirectory(myOutputFolder);

                    destFileName = string.Format("{0}\\{1}", myOutputFolder, onlyFileName);
                    destFileName = ExtractActualFileName(destFileName);

                    if (File.Exists(destFileName))
                    {
                        string buName = Path.Combine(OutputFolder, string.Format("{0}_{1}", RenamedToIdentifier, onlyFileName));
                        new FileUtility().FileCopy(destFileName, buName, true); //backup existing                        
                    }

                    new FileUtility().FileCopy(unzippedFileName, destFileName, false);  //ignored and copied to output folder
                    List<SreKey> appKeys = _datasourceKeys;
                    if (appKeys == null)
                        appKeys = Cache.Instance.Bag[DataSourceId + ".keys"] as List<SreKey>;
                    Registry.Instance.Pullers.InvokeFileProcessed(this.DataSourceId, string.Empty, appKeys, 
                        destFileName, DataSource.GetOutputFolder(DataSourceId, DataSourceKeys), this.ZipUniqueId);
                  
                    break;

                case FileStatus.Ignore:
                 
                    AddIgnoredFileCountToZipFileInfo(zipId);                  
                    break;

                default:
                    throw new Exception("'fileStatus' can be set either to 'Ignore' or 'IgnoreMoveToOutput'!");
            }            
        }

        #region Helpers
        string[] AddUniqueIdToFiles(string[] files, string uniqueId)
        {
            List<string> result = new List<string>();

            foreach (string file in files)
            {
                string onlyFileName = Path.GetFileNameWithoutExtension(file);
                string newFileName = string.Format("{0}°{1}°{2}", Constants.UnzippedFilePrefix, uniqueId, onlyFileName);
                result.Add(file.Replace(onlyFileName, newFileName));
            }
            return result.ToArray();
        }


        internal static string ExtractUniqueId(string onlyFileName)
        {
            string[] fileInfo = onlyFileName.Split("°".ToCharArray());
            return fileInfo[1];
        }

        internal static string ExtractActualFileName(string unzippedFileName)
        {
            string onlyFileName = Path.GetFileName(unzippedFileName);
            if (onlyFileName.StartsWith(Constants.UnzippedFilePrefix))
            {
                string fileDir = Path.GetDirectoryName(unzippedFileName);
                string[] fileInfo = onlyFileName.Split("°".ToCharArray());
                string actualFileName = string.Empty;
                for (int i = 2; i < fileInfo.Length; i++)
                {
                    actualFileName += fileInfo[i];
                }
                unzippedFileName = Path.Combine(fileDir, actualFileName);
            }
            return unzippedFileName;
        }

        internal static string RenameToOriginal(string unzippedFileName)
        {
            //string theUnzippedFileName = unzippedFileName;
            string actualFileName = ExtractActualFileName(unzippedFileName);
            if (File.Exists(actualFileName))
            {
                string moveToBUName = Path.Combine(Path.GetDirectoryName(actualFileName),
                    string.Format("{0}_{1}", Guid.NewGuid().ToString(), Path.GetFileName(actualFileName)));

                new FileUtility().FileCopy(actualFileName, moveToBUName, true);
            }
            new FileUtility().FileCopy(unzippedFileName, actualFileName, true);
            return actualFileName;
        }

        static void SetZipFileInfo(string zipUniqueId, int totalFiles)
        {
            if (!Registry.Instance.ZipFiles.ContainsKey(zipUniqueId))
                throw new Exception(string.Format("Registry does not contain zip file information of {0}", zipUniqueId));

            ZipFileInformation zipInfo = Registry.Instance.ZipFiles[zipUniqueId];
            zipInfo.TotalFiles = totalFiles;
            zipInfo.TotalToBeProcessedFiles = totalFiles;

            //Registry.Instance.ZipFiles[zipUniqueId].TotalFiles = totalFiles; ;
        }

        static void AddIgnoredFileCountToZipFileInfo(string zipUniqueId)
        {
            if (!Registry.Instance.ZipFiles.ContainsKey(zipUniqueId))
                throw new Exception(string.Format("Registry does not contain zip file information of {0}", zipUniqueId));

            ZipFileInformation zipInfo = Registry.Instance.ZipFiles[zipUniqueId];
            zipInfo.TotalToBeProcessedFiles = zipInfo.TotalToBeProcessedFiles - 1;

            //Registry.Instance.ZipFiles[zipUniqueId].TotalFiles = totalFiles;
        }

        #endregion Helpers

    }

    public class ZipFileInformation
    {
        public ZipFileInformation(string zipUniqueId, string zipFileName)
        {
            this.ZipUniqueId = zipUniqueId;
            this.ZipFileName = zipFileName;            
        }
        public string ZipFileName { get; private set; }
        public string ZipUniqueId {get; private set;}
        public int TotalFiles { get; internal set; }
        public int TotalToBeProcessedFiles { get; internal set; }
        public int TotalProcessedFiles { get; internal set; }        
    }
    
}





