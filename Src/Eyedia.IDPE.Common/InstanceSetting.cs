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
using Eyedia.Core.Data;
using Eyedia.Core.Windows.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Configuration;

namespace Eyedia.IDPE.Common
{
    public class InstanceSettings
    {        

        public InstanceSettings(){}
        public InstanceSettings(int instanceNumber)
        {
            this.InstanceNumber = instanceNumber;         
        }
        
        public string SMTPServer { get; set; }
        public string FromEmailId { get; set; }
        public string ToEmailIds { get; set; }

        [CategoryAttribute("Rule Engine Settings - Local File System")]
        public string TempFolder { get; set; }

        [CategoryAttribute("Rule Engine Settings - Local File System")]
        public string LocalFileWatcherFolderNamePull { get; set; }

        [CategoryAttribute("Rule Engine Settings - Local File System")]
        public string LocalFileWatcherFolderNameArchive { get; set; }

        [CategoryAttribute("Rule Engine Settings - Local File System")]
        public string LocalFileWatcherFolderNameOutput { get; set; }

        [CategoryAttribute("Rule Engine Settings - Local File System")]
        public string LocalFileWatcherMaximumRetryPeriod { get; set; }

        [CategoryAttribute("Database Settings"),
        DefaultValueAttribute(DatabaseTypes.SqlCe)]
        public DatabaseTypes DatabaseType { get; set; }

        [CategoryAttribute("Database Settings")]
        public string ConnectionString { get; set; }


        [CategoryAttribute("Rule Engine Settings")]
        public int InstanceNumber { get; set; }

        [CategoryAttribute("Rule Engine Settings")]
        public string ServiceName { get; set; }

        [CategoryAttribute("Rule Engine Settings")]
        public string ServiceDescription { get; set; }

        [CategoryAttribute("Rule Engine Settings")]
        public string SREInstanceName { get; set; }

        [CategoryAttribute("Rule Engine Settings"),
        DefaultValueAttribute(true)]
        public string SRECache { get; set; }

        [CategoryAttribute("Rule Engine Settings"),
        DefaultValueAttribute(12)]
        public int MemoryReset { get; set; }

        [CategoryAttribute("Rule Engine Settings"),
        DefaultValueAttribute(true)]
        public bool IsSilientBusinessRuleError { get; set; }

        [CategoryAttribute("Rule Engine Settings"),
        DefaultValueAttribute(false)]
        public bool PerformanceCounter { get; set; }

        [CategoryAttribute("Rule Engine Settings"),
        DefaultValueAttribute(false)]
        public bool TrackUpdateMatrix { get; set; }

        [CategoryAttribute("Rule Engine Settings"),
        DefaultValueAttribute("[{0}] ? {1} >>")]
        public string UpdateMatrixFormat { get; set; }

        [CategoryAttribute("Tracing"),
        DefaultValueAttribute(true)]
        public bool TraceEnabled { get; set; }

        [CategoryAttribute("Tracing"),
        DefaultValueAttribute(SourceLevels.Information)]
        public SourceLevels TraceFilterType { get; set; }

        [CategoryAttribute("Tracing")]
        public string TraceFilePath { get; set; }

        [CategoryAttribute("Tracing"),
        DefaultValueAttribute(false)]
        public bool TraceDBLogErrors { get; set; }

        [CategoryAttribute("Tracing")]
        public string TraceDBConnectionStringName { get; set; }

        [CategoryAttribute("Tracing"),
        DefaultValueAttribute(true)]
        public bool EmailErrors { get; set; }



        public void WriteToRegistry(string registrySubKey)
        {
            RegistryUtility regUtil = new RegistryUtility(registrySubKey + "\\" +  this.InstanceNumber.ToString("00"));
            foreach (var prop in this.GetType().GetProperties())
            {
                regUtil.Write(prop.Name, prop.GetValue(this, null));
            }
        }
        public void DeleteFromRegistry(string registrySubKey)
        {
            RegistryUtility regUtil = new RegistryUtility(registrySubKey + "\\" + this.InstanceNumber.ToString("00"));
            regUtil.DeleteSubKeyTree();
        }

        public void ReadFromRegistry(string registrySubKey)
        {
            RegistryUtility regUtil = new RegistryUtility(registrySubKey + "\\" + this.InstanceNumber.ToString("00"));

            foreach (var prop in this.GetType().GetProperties())
            {

                string strValue = regUtil.Read(prop.Name).ToString();

                if (prop.PropertyType == typeof(Int32))
                {
                    int intVal = int.Parse(strValue);
                    prop.SetValue(this, intVal, null);
                }
                else if (prop.PropertyType == typeof(DatabaseTypes))
                {
                    DatabaseTypes dbType = (DatabaseTypes)Enum.Parse(typeof(DatabaseTypes), strValue);
                    prop.SetValue(this, dbType, null);
                }
                else if (prop.PropertyType == typeof(SourceLevels))
                {
                    SourceLevels level = (SourceLevels)Enum.Parse(typeof(SourceLevels), strValue);
                    prop.SetValue(this, level, null);
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    bool boolValue = bool.Parse(strValue);
                    prop.SetValue(this, boolValue, null);
                }
                else
                {
                    prop.SetValue(this, strValue, null);
                }
            }
        }
    }
}




