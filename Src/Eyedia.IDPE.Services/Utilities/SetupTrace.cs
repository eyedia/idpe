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
using System.Diagnostics;
using Symplus.Core;
using Symplus.RuleEngine.Common;

namespace Symplus.RuleEngine.Services
{
    internal class SetupTrace
    {
        internal static void SetupTraceListners()
        {

            if (!Registry.Instance.CoreConfig.Trace.Enabled)
                return;

            //CustomTraceEnabled is enabled, lets try to instantiate it
            TextWriterTraceListenerWithTime traceListner = null;
            try
            {
                traceListner = new TextWriterTraceListenerWithTime(Registry.Instance.CoreConfig.Trace.File);
                traceListner.Filter = new EventTypeFilter(Registry.Instance.CoreConfig.Trace.Filter);
                traceListner.Attributes.Add("EmailErrors", Registry.Instance.CoreConfig.Trace.EmailError.ToString());
                traceListner.Attributes.Add("SMTPServer", Registry.Instance.CoreConfig.Email.SmtpServer);
                traceListner.Attributes.Add("FromEmailId", Registry.Instance.CoreConfig.Email.FromEmail);
                traceListner.Attributes.Add("ToEmailIds", Registry.Instance.CoreConfig.Email.ToEmails);

                Trace.Listeners.Add(traceListner);
                Trace.WriteLine(string.Format("Trying to initialize {0} type trace listner.", traceListner.GetType().ToString()));
                Trace.Flush();
                //if we are here, we are good, lets remove Default
                Trace.Listeners.Remove("Default");
            }
            catch (Exception ex)
            {
                string errMsg = "Could not instantiate TextWriterTraceListenerWithTime type listner. Check configuration, file path & permission."
                    + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);

                if (!EventLog.SourceExists(Information.EventLogSource))
                    EventLog.CreateEventSource(Information.EventLogSource, Information.EventLogName);
                EventLog.WriteEntry(Information.EventLogSource, errMsg, EventLogEntryType.Error);

            }
        }
    }
}





