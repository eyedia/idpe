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
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.Linq;
using System.Threading;
using System.Workflow.Runtime;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Collections;
using System.Text;
using Eyedia.Core.Data;
using System.IO;
using System.Configuration;

namespace Eyedia.IDPE.Services
{
    [ServiceContract]
    public interface IIdpeEnvironmentService    
    {
        [OperationContract]
        string Ping(EnvironmentServicePacket packet);

        [OperationContract]
        int GetNumberOfInstances(EnvironmentServicePacket packet);

        [OperationContract]
        string ExecuteCommand(EnvironmentServicePacket packet);

        [OperationContract]
        string Deploy(EnvironmentServicePacket packet);
     
        [OperationContract]
        string DeploySdf(EnvironmentServicePacket packet);

        [OperationContract]
        string DeployArtifacts(EnvironmentServicePacket packet);

        [OperationContract]
        FileTransferPacket GetLastDeploymentLog();

        [OperationContract]
        string ProcessFile(EnvironmentServicePacket packet);

        [OperationContract]
        List<SreDataSource> GetDataSources(EnvironmentServicePacket packet);

        [OperationContract]
        FileTransferPacket GetConfigFile(EnvironmentServicePacket packet);

        [OperationContract]
        string SetConfigFile(EnvironmentServicePacket packet);

        [OperationContract]
        string SetServiceLogonUser(EnvironmentServicePacket packet);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class IdpeEnvironmentService: IIdpeEnvironmentService
    {
        string sysDir = Path.Combine(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, "100");

        public string Ping(EnvironmentServicePacket packet)
        {
            return "success";
        }

        public int GetNumberOfInstances(EnvironmentServicePacket packet)
        {
            return SreEnvironment.GetNumberOfInstances();
        }

        public string ExecuteCommand(EnvironmentServicePacket packet)
        {
            string response = string.Empty;
                       
            if (packet.Command != Common.EnvironmentServiceCommands.Deploy)                          
                return new CommandExecutor(packet).Execute();            

            return response;
        }

        public string Deploy(EnvironmentServicePacket packet)
        {
            string response = string.Empty;

            if (packet.Command == Common.EnvironmentServiceCommands.Deploy)
            {
                packet.DataSourceBundle.IsImporting = true;
                Trace.TraceInformation("srecmd: Deploying");
                packet.DataSourceBundle.Import(packet.FromEnvironment.IpAddress, packet.FromEnvironment.MachineName);
            }           

            return response;

        }

        public string DeployArtifacts(EnvironmentServicePacket packet)
        {           
            Trace.TraceInformation("srecmd: Deploying artifacts...");
            return new CommandExecutor(packet).Execute();            
        }

        public FileTransferPacket GetLastDeploymentLog()
        {
            FileTransferPacket response = new FileTransferPacket();
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sreupdate.log");
            if (File.Exists(fileName))
            {
                response.FileName = fileName;
                response.Content = File.ReadAllBytes(fileName);
            }
            return response;
        }

        public string DeploySdf(EnvironmentServicePacket packet)
        {
            string response = "success";
            string fileName = packet.FileTransferPacket.FileName;
            if (packet.FileTransferPacket.Content != null)
            {
                //save locally in temp location
                fileName = Path.Combine(Information.TempDirectorySre, Path.GetFileName(packet.FileTransferPacket.FileName));
                this.SaveFileStream(fileName, new MemoryStream(packet.FileTransferPacket.Content));
            }
            DeploySdfInternal(fileName);
            return response;
        }

        public List<SreDataSource> GetDataSources(EnvironmentServicePacket packet)
        {
            List<SreDataSource> dataSources = new Manager().GetDataSources().OrderBy(ds => ds.Name).ToList();
            dataSources.Where(ds => ds.DataFeederType == null).ToList().ForEach(ds1 => ds1.DataFeederType = 0);
            return dataSources;
        }

        public string ProcessFile(EnvironmentServicePacket packet)
        {
            if (string.IsNullOrEmpty(new Manager().GetApplicationName(packet.DataSourceId)))
                return "No data source defined with id " + packet.DataSourceId;

            string response = "success";
            string fileName = packet.FileTransferPacket.FileName;
            if (packet.FileTransferPacket.Content != null)
            {
                //save locally in temp location
                fileName = Path.Combine(Information.TempDirectorySre, Path.GetFileName(packet.FileTransferPacket.FileName));
                this.SaveFileStream(fileName, new MemoryStream(packet.FileTransferPacket.Content));
            }
            ProcessFileInternal(fileName, packet.DataSourceId);
            return response;
        }

        public FileTransferPacket GetConfigFile(EnvironmentServicePacket packet)
        {            
            FileTransferPacket response = new FileTransferPacket();
            if (File.Exists(packet.ConfigFileName + ".config"))
            {
                response.FileName = packet.ConfigFileName;
                response.Content = File.ReadAllBytes(packet.ConfigFileName + ".config");
            }
            return response;
        }

        public string SetConfigFile(EnvironmentServicePacket packet)
        {                        
            string tempFileName = Path.Combine(Information.TempDirectorySre, Path.GetFileName(packet.FileTransferPacket.FileName));
            if (packet.FileTransferPacket.Content != null)
                this.SaveFileStream(tempFileName, new MemoryStream(packet.FileTransferPacket.Content));
            
            if (File.Exists(packet.FileTransferPacket.FileName))           
                new FileUtility().Backup(packet.FileTransferPacket.FileName);

            File.Copy(tempFileName, packet.FileTransferPacket.FileName, true);

            return Constants.success;
        }

        public string SetServiceLogonUser(EnvironmentServicePacket packet)
        {
            Trace.TraceInformation("srecmd: Setting logon user credentials...");
            string batchFile = Path.Combine(Information.TempDirectoryTempData, DateTime.Now.ToString("yyyyMMddHHmmss") + ".bat");
            StreamWriter sw = new StreamWriter(batchFile);
            sw.Write(packet.Params["BatchFileContent"]);
            sw.Close();
            Eyedia.Core.Windows.Utilities.WindowsUtility.ExecuteBatchFile(batchFile, true);
            Trace.TraceInformation("srecmd: Restarting the service(s)...");
            Trace.Flush();
            Eyedia.Core.Windows.Utilities.WindowsUtility.ExecuteBatchFile(EnvironmentFiles.RestartFileName, true);
            return Constants.success;
        }

        #region Helpers

        private void ProcessFileInternal(string fileName, int dataSourceId)
        {
            Trace.TraceInformation("srecmd: Processing file " + Path.GetFileName(fileName));
            File.Copy(fileName, Path.Combine(DataSource.GetPullFolder(dataSourceId, new Manager().GetKeys(dataSourceId)), Path.GetFileName(fileName)));
        }

        private void DeploySdfInternal(string newFileNameToBeDeployed)
        {
            Trace.TraceInformation("srecmd: Deploying sdf " + Path.GetFileName(newFileNameToBeDeployed));

            //backup
            string currentSdfFile = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].GetSdfFileName();       
            new FileUtility().Backup(currentSdfFile, false);
            //delete old
            File.Delete(currentSdfFile);

            //copy keep same file name so that we dont have to update config files            
            File.Copy(newFileNameToBeDeployed, currentSdfFile, true);

            //restart
            EnvironmentServicePacket packet = new EnvironmentServicePacket();
            packet.Command = Common.EnvironmentServiceCommands.Restart;
            new CommandExecutor(packet).Execute();
        }
     

        /// <summary>
        /// Write the Stream in the hard drive
        /// </summary>
        /// <param name="filePath">path to write the file in</param>
        /// <param name="stream">stream to write</param>
        private void SaveFileStream(string filePath, Stream stream)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Dispose();
        }
        #endregion Helpers
    }
}


