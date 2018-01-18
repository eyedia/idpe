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
using System.Security.Permissions;
using Eyedia.Core.Net;

namespace Eyedia.Core
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class EyediaCoreConfigurationSection : ConfigurationSection
    {
        public enum HostingEnvironments { Library, WindowsService, Web }

        [ConfigurationProperty("instanceName", DefaultValue = "IDPE", IsRequired = true)]
        public String InstanceName
        {
            get
            {
                return (String)this["instanceName"];
            }
            set
            {
                this["instanceName"] = value;
            }
        }

        [ConfigurationProperty("hostingEnvironment", DefaultValue = "WindowsService")]
        public HostingEnvironments HostingEnvironment
        {
            get
            {
                return (HostingEnvironments)this["hostingEnvironment"];
            }
            set
            {
                this["hostingEnvironment"] = value;
            }
        }

        [ConfigurationProperty("authenticationType", DefaultValue = "ActiveDirectory", IsRequired = true)]
        public AuthenticationTypes AuthenticationType
        {
            get
            {
                return (AuthenticationTypes)this["authenticationType"];
            }
            set
            {
                this["cache"] = value;
            }
        }

        [ConfigurationProperty("authorizedGroups", IsRequired = false)]
        public string AuthorizedGroups
        {
            get
            {
                return (string)this["authorizedGroups"];
            }
            set
            {
                this["cache"] = value;
            }
        }

        [ConfigurationProperty("cache", DefaultValue = "true", IsRequired = false)]
        public Boolean Cache
        {
            get
            {
                return (Boolean)this["cache"];
            }
            set
            {
                this["cache"] = value;
            }
        }

        [ConfigurationProperty("debug", DefaultValue = "false", IsRequired = false)]
        public Boolean Debug
        {
            get
            {
                return (Boolean)this["debug"];
            }
            set
            {
                this["debug"] = value;
            }
        }


        [ConfigurationProperty("tempDirectory", DefaultValue = "c:\\temp", IsRequired = true)]
        [StringValidator(MinLength = 4, MaxLength = 128)]
        public String TempDirectory
        {
            get
            {
                return (String)this["tempDirectory"];
            }
            set
            {
                this["tempDirectory"] = value;
            }
        }

        [ConfigurationProperty("outDirectory", DefaultValue = "c:\\temp")]
        [StringValidator(MinLength = 4)]
        public String OutDirectory
        {
            get
            {
                return (String)this["outDirectory"];
            }
            set
            {
                this["outDirectory"] = value;
            }
        }

        [ConfigurationProperty("recordsPerThread", DefaultValue = 3)]
        [IntegerValidator(ExcludeRange = false, MinValue = 3)]
        public int RecordsPerThread
        {
            get
            { return (int)this["recordsPerThread"]; }
            set
            { this["recordsPerThread"] = value; }
        }

        [ConfigurationProperty("autoRecordsPerThread", DefaultValue = false, IsRequired = false)]
        public bool AutoRecordsPerThread
        {
            get
            { return (bool)this["autoRecordsPerThread"]; }
            set
            { this["autoRecordsPerThread"] = value; }
        }

        [ConfigurationProperty("maxThreads", DefaultValue = 20)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 100, MinValue = 2)]
        public int MaxThreads
        {
            get
            { return (int)this["maxThreads"]; }
            set
            { this["maxThreads"] = value; }
        }

        [ConfigurationProperty("currentCulture", DefaultValue = "en")]
        public System.Globalization.CultureInfo CurrentCulture
        {
            get
            {
                if ((this["currentCulture"] != null)
                    && (this["currentCulture"] is System.Globalization.CultureInfo))
                    return (System.Globalization.CultureInfo)this["currentCulture"];

                string text = (string)this["currentCulture"];
                return (text == null) ? System.Globalization.CultureInfo.CreateSpecificCulture("en") : System.Globalization.CultureInfo.CreateSpecificCulture(text);
            }
            set
            {
                this["currentCulture"] = value;
            }
        }


        [ConfigurationProperty("database", IsRequired = true)]
        public DatabaseElement Database
        {
            get
            {
                return (DatabaseElement)this["database"];
            }
            set
            { this["database"] = value; }
        }

        [ConfigurationProperty("email", IsRequired = true)]
        public EmailElement Email
        {
            get
            {
                return (EmailElement)this["email"];
            }
            set
            { this["email"] = value; }
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

        [ConfigurationProperty("accelerators", IsDefaultCollection = true)]
        public EyediaConfigElements Accelerators
        {
            get
            {
                EyediaConfigElements accelerators = (EyediaConfigElements)this["accelerators"];
                return accelerators;
            }
        }

        public static EyediaCoreConfigurationSection CurrentConfig
        {
            get
            {
                return (EyediaCoreConfigurationSection)ConfigurationManager.GetSection("eyediaCoreConfigurationSection");

            }
        }
    }


    public class DatabaseElement : ConfigurationElement
    {
        [ConfigurationProperty("databaseType", DefaultValue = "SqlCe", IsRequired = false)]
        public DatabaseTypes DatabaseType
        {
            get
            {
                return (DatabaseTypes)this["databaseType"];
            }
            set
            {
                this["databaseType"] = value;
            }
        }
     
    }

    public class EmailElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", DefaultValue = "true", IsRequired = true)]
        public Boolean Enabled
        {
            get
            { return (Boolean)this["enabled"]; }
            set
            { this["enabled"] = value; }
        }

        [ConfigurationProperty("smtpServer", IsRequired = true)]
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String SmtpServer
        {
            get
            {
                return (String)this["smtpServer"];
            }
            set
            {
                this["smtpServer"] = value;
            }
        }

        [ConfigurationProperty("smtpPort", IsRequired = false)]       
        public int SmtpPort
        {
            get
            {
                return (int)this["smtpPort"];
            }
            set
            {
                this["smtpPort"] = value;
            }
        }

        [ConfigurationProperty("enableSsl", IsRequired = false)]
        public bool EnableSsl
        {
            get
            {
                return (bool)this["enableSsl"];
            }
            set
            {
                this["enableSsl"] = value;
            }
        }

        [ConfigurationProperty("fromEmail", IsRequired = true)]
        public String FromEmail
        {
            get
            { return (String)this["fromEmail"]; }
            set
            { this["fromEmail"] = value; }
        }

        [ConfigurationProperty("fromDisplayName", IsRequired = false)]
        public String FromDisplayName
        {
            get
            { return (String)this["fromDisplayName"]; }
            set
            { this["fromDisplayName"] = value; }
        }

        [ConfigurationProperty("toEmails", IsRequired = true)]
        public String ToEmails
        {
            get
            { return (String)this["toEmails"]; }
            set
            { this["toEmails"] = value; }
        }

        [ConfigurationProperty("userName", IsRequired = false)]
        public String UserName
        {
            get
            { return (String)this["userName"]; }
            set
            { this["userName"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = false)]
        public String Password
        {
            get
            { return (String)this["password"]; }
            set
            { this["password"] = value; }
        }

        [ConfigurationProperty("maxAttachmentSize", IsRequired = false, DefaultValue = 10.00)]
        public double MaxAttachmentSize
        {
            get
            { return (double)this["maxAttachmentSize"]; }
            set
            { this["maxAttachmentSize"] = value; }
        }

        [ConfigurationProperty("maxNumberOfEmails", IsRequired = false, DefaultValue = 10)]
        public int MaxNumberOfEmails
        {
            get
            { return (int)this["maxNumberOfEmails"]; }
            set
            { this["maxNumberOfEmails"] = value; }
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

        [ConfigurationProperty("maxFileCount", IsRequired = false, DefaultValue = 100)]
        public int MaxFileCount
        {
            get
            { return (int)this["maxFileCount"]; }
            set
            { this["maxFileCount"] = value; }
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

        [ConfigurationProperty("updateMatrixFormat")]
        public String UpdateMatrixFormat
        {
            get
            { return (String)this["updateMatrixFormat"]; }
            set
            { this["updateMatrixFormat"] = value; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    }


}



