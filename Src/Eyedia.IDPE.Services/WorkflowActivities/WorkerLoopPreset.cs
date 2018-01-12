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
using System.Data;
using System.IO;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Services
{

    public sealed class WorkerLoopPreset : CodeActivity
    {
        
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<DataRow> DataRow { get; set; }
        public OutArgument<bool> ThisRowIsHavingContainerError { get; set; }
        public OutArgument<Int32> OriginalPosition { get; set; }

        
        protected override void Execute(CodeActivityContext context)
        {            
            WorkerData data = context.GetValue(this.Data);
            DataRow dataRow = context.GetValue(this.DataRow);

            int originalRowPosition = (int)dataRow[Constants.ColumnNamePosition].ToString().ParseInt();
            OriginalPosition.Set(context, originalRowPosition);
            string containerError = dataRow[Constants.ColumnNameContainerError].ToString(); //data.Job.InputData.Rows[originalRowPosition - 1][Constants.ColumnNameContainerError].ToString();
            int rawColumnPosition = 1;  //skip first 2 columns

            data.Job.PerformanceCounter.Start(data.Job.JobIdentifier, RowPerformanceTaskNames.LoopPreset, originalRowPosition);

            //fill Columns with incoming data, which might got modified in PreParse BS, and then get parsed.
            data.Rows.Add(new Row(originalRowPosition));
            SreTraceLogWriter traceLog = data.CurrentRow.TraceLog;
            
            foreach (SreAttribute a in data.Job.DataSource.Attributes)
            {                

                string value = string.Empty;
                if (a.IsAcceptable == true)
                {
                    rawColumnPosition++;
                    value = dataRow[rawColumnPosition].ToString();
                }
                data.CurrentRow.Columns.Add(new Attribute(a.AttributeId, a.Name, value, string.Empty, traceLog, (bool)a.IsAcceptable));
            }
            foreach (SreAttribute a in data.Job.DataSource.AttributesSystem)
            {
                data.CurrentRow.ColumnsSystem.Add(new Attribute(a.AttributeId, a.Name, string.Empty, string.Empty, traceLog, (bool)a.IsAcceptable, true, a.AttributePrintValueType, a.AttributePrintValueCustom));
            }

            Attribute attrbIsValid = data.CurrentRow.ColumnsSystem["IsValid"];
            if (!string.IsNullOrEmpty(containerError))
            {
                ThisRowIsHavingContainerError.Set(context, true);
                traceLog.WriteLine("The row '{0}' has container validation errors, skipping it.", originalRowPosition);

                if (attrbIsValid != null)
                {
                    attrbIsValid.Value = "false";
                    if (attrbIsValid.Error != null)
                    {
                        attrbIsValid.Error.Message = data.PrintRowColPosition(originalRowPosition, "*") + containerError;
                    }
                    else
                    {
                        containerError = string.Format("Row[{0}][IsValid]:{1}", originalRowPosition, containerError);
                        attrbIsValid.Error = new SreMessage(containerError);

                    }

                    //Lets instantiate attribute results with a generic error,
                    // else it will be null and throw error through out the life cycle.
                    foreach (Attribute a in data.CurrentRow.Columns)
                    {
                        a.Error = new SreMessage(SreMessageCodes.SRE_FAILED_BLANK);
                    }
                    foreach (Attribute a in data.CurrentRow.ColumnsSystem)
                    {
                        if (!a.Name.Equals("IsValid", StringComparison.OrdinalIgnoreCase))
                            a.Error = new SreMessage(SreMessageCodes.SRE_FAILED_BLANK);
                    }
                }

            }
            else
            {
                ThisRowIsHavingContainerError.Set(context, false);
                if (attrbIsValid != null)
                    attrbIsValid.Value = "true";    //by default true, if any error occurres, will be false in 'Finally'
                else
                    data.Errors.Add(string.Format("{0}Critical mandatory system attribute 'IsValid' not found in the collection.", data.PrintRowColPosition(originalRowPosition, "", true)));
            }
            
            data.ValueUpdationNotPermitted = false;
            data.Job.PerformanceCounter.Stop(data.Job.JobIdentifier, RowPerformanceTaskNames.LoopPreset, originalRowPosition);
        }
    }
}



