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

namespace Eyedia.IDPE.Services
{
    public class GlobalEventsOnCompletes : List<GlobalEventsOnComplete>
    {       
        public GlobalEventsOnCompletes(string rawString)
        {
            RawString = rawString;           
            Init();
        }

        private void Init()
        {
            if (string.IsNullOrEmpty(RawString))
                return;

            ExtensionMethods.TraceInformation("GEC:Initializing:" + RawString);
            if (RawString.Contains("°"))
            {
                string[] strEvents = RawString.Split("°".ToCharArray());
                foreach (string strEvent in strEvents)
                {
                    CreateOneEvent(strEvent);
                }
            }
            else
            {
                CreateOneEvent(RawString);
            }
        }

        private void CreateOneEvent(string rawStringOneEvent)
        {
            ExtensionMethods.TraceInformation("    GEC:Initializing 1 GEC:" + rawStringOneEvent);
            if (rawStringOneEvent.Contains("|"))
            {
                //RawString = name|101,102,103|5|dos commands
                GlobalEventsOnComplete gec = GlobalEventsOnComplete.ParseRawString(rawStringOneEvent);
                if (gec != null)
                {                
                    gec.TimedOut += new GlobalEvents.GlobalEventsHandler(gec_TimedOut);
                    gec.AllCompleted += new GlobalEvents.GlobalEventsHandler(gec_AllCompleted);
                    this.Add(gec);
                }
            }

        }

        public void Complete(int dataSourceId, PullersEventArgs e)
        {
            foreach (var item in this)
            {
                if (item.HasEntry(dataSourceId))
                    item.Complete(dataSourceId, e);
            }
        }

        void gec_TimedOut(GlobalEventArgs e)
        {            
            e.GlobalEvent.Reset();
        }

        void gec_AllCompleted(GlobalEventArgs e)
        {
            ExtensionMethods.TraceInformation("GEC: '{0}' completed, executing commands!", e.GlobalEventName);
            e.GlobalEvent.ExecuteCommand(e.PullersEventArgs);
            ExtensionMethods.TraceInformation("GEC: commands executed for '{0}'.", e.GlobalEventName);
            e.GlobalEvent.Reset();
        }

        

        public string RawString { get; private set; }

        public GlobalEventsOnComplete this[string name]
        {
            get
            {
                GlobalEventsOnComplete gec = (from a in this
                                            where a.Name.ToLower().Equals(name.ToLower())
                                            select a).SingleOrDefault();

                if (gec == null)
                {
                    ExtensionMethods.TraceError("GEC: No gec found with name {0}.{1}{2}",
                        name, Environment.NewLine, new StackTrace().ToString());
                }

                return gec;
            }
        }      

        internal void StartWatching()
        {
            foreach (var item in this)
            {
                item.StartWatching();
            }
        }

        internal void Tick()
        {
            foreach (var item in this)
            {
                item.Tick();
            }
        }
   
    }
}


