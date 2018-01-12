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

using System.Diagnostics;
using System.ServiceProcess;
using System.ServiceModel;
using Eyedia.IDPE.Services;


namespace Eyedia.IDPE.Host.Windows
{
    public partial class IdpeService : ServiceBase
    {
        internal static ServiceHost _ServiceHostSre = null;
        internal static ServiceHost _ServiceHostSrees = null;

        public IdpeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (_ServiceHostSre != null)
                _ServiceHostSre.Close();
            _ServiceHostSre = new ServiceHost(typeof(Idpe));
            new Idpe().StartPullers();
            _ServiceHostSre.Open();


            if (_ServiceHostSrees != null)
                _ServiceHostSrees.Close();
            _ServiceHostSrees = new ServiceHost(typeof(IdpeEnvironmentService));
            _ServiceHostSrees.Open();


            Trace.TraceInformation("idpe:Service started. Reading configurations...");

        }

        protected override void OnStop()
        {
            if (_ServiceHostSre != null)
            {                
                _ServiceHostSre.Close();
                _ServiceHostSre = null;                
            }

            if (_ServiceHostSrees != null)
            {
                _ServiceHostSrees.Close();
                _ServiceHostSrees = null;
            }

            Trace.TraceInformation("idpe:Service stopped.");

        }

        public void StartConsole(string[] args)
        {
            OnStart(args);
        }

        public void StopConsole()
        {
            OnStop();
        }
        
    }
}






