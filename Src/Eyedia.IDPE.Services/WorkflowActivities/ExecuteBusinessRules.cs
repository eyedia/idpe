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

namespace Eyedia.IDPE.Services
{

    public sealed class ExecuteBusinessRules : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<RuleSetTypes> RuleSetType { get; set; }
        public InArgument<Int32> OriginalPostion { get; set; }
       
        protected override void Execute(CodeActivityContext context)
        {
            Job job = context.GetValue(this.Job);
            WorkerData data = context.GetValue(this.Data);
            RuleSetTypes ruleSetType = context.GetValue(this.RuleSetType);
         
            int originalPosition = context.GetValue(this.OriginalPostion);

            job = job == null ? data.Job : job;
            
            StartPerformanceCounter(job, ruleSetType, originalPosition);
            job.DataSource.BusinessRules.Execute(job, data, ruleSetType);
            //if (job.DataSource.BusinessRules.ExceptionOccurred)
            //{
            //    job.AbortRequested = true;
            //    job.AbortReason = Services.Job.AbortReasons.BusinessRuleFailed;
            //}
            StopPerformanceCounter(job, ruleSetType, originalPosition);

        }

        #region Helpers
        private void StartPerformanceCounter(Job job, RuleSetTypes ruleSetType, int originalPosition)
        {
            if (ruleSetType == RuleSetTypes.PreValidate)
                job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.PreValidate);
            else if (ruleSetType == RuleSetTypes.RowPreparing)
                job.PerformanceCounter.Start(job.JobIdentifier, RowPerformanceTaskNames.RowPreparing, originalPosition);
            else if (ruleSetType == RuleSetTypes.RowPrepared)
                job.PerformanceCounter.Start(job.JobIdentifier, RowPerformanceTaskNames.RowPrepared, originalPosition);
            else if (ruleSetType == RuleSetTypes.RowValidate)
                job.PerformanceCounter.Start(job.JobIdentifier, RowPerformanceTaskNames.RowValidate, originalPosition);
            else if (ruleSetType == RuleSetTypes.PostValidate)
                job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.PostValidate);
        }

        private void StopPerformanceCounter(Job job, RuleSetTypes ruleSetType, int originalPosition)
        {
            if (ruleSetType == RuleSetTypes.PreValidate)
                job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.PreValidate);
            else if (ruleSetType == RuleSetTypes.RowPreparing)
                job.PerformanceCounter.Stop(job.JobIdentifier, RowPerformanceTaskNames.RowPreparing, originalPosition);
            else if (ruleSetType == RuleSetTypes.RowPrepared)
                job.PerformanceCounter.Stop(job.JobIdentifier, RowPerformanceTaskNames.RowPrepared, originalPosition);
            else if (ruleSetType == RuleSetTypes.RowValidate)
                job.PerformanceCounter.Stop(job.JobIdentifier, RowPerformanceTaskNames.RowValidate, originalPosition);
            else if (ruleSetType == RuleSetTypes.PostValidate)
                job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.PostValidate);
        }
        #endregion Helpers
    }
}



