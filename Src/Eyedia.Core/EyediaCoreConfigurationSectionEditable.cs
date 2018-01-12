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
using System.ComponentModel;
using Eyedia.Core.Data;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;
using Eyedia.Core.Net;

namespace Eyedia.Core
{
    /// <summary>
    /// Editable items of EyediaCoreConfiguration. This object helps you to bind configuration into Property grid and save changes back to config file
    /// </summary>
    public class EyediaCoreConfigurationSectionEditable
    {

        public EyediaCoreConfigurationSectionEditable(Configuration configuration)
        {
            EyediaCoreConfigurationSection sreSection = (EyediaCoreConfigurationSection)configuration.GetSection("eyediaCoreConfigurationSection");
            
            InstanceName = sreSection.InstanceName;
            HostingEnvironment = sreSection.HostingEnvironment;
            AuthenticationType = sreSection.AuthenticationType;
            AuthorizedGroups = sreSection.AuthorizedGroups;
            Cache = sreSection.Cache;
            Debug = sreSection.Debug;
            TempDirectory = sreSection.TempDirectory;
            OutDirectory = sreSection.OutDirectory;
            RecordsPerThread = sreSection.RecordsPerThread;
            MaxThreads = sreSection.MaxThreads;
            CurrentCulture = sreSection.CurrentCulture;

            DatabaseType = sreSection.Database.DatabaseType;
            ConnectionString = configuration.ConnectionStrings.ConnectionStrings["cs"].ConnectionString;

            EmailEnabled = sreSection.Email.Enabled;
            SmtpServer = sreSection.Email.SmtpServer;
            FromEmail = sreSection.Email.FromEmail;
            FromDisplayName = sreSection.Email.FromDisplayName;
            ToEmails = sreSection.Email.ToEmails;
            UserName = sreSection.Email.UserName;
            Password = sreSection.Email.Password;
            MaxNumberOfEmails = sreSection.Email.MaxNumberOfEmails;

            TraceEnabled = sreSection.Trace.Enabled;
            File = sreSection.Trace.File;
            Filter = sreSection.Trace.Filter;
            EmailError = sreSection.Trace.EmailError;

            TrackingEnabled = sreSection.Tracking.Enabled;
            PerformanceCounter = sreSection.Tracking.PerformanceCounter;
            SilientBusinessRuleError = sreSection.Tracking.SilientBusinessRuleError;
            UpdateMatrix = sreSection.Tracking.UpdateMatrix;
            UpdateMatrixFormat = sreSection.Tracking.UpdateMatrixFormat;
            Value = sreSection.Tracking.Value;

        }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("A string value which identifies this instance(this will contain in subject line of all exception email). e.g. - SRE.PROD"),
        DefaultValueAttribute("SRE.PROD")]
        public String InstanceName { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Select one of the hosting environment. This setting helps SRE to take various automatic decisions"),
        DefaultValueAttribute(Eyedia.Core.EyediaCoreConfigurationSection.HostingEnvironments.WindowsService)]
        public Eyedia.Core.EyediaCoreConfigurationSection.HostingEnvironments HostingEnvironment { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Cache can be on or off"),
        DefaultValueAttribute(true)]
        public Boolean Cache { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Authentication type to be used in configurator user interface"),
        DefaultValueAttribute(AuthenticationTypes.ActiveDirectory)]
        public AuthenticationTypes AuthenticationType { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Active directory group name or names(comma separated) to enable authorization")]
        public string AuthorizedGroups { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Debug mode"),
        DefaultValueAttribute(false)]
        public Boolean Debug { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Temp directory to be used internally. Note - SRE cleans this directory everym mid night"),
        DefaultValueAttribute("c:\\temp")]
        public String TempDirectory { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("This is not in use"),
        DefaultValueAttribute("c:\\temp")]
        public String OutDirectory { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Number of records per thread"),
        DefaultValueAttribute(3)]
        public int RecordsPerThread { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Maximum threads."),
        DefaultValueAttribute(20)]
        public int MaxThreads { get; set; }

        [CategoryAttribute("Global Settings"),
        DescriptionAttribute("Current Culture"),
        DefaultValueAttribute("en-US")]
        public CultureInfo CurrentCulture { get; set; }


        /*Database - Start */
        [CategoryAttribute("Database"),
        DescriptionAttribute("Select database type. Note - Connection string should support selected database type"),
        DefaultValueAttribute(DatabaseTypes.SqlCe)]
        public DatabaseTypes DatabaseType { get; set; }

        [CategoryAttribute("Database"),
        DescriptionAttribute("Connection string. Note - Connection string should support selected database type")]
        public string ConnectionString { get; set; }

        /*Database - End */

        /*Email - Start */
        [CategoryAttribute("Email"),
        DescriptionAttribute("If configured, SRE will send all exception/business errors emails to configured email ids"),
        DefaultValueAttribute(true)]
        public bool EmailEnabled { get; set; }

        [CategoryAttribute("Email"),
        DescriptionAttribute("The SMTP server")]
        public string SmtpServer { get; set; }

        [CategoryAttribute("Email"),
        DescriptionAttribute("From email id")]
        public string FromEmail { get; set; }

        [CategoryAttribute("Email"),
       DescriptionAttribute("From email display name")]
        public string FromDisplayName { get; set; }

        [CategoryAttribute("Email"),
        DescriptionAttribute("To email ids, separated by comma(,)"),
        DefaultValueAttribute(true)]
        public string ToEmails { get; set; }

        [CategoryAttribute("Email"),
        DescriptionAttribute("(Optional) user name"),
        DefaultValueAttribute(true)]
        public string UserName { get; set; }

        [CategoryAttribute("Email"),
        DescriptionAttribute("(Optional) user password"),
        DefaultValueAttribute(true)]
        public string Password { get; set; }

        [CategoryAttribute("Email"),
        DescriptionAttribute("(Optional, default - 10) Max number of same subject, same body emails"),
        DefaultValueAttribute(10)]
        public int MaxNumberOfEmails { get; set; }

        /*Email - End */

        /*Trace - Start */
        [CategoryAttribute("Trace"),
        DescriptionAttribute("Trace to log file"),
        DefaultValueAttribute(true)]
        public bool TraceEnabled { get; set; }

        [CategoryAttribute("Trace"),
        DescriptionAttribute("Trace log file name")]
        public string File { get; set; }

        [CategoryAttribute("Trace"),
        DescriptionAttribute("Trace filter"),
        DefaultValueAttribute(SourceLevels.Error)]
        public SourceLevels Filter { get; set; }

        [CategoryAttribute("Trace"),
        DescriptionAttribute("SRE automatically sends all exceptions if enabled"),
        DefaultValueAttribute(true)]
        public bool EmailError { get; set; }
        /*Trace - End */

        /*Tracking - Start */
        [CategoryAttribute("Tracking"),
        DescriptionAttribute("Keeps track of attribute values, performace counter, etc"),
        DefaultValueAttribute(false)]
        public bool TrackingEnabled { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("If enabled SRE keeps track of elapsed time of all important operations. For example, if a web method is being called within a business rule, SRE can keep track of elapsed time to that call"),
        DefaultValueAttribute(false)]
        public bool PerformanceCounter { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("SRE will capture all exceptions in business rule silently and send email"),
        DefaultValueAttribute(false)]
        public bool SilientBusinessRuleError { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("If update matrix is enabled SRE keeps track of each update to each attribute value"),
        DefaultValueAttribute(false)]
        public bool UpdateMatrix { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("Update matrix format. The default is '[{0}] ? {1} &gt;&gt;'"),
        DefaultValueAttribute("[{0}] ? {1} &gt;&gt;")]
        public string UpdateMatrixFormat { get; set; }

        [CategoryAttribute("Tracking"),
        DescriptionAttribute("")]
        public string Value { get; set; }

        /*Tracking - End */

        public void Save(Configuration configuration)
        {
            EyediaCoreConfigurationSection sreSection = (EyediaCoreConfigurationSection)configuration.GetSection("eyediaCoreConfigurationSection");

            sreSection.InstanceName = InstanceName;
            sreSection.HostingEnvironment = HostingEnvironment;
            sreSection.AuthenticationType = AuthenticationType;
            sreSection.AuthorizedGroups = AuthorizedGroups;
            sreSection.Cache = Cache;
            sreSection.Debug = Debug;
            sreSection.TempDirectory = TempDirectory;
            sreSection.OutDirectory = OutDirectory;
            sreSection.RecordsPerThread = RecordsPerThread;
            sreSection.MaxThreads = MaxThreads;
            sreSection.CurrentCulture = CurrentCulture;

            sreSection.Database.DatabaseType = DatabaseType;

            sreSection.Email.Enabled = EmailEnabled;
            sreSection.Email.SmtpServer = SmtpServer;
            sreSection.Email.FromEmail = FromEmail;
            sreSection.Email.FromDisplayName = FromDisplayName;
            sreSection.Email.ToEmails = ToEmails;
            sreSection.Email.UserName = UserName;
            sreSection.Email.Password = Password;
            sreSection.Email.MaxNumberOfEmails = MaxNumberOfEmails;

            sreSection.Trace.Enabled = TraceEnabled;
            sreSection.Trace.File = File;
            sreSection.Trace.Filter = Filter;
            sreSection.Trace.EmailError = EmailError;

            sreSection.Tracking.Enabled = TrackingEnabled;
            sreSection.Tracking.PerformanceCounter = PerformanceCounter;
            sreSection.Tracking.SilientBusinessRuleError = SilientBusinessRuleError;
            sreSection.Tracking.UpdateMatrix = UpdateMatrix;
            sreSection.Tracking.UpdateMatrixFormat = UpdateMatrixFormat;
            sreSection.Tracking.Value = Value;

            configuration.ConnectionStrings.ConnectionStrings["cs"].ConnectionString = ConnectionString;
            configuration.Save();

        }
    }
}


