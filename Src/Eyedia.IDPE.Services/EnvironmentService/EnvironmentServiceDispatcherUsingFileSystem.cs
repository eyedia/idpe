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

namespace Eyedia.IDPE.Services
{
    public class EnvironmentServiceDispatcherUsingFileSystem : EnvironmentServiceDispatcher
    {
        public EnvironmentServiceDispatcherUsingFileSystem() : base(false)
        { }

        /// <summary>
        /// Deploys data source into specific targetConfigironment
        /// </summary>
        /// <param name="toEnvironment">Target environment</param>
        /// <param name="datasourceId">Data source id</param>       
        public override string DeployDataSource(SreEnvironmentConfig toEnvironment, int datasourceId)
        {
            base.DeployDataSource(toEnvironment, datasourceId);
            return CreateAndCopyFile(toEnvironment, Packet) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Deploys data source into specific targetConfigironment
        /// </summary>
        /// <param name="toConfig">Target environment</param>
        /// <param name="rule">The rule object</param>
        public override string DeployRule(SreEnvironmentConfig toEnvironment, SreRule rule)
        {
            base.DeployRule(toEnvironment, rule);
            return CreateAndCopyFile(toEnvironment, Packet) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Deploys data source into specific targetConfigironment
        /// </summary>
        /// <param name="toConfig">Target environment</param>       
        public override string DeployKeys(SreEnvironmentConfig toEnvironment, List<SreKey> keys)
        {
            base.DeployKeys(toEnvironment, keys);
            return CreateAndCopyFile(toEnvironment, Packet) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Deploys data source into specific environment
        /// </summary>
        /// <param name="targetConfig">Target environment</param>       
        public override string DeploySdf(SreEnvironmentConfig targetConfig)
        {
            string sourceFileName = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].GetSdfFileName();
            Packet.FileTransferPacket = new FileTransferPacket();
            Packet.FileTransferPacket.FileName = sourceFileName;
            Packet.FileTransferPacket.Content = File.ReadAllBytes(sourceFileName);
            return CreateAndCopyFile(targetConfig, Packet) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Restarts service
        /// </summary>
        /// <param name="targetConfig">Target environment</param>        
        public override string RestartService(SreEnvironmentConfig targetConfig)
        {
            return CreateAndCopyFile(targetConfig, new EnvironmentServicePacket(EnvironmentServiceCommands.Restart)) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Restarts service
        /// </summary>
        /// <param name="targetConfig">Target environment</param>      
        /// <param name="dataSourceId">Data source id</param>
        public override string StopSqlPuller(SreEnvironmentConfig targetConfig, int dataSourceId)
        {
            EnvironmentServicePacket packet = new EnvironmentServicePacket(EnvironmentServiceCommands.StopSqlPuller);
            packet.DataSourceId = dataSourceId;
            return CreateAndCopyFile(targetConfig, packet) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Restarts service
        /// </summary>
        /// <param name="targetConfig">Target environment</param>      
        /// <param name="dataSourceId">Data source id</param>
        public override string StartSqlPuller(SreEnvironmentConfig targetConfig, int dataSourceId)
        {
            EnvironmentServicePacket packet = new EnvironmentServicePacket(EnvironmentServiceCommands.StartSqlPuller);
            packet.DataSourceId = dataSourceId;
            return CreateAndCopyFile(targetConfig, packet) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Executes command or batch file
        /// </summary>
        /// <param name="targetConfig">Target environment</param>      
        /// <param name="commandFileName">Command file name relative to target service path</param>
        public override string ExecuteCommand(SreEnvironmentConfig targetConfig, string commandFileName)
        {
            EnvironmentServicePacket packet = new EnvironmentServicePacket(EnvironmentServiceCommands.ExecuteDOSCommand);
            packet.Params.Add("Command", commandFileName);
            return CreateAndCopyFile(targetConfig, packet) ? Constants.success : Constants.failed;
        }

        /// <summary>
        /// Deploys data source into specific environment
        /// </summary>
        /// <param name="targetConfig">Target environment</param>       
        public override string ProcessFile(SreEnvironmentConfig targetConfig, int datasourceId, string fileName)
        {
            string destFileName = Path.Combine(targetConfig.Environment.PullFolder, "100", Path.GetFileName(fileName));
            File.Copy(fileName, destFileName, true);
            return "success";
        }

        public override List<SreDataSource> GetDataSources(SreEnvironmentConfig targetConfig)
        {
            EnvironmentServicePacket packet = new EnvironmentServicePacket();
            return new IdpeEnvironmentService().GetDataSources(packet);
        }
        public override FileTransferPacket GetConfigFile(SreEnvironmentConfig targetConfig, string configFileName)
        {
            FileTransferPacket packet = new FileTransferPacket();
            packet.FileName = configFileName + ".config";
            packet.Content = File.ReadAllBytes(configFileName + ".config");
            return packet;
        }
        public override string SetConfigFile(SreEnvironmentConfig targetConfig, FileTransferPacket fileTransferPacket)
        {           
            Packet.FileTransferPacket = fileTransferPacket;
            return new IdpeEnvironmentService().SetConfigFile(Packet);
        }

        public override string DeployArtifacts(SreEnvironmentConfig toEnvironment)
        {
            if (base.DeployArtifacts(toEnvironment) == Constants.success)
            {
                return CreateAndCopyFile(toEnvironment, Packet) ? Constants.success : Constants.failed;
            }
            else
            {
                return Constants.failed;
            }
        }

        public override FileTransferPacket GetLastDeploymentLog(SreEnvironmentConfig fromEnvironment)
        {
            FileTransferPacket packet = new FileTransferPacket();
            packet.FileName = Path.Combine(fromEnvironment.Environment.RootFolder, "sreupdate.log");
            if (File.Exists(packet.FileName))
                packet.Content = File.ReadAllBytes(packet.FileName);
            return packet;
        }

        public override string StopService(SreEnvironmentConfig toEnvironment)
        {
            return CreateAndCopyFile(toEnvironment, new EnvironmentServicePacket(EnvironmentServiceCommands.Stop)) ? Constants.success : Constants.failed;
        }

        public override string SetServiceLogonUser(SreEnvironmentConfig toEnvironment, string batchFileContent)
        {
            base.SetServiceLogonUser(toEnvironment, batchFileContent);
            return CreateAndCopyFile(toEnvironment, Packet) ? Constants.success : Constants.failed;
        }

        #region Helpers
        private bool CreateAndCopyFile(SreEnvironmentConfig targetConfig, EnvironmentServicePacket packet)
        {
            try
            {
                string sourceFileName = Path.Combine(Path.GetTempPath(), ShortGuid.NewGuid().Value + ".sred");
                StreamWriter sw = new StreamWriter(sourceFileName);
                sw.Write(packet.Serialize());
                sw.Close();
                File.Copy(sourceFileName, GetEnvironmentTargetLocation(targetConfig), true);
                return true;
            }
            catch (Exception ex)
            {
                CallCallBack(ex.Message, true);
                return false;
            }
        }

        private string GetEnvironmentTargetLocation(SreEnvironmentConfig targetConfig)
        {
            string destFileName = Path.Combine(targetConfig.Environment.PullFolder, "100", ShortGuid.NewGuid().Value + ".sred");
            if (!Directory.Exists(Path.GetDirectoryName(destFileName)))
                throw new DirectoryNotFoundException(string.Format("The directory '{0}' does not exist on '{1}' environment, please make sure the environment is correctly configured!",
                    Path.GetDirectoryName(destFileName), targetConfig.Environment.Name));

            return destFileName;
        }
      
        #endregion Helpers
    }
}


