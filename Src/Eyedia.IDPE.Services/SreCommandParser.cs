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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Parses command
    /// </summary>
    public class SreCommandParser
    {
        public SreCommandParser(DataSource dataSource) { this.DataSource = dataSource; }

        public DataSource DataSource { get; private set; }

        public string Parse(string command, string inputFileName = null, string outputFileName = null)
        {
            //ExtensionMethods.TraceInformation("Parsing commands:" + command);
            //Trace.Indent();
            command = ParseDosCommands(command, inputFileName, outputFileName);           
            command = ParseSqlCommands(command);
            //Trace.Unindent();
            //ExtensionMethods.TraceInformation("Parsed commands:" + command);
            return command;
        }

        private string ParseSqlCommands(string command)
        {
            //ExtensionMethods.TraceInformation("Parsing Sql commands:" + command);
            command = command.Replace("_ipaddress", Core.Net.Dns.GetLocalIpAddress());
            command = command.Replace("_instancename", Common.Information.EyediaCoreConfigurationSection.InstanceName);
            //ExtensionMethods.TraceInformation("Parsed Sql commands:" + command);
            return command;
        }

        private string ParseDosCommands(string command, string inputFileName, string outputFileName)
        {

            //ExtensionMethods.TraceInformation("Parsing DOS commands:" + command);

            command = command.Replace("_filei", "\"" + inputFileName + "\"");
            command = command.Replace("_fileo", "\"" + outputFileName + "\"");

            command = GetFolderConfiguration(command, "_dira");
            command = GetFolderConfiguration(command, "_diro");
            command = GetFolderConfiguration(command, "_dirp");

            command = command.Replace("_dira", DataSource.LocalFileSystemFolderArchiveFolder);
            command = command.Replace("_diro", DataSource.LocalFileSystemFolderOutputFolder);
            command = command.Replace("_dirp", DataSource.LocalFileSystemFolderPullFolder);

            Regex regex = new Regex("_key\\((.*?)\\)");
            MatchCollection matches = regex.Matches(command);
            foreach (Match match in matches)
            {
                string fullExpr = match.Groups[0].ToString();
                string keyName = match.Groups[1].ToString();

                string keyValue = DataSource.Keys.GetKeyValue(keyName);
                command = command.Replace(fullExpr, "\"" + keyValue + "\"");
            }


            //ExtensionMethods.TraceInformation("Parsed DOS commands:" + command);
            return command;

        }

        private string GetFolderConfiguration(string command, string folderType)
        {
            Regex regex = new Regex(folderType + "\\((.*?)\\)");
            MatchCollection matches = regex.Matches(command);
            foreach (Match match in matches)
            {
                string fullExpr = match.Groups[0].ToString();
                string strDataSourceId = match.Groups[1].ToString();

                int dataSourceId = 0;
                if (int.TryParse(strDataSourceId, out dataSourceId))
                {
                    DataSource ds = new DataSource(dataSourceId, string.Empty);
                    if (ds.Keys == null)
                        throw new Exception(string.Format("Data source with id {0} referred in post event DOS commands was not defined!", dataSourceId));

                    string folder = string.Empty;
                    if (folderType == "_dira")
                        folder = DataSource.GetArchiveFolder(dataSourceId, ds.Keys);
                    else if (folderType == "_diro")
                        folder = DataSource.GetOutputFolder(dataSourceId, ds.Keys);
                    else if (folderType == "_dirp")
                        folder = DataSource.GetPullFolder(dataSourceId, ds.Keys);
                    else
                        throw new Exception(string.Format("'{0}' is not recognized as a valid SRE command!", command));

                    command = command.Replace(fullExpr, folder);
                }
            }

            return command;
        }

    }
}


