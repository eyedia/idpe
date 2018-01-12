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
    public abstract class EnvironmentServiceDispatcher
    {
        public EnvironmentServiceDispatcher(bool wcfMode)
        {            
            this.WcfMode = wcfMode;
            this.Packet = new EnvironmentServicePacket();
            this.Packet.FromEnvironment = new SreEnvironment();
            this.Packet.FromEnvironment.IpAddress = Eyedia.Core.Net.Dns.GetLocalIpAddress();
            this.Packet.FromEnvironment.MachineName = Environment.MachineName;
        }
        public bool WcfMode { get; private set; }
        public delegate void CallbackEventHandler(string info, bool isError);
        public event CallbackEventHandler Callback;

        public virtual string DeployDataSource(SreEnvironmentConfig toEnvironment, int datasourceId)
        {
            Packet.DataSourceBundle = new DataSourceBundle();
            Packet.DataSourceBundle.Export(datasourceId, true);
            return Constants.success;
        }

        public virtual string DeployRule(SreEnvironmentConfig toEnvironment, IdpeRule rule)
        {
            Packet.DataSourceBundle = new DataSourceBundle();
            Packet.DataSourceBundle.Rules.Add(rule);
            return Constants.success;
        }
        public virtual string DeployKeys(SreEnvironmentConfig toEnvironment, List<IdpeKey> keys)
        {
            Packet.DataSourceBundle = new DataSourceBundle();
            Packet.DataSourceBundle.Keys.AddRange(keys);
            return Constants.success;
        }
        public abstract string DeploySdf(SreEnvironmentConfig toEnvironment);
        public virtual string DeployArtifacts(SreEnvironmentConfig toEnvironment)
        {            
            Packet.Command = EnvironmentServiceCommands.DeployArtifacts;
            Packet.FileTransferPacket = new FileTransferPacket();
            DeploymentService deploymentService = new DeploymentService();
            deploymentService.Callback += CallbackEvent;
            Packet.FileTransferPacket.FileName = deploymentService.PickupDlls();
            if (!string.IsNullOrEmpty(Packet.FileTransferPacket.FileName))
            {
                Packet.FileTransferPacket.Content = File.ReadAllBytes(Packet.FileTransferPacket.FileName);
                return Constants.success;
            }
            else
            {
                return Constants.failed;
            }
        }
        public abstract string RestartService(SreEnvironmentConfig toEnvironment);
        public abstract string StopService(SreEnvironmentConfig toEnvironment);        
        public abstract string StopSqlPuller(SreEnvironmentConfig toEnvironment, int datasourceId);
        public abstract string StartSqlPuller(SreEnvironmentConfig toEnvironment, int datasourceId);
        public abstract string ExecuteCommand(SreEnvironmentConfig toEnvironment, string commandFileName);
        public abstract string ProcessFile(SreEnvironmentConfig toEnvironment, int datasourceId, string fileName);
        public abstract List<IdpeDataSource> GetDataSources(SreEnvironmentConfig toEnvironment);
        public abstract FileTransferPacket GetConfigFile(SreEnvironmentConfig toEnvironment, string configFileName);
        public abstract string SetConfigFile(SreEnvironmentConfig toEnvironment, FileTransferPacket fileTransferPacket);
        public abstract FileTransferPacket GetLastDeploymentLog(SreEnvironmentConfig fromEnvironment);
        public virtual string SetServiceLogonUser(SreEnvironmentConfig toEnvironment, string batchFileContent)
        {
            Packet.Params.Add("BatchFileContent", batchFileContent);
            return Constants.success;
        }

        protected void SaveFileStream(string filePath, Stream stream)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Dispose();
        }

        protected void CallCallBack(string info, bool isError = false)
        {
            if (Callback != null)
                Callback(info, isError);
        }

        protected void CallbackEvent(string info, bool isError)
        {
            CallCallBack(info, isError);
        }

        public EnvironmentServicePacket Packet { get; private set; }
        
    }
}


