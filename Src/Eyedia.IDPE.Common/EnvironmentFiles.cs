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
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Eyedia.Core;
using System.Diagnostics;
using System.Configuration;

namespace Eyedia.IDPE.Common
{
    public class EnvironmentFiles
    {

        const string __stopFileName = "srestop.cmd";
        const string __startFileName = "srestart.cmd";
        const string __restartFileName = "srerestart.cmd";
        const string __setlogonAsFileName = "sresetlogonas.cmd";
        const string __defaultconfig = @"<Keys>
  <Key>
    <Name>instances</Name>
    <Value>1</Value>
  </Key>
</Keys>";

        public static string StartFileName { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, __startFileName); } }

        public static string StopFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, __stopFileName);
            }
        }

        public static string RestartFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, __restartFileName);
            }
        }

        public static string SetlogonAsFileName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, __setlogonAsFileName);
            }
        }

        public static void GenerateStartStopCommands(bool forceClean = false, int instances = 1)
        {
            

            if (forceClean)
            {
                if (File.Exists(StartFileName))
                    File.Delete(StartFileName);

                if (File.Exists(StopFileName))
                    File.Delete(StopFileName);  
                              
                if (File.Exists(RestartFileName))
                    File.Delete(RestartFileName);
            }
            GenerateStart(StartFileName, instances);
            GenerateStop(StopFileName, instances);
            GenerateRestart(RestartFileName, instances);            
        }

        public static Dictionary<string, string> ReadEnvironmentConfigs(string fileName = null)
        {            
            Dictionary<string, string> configs = new Dictionary<string, string>();
            if(fileName == null)
                 fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.EnvironmentVariablesFileName);
            if (File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                foreach(XmlNode node in doc.ChildNodes[0].ChildNodes)
                {
                    configs.Add(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText);
                }               
            }
            else
            {
                StreamWriter sw = new StreamWriter(fileName);
                sw.Write(__defaultconfig);
                sw.Close();
            }
            return configs;
        }
        public static void SaveEnvironmentConfigs(Dictionary<string, string> configs)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.EnvironmentVariablesFileName);
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(configs.Serialize());
            sw.Close();
        }

        public static void KillTestBedService()
        {
            ProcessStartInfo info = new ProcessStartInfo("cmd.exe", "/c taskkill /F /IM idpewoa.exe /T");
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(info);
        }

        #region Helpers
        private static void GenerateStop(string fileName, int instances = 1)
        {
            GenerateFile(fileName, "net stop", instances);
        }

        private static void GenerateStart(string fileName, int instances = 1)
        {
            GenerateFile(fileName, "net start", instances);
        }

        private static void GenerateRestart(string fileName, int instances = 1)
        {
            if (!File.Exists(fileName))
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine("call " + __stopFileName);
                    sw.WriteLine("call " + __startFileName);
                    sw.Close();
                }
            }
        }

        public static void GenerateSetSetLogonAs(string fileName, string userName, string password, int instances = 1)
        {
            GenerateFile(fileName, "sc config", instances, string.Format(" obj=\"{0}\" password=\"{1}\" type=\"own\""
                , userName, password));
        }

        public static string StartTestBed(bool restart = false)
        {
            if (restart)
                KillTestBedService();
            System.Threading.Thread.Sleep(1000);

            string srewoa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpewoa.exe");
            string srewoa_config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpewoa.exe.config");
            if (!File.Exists(srewoa_config))
            {
                string sre_config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe.config");
                if (File.Exists(sre_config))
                    File.Copy(sre_config, srewoa_config);
                else
                    return string.Empty;
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(srewoa);
            EyediaCoreConfigurationSection sccs = (EyediaCoreConfigurationSection)config.GetSection("eyediaCoreConfigurationSection");
            sccs.Trace.File = Path.Combine(Path.GetDirectoryName(sccs.Trace.File), "testbed" + Path.GetExtension(sccs.Trace.File));
            config.Save(ConfigurationSaveMode.Modified);

            //if trace file exists, place a divider
            try
            {
                if (File.Exists(sccs.Trace.File))
                {
                    StreamWriter sw = new StreamWriter(sccs.Trace.File);
                    sw.WriteLine(Environment.NewLine);
                    sw.WriteLine(Environment.NewLine);
                    sw.WriteLine("**********************************************************************");
                    sw.WriteLine("Test bed started at:" + DateTime.Now);
                    sw.WriteLine("**********************************************************************");
                    sw.WriteLine(Environment.NewLine);
                    sw.WriteLine(Environment.NewLine);
                    sw.Close();
                }
            }
            catch { }

            //start the service
            if (File.Exists(srewoa))
            {
                ProcessStartInfo info = new ProcessStartInfo(srewoa, "-c");
                info.WindowStyle = ProcessWindowStyle.Minimized;
                Process.Start(info);
                //Process.Start(srewoa, "-c");
                return sccs.Trace.File;
            }
            return string.Empty;
        }

        public static string ProcessTestFile(string pullFolder, string fileName)
        {
            //Eyedia.Core.Windows.Utilities.WindowsUtility.ExecuteBatchFile(EnvironmentFiles.StopFileName);
            string traceFileName = StartTestBed(true);
            if (!string.IsNullOrEmpty(traceFileName))
            {
                string toFileName = Path.Combine(pullFolder, Path.GetFileName(fileName));
                if (File.Exists(toFileName))
                    new FileUtility().Delete(toFileName);

                new FileUtility().FileCopy(fileName, toFileName, false);
                return traceFileName;
            }
            else
            {
                return string.Empty;
            }
        }

        private static void GenerateFile(string fileName, string prefixCommand, int instances = 1, string postFixCommand = null)
        {
            if (postFixCommand == null)
                postFixCommand = String.Empty;

            if (!File.Exists(fileName))
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine(prefixCommand + " \"IDPE\"" + postFixCommand);
                    if (instances > 1)
                    {
                        for (int i = 1; i < instances; i++)
                        {
                            sw.WriteLine(prefixCommand + " \"IDPE " + i + "\"" + postFixCommand);
                        }

                    }
                    sw.Close();
                }
            }
        }

        #endregion Helpers
    }
}


