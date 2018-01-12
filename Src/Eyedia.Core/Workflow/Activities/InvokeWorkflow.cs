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
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace Symplus.Core.Workflow
{
    public partial class InvokeWorkflow : Activity, ITypeFilterProvider
      {
        public static readonly DependencyProperty TargetWorkflowProperty = DependencyProperty.Register("TargetWorkflow", typeof(Type), typeof(InvokeWorkflow), new PropertyMetadata(null));
        public static readonly DependencyProperty ParametersProperty = DependencyProperty.Register("Parameters", typeof(Dictionary<string, object>), typeof(InvokeWorkflow), new PropertyMetadata(DependencyPropertyOptions.ReadOnly, new Attribute[] { new BrowsableAttribute(false), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content) }));
        public static readonly DependencyProperty InstanceIdProperty = DependencyProperty.Register("InstanceId", typeof(Guid), typeof(InvokeWorkflow));
        public static DependencyProperty BeforeInvokedEvent = DependencyProperty.Register("BeforeInvoked", typeof(EventHandler), typeof(InvokeWorkflow));
        public static DependencyProperty AfterInvokedEvent = DependencyProperty.Register("AfterInvoked", typeof(EventHandler), typeof(InvokeWorkflow));
        
        public InvokeWorkflow()
        {
            base.SetReadOnlyPropertyValue(ParametersProperty, new Dictionary<string, object>());
            InitializeComponent();
        }
 
        #region Designer generated code
 
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // InvokeWorkflow
            // 
            this.Name = "InvokeWorkflow";
 
        }
 
        #endregion

        [Category("Activity")]
        [Description("InstanceId of of the invoked workflow")]
        public Guid InstanceId
        {
            get
            {
                return (Guid)base.GetValue(InstanceIdProperty);
            }
            set
            {
                base.SetValue(InstanceIdProperty, value);
            }
        }

        [Category("Activity")]
        [Description("Workflow type to be invoked")]
        [Editor(typeof(TypeBrowserEditor), typeof(UITypeEditor))]
        [DefaultValue(null)]
        public Type TargetWorkflow
        {
            get
            {
                return base.GetValue(TargetWorkflowProperty) as Type;
            }
            set
            {
                base.SetValue(TargetWorkflowProperty, value);
            }
        }

        [Description("Fired before invoking target workflow")]
        [Category("Handlers")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event EventHandler BeforeInvoked
        {
            add
            {
                base.AddHandler(InvokeWorkflow.BeforeInvokedEvent, value);
            }
            remove
            {
                base.RemoveHandler(InvokeWorkflow.BeforeInvokedEvent, value);
            }
        }

        [Description("Fired after invoking target workflow")]
        [Category("Handlers")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event EventHandler AfterInvoked
        {
            add
            {
                base.AddHandler(InvokeWorkflow.AfterInvokedEvent, value);
            }
            remove
            {
                base.RemoveHandler(InvokeWorkflow.AfterInvokedEvent, value);
            }
        }
 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public Dictionary<string, object> Parameters
        {
            get
            {
                return base.GetValue(ParametersProperty) as Dictionary<string, object>;
            }
        }        
 
        #region ITypeFilterProvider Members
 
        bool ITypeFilterProvider.CanFilterType(Type type, bool throwOnError)
        {
            bool canFilterType = TypeProvider.IsAssignable(typeof(Activity), type) && type != typeof(Activity) && !type.IsAbstract;
            
            if (throwOnError && !canFilterType)
                throw new Exception("Type does not derive from Activity");
 
            return canFilterType;
        }
 
        string ITypeFilterProvider.FilterDescription
        {
            get 
            {
                return "Select a Type that derives from Activity.";
            }
        }
 
        #endregion
 
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            
            this.RaiseEvent(InvokeWorkflow.BeforeInvokedEvent, this , new EventArgs());
            
            if (this.TargetWorkflow == null)
                throw new InvalidOperationException("TargetWorkflow property must be set to a valid Type that derives from Activity.");
            
            IStartWorkflow startWorkflow = executionContext.GetService(typeof(IStartWorkflow)) as IStartWorkflow;
            this.InstanceId = startWorkflow.StartWorkflow(this.TargetWorkflow, this.Parameters);            

            //base.SetValue(InstanceIdProperty, instanceId);
            this.RaiseEvent(InvokeWorkflow.AfterInvokedEvent, this, new EventArgs());

            return ActivityExecutionStatus.Closed;
        }
    }
}





