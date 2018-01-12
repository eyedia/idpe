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
using Symplus.RuleEngine.Common;
using System.Activities;
using System.IO;
using System.Activities.XamlIntegration;
using System.Diagnostics;
using Symplus.Core;

namespace Symplus.RuleEngine.Services
{
    public class BusinessRule
    {
        public BusinessRule(string ruleSetAsXaml, int priority, RuleSetTypes ruleSetType)
        {
            this.Priority = priority;
            this.RuleSetType = ruleSetType;
            Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ruleSetAsXaml));
            this.Activity = ActivityXamlServices.Load(stream) as DynamicActivity;

        }
        public BusinessRule() { }

        public int Priority { get; private set; }
        public DynamicActivity Activity { get; private set; }
        public RuleSetTypes RuleSetType { get; private set; }

        public IDictionary<string, object> Execute(Job job, WorkerData data)
        {
           
            IDictionary<string, object> outArgs = null;
            Dictionary<string, object> inArgs = new Dictionary<string, object>();
            try
            {
                
                if ((this.RuleSetType == RuleSetTypes.PreValidate)
                    || (this.RuleSetType == RuleSetTypes.PostValidate))
                {
                    job.ThrowErrorIfNull(this.Activity.DisplayName);
                    inArgs.Add("Job", job);
                }
                else
                {
                    data.ThrowErrorIfNull(this.Activity.DisplayName);
                    inArgs.Add("Data", data);
                }
             
                if (this.Activity != null)
                {
                    Trace.TraceInformation("Executing business a '{0}' type rule '{1}'", this.RuleSetType.ToString(), this.Activity.Name);
                    WorkflowInvoker invoker = new WorkflowInvoker(this.Activity);
                    outArgs = invoker.Invoke(inArgs);
                    //outArgs = WorkflowInvoker.Invoke(this.Activity, inArgs);
                }
                
            }
            catch (BusinessException ex)
            {
                Trace.TraceInformation(ex.Message);   //It is actually not an exception or error                
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);                
            }
            catch(Exception ex)
            {                
                Trace.TraceError("There was an error occurred while executing business rule! Job = '{0}', Rule Name = '{1}' Rule Type = '{2}'. {3}{4}",
                    job.JobIdentifier, this.Activity.Name, this.RuleSetType, Environment.NewLine, ex.ToString());
                Trace.Flush();
                throw ex;
            }

            return outArgs;
        }
    }

    public class BusinessRules : List<BusinessRule>
    {
        public BusinessRules() { }
        public BusinessRules(List<BusinessRule> businessRules)
            : base(businessRules)
        {
        }

        /// <summary>
        /// Executes business rules based on ruleSetType. Executes all business rules if ruleSetType is not passed
        /// </summary>
        /// <param name="job">The job in context</param>
        /// <param name="data">The worker data in context</param>
        /// <param name="ruleSetType">The ruleSetType (Executes all business rules if ruleSetType is not passed)</param>
        public void Execute(Job job, WorkerData data, RuleSetTypes ruleSetType = RuleSetTypes.Unknown)
        {
            List<BusinessRule> businessRules = ruleSetType == RuleSetTypes.Unknown ? this : this.Where(bs => bs.RuleSetType == ruleSetType).ToList();

            foreach (BusinessRule businessRule in businessRules)
            {
                businessRule.Execute(job, data);
            }
        }

    }    
}




