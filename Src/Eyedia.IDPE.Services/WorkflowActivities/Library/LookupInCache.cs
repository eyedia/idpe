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
using System.ComponentModel;
using Eyedia.Core;
using Eyedia.Core.Data;
using System.Data;
using System.Diagnostics;


namespace Eyedia.IDPE.Services
{
    [Designer(typeof(Eyedia.IDPE.Services.WorkflowActivities.LookupInCache))]
    public sealed class LookupInCache : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<string> CacheName { get; set; }
        public InArgument<string> Criteria { get; set; }
        public InArgument<string> AttributeName { get; set; }
        public InArgument<bool> IsSystemAttribute { get; set; }
        public InArgument<string> ReturnColumn { get; set; }        
        public InArgument<bool> ThrowErrorIfNullOrEmpty { get; set; }
        public OutArgument<string> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Job job = context.GetValue(this.Job);
            WorkerData data = context.GetValue(this.Data);

            string cacheName = context.GetValue(CacheName);
            string criteria = context.GetValue(Criteria);
            string attributeName = context.GetValue(AttributeName);
            bool isSystemAttribute = context.GetValue(IsSystemAttribute);
            string returnColumn = context.GetValue(ReturnColumn);
            bool throwErrorIfNullOrEmpty = context.GetValue(ThrowErrorIfNullOrEmpty);
            string theResult = string.Empty;
            object obj = Registry.Instance.CachedTables.GetValueOrDefault(cacheName, null);
            if (obj != null)
            {
                DataTable table = obj as DataTable;

                theResult = table.LookUp(criteria, returnColumn);
                new CodeActivityTraceWriter(job, data).WriteLine(string.Format("CLookup:'{0}, Return:'{1}' - '{2}'", criteria, returnColumn, theResult));

                context.SetValue(Result, theResult);

                if ((throwErrorIfNullOrEmpty) && ((!(string.IsNullOrEmpty(attributeName))) && (string.IsNullOrEmpty(theResult))))
                    data.CurrentRow.AddError(string.Format("Cache look up result was empty or null! Cache name = '{0}', Criteria = '{1}'",
                        cacheName, criteria), attributeName, isSystemAttribute);
            }
            else
            {
                job.TraceError(string.Format("The cache '{0}' was not initialized!", cacheName));
            }
        }
    }
}



