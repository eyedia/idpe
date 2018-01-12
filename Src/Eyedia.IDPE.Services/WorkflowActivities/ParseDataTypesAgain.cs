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
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using System.IO;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{

    public sealed class ParseDataTypesAgain : CodeActivity
    {

        public InArgument<WorkerData> Data { get; set; }
        public InArgument<Int32> OriginalPostion { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            WorkerData data = context.GetValue(this.Data);
            int originalPosition = context.GetValue(this.OriginalPostion);
            SreTraceLogWriter traceLog = data.CurrentRow.TraceLog;
            data.Job.PerformanceCounter.Start(data.Job.JobIdentifier, RowPerformanceTaskNames.ParseDataTypes_ParseAgain, originalPosition);
            
            data.ValueUpdationNotPermitted = true;
            for (int c = 0; c < data.CurrentRow.Columns.Count; c++)
            {
                //if (data.CurrentRow.Columns[c].Name == "AssetId")
                //    Debugger.Break();

                if (data.CurrentRow.Columns[c].HasBusinessError)
                    continue;

                SreMessage sreMsg = data.CurrentRow.Columns[c].Type.Parse(true);
                if (data.CurrentRow.Columns[c].Type is SreCodeset)
                {
                    SreCodeset sreCodeset = data.CurrentRow.Columns[c].Type as SreCodeset;
                    data.CurrentRow.Columns[c].ValueEnumCode = sreCodeset.ValueEnumCode;
                    data.CurrentRow.Columns[c].ValueEnumValue = sreCodeset.ValueEnumValue;
                    data.CurrentRow.Columns[c].ValueEnumReferenceKey = sreCodeset.ReferenceKey;
                }
                if ((sreMsg.Code != SreMessageCodes.SRE_SUCCESS) && (sreMsg.Code != data.CurrentRow.Columns[c].Error.Code))
                {
                    string oldMsg = data.CurrentRow.Columns[c].Error.Message;
                    data.CurrentRow.Columns[c].Error = sreMsg;
                    data.CurrentRow.Columns[c].Error.Message = string.Format("{0},{1}", oldMsg, data.CurrentRow.Columns[c].Error.Message);
                }
                else
                {
                    data.CurrentRow.Columns[c].Error = sreMsg;
                }
            }

            for (int c = 0; c < data.CurrentRow.ColumnsSystem.Count; c++)
            {
                if ((data.CurrentRow.ColumnsSystem[c].Name == "IsValid")
                    || (data.CurrentRow.ColumnsSystem[c].Value == "NULL")
                    || (data.CurrentRow.ColumnsSystem[c].IsNull))
                {
                    data.CurrentRow.ColumnsSystem[c].Error.Message = string.Empty; //Clear error when INT type updated with using 'lookup' and it returned 'NULL'
                    continue;
                }

                //if (data.CurrentRow.ColumnsSystem[c].Name == "AssetId")
                //    Debugger.Break();

                SreMessage sreMsg = data.CurrentRow.ColumnsSystem[c].Type.Parse(true);
                if (data.CurrentRow.ColumnsSystem[c].Type is SreCodeset)
                {
                    SreCodeset sreCodeset = data.CurrentRow.ColumnsSystem[c].Type as SreCodeset;
                    data.CurrentRow.ColumnsSystem[c].ValueEnumCode = sreCodeset.ValueEnumCode;
                    data.CurrentRow.ColumnsSystem[c].ValueEnumValue = sreCodeset.ValueEnumValue;
                    data.CurrentRow.ColumnsSystem[c].ValueEnumReferenceKey = sreCodeset.ReferenceKey;
                }
               
                if ((data.CurrentRow.ColumnsSystem[c].HasBusinessError) && (sreMsg.Code != SreMessageCodes.SRE_SUCCESS))
                {
                    //has business error, parsing failed
                    string oldMsg = data.CurrentRow.ColumnsSystem[c].Error.Message;
                    data.CurrentRow.ColumnsSystem[c].Error = sreMsg;
                    data.CurrentRow.ColumnsSystem[c].Error.Message = string.Format("{0},{1}", oldMsg, data.CurrentRow.ColumnsSystem[c].Error.Message);
                }
                else if ((data.CurrentRow.ColumnsSystem[c].HasBusinessError) && (sreMsg.Code == SreMessageCodes.SRE_SUCCESS))
                {
                    //has business error, parsing passed
                    //don't do anything, let the error continue
                }
                else
                {
                    data.CurrentRow.ColumnsSystem[c].Error = sreMsg;
                }
            }            
            data.Job.PerformanceCounter.Stop(data.Job.JobIdentifier, RowPerformanceTaskNames.ParseDataTypes_ParseAgain, originalPosition);
        }
    }
}



