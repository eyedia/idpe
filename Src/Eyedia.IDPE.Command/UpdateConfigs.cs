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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Core;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Command
{
    public class UpdateConfigs
    {
        public string RootDirectory { get; private set; }
        public string DirectoryLogs { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"); } }
        public string DirectoryTemp { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp"); } }
        public string DirectoryWatchFolder { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WatchFolder"); } }

        public UpdateConfigs(string rootDirectory = null)
        {
            if (rootDirectory == null)
                rootDirectory = AppDomain.CurrentDomain.BaseDirectory;

            RootDirectory = rootDirectory;
        }
        public void Update(bool setFolder = false)
        {            
            if(setFolder)
            {
                Directory.CreateDirectory(DirectoryLogs);
                Directory.CreateDirectory(DirectoryTemp);
                Directory.CreateDirectory(DirectoryWatchFolder);
            }
            UpdateOne("idpe.exe", setFolder);
            UpdateOne("idped.exe", setFolder);
            UpdateOne("idpec.exe", setFolder);
        }

        private void UpdateOne(string configName, bool setFolder = false)
        {
            string configPath = Path.Combine(RootDirectory, configName);
            if (!File.Exists(configPath))
            {
                Console.WriteLine("{0} skipped", configName);
                return;
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);
            EyediaCoreConfigurationSection eccs = (EyediaCoreConfigurationSection)config.GetSection("eyediaCoreConfigurationSection");
            if(eccs == null)
            {
                Console.WriteLine("{0} skipped", configName);
                return;
            }
            eccs.Database.DatabaseType = Core.Data.DatabaseTypes.SqlCe;
            eccs.Debug = false;
            eccs.AuthenticationType = Core.Net.AuthenticationTypes.Eyedia;

            if (setFolder)
            {
                if (eccs != null)
                {
                    eccs.TempDirectory = DirectoryTemp;
                    eccs.Trace.Filter = System.Diagnostics.SourceLevels.Information;
                    if(configName == "idpe.exe")
                        eccs.Trace.File = Path.Combine(DirectoryLogs, "idpe.txt");
                    else if (configName == "idped.exe")
                        eccs.Trace.File = Path.Combine(DirectoryLogs, "idped.txt");
                    else if (configName == "idpec.exe")
                        eccs.Trace.File = Path.Combine(DirectoryLogs, "idpec.txt");
                }
                IdpeConfigurationSection idpecs = (IdpeConfigurationSection)config.GetSection("idpeConfigurationSection");
                if (idpecs != null)
                {
                    idpecs.LocalFileWatcher.BaseDirectory = DirectoryWatchFolder;
                }            
            }
            var cscs = (ConnectionStringsSection)config.GetSection("connectionStrings");
            cscs.ConnectionStrings.Clear();            
            cscs.ConnectionStrings.Add(new ConnectionStringSettings("cs", "Data Source='samples.sdf';password=acc3s$"));
            config.Save(ConfigurationSaveMode.Full);
            Console.WriteLine("{0} saved", configName);
        }
    }
}
