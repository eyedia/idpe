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
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{

    public sealed class StoreBadData : CodeActivity
    {
        
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<bool> ThisRowIsHavingContainerError { get; set; }
        public InArgument<Int32> OriginalPostion { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            WorkerData data = context.GetValue(this.Data);
            int originalPosition = context.GetValue(this.OriginalPostion);

            bool thisRowIsHavingContainerError = context.GetValue(this.ThisRowIsHavingContainerError);
            data.Job.PerformanceCounter.Start(data.Job.JobIdentifier, RowPerformanceTaskNames.StoreBadData, originalPosition);
            if ((data.CurrentRow.HasAnyError())
               || (thisRowIsHavingContainerError))
            {
                data.BadDataInCsvFormat.Add(data.Job.JobSlices[data.SlicePosition].CSVRows[data.RowPosition]);
            }
            data.Job.PerformanceCounter.Stop(data.Job.JobIdentifier, RowPerformanceTaskNames.StoreBadData, originalPosition);
        }
    }
}



