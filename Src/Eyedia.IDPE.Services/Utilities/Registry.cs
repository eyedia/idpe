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
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Text;
using System.Diagnostics;
using System.IO;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Text.RegularExpressions;
using Eyedia.Core.Data;
using System.Collections.Concurrent;
using System.Data;

namespace Eyedia.IDPE.Services
{

    /// <summary>
    /// Maintains job registry, expires job by removing it from memory. Job expire duration can be set in 'MemoryReset' in config
    /// </summary>
    internal class Registry
    {
        public static volatile Registry instance;
        private static object syncRoot = new Object();

        public GlobalEventsOnCompletes GlobalEventsOnCompletes { get; private set; }
        public Pullers Pullers;
        public FileSystemWatcher LocalFileWatcher;
        public Dictionary<int,SreFileSystemWatcher> DataSourceFileWatcher { get; internal set;}
        public List<CodeSet> CodeSets;
        public Dictionary<string, ZipFileInformation> ZipFiles;
        public List<FtpFileSystemWatcher> FtpWatchers;
        public Dictionary<string, string> EnvironmentVariables { get; internal set; }
        Hashtable _Entries;
        System.Timers.Timer _Timer;
        
        /// <summary>
        /// Common dictionary to store any object during the process
        /// </summary>
        public ConcurrentDictionary<string, object> CachedTables { get; internal set; }

        Registry()
        {
            DataSourceFileWatcher = new Dictionary<int, SreFileSystemWatcher>();
            ZipFiles = new Dictionary<string, ZipFileInformation>();            
            FtpWatchers = new List<FtpFileSystemWatcher>();
            EnvironmentVariables = new Dictionary<string, string>();
            //ExtensionMethods.TraceInformation("Starting singleton instance of Registry.");
            NumberOfCores = Eyedia.Core.Windows.Utilities.WindowsUtility.NumberOfCores;
            PhysicalProcessors = Eyedia.Core.Windows.Utilities.WindowsUtility.PhysicalProcessors;

            #if WEB
            if (EyediaCoreConfigurationSection.CurrentConfig.HostingEnvironment == EyediaCoreConfigurationSection.HostingEnvironments.Web)
                    WebTaskTimer.Start();
            #endif

            this._Entries = new Hashtable();
            this.CachedTables = new ConcurrentDictionary<string, object>();
            _Timer = new System.Timers.Timer(2 * (60 * 1000));    //2 minutes interval
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
            _Timer.Elapsed += new ElapsedEventHandler(Timer_Interval);

            //GetNumberOfInstances();
            RefreshGlobalEvents();
        }
    

        public static Registry Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Registry();                           
                        }
                    }
                }               
                return instance;
            }
         
        }

        public Hashtable Entries
        {
            get { return _Entries; }
        }

        public int NumberOfCores { get; private set; }
        public int PhysicalProcessors { get; private set; }

        //public void ClearLog()
        //{
        //    try
        //    {
        //        Trace.Close();
        //        if (File.Exists(SymplusCoreConfigurationSection.CurrentConfig.Trace.File))
        //        {
        //            File.Copy(SymplusCoreConfigurationSection.CurrentConfig.Trace.File, 
        //                GetBackupFileName(SymplusCoreConfigurationSection.CurrentConfig.Trace.File));
        //            File.Delete(SymplusCoreConfigurationSection.CurrentConfig.Trace.File);
        //            SetupTrace.SetupTraceListners(Information.EventLogSource, Information.EventLogName);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        string errMsg = "Could not clear IDPE event log, generally this get called from monitor page."
        //           + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);

        //        if (!EventLog.SourceExists(Information.EventLogSource))
        //            EventLog.CreateEventSource(Information.EventLogSource, Information.EventLogName);
        //        EventLog.WriteEntry(Information.EventLogSource, errMsg, EventLogEntryType.Error);
        //    }
        //}

        private string GetBackupFileName(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            string fn = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);

            string[] files = Directory.GetFiles(dir, fn + "*");
            int max = 0;
            foreach (string file in files)
            {
                Regex regex = new Regex(string.Format("{0}(.*){1}", fn, ext));
                var v = regex.Match(file);
                string s = v.Groups[1].ToString();
                int fileSequence = 0;
                if (!string.IsNullOrEmpty(s))
                {
                    fileSequence = (int)s.ParseInt();

                    if (fileSequence > max)
                        max = fileSequence;
                }
            }
            max = max + 1;
            return string.Format("{0}\\{1}{2}{3}", dir, fn, max.ToString("D2"), ext);
        }
        private static object _lock = new object();
        private void Timer_Interval(object source, ElapsedEventArgs e)
        {

            if (this.LocalFileWatcher == null)
            {
                ExtensionMethods.TraceInformation("Local file watcher died. Restarting it");
                new Idpe().StartPullers();
            }

            if ((this.Pullers != null)
                && (this.Pullers._LocalFileSystemWatcher != null))
                this.Pullers._LocalFileSystemWatcher.HandleExistingFiles();

            ExecuteFtpWatchers();
            if (this.LocalFileWatcher.EnableRaisingEvents == false)
                this.LocalFileWatcher.EnableRaisingEvents = true;

            ExtensionMethods.TraceInformation("T [{0}]", this.LocalFileWatcher.EnableRaisingEvents);
            Trace.Flush();
            MidnightSwiffer();
            if (_Entries.Count == 0)           
                return;
           
            
            List<string> removeEntries = new List<string>();
            foreach (string processId in this._Entries.Keys)
            {
                Job thisJob = this._Entries[processId] as Job;
                if (thisJob != null)
                {
                    if ((thisJob.IsFinished != true) && (thisJob.StartedAt.AddMinutes(IdpeConfigurationSection.CurrentConfig.TimeOut) < DateTime.Now))
                    {
                        ExtensionMethods.TraceInformation("Job '{0}' is timed out / expired, sending abort signal to the job. Time out was set to {1} minutes.", processId, IdpeConfigurationSection.CurrentConfig.TimeOut);
                        lock (_lock)
                        {
                            thisJob.AbortRequested = true;
                            thisJob.AbortReason = Job.AbortReasons.TimedOut;
                        }
                        //removeEntries.Add(processId);
                    }
                    else if ((thisJob.IsFinished == true) && (thisJob.KeepInMemoryUntilTimeOut))
                    {
                        ExtensionMethods.TraceInformation("Job '{0}' is finished, but waiting for caller to retreive result.", processId);
                    }
                }
            }
            
            if(removeEntries.Count > 0)
                ExtensionMethods.TraceInformation(string.Format("Removing {0} expired jobs from Registry.", removeEntries.Count));            
            
            lock (_lock)
            {
                foreach (string processId in removeEntries)
                {
                    ExtensionMethods.TraceInformation(string.Format("Removing '{0}'.", processId));
                    
                    Job theJob = this._Entries[processId] as Job;                    
                    this._Entries.Remove(processId);
                    theJob.Dispose();
                    theJob = null;
                }
            }

            GlobalEventsOnCompletes.Tick();
            Trace.Flush();

        }

        private void ExecuteFtpWatchers()
        {
            foreach (FtpFileSystemWatcher ftpWatcher in FtpWatchers)
            {
                ftpWatcher.Download();
            }
        }

        DateTime LastCleaned;
        private void MidnightSwiffer()
        {
            if (DateTime.Now.Hour > 1 && DateTime.Now.Hour < 3)
            {
                if (Math.Round(DateTime.Now.Subtract(LastCleaned).TotalDays, 0) > 0)
                {
                    ExtensionMethods.TraceInformation("Midnight Swiffer last run at '{0}', starting cleaning process...", LastCleaned);
                    Clean();
                }
            }
        }

        private void Clean()
        {            
            ExtensionMethods.TraceInformation("Clearing global cache.");
            Cache.Instance.Bag.Clear();
            ExtensionMethods.TraceInformation("Done.");
            

            ExtensionMethods.TraceInformation("Clearing temp directories.");
            Directory.Delete(Path.Combine(EyediaCoreConfigurationSection.CurrentConfig.TempDirectory, "idpe"), true);
            ExtensionMethods.TraceInformation("Done.");

            ExtensionMethods.TraceInformation("Clearing data cache...");
            if (Registry.Instance.CachedTables != null)
            {
                foreach (object obj in Registry.Instance.CachedTables)
                {
                    if (obj is DataTable)
                    {
                        DataTable cachedLookup = obj as DataTable;
                        cachedLookup.Dispose();
                    }
                }
            }
            ExtensionMethods.TraceInformation("Done.");

            ExtensionMethods.TraceInformation("Clearing tracked emails...");
            new Eyedia.Core.Net.EmailTracker().Clean();
            ExtensionMethods.TraceInformation("Done.");

            LastCleaned = DateTime.Now;
        }

        public void RefreshGlobalEvents()
        {           
            IdpeKey key = new Manager().GetKey(IdpeKeyTypes.GlobalEventsOnComplete);
            if (key != null)
                GlobalEventsOnCompletes = new GlobalEventsOnCompletes(key.Value);
            else
                GlobalEventsOnCompletes = new GlobalEventsOnCompletes(string.Empty);
            
            GlobalEventsOnCompletes.StartWatching();
        }

    }
}





