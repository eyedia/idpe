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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Services
{
    [CollectionDataContract]
    public class SreEnvironments : List<SreEnvironment>, IDisposable
    {
        public SreEnvironments() { }

        public SreEnvironments(bool fresh)
        {
            SreEnvironment localEnv = new SreEnvironment(true);
            localEnv.Name = "<local>";
            localEnv.RootFolder = AppDomain.CurrentDomain.BaseDirectory;
            localEnv.PullFolder = Path.Combine(localEnv.RootFolder, "WatchFolder");

            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe");
            if (File.Exists(configPath))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);
                EyediaCoreConfigurationSection eccs = (EyediaCoreConfigurationSection)config.GetSection("eyediaCoreConfigurationSection");
                if (eccs == null)
                {
                    SreConfigurationSection idpecs = (SreConfigurationSection)config.GetSection("sreConfigurationSection");
                    if (idpecs != null)
                    {
                        localEnv.RootFolder = AppDomain.CurrentDomain.BaseDirectory;
                        localEnv.PullFolder = idpecs.LocalFileWatcher.BaseDirectory;
                    }
                }
            }

            localEnv.LoadDefaultConfigs();
            this.Add(localEnv);
        }

        public SreEnvironment this[string environmentName]
        {
            get
            {
                return (from e in this
                        where e.Name.Equals(environmentName, StringComparison.OrdinalIgnoreCase)
                        select e).SingleOrDefault();

            }
        }
     
        #region IDisposable Members

        public void Dispose()
        {
            foreach (SreEnvironment e in this)
            {

            }
        }

        #endregion
    }

    [DataContract]
    public class SreEnvironment
    {
        public SreEnvironment()
        {
            this.EnvironmentConfigs = new List<SreEnvironmentConfig>();
        }

        public SreEnvironment(bool isLocal = false)
        {
            this.IsLocal = isLocal;
            this.EnvironmentConfigs = new List<SreEnvironmentConfig>();
        }        

        public string EnvXmlFile
        {
            get
            {
                return Path.Combine(RootFolder, "idpe.env.xml");
            }
        }

        [DataMember]
        public bool IsLocal { get; private set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RootFolder { get; set; }

        [DataMember]
        public string PullFolder { get; set; }

        [DataMember]
        public bool WcfMode { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
        public string Param1 { get; set; }

        [DataMember]
        public string Param2 { get; set; }

        [DataMember]
        public List<SreEnvironmentConfig> EnvironmentConfigs
        {
            get { return _EnvironmentConfigs; }
            set
            {
                _EnvironmentConfigs = value;
                var x = _EnvironmentConfigs.Where(s => s.IsService).GroupBy(ecf => ecf.InstanceNumber).Where(g => g.Skip(1).Any()).SelectMany(c => c).ToList();
                if (x.Count > 1)
                    throw new Exception("Can not have 2 instances with same name!");
                
            }
        }
        List<SreEnvironmentConfig> _EnvironmentConfigs;

        public List<SreEnvironmentConfig> EnvironmentConfigsInstancesOnly { get { return EnvironmentConfigs.Where(ecf => (ecf.IsService)).ToList(); } }
        
        public SreEnvironmentConfig GetEnvironmentConfig(int instanceNumber)
        {
            return EnvironmentConfigs.Where(ecf => ((ecf.IsService) && (ecf.InstanceNumber == instanceNumber))).SingleOrDefault();
        }

        public bool SetRemoteUrl(int instanceNumber, string remoteUrl)
        {
            if (GetEnvironmentConfig(instanceNumber) != null)
            {
                GetEnvironmentConfig(instanceNumber).RemoteUrl = remoteUrl;
                return true;
            }
            return false;
        }

        public static int GetNumberOfInstances()
        {
            int instances = 1;           
            if (Registry.Instance.EnvironmentVariables.ContainsKey("instances"))
                int.TryParse(Registry.Instance.EnvironmentVariables["instances"], out instances);

            return instances;
        }

        public int GetNumberOfInstances(string remoteUrl)
        {
            EnvironmentServicePacket packet = new EnvironmentServicePacket();
            return EnvironmentServiceDispatcherUsingWcf.GetWcfClient(remoteUrl).GetNumberOfInstances(packet);

        }

        public void SetNumberOfInstances(int noOfInstances)
        {
            if (this.IsLocal)
            {
                Dictionary<string,string> configs = EnvironmentFiles.ReadEnvironmentConfigs();
                if (Registry.Instance.EnvironmentVariables.ContainsKey("instances"))
                    Registry.Instance.EnvironmentVariables["instances"] = noOfInstances.ToString();
                else
                    Registry.Instance.EnvironmentVariables.Add("instances", noOfInstances.ToString());

                EnvironmentFiles.SaveEnvironmentConfigs(configs);

            }
        }

        public void LoadDefaultConfigs()
        {
            EnvironmentConfigs.Clear();
            EnvironmentConfigs.Add(new SreEnvironmentConfig(this, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idped.exe"), 0, false));
            EnvironmentConfigs.Add(new SreEnvironmentConfig(this, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe")));
            EnvironmentConfigsInstancesOnly[0].RemoteUrl = "net.tcp://localhost:7200/srees";
        }
        public void CheckAdditionalInstances(string remoteUrl)
        {
            int instances = GetNumberOfInstances(remoteUrl);
            
            for (int i = 1; i < instances; i++)
            {
                EnvironmentConfigs.Add(new SreEnvironmentConfig(this, Path.Combine(RootFolder, "idpe" + i + ".exe"), i));
            }
        }

        public void SetEnvironmentConfigsPath()
        {
            foreach(SreEnvironmentConfig config in EnvironmentConfigs)
            {
                config.ConfigFileName = Path.Combine(RootFolder, Path.GetFileName(config.ConfigFileName));               
            }


            if ((EnvironmentConfigs.Where(c => c.DisplayName == "Configurator").SingleOrDefault() != null)
                && (EnvironmentConfigsInstancesOnly.Where(ce => ce.InstanceNumber == 0).SingleOrDefault() != null))
                EnvironmentConfigs.Where(c => c.DisplayName == "Configurator").SingleOrDefault().RemoteUrl = EnvironmentConfigsInstancesOnly.Where(ce => ce.InstanceNumber == 0).SingleOrDefault().RemoteUrl;
                        
        }
    }

    [DataContract]
    public class SreEnvironmentConfig
    {

        [DataMember]
        public int InstanceNumber { get; set; }

        [DataMember]
        public bool IsService { get; set; }

        [DataMember]
        public string ConfigFileName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string RemoteUrl { get; set; }

        public SreEnvironment Environment { get; set; }

        public SreEnvironmentConfig(SreEnvironment environment, string configFileName, int instanceNumber = 0, bool isService = true)
        {
            this.Environment = environment;
            ConfigFileName = configFileName;
            this.InstanceNumber = instanceNumber;
            this.IsService = isService;
            DisplayName = IsService ? "Service " + instanceNumber : "IDE";
            RemoteUrl = "net.tcp://<remoteserver>:<port>/srees";

        }

        public SreEnvironmentConfig(SreEnvironment environment, int instanceNumber = 0, bool isService = true)
        {
            this.Environment = environment;
            this.InstanceNumber = instanceNumber;
            this.IsService = isService;
            RemoteUrl = "net.tcp://<remoteserver>:<port>/srees";
        }

    }
}


