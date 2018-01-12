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
using System.Timers;
using System.IO;
using System.Data;
using System.Diagnostics;
using Symplus.Core;
using Symplus.Core.Data;
using Symplus.RuleEngine.Common;
using Symplus.RuleEngine.DataManager;

namespace Symplus.RuleEngine.Services
{
	
	public class SqlWatcher : Watchers
    {
        #region Properties

        public DataSource DataSource { get; private set; }
        //public int DataSourceId { get; private set; }
        //public string DataSourceName { get; private set; }
        //const string __tracePrefix = "SW({0})";
        public DatabaseTypes DatabaseType
        {
            get
            {
                return ConnectionStringKey.GetDatabaseType();
            }
        }
        public string ConnectionStringName
        { 
            get
            {
                return DataSource.Keys.GetKeyValue(SreKeyTypes.PullSqlConnectionString);
                
            }
        }

        public string ConnectionStringNameRunTime
        {
            get
            {
                return DataSource.Keys.GetKeyValue(SreKeyTypes.PullSqlConnectionStringRunTime);

            }
        }

        public SreKey ConnectionStringKey
        {
            get
            {
                if (!string.IsNullOrEmpty(ConnectionStringNameRunTime))
                {
                    object rtConnectionStringName = Job.GetProcessVariableValue(ConnectionStringNameRunTime);
                    SreKey key = DataSource.Keys.GetKey(rtConnectionStringName.ToString());

                    if (key == null)
                        throw new Exception(string.Format("{0} - A database connection '{1}' was not defined!", DataSource.Name, rtConnectionStringName));

                    //DataSource.TraceInformation(__tracePrefix + "Run time connection string '{1}' was set!", DataSource.Name, rtConnectionStringName);
                    return key;
                }
                else
                {
                    return DataSource.Keys.GetKey(ConnectionStringName);
                }

            }
        }
        public string ProcessingBy { get; private set; }
        
        public string ReturnType { get; private set; }
        public string SelectQuery
        {
            get
            {
                return DataSource.Keys.GetKeyValue(SreKeyTypes.SqlQuery);               
            }
        }
        public string UpdateQuery { get { return DataSource.Keys.GetKeyValue(SreKeyTypes.SqlUpdateQueryProcessing); } }
        public string InterfaceName { get; private set; }        
        public int Interval { get; private set; }

        public Job Job { get; private set; }
        InputFileGenerator InputFileGenerator { get; set; }
        Timer timer;
        bool _IsTemporarilyStopped;
        internal bool IsTemporarilyStopped
        {
            get
            {
                return _IsTemporarilyStopped;
            }

            set
            {
                _IsTemporarilyStopped = value;

                if(_IsTemporarilyStopped)
                    DataSource.TraceInformation(" - Stop Requested.");
                else
                    DataSource.TraceInformation(" - Start Requested.");

                Trace.Flush();
            }
        }
        //private List<SreKey> Keys
        //{
        //    get
        //    {
        //        List<SreKey> keys = Cache.Instance.Bag[DataSourceId + ".keys"] as List<SreKey>;
        //        if (keys == null)
        //        {
        //            keys = new Manager().GetApplicationKeys(DataSourceId, true);
        //            Cache.Instance.Bag.Add(DataSourceId + ".keys", keys);
        //        }
        //        return keys;
        //    }
        //}
        #endregion Properties

        /// <summary>
        /// Keeps pulling from database (MS SQL, MS SQL CE, Oracle)
        /// </summary>
        /// <param name="dataSource">The data source object</param>       
        public SqlWatcher(SreDataSource dataSource)
        {            
            this.DataSource = new DataSource(dataSource.Id, string.Empty); 
            //DataSourceName = dataSource.Name;
            ProcessingBy = dataSource.ProcessingBy;
            if (string.IsNullOrEmpty(ProcessingBy))
                ProcessingBy = dataSource.Name;

            string strinterval = DataSource.Keys.GetKeyValue(SreKeyTypes.SqlWatchInterval);
            int interval = 0;
            int.TryParse(strinterval, out interval);
            if (interval <= 0) interval = 2;
            this.Interval = interval;

            string appPullFolder = SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull + "\\" + dataSource.Id;
            string appOutputFolder = SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryOutput + "\\" + dataSource.Id + "\\" + DateTime.Now.ToString("yyyyMMdd");

            this.DataSourceParameters = new Dictionary<string, object>();

            DataSourceParameters.Add("DataSourceId", dataSource.Id);
            DataSourceParameters.Add("PullFolder", appPullFolder);
            DataSourceParameters.Add("OutputFolder", appOutputFolder);
            DataSourceParameters.Add("ProcessingBy", dataSource.ProcessingBy);

            this.ReturnType = DataSource.Keys.GetKeyValue(SreKeyTypes.PullSqlReturnType);
            if (this.ReturnType == "I")
            {
              
                this.InterfaceName = DataSource.Keys.GetKeyValue(SreKeyTypes.PullSqlInterfaceName);
                if ((!string.IsNullOrEmpty(this.InterfaceName))
                && (Type.GetType(this.InterfaceName) != null))
                {
                    object objInputFileGenerator = Activator.CreateInstance(Type.GetType(this.InterfaceName), this);
                    this.InputFileGenerator = (InputFileGenerator)objInputFileGenerator;
                }
            }

        }

        public void StartDownloading()
        {
            timer = new Timer(this.Interval * 60 * 1000);
            timer.AutoReset = true;
            timer.Enabled = false;
            timer.Elapsed += new ElapsedEventHandler(JobProcessor_Elapsed);           
            timer.Start();

            DataSource.TraceInformation("Initialized 'PullSql'; '{0}' [ReturnType = {1}, Select Query = {2}, Update Query = {3}, InterfaceName = {4}, Interval = {5}]",
                      DataSource.Name, ReturnType == "D" ? "Direct" : "Interfaced", SelectQuery, UpdateQuery, InterfaceName, Interval);
        }

        private static readonly object _lock = new object();
        void JobProcessor_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (DataSourceIsDisabled())
                    return;

                if (DataSourceIsTemporarilyStopped())
                    return;

                if (!this.IsRunning)
                {
                    this.IsRunning = true;
                    DataSource.TraceInformation(" - Starting");
                    Job = JobProcessor.Instantiate(DataSource.Id, DataSource.Name, ProcessingBy);
                    Job.IsTriggeredBySqlPuller = true;
                    this.DataSource.BusinessRules.Execute(Job, null, RuleSetTypes.SqlPullInit);
                    DataTable table = ExecuteQuery();
                    InvokeAfterExecutingSelect(new SqlWatcherEventArgs(table));
                    if (table.Rows.Count == 0)
                    {
                        Job.TraceInformation(" - No records found.");                        
                        lock (_lock)
                        {
                            if (Registry.Instance.Entries.ContainsKey(Job.JobIdentifier))
                            {
                                Job job = Registry.Instance.Entries[Job.JobIdentifier] as Job;
                                Registry.Instance.Entries.Remove(Job.JobIdentifier);
                                job.Dispose();
                                job = null;
                            }
                        }
                    }
                    else
                    {
                        if (this.ReturnType == "I")
                        {
                            DataSource.TraceInformation("{0} - found {1} records. Processing with interface {2}", DataSource.Name, table.Rows.Count, InterfaceName);
                            string fileName = GenerateFile(table);
                            InvokeFileDownloaded(new FileSystemWatcherEventArgs(DataSourceParameters, fileName));
                            DataSource.TraceInformation("{0} - records processed with interface.", DataSource.Name);
                        }
                        else
                        {
                            DataSource.TraceInformation("{0} - found {1} records. Processing...", DataSource.Name, table.Rows.Count);
                            Job.Feed(table);

                            JobProcessor jobProcessor = new JobProcessor();
                            StringBuilder result = jobProcessor.ProcessJob(Job);
                            if (Job.IsErrored)
                                new SqlWatcherHelper(Job).ExecuteRecoveryScript();

                            string outputFolder = DataSource.GetOutputFolder(DataSource.Id, DataSource.Keys);
                            string actualOutputFolder = outputFolder;
                            string outputFileName = DataSource.GetOutputFileName(DataSource.Id, DataSource.Keys, outputFolder, DataSource.Id.ToString());

                            if (File.Exists(outputFileName))
                            {
                                string buName = Path.Combine(outputFolder, string.Format("{0}{1}_{2}", DataSource.Name, Guid.NewGuid().ToString(), Path.GetFileName(outputFileName)));
                                new FileUtility().FileCopy(outputFileName, buName, true);
                            }

                            if (result.Length > 0)
                            {
                                using (StreamWriter tw = new StreamWriter(outputFileName))
                                {
                                    tw.Write(result);
                                    tw.Close();
                                }

                                DataSource.TraceInformation("{0} - A data set from {1} was successfully processed. Output file was {2}", DataSource.Name, DataSource.Name, outputFileName);
                                Registry.Instance.Pullers.InvokeFileProcessed(DataSource.Id, jobProcessor.JobId, DataSource.Keys, outputFileName, outputFolder, string.Empty);
                            }
                            else
                            {
                                DataSource.TraceInformation("{0} - Failed to process the SQL data set from '{1}', empty data came from output writer! Check log for more details.", DataSource.Name, DataSource.Name);
                            }
                            jobProcessor.Dispose();
                        }
                    }
                    this.IsRunning = false;
                    timer.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                this.IsRunning = false;
                DataSource.TraceError(exp.ToString());
                timer.Enabled = true;
            }
            Trace.Flush();
        }

      
        private bool DataSourceIsDisabled()
        {
            int dataSourceId = 0;
            int.TryParse(DataSourceParameters["DataSourceId"].ToString(), out dataSourceId);
            if (dataSourceId == 0)
                return true;

            DataSource dataSource = new DataSource(dataSourceId, string.Empty);
            if (dataSource.IsActive == false)
            {
                dataSource.TraceInformation(" - ignored as disabled");
                return true;
            }
            return false;
        }

        
        private bool DataSourceIsTemporarilyStopped()
        {            
            if (IsTemporarilyStopped)
            {
                ExtensionMethods.TraceInformation(" - ignored as temporarily stopped");
                return true;
            }
            return false;
        }

        public void StopDownloading()
        {
            try
            {
                this.timer.Dispose();
                this.IsRunning = false;
            }
            catch { }
        }

        #region Private Methods

        DataTable ExecuteQuery()
        {            

            DataTable data = new DataTable();
            IDal myDal = new DataAccessLayer(DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(ConnectionStringKey.Value);
            conn.Open();
            
            IDbTransaction transaction = myDal.CreateTransaction(conn);
            IDbCommand command = myDal.CreateCommand();            
            command.Connection = conn;
            command.Transaction = transaction;
            command.CommandText = new SreCommandParser(DataSource).Parse(SelectQuery);

            IDbCommand commandUpdate = null;
            if (!string.IsNullOrEmpty(UpdateQuery))
            {
                commandUpdate = myDal.CreateCommand();
                commandUpdate.Connection = conn;
                commandUpdate.Transaction = transaction;
                commandUpdate.CommandText = new SreCommandParser(DataSource).Parse(UpdateQuery);
            }

            try
            {
                
                data.Load(command.ExecuteReader());

                if(commandUpdate != null)
                    commandUpdate.ExecuteNonQuery();

                transaction.Commit();
               
            }
            catch (Exception ex)
            {
                transaction.Rollback();                
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
                command.Dispose();
                commandUpdate.Dispose();
            }

            return data;
        }

        string GenerateFile(DataTable data)
        {
            string fileName = this.DataSourceParameters["PullFolder"] + "\\" + DataSource.Id + ".csv";

            if (this.InputFileGenerator != null)
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(this.InputFileGenerator.GenerateFileContent(data));
                    sw.Close();
                }
            }
            //else
            //{
            //    data.ToCsv(fileName, ",");
            //}
            return fileName;
        }
     
        #endregion Private Methods
    }
}





