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
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Diagnostics;
using System.Threading;

namespace Eyedia.Core.Windows.Utilities
{
    public class WindowsUtility
    {
        public static void SetFolderPermission(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            IdentityReference everybodyIdentity = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            FileSystemAccessRule rule = new FileSystemAccessRule(everybodyIdentity, FileSystemRights.FullControl,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
            DirectorySecurity ds = Directory.GetAccessControl(folder);
            ds.AddAccessRule(rule);
            Directory.SetAccessControl(folder, ds); 

        }

        public static int NumberOfCores
        {
            get
            {
                int numberOfCores = 0;
                
                foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                {
                    numberOfCores += int.Parse(item["NumberOfCores"].ToString());
                }
                return numberOfCores;
            }
        }

        public static int PhysicalProcessors
        {
            get
            {
                int numberOfProcessors = 0;
                foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
                {
                    int.TryParse(item["NumberOfProcessors"].ToString(), out numberOfProcessors);
                }
                return numberOfProcessors;
            }
        }

        public static void ExecuteBatchFile(string batchFile, bool runAsAdministrator = false)
        {
            Trace.TraceInformation("Executing batch file '{0}'", batchFile);
            Trace.Flush();
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            p.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");
            p.StartInfo.Arguments = string.Format("/C \"{0}\"", batchFile);
            if(runAsAdministrator)
                p.StartInfo.Verb = "runas";

            p.StartInfo.ErrorDialog = true;
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            bool result = p.Start();
            Trace.TraceInformation("Executed. Return code was '{0}'", result);
            Trace.Flush();
        }

        public static bool ExecuteDosCommand(string command, bool useCmdExeWrapper, ref List<string> output, ref List<string> error)
        {
            List<string> internalOutput = new List<string>();
            List<string> internalError = new List<string>();
            int timeout = 1000 * 60 * 2;    //2 minutes


            using (Process process = new Process())
            {
                if (useCmdExeWrapper)
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/C " + command;
                }
                else
                {                    
                    process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    process.StartInfo.FileName = command;
                }
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            internalOutput.Add(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            internalError.Add(e.Data);
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    if (process.WaitForExit(timeout)
                        && outputWaitHandle.WaitOne(timeout)
                        && errorWaitHandle.WaitOne(timeout))
                    {
                        // Process completed. Check process.ExitCode here.    
                        if (error.Count == 0)
                        {
                        }
                    }

                    else
                    {
                        return false;
                    }
                }

            }
            output = internalOutput;
            error = internalError;
            return true;
        }
    }
}





