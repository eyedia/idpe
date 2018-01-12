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
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Diagnostics;
using System.Configuration;
using SymplusRuleEngine.Common;
using System.IO;
using System.ServiceProcess;
using SymplusRuleEngine.Core.Encryption;
using SymplusRuleEngine.Host.Windows;

namespace SymplusRuleEngine.Host.Windows
{
    [RunInstaller(true)]
    public partial class SymplusRuleEngineInstaller : Installer
    {
        public SymplusRuleEngineInstaller()
        {
            InitializeComponent();
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        const string MainExeName = "SymplusRuleEngineService.exe";
        const string ConnectionString = "Data Source='{0}scfcg.sdf';password=acc3s$";

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            
            string installedDir = Context.Parameters["TARGETDIR"];
            string mainExe = string.Format("{0}{1}", installedDir, MainExeName);
            string utilExe = string.Format("{0}{1}", installedDir, "sreutil.exe");
            

            var configuration = ConfigurationManager.OpenExeConfiguration(mainExe);
            ConnectionStringsSection conSection = (ConnectionStringsSection)configuration.GetSection("connectionStrings"); 
            conSection.ConnectionStrings.Clear();
            conSection.ConnectionStrings.Add(new ConnectionStringSettings("cs", string.Format(ConnectionString, installedDir)));

            AppSettingsSection section = (AppSettingsSection)configuration.GetSection("appSettings");
            section.Settings["DatabaseType"].Value = "SQLCe2005";
            section.Settings["SREInstanceName"].Value = Context.Parameters["SREINSTANCENAME"];
            section.Settings["ToEmailIds"].Value = Context.Parameters["SREERROREMAILS"];
            section.Settings["TempFolder"].Value = Context.Parameters["TEMPFOLDER"]; Directory.CreateDirectory(Context.Parameters["TEMPFOLDER"]);
            string rootFolder = Context.Parameters["ROOTFOLDER"]; Directory.CreateDirectory(Context.Parameters["ROOTFOLDER"]);
            if ((rootFolder.Length > 1) && (rootFolder.Substring(rootFolder.Length - 1, 1) != "\\"))
                rootFolder = rootFolder + "\\";

            section.Settings["LocalFileWatcherFolderNamePull"].Value = string.Format("{0}AppData\\InBound\\Pull", rootFolder); Directory.CreateDirectory(string.Format("{0}AppData\\InBound\\Pull", rootFolder));
            section.Settings["LocalFileWatcherFolderNameArchive"].Value = string.Format("{0}AppData\\InBound\\Archive", rootFolder); Directory.CreateDirectory(string.Format("{0}AppData\\InBound\\Archive", rootFolder));
            section.Settings["LocalFileWatcherFolderNameOutput"].Value = string.Format("{0}AppData\\OutBound\\Output", rootFolder); Directory.CreateDirectory(string.Format("{0}AppData\\OutBound\\Output", rootFolder));
            section.Settings["CustomTraceFilePath"].Value = string.Format("{0}Logs\\sre.log", installedDir); Directory.CreateDirectory(string.Format("{0}Logs", installedDir));

            configuration.Save();

            configuration = ConfigurationManager.OpenExeConfiguration(utilExe);
            section = (AppSettingsSection)configuration.GetSection("appSettings");
            section.Settings["Debug"].Value = "False";
            section.Settings["DatabaseType"].Value = "SQLCe2005";

            conSection = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
            conSection.ConnectionStrings.Clear();
            conSection.ConnectionStrings.Add(new ConnectionStringSettings("cs", string.Format(ConnectionString, installedDir)));
            configuration.Save();

            //finally encrypt config
            Encryptor.EncryptAppConfigSections(mainExe, "appSettings,connectionStrings");
            Encryptor.EncryptAppConfigSections(utilExe, "appSettings,connectionStrings");

            //aspnet_regiis -pa "NetFrameworkConfigurationKey" "NT AUTHORITY\NETWORK SERVICE"
            string aspnet_regiis = string.Format("{0}aspnet_regiis.exe", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
            ProcessStartInfo procStartInfo = new ProcessStartInfo(aspnet_regiis, "-pa \"NetFrameworkConfigurationKey\" \"NT AUTHORITY\\NETWORK SERVICE\"");
            procStartInfo.CreateNoWindow = true;
            procStartInfo.UseShellExecute = false;
            Process proc = new Process();

            proc.StartInfo = procStartInfo;
            bool result = proc.Start(); 


        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            string installedDir = Context.Parameters["TARGETDIR"];
            string mainExe = string.Format("{0}{1}", installedDir, "SymplusRuleEngineService.exe");
            //EventLog.WriteEntry(Information.EventLogSource, mainExe, EventLogEntryType.Error);
            //StopService("Supply Chain Finance Common Gateway", 12000);
            ManagedInstallerClass.InstallHelper(new string[] { "/u", mainExe });
            base.Uninstall(savedState);
        }


        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            //ServiceController controller = new ServiceController(ProjectInstaller._ServiceName);            
            try
            {
                string installedDir = Context.Parameters["TARGETDIR"];
                string mainExe = string.Format("{0}{1}", installedDir, "SymplusRuleEngineService.exe");                
                //EventLog.WriteEntry(Information.EventLogSource, installedDir, EventLogEntryType.Error);
                //Process.Start(mainExe, "-u");
                //if (controller.Status == ServiceControllerStatus.Running | controller.Status == ServiceControllerStatus.Paused)
                //{
                //    EventLog.WriteEntry(Information.EventLogSource, "3", EventLogEntryType.Error);
                //    controller.Stop();
                //    controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 15));
                //    controller.Close();
                //}                
            }
            catch (Exception ex)
            {                
                EventLog.WriteEntry(Information.EventLogSource, string.Concat(@"The service could not be stopped. Please stop the service manually. Error ", ex.Message), EventLogEntryType.Error);

            }
            finally
            {             
                base.OnBeforeUninstall(savedState);
            }

        }	 
 


        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(Information.EventLogSource, ex.ToString(), EventLogEntryType.Error);
            }
        }


    }
}






