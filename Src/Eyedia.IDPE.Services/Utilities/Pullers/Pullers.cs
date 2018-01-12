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
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using Eyedia.Core;
using System.Web.Hosting;
using System.Configuration;
using System.Threading;
using Eyedia.Core.Windows.Utilities;
using System.Text.RegularExpressions;

namespace Eyedia.IDPE.Services
{
    #region Pullers Class
    public class Pullers
    {
        public delegate void PullersEventHandler(PullersEventArgs e);        
        internal LocalFileSystemWatcher _LocalFileSystemWatcher;
        Manager _Manager;
        public const string FileExtensionSupportAll = "All(No Filter)";
        internal Dictionary<int, SqlWatcher> SqlWatchers;       
        System.Timers.Timer InitDoWorkTimer;

        private static readonly object _lock = new object();

        public Pullers()
        {            
            InitDoWorkTimer = new System.Timers.Timer(30 * 1000);
            InitDoWorkTimer.AutoReset = true;
            InitDoWorkTimer.Enabled = true;
            InitDoWorkTimer.Elapsed += new System.Timers.ElapsedEventHandler(InitDoWorkTimer_Interval);

            ExtensionMethods.TraceInformation("Pullers - Instantiated...");
        }

        #region Start

        public void Start()
        {            
            SqlWatchers = new Dictionary<int, SqlWatcher>();           
            try
            {
                if (!Directory.Exists(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull))
                    Directory.CreateDirectory(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull);
                WindowsUtility.SetFolderPermission(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull);
            }
            catch(Exception ex)
            {
                Trace.TraceInformation(ex.Message);
                Trace.Flush();
            }
          
            DateTime stTm = DateTime.Now;
            if (EyediaCoreConfigurationSection.CurrentConfig.TempDirectory.Length > Constants.MaxTempFolderPath)
                throw new ConfigurationErrorsException(string.Format("'{0}' can not have more than '{1}' length", EyediaCoreConfigurationSection.CurrentConfig.TempDirectory, Constants.MaxTempFolderPath));

            _Manager = new Manager();
            List<int> allDataSourceIds = _Manager.GetAllDataSourceIds(false);
          
            foreach (int dataSourceId in allDataSourceIds)
            {
                string dir = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, dataSourceId.ToString());
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
           
            string sysDir = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, "100");
            if (!Directory.Exists(sysDir))
                Directory.CreateDirectory(sysDir);
            
            StartLocalFileSystemWatcher();          
            List<IdpeDataSource> dataSources = _Manager.GetDataSources();
            foreach (IdpeDataSource dataSource in dataSources)
            {
                List<IdpeKey> keys = Cache.Instance.Bag[dataSource.Id + ".keys"] as List<IdpeKey>;
                if (keys == null)
                {
                    //keys = _Manager.GetApplicationKeys(dataSource.Id, true);
                    //Cache.Instance.Bag.Add(dataSource.Id + ".keys", keys);                   
                    keys = DataSource.LoadKeys(dataSource.Id);
                }

                if ((dataSource.DataFeederType != null)
                    && ((DataFeederTypes)dataSource.DataFeederType == DataFeederTypes.PullFtp))
                {
                    #region Initializing FtpPullers
                    string ftpRemoteLocation = FindSREKeyUsingType(keys, SreKeyTypes.FtpRemoteLocation);
                    string ftpUserName = FindSREKeyUsingType(keys, SreKeyTypes.FtpUserName);
                    string ftpPassword = FindSREKeyUsingType(keys, SreKeyTypes.FtpPassword);
                    string strinterval = FindSREKeyUsingType(keys, SreKeyTypes.FtpWatchInterval);
                    int ftpWatchIntervalInMinutes = 0;
                    int.TryParse(strinterval, out ftpWatchIntervalInMinutes);
                    if (ftpWatchIntervalInMinutes == 0)
                        ftpWatchIntervalInMinutes = 1;

                    string ftpLocalLocation = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, dataSource.Id.ToString());
                    string appOutputFolder = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryOutput, dataSource.Id.ToString(), DateTime.Now.ToString("yyyyMMdd"));


                    Dictionary<string, object> datasourceParameters = new Dictionary<string, object>();
                    lock (_lock)
                    {
                        datasourceParameters.Add("DataSourceId", dataSource.Id);
                        datasourceParameters.Add("OutputFolder", appOutputFolder);
                        datasourceParameters.Add("ProcessingBy", dataSource.ProcessingBy);
                    }

                    FtpFileSystemWatcher ftpWatcher =
                        new FtpFileSystemWatcher(datasourceParameters, ftpRemoteLocation, ftpLocalLocation,
                        ftpWatchIntervalInMinutes, ftpUserName, ftpPassword, false, true);

                    ExtensionMethods.TraceInformation("Pullers - Initialized 'PullFtp'; '{0}' [ftpRemoteLocation = {1}, ftpLocalLocation = {2}, ftpUserName = {3},ftpPassword = {4}, interval = {5}]",
                       dataSource.Name, ftpRemoteLocation, ftpLocalLocation, ftpUserName, ftpPassword, ftpWatchIntervalInMinutes);
                    Registry.Instance.FtpWatchers.Add(ftpWatcher);

                    #endregion Initializing FtpPullers
                }
                else if ((dataSource.DataFeederType != null)
                    && ((DataFeederTypes)dataSource.DataFeederType == DataFeederTypes.PullLocalFileSystem))
                {

                    if (IsLocalFileSystemFoldersOverriden(keys))
                    {
                        SreFileSystemWatcher sreFileSystemWatcher = new SreFileSystemWatcher(dataSource.Id, keys);
                        Watchers.FileSystemWatcherEventHandler handler = new Watchers.FileSystemWatcherEventHandler(FileDownloaded);
                        sreFileSystemWatcher.FileDownloaded -= handler;
                        sreFileSystemWatcher.FileDownloaded += handler;
                        sreFileSystemWatcher.StartWatching();
                        Registry.Instance.DataSourceFileWatcher.Add(dataSource.Id, sreFileSystemWatcher);
                    }

                    //we dont have to do anything if it is generic
                }
                else if ((dataSource.DataFeederType != null)
                    && ((DataFeederTypes)dataSource.DataFeederType == DataFeederTypes.PullSql))
                {
                    #region Initializing SQL Pullers

                    SqlWatcher sqlWatcher = new SqlWatcher(dataSource);
                    sqlWatcher.StartDownloading();
                    SqlWatchers.Add(dataSource.Id, sqlWatcher);

                    #endregion Initializing SQL Pullers
                }

                Trace.Flush();
            }

            ExtensionMethods.TraceInformation("Pullers - Initialization finished.{0}", DateTime.Now - stTm);
        }
        

        #endregion Start

        #region Stop
        public void Stop()
        {
            ExtensionMethods.TraceInformation("Pullers - Trying to stop all 'Pullers'");
            if(_LocalFileSystemWatcher != null)
                _LocalFileSystemWatcher.Stop();
            ExtensionMethods.TraceInformation("All 'Pullers' stopped.");
        }
        #endregion Stop

        internal void FileDownloaded(FileSystemWatcherEventArgs e)
        {
            #region Commented1
            /*
            int dataSourceId = 0;
            int.TryParse(e.ApplicationParameters["DataSourceId"].ToString(), out dataSourceId);
            if (dataSourceId == 0)
                return;

            string processingBy = "u175675";    //todo            
            string onlyFileName = Path.GetFileNameWithoutExtension(e.FileName);
            string fileExt = Path.GetExtension(e.FileName);

            if (_Manager == null)
                _Manager = new Manager();

            List<SreKey> keys = Cache.Instance.Bag[dataSourceId + ".keys"] as List<SreKey>;
            if (keys == null)
            {
                //keys = _Manager.GetApplicationKeys(dataSourceId, true);
                //Cache.Instance.Bag.Add(dataSourceId + ".keys", keys);               
                keys = DataSource.LoadKeys(dataSourceId);
            }

            string outputFolder = DataSource.GetOutputFolder(dataSourceId, keys);
            string actualOutputFolder = outputFolder;
            string outputFileName = DataSource.GetOutputFileName(dataSourceId, keys, outputFolder, onlyFileName);

            string withError = string.Empty;
            string withWarning = string.Empty;

            string appWatchFilter = keys.GetKeyValue(SreKeyTypes.WatchFilter);
            string zipInterfaceName = keys.GetKeyValue(SreKeyTypes.ZipInterfaceName);

            if ((fileExt.ToLower() == ".zip") || (fileExt.ToLower() == ".rar") || (fileExt.ToLower() == ".tar"))
            {
                outputFolder = Path.Combine(SymplusCoreConfigurationSection.CurrentConfig.TempDirectory, Constants.SREBaseFolderName);
                outputFolder = Path.Combine(outputFolder, "RedirectedOutput");
                outputFolder = Path.Combine(outputFolder, DateTime.Now.ToDBDateFormat());
                outputFolder = Path.Combine(outputFolder, dataSourceId.ToString());
            }            

            if ((!string.IsNullOrEmpty(appWatchFilter))
                && (appWatchFilter != FileExtensionSupportAll))
            {
                List<string> filters = new List<string>();
                if (appWatchFilter.Contains("|"))
                    filters.AddRange(appWatchFilter.ToLower().Split("|".ToCharArray()));
                else
                    filters.Add(appWatchFilter.ToLower());

                var filterOrNot = (from f in filters
                                   where f == e.FileExtension.ToLower()
                                   select f).SingleOrDefault();
                if (filterOrNot == null)
                {
                    if (!onlyFileName.StartsWith(Constants.UnzippedFilePrefix))
                    {
                        SreMessage warn = new SreMessage(SreMessageCodes.SRE_FILE_TYPE_NOT_SUPPORTED);
                        DataSource dataSource = new DataSource(dataSourceId, string.Empty);
                        withWarning = string.Format(warn.Message, dataSource.Name, appWatchFilter, Path.GetFileName(e.FileName));
                        ExtensionMethods.TraceInformation(withWarning);
                        new PostMan(dataSource).Send(PostMan.__warningStartTag + withWarning + PostMan.__warningEndTag, "File Ignored");
                        return;
                    }
                }
            }

            string zipUniuqeId = string.Empty;
            bool isRequestFromWCF = false;
            string jobId = string.Empty;
            if (onlyFileName.StartsWith(Constants.WCFFilePrefix))
            {
                isRequestFromWCF = true;
                jobId = onlyFileName.Replace(Constants.WCFFilePrefix, "");
                jobId = jobId.Replace(fileExt, "");
            }
            else if (onlyFileName.StartsWith(Constants.UnzippedFilePrefix))
            {
                zipUniuqeId = ZipFileWatcher.ExtractUniqueId(onlyFileName);
                outputFolder = Path.Combine(outputFolder, zipUniuqeId);
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                outputFileName = Path.Combine(outputFolder, onlyFileName + Path.GetExtension(outputFileName));
                outputFileName = ZipFileWatcher.ExtractActualFileName(outputFileName);
            }
            */

            #endregion Commented1

            JobProcessorFileHandler jobProcessorFile = new JobProcessorFileHandler(e);
            jobProcessorFile.PrepareInput();

            if (!jobProcessorFile.IsRequestFromWCF)
                ExtensionMethods.TraceInformation("Pullers - Got a new file for {0} - '{1}'", jobProcessorFile.DataSourceId, e.FileName);
            else
                ExtensionMethods.TraceInformation("Pullers - Got a new WCF request for {0}, JobId = '{1}'", jobProcessorFile.DataSourceId, jobProcessorFile.JobId);

            if (DataSourceIsDisabled(jobProcessorFile.DataSourceId, e))
                return;

            StringBuilder result = new StringBuilder();
            JobProcessor jobProcessor = new JobProcessor();
            if (jobProcessorFile.IsRequestFromWCF)
            {
                jobProcessor.IsWCFRequest = true;
                result = jobProcessor.ProcessJob(jobProcessorFile.DataSourceId, string.Empty, jobProcessorFile.ProcessingBy,
                    jobProcessorFile.InputFileNameOnly, jobProcessorFile.JobId, jobProcessorFile.WithError, jobProcessorFile.WithWarning);
            }
            else if ((jobProcessorFile.InputFileExtension.ToLower() == ".zip") || (jobProcessorFile.InputFileExtension.ToLower() == ".rar") 
                || (jobProcessorFile.InputFileExtension.ToLower() == ".tar"))
            {
                jobProcessorFile.PrepareInputZip();
                result.AppendLine(jobProcessorFile.ZipUniuqeId);
            }
            else if ((jobProcessorFile.InputFileExtension.ToLower() == ".xls") || (jobProcessorFile.InputFileExtension.ToLower() == ".xlsx"))
            {
                result = jobProcessor.ProcessSpreadSheet(jobProcessorFile.DataSourceId, string.Empty, jobProcessorFile.ProcessingBy, e.FileName);
            }
            else if (jobProcessorFile.InputFileExtension.ToLower() == ".edi")
            {
                new EDIX12FileWatcher(jobProcessorFile.DataSourceId, e.FileName).Process();
                ExtensionMethods.TraceInformation("Pullers - {0} successfully processed. Output file was {1}", 
                    e.FileName, jobProcessorFile.OutputFileName);
                if (File.Exists(jobProcessorFile.OutputFileName))
                    InvokeFileProcessed(jobProcessorFile.DataSourceId, jobProcessor.JobId, 
                        jobProcessorFile.Keys, jobProcessorFile.OutputFileName, 
                        jobProcessorFile.ActualOutputFolder, jobProcessorFile.ZipUniuqeId);

                jobProcessor.Dispose();
                return;
            }
            else
            {
                result = jobProcessor.ProcessJob(jobProcessorFile.DataSourceId, string.Empty,
                    jobProcessorFile.ProcessingBy, e.FileName, string.Empty, jobProcessorFile.WithError, jobProcessorFile.WithWarning);
            }
            #region Commented2
            /*
            if (File.Exists(jobProcessorFile.OutputFileName))
            {
                string buName = Path.Combine(jobProcessorFile.OutputFolder, string.Format("{0}_{1}", e.RenamedToIdentifier, Path.GetFileName(jobProcessorFile.OutputFileName)));
                FileCopy(jobProcessorFile.OutputFileName, buName, true); //backup existing
            }

            if (((jobProcessorFile.InputFileExtension.ToLower() == ".zip") 
                || (jobProcessorFile.InputFileExtension.ToLower() == ".rar") || (jobProcessorFile.InputFileExtension.ToLower() == ".tar"))
                && (jobProcessorFile.Keys.GetKeyValue(SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder).ParseBool()))
            {
                ExtensionMethods.TraceInformation("Pullers - The data source '{0}' has been configured as not to copy zip acknoledgement file. File will not be created!",
                    jobProcessorFile.DataSourceId);
                return;
            }

            if (result.Length > 0)
            {
                using (StreamWriter tw = new StreamWriter(jobProcessorFile.OutputFileName))
                {
                    tw.Write(result);
                    tw.Close();
                }

                ExtensionMethods.TraceInformation("{0} successfully processed. Output file was {1}", e.FileName, outputFileName);
                InvokeFileProcessed(dataSourceId, jobProcessor.JobId, keys, outputFileName, actualOutputFolder, zipUniuqeId);
            }
            else
            {
                ExtensionMethods.TraceInformation("Pullers - Failed to process '{0}', empty data came from output writer! Check log for more details.", e.FileName);
            }
            */
            #endregion Commented2

            if (jobProcessorFile.PrepareOutput(result))
                InvokeFileProcessed(jobProcessorFile.DataSourceId, jobProcessor.JobId, jobProcessorFile.Keys,
                    jobProcessorFile.OutputFileName, jobProcessorFile.ActualOutputFolder, jobProcessorFile.ZipUniuqeId);

            jobProcessor.Dispose();
        }

        private bool DataSourceIsDisabled(int dataSourceId, FileSystemWatcherEventArgs e)
        {
            DataSource dataSource = new DataSource(dataSourceId, string.Empty);
            if (dataSource.IsActive == false)
            {
                #region Getting Ignored Folder

                string ignoredFolder = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryIgnored, dataSourceId.ToString());
                ignoredFolder = Path.Combine(ignoredFolder, DateTime.Now.ToDBDateFormat());
                if (!Directory.Exists(ignoredFolder))
                    Directory.CreateDirectory(ignoredFolder);

                #endregion Getting Ignored Folder

                #region Getting Ignored File
                string ignoredFile = Path.Combine(ignoredFolder, Path.GetFileName(e.FileName));

                if (File.Exists(ignoredFile))
                {
                    string ignoredFileBUName = Path.Combine(ignoredFolder, string.Format("{0}_{1}", ShortGuid.NewGuid(), Path.GetFileName(e.FileName)));
                    FileCopy(ignoredFile, ignoredFileBUName, true); //backup existing
                }

                #endregion Getting Ignored File

                new FileUtility().FileCopy(e.FileName, ignoredFile, true);
                ExtensionMethods.TraceInformation("Pullers - A file from {0} was ignored as the datasource is disabled. The file can be found at {1}",
                    dataSource.Name, ignoredFile);

                #region Sending Email

                string emailBody = string.Format("A file from <b>{0}</b> was ignored as the datasource is disabled. The file can be found at <b>{1}</b>",
                    dataSource.Name, ignoredFile);
                emailBody = PostMan.__warningStartTag + emailBody + PostMan.__warningEndTag;
                new PostMan(dataSource, false).Send(emailBody, "File Ignored");

                #endregion Sending Email

                return true;
            }
            return false;
        }

        #region Helper Methods

        bool IsLocalFileSystemFoldersOverriden(List<IdpeKey> keys)
        {

            IdpeKey key = (from e in keys
                          where e.Type == (int)SreKeyTypes.LocalFileSystemFoldersOverriden
                          select e).SingleOrDefault();

            bool result = false;
            if (key != null)
                bool.TryParse(key.Value, out result);
            return result;

        }

        void StartLocalFileSystemWatcher()
        {
            ExtensionMethods.TraceInformation("Pullers - Initializing local file watcher...");

            _LocalFileSystemWatcher = new LocalFileSystemWatcher(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryArchive);
            Watchers.FileSystemWatcherEventHandler handler = new Watchers.FileSystemWatcherEventHandler(FileDownloaded);
            _LocalFileSystemWatcher.FileDownloaded -= handler;
            _LocalFileSystemWatcher.FileDownloaded += handler;
            _LocalFileSystemWatcher.Run();

            ExtensionMethods.TraceInformation("Pullers - Local file watcher initialized  with Pull = '{0}' and Archive = '{1}'.",
                SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryArchive);

        }

        void FileCopy(string fromFileName, string toFileName, bool move)
        {
            try
            {
                DateTime fileReceived = DateTime.Now;
                File.Copy(fromFileName, toFileName);
                while (true)
                {
                    if (FileDownloadCompleted(toFileName))
                    {
                        if (move)
                            File.Delete(fromFileName);
                        return;
                    }
                    // Calculate the elapsed time and stop if the maximum retry        
                    // period has been reached.        
                    TimeSpan timeElapsed = DateTime.Now - fileReceived;
                    if (timeElapsed.TotalMinutes > SreConfigurationSection.CurrentConfig.LocalFileWatcher.RetryTimeOut)
                    {
                        ExtensionMethods.TraceError("The file \"{0}\" could not be copied.", fromFileName);
                        return;
                    }
                    Thread.Sleep(300);
                }
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError("An unknown error occurred!. {0}. {1} This error needs immediate attention", ex.ToString(), Environment.NewLine + Environment.NewLine);
                Trace.Flush();
            }

        }
        static bool FileDownloadCompleted(string filename)
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

        
        string FindSREKeyUsingType(List<IdpeKey> keys, SreKeyTypes sreKeyType)
        {
            IdpeKey key =(from e in keys
                    where e.Type == (int)sreKeyType
                    select e).SingleOrDefault();

            return key != null ? key.Value : string.Empty;
        }

        string FindSREKeyUsingName(List<IdpeKey> keys, SreKeyTypes sreKeyType)
        {
            IdpeKey key = (from e in keys
                           where e.Name == sreKeyType.ToString()
                           select e).SingleOrDefault();

            return key != null ? key.Value : string.Empty;
        }

        private void InitDoWorkTimer_Interval(object source, System.Timers.ElapsedEventArgs e)
        {
            //this will be executed only once after starting the service
            //  - Reduce starting time 
            //  - Handle existing file after starting the service

            ExtensionMethods.TraceInformation("Pullers - Initial checks started.");
            InitDoWorkTimer.Enabled = false;
            Start();           
            _LocalFileSystemWatcher.HandleExistingFiles();           

            ExtensionMethods.TraceInformation("Pullers - Initial checks ended. System is Ready!");
            PrepareEnvironment();
            Trace.Flush();
            
        }

        internal void InvokeFileProcessed(int datasourceId, string jobId, List<IdpeKey> appKeys, string fileName, string outputFolder, string zipUniqueId)
        {
            Trace.TraceInformation("Invoke");
            Trace.Flush();
            Trace.TraceInformation("datasourceId:{0}, JobId:{1},appkeys={2},filename={3},outputfolder={4},zipuniqueid={5}",
                datasourceId, jobId, appKeys.Count, fileName, outputFolder, zipUniqueId);
            Trace.Flush();

            Job currentJob = null;
            if ((!(string.IsNullOrEmpty(jobId)))
               && (Registry.Instance.Entries.ContainsKey(jobId)))
                currentJob = Registry.Instance.Entries[jobId] as Job;

            #region Handling ZipFile

            ZipFileInformation zipInfo = null;
            if (!string.IsNullOrEmpty(zipUniqueId))
            {
                zipInfo = Registry.Instance.ZipFiles[zipUniqueId];
                if (zipInfo.TotalFiles == zipInfo.TotalProcessedFiles)
                    return;
                else
                    zipInfo.TotalProcessedFiles = zipInfo.TotalProcessedFiles + 1;
            }

            #endregion Handling ZipFile

            #region Handling Pusher
            
            if ((currentJob != null)                
                && (!string.IsNullOrEmpty(currentJob.DataSource.PusherTypeFullName)))
            {
                ExtensionMethods.TraceInformation("Pullers - Initializing '{0}' Pusher '{1}'.", currentJob.DataSource.Name, currentJob.DataSource.PusherTypeFullName);
                object objPusher = null;
                if(currentJob.DataSource.PusherType == PusherTypes.Ftp)
                    objPusher = new PusherFtp();
                else if (currentJob.DataSource.PusherType == PusherTypes.DosCommands)
                    objPusher = new PusherDosCommands();
                else if (currentJob.DataSource.PusherType == PusherTypes.SqlQuery)
                    objPusher = new PusherSqlQuery();
                else if(currentJob.DataSource.PusherType == PusherTypes.Custom)
                    objPusher = Activator.CreateInstance(Type.GetType(currentJob.DataSource.PusherTypeFullName));
                if (objPusher != null)
                {
                    if ((currentJob.Errors.Count == 0)
                        && (currentJob.DataSource.OutputWriter.IsErrored == false))
                    {
                        ((Pushers)objPusher).FileProcessed(new PullersEventArgs(datasourceId, jobId, appKeys, fileName, outputFolder, zipUniqueId, zipInfo));
                        ExtensionMethods.TraceInformation("Pullers - Pusher called!");
                    }
                    else if ((currentJob.Errors.Count > 0)
                        && (currentJob.DataSource.DataFeederType == DataFeederTypes.PullSql))
                    {
                        new SqlWatcherHelper(currentJob).ExecuteRecoveryScript();
                    }
                    else
                    {
                        if (currentJob.DataSource.AllowPartial())
                        {
                            ((Pushers)objPusher).FileProcessed(new PullersEventArgs(datasourceId, jobId, appKeys, fileName, outputFolder, zipUniqueId, zipInfo));
                            ExtensionMethods.TraceInformation("Pullers - Pusher called!");
                        }
                        else
                        {
                            string message = "Pullers - There were error(s) while processing, the pusher was not called. Please study the error(s) and do the needful.";
                            ExtensionMethods.TraceError(message);
                        }
                    
                    }
                    
                }

            }
            #endregion Handling Pusher

            #region Logging History

            if (currentJob != null)
            {                
                string subFileName = null;
                if (!string.IsNullOrEmpty(zipUniqueId))
                {
                    subFileName = Path.GetFileName(fileName);
                    //removing output extension
                    if (subFileName.Contains("."))
                        subFileName = subFileName.Substring(0, subFileName.LastIndexOf("."));
                }
                new Manager().SaveLog(currentJob.FileName, subFileName, datasourceId,
                    currentJob.TotalRowsToBeProcessed, currentJob.TotalValid, currentJob.StartedAt, DateTime.Now, SreEnvironmentDetails());
            }

            #endregion Logging History
           
            #region Sending Email

            if (currentJob != null)
            {
                //send email in positive scenario. If would have failed, an error email would have automatically sent.
                ExtensionMethods.TraceInformation("Pullers - A job processed, total rows to be processed = {0}, total valid rows = {1}", currentJob.TotalRowsToBeProcessed, currentJob.TotalValid);
                Trace.Flush();
                string strEmailAfterFileProcessed = currentJob.DataSource.Keys.GetKeyValue(SreKeyTypes.EmailAfterFileProcessed);
                string strEmailAfterFileProcessedAttachInputFile = currentJob.DataSource.Keys.GetKeyValue(SreKeyTypes.EmailAfterFileProcessedAttachInputFile);
                string strEmailAfterFileProcessedAttachOutputFile = currentJob.DataSource.Keys.GetKeyValue(SreKeyTypes.EmailAfterFileProcessedAttachOutputFile);

                if (strEmailAfterFileProcessed.ParseBool())
                {
                    string strEmailAfterFileProcessedAttachOtherFiles = currentJob.DataSource.Keys.GetKeyValue(SreKeyTypes.EmailAfterFileProcessedAttachOtherFiles);

                    string message = string.Format("Pullers - A file from '{0}' was just processed with {1} record(s)!",
                        currentJob.DataSource.Name, currentJob.TotalRowsProcessed);

                    if(string.IsNullOrEmpty(strEmailAfterFileProcessedAttachOtherFiles))
                    {
                        List<string> outFile = new List<string>();
                        if (strEmailAfterFileProcessedAttachOutputFile.ParseBool())
                        {
                            outFile.Add(fileName);
                            new PostMan(currentJob, false).Send(message, "File Processed", !strEmailAfterFileProcessedAttachInputFile.ParseBool(), outFile);
                        }
                        else
                        {
                            new PostMan(currentJob, false).Send(message, "File Processed", !strEmailAfterFileProcessedAttachInputFile.ParseBool(), outFile);
                        }
                    }
                    else
                    {
                        List<string> otherFiles = new List<string>(strEmailAfterFileProcessedAttachOtherFiles.Split(",".ToCharArray()));
                        new PostMan(currentJob, false).Send(message, "File Processed", !strEmailAfterFileProcessedAttachInputFile.ParseBool(), otherFiles);
                    }

                }
            }

            #endregion Sending Email         

            #region Handling Global Events

            if ((currentJob != null)  && (currentJob.ErroredByPusher == false))
            {
                PullersEventArgs e = new PullersEventArgs(datasourceId, jobId, appKeys, fileName, outputFolder, zipUniqueId, zipInfo);
                Registry.Instance.GlobalEventsOnCompletes.Complete(datasourceId, e);
            }

            #endregion Handling Global Events         

            if (currentJob != null)                
                currentJob.PerformanceCounter.PrintTrace(jobId);
        }

        private void PrepareEnvironment()
        {
            try
            {
                Registry.Instance.EnvironmentVariables = EnvironmentFiles.ReadEnvironmentConfigs();
                Trace.TraceInformation("Reading environment variables from {0}...{1} found.",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.EnvironmentVariablesFileName),
                    Registry.Instance.EnvironmentVariables.Count);

                int noOfInstances = 1;
                if (Registry.Instance.EnvironmentVariables.ContainsKey("instances"))
                    noOfInstances = int.Parse(Registry.Instance.EnvironmentVariables["instances"]);
                //EnvironmentFiles.GenerateStartStopCommands(false, noOfInstances);

                Trace.TraceInformation("Environment is registered for {0} instances", noOfInstances);
            }
            catch(Exception ex)
            {
                Trace.TraceError("Failed to intialize environment variables." + ex.ToString());
            }
        }

        private string SreEnvironmentDetails()
        {
            return string.Format("{0}:{1} Core(s),{2} PP(s),{3} LP(s), MT={4},AutoRPT={5},RPT={6},Trace={7},Performance={8}",
                Eyedia.Core.Net.Dns.GetLocalIpAddress(),
                Registry.Instance.NumberOfCores,
                Registry.Instance.PhysicalProcessors,
                Environment.ProcessorCount,
                EyediaCoreConfigurationSection.CurrentConfig.MaxThreads,
                EyediaCoreConfigurationSection.CurrentConfig.AutoRecordsPerThread ? "On" : "Off",
                EyediaCoreConfigurationSection.CurrentConfig.AutoRecordsPerThread ? Environment.ProcessorCount : EyediaCoreConfigurationSection.CurrentConfig.RecordsPerThread,
                Eyedia.IDPE.Common.Information.TraceFilter,
                SreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter ? "On" : "Off");
        }

        
        #endregion Helper Methods

        #region properties
        public bool LocalFileSystemWatcherIsRunning
        {
            get { return _LocalFileSystemWatcher == null ? false : _LocalFileSystemWatcher.IsRunning; }
        }
        #endregion properties

    }
    #endregion Pullers Class

    #region PullersEventArgs Class
    public class PullersEventArgs
    {
        public PullersEventArgs(int datasourceId, string jobId, List<IdpeKey> dataSourceKeys, string outputFileName, string outputFolder, string zipUniqueId, ZipFileInformation zipFileInformation)
        {
            this.DataSourceId = datasourceId;
            this.JobId = jobId;
            this.DataSourceKeys = dataSourceKeys;
            this.OutputFileName = outputFileName;
            this.OutputFolder = outputFolder;
            this.ZipUniqueId = zipUniqueId;
            this.ZipFileInformation = zipFileInformation;
            this.CustomData = new Dictionary<string, object>();
        }

        public Job Job
        {
            get
            {
                if (string.IsNullOrEmpty(JobId))
                    return null;
                else if (Registry.Instance.Entries.Contains(JobId))
                    return Registry.Instance.Entries[JobId] as Job;
                else
                    return null;
            }
        }
        public int DataSourceId { get; private set; }
        public string JobId { get; private set; }
        public List<IdpeKey> DataSourceKeys { get; private set; }
        public string OutputFileName { get; private set; }
        public string OutputFolder { get; private set; }
        public string ZipUniqueId { get; private set; }
        public ZipFileInformation ZipFileInformation { get; private set; }
        public Dictionary<string, object> CustomData { get; set; }
    }

    #endregion PullersEventArgs Class
}





