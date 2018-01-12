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
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Eyedia.Core.Encryption;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Command
{
    class Program
    {        
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("usage: srecmd e/d/t [datasource id]");
                Environment.Exit(101);
            }

            try
            {
                switch (args[0].ToLower())
                {
                    case "e":
                        Encrypt();
                        break;

                    case "d":
                        Decrypt();
                        break;

                    case "u":
                        UpgradeToIdpe();
                        break;

                    default:
                        Console.WriteLine("usage: srecmd e/d/t");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(103);
            }
        }       


        static void AllowDecryption()
        {
            try
            {
                string aspnet_regiis = string.Format("{0}aspnet_regiis.exe", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
                ProcessStartInfo procStartInfo = new ProcessStartInfo(aspnet_regiis, "-pa \"NetFrameworkConfigurationKey\" \"NT AUTHORITY\\NETWORK SERVICE\"");
                procStartInfo.CreateNoWindow = true;
                procStartInfo.UseShellExecute = false;
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                bool result = proc.Start();
            }
            catch { }
        }

        private static void Encrypt()
        {
            Console.Write("Encrypting...");
            ConfigEncryptor.CreateKey(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Eyedia.xml"));
            Console.Write(".");
            ConfigEncryptor.EncryptAppConfigSections(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe"), "appSettings,connectionStrings,eyediaCoreConfigurationSection,sreConfigurationSection");
            Console.Write(".");
            ConfigEncryptor.EncryptAppConfigSections(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idped.exe"), "appSettings,connectionStrings,eyediaCoreConfigurationSection");
            Console.Write(".");
            AllowDecryption();
            Console.Write(".");
            Console.WriteLine("Done!");
        }
        
        private static void Decrypt()
        {            
            Console.Write("Decrypting...");
            ConfigEncryptor.DecryptAppConfigSections(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe"), "appSettings,connectionStrings,eyediaCoreConfigurationSection,sreConfigurationSection");
            Console.Write(".");
            ConfigEncryptor.DecryptAppConfigSections(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idped.exe"), "appSettings,connectionStrings,eyediaCoreConfigurationSection");
            Console.Write(".");            
            Console.WriteLine("Done!");
        }

        private static void UpgradeToIdpe()
        {
            Manager manager = new Manager();
            List<IdpeRule> rules = manager.GetRules();

            foreach(IdpeRule rule in rules)
            {
                rule.Xaml = rule.Xaml.Replace("Symplus.RuleEngine.Services", "Eyedia.IDPE.Services");
                manager.Save(rule);
            }
        }

    }

    #region Commented Code
    /*
    class Program
    {
        const string securityCode = "syne$x";
        static void Main(string[] args)
        {
            
            string allargs = string.Empty;
            foreach (string a in args)
            {
                allargs += a + ",";
            }
            //EventLog.WriteEntry(Information.EventLogSource, allargs);
            if (args.Length < 1)
            {
                Console.WriteLine("usage: sreutilcmd securitycode [command [params]]");
                Environment.Exit(101);
            }

            if (args[0] != securityCode)
            {
                Console.WriteLine("usage: sreutilcmd securitycode [command [params]]");
                Console.WriteLine("Invalid security code.");
                Environment.Exit(102);
            }
           
            try
            {
                switch (args[1].ToLower())
                {
                    case "install":
                        Install(args[2].Split("|".ToCharArray())[0], args[2].Split("|".ToCharArray())[1]);
                        break;

                    case "uninstall":
                        UnInstall();
                        break;

                    case "renmsi":
                        RenameMSI.Rename(args[2], args[3] == "Debug" ? "d" : "r");
                        break;

                    case "creatersakey":
                        ConfigEncryptor.CreateKey(args[2]);
                        break;

                    case "encrypt":
                        ConfigEncryptor.EncryptAppConfigSections(args[2], args[3]);
                        break;

                    case "decrypt":
                        ConfigEncryptor.DecryptAppConfigSections(args[2], args[3]);
                        break;

                    case "replace":
                        ReplaceInFile(args[2], args[3], args[4]);
                        break;

                    case "setupwatchfolder":
                        SetupWatchFolder(args[2]);
                        break;

                    case "allowdecryption":
                        AllowDecryption();
                        break;

                    case "copyright":
                        WriteCopyRightNotice(args);
                        break;

                    case "sqlce40":
                        if (args.Length == 3)
                            SqlCe.Ensure40(args[2]);
                        else
                            SqlCe.Ensure40(args[2], args[3]);
                        break;

                    case "sqlceddl":
                        if (args.Length == 4)
                            SqlCe.ExecuteDDL(args[2], args[3]);
                        else if (args.Length == 5)
                            SqlCe.ExecuteDDL(args[2], args[3], args[4]);
                        break;

                    case "sreclientconfig":
                        SreClientConfigExtracter.ExtractConfig(args[2]);
                        break;

                    default:
                        Console.WriteLine("usage: sreutilcmd securitycode renmsi <MSI File> <Build Mode (d/r)>");
                        Console.WriteLine("usage: sreutilcmd securitycode genmsiconfig <SRE Config File> <Config Name>");
                        Console.WriteLine("usage: sreutilcmd securitycode encrypt <EXE File> <appSettings,connectionStrings,...>");
                        Console.WriteLine("usage: sreutilcmd securitycode copyright <disclaimer loc> <source loc>");
                        break;

                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(Information.EventLogSource, ex.ToString());               
                Console.WriteLine(ex.Message);
                Environment.Exit(103);
            }           
        }      
        
        private static void GetAssemblyVersion(string[] args)
        {
            Version version = null, previousverion = null;
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            if (args.Length == 3)
                dir = args[2];
            string outFileName = Path.Combine(dir, "srever.txt");
            string outFileNamePrev = Path.Combine(dir, "srever.prev.txt");
            if (File.Exists(outFileName))
                File.Delete(outFileName);

            if (File.Exists(outFileNamePrev))
                File.Delete(outFileNamePrev);

            List<string> ourAssemblies = new List<string>();
            ourAssemblies.AddRange(Directory.GetFiles(dir, "Eyedia.IDPE.*.dll"));
            ourAssemblies.Add(Path.Combine(dir, "idped.exe"));
            ourAssemblies.Add(Path.Combine(dir, "idpe.exe"));

            foreach (string ourAssembly in ourAssemblies)
            {
                Version v = Assembly.LoadFile(ourAssembly).GetName().Version;
                if (ourAssembly.Contains("configurator.exe"))
                    Debugger.Break();

                if (version == null)
                    version = v;
                if (previousverion == null)
                    previousverion = v;
                if (v > previousverion)
                    version = v;
            }
          
            StreamWriter sw = new StreamWriter(outFileName);
            sw.Write(version.ToString());
            sw.Close();

            sw = new StreamWriter(outFileNamePrev);
            sw.Write(previousverion.ToString());
            sw.Close();
        }

        private static void WriteCopyRightNotice(string[] args)
        {
            DisclaimerWriter writer = new DisclaimerWriter(args[3]);
            writer.Write();           
        }

        static void AllowDecryption()
        {
            try
            {
                string aspnet_regiis = string.Format("{0}aspnet_regiis.exe", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
                ProcessStartInfo procStartInfo = new ProcessStartInfo(aspnet_regiis, "-pa \"NetFrameworkConfigurationKey\" \"NT AUTHORITY\\NETWORK SERVICE\"");
                procStartInfo.CreateNoWindow = true;
                procStartInfo.UseShellExecute = false;
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                bool result = proc.Start(); 
            }
            catch { }
        }

        private static void Install(string location, string watchFolder)
        {
            Console.Write("Configuring. Please wait.");
            ConfigEncryptor.CreateKey(Path.Combine(location, "Eyedia.xml"));
            Console.Write(".");
            SetupWatchFolder(watchFolder);
            Console.Write(".");
            //ConfigEncryptor.EncryptAppConfigSections(Path.Combine(location, "idpe.exe"), "appSettings,connectionStrings,eyediaCoreConfigurationSection,sreConfigurationSection");
            Console.Write(".");
            //ConfigEncryptor.EncryptAppConfigSections(Path.Combine(location, "idped.exe"), "appSettings,connectionStrings,eyediaCoreConfigurationSection");
            Console.Write(".");
            AllowDecryption();
            Console.Write(".");
            Console.WriteLine("Done!");
        }

        static void UnInstall()
        {            
            
            try
            {                
                RegistryUtility regUtil = new RegistryUtility();
                string baseKey = regUtil.SubKey;
                int totalInstances = regUtil.SubKeyCount();                
                for (int i = 0; i < totalInstances; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        try
                        {

                            InstanceSettings instance = new InstanceSettings(j);
                            instance.ReadFromRegistry(regUtil.SubKey);
                            ServiceInstaller.StopService(instance.ServiceName, true);
                            ServiceInstaller.Uninstall(instance.ServiceName);
                            instance.DeleteFromRegistry(regUtil.SubKey);
                            File.Delete(string.Format("{0}sre.{1}.exe", AppDomain.CurrentDomain.BaseDirectory, instance.InstanceNumber.ToString("00")));
                            File.Delete(string.Format("{0}sre.{1}.exe.config",AppDomain.CurrentDomain.BaseDirectory, instance.InstanceNumber.ToString("00")));
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "sre.ini");
                            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "Logs", true);
                        }
                        catch { }
                    }
                    
                }

            }
            catch{}

            try
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "sre.ini");
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "Logs", true);
                for (int i = 1; i < 5; i++)
                {
                    string file1 = string.Format("{0}sre.{1}.exe", AppDomain.CurrentDomain.BaseDirectory, i.ToString("00"));
                    string file2 = string.Format("{0}sre.{1}.exe.config", AppDomain.CurrentDomain.BaseDirectory, i.ToString("00"));
                    if (File.Exists(file1))
                        File.Delete(file1);
                    if (File.Exists(file2))
                        File.Delete(file2);
                }
            }
            catch { }
           
        }       

        static void SetupWatchFolder(string rootFolder)
        {

            WindowsUtility.SetFolderPermission(rootFolder);

            Directory.CreateDirectory(Path.Combine(rootFolder, "InBound\\Pull"));
            Directory.CreateDirectory(Path.Combine(rootFolder, "InBound\\Archive"));
            Directory.CreateDirectory(Path.Combine(rootFolder, "OutBound\\Output"));
        }

        static void ReplaceInFile(string fileName, string toFind, string toReplace)
        {
            StreamReader sr = new StreamReader(fileName);
            string fileContent = sr.ReadToEnd();
            sr.Close();

            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(fileContent.Replace(toFind, toReplace));
            sw.Close();
        }
    }
     */
    #endregion Commented Code
}




