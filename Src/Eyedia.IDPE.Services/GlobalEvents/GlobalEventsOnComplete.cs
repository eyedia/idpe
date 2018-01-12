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
using Eyedia.Core;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    public class GlobalEventsOnComplete : GlobalEvents
    {
        const int __IntervalMinutes = 2;//should be equal to registry interval minutes
        //public GlobalEventsOnComplete(string rawString)
        //{            
        //    GlobalEventsOnComplete gec = ParseRawString(rawString);
        //    if (gec != null)
        //    {
        //        this.RawString = rawString;
        //        Name = gec.Name;
        //        this.DataSourceIds = gec.DataSourceIds;
        //    }
        //}

        public GlobalEventsOnComplete(List<int> dataSourceIds, string name, string dosCommands, int timerIntervalMinutes)
            : base(name, dosCommands, timerIntervalMinutes) 
        {
            DataSourceIds = dataSourceIds.Distinct().ToList();
        }
        public GlobalEventsOnComplete(List<int> dataSourceIds, string name, string dosCommands, int timerIntervalMinutes, int timeOut)
            : base(name, dosCommands, timerIntervalMinutes, timeOut) 
        {
            DataSourceIds = dataSourceIds.Distinct().ToList();
        }


        public string RawString
        {
            get
            {
                return string.Format("{0}|{1}|{2}|{3}",
                    Name, string.Join(",", DataSourceIds), TimeOutInMinutes, DosCommands);
            }

        }

        private List<int> DataSourceIds;
        public Dictionary<int, bool> DataSources { get; private set; }
        public bool Watching { get; private set; }

        protected override void OnStartWatching()
        {            
            DataSources = new Dictionary<int, bool>();
            foreach (int dataSourceId in DataSourceIds)
            {
                DataSources.Add(dataSourceId, false);
            }            
        }

        public string CommaSeparatedDataSourceIds
        {
            get
            {
                return string.Join(",", DataSourceIds.Select(n => n.ToString()).ToArray());
            }
            set
            {
                DataSourceIds = CommaToList(value);
            }
        }

        public bool HasEntry(int dataSourceId)
        {
            return DataSources.ContainsKey(dataSourceId);
        }

        protected override void OnComplete(int dataSourceId, PullersEventArgs e)
        {
            if (DataSources.ContainsKey(dataSourceId))
            {
                DataSources[dataSourceId] = true;
                ExtensionMethods.TraceInformation("GEC:Completed:{0}", dataSourceId);                
            }

            else
            {
                ExtensionMethods.TraceInformation("GEC:The data source {0} was not registered with global event '{1}'! This operation was ignored.", 
                    dataSourceId, Name);
            }

            CheckIfAllCompleted(e);
        }

        protected override void OnReset()
        {
            foreach (var item in DataSources)
            {
                DataSources[item.Key] = false;
            }
        }

        private void CheckIfAllCompleted(PullersEventArgs e)
        {            
            string yetToComplete = string.Empty;
            foreach (var item in DataSources)
            {
                if (item.Value == false)               
                    yetToComplete += item.Key + ",";                
            }

            if (!string.IsNullOrEmpty(yetToComplete))
            {
                yetToComplete = yetToComplete.Substring(0, yetToComplete.Length - 1);
                ExtensionMethods.TraceInformation("GEC:Yet to complete:{0}", yetToComplete);
            }
            else
            {
                ExtensionMethods.TraceInformation("GEC: all tasks completed!");
                InvokeAllCompleted(this, e);
            }
            Trace.Flush();
        }

        public static GlobalEventsOnComplete ParseRawString(string rawString)
        {
            string[] commands = rawString.Split("|".ToCharArray());
            if (commands.Length == 4)
            {
                int timeOut = 5;
                int.TryParse(commands[2], out timeOut);
                GlobalEventsOnComplete gec = new GlobalEventsOnComplete(CommaToList(commands[1]), commands[0], commands[3], __IntervalMinutes, timeOut);            
                return gec;
            }
            return null;
        }

        private static List<int> CommaToList(string strCommaSeparated)
        {
            try
            {                
                return strCommaSeparated.Split(',').Select(int.Parse).ToList();
            }
            catch
            {
                return new List<int>();
            }
        }
        
    }


}


