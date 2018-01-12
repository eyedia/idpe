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
using System.Activities;
using System.Diagnostics;
using System.ComponentModel;

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(Eyedia.IDPE.Services.WorkflowActivities.AddOrUpdateProcessVariable))]
    public sealed class AddOrUpdateProcessVariable : CodeActivity
    {        
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<string> Key { get; set; }
        public InArgument<object> Value { get; set; }
        
        protected override void Execute(CodeActivityContext context)
        {
            //Even though concurrent dictionary is threadsafe, our threads are not executing synchronized
            //hence in this case a lock is needed           

            Job job = context.GetValue(this.Job);
            if (job == null)
            {
                WorkerData data = context.GetValue(this.Data);
                data.ThrowErrorIfNull(this.DisplayName);
                job = data.Job;
            }
            lock (job._lock)
            {
                
                string key = context.GetValue(this.Key);
                object value = context.GetValue(this.Value);
                job.ProcessVariables.AddOrUpdate(key, value, (keyx, oldValue) => value);
                if (value != null)
                {
                    string strValue = value.ToString();                   
                    new CodeActivityTraceWriter(job).WriteLine(string.Format("PV : Key='{0}', Value='{1}'", key, 
                        strValue.Length > 5 ? strValue.Substring(0, 5) + string.Format("...({0})", strValue.Length) : strValue));
                }
                else
                {

                }
            }
        }
    }
}



