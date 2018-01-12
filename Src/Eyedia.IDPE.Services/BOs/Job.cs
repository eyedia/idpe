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
using Symplus.Core;
using Symplus.Core.Data;
using Symplus.RuleEngine.Common;
using Symplus.RuleEngine.DataManager;
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Threading;


namespace Symplus.RuleEngine.Services
{
    /// <summary>
    /// Job stores information of new job. Every job has a unique identifier. 
    /// A job instance used by WorkerManager
    /// WorkerManager assignes job slices to multiple Workers
    /// </summary>
	public class Job : IDisposable
	{
        
        /// <summary>
        /// Instantiate new job with a data source(id OR name to be passed)
        /// </summary>
        /// <param name="dataSourceId">The data source id (id OR name is required)</param>
        /// <param name="dataSourceName">The data source name (id OR name is required)</param>
        /// <param name="processingBy">Processing by user name</param>
        public Job(int dataSourceId, string dataSourceName, string processingBy)
        {           
            
            this.ProcessingBy = processingBy;
            this.Rows = new List<Row>();        //these list are in this case thread OK, because we add from individual splitted job return items
            this.Errors = new List<string>();
            this.Warnings = new List<string>();
            this.BadDataInCSVFormat = new List<string>();
            this.StartedAt = DateTime.Now;
            this.JobIdentifier = ShortGuid.NewGuid();
            this.ContainerData = new ConcurrentDictionary<string, object>();            
            this.CSVRows = new List<string>();            
            this.PerformanceCounter = new PerformanceCounter();

            if ((dataSourceId == 0) && (string.IsNullOrEmpty(dataSourceName)))
            {
                return;//Invalid dummy job;
            }
            

            this.DataSource = new DataSource(dataSourceId, dataSourceName);

            if (!this.DataSource.IsValid)
            {
                string errorMessage = string.Format("Could not create job as data source was not valid. Data source id was {0} and name was {1}"
                    , dataSourceId, string.IsNullOrEmpty(dataSourceName)? "<Unknown>" : dataSourceName);
                Trace.TraceError(errorMessage);
                return; //we need not to do anything here as job is automatically invalid
            }
            if ((!string.IsNullOrEmpty(this.DataSource.OutputWriterType))
                && (Type.GetType(this.DataSource.OutputWriterType) != null))
            {
                object objOutputWriter = Activator.CreateInstance(Type.GetType(this.DataSource.OutputWriterType), this);
                this.DataSource.OutputWriter = (OutputWriter)objOutputWriter;
            }
            else
            {
                //set default                
                object objOutputWriter = Activator.CreateInstance(Type.GetType("Symplus.RuleEngine.Services.OutputWriterGeneric"), this);
                this.DataSource.OutputWriter = (OutputWriter)objOutputWriter;
            }

            if ((!string.IsNullOrEmpty(this.DataSource.DataContainerValidatorType))
                && (Type.GetType(this.DataSource.DataContainerValidatorType) != null))
            {

                object objDataContainerValidator = Activator.CreateInstance(Type.GetType(this.DataSource.DataContainerValidatorType), this);
                this.DataSource.DataContainerValidator = (DataContainerValidator)objDataContainerValidator;
            }
            else
            {
                //set default
                object objDataContainerValidator = Activator.CreateInstance(Type.GetType("Symplus.RuleEngine.Services.DataContainerValidatorGeneric"), this);
                this.DataSource.DataContainerValidator = (DataContainerValidator)objDataContainerValidator;
            }

            if ((!string.IsNullOrEmpty(this.DataSource.PlugInsType))
                && (Type.GetType(this.DataSource.PlugInsType) != null))
            {

                object objPlugIns = Activator.CreateInstance(Type.GetType(this.DataSource.PlugInsType), this);
                this.DataSource.PlugIns = (PlugIns)objPlugIns;
            }
            else
            {
                //set default
                object objPlugIns = Activator.CreateInstance(Type.GetType("Symplus.RuleEngine.Services.PlugInsGeneric"), this);
                this.DataSource.PlugIns = (PlugIns)objPlugIns;
            }

            
            this.SqlClientManager = new SQLClientManager(this.DefaultConnectionString, this.DefaultConnectionType);
            this.Parameters = new Parameters(this.DataSource.Id, this.DataSource.Name, this.DataSource.Key(SreKeyTypes.GenerateParametersFromDatabase), this.SqlClientManager);

            Trace.TraceInformation(Environment.NewLine);
            Trace.TraceInformation("New job '{0}' created.", this.JobIdentifier);
        }

        List<JobSlice> _JobSlices;

        /// <summary>
        /// Returns read only collection of job slices
        /// </summary>
        public ReadOnlyCollection<JobSlice> JobSlices
        {
            get
            {
                return _JobSlices.AsReadOnly();
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
        /// <param name="xmlOrCSVOrFixedLengthOrFileName"></param>
        /// <returns></returns>
        public List<string> Feed(string xmlOrCSVOrFixedLengthOrFileName)
        {
            AnyToDataTable anyToDataTable = new AnyToDataTable(this);
            List<string> errors = anyToDataTable.Feed(xmlOrCSVOrFixedLengthOrFileName);
            this.DataSource.IsFirstRowHeader = anyToDataTable.IsFirstRowHeader;

            if (!AddAdditionalColumns())
                return new List<string>();

            this.TotalRowsToBeProcessed = this.InputData.Rows.Count;
            int skipHeaderInCsvRows = this.DataSource.IsFirstRowHeader ? 1 : 0;
           
            List<DataTable> chunks = InputData.Split(SymplusCoreConfigurationSection.CurrentConfig.RecordsPerThread);
            _JobSlices = new List<JobSlice>();
            int counter = 0;
            foreach (DataTable table in chunks)
            {
                JobSlice jobSlice = new JobSlice(this.JobIdentifier, counter, table, this.CSVRows.Skip(skipHeaderInCsvRows).Skip(counter * SymplusCoreConfigurationSection.CurrentConfig.RecordsPerThread).Take(SymplusCoreConfigurationSection.CurrentConfig.RecordsPerThread).ToList());
                _JobSlices.Add(jobSlice);
                counter++;
            }
            return errors;
        }

        /// <summary>
        /// Feeds data into the job from a list objects
        /// </summary>
        /// <param name="inputData">List of objects</param>
        ///<param name="overridenMapping">Comma separated additional mapping override information. For example if "EmpId" from object to be mapped with "EmployeeId" of attribute, then "EmpId=EmployeeId,Ename=EmployeeName"</param>
        /// <returns></returns>
        public List<string> Feed(List<Object> inputData, string overridenMapping)
        {            
            this.DataFeededFrom = inputData;
            AnyToDataTable anyToDataTable = new AnyToDataTable(this);
            List<string> errors = anyToDataTable.Feed(inputData, overridenMapping);
            this.DataSource.IsFirstRowHeader = anyToDataTable.IsFirstRowHeader;            
            this.TotalRowsToBeProcessed = this.InputData.Rows.Count;
            int skipHeaderInCsvRows = this.DataSource.IsFirstRowHeader ? 1 : 0;
            
            List<DataTable> chunks = InputData.Split(SymplusCoreConfigurationSection.CurrentConfig.RecordsPerThread);
            _JobSlices = new List<JobSlice>();
            int counter = 0;
            foreach (DataTable table in chunks)
            {
                JobSlice jobSlice = new JobSlice(this.JobIdentifier, counter, table, this.CSVRows.Skip(skipHeaderInCsvRows).Skip(counter * SymplusCoreConfigurationSection.CurrentConfig.RecordsPerThread).Take(SymplusCoreConfigurationSection.CurrentConfig.RecordsPerThread).ToList());
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
        /// <returns>output as string</returns>
        public string GetOutput(string errors = null, string warnings = null, Exception execption = null)
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
                Trace.TraceError("Can not generate output as output writer does not exists or it is yet to be instantiated!" 
                    + Environment.NewLine + execption.ToString());
                return string.Empty;
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
                Trace.TraceError("Can not generate output as output writer does not exists or it is yet to be instantiated!"
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
        public List<string> CSVRows { get; set; }


        /// <summary>
        /// CSV Format: InputData.Columns.Count will always return number of columns in schema, even if we actually receive less columns
        /// ColumnCount always returns the original column count from the file
        /// FixedLength: Return max row width
        /// </summary>
        public int ColumnCount { get; set; }

        public bool IgnoreContainerValidation { get; set; }
        public string JobIdentifier { get; private set; }        
        public string ProcessingBy { get; private set; }
        public DataSource DataSource { get; private set; }

        /// <summary>
        /// Returns true if data source is valid (which means a valid data found for a valid data source)
        /// </summary>
        public bool IsValid
        {
            get { return DataSource == null ? false : DataSource.IsValid; }
        }

        public List<Row> Rows { get; private set; }
        public List<string> Errors { get; set;}        
        public List<string> Warnings { get;set;}


        public Parameters Parameters { get; private set; }
        public PerformanceCounter PerformanceCounter { get; private set; }
        public SQLClientManager SqlClientManager { get; private set; }

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
                        if ((key.IsDefault == true) && keyType.SRETypeIsConnectionStringType())
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
        public List<string> BadDataInCSVFormat { get; internal set; }
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
        /// Input file name if input feeded from a file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Input file extension if input feeded from a file
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
        public ConcurrentDictionary<string, object> ContainerData { get; internal set; }

        private readonly object _lock = new object();

        /// <summary>
        /// Thread safe agreegate function, should be called from Pre/Post validate only
        /// </summary>
        /// <param name="containerDataKeyName">Name of the container data key where the result will be added or updated</param>
        /// <param name="columnName">Name of the column which is to be agreegated</param>
        /// <param name="isSytemColumn">send true if the column is system column</param>
        /// <returns>none</returns>
        public void Aggregate(string containerDataKeyName, string columnName,bool isSytemColumn = false)
        {
            double calculatedValue = 0;

            lock (_lock)
            {
                foreach (var row in Rows)
                {
                    double oneValue = 0;
                    if (!isSytemColumn)
                    {
                        double.TryParse(row.Columns[columnName].Value, out oneValue);
                    }
                    else
                    {
                        double.TryParse(row.ColumnsSystem[columnName].Value, out oneValue);
                    }

                    calculatedValue += oneValue;
                }

                calculatedValue = Math.Round(calculatedValue, 2);
                ContainerData.AddOrUpdate(containerDataKeyName, calculatedValue, (keyx, oldValue) => calculatedValue);
                Trace.TraceInformation("Agregate method sum has been called on '{0}' and calculate value '{1}' has been assigned to ContainerData with key as '{2}'"
                    , containerDataKeyName, calculatedValue, containerDataKeyName);             
            }
            
        }

        /// <summary>
        /// In case data feeded from list of custom objects, this will contain the original object
        /// </summary>
        public List<object> DataFeededFrom { get; private set; }

        public ManualResetEvent JobCompleted { get; internal set; }


        #region IDisposable Members

        public void Dispose()
        {
            foreach (JobSlice jobSlice in this.JobSlices)
            {
                jobSlice.Dispose();
            }
            foreach (Row row in this.Rows)
            {
                row.Dispose();
            }
            this.SqlClientManager.Dispose();
            this._DefaultConnectionString = null;
            this._DefaultConnectionStringName = null;
            this._JobSlices = null;
            this.BadDataInCSVFormat = null;
            this.ContainerData = null;
            this.DataFeededFrom = null;            
            this.DataSource.Dispose();

        }

        #endregion
    }
    
}






