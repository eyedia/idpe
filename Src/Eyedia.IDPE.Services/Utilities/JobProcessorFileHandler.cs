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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    public class JobProcessorFileHandler
    {
        public JobProcessorFileHandler(FileSystemWatcherEventArgs e)
        {
            InputFileName = e.FileName;
            InputFileExtension = e.FileExtension;
            RenamedToIdentifier = e.RenamedToIdentifier;
            int dataSourceId = 0;
            int.TryParse(e.ApplicationParameters["DataSourceId"].ToString(), out dataSourceId);
            if (dataSourceId == 0)
                throw new Exception("The data source id was not found!");
            DataSourceId = dataSourceId;
            ProcessingBy = "Deb";
        }
        public string InputFileName { get; private set; }
        public string InputFileNameOnly { get; private set; }
        public string InputFileExtension { get; private set; }
        public string RenamedToIdentifier { get; private set; }
        public int DataSourceId { get; private set; }
        public string OutputFolder { get; private set; }
        public string ActualOutputFolder { get; private set; }
        public string OutputFileName { get; private set; }
        public string WithError { get; private set; }
        public string WithWarning { get; private set; }
        public bool IsRequestFromWCF { get; private set; }
        public string JobId { get; private set; }
        public string ZipUniuqeId {get; private set; }
        public List<SreKey> Keys { get; private set; }
        public string ProcessingBy { get; private set; }
        public string ZipInterfaceName { get; private set; }
        public bool PrepareInput()
        {

            //int dataSourceId = 0;
            //int.TryParse(e.ApplicationParameters["DataSourceId"].ToString(), out dataSourceId);
            //if (dataSourceId == 0)
            //    return;

                       
            InputFileNameOnly = Path.GetFileNameWithoutExtension(InputFileName);
            //string InputFileExtension = Path.GetExtension(InputFileName);


            Keys = Cache.Instance.Bag[DataSourceId + ".keys"] as List<SreKey>;
            if (Keys == null)            
                Keys = DataSource.LoadKeys(DataSourceId);
            

            OutputFolder = DataSource.GetOutputFolder(DataSourceId, Keys);
            ActualOutputFolder = OutputFolder;
            OutputFileName = DataSource.GetOutputFileName(DataSourceId, Keys, OutputFolder, InputFileNameOnly);

           

            string appWatchFilter = Keys.GetKeyValue(SreKeyTypes.WatchFilter);
            ZipInterfaceName = Keys.GetKeyValue(SreKeyTypes.ZipInterfaceName);

            if ((InputFileExtension.ToLower() == ".zip") || (InputFileExtension.ToLower() == ".rar") || (InputFileExtension.ToLower() == ".tar"))
            {
                OutputFolder = Path.Combine(EyediaCoreConfigurationSection.CurrentConfig.TempDirectory, Constants.SREBaseFolderName);
                OutputFolder = Path.Combine(OutputFolder, "RedirectedOutput");
                OutputFolder = Path.Combine(OutputFolder, DateTime.Now.ToDBDateFormat());
                OutputFolder = Path.Combine(OutputFolder, DataSourceId.ToString());
            }

            if ((!string.IsNullOrEmpty(appWatchFilter))
                && (appWatchFilter != Pullers.FileExtensionSupportAll))
            {
                List<string> filters = new List<string>();
                if (appWatchFilter.Contains("|"))
                    filters.AddRange(appWatchFilter.ToLower().Split("|".ToCharArray()));
                else
                    filters.Add(appWatchFilter.ToLower());

                var filterOrNot = (from f in filters
                                   where f == InputFileExtension.ToLower()
                                   select f).SingleOrDefault();
                if (filterOrNot == null)
                {
                    if (!InputFileNameOnly.StartsWith(Constants.UnzippedFilePrefix))
                    {
                        SreMessage warn = new SreMessage(SreMessageCodes.SRE_FILE_TYPE_NOT_SUPPORTED);
                        DataSource dataSource = new DataSource(DataSourceId, string.Empty);
                        WithWarning = string.Format(warn.Message, dataSource.Name, appWatchFilter, Path.GetFileName(InputFileName));
                        ExtensionMethods.TraceInformation(WithWarning);
                        new PostMan(dataSource).Send(PostMan.__warningStartTag + WithWarning + PostMan.__warningEndTag, "File Ignored");
                        return false;
                    }
                }
            }
            
            if (InputFileNameOnly.StartsWith(Constants.WCFFilePrefix))
            {
                IsRequestFromWCF = true;
                JobId = InputFileNameOnly.Replace(Constants.WCFFilePrefix, "");
                JobId = JobId.Replace(InputFileExtension, "");
            }
            else if (InputFileNameOnly.StartsWith(Constants.UnzippedFilePrefix))
            {
                ZipUniuqeId = ZipFileWatcher.ExtractUniqueId(InputFileNameOnly);
                OutputFolder = Path.Combine(OutputFolder, ZipUniuqeId);
                if (!Directory.Exists(OutputFolder))
                    Directory.CreateDirectory(OutputFolder);

                OutputFileName = Path.Combine(OutputFolder, InputFileNameOnly + Path.GetExtension(OutputFileName));
                OutputFileName = ZipFileWatcher.ExtractActualFileName(OutputFileName);
            }

            return true;
        }

        public void PrepareInputZip()
        {
            ZipUniuqeId = new ZipFileWatcher(InputFileName, DataSourceId,
                   ZipInterfaceName, ProcessingBy,
                   OutputFolder, RenamedToIdentifier).Handle();
            OutputFolder = Path.Combine(DataSource.GetOutputFolder(DataSourceId, Keys),
                ZipUniuqeId);

            if (!Directory.Exists(OutputFolder))
                Directory.CreateDirectory(OutputFolder);

            OutputFileName = Path.Combine(OutputFolder, InputFileNameOnly + InputFileExtension);            
        }

        public bool PrepareOutput(StringBuilder result)
        {
            if (File.Exists(OutputFileName))
            {
                string buName = Path.Combine(OutputFolder, string.Format("{0}_{1}", RenamedToIdentifier, Path.GetFileName(OutputFileName)));
                new FileUtility().FileCopy(OutputFileName, buName, true); //backup existing
            }

            if (((InputFileExtension.ToLower() == ".zip") || (InputFileExtension.ToLower() == ".rar") || (InputFileExtension.ToLower() == ".tar"))
                && (Keys.GetKeyValue(SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder).ParseBool()))
            {
                ExtensionMethods.TraceInformation("Pullers - The data source '{0}' has been configured as not to copy zip acknoledgement file. File will not be created!",
                    DataSourceId);
                return false;
            }

            if (result.Length > 0)
            {
                using (StreamWriter tw = new StreamWriter(OutputFileName))
                {
                    tw.Write(result);
                    tw.Close();
                }

                ExtensionMethods.TraceInformation("{0} successfully processed. Output file was {1}", InputFileName, OutputFileName);
                //InvokeFileProcessed(dataSourceId, jobProcessor.JobId, Keys, OutputFileName, actualOutputFolder, zipUniuqeId);
                return true;
            }
            else
            {
                ExtensionMethods.TraceInformation("Pullers - Failed to process '{0}', empty data came from output writer! Check log for more details.", InputFileName);
                return false;
            }
        }
       
    }
}


