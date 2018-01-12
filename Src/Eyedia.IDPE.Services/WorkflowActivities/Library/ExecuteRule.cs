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
using Eyedia.IDPE.Common;
using System.Diagnostics;
using System.IO;
using Eyedia.IDPE.DataManager;
using System.Activities.XamlIntegration;
using System.ComponentModel;

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(WorkflowActivities.ExecuteRule))]
    public sealed class ExecuteRule : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<int> RuleId { get; set; }
        public InArgument<string> RuleName { get; set; }
        public InArgument<Dictionary<string, object>> InArgs { get; set; }
        public OutArgument<Dictionary<string, object>> OutArgs { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Job job = context.GetValue(this.Job);
            WorkerData data = context.GetValue(this.Data);

            IdpeRule rule = null;
            int ruleId = context.GetValue(this.RuleId);
            string ruleName = context.GetValue(this.RuleName);

            if (ruleId > 0)
                rule = new Manager().GetRule(ruleId);
            else if(!string.IsNullOrEmpty(ruleName))
                rule = new Manager().GetRule(ruleName);

            if (rule != null)
            {
                Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rule.Xaml));
                DynamicActivity activity = ActivityXamlServices.Load(stream) as DynamicActivity;

                IDictionary<string, object> outArgs = null;
                Dictionary<string, object> inArgs = context.GetValue(this.InArgs);

                if (job != null)
                    inArgs.Add("Job", job);
                else
                    inArgs.Add("Data", data);

                WorkflowInvoker invoker = new WorkflowInvoker(activity);
                //outArgs = invoker.Invoke(inArgs);
                invoker.Invoke(inArgs);
                //context.SetValue(OutArgs, outArgs);
            }
            else
            {
                job.TraceError("Could not execute rule {0}, as it is not defined yet!", ruleName);
            }
        }
    }
}



