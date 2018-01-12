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
using System.IO;

namespace Eyedia.Core
{
    public class SetupTrace
    {
        public static void SetupTraceListners(string eventLogSource, string eventLogName)
        {

            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                return;

            TextWriterTraceListenerWithTime traceListner = null;
            try
            {
                FileStreamWithBackup fileStreamWithBackup = new FileStreamWithBackup(EyediaCoreConfigurationSection.CurrentConfig.Trace.File, 5 * 1024 * 1024, EyediaCoreConfigurationSection.CurrentConfig.Trace.MaxFileCount, FileMode.Append);
                traceListner = new TextWriterTraceListenerWithTime(EyediaCoreConfigurationSection.CurrentConfig.Trace.File, EyediaCoreConfigurationSection.CurrentConfig.Trace.MaxFileCount, fileStreamWithBackup);
                traceListner.Filter = new EventTypeFilter(EyediaCoreConfigurationSection.CurrentConfig.Trace.Filter);
                traceListner.Attributes.Add("EmailErrors", EyediaCoreConfigurationSection.CurrentConfig.Trace.EmailError.ToString());
                traceListner.Attributes.Add("SMTPServer", EyediaCoreConfigurationSection.CurrentConfig.Email.SmtpServer);
                traceListner.Attributes.Add("FromEmailId", EyediaCoreConfigurationSection.CurrentConfig.Email.FromEmail);
                traceListner.Attributes.Add("ToEmailIds", EyediaCoreConfigurationSection.CurrentConfig.Email.ToEmails);

                Trace.Listeners.Clear();
                Trace.Listeners.Add(traceListner);
                Trace.Listeners.Remove("Default");
            }
            catch (Exception ex)
            {
                string errMsg = "Could not instantiate TextWriterTraceListenerWithTime type listner. Check configuration, file path & permission."
                    + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);

                if (!EventLog.SourceExists(eventLogSource))
                    EventLog.CreateEventSource(eventLogSource, eventLogName);

                EventLog.WriteEntry(eventLogSource, errMsg, EventLogEntryType.Error);
            }
        }

        public static void Clear(string eventLogSource, string eventLogName)
        {
            try
            {                
                if (Trace.Listeners[0] is TextWriterTraceListenerWithTime)
                {
                    TextWriterTraceListenerWithTime listener = Trace.Listeners[0] as TextWriterTraceListenerWithTime;
                    listener.FileStream.BackupAndResetStream();
                }
            }
            catch (Exception ex)
            {
                string errMsg = "Could not instantiate TextWriterTraceListenerWithTime type listner. Check configuration, file path & permission."
                    + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);

                if (!EventLog.SourceExists(eventLogSource))
                    EventLog.CreateEventSource(eventLogSource, eventLogName);
                EventLog.WriteEntry(eventLogSource, errMsg, EventLogEntryType.Error);

            }
        }
    }
}





