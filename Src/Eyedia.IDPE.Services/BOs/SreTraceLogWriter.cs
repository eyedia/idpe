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
using System.IO;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
    public class SreTraceLogWriter : IDisposable
    {
        
        public SreTraceLogWriter(int position)
        {
            if (EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                this.TraceLog = new StringWriter();

            this.Position = position;
        }

        /// <summary>
        /// Returns position if set through constructor. This is only for display purpose
        /// </summary>
        public int Position { get; private set; }

        private string Prefix
        {
            get { return string.Format("    {0:s} {1} ", DateTime.Now, this.Position); }
        }

        public void WriteLine(string value)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                return;

            this.TraceLog.WriteLine(Prefix + value);
        }

        public void WriteLine(string format, object arg0)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                return;

            this.TraceLog.WriteLine(Prefix + format, arg0);
        }

        public void WriteLine(string format, object arg0, object arg1)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                return;

            this.TraceLog.WriteLine(Prefix + format, arg0, arg1);
        }

        public void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                return;

            this.TraceLog.WriteLine(Prefix + format, arg0, arg1, arg2);
        }

        public void Write(string format, params object[] arg)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                return;

            this.TraceLog.Write(Prefix + format, arg);
        }

        public void Close()
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled)
                return;

            this.TraceLog.Close();
        }
        public StringWriter TraceLog { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.TraceLog != null)
            {                
                this.TraceLog.Close();
                this.TraceLog.Dispose();
            }
        }

        #endregion

        public override string ToString()
        {
            return EyediaCoreConfigurationSection.CurrentConfig.Trace.Enabled ?
                this.TraceLog.ToString() : base.ToString();
        }
    }
}



