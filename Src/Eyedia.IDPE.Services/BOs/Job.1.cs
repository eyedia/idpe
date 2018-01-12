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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Threading;


namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Job stores information of new job. Every job has a unique identifier. 
    /// A job instance used by WorkerManager
    /// WorkerManager assignes job slices to multiple Workers
    /// </summary>
    public partial class Job : IDisposable
    {

        /// <summary>
        /// Instantiate new job with a data source(id OR name to be passed)
        /// </summary>
        /// <param name="dataSourceId">The data source id (id OR name is required)</param>
        /// <param name="dataSourceName">The data source name (id OR name is required)</param>
        /// <param name="processingBy">Processing by user name</param>
        /// <param name="fileName">The file name</param>
        public Job(int dataSourceId, string dataSourceName, string processingBy, string fileName = null)
        {

            this.ProcessingBy = processingBy;
            this.FileName = fileName;
            this.Rows = new List<Row>();        //these list are in this case thread OK, because we add from individual splitted job return items
            this.Errors = new List<string>();
            this.Warnings = new List<string>();
            this.BadDataInCsvFormat = new List<string>();
            this.StartedAt = DateTime.Now;
            this.JobIdentifier = ShortGuid.NewGuid();
            this.ProcessVariables = new ConcurrentDictionary<string, object>();
            this.CsvRows = new List<string>();
            this.PerformanceCounter = new PerformanceCounter();
            PerformanceCounter.StartNew(JobIdentifier);
            if ((dataSourceId == 0) && (string.IsNullOrEmpty(dataSourceName)))
            {
                return;//Invalid dummy job;
            }


            this.DataSource = new DataSource(dataSourceId, dataSourceName);
            this.DataSource.ClearAdditionalAttachments();

            if (!this.DataSource.IsValid)
            {
                string errorMessage = string.Format("Could not create job as data source was not valid. Data source id was {0} and name was {1}"
                    , dataSourceId, string.IsNullOrEmpty(dataSourceName) ? "<Unknown>" : dataSourceName);
                this.DataSource.TraceError(errorMessage);
                return; //we need not to do anything here as job is automatically invalid
            }

            #region OutputWriter
            object objOutputWriter = null;
            if (DataSource.OutputType == OutputTypes.Xml)
            {
                //set default                
                objOutputWriter = new OutputWriterGeneric(this);

            }
            else if (DataSource.OutputType == OutputTypes.Delimited)
            {
                objOutputWriter = new OutputWriterDelimited(this);
            }
            else if (DataSource.OutputType == OutputTypes.FixedLength)
            {
                objOutputWriter = new OutputWriterFixedLength(this);
            }
            else if (DataSource.OutputType == OutputTypes.CSharpCode)
            {
                objOutputWriter = new OutputWriterCSharpCode(this);
            }
            else if (DataSource.OutputType == OutputTypes.Database)
            {                
                objOutputWriter = new OutputWriterDatabase(this);
            }
            else if (DataSource.OutputType == OutputTypes.Custom)
            {
                if ((!string.IsNullOrEmpty(this.DataSource.OutputWriterTypeFullName))
                    && (Type.GetType(this.DataSource.OutputWriterTypeFullName) != null))
                {
                    objOutputWriter = Activator.CreateInstance(Type.GetType(this.DataSource.OutputWriterTypeFullName), this);
                }
                else
                {
                    objOutputWriter = new OutputWriterGeneric(this);
                }
            }
            else
            {
                objOutputWriter = new OutputWriterGeneric(this);
            }

            this.DataSource.OutputWriter = (OutputWriter)objOutputWriter;
            #endregion OutputWriter

            #region PlugIns

            if ((!string.IsNullOrEmpty(this.DataSource.PlugInsType))
                && (Type.GetType(this.DataSource.PlugInsType) != null))
            {

                object objPlugIns = Activator.CreateInstance(Type.GetType(this.DataSource.PlugInsType), this);
                this.DataSource.PlugIns = (PlugIns)objPlugIns;
            }
            else
            {
                //set default
                object objPlugIns = Activator.CreateInstance(Type.GetType("Eyedia.IDPE.Services.PlugInsGeneric"), this);
                this.DataSource.PlugIns = (PlugIns)objPlugIns;
            }

            #endregion PlugIns

     
            SreKey key = DataSource.Key(SreKeyTypes.IsFirstRowHeader);
            if (key != null)
                DataSource.IsFirstRowHeader = key.Value.ParseBool();

            if ((DataSource.DataFormatType == DataFormatTypes.Delimited)
                || (DataSource.DataFormatType == DataFormatTypes.FixedLength))
                ExtractHeaderFooter();


            this.SqlClientManager = new SqlClientManager(this.DefaultConnectionString, this.DefaultConnectionType);
            this.Parameters = new Parameters(this.DataSource.Id, this.DataSource.Name, this.DataSource.Key(SreKeyTypes.GenerateParametersFromDatabase), this.SqlClientManager);

            ExtensionMethods.TraceInformation(Environment.NewLine);
            this.TraceInformation("New job '{0}' created.", this.JobIdentifier);
        }

        List<JobSlice> _JobSlices;

        /// <summary>
        /// Returns read only collection of job slices
        /// </summary>
        public ReadOnlyCollection<JobSlice> JobSlices
        {
            get
            {
                if (_JobSlices != null)
                    return _JobSlices.AsReadOnly();
                else
                    return new ReadOnlyCollection<JobSlice>(new List<JobSlice>());
            }
        }


        /// <summary>
        /// Returns number of job slices getting processed (Active workers)
        /// </summary>
        public int NumberOfSlicesProcessing
        {
            get
            {
                return JobSlices.Where(js => js.Status == JobSlice.JobSliceStatus.Processing).Count();
            }
        }

        /// <summary>
        /// Returns number of job slices completed
        /// </summary>
        public int NumberOfSlicesProcessed
        {
            get
            {
                return JobSlices.Where(js => js.Status == JobSlice.JobSliceStatus.Processed).Count();
            }
        }

        /// <summary>
        /// Feeds data into the job from an xml or CSV or FL or file name (in case of Excel or custom feeder)
        /// </summary>
        /// <param name="inputData">The parsed data table</param>
        /// <returns></returns>
        public List<string> Feed(DataTable inputData = null)
        {
            List<string> errors = new List<string>();
            if (inputData == null)
            {
                PerformanceCounter.Start(JobIdentifier, JobPerformanceTaskNames.FeedData);
                AnyToDataTable anyToDataTable = new AnyToDataTable(this);
                errors = anyToDataTable.Feed();
                PerformanceCounter.Stop(JobIdentifier, JobPerformanceTaskNames.FeedData);
            }
            else
            {
                InputData = inputData;
                CsvRows = new List<string>();
                if (DataSource.IsFirstRowHeader)
                {
                    var columnNames = inputData.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                    CsvRows.Add(string.Join(",", columnNames));
                }
                foreach (DataRow row in inputData.Rows)
                {
                    var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    CsvRows.Add("\"" + string.Join("\",\"", fields) + "\"");
                }


                if ((DataSource.IsFirstRowHeader) && (CsvRows.Count > 0))
                    BadDataInCsvFormat.Add(CsvRows[0]); //storing header information
                ColumnCount = inputData.Columns.Count;
            }

            if (this.InputData.Rows.Count == 0)
                return new List<string>();

            PerformanceCounter.Start(JobIdentifier, JobPerformanceTaskNames.SliceData);
            if (!AddAdditionalColumns())
                return new List<string>();

            this.TotalRowsToBeProcessed = this.InputData.Rows.Count;
            int skipHeaderInCsvRows = this.DataSource.IsFirstRowHeader ? 1 : 0;

            int recordsPerThread = GetRecordsPerThread();            
            this.TraceInformation("Slicing jobs with '{0}' records per thread.", recordsPerThread);
            Trace.Flush();
            List<DataTable> chunks = InputData.Split(recordsPerThread);
            this.TraceInformation("Job sliced. Total = {0}.", chunks.Count);
            Trace.Flush();
            _JobSlices = new List<JobSlice>();
            int counter = 0;
            foreach (DataTable table in chunks)
            {              
                JobSlice jobSlice = new JobSlice(this.JobIdentifier, counter, table, this.CsvRows.Skip(skipHeaderInCsvRows).Skip(counter * recordsPerThread).Take(recordsPerThread).ToList());
                _JobSlices.Add(jobSlice);
                counter++;
            }
            this.TraceInformation("All slices are initialized.");
            Trace.Flush();
            PerformanceCounter.Stop(JobIdentifier, JobPerformanceTaskNames.SliceData);
            return errors;
        }

        private int GetRecordsPerThread()
        {
            int recordsPerThread = EyediaCoreConfigurationSection.CurrentConfig.RecordsPerThread;
            if (EyediaCoreConfigurationSection.CurrentConfig.AutoRecordsPerThread)
            {
                recordsPerThread = InputData.Rows.Count / Environment.ProcessorCount;
                if (recordsPerThread < 3)
                    recordsPerThread = 3;
                this.TraceInformation("Auto records per thread was set! Records per thread was set to {0} based on {1} logical processors", recordsPerThread, Environment.ProcessorCount);
            }
            return recordsPerThread;
        }

        /// <summary>
        /// Feeds data into the job from a list objects
        /// </summary>
        /// <param name="inputData">List of objects</param>
        ///<param name="overridenMapping">Comma separated additional mapping override information. For example if "EmpId" from object to be mapped with "EmployeeId" of attribute, then "EmpId=EmployeeId,Ename=EmployeeName"</param>
        /// <returns></returns>
        public List<string> Feed(List<Object> inputData, string overridenMapping)
        {
            this.DataFedFrom = inputData;
            AnyToDataTable anyToDataTable = new AnyToDataTable(this);
            List<string> errors = anyToDataTable.Feed(inputData, overridenMapping);            
            this.TotalRowsToBeProcessed = this.InputData.Rows.Count;
            int skipHeaderInCsvRows = this.DataSource.IsFirstRowHeader ? 1 : 0;

            int recordsPerThread = GetRecordsPerThread();

            List<DataTable> chunks = InputData.Split(recordsPerThread);
            _JobSlices = new List<JobSlice>();
            int counter = 0;
            foreach (DataTable table in chunks)
            {
                JobSlice jobSlice = new JobSlice(this.JobIdentifier, counter, table, this.CsvRows.Skip(skipHeaderInCsvRows).Skip(counter * recordsPerThread).Take(recordsPerThread).ToList());
                _JobSlices.Add(jobSlice);
                counter++;
            }
            return errors;
        }


        /// <summary>
        /// generates output from its data source and returns string
        /// </summary>
        /// <param name="errors">In case of any error</param>
        /// <param name="warnings">In case of any warning</param>
        /// <param name="execption">In case of any execption and is available</param>
        /// <returns>output as StringBuilder</returns>
        public StringBuilder GetOutput(string errors = null, string warnings = null, Exception execption = null)
        {
            if (!this.IsFinished)
                this.Errors.Add(string.Format("The job '{0}' has not finished yet.", this.JobIdentifier));

            if (!(string.IsNullOrEmpty(errors)))
                this.Errors.Add(errors);

            if (!(string.IsNullOrEmpty(warnings)))
                this.Warnings.Add(warnings);

            if ((this.DataSource != null) && (this.DataSource.OutputWriter != null))
            {
                return this.DataSource.OutputWriter.GetOutput();
            }
            else
            {
                this.DataSource.TraceError("Can not generate output as output writer does not exists or it is yet to be instantiated!"
                    + Environment.NewLine + execption.ToString());
                return new StringBuilder();
            }
        }

        /// <summary>
        /// generates output from its data source and returns string
        /// </summary>
        /// <param name="errors">In case of any error</param>
        /// <param name="warnings">In case of any warning</param>
        /// <param name="execption">In case of any execption and is available</param>
        /// <returns>output as object</returns>
        public object GetCustomOutput(string errors = null, string warnings = null, Exception execption = null)
        {
            if (!this.IsFinished)
                this.Errors.Add(string.Format("The job '{0}' has not finished yet.", this.JobIdentifier));


            if (!(string.IsNullOrEmpty(errors)))
                this.Errors.Add(errors);

            if (!(string.IsNullOrEmpty(warnings)))
                this.Warnings.Add(warnings);


            if ((this.DataSource != null) && (this.DataSource.OutputWriter != null))
            {
                return this.DataSource.OutputWriter.GetCustomOutput();
            }
            else
            {
                this.DataSource.TraceError("Can not generate output as output writer does not exists or it is yet to be instantiated!"
                    + Environment.NewLine + (execption == null ? string.Empty : execption.ToString()));
                return new object();
            }
        }

        private void FinishJob()
        {
            this.InputData.Dispose();
            this.Rows = this.Rows.OrderBy(r => r.OriginalRowPosition).ToList();
        }

        private bool AddAdditionalColumns()
        {
            if (InputData == null)
                return false;

            DataTable dtIncremented = new DataTable(InputData.TableName);

            DataColumn dc = new DataColumn(Constants.ColumnNamePosition);
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.AutoIncrementStep = 1;
            dc.DataType = typeof(Int32);
            dtIncremented.Columns.Add(dc);

            dc = new DataColumn(Constants.ColumnNameContainerError);
            dc.DataType = typeof(String);
            dtIncremented.Columns.Add(dc);

            dtIncremented.BeginLoadData();
            DataTableReader dtReader = new DataTableReader(InputData);
            dtIncremented.Load(dtReader);
            dtIncremented.EndLoadData();

            InputData = dtIncremented;

            return true;
        }

        /// <summary>
        /// Adds container error (generally to be called from ContainerValidator.Validate()) to a specific record
        /// </summary>
        /// <param name="position">Record position, starts from 0(zero)</param>
        /// <param name="errorMessage">The business error message</param>
        public void AddContainerError(int position, string errorMessage)
        {
            //this.InputData.Rows[position][Constants.ColumnNameContainerError] = errorMessage;

            position++; //internally we start from 1
            foreach (JobSlice jobSlice in JobSlices)
            {
                foreach (DataRow row in jobSlice.InputData.Rows)
                {
                    if (row[Constants.ColumnNamePosition].ToString() == position.ToString())
                    {
                        row[Constants.ColumnNameContainerError] = errorMessage;
                    }
                }
            }
        }

        public DataTable InputData { get; set; }
        public List<string> CsvRows { get; set; }


        /// <summary>
        /// CSV Format: InputData.Columns.Count will always return number of columns in schema, even if we actually receive less columns
        /// ColumnCount always returns the original column count from the file
        /// FixedLength: Return max row width
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Unique ideentifier of current job
        /// </summary>
        public string JobIdentifier { get; private set; }

        /// <summary>
        /// User name, who has initialized the job, (who can request for job result)
        /// </summary>
        public string ProcessingBy { get; private set; }

        /// <summary>
        /// The data source for which job was created
        /// </summary>
        public DataSource DataSource { get; private set; }

        /// <summary>
        /// If pre-validation rule failed, it will be true
        /// </summary>
        public bool PreValidationRuleFailed { get; set; }

        /// <summary>
        /// Returns true if data source is valid (which means a valid data found for a valid data source)
        /// </summary>
        public bool IsValid
        {
            get { return DataSource == null ? false : DataSource.IsValid; }
        }

        /// <summary>
        /// If job processing was failed, this will return true
        /// </summary>
        public bool IsErrored { get; internal set; }

        /// <summary>
        /// If job was forcefully failed from pusher
        /// </summary>
        public bool ErroredByPusher { get; set; }

        /// <summary>
        /// If job was aborted, this will return true
        /// </summary>
        public bool IsAborted { get; internal set; }

        /// <summary>
        /// Set this to true if you want to stop workflow while job is being processed. If job is already aborted/finished this will not have any effect
        /// </summary>
        public bool AbortRequested { get; set; }

        public enum AbortReasons
        {
            BusinessRuleFailed,
            TimedOut            
        }

        public AbortReasons AbortReason { get; set; }

        public List<Row> Rows { get; private set; }
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }

        /// <summary>
        /// Pullers set it to true in case the job actually getting processed from WCF
        /// </summary>
        public bool IsWCFRequest { get; set; }

        /// <summary>
        /// If the job was triggered by Sql puller
        /// </summary>
        public bool IsTriggeredBySqlPuller { get; set; }

        public Parameters Parameters { get; private set; }
        public PerformanceCounter PerformanceCounter { get; private set; }
        public SqlClientManager SqlClientManager { get; private set; }

        string _DefaultConnectionString;
        public string DefaultConnectionString
        {
            get
            {

                if ((string.IsNullOrEmpty(_DefaultConnectionString))
                    && (this.DataSource.Keys != null))
                {
                    foreach (SreKey key in this.DataSource.Keys)
                    {
                        SreKeyTypes keyType = (SreKeyTypes)key.Type;
                        if (keyType.IsConnectionStringType())
                        {
                            _DefaultConnectionString = key.Value;
                            _DefaultConnectionStringName = key.Name;
                            _DefaultConnectionType = keyType;
                            break;
                        }
                    }

                }
                return _DefaultConnectionString;
            }
        }

        public string _DefaultConnectionStringName;
        public string DefaultConnectionStringName
        {
            get
            {
                string str = string.Empty;
                if (string.IsNullOrEmpty(_DefaultConnectionString))
                    str = _DefaultConnectionString;
                return _DefaultConnectionStringName;
            }
        }
        SreKeyTypes _DefaultConnectionType;

        public SreKeyTypes DefaultConnectionType
        {
            get
            {
                string str = string.Empty;
                if (string.IsNullOrEmpty(_DefaultConnectionString))
                    str = _DefaultConnectionString;
                return _DefaultConnectionType;
            }
        }

        /// <summary>
        /// Returns bad data in csv file format
        /// </summary>
        public List<string> BadDataInCsvFormat { get; internal set; }
        public DateTime StartedAt { get; private set; }
        public DateTime FinishedAt { get; internal set; }
        public bool KeepInMemoryUntilTimeOut { get; set; }

        bool _IsFinished;

        /// <summary>
        /// This will return true if job is processed
        /// </summary>
        public bool IsFinished
        {
            get { return _IsFinished; }
            internal set
            {
                if (value == false) throw new Exception("Only true can be set to this property.");
                this._IsFinished = true;
                this.FinishedAt = DateTime.Now;
                FinishJob();
            }
        }

        /// <summary>
        /// Total number of rows to be processed
        /// </summary>
        public int TotalRowsToBeProcessed { get; set; }

        /// <summary>
        /// Total rows processed (till now)
        /// </summary>
        public int TotalRowsProcessed { get; set; }


        /// <summary>
        /// Returns valid rows
        /// </summary>
        public int TotalValid
        {
            get
            {
                int count = this.Rows.Where(r => r.ColumnsSystem["IsValid"].Value.ParseBool() == true).Count();
                return count;
            }
        }

        /// <summary>
        /// Input file name if input fed from a file
        /// </summary>
        public string FileName { get; internal set; }

        /// <summary>
        /// Input file name only (without path) if input fed from a file
        /// </summary>
        public string FileNameOnly { get { return Path.GetFileName(FileName); } }

        /// <summary>
        /// File content when required
        /// </summary>
        public StringBuilder FileContent
        {
            get
            {
                ExtensionMethods.TraceInformation("File content called!");
                string localFileName = FileName;
                if (IsHavingSpecialHeaderAndOrFooter)
                    localFileName = FileNameWithoutHeaderAndOrFooter;

                if (string.IsNullOrEmpty(localFileName))
                {
                    return new StringBuilder();
                }
                else if (File.Exists(localFileName))
                {
                    using (StreamReader sr = new StreamReader(localFileName))
                    {
                        StringBuilder sb = new StringBuilder(sr.ReadToEnd());
                        sr.Close();
                        return sb;
                    }
                }
                else
                {
                    return new StringBuilder();
                }
            }
        }

        public bool IsHavingSpecialHeaderAndOrFooter { get { return !string.IsNullOrEmpty(FileNameWithoutHeaderAndOrFooter); } }
        public string FileNameWithoutHeaderAndOrFooter { get; private set; }       

        private void ExtractHeaderFooter()
        {
            SreKey headerAttribute = DataSource.Key(SreKeyTypes.HeaderLine1Attribute);
            SreKey footerAttribute = DataSource.Key(SreKeyTypes.FooterLine1Attribute);

            if ((headerAttribute == null)
                && (footerAttribute == null))                
                return;

            headerAttribute = null;
            footerAttribute = null;

            try
            {                
                string[] allLines = FileContent.ToString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string line = string.Empty;

                int howManyFound = 0;
                for (int i = 1; i <= 6; i++)
                {
                    SreKeyTypes keyType = (SreKeyTypes)Enum.Parse(typeof(SreKeyTypes), "HeaderLine" + i + "Attribute");
                    headerAttribute = DataSource.Key(keyType);
                    if (headerAttribute != null)
                    {
                        line = allLines[i - 1];
                        ProcessVariables.AddOrUpdate(headerAttribute.Value, line, (key, oldValue) => line);
                    }
                    else
                    {
                        break;
                    }

                    howManyFound++;
                }

                allLines = allLines.SubArray(howManyFound, allLines.Length - howManyFound);

                howManyFound = 0;
                for (int i = 6; i >= 1; i--)
                {
                    SreKeyTypes keyType = (SreKeyTypes)Enum.Parse(typeof(SreKeyTypes), "FooterLine" + i + "Attribute");
                    footerAttribute = DataSource.Key(keyType);
                    if (footerAttribute == null)
                    {
                        continue;
                    }
                    else
                    {
                        line = allLines[allLines.Length - (howManyFound + 1)];
                        ProcessVariables.AddOrUpdate(footerAttribute.Value, line, (key, oldValue) => line);
                    }

                    howManyFound++;
                }

                allLines = allLines.SubArray(0, allLines.Length - howManyFound);

                FileNameWithoutHeaderAndOrFooter = FileName + ShortGuid.NewGuid().ToString();
                using (StreamWriter sw = new StreamWriter(FileNameWithoutHeaderAndOrFooter))
                {
                    for (int i = 0; i < allLines.Length; i++)
                    {
                        sw.WriteLine(allLines[i]);
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceInformation(ex.ToString());
                this.Errors.Add("File content is invalid! Please check that file has required header(s) and footer(s) and at least 1 valid record!");
            }            
        }


        /// <summary>
        /// Input file extension if input fed from a file
        /// </summary>
        public string FileExtension
        {
            get
            {
                if ((!string.IsNullOrEmpty(FileName))
                    && (File.Exists(FileName)))
                    return Path.GetExtension(FileName);

                return string.Empty;
            }
        }

        /// <summary>
        /// Common dictionary to store any object during the process
        /// </summary>
        public ConcurrentDictionary<string, object> ProcessVariables { get; internal set; }

        internal readonly object _lock = new object();


        /// <summary>
        /// In case data Fed from list of custom objects, this will contain the original object
        /// </summary>
        public List<object> DataFedFrom { get; private set; }

        public ManualResetEvent JobCompleted { get; internal set; }

        public string ErrorBlankInput
        {
            get
            {
                if ((this.DataSource != null)
                    && (this.DataSource.IsFirstRowHeader == false))
                    return string.Format("System expects at least one request. The input should at least 1 record(line)", this.DataSource.Name);
                else if ((this.DataSource != null)
                    && (this.DataSource.IsFirstRowHeader))
                    return string.Format("System expects at least one request. As {0} expects header, the input should contain header and at least 1 record(line)", this.DataSource.Name);
                else
                    return "System expects at least one request. If the expected format has 'HEADER' and 'FOOTER', please make sure that the request contains 'HEADER', at least 1 record, along with 'FOOTER'";
            }
        }

        public ReadOnlyCollection<SymplusCodeSet> CodeSets { get { return Registry.Instance.CodeSets.AsReadOnly(); } }

        #region IDisposable Members

        public void Dispose()
        {
            if (InputData != null)
                InputData.Dispose();

            if (!(string.IsNullOrEmpty(FileNameWithoutHeaderAndOrFooter))
                && (File.Exists(FileNameWithoutHeaderAndOrFooter)))
                File.Delete(FileNameWithoutHeaderAndOrFooter);
            try
            {
                if (JobSlices != null)
                {
                    foreach (JobSlice jobSlice in JobSlices)
                    {
                        jobSlice.Dispose();
                    }
                }

            }
            catch { }


            if (this.Rows != null)
            {
                foreach (Row row in this.Rows)
                {
                    row.Dispose();
                }
            }
            if (this.SqlClientManager != null)
                this.SqlClientManager.Dispose();

            this._DefaultConnectionString = null;
            this._DefaultConnectionStringName = null;
            this._JobSlices = null;
            this.BadDataInCsvFormat = null;
            this.ProcessVariables = null;
            this.DataFedFrom = null;
            Trace.Flush();
            if (this.DataSource != null)
                this.DataSource.Dispose();

            //ExtensionMethods.TraceInformation("Memory used before collection:{0:N0}", GC.GetTotalMemory(false));
            GC.Collect();
            //ExtensionMethods.TraceInformation("Memory used after full collection:{0:N0}", GC.GetTotalMemory(true));

        }

        #endregion
    }
    
}






