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
using System.Windows.Forms;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using System.IO;
using System.Configuration;

namespace Eyedia.IDPE.Interface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

            if (!CheckSqlCe())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
               // Debugger.Launch();
                GetDummyUser();
                SetupTrace.SetupTraceListners(Information.EventLogSource, Information.EventLogName);
                EncryptConfigs();
                Application.Run(new MainWindow(args));
                //Application.Run(new Test());
                //GetDummyUser();
                //Application.Run(new SreEnvironmentWindow());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Trace.TraceError(ex.ToString());
                Trace.Flush();
            }
        }

        private static void GetDummyUser()
        {
            Information.LoggedInUser = new Core.Data.User();
            Information.LoggedInUser.FullName = "Deb'jyoti Das";
            Information.LoggedInUser.UserName = "DD";
            Information.LoggedInUser.IsDebugUser = true;
        }

        static void EncryptConfigs()
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Debug)
            {
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe"));
                    SectionInformation sreSection = config.GetSection("idpeConfigurationSection").SectionInformation;
                    if (sreSection == null) return;
                    string srecmdFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpec.exe");
                    if ((!sreSection.IsProtected)
                        && (File.Exists(srecmdFile)))
                        Process.Start(srecmdFile, "e");
                }
                catch { }
            }
        }

        static bool CheckSqlCe()
        {
            if (!Core.Data.DataExtensionMethods.IsSqlCeInstalled())
            {
                string msg = "SQL Server Compact 4.0 is not installed! " + Environment.NewLine;
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SSCERuntime_x64-ENU.exe")))
                {
                    msg += "SSCERuntime_x64-ENU.exe was provided with IDPE bundle. " + Environment.NewLine;
                    msg += "Please right click on that and 'Run as Administrator' to install SQL Server Compact 4.0";
                }
                else
                {
                    msg += "Download SQL Server Compact 4.0 from https://www.microsoft.com/en-us/download/details.aspx?id=17876 and install!";
                }

                MessageBox.Show(msg, "SQL Server Compact 4.0 Not Installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}





