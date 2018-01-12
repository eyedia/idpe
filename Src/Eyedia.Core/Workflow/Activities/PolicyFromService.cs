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
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Symplus.Core;

namespace Symplus.Core.Workflow
{
    [ToolboxBitmapAttribute(typeof(PolicyActivity), "Resources.Rule.png")]
    [ToolboxItemAttribute(typeof(ActivityToolboxItem))]
    public partial class PolicyFromService : System.Workflow.ComponentModel.Activity
    {        
        public PolicyFromService()
        {
            InitializeComponent();            

        }
        

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.RaiseEvent(PolicyFromService.BeforePolicyAppliedEvent, this, new EventArgs());
            ExternalRuleSetService ruleSetService = context.GetService<ExternalRuleSetService>();

            if (ruleSetService != null)
            {
                RuleSet ruleSet = Cache.Instance.Bag[this.RuleSetName + this.MajorVersion + this.MinorVersion] as RuleSet;
                if (ruleSet == null)
                {
                    ruleSet = ruleSetService.GetRuleSet(new RuleSetInfo(this.RuleSetName, this.MajorVersion, this.MinorVersion));
                    Cache.Instance.Bag.Add(this.RuleSetName + this.MajorVersion + this.MinorVersion, ruleSet);
                }

                if (ruleSet != null)
                {
                    Activity targetActivity = this.GetRootWorkflow(this.Parent);
                    RuleValidation validation = new RuleValidation(targetActivity.GetType(), null);
                    RuleExecution execution = new RuleExecution(validation, targetActivity, context);

                    try
                    { 
                        Dictionary<string, object> param = new Dictionary<string, object>();
                        param.Add("rulesetname", string.Format("{0}.{1}.{2}", ruleSet.Name, this.MajorVersion, this.MinorVersion));                        
                        ruleSet.Execute(execution);
                        
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.InnerException);
                        if (!Information.IsSilientBusinessRuleError)
                            throw new Exception(ex.Message, ex.InnerException);
                        string mapId = Guid.NewGuid().ToString();
                        this.ErrorMessage = string.Format("A critical error occurred while executing rule {0}. MapId:{1}", ruleSet.Name, mapId);
                        PrintRuleSetTraceInformation(mapId, ruleSet, validation);
                    }

                    base.RaiseEvent(PolicyFromService.AfterPolicyAppliedEvent, this, EventArgs.Empty);
                }
            }
            else
            {
                throw new InvalidOperationException("A RuleSetService must be configured on the host to use the PolicyFromService activity.");
            }

            return ActivityExecutionStatus.Closed;
        }

        private CompositeActivity GetRootWorkflow(CompositeActivity activity)
        {
            if (activity.Parent != null)
            {
                CompositeActivity workflow = GetRootWorkflow(activity.Parent);
                return workflow;
            }
            else
            {
                return activity;
            }
        }

        private void PrintRuleSetTraceInformation(string mapId, RuleSet ruleSet, RuleValidation validation)
        {
            Trace.Indent();
            Trace.TraceInformation("Failed Ruleset name:{0}, MapId:{1}", ruleSet.Name, mapId);
            Trace.Indent();
            foreach (Rule rule in ruleSet.Rules)
            {
                Trace.TraceInformation("rule name:{0}, IsActive:{1}", rule.Name, rule.Active);
                Trace.TraceInformation("Condition:{0}", rule.Condition.ToString());
                Trace.TraceInformation("Then");
                Trace.Indent();
                foreach (RuleAction ra in rule.ThenActions)
                {
                    foreach (string sa in ra.GetSideEffects(validation))
                        Trace.TraceInformation("Side effect:{0}", sa);
                }
                Trace.Unindent();

                Trace.TraceInformation("Else");
                Trace.Indent();
                foreach (RuleAction ra in rule.ElseActions)
                {
                    foreach (string sa in ra.GetSideEffects(validation))
                        Trace.TraceInformation("Side effect:{0}", sa);
                }
                Trace.Unindent();

            }
            Trace.Unindent();
            Trace.Unindent();
        }

        #region Dependency properties

        public static DependencyProperty RuleSetNameProperty = DependencyProperty.Register("RuleSetName", typeof(System.String), typeof(PolicyFromService));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Required)]
        [BrowsableAttribute(true)]
        public string RuleSetName
        {
            get
            {
                return ((String)(base.GetValue(PolicyFromService.RuleSetNameProperty)));
            }
            set
            {
                base.SetValue(PolicyFromService.RuleSetNameProperty, value);
            }
        }

        public static DependencyProperty MajorVersionProperty = DependencyProperty.Register("MajorVersion", typeof(System.Int32), typeof(PolicyFromService));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Optional)]
        [BrowsableAttribute(true)]
        public int MajorVersion
        {
            get
            {
                return ((int)(base.GetValue(PolicyFromService.MajorVersionProperty)));
            }
            set
            {
                base.SetValue(PolicyFromService.MajorVersionProperty, value);
            }
        }

        public static DependencyProperty MinorVersionProperty = DependencyProperty.Register("MinorVersion", typeof(System.Int32), typeof(PolicyFromService));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Optional)]
        [BrowsableAttribute(true)]
        public int MinorVersion
        {
            get
            {
                return ((int)(base.GetValue(PolicyFromService.MinorVersionProperty)));
            }
            set
            {
                base.SetValue(PolicyFromService.MinorVersionProperty, value);
            }
        }

        public static DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(System.String), typeof(PolicyFromService));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Optional)]
        [BrowsableAttribute(false)]
        public string ErrorMessage
        {
            get
            {
                return ((string)(base.GetValue(PolicyFromService.ErrorMessageProperty)));
            }
            set
            {
                base.SetValue(PolicyFromService.ErrorMessageProperty, value);
            }
        }


        public static DependencyProperty AfterPolicyAppliedEvent = DependencyProperty.Register("AfterPolicyApplied", typeof(EventHandler), typeof(PolicyFromService));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Optional)]
        [BrowsableAttribute(true)]
        public event EventHandler AfterPolicyApplied
        {
            add { base.AddHandler(AfterPolicyAppliedEvent, value); }
            remove { base.RemoveHandler(AfterPolicyAppliedEvent, value); }
        }

        
        public static DependencyProperty BeforePolicyAppliedEvent = DependencyProperty.Register("BeforePolicyApplied", typeof(EventHandler), typeof(PolicyFromService));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Optional)]
        [BrowsableAttribute(true)]
        public event EventHandler BeforePolicyApplied
        {
            add { base.AddHandler(BeforePolicyAppliedEvent, value); }
            remove { base.RemoveHandler(BeforePolicyAppliedEvent, value); }
        }  

        #endregion
    }
}




