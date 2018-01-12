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
using System.Threading;
using System.Diagnostics;
using System.Data;
using Eyedia.IDPE.Common;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{

    public sealed class ExecuteWorker : CodeActivity
    {
        public InArgument<Job> Job { get; set; }

        int MaxThreads = 0;
        int NumberOfWorkerRemains = 0;
        ManualResetEvent TheJobCompleted = new ManualResetEvent(false);
        ManualResetEvent AllThreadsAreBusy = new ManualResetEvent(false);
        Job job;
        protected override void Execute(CodeActivityContext context)
        {
            MaxThreads = EyediaCoreConfigurationSection.CurrentConfig.MaxThreads;
            job = context.GetValue(this.Job);
            NumberOfWorkerRemains = job.JobSlices.Count;
            job.TraceInformation("Initializing workers. Job slices:{0}. Max threads:{1}", job.JobSlices.Count, MaxThreads);
            Trace.Flush();

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < job.JobSlices.Count; i++)
            {
                Dictionary<string, object> inputs = new Dictionary<string, object>();
                WorkerData data = new WorkerData(job, i);
                inputs.Add("Data", data);
                WorkflowApplication workflowApp = new WorkflowApplication(new Worker(), inputs);

                workflowApp.Completed = WorkerCompleted;
                workflowApp.Aborted = WorkerAborted;
                workflowApp.Idle = WorkerIdle;
                workflowApp.PersistableIdle = WorkerPersistableIdle;
                workflowApp.Unloaded = WorkerUnloaded;
                workflowApp.OnUnhandledException = WorkerUnhandledException;

                try
                {
                    workflowApp.Run(new TimeSpan(0, SreConfigurationSection.CurrentConfig.WorkerTimeOut, 0));
                }
                catch (TimeoutException timeoutException)
                {
                    ExtensionMethods.TraceError("Worker has been timed out! {0}{1}", Environment.NewLine, timeoutException.ToString());
                    Trace.Flush();
                    CompleteJobIfAllWorkersCompleted();

                    if (NumberOfWorkerRemains < MaxThreads)
                    {
                        AllThreadsAreBusy.Set();
                    }

                }
                job.JobSlices[i].Status = JobSlice.JobSliceStatus.Processing;
                job.JobSlices[i].WorkflowInstanceId = workflowApp.Id;
                job.TraceInformation("Initializing worker '{0}' with {1}", i + 1, job.JobSlices[i].WorkflowInstanceId);
                if (job.NumberOfSlicesProcessing >= MaxThreads)
                {
                    ExtensionMethods.TraceInformation("All threads are busy, waiting...Threads: [Max allowed:{0}, Completed:{1}, Running:{2}", MaxThreads, NumberOfWorkerRemains, job.NumberOfSlicesProcessing);
                    Trace.Flush();
                    if (!AllThreadsAreBusy.WaitOne(new TimeSpan(0, SreConfigurationSection.CurrentConfig.TimeOut, 0)))   //config timeout in seconds
                    {
                        //timed out
                        job.AbortRequested = true;
                        job.AbortReason = Services.Job.AbortReasons.TimedOut;
                        string errorMsg = "All threads are busy since long, the complete process has been timed out! Time out (in Minutes) configured as "
                            + SreConfigurationSection.CurrentConfig.TimeOut + Environment.NewLine;
                        errorMsg += "Job Id:" + job.JobIdentifier + Environment.NewLine;
                        errorMsg += "File Name:" + job.FileName + Environment.NewLine;
                        errorMsg += "TotalRowsToBeProcessed:" + job.TotalRowsToBeProcessed + Environment.NewLine;
                        errorMsg += "TotalRowsProcessed:" + job.TotalRowsProcessed + Environment.NewLine;
                        job.TraceError(errorMsg);
                        Trace.Flush();
                        AbortWorkers();
                        AllThreadsAreBusy.Set();
                    }
                }
                Trace.Flush();
            }

            job.TraceInformation("Waiting to get finished...");
            Trace.Flush();
            TheJobCompleted.WaitOne();
            DoWithJob(job.IsErrored);
            if (!job.AbortRequested)
                ExtensionMethods.TraceInformation("Job '{0}' is finished at '{1}'. Workers elapsed time:{2}", job.JobIdentifier, job.FinishedAt, sw.Elapsed.ToString());
            else
                ExtensionMethods.TraceInformation("Job '{0}' is aborted at '{1}'. Workers elapsed time:{2}", job.JobIdentifier, job.FinishedAt, sw.Elapsed.ToString());
        }

        internal void WorkerCompleted(WorkflowApplicationCompletedEventArgs e)
        {
            if (!(e.Outputs.ContainsKey("ProcessedData")))
            {
                if (!job.AbortRequested)
                    ExtensionMethods.TraceInformation("The worker '{0}' was aborted with exception.", e.InstanceId);//dont want print if abort was requested
                AbortJob();
                return;
            }

            if (job.IsFinished)  //if job is already finished (most probably because of some error), dont do anything
            {
                return;
            }

            WorkerData data = e.Outputs["ProcessedData"] as WorkerData;
            if (!job.AbortRequested)
                job.TraceInformation("The worker '{0}' has finished its task. Slice position:{1}", e.InstanceId, data.SlicePosition);

            if (e.CompletionState == ActivityInstanceState.Faulted)
            {
                ExtensionMethods.TraceInformation(string.Format("Worker {0} Terminated.", e.InstanceId.ToString()));
                ExtensionMethods.TraceInformation(string.Format("Exception: {0}\n{1}",
                    e.TerminationException.GetType().FullName,
                    e.TerminationException.Message));
                Trace.Flush();
            }
            else if (e.CompletionState == ActivityInstanceState.Canceled)
            {
                ExtensionMethods.TraceInformation("Worker {0} was Canceled!", e.InstanceId);
                Trace.Flush();
            }
            else
            {
                DoWithData(data);
                CompleteJobIfAllWorkersCompleted();
            }

            AllThreadsAreBusy.Set();
            Trace.Flush();
        }

        bool OneAbortMessageLogged;
        internal void WorkerAborted(WorkflowApplicationAbortedEventArgs e)
        {
            //CompleteJobIfAllWorkersCompleted();
            if (!OneAbortMessageLogged)
            {
                //because we dont want to send same message for all workers.
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Fatal Error: A worker {0} was aborted. Forcefully aborting the job!", e.InstanceId));
                sb.AppendLine();
                sb.Append(string.Format("Reason was: {0}\n{1}", e.Reason.GetType().FullName, e.Reason.ToString()));
                ExtensionMethods.TraceError(sb.ToString());
                Trace.Flush();
                OneAbortMessageLogged = true;
            }

            AbortJob();
        }

        internal void WorkerIdle(WorkflowApplicationIdleEventArgs e)
        {
           
        }

        internal PersistableIdleAction WorkerPersistableIdle(WorkflowApplicationIdleEventArgs e)
        {           
            return PersistableIdleAction.Unload;
        }

        internal void WorkerUnloaded(WorkflowApplicationEventArgs e)
        {
            //ExtensionMethods.TraceInformation("Worker '{0}' unloaded.", e.InstanceId);
            //Trace.Flush();
        }

        bool OneUnhandledExceptionMessageLogged;
        internal UnhandledExceptionAction WorkerUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs e)
        {
            lock (_lock)
            {
                if (!OneUnhandledExceptionMessageLogged)
                {
                    if (job.AbortRequested)
                    {
                        //if job was forcefully aborted, we would like to ignore errors and send a clean error message
                        string errorMessage = string.Empty;
                        if (job.AbortReason == Services.Job.AbortReasons.TimedOut)
                        {
                            errorMessage = string.Format("The Job '{0}' timed out. The time out was set to {1} minutes.",
                                job.JobIdentifier, SreConfigurationSection.CurrentConfig.TimeOut);
                            errorMessage += string.Format(" Started at {0} & file {1} had {2} records.", job.StartedAt, job.FileNameOnly, job.TotalRowsToBeProcessed);
                        }
                        else
                        {
                            errorMessage = string.Format("A business rule has requested to abort the job '{0}'! Aborting job...", job.JobIdentifier);
                        }

                        ExtensionMethods.TraceInformation(errorMessage);
                        new PostMan(job, false).Send(PostMan.__errorStartTag + errorMessage + PostMan.__errorEndTag);
                    }
                    else
                    {
                        //We dont want to send from here. commented 10/12/2016
                        //StringBuilder sb = new StringBuilder();
                        //sb.AppendLine(string.Format("Fatal Error: A worker {0} was aborted because of an unhandled exception. Forcefully aborting the job!", e.InstanceId));
                        //sb.AppendLine();
                        //sb.Append(string.Format("Exception occurred in worker {0}\n{1}.\nExceptionSource: {2}", e.InstanceId, e.UnhandledException.ToString(), e.ExceptionSource.DisplayName));
                        //job.TraceError(sb.ToString());
                    }
                    Trace.Flush();
                    OneUnhandledExceptionMessageLogged = true;
                }
            }
            AbortJob();
            return UnhandledExceptionAction.Terminate;
        }

        internal void CompleteJobIfAllWorkersCompleted()
        {
            if (Interlocked.Decrement(ref NumberOfWorkerRemains) == 0)
            {
                TheJobCompleted.Set();
            }
            else
            {
                ExtensionMethods.TraceInformation("{0} worker(s) are still working. Waiting...", NumberOfWorkerRemains);
            }
        }

        private static readonly object _lock = new object();

        private void DoWithData(WorkerData data)
        {
            if ((data.Job.AbortRequested)
                    && (data.Job.IsFinished == false))
            {
                ExtensionMethods.TraceInformation("Abort was requested. Trying to abort job...");
                Trace.Flush();
                AbortJob();
            }
            else
            {
                lock (_lock)
                {

                    JobSlice processedJobSlice = null;
                    try
                    {
                        if (job.JobSlices.Count > 0)
                        {
                            processedJobSlice = job.JobSlices[data.SlicePosition];
                        }
                        else
                        {
                            //job was already disposed for some reason. Errors should get already taken care.
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        StringBuilder sb = new StringBuilder("Dumping ArgumentException:" + Environment.NewLine + ex.ToString());
                        sb.AppendLine(string.Format("Trying to access job.JobSlices[{0}]. Total job slices were {1}", data.SlicePosition, job.JobSlices.Count));
                        job.TraceError(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        StringBuilder sb = new StringBuilder("Dumping Exception:" + Environment.NewLine + ex.ToString());
                        sb.AppendLine(string.Format("Trying to access job.JobSlices[{0}]. Total job slices were {1}", data.SlicePosition, job.JobSlices.Count));
                        job.TraceError(sb.ToString());
                    }
                    if (processedJobSlice == null)
                    {
                        AbortJob();
                        return;
                    }

                    processedJobSlice.Status = JobSlice.JobSliceStatus.Processed;

                    if (!job.AbortRequested)
                    {
                        ExtensionMethods.TraceInformation(Environment.NewLine);
                        job.TraceInformation("JS - '{0}' - processed '{1}' records. {2}{3}",
                            processedJobSlice.JobSliceId, data.Rows.Count, Environment.NewLine, data.TraceLog.ToString());
                        Trace.Flush();
                    }

                    job.TotalRowsProcessed += processedJobSlice.InputData.Rows.Count;
                    job.Rows.AddRange(data.Rows);
                    job.Errors.AddRange(data.Errors);
                    job.Warnings.AddRange(data.Warnings);
                    job.BadDataInCsvFormat.AddRange(data.BadDataInCsvFormat);

                    data.Dispose();
                }
            }
        }

        private void DoWithJob(bool isErrored = false)
        {
            lock (_lock)
            {
                job.IsFinished = true;
                job.IsErrored = isErrored;
                job.FinishedAt = DateTime.Now;
            }
        }

        private void AbortJob()
        {
            lock (_lock)
            {
                TheJobCompleted.Set();
                AbortWorkers();
                AllThreadsAreBusy.Set();
                DoWithJob(true);
            }
        }

        private void AbortWorkers()
        {
            foreach (JobSlice jobSlice in job.JobSlices)
            {
                try
                {
                    if (jobSlice.WorkflowInstanceId != Guid.Empty)
                    {
                        WorkflowApplication wfApp = new WorkflowApplication(new Worker());
                        wfApp.Load(jobSlice.WorkflowInstanceId);
                        wfApp.Cancel();
                    }
                }
                catch { }

            }
        }

    }
}



