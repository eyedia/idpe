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
using System.ComponentModel;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    public class GlobalEvents : IDisposable
    {
        public GlobalEvents() { }
        /// <summary>
        /// Gloval event handler to watch data source complete events
        /// </summary>
        /// <param name="name">Name of the global event</param>
        /// <param name="dosCommands">DOS commands to be executed on completion</param>
        /// <param name="timerIntervalMinutes">The timer interval in minutes</param>        
        public GlobalEvents(string name, string dosCommands, int timerIntervalMinutes)
        {
            this.Name = name;
            this.DosCommands = dosCommands;
            if (timerIntervalMinutes < 1)
                timerIntervalMinutes = 1;
            this.TimeOut = (5 * 60) / timerIntervalMinutes;
        }

        /// <summary>
        /// Gloval event handler to watch data source complete events
        /// </summary>
        /// <param name="name">Name of the global event</param>
        /// <param name="dosCommands">DOS commands to be executed on completion</param>
        /// <param name="timerIntervalMinutes">The timer interval in minutes</param>
        /// <param name="timeOut">Timeout in minutes, if not passed, default 5 minutes will be set</param>        
        public GlobalEvents(string name, string dosCommands, int timerIntervalMinutes, int timeOut)
        {
            this.Name = name;
            this.DosCommands = dosCommands;
            if (timerIntervalMinutes < 1)
                timerIntervalMinutes = 1;
            this.TimeOutInMinutes = timeOut;
            this.TimeOut = (TimeOutInMinutes * 60) / timerIntervalMinutes;   
        }

        [DefaultValue(5)]
        public int TimeOutInMinutes { get; set; }
        public delegate void GlobalEventsHandler(GlobalEventArgs e);
        public event GlobalEventsHandler AllCompleted;
        public event GlobalEventsHandler TimedOut;
        public string Name { get; set; }
        public string DosCommands { get; set; }
        public int TimeOut { get; private set; }
        public bool HasTimedOut { get; private set; }
        public DateTime StartedAt { get; private set; }
        public int TimeElapsed { get; private set; }

        public void Tick()
        {
            TimeElapsed++;
            //Trace.WriteLine(TimeElapsed);
            if (TimeElapsed >= TimeOut)
            {                
                HasTimedOut = true;
                InvokeTimedOut();
            }
        }
        private void CheckIfTimedOut()
        {
            if (HasTimedOut)
                throw new Exception(string.Format("The global event '{0}' was timed out!", Name));
        }
        protected void InvokeAllCompleted(GlobalEvents globalEvent, PullersEventArgs e)
        {           
           
            if (AllCompleted != null)
            {
                AllCompleted(new GlobalEventArgs(Name, globalEvent, e));
            }
        }

        protected void InvokeTimedOut()
        {
            if (TimedOut != null)
            {
                TimedOut(new GlobalEventArgs(Name, this));
            }
        }

        public void StartWatching()
        {            
            StartedAt = DateTime.Now;            
            OnStartWatching();
        }       
        protected virtual void OnStartWatching()
        {
            
        }
        public void Complete(int dataSourceId, PullersEventArgs e)
        {
            CheckIfTimedOut();
            OnComplete(dataSourceId, e);
        }

        protected virtual void OnComplete(int dataSourceId, PullersEventArgs e)
        {
        }

        public void Reset()
        {           
            HasTimedOut = false;
            TimeElapsed = 0;
            StartWatching();
            Trace.WriteLine("GEC:Reset called for '{0}'", Name);
        }
        protected virtual void OnReset()
        {
        }

        public void ExecuteCommand(PullersEventArgs e)
        {
            PusherDosCommands pdc = new PusherDosCommands();
            pdc.Process(e, DosCommands);
            OnExecuteCommand(e);
            Trace.WriteLine("GEC:Commands executed '{0}'", Name);
        }

        protected virtual void OnExecuteCommand(PullersEventArgs e)
        {

        }    

        public void Dispose()
        {            
            if (AllCompleted != null)
                AllCompleted = null;
        }
    }

    public class GlobalEventArgs : EventArgs
    {
        public GlobalEventArgs(string globalEventName, GlobalEvents globalEvent, PullersEventArgs pullersEventArgs = null)
        {
            this.GlobalEventName = globalEventName;
            this.GlobalEvent = globalEvent;
            this.PullersEventArgs = pullersEventArgs;
        }

        public PullersEventArgs PullersEventArgs { get; private set; }
        public string GlobalEventName { get; private set; }
        public GlobalEvents GlobalEvent { get; private set; }
    }
}


