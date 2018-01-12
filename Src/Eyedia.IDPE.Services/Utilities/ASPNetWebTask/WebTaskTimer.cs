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




#if WEB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{    
    public static class WebTaskTimer
    {
        private static readonly Timer _timer = new Timer(OnTimerElapsed);
        private static readonly WebTaskHost _WebTaskHost = new WebTaskHost();

        public static void Start()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(5 * 60 * 1000));
        }

        private static void OnTimerElapsed(object sender)
        {
            _WebTaskHost.DoWork(() =>
            {
                ExtensionMethods.TraceInformation("WebTaskTimer.Start");                
                if (Registry.Instance.Pullers == null)
                {
                    ExtensionMethods.TraceInformation("Pullers died, initiating again.");
                    Registry.Instance.Pullers = new Pullers();
                    Registry.Instance.Pullers.Start();
                }
                else if (Registry.Instance.Pullers.LocalFileSystemWatcherIsRunning == false)
                {
                    ExtensionMethods.TraceInformation("Pullers.LocalFileSystemWatcher stopped running. Starting it again.");
                    Registry.Instance.Pullers.Start();
                }                
                ExtensionMethods.TraceInformation("WebTaskTimer.End");
            });
        }
    }
}
#endif





