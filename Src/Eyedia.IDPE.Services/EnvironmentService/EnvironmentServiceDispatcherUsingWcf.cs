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
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Clients;

namespace Eyedia.IDPE.Services
{
    public class EnvironmentServiceDispatcherUsingWcf: EnvironmentServiceDispatcher
    {
        public EnvironmentServiceDispatcherUsingWcf() : base(true) { }

        public static IdpeEnvironmentServiceClient GetWcfClient(SreEnvironmentConfig toEnvironment)
        {
            return new IdpeEnvironmentServiceClient(new System.ServiceModel.NetTcpBinding(), 
                new System.ServiceModel.EndpointAddress(toEnvironment.RemoteUrl));
        }

        public static IdpeEnvironmentServiceClient GetWcfClient(string remoteUrl)
        {
            return new IdpeEnvironmentServiceClient(new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress(remoteUrl));
        }

        public override string DeployDataSource(SreEnvironmentConfig toEnvironment, int datasourceId)
        {
            base.DeployDataSource(toEnvironment, datasourceId);
            return GetWcfClient(toEnvironment).Deploy(Packet);            
        }

        public override string DeployRule(SreEnvironmentConfig toEnvironment, IdpeRule rule)
        {
            base.DeployRule(toEnvironment, rule);
            return GetWcfClient(toEnvironment).Deploy(Packet);
        }
      
        public override string DeployKeys(SreEnvironmentConfig toEnvironment, List<IdpeKey> keys)
        {
            base.DeployKeys(toEnvironment, keys);
            return GetWcfClient(toEnvironment).Deploy(Packet);
        }

        public override string DeploySdf(SreEnvironmentConfig toEnvironment)
        {           
            string sourceFileName = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].GetSdfFileName();

            Packet.FileTransferPacket = new FileTransferPacket();
            Packet.FileTransferPacket.FileName = sourceFileName;
            Packet.FileTransferPacket.Content = File.ReadAllBytes(sourceFileName);
            return GetWcfClient(toEnvironment).DeploySdf(Packet);            
        }
        public override string RestartService(SreEnvironmentConfig toEnvironment)
        {
            return GetWcfClient(toEnvironment).ExecuteCommand(new EnvironmentServicePacket(EnvironmentServiceCommands.Restart));           
        }
        public override string StopSqlPuller(SreEnvironmentConfig toEnvironment, int datasourceId)
        {
            Packet.Command = EnvironmentServiceCommands.StopSqlPuller;
            Packet.Params.Add("DataSourceId", datasourceId.ToString());
            return GetWcfClient(toEnvironment).ExecuteCommand(Packet);
            
        }
        public override string StartSqlPuller(SreEnvironmentConfig toEnvironment, int datasourceId)
        {
            Packet.Command = EnvironmentServiceCommands.StartSqlPuller;
            Packet.Params.Add("DataSourceId", datasourceId.ToString());
            return GetWcfClient(toEnvironment).ExecuteCommand(Packet);            
        }
        public override string ExecuteCommand(SreEnvironmentConfig toEnvironment, string commandFileName)
        {
            Packet.Command = EnvironmentServiceCommands.ExecuteDOSCommand;
            Packet.Params.Add("Command", commandFileName);
            return GetWcfClient(toEnvironment).ExecuteCommand(Packet);            
        }
        public override string ProcessFile(SreEnvironmentConfig toEnvironment, int dataSourceId, string fileName)
        {
            Packet.DataSourceId = dataSourceId;
            Packet.FileTransferPacket = new FileTransferPacket();
            Packet.FileTransferPacket.FileName = fileName;
            Packet.FileTransferPacket.Content = File.ReadAllBytes(fileName);
            return GetWcfClient(toEnvironment).ProcessFile(Packet);            
        }

        public override List<IdpeDataSource> GetDataSources(SreEnvironmentConfig toEnvironment)
        {
            EnvironmentServicePacket packet = new EnvironmentServicePacket();
            return new List<IdpeDataSource>(GetWcfClient(toEnvironment).GetDataSources(packet));
        }
        public override FileTransferPacket GetConfigFile(SreEnvironmentConfig toEnvironment, string configFileName)
        {
            EnvironmentServicePacket packet = new EnvironmentServicePacket();
            packet.ConfigFileName = configFileName;
            return GetWcfClient(toEnvironment).GetConfigFile(packet);
        }
        public override string SetConfigFile(SreEnvironmentConfig toEnvironment, FileTransferPacket fileTransferPacket)
        {            
            Packet.FileTransferPacket = fileTransferPacket;
            return GetWcfClient(toEnvironment).SetConfigFile(Packet);
        }

        public override string DeployArtifacts(SreEnvironmentConfig toEnvironment)
        {
            if (base.DeployArtifacts(toEnvironment) == Constants.success)
            {
                GetWcfClient(toEnvironment).DeployArtifacts(Packet);
                return Constants.success;
            }
            else
            {
                return Constants.failed;
            }
        }

        public override string SetServiceLogonUser(SreEnvironmentConfig toEnvironment, string batchFileContent)
        {
            base.SetServiceLogonUser(toEnvironment, batchFileContent);
            //GetWcfClient(toEnvironment).SetServiceLogonUser(Packet);
            return Constants.success;
        }

        public override FileTransferPacket GetLastDeploymentLog(SreEnvironmentConfig fromEnvironment)
        {           
            return GetWcfClient(fromEnvironment).GetLastDeploymentLog();          
        }

        public override string StopService(SreEnvironmentConfig toEnvironment)
        {
            return GetWcfClient(toEnvironment).ExecuteCommand(new EnvironmentServicePacket(EnvironmentServiceCommands.Stop));
        }       

    }
}


