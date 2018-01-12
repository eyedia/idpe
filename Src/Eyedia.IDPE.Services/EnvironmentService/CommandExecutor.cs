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
using System.Diagnostics;
using System.Threading.Tasks;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Threading;
using System.IO;
using Eyedia.Core.Windows.Utilities;

namespace Eyedia.IDPE.Services
{
    public class CommandExecutor
    {
        
        public EnvironmentServicePacket Packet { get; set; }

        public CommandExecutor(EnvironmentServicePacket packet)
        {
            this.Packet = packet;
        }

        public string Execute()
        {
            switch (Packet.Command)
            {               
                case EnvironmentServiceCommands.Restart:
                    Trace.TraceInformation("srecmd: Restarting the service");                    
                    Environment.Exit(1);    //the OS will recover it, need to set 'Restart After Failure' in Recovery Tab
                    break;
              
                case EnvironmentServiceCommands.Stop:
                    Trace.TraceInformation("srecmd: Starting the service(s)");
                    WindowsUtility.ExecuteBatchFile(EnvironmentFiles.StartFileName, true);
                    break;

                case EnvironmentServiceCommands.StopSqlPuller:
                    Trace.TraceInformation("srecmd: Stopping sql puller datasource id = " + Packet.Params["DataSourceId"]);
                    new Idpe().StopSqlPuller(int.Parse(Packet.Params["DataSourceId"]));
                    return Constants.success;
                    break;

                case EnvironmentServiceCommands.StartSqlPuller:
                    Trace.TraceInformation("srecmd: Starting sql puller datasource id = " + Packet.Params["DataSourceId"]);
                    new Idpe().StartSqlPuller(int.Parse(Packet.Params["DataSourceId"]));
                    return Constants.success;
                    break;

                case EnvironmentServiceCommands.ExecuteDOSCommand:
                    ExecuteCommand(Packet.Params["Command"]);
                    break;

                case EnvironmentServiceCommands.DeployArtifacts:
                    Trace.TraceInformation("srecmd: Deplying dlls");
                    new DeploymentService().DeployArtifacts(Packet.FileTransferPacket);
                    break;
              
                default:
                    throw new InvalidOperationException(string.Format("srecmd: Command executor does not recognize '{0}' command", Packet.Command.ToString()));
            }

            return Constants.failed;
        }
      
        private string ExecuteCommand(string command)
        {
            if (Packet.Params.Count > 0)
            {
                Trace.TraceInformation("srecmd: Executing command = " + command);
                try
                {
                    List<string> output = new List<string>();
                    List<string> error = new List<string>();
                    Eyedia.Core.Windows.Utilities.WindowsUtility.ExecuteDosCommand(Packet.Params["Command"], false, ref output, ref error);
                    Trace.TraceInformation("srecmd: OS Command '{0}' Starts-----------------------------------------------------------"
                        , Packet.Params["Command"]);
                    Trace.TraceInformation(output.ToLine());
                    Trace.TraceInformation(error.ToLine());
                    Trace.TraceInformation("srecmd: OS Command '{0}' Ends--------------------------------------------------------------"
                        , Packet.Params["Command"]);

                    if (error.Count == 0)
                        return Constants.success + " : Output = " + output.ToLine();
                    else
                        return Constants.failed + " : Output = " + output.ToLine() + " : Errors = " + error.ToLine();
                }
                catch (Exception ex)
                {
                    return Constants.failed + " :" + ex.Message;
                }

                return Constants.failed;

            }

            return Constants.failed;
        }
       

    }
}


