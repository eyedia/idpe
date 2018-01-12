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

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(Eyedia.IDPE.Services.WorkflowActivities.SetDefaultValue))]
    public sealed class SetDefaultValue : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }

        [DefaultValue("NULL")]
        public InArgument<string> DefaultValue { get; set; }

        [DefaultValue(2)]
        public InArgument<int> AttributeType { get; set; }

        [DefaultValue(true)]
        public InArgument<bool> IfNullOrEmpty { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Job job = context.GetValue(this.Job);
            if (job != null)
            {                
                
            }
            else
            {
                WorkerData data = context.GetValue(this.Data);
                data.ThrowErrorIfNull(this.DisplayName);

                string defaultValue = context.GetValue(this.DefaultValue);
                int attributeType = context.GetValue(this.AttributeType);
                bool ifNullOrEmpty = context.GetValue(this.IfNullOrEmpty);

                List<Services.Attribute> columns = new List<Services.Attribute>();
                List<Services.Attribute> columnsSystem = new List<Services.Attribute>();

                if(!ifNullOrEmpty)
                {
                    columns = data.CurrentRow.Columns;
                    columnsSystem = data.CurrentRow.ColumnsSystem;
                }
                else
                {
                    columns = data.CurrentRow.Columns.Where(c => String.IsNullOrEmpty(c.Value)).ToList();
                    columnsSystem = data.CurrentRow.ColumnsSystem.Where(c => String.IsNullOrEmpty(c.Value)).ToList();
                }

                if(attributeType == 0) //all
                {
                    SetDefaultValueOfAColumn(columns, defaultValue);
                    SetDefaultValueOfAColumn(columnsSystem, defaultValue);
                }
                else if (attributeType == 1) //attribute
                {
                    SetDefaultValueOfAColumn(columns, defaultValue);                   
                }
                else if (attributeType == 2) //system attribute
                {
                    SetDefaultValueOfAColumn(columnsSystem, defaultValue);
                }
            }
        }

        private void SetDefaultValueOfAColumn(List<Services.Attribute> columns, string defaultValue)
        {
            columns.Select(c => { c.Value = defaultValue; return c; }).ToList();
        }
    }
}



