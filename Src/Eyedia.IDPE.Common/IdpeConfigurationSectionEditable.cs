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
using System.Configuration;
using System.IO;
using System.ComponentModel;
using Eyedia.Core;

namespace Eyedia.IDPE.Common
{
    public class IdpeConfigurationSectionEditable : EyediaCoreConfigurationSectionEditable
    {
        public IdpeConfigurationSectionEditable(Configuration configuration) : base(configuration)
        {
            IdpeConfigurationSection idpeSection = (IdpeConfigurationSection)configuration.GetSection("idpeConfigurationSection");

            PullersEnabled = idpeSection.PullersEnabled;            
            TimeOut = idpeSection.TimeOut;
            WorkerTimeOut = idpeSection.WorkerTimeOut;
            LocalFileWatcherEnabled = idpeSection.LocalFileWatcher.Enabled;
            BaseDirectory = idpeSection.LocalFileWatcher.BaseDirectory;
            RetryTimeOut = idpeSection.LocalFileWatcher.RetryTimeOut;
            MicrosoftExcelOLEDataReaderEnabled = idpeSection.MicrosoftExcelOLEDataReader.Enabled;
            MicrosoftExcelOLEProvider = idpeSection.MicrosoftExcelOLEDataReader.Provider;
            MicrosoftExcelOLEVersion = idpeSection.MicrosoftExcelOLEDataReader.Version;

            TrackingEnabled = idpeSection.Tracking.Enabled;
            PerformanceCounter = idpeSection.Tracking.PerformanceCounter;
            SilientBusinessRuleError = idpeSection.Tracking.SilientBusinessRuleError;
            UpdateMatrix = idpeSection.Tracking.UpdateMatrix;
            UpdateMatrixFormat = idpeSection.Tracking.UpdateMatrixFormat;
        }

        [CategoryAttribute("Sumplus Rule Engine"),
        DescriptionAttribute("")]
        public Boolean PullersEnabled { get; set; }

        [CategoryAttribute("Sumplus Rule Engine"),
        DescriptionAttribute("Complete job time out in minutes")]
        public int TimeOut { get; set; }

        [CategoryAttribute("Sumplus Rule Engine"),
        DescriptionAttribute("One worker timeout in minutes")]
        public int WorkerTimeOut { get; set; }


        [CategoryAttribute("Local File Watcher"),
        DescriptionAttribute(""),
        DefaultValueAttribute(false)]
        public Boolean LocalFileWatcherEnabled { get; set; }

        [CategoryAttribute("Local File Watcher"),
        DescriptionAttribute("")]
        public String BaseDirectory { get; set; }

        [CategoryAttribute("Local File Watcher"),
        DescriptionAttribute("")]
        [IntegerValidator(ExcludeRange = false, MaxValue = 10, MinValue = 1)]
        public int RetryTimeOut { get; set; }

        [CategoryAttribute("Local File Watcher"),
        DescriptionAttribute("")]
        public String DirectoryPull
        {
            get
            {
                return Path.Combine(BaseDirectory, "InBound\\Pull");

            }
        }

        [CategoryAttribute("Local File Watcher"),
        DescriptionAttribute("")]
        public String DirectoryArchive
        {
            get
            {
                return Path.Combine(BaseDirectory, "InBound\\Archive");

            }
        }

        [CategoryAttribute("Local File Watcher"),
        DescriptionAttribute("")]
        public String DirectoryIgnored
        {
            get
            {
                return Path.Combine(BaseDirectory, "InBound\\Ignored");
            }
        }

        [CategoryAttribute("Local File Watcher"),
        DescriptionAttribute("")]
        public String DirectoryOutput
        {
            get
            {
                return Path.Combine(BaseDirectory, "OutBound\\Outputs");

            }
        }

        [CategoryAttribute("Microsoft Excel OLE Data Reader"),
        DescriptionAttribute("")]
        public Boolean MicrosoftExcelOLEDataReaderEnabled { get; set; }

        [CategoryAttribute("Microsoft Excel OLE Data Reader"),
        DescriptionAttribute(""),
        DefaultValue("Microsoft.ACE.OLEDB.12.0")]
        public string MicrosoftExcelOLEProvider { get; set; }

        [CategoryAttribute("Microsoft Excel OLE Data Reader"),
        DescriptionAttribute("")]
        public string MicrosoftExcelOLEVersion { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("")]
        public Boolean TrackingEnabled { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("")]
        public Boolean PerformanceCounter { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("")]
        public Boolean SilientBusinessRuleError { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("")]
        public Boolean UpdateMatrix { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("")]
        public String UpdateMatrixFormat { get; set; }


        public void Save(Configuration configuration)
        {
            base.Save(configuration);
            IdpeConfigurationSection idpeSection = (IdpeConfigurationSection)configuration.GetSection("idpeConfigurationSection");

            idpeSection.PullersEnabled = PullersEnabled;
            idpeSection.TimeOut = TimeOut;
            idpeSection.WorkerTimeOut = WorkerTimeOut;
            idpeSection.LocalFileWatcher.Enabled = LocalFileWatcherEnabled;
            idpeSection.LocalFileWatcher.BaseDirectory = BaseDirectory;
            idpeSection.LocalFileWatcher.RetryTimeOut = RetryTimeOut;
            idpeSection.MicrosoftExcelOLEDataReader.Enabled = MicrosoftExcelOLEDataReaderEnabled;
            idpeSection.MicrosoftExcelOLEDataReader.Provider = MicrosoftExcelOLEProvider;
            idpeSection.MicrosoftExcelOLEDataReader.Version = MicrosoftExcelOLEVersion;

            idpeSection.Tracking.Enabled = TrackingEnabled;
            idpeSection.Tracking.PerformanceCounter = PerformanceCounter;
            idpeSection.Tracking.SilientBusinessRuleError = SilientBusinessRuleError;
            idpeSection.Tracking.UpdateMatrix = UpdateMatrix;
            idpeSection.Tracking.UpdateMatrixFormat = UpdateMatrixFormat;

            configuration.Save();
        }
    }


}


