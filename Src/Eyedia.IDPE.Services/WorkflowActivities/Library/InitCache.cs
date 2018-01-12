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
using System.Data;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(Eyedia.IDPE.Services.WorkflowActivities.InitCache))]
    public sealed class InitCache : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<string> Name { get; set; }
        public InArgument<string> ConnectionStringKeyName { get; set; }
        public InArgument<string> Query { get; set; }
        public InArgument<bool> ThrowErrorIfNullOrEmpty { get; set; }
     

        protected override void Execute(CodeActivityContext context)
        {
            Job job = context.GetValue(this.Job);
            string name = context.GetValue(this.Name);
            string connectionStringKeyName = context.GetValue(this.ConnectionStringKeyName);
            string query = context.GetValue(this.Query);
            bool throwErrorIfNullOrEmpty = context.GetValue(ThrowErrorIfNullOrEmpty);
            DataTable table = null;
            new CodeActivityTraceWriter(job).WriteLine(string.Format("Initializing cache '{0}'...", name));
            Trace.Flush();
            if (job != null)
            {
                table = job.DataSource.LoadDataTable(connectionStringKeyName, query);
                //job.ProcessVariables.AddOrUpdate(name, table, (keyx, oldValue) => table);
            }
            else
            {
                WorkerData data = context.GetValue(this.Data);
                table = data.Job.DataSource.LoadDataTable(connectionStringKeyName, query);
                //data.Job.ProcessVariables.AddOrUpdate(name, table, (keyx, oldValue) => table);
            }

            new CodeActivityTraceWriter(job).WriteLine(string.Format("Cache initialized with connection string name:'{0}', query:'{1}';{2} records.", 
                connectionStringKeyName, query, table.Rows.Count));
            
            //keeping in global instance
            Registry.Instance.CachedTables.AddOrUpdate(name, table, (keyx, oldValue) => table);

            if ((throwErrorIfNullOrEmpty) && (table.Rows.Count == 0))
                job.AddContainerError(-1, string.Format("Init cache the query result was empty or null! Connection = '{0}', Query = '{1}'",
                    connectionStringKeyName, query));
            
        }
    }
}



