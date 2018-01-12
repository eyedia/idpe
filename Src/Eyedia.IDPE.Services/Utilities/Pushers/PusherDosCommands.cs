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
using System.Threading;
using System.Text.RegularExpressions;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;


namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Executes DOS commands
    /// </summary>
    public class PusherDosCommands : Pushers
    {
        /// <summary>
        /// Initializes PusherDosCommands object
        /// </summary>
        public PusherDosCommands() { }

        /// <summary>
        /// Executes DOS commands
        /// </summary>
        /// <param name="e">PullersEventArgs</param>
        public override void FileProcessed(PullersEventArgs e)
        {
            List<string> commands = new List<string>(e.Job.DataSource.PusherTypeFullName.Split(Environment.NewLine.ToCharArray()));
            commands = commands.RemoveEmptyStrings();

            foreach (string command in commands)
            {
                if (command.StartsWith("_if", StringComparison.OrdinalIgnoreCase))
                {
                    string updatedCommand = string.Empty;
                    if (EvaluateIf(e, command, ref updatedCommand))
                        Execute(e, updatedCommand);
                }
                else
                {
                    Execute(e, command);
                }
            }
        }

        public void Process(PullersEventArgs e, string rawCommands)
        {
            List<string> commands = new List<string>(rawCommands.Split(Environment.NewLine.ToCharArray()));
            commands = commands.RemoveEmptyStrings();

            foreach (string command in commands)
            {
                if (command.StartsWith("_if", StringComparison.OrdinalIgnoreCase))
                {
                    string updatedCommand = string.Empty;
                    if (EvaluateIf(e, command, ref updatedCommand))
                        Execute(e, updatedCommand);
                }
                else
                {
                    Execute(e, command);
                }
            }
        }

        private bool EvaluateIf(PullersEventArgs e, string command, ref string updatedCommand)
        {
            Regex regex = new Regex("_if\\((.*?)\\)");
            Match match = regex.Match(command);
            string full = match.Groups[0].ToString();
            string processVariableName = match.Groups[1].ToString();
            updatedCommand = command.Replace(full, string.Empty).Trim();

            if (e.Job.ProcessVariables.ContainsKey(processVariableName))
                return e.Job.ProcessVariables[processVariableName].ToString().ParseBool();
            else
                return false;

        }

        #region Private Methods

        private string DataSourceInformation(PullersEventArgs e)
        {
            string dsInfo = string.Format("DataSource Id = {0}{1}", e.Job.DataSource.Id, Environment.NewLine);
            dsInfo += string.Format("DataSource Name = {0}{1}", e.Job.DataSource.Name, Environment.NewLine);
            return dsInfo;
        }

        private void Execute(PullersEventArgs e, string command)
        {
            List<string> output = new List<string>();
            List<string> error = new List<string>();

            command = ParseCommand(e, command);
            Eyedia.Core.Windows.Utilities.WindowsUtility.ExecuteDosCommand(command, true, ref output, ref error);

            #region Error Handling

            if (error.Count != 0)
            {
                if (!ErrorCanBeIgnored(command, error))
                {
                    string commandError = error.ToLine("<br />");
                    commandError = PostMan.__errorStartTag + commandError + PostMan.__errorEndTag;

                    string errorMessage = "Error while executing pusher commands!" + Environment.NewLine;
                    errorMessage += string.Format("Command executed = '{0}'", command);
                    errorMessage += string.Format(". The error was {0}", commandError);
                    errorMessage += DataSourceInformation(e);
                    new PostMan(e.Job, true).Send(errorMessage, "Pusher Failed");
                    ExtensionMethods.TraceInformation(errorMessage);
                }
            }

            if (ValidateExecution(output) != true)
            {
                string commandError = output.ToLine("<br />");
                commandError = PostMan.__errorStartTag + commandError + PostMan.__errorEndTag;

                string errorMessage = "Error while executing pusher commands!" + Environment.NewLine;
                errorMessage += string.Format("Command executed = '{0}'", command);
                errorMessage += string.Format(". The error was {0}", commandError);
                errorMessage += DataSourceInformation(e);
                new PostMan(e.Job, true).Send(errorMessage, "Pusher Failed");
                ExtensionMethods.TraceInformation(errorMessage);
            }

            #endregion Error Handling
        }

        private bool ErrorCanBeIgnored(string command, List<string> error)
        {
            if (command.StartsWith("mkdir", StringComparison.OrdinalIgnoreCase))
            {
                if ((error.Count > 0)
                    && (error[0].StartsWith("A subdirectory or file", StringComparison.OrdinalIgnoreCase)))
                    return true;
            }
            return false;

        }

        private string ParseCommand(PullersEventArgs e, string command)
        {
            return new SreCommandParser(e.Job.DataSource).Parse(command, e.Job.FileName, e.OutputFileName);
        }

        //private string ParseCommand(PullersEventArgs e, string command)
        //{
        //    ExtensionMethods.TraceInformation("Parsing DOS command:" + command);

        //    command = command.Replace("_filei", "\"" + e.Job.FileName + "\"");
        //    command = command.Replace("_fileo", "\"" + e.OutputFileName + "\"");

        //    command = GetFolderConfiguration(command, "_dira");
        //    command = GetFolderConfiguration(command, "_diro");
        //    command = GetFolderConfiguration(command, "_dirp");

        //    command = command.Replace("_dira", e.Job.DataSource.LocalFileSystemFolderArchiveFolder);
        //    command = command.Replace("_diro", e.Job.DataSource.LocalFileSystemFolderOutputFolder);
        //    command = command.Replace("_dirp", e.Job.DataSource.LocalFileSystemFolderPullFolder);

        //    Regex regex = new Regex("_key\\((.*?)\\)");
        //    MatchCollection matches = regex.Matches(command);
        //    foreach (Match match in matches)
        //    {
        //        string fullExpr = match.Groups[0].ToString();
        //        string keyName = match.Groups[1].ToString();

        //        string keyValue = e.Job.DataSource.Keys.GetKeyValue(keyName);
        //        command = command.Replace(fullExpr, "\"" + keyValue + "\"");
        //    }


        //    ExtensionMethods.TraceInformation("Parsed DOS command  :" + command);
        //    return command;
        //}

        //private string GetFolderConfiguration(string command, string folderType)
        //{
        //    Regex regex = new Regex(folderType + "\\((.*?)\\)");
        //    MatchCollection matches = regex.Matches(command);
        //    foreach (Match match in matches)
        //    {
        //        string fullExpr = match.Groups[0].ToString();
        //        string strDataSourceId = match.Groups[1].ToString();

        //        int dataSourceId = 0;
        //        if (int.TryParse(strDataSourceId, out dataSourceId))
        //        {                    
        //            DataSource ds = new DataSource(dataSourceId, string.Empty);
        //            if (ds.Keys == null)
        //                throw new Exception(string.Format("Data source with id {0} referred in post event DOS commands was not defined!", dataSourceId));

        //            string folder = string.Empty;
        //            if (folderType == "_dira")
        //                folder = DataSource.GetArchiveFolder(dataSourceId, ds.Keys);
        //            else if (folderType == "_diro")
        //                folder = DataSource.GetOutputFolder(dataSourceId, ds.Keys);
        //            else if (folderType == "_dirp")
        //                folder = DataSource.GetPullFolder(dataSourceId, ds.Keys);
        //            else
        //                ThrowInvalidCommandException (folderType);

        //            command = command.Replace(fullExpr, folder);
        //        }
        //    }

        //    return command;
        //}

        //private void ThrowInvalidCommandException(string command)
        //{
        //    throw new Exception(string.Format("'{0}' is not recognized as a valid SRE command!", command));
        //}

            /*
        private bool ExecuteDosCommand(string command, ref List<string> output, ref List<string> error)
        {
            List<string> internalOutput = new List<string>();
            List<string> internalError = new List<string>();
            int timeout = 1000 * 60 * 2;    //2 minutes


            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/C " + command;
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
        }      */
      
        private bool ValidateExecution(List<string> output)
        {
            foreach (string line in output)
            {
                if (line.Contains("Error"))
                    return false;
                else if (line.Contains("Exception"))
                    return false;
                else if (line.Contains("Invalid syntax"))
                    return false;
                else if (line.Contains("0 file(s) copied"))
                    return false;
            }
            return true;
        }
        #endregion Private Methods
    }

   
}


