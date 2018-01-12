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
using Eyedia.IDPE.Common;
using System.Activities;
using System.IO;
using System.Activities.XamlIntegration;
using System.Diagnostics;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
    public class BusinessRule
    {
        /// <summary>
        /// Business rule will be initialied with xaml, priority and type
        /// </summary>
        /// <param name="ruleSetAsXaml"></param>
        /// <param name="priority"></param>
        /// <param name="ruleSetType"></param>
        public BusinessRule(string ruleSetAsXaml, int priority, RuleSetTypes ruleSetType)
        {            
            this.Priority = priority;
            this.RuleSetType = ruleSetType;
            Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ruleSetAsXaml));
            this.Activity = ActivityXamlServices.Load(stream) as DynamicActivity;

        }

        /// <summary>
        /// Business rule default constructor not in use
        /// </summary>
        public BusinessRule() { }

        /// <summary>
        /// Priority of the rule
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// The workflow activity will be initialized through constructor xaml
        /// </summary>
        public DynamicActivity Activity { get; private set; }

        /// <summary>
        /// Rule set type
        /// </summary>
        public RuleSetTypes RuleSetType { get; private set; }

        /// <summary>
        /// true when any exception occurred while executing the rule
        /// </summary>
        public bool ExceptionOccurred { get; internal set; }

        public IDictionary<string, object> Execute(Job job, WorkerData data)
        {

            IDictionary<string, object> outArgs = null;
            Dictionary<string, object> inArgs = new Dictionary<string, object>();
            string preFix = string.Empty;

            try
            {
                if ((this.RuleSetType == RuleSetTypes.SqlPullInit)
                    || (this.RuleSetType == RuleSetTypes.PreValidate)
                    || (this.RuleSetType == RuleSetTypes.PostValidate))
                {
                    job.ThrowErrorIfNull(this.Activity.DisplayName);
                    inArgs.Add("Job", job);
                    preFix = ExtensionMethods.GetTracePrefix(job);
                }
                else
                {
                    data.ThrowErrorIfNull(this.Activity.DisplayName);
                    inArgs.Add("Data", data);
                    preFix = ExtensionMethods.GetTracePrefix(data);
                }
                if (inArgs.Keys.Count == 0)
                    throw new ArgumentNullException(string.Format("{0}All Pre/PostValidate rules should have Job argument and all Row Preparing/Prepared/Validate should have Data as argument!",
                        preFix));


                if (this.Activity != null)
                {
                    if (string.IsNullOrEmpty(this.Activity.Name))
                    {
                        string datasourceName = string.Empty;
                        if (job != null)
                            datasourceName = job.DataSource.Name;
                        else if (data != null)
                            datasourceName = data.Job.DataSource.Name;

                        throw new Exception(string.Format("{0}Corrupt rule found while executing a '{1} type rule, associated with '{2}' datasource.",preFix , this.RuleSetType.ToString(), datasourceName));
                    }


                    string traceInfo = string.Format("{0}BRE - '{1}' - '{2}'", preFix, this.RuleSetType.ToString(), this.Activity.Name);
                    if (data != null)
                        data.CurrentRow.TraceLog.WriteLine(traceInfo);
                    else
                        ExtensionMethods.TraceInformation(traceInfo);  //pre/postvalidate

                    WorkflowInvoker invoker = new WorkflowInvoker(this.Activity);
                    outArgs = invoker.Invoke(inArgs);
                }

            }
            catch (BusinessException ex)
            {
                ExtensionMethods.TraceInformation(ex.Message);   //It is actually not an exception or error                            
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);
            }
            catch (Exception ex)
            {
                //There was some issue with the job and it was aborted/errored out, in that case we dont have log multiple errors
                if (!job.IsErrored)
                {
                    if (!job.AbortRequested)
                    {
                        job.IsErrored = true;
                        job.TraceError("{0}.Error. Job = '{1}', Rule Name = '{2}' Rule Type = '{3}'. {4}{5}",
                            preFix, job.JobIdentifier, this.Activity.Name, this.RuleSetType, Environment.NewLine, ex.ToString());
                        Trace.Flush();
                        throw ex;
                    }
                }
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
        /// true when any exception occurred while executing the rules
        /// </summary>
        public bool ExceptionOccurred { get; internal set; }


        /// <summary>
        /// Executes business rules based on ruleSetType. Executes all business rules if ruleSetType is not passed
        /// </summary>
        /// <param name="job">The job in context</param>
        /// <param name="data">The worker data in context</param>
        /// <param name="ruleSetType">The ruleSetType (Executes all business rules if ruleSetType is not passed)</param>
        public void Execute(Job job, WorkerData data, RuleSetTypes ruleSetType = RuleSetTypes.Unknown)
        {
            List<BusinessRule> businessRules = ruleSetType == RuleSetTypes.Unknown ? this : this.Where(bs => bs.RuleSetType == ruleSetType).OrderBy(bp => bp.Priority).ToList();

            foreach (BusinessRule businessRule in businessRules)
            {
                businessRule.Execute(job, data);
                //businessRule.ExceptionOccurred = true;
                //this.ExceptionOccurred = true;
            }
        }

    }
}




