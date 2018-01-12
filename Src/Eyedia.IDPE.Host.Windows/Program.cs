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
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using System.Configuration.Install;
using System.Diagnostics;
using Eyedia.IDPE.Common;
using System.IO;

namespace Eyedia.IDPE.Host.Windows
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            
            if (args.Length == 1 && args[0] == "INSTALLER")
            {                
                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, "-i"); 
                return 0;
            }
            else if (args.Length == 1 && args[0] == "UNINSTALLER")
            {
                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, "-u");
                return 0;
            }
            bool install = false, uninstall = false, console = false, rethrow = false;
            
            try
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-i":
                        case "-install":                            
                            install = true; break;
                        case "-u":
                        case "-uninstall":
                            uninstall = true; break;
                        case "-c":
                        case "-console":
                            console = true; break;
                        default:
                            Console.Error.WriteLine("Argument not expected: " + arg);
                            break;
                    }
                }

                if (install ||uninstall)
                {
                    Install(install);
                }                
                else if (console)
                {
                    try
                    {
                        Console.Title = "IDPE - Test Bed";
                        Console.WriteLine("Starting...");
                        IdpeService service = new IdpeService();
                        service.StartConsole(args);
                        Console.WriteLine("IDPE is running; press any key to stop");
                        Console.ReadKey(true);
                        service.StopConsole();
                        Console.WriteLine("Engine stopped");
                    }
                    catch(Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                        Services.Utility.WriteToTrace(ex.Message);
                        Services.Utility.WriteToTrace(Environment.NewLine);
                        Services.Utility.WriteToTrace("Test bed cannot continue!");
                        Console.ReadKey(true);
                    }
                }
                else if (!(install || uninstall))
                {
                    rethrow = true; // so that windows sees error...
                    ServiceBase[] services = { new IdpeService() };
                    ServiceBase.Run(services);
                    rethrow = false;
                }
                return 0;
            }
            catch (Exception ex)
            {
                if (rethrow) throw;
                Console.Error.WriteLine(ex.Message);
                return -1;
            }
        }

        static void Install(bool install)
        {
            if (Environment.UserInteractive)
            {                
                if (install)
                {
                    ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                    StartService(ProjectInstaller._ServiceName, 20000);
                }
                else
                {
                    StopService(ProjectInstaller._ServiceName, 12000);
                    ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                }
            }
        }
       
        static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry(Information.EventLogSource, ex.ToString(), EventLogEntryType.Error);
            }
        }

        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(Information.EventLogSource, ex.ToString(), EventLogEntryType.Error);
            }
        }





    }
}






