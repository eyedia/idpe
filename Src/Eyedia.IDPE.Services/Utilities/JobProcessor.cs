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
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Workflow.Runtime;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Collections;
using System.IO;
using System.Activities;
using System.Text;

namespace Eyedia.IDPE.Services
{    

    public class JobProcessor : IDisposable
    {
        public JobProcessor(){}

        public bool IsWCFRequest { get; internal set; }
        public string JobId { get; private set; }

        /// <summary>
        /// Start called when 'Caller' wants to parse one row at a time. Initiate a new job based on input parameters.
        /// Either applicationId should contain a valid application id or applicationName should be a valid application name.
        /// </summary>
        /// <param name="dataSourceId">Should be a valid application id. If this is filled, then applicationName is optional.</param>
        /// <param name="dataSourceName">Should be a valid application name. If this is filled, then applicationId is optional.</param>
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like 'a126042, Deb'jyoti Das', let caller decide the user name.</param>
        /// <returns>Retruns process identifier as GUID or string.empty (fail condition)</returns>
        public string InitializeJob(int dataSourceId, string dataSourceName, string processingBy)
        {
            if ((dataSourceId <= 0) && (string.IsNullOrEmpty(dataSourceName)))
            {
                ExtensionMethods.TraceInformation("Job could not be initialized, data source is not defined. Id = {0}, name = '{1}'", dataSourceId, dataSourceName);
                return string.Empty;    //fail response
            }

            try
            {
                Job newJob = new Job(dataSourceId, dataSourceName, processingBy);

                if (!newJob.IsValid)
                {
                    ExtensionMethods.TraceInformation("Job could not be initialized, data source is not defined. Id ={0}, name = '{1}'", dataSourceId, dataSourceName);
                    return string.Empty;    //fail response
                }

                Registry.Instance.Entries.Add(newJob.JobIdentifier, newJob);
                Trace.Flush();
                return newJob.JobIdentifier;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                ExtensionMethods.TraceError(errorMessage);                
                Trace.Flush();
                throw ex;
            }
        }
      


        /// <summary>
        /// Finish to be called when 'Caller' finished parsing row by row, and wants to get the result.
        /// </summary>
        /// <param name="jobId">Process identifier</param>
        /// <returns>Output xml. for details, read documentation.</returns>
        public StringBuilder GetJobResult(string jobId)
        {
            Job finishedJob = null;
            try
            {
                finishedJob = Registry.Instance.Entries[jobId] as Job;
                finishedJob.TraceInformation("Got request for job result of '{0}'. Making KeepInMemoryUntilTimeOut = true to be cleaned next 'registry clean cycle'", jobId);
                finishedJob.KeepInMemoryUntilTimeOut = false;   //Registry cleaner will remove it next time
                if (finishedJob != null)
                    return finishedJob.DataSource.OutputWriter.GetOutput();

                finishedJob.TraceError("The job with id '{0}' was not found! An empty string was returned.", jobId);
                Trace.Flush();
                return new StringBuilder();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                finishedJob.TraceError(errorMessage);
                Trace.Flush();
                throw ex;
            }
        }

        /// <summary>
        /// This method is used when 'Caller' wants to parse complete file at a time.
        /// </summary>        
        /// <param name="xmlInput">Input data in xml format.</param>        
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like 'a126042, Deb'jyoti Das', let caller decide the user name.</param>
        /// <returns>Output xml. for details, read documentation.</returns>
        public StringBuilder ProcessXml(string xmlInput, string processingBy)
        {
            int dataSourceId = 0;
            Job job = null;
            string dataSourceName = string.Empty;
            try
            {
                List<string> xmlValidationErrors = new List<string>();
                List<string> xmlValidationWarnings = new List<string>();

                DataSource unknownDataSource = new DataSource(dataSourceId, dataSourceName);
                new SreXmlToDataTable(unknownDataSource).ParseDataSourceDetails(xmlInput, ref dataSourceId, ref dataSourceName, ref xmlValidationErrors, ref xmlValidationWarnings);

                if (string.IsNullOrEmpty(xmlInput))   //we can not accept empty string
                {
                    job.TraceError("Input xml was blank! Nothing to parse.");
                    return new StringBuilder();
                }
                else if (xmlValidationErrors.Count > 0)
                {
                    job.TraceError("Input xml had validation errors! {0}", xmlValidationErrors.ToLine());
                    return new StringBuilder();
                }

                job = new Job(dataSourceId, dataSourceName, processingBy);
                if (!job.IsValid)
                {
                    job.TraceInformation("Job could not be initialized, application is not defined. Application id ={0}, name = '{1}'", dataSourceId, dataSourceName);
                    return new StringBuilder();

                }
                this.JobId = job.JobIdentifier.ToString();

                //WorkerData data = new WorkerData(newJob.DataSource, newJob.JobIdentifier, newJob.ProcessingBy);
                WorkerData data = new WorkerData(job, 0); //todo
                data.RowPosition = 1;
                job.Warnings.AddRange(data.Warnings);


                if ((job.CsvRows.Count == 0)
                    || (job.CsvRows.Count == 1))
                {
                    //blank file                    
                    job.TraceInformation("System expects at least one request.");
                    return new StringBuilder();
                }

                Registry.Instance.Entries.Add(job.JobIdentifier, job);
                ExecuteWorkerManager(job);

                //Get the results from worker (data)
                job.Errors.AddRange(data.Errors);       //get all errors                
                job.Warnings.AddRange(data.Warnings);
                job.BadDataInCsvFormat.AddRange(data.BadDataInCsvFormat);    //bad csv data (invalid)                
                Trace.Flush();
                return job.DataSource.OutputWriter.GetOutput();
            }
            catch (BusinessException ex)
            {
                job.TraceInformation(ex.Message);   //It is actually not an exception or error                
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);
                return new StringBuilder();
            }
            catch (Exception ex)
            {
                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                job.TraceError("A server exception occurred. Please contact support. ErrorId={0}, Time={1}\n{2}", errorId, DateTime.Now, errorMessage);
                Trace.Flush();
                return new StringBuilder();
            }
        }


        /// <summary>
        /// This method is used when 'Caller' wants to parse complete file at a time.
        /// </summary>
        /// <param name="dataSourceId">Id of the data source (either id or name required)</param>        
        /// <param name="dataSourceName">Name of the data source (either id or name required)</param>   
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like &apos;a126042, Deb'jyoti Das&apos;, let caller decide the user name.</param>        
        /// <returns>Output xml. for details, read documentation.</returns>

        public StringBuilder ProcessJob(int dataSourceId, string dataSourceName, string processingBy)
        {
            Job job = null;
            try
            {
                string error = string.Empty;
                StringBuilder result = new StringBuilder();
                job = new Job(dataSourceId, dataSourceName, processingBy);
                Registry.Instance.Entries.Add(job.JobIdentifier, job);


                job.IsWCFRequest = true;
                this.JobId = job.JobIdentifier.ToString();
                if (!job.IsValid)
                {
                    error = string.Format("Job could not be initialized, data source is not defined. data source id ={0}, name = '{1}'", dataSourceId, dataSourceName);
                    job.TraceInformation(error);
                    return result;
                }

                job.Feed();
                
                if (job.CsvRows.Count == 0)
                {
                    //blank file                    
                    job.TraceInformation(job.ErrorBlankInput);
                    job.Errors.Add(job.ErrorBlankInput);
                    result = job.DataSource.OutputWriter.GetOutput();
                    //job.Dispose();
                    return result;
                }
                ExecuteWorkerManager(job);

                //Get the results from worker (data)
                job.Errors.AddRange(GetAllAttributeErrors(job.Rows));     //get all attribute errors              

                if (!job.AbortRequested)
                {
                    job.TraceInformation("Calling output writer '{0}'. Total processed rows:{1}", job.DataSource.OutputWriterTypeFullName, job.Rows.Count);
                    Trace.Flush();

                    if (job.Errors.Count > 0)
                        new PostMan(job).Send();

                    result = job.DataSource.OutputWriter.GetOutput();
                    job.Dispose();
                    return result;
                }

                return new StringBuilder();

            }
            catch (BusinessException ex)
            {
                job.TraceInformation(ex.Message);   //It is actually not an exception or error
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);
                return new StringBuilder();
            }
            catch (Exception ex)
            {

                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);                
                job.TraceError("A server exception occurred. Please contact support. ErrorId = '{0}'\n. Server Time = '{1}'\n{2}", errorId, DateTime.Now, errorMessage);
                Trace.Flush();
                return new StringBuilder();              
            }
        }       
        
        /// <summary>
        /// This method is used when 'Caller' wants to parse complete file at a time.
        /// </summary>
        /// <param name="jobId">JobId which you got after initializing a job</param>        
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like &apos;a126042, Deb'jyoti Das&apos;, let caller decide the user name.</param>
        /// <param name="inputData">List of custom objects</param>        
        ///<param name="overridenMapping">Comma separated additional mapping override information. For example if "EmpId" from object to be mapped with "EmployeeId" of attribute, then "EmpId=EmployeeId,Ename=EmployeeName"</param>
        /// <returns>Output object. for details, read documentation.</returns>
        public object ProcessJob(string jobId, string processingBy, List<object> inputData, string overridenMapping)
        {
            string error = string.Empty;
            Job job = ValidateJob(jobId, processingBy, inputData);
            if (!job.IsValid)            
                return job.GetCustomOutput();   //errors will be filled by ValidateJob()            

            try
            {
                job.Feed(inputData, overridenMapping);
                if (job.CsvRows.Count == 0)
                {
                    error = "System expects at least one request.";
                    job.TraceInformation(error);
                    job.Errors.Add(error);
                    return job.GetCustomOutput();
                }

                ExecuteWorkerManager(job);
                if (!job.AbortRequested)
                {
                    job.Errors.AddRange(GetAllAttributeErrors(job.Rows));
                    job.TraceInformation("Calling output writer '{0}'. Total processed rows:{1}", job.DataSource.OutputWriterTypeFullName, job.Rows.Count);
                    Trace.Flush();

                    if (job.Errors.Count > 0)
                        new PostMan(job).Send();

                    object returnObject = job.GetCustomOutput();
                    job.Dispose();
                    job = null;
                    RemoveJobFromRegistry(jobId);
                    return returnObject;
                }
                else
                {
                    job.Dispose();
                    job = null;
                    RemoveJobFromRegistry(jobId);
                    return new object();
                }

            }
            catch (BusinessException ex)
            {
                job.TraceInformation(ex.Message);   //It is actually not an exception or error
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);
                return new Object();
            }
            catch (Exception ex)
            {
                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                job.TraceError(errorMessage);
                Trace.Flush();
                if ((job != null)
                    && (job.DataSource != null)
                    && (job.DataSource.OutputWriter != null))
                {
                    job.Errors.Add(ex.ToString());
                    if (job != null)                    
                    {
                        return job.DataSource.OutputWriter.GetCustomOutput();
                    }
                    else
                    {
                        return new Object();
                    }
                }
                return new object();
            }
        }

        private static readonly object _lock = new object();
        void RemoveJobFromRegistry(string jobId)
        {
            lock (_lock)
            {
                if (Registry.Instance.Entries.ContainsKey(jobId))
                {
                    Job theJob = Registry.Instance.Entries[jobId] as Job;
                    Registry.Instance.Entries.Remove(jobId);
                    theJob.Dispose();
                    theJob = null;
                }
            }
        }

        Job ValidateJob(string jobId, string processingBy, List<object> inputData)
        {

            string error = string.Format("There is no job found with id '{0}', or job might have expired.", jobId);

            if (!(Registry.Instance.Entries.ContainsKey(jobId)))
            {
                ExtensionMethods.TraceInformation(error);
                Job job = new Job(0, string.Empty, string.Empty);
                job.Errors.Add(error);
                return job;
            }

            Job currentJob = Registry.Instance.Entries[jobId] as Job;
            if (currentJob == null)
            {
                currentJob.TraceInformation(error);
                Job job = new Job(0, string.Empty, string.Empty);
                job.Errors.Add(error);
                return job;
            }

            this.JobId = currentJob.JobIdentifier.ToString();

            if (inputData == null)   //we can not accept null object
            {
                error = "input data is null or empty. Nothing to parse.";
                currentJob.TraceInformation(error);
                currentJob.Errors.Add(error);
                return currentJob;
            }
            if (!currentJob.ProcessingBy.Equals(processingBy, StringComparison.OrdinalIgnoreCase))
            {
                error = string.Format("The job '{0}' was not initialized by '{1}'. Can not continue.", jobId, processingBy);
                currentJob.TraceInformation(error);
                currentJob.Errors.Add(error);
                return currentJob;
            }

            return currentJob;
        }

        /// <summary>
        /// Initialize a new job if jobId is NULL, feed CSV/FixedLength data into worker &amp; executes worker
        /// </summary>        
        /// <param name="dataSourceId">Should be a valid application id. If this is filled, then datasource name is optional.</param>
        /// <param name="dataSourceName">Should be a valid datasource name. If this is filled, then datasource id is optional.</param>        
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like &apos;a126042, Deb'jyoti Das&apos;, let caller decide the user name.</param>
        /// <param name="fileName">fileName is used for just reference</param>        
        /// <param name="jobId">When initial request comes from WCF, we already have jobId, else string.Empty</param>
        /// <param name="withError">Error message if you want to execute a dummy execution just to pass the error message to caller</param>
        /// <param name="withWarning">Warning message if you want to execute a dummy execution just to pass the error message to caller</param>
        /// <returns>Output StringBuilder. for details, read documentation.</returns>
        public StringBuilder ProcessJob(int dataSourceId, string dataSourceName, string processingBy, string fileName, string jobId, string withError, string withWarning)
        {
            
            Job job = null;
            try
            {                
                string error = string.Empty;
                StringBuilder result = new StringBuilder();
                if (string.IsNullOrEmpty(jobId))
                {
                    job = new Job(dataSourceId, dataSourceName, processingBy, fileName);
                    if (job.Errors.Count > 0)
                        throw new BusinessException(job.Errors.ToLine("<br />"));

                    Registry.Instance.Entries.Add(job.JobIdentifier, job);                    
                }
                else
                {
                    error = string.Format("There is no job found with id '{0}', or job might have expired.", jobId);

                    if (!(Registry.Instance.Entries.ContainsKey(jobId)))
                        job.TraceInformation(error);
                    else                        
                        job = Registry.Instance.Entries[jobId] as Job;

                    if (job == null)
                    {
                        job.TraceInformation(error);
                        return result;
                    }
                }
                job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                job.IsWCFRequest = this.IsWCFRequest;
                this.JobId = job.JobIdentifier.ToString();
                if (!job.IsValid)
                {
                    error = string.Format("Job could not be initialized, application is not defined. Application id ={0}, name = '{1}'", dataSourceId, dataSourceName);
                    job.TraceInformation(error);

                    job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                    return result;
                } 

                if ((!string.IsNullOrEmpty(withError)) || (!string.IsNullOrEmpty(withWarning)))
                {                    
                    if (job.FileNameOnly.StartsWith(Constants.UnzippedFilePrefix))
                    {
                        job.FileName = ZipFileWatcher.ExtractActualFileName(job.FileName);                        

                        string originalFileName = Path.GetFileName(job.FileName);
                        withError = withError.Replace(job.FileNameOnly, originalFileName);
                        withWarning = withWarning.Replace(job.FileNameOnly, originalFileName);
                        File.Delete(fileName);//we do not need as zip will be archived

                        //this is required, as the consumer most of the time attaches the errored file in emails
                        //as we deleted the actual file and job.FileName will never be retrieved. Hence set the actual-actual file, i.e. original zip file
                        job.FileName = Registry.Instance.ZipFiles[ZipFileWatcher.ExtractUniqueId(job.FileNameOnly)].ZipFileName;
                    }
                    
                    if (!string.IsNullOrEmpty(withError))
                    {
                        withError = withError.Replace("APPNAME", job.DataSource.Name);
                        job.Errors.Add(withError);
                    }

                    if (!string.IsNullOrEmpty(withWarning))
                    {
                        withWarning = withWarning.Replace("APPNAME", job.DataSource.Name);
                        job.Warnings.Add(withWarning);
                    }

                    Trace.Flush();                    
                    result = job.DataSource.OutputWriter.GetOutput();

                    job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                    //job.Dispose();
                    return result;
                }
                job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                
                job.Feed();
                
                //Debugger.Break();
                if (job.InputData.Rows.Count == 0)
                {
                    job.IsFinished = true;
                    //blank file                    
                    job.TraceInformation(job.ErrorBlankInput);
                    job.Errors.Add(job.ErrorBlankInput);
                    //new PostMan(job, false).Send(PostMan.__errorStartTag + job.ErrorBlankInput + PostMan.__errorEndTag);                    
                    return new StringBuilder();
                }

                job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.ExecuteWorkerManager);
                ExecuteWorkerManager(job);
                job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.ExecuteWorkerManager);

                //Get the results from worker (data)
                job.Errors.AddRange(GetAllAttributeErrors(job.Rows));     //get all attribute errors         

                if ((job.FileNameOnly != null)
                    && (job.FileNameOnly.StartsWith(Constants.UnzippedFilePrefix)))
                {
                    //1: orginal file
                    //job.FileName = ZipFileWatcher.ExtractActualFileName(job.FileName);                    
                    File.Delete(fileName);//we do not need as zip will be archived

                    //2: zip file (this is important if consumer wants to refer the file (zip file exists)
                    job.FileName = Registry.Instance.ZipFiles[ZipFileWatcher.ExtractUniqueId(job.FileNameOnly)].ZipFileName;
                }

                if (job.JobCompleted != null)
                {
                    //from WCF - we dont have to process output, wcf function will anyway call it
                    job.JobCompleted.Set();
                    IdpeKey key = job.DataSource.Key(SreKeyTypes.WcfCallsGenerateStandardOutput);
                    bool boolValue = false;
                    if(key != null)
                        boolValue = key.Value.ParseBool();
                    if (boolValue)
                    {
                        if (!job.AbortRequested)
                        {
                            job.TraceInformation("Calling output writer '{0}'. Total processed rows:{1}", job.DataSource.OutputWriterTypeFullName, job.Rows.Count);
                            Trace.Flush();
                            return job.DataSource.OutputWriter.GetOutput();
                        }
                        else
                        {
                            return new StringBuilder(string.Format("Job '{0}' was aborted! See server log for more details.", job.JobIdentifier));
                        }
                    }
                    else
                    {
                        return new StringBuilder();
                    }
                }
                else
                {
                    if ((job.IsFinished) && (!job.IsErrored) && (job.Errors.Count > 0))
                        new PostMan(job).Send();

                    if ((!job.AbortRequested) && (job.Rows.Count > 0))
                    {
                        job.TraceInformation("Calling output writer '{0}'. Total processed rows:{1}", job.DataSource.OutputWriterTypeFullName, job.Rows.Count);
                        Trace.Flush();
                        job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.OutputWriter);
                        result = job.DataSource.OutputWriter.GetOutput();
                        job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.OutputWriter);
                        return result;
                    }
                    else
                    {
                        return new StringBuilder();
                    }
                }
            }
            catch (BusinessException ex)
            {
                job.TraceInformation(ex.Message);   //It is actually not an exception or error
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);
                return new StringBuilder();
            }
            catch (Exception ex)
            {

                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                job.TraceError(errorMessage);
                Trace.Flush();
                
                if (job != null)
                {
                    job.TraceError("A server exception occurred. Please contact support. ErrorId = '{0}'. Server Time = '{1}'", errorId, DateTime.Now);
                }

                if (job != null)
                {
                    if (job.JobCompleted != null)
                    {
                        //from WCF - we dont have to process output, wcf function will anyway call it                        
                        job.JobCompleted.Set();
                    }
                    job.Dispose();
                }
                return new StringBuilder();
            }
        }

       
        /// <summary>
        /// Process a job already created from data table
        /// </summary>          
        /// <param name="job">The job instance</param>
        /// <returns></returns>
        public StringBuilder ProcessJob(Job job)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                this.JobId = job.JobIdentifier.ToString();
                job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.ExecuteWorkerManager);
                ExecuteWorkerManager(job);
                job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.ExecuteWorkerManager);
                
                job.Errors.AddRange(GetAllAttributeErrors(job.Rows));

                if ((job.IsFinished) && (!job.IsErrored) && (job.Errors.Count > 0))
                    new PostMan(job).Send();

                if ((!job.AbortRequested) && (job.Rows.Count > 0))
                {
                    job.TraceInformation("Calling output writer '{0}'. Total processed rows:{1}", job.DataSource.OutputWriterTypeFullName, job.Rows.Count);
                    Trace.Flush();
                    job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.OutputWriter);
                    result = job.DataSource.OutputWriter.GetOutput();
                    job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.OutputWriter);
                    return result;
                }
                else
                {
                    return new StringBuilder();
                }

            }
            catch (BusinessException ex)
            {
                if (job.IsTriggeredBySqlPuller)
                    new SqlWatcherHelper(job).ExecuteRecoveryScript();

                job.TraceInformation(ex.Message);   //It is actually not an exception or error
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);
                return new StringBuilder();
            }
            catch (Exception ex)
            {
                if (job.IsTriggeredBySqlPuller)
                    new SqlWatcherHelper(job).ExecuteRecoveryScript();

                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                job.TraceError(errorMessage);
                Trace.Flush();

                if (job != null)
                {
                    job.TraceError("A server exception occurred. Please contact support. ErrorId = '{0}'. Server Time = '{1}'", errorId, DateTime.Now);
                }

                if (job != null)
                {
                    if (job.JobCompleted != null)
                    {
                        //from WCF - we dont have to process output, wcf function will anyway call it                        
                        job.JobCompleted.Set();
                    }
                    job.Dispose();
                }
                return new StringBuilder();
            }
        }

        /// <summary>
        /// Initialize a new job, feed MS Excel format data into worker &amp; executes worker
        /// </summary>        
        /// <param name="dataSourceId">Should be a valid application id. If this is filled, then applicationName is optional.</param>
        /// <param name="dataSourceName">Should be a valid application name. If this is filled, then applicationId is optional.</param>        
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like &apos;a126042, Deb'jyoti Das&apos;, let caller decide the user name.</param>
        /// <param name="excelFileName">Excel file name is used for just reference</param>        
        /// <returns>Output xml. for details, read documentation.</returns>
        public StringBuilder ProcessSpreadSheet(int dataSourceId, string dataSourceName, string processingBy, string excelFileName)
        {
            Job job = null;            

            try
            {                
                string error = string.Empty;
                if ((string.IsNullOrEmpty(excelFileName))
                    || (!File.Exists(excelFileName)))
                {
                    //we should never come here, this issue has been taken care in LocalFileSystemWatcher
                    //job.TraceError("File {0} does not exist. Nothing to parse.", excelFileName);
                    return new StringBuilder();
                }


                job = new Job(dataSourceId, dataSourceName, processingBy, excelFileName);
                Registry.Instance.Entries.Add(job.JobIdentifier, job);
                job.FileName = excelFileName;
                this.JobId = job.JobIdentifier.ToString();

                if (!job.IsValid)
                {
                    job.TraceError("Job could not be initialized, data source is not defined. Data source id ={0}, name = '{1}'", dataSourceId, dataSourceName);
                    return new StringBuilder();
                }

                string onlyFileName = Path.GetFileName(job.FileName);
                
                job.Feed();
                if (job.CsvRows.Count == 0)
                {
                    //blank file                    
                    job.TraceInformation(job.ErrorBlankInput);
                    return new StringBuilder();
                }

                ExecuteWorkerManager(job);

                //Get the results from worker (data)
                job.Errors.AddRange(GetAllAttributeErrors(job.Rows));     //get all attribute errors         

                if ((onlyFileName != null)
                    && (onlyFileName.StartsWith(Constants.UnzippedFilePrefix)))
                {
                    //1: orginal file
                    //job.FileName = ZipFileWatcher.ExtractActualFileName(job.FileName);                    
                    File.Delete(excelFileName);//we do not need as zip will be archived

                    //2: zip file (this is important if consumer wants to refer the file (zip file exists)
                    job.FileName = Registry.Instance.ZipFiles[ZipFileWatcher.ExtractUniqueId(onlyFileName)].ZipFileName;
                }

                if (!job.AbortRequested)
                {
                    job.TraceInformation("Calling output writer '{0}'. Total processed rows:{1}", job.DataSource.OutputWriterTypeFullName, job.Rows.Count);
                    Trace.Flush();
                    if (job.Errors.Count > 0)
                        new PostMan(job).Send();
                    return job.DataSource.OutputWriter.GetOutput();
                }

                return new StringBuilder();
            }
            catch (BusinessException ex)
            {
                job.TraceInformation(ex.Message);   //It is actually not an exception or error
                new PostMan(job, false).Send(PostMan.__warningStartTag + ex.Message + PostMan.__warningEndTag);
                return new StringBuilder();
            }
            catch (Exception ex)
            {
                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);                
                job.TraceError("A server exception occurred. Please contact support. ErrorId = '{0}'. Server Time = '{1}'\n {2}", errorId, DateTime.Now, errorMessage);
                Trace.Flush();
                return new StringBuilder();
            }
        }

        public string GetActiveJobs()
        {
            List<string> jobs = new List<string>();
            foreach (DictionaryEntry e in Registry.Instance.Entries)
            {
                Job job = e.Value as Job;
                TimeSpan timeTaken;
                if (!job.IsFinished)
                    timeTaken = DateTime.Now.Subtract(job.StartedAt);
                else
                    timeTaken = job.FinishedAt.Subtract(job.StartedAt);

                double perRow = timeTaken.TotalMilliseconds / job.TotalRowsProcessed;

                jobs.Add(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", e.Key,
                    job.StartedAt, job.TotalRowsToBeProcessed, job.TotalRowsProcessed, timeTaken.ToString(), perRow));
            }            
            Trace.Flush();
            return jobs.ToLine();
        }

        public string GetJobStatus(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
                return string.Empty;
            else if (!Registry.Instance.Entries.Contains(jobId))
                return string.Empty;

            Job job = Registry.Instance.Entries[jobId] as Job;
            TimeSpan timeTaken;
            if (!job.IsFinished)
                timeTaken = DateTime.Now.Subtract(job.StartedAt);
            else
                timeTaken = job.FinishedAt.Subtract(job.StartedAt);

            double perRow = timeTaken.TotalMilliseconds / job.TotalRowsProcessed;

            Trace.Flush();
            return string.Format("{0},{1},{2},{3}", job.TotalRowsToBeProcessed, job.TotalRowsProcessed, timeTaken.ToString(), perRow);
        }

        #region Private Methods

        /// <summary>
        /// Process a new job from data table
        /// </summary>        
        /// <param name="dataSourceId">Should be a valid application id. If this is filled, then datasource name is optional.</param>
        /// <param name="dataSourceName">Should be a valid datasource name. If this is filled, then datasource id is optional.</param>        
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like &apos;a126042, Deb'jyoti Das&apos;, let caller decide the user name.</param>      
        /// <returns></returns>
        static internal Job Instantiate(int dataSourceId, string dataSourceName, string processingBy)
        {
            Job job = null;
            try
            {
                string error = string.Empty;
                StringBuilder result = new StringBuilder();

                job = new Job(dataSourceId, dataSourceName, processingBy);
                job.FileName = string.Format("{0} Data Set", dataSourceId);
                if (job.Errors.Count > 0)
                    throw new BusinessException(job.Errors.ToLine("<br />"));

                Registry.Instance.Entries.Add(job.JobIdentifier, job);


                job.PerformanceCounter.Start(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                job.IsWCFRequest = false;               
                if (!job.IsValid)
                {
                    error = string.Format("Job could not be initialized, application is not defined. Application id ={0}, name = '{1}'", dataSourceId, dataSourceName);
                    job.TraceInformation(error);

                    job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                    return null;
                }
                job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                return job;

            }
            catch (Exception ex)
            {

                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                job.TraceError("Could not instantiate a new job!{0}{1}", Environment.NewLine, errorMessage);
                Trace.Flush();
                job.PerformanceCounter.Stop(job.JobIdentifier, JobPerformanceTaskNames.JobInit);
                return null;
            }
        }

        void ExecuteWorkerManager(Job job)
        {

            eventDone.Reset();
            Dictionary<string, object> inputs = new Dictionary<string, object>();            
            inputs.Add("Job", job);

            WorkflowApplication workflowApp = new WorkflowApplication(new WorkerManager(), inputs);
            workflowApp.Completed = WorkerManagerCompleted;
            workflowApp.Aborted = WorkerManagerAborted;
            workflowApp.Idle = WorkerManagerIdle;
            workflowApp.PersistableIdle = WorkerManagerPersistableIdle;
            workflowApp.Unloaded = WorkerManagerUnloaded;
            workflowApp.OnUnhandledException = WorkerManagerUnhandledException;
            workflowApp.Run();
            eventDone.WaitOne();

            job.IsFinished = true;
        }

        ManualResetEvent eventDone = new ManualResetEvent(false);
        void WorkerManagerCompleted(WorkflowApplicationCompletedEventArgs e)
        {
            if (e.CompletionState == ActivityInstanceState.Faulted)
            {
                ExtensionMethods.TraceInformation(string.Format("WorkerManager {0} Terminated.", e.InstanceId.ToString()));
                ExtensionMethods.TraceInformation(string.Format("Exception: {0}\n{1}",
                    e.TerminationException.GetType().FullName,
                    e.TerminationException.Message));
            }
            else if (e.CompletionState == ActivityInstanceState.Canceled)
            {
                ExtensionMethods.TraceInformation("WorkerManager Canceled!");
            }

            eventDone.Set();
        }


        void WorkerManagerAborted(WorkflowApplicationAbortedEventArgs e)
        {
            ExtensionMethods.TraceError("WorkerManager {0} Aborted.", e.InstanceId);
            ExtensionMethods.TraceError("Exception: {0}\n{1}",
                e.Reason.GetType().FullName,
                e.Reason.Message);
            Trace.Flush();
            eventDone.Set();            
        }

        void WorkerManagerIdle(WorkflowApplicationIdleEventArgs e)
        {
            ExtensionMethods.TraceError("WorkerManager {0} Idle.", e.InstanceId);
            Trace.Flush();
        }

        PersistableIdleAction WorkerManagerPersistableIdle(WorkflowApplicationIdleEventArgs e)
        {
            ExtensionMethods.TraceError("WorkerManager {0} Idle.", e.InstanceId);
            Trace.Flush();
            return PersistableIdleAction.Unload;
        }

        void WorkerManagerUnloaded(WorkflowApplicationEventArgs e)
        {
            //ExtensionMethods.TraceInformation("WorkerManager '{0}' unloaded.", e.InstanceId);
            //Trace.Flush();
        }

        UnhandledExceptionAction WorkerManagerUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs e)
        {
            ExtensionMethods.TraceError("OnUnhandledException in WorkerManager {0}\n{1}",
                e.InstanceId, e.UnhandledException.Message);
            Trace.Flush();

            return UnhandledExceptionAction.Terminate;
        }
      

        List<string> GetAllAttributeErrors(List<Row> rows)
        {
            List<string> allErrors = new List<string>();
            foreach (var row in rows)
            {
                if (row == null) break;
                foreach (var col in row.Columns)
                {
                    if ((col.Error.Code != SreMessageCodes.SRE_FAILED_BLANK) 
                        && (col.Error.Message != string.Empty))
                        allErrors.Add(col.Error.Message);
                }
                foreach (var col in row.ColumnsSystem)
                {
                    if ((col.Error.Code != SreMessageCodes.SRE_FAILED_BLANK)
                        && (col.Error.Message != string.Empty))
                        allErrors.Add(col.Error.Message);
                }
            }
            return allErrors;            
        }

        #endregion Private Methods

        #region IDisposable Members

        public void Dispose()
        {
            this.eventDone.Dispose();

            if (string.IsNullOrEmpty(this.JobId))   //in case of zip/tar file
                return;

            if (Registry.Instance.Entries.ContainsKey(this.JobId))
            {
                Job job = Registry.Instance.Entries[this.JobId] as Job;
                if (!job.IsWCFRequest)
                {
                    //If job is not WCF request, lets dispose everthing, else WCF interface will call dispose
                    //once it calls outputwriter
                    job.Dispose();
                    RemoveJobFromRegistry(this.JobId);
                }
            }
        }

        #endregion
    }

    
}





