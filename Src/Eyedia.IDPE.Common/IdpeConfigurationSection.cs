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
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Diagnostics;
using System.IO;
using CultureInfo = System.Globalization.CultureInfo;
using Eyedia.Core.Data;

namespace Eyedia.IDPE.Common
{
    public class IdpeConfigurationSection : ConfigurationSection
    {

        [ConfigurationProperty("pullersEnabled", DefaultValue = "true")]
        public Boolean PullersEnabled
        {
            get
            { return (Boolean)this["pullersEnabled"]; }
            set
            { this["pullersEnabled"] = value; }
        }

        [ConfigurationProperty("localFileWatcher")]
        public LocalFileWatcherElement LocalFileWatcher
        {
            get
            {
                return (LocalFileWatcherElement)this["localFileWatcher"];
            }
            set
            { this["localFileWatcher"] = value; }
        }

        [ConfigurationProperty("trace")]
        public TraceElement Trace
        {
            get
            {
                return (TraceElement)this["trace"];
            }
            set
            { this["trace"] = value; }
        }
       
        [ConfigurationProperty("microsoftExcelOLEDataReader")]
        public MicrosoftExcelOLEDataReaderElement MicrosoftExcelOLEDataReader
        {
            get
            {
                return (MicrosoftExcelOLEDataReaderElement)this["microsoftExcelOLEDataReader"];
            }
            set
            { this["microsoftExcelOLEDataReader"] = value; }
        }

        [ConfigurationProperty("tracking")]
        public TrackingElement Tracking
        {
            get
            {
                return (TrackingElement)this["tracking"];
            }
            set
            { this["tracking"] = value; }
        }

        /// <summary>
        /// All thread timeout in minutes
        /// </summary>
        [ConfigurationProperty("timeOut", DefaultValue = 3)]
        public int TimeOut
        {
            get
            {
                return (int)this["timeOut"];
            }
            set
            { this["timeOut"] = value; }
        }

        /// <summary>
        /// Worker timeout in minutes
        /// </summary>
        [ConfigurationProperty("workerTimeOut", DefaultValue = 60)]
        public int WorkerTimeOut
        {
            get
            {
                return (int)this["workerTimeOut"];
            }
            set
            { this["workerTimeOut"] = value; }
        }

        public static IdpeConfigurationSection CurrentConfig
        {
            get
            {
                return (IdpeConfigurationSection)ConfigurationManager.GetSection("idpeConfigurationSection");

            }
        }

    }

    public class LocalFileWatcherElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public Boolean Enabled
        {
            get
            { return (Boolean)this["enabled"]; }
            set
            { this["enabled"] = value; }
        }

        [ConfigurationProperty("baseDirectory", IsRequired = true)]
        public String BaseDirectory
        {
            get
            { return (String)this["baseDirectory"]; }
            set
            { this["baseDirectory"] = value; }
        }

        [ConfigurationProperty("retryTimeOut", DefaultValue = 10)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 10, MinValue = 1)]
        public int RetryTimeOut
        {
            get
            { return (int)this["retryTimeOut"]; }
            set
            { this["retryTimeOut"] = value; }
        }

        public String DirectoryPull 
        { 
            get 
            {
                string dir = Path.Combine(BaseDirectory, "InBound\\Pull");
                Directory.CreateDirectory(dir);
                return dir;
            } 
        }

        public String DirectoryArchive
        {
            get
            {
                string dir = Path.Combine(BaseDirectory, "InBound\\Archive");
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public String DirectoryIgnored
        {
            get
            {
                string dir = Path.Combine(BaseDirectory, "InBound\\Ignored");
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public String DirectoryOutput
        {
            get
            {
                string dir = Path.Combine(BaseDirectory, "OutBound\\Outputs");
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

    }

    public class TraceElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public Boolean Enabled
        {
            get
            { return (Boolean)this["enabled"]; }
            set
            { this["enabled"] = value; }
        }

        [ConfigurationProperty("filter", DefaultValue = SourceLevels.Error)]
        public SourceLevels Filter
        {
            get
            { return (SourceLevels)this["filter"]; }
            set
            { this["filter"] = value; }
        }

        [ConfigurationProperty("file", IsRequired = true)]
        public String File
        {
            get
            { return (String)this["file"]; }
            set
            { this["file"] = value; }
        }

        [ConfigurationProperty("emailError", DefaultValue = "true")]
        public Boolean EmailError
        {
            get
            { return (Boolean)this["emailError"]; }
            set
            { this["emailError"] = value; }
        }
    }
   
    public class MicrosoftExcelOLEDataReaderElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", DefaultValue = "false")]
        public Boolean Enabled
        {
            get
            { return (Boolean)this["enabled"]; }
            set
            { this["enabled"] = value; }
        }

        [ConfigurationProperty("provider", DefaultValue = "Microsoft.ACE.OLEDB.12.0")]
        public string Provider
        {
            get
            { return this["provider"].ToString(); }
            set
            { this["provider"] = value; }
        }

        [ConfigurationProperty("version", DefaultValue = "12.0")]
        public string Version
        {
            get
            { return this["version"].ToString(); }
            set
            { this["version"] = value; }
        }

    }

    public class TrackingElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public Boolean Enabled
        {
            get
            { return (Boolean)this["enabled"]; }
            set
            { this["enabled"] = value; }
        }

        [ConfigurationProperty("performanceCounter", DefaultValue = "false")]
        public Boolean PerformanceCounter
        {
            get
            { return (Boolean)this["performanceCounter"]; }
            set
            { this["performanceCounter"] = value; }
        }


        [ConfigurationProperty("silientBusinessRuleError", DefaultValue = "true")]
        public Boolean SilientBusinessRuleError
        {
            get
            { return (Boolean)this["silientBusinessRuleError"]; }
            set
            { this["silientBusinessRuleError"] = value; }
        }

        [ConfigurationProperty("updateMatrix", DefaultValue = "false")]
        public Boolean UpdateMatrix
        {
            get
            { return (Boolean)this["updateMatrix"]; }
            set
            { this["updateMatrix"] = value; }
        }

        [ConfigurationProperty("updateMatrixFormat", DefaultValue = "false")]
        public String UpdateMatrixFormat
        {
            get
            { return (String)this["updateMatrixFormat"]; }
            set
            { this["updateMatrixFormat"] = value; }
        }


    }


}


