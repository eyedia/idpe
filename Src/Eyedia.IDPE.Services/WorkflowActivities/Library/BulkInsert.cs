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
using ErikEJ.SqlCe;
using System.Activities;
using System.ComponentModel;
using Eyedia.IDPE.DataManager;
using System.Data;
using System.Data.SqlClient;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using System.Configuration;

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(WorkflowActivities.BulkInsert))]
    public sealed class BulkInsert : CodeActivity
    {
        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }
        public InArgument<DataTable> Table { get; set; }
        public InArgument<bool> CreateTable { get; set; }
        public InArgument<string> ConnectionStringKeyName { get; set; }
        public InArgument<string> TableName { get; set; }
        public InArgument<string> SpecificColumnTypes { get; set; }

        [DefaultValue(5)]
        public InArgument<int> TimeOut { get; set; }

        [DefaultValue(5000)]
        public InArgument<int> BatchSize { get; set; }
        
        public OutArgument<string> ErrorMessage { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DataSource dataSource = null;
            Job job = context.GetValue(this.Job);
            if (job != null)
            {
                dataSource = job.DataSource;
            }
            else
            {
                WorkerData data = context.GetValue(this.Data);
                data.ThrowErrorIfNull(this.DisplayName);
                dataSource = data.Job.DataSource;
            }

            bool createTable = context.GetValue(this.CreateTable);
            DataTable table = context.GetValue(this.Table);
            string connectionStringKeyName = context.GetValue(this.ConnectionStringKeyName);
            string tableName = context.GetValue(this.TableName);
            string specificColumnTypes = context.GetValue(this.SpecificColumnTypes);
            int timeOut = context.GetValue(this.TimeOut);
            if (timeOut == 0)
                timeOut = 5;//default

            int batchSize = context.GetValue(this.BatchSize);
            if (batchSize == 0)
                batchSize = 5000;//default      

            SreKey connectionStringKey = null;
            if (string.IsNullOrEmpty(connectionStringKeyName))
            {
                //default is repository database
                connectionStringKey = new SreKey();
                connectionStringKey.Name = "cs";
                connectionStringKey.Type = (int)Information.EyediaCoreConfigurationSection.Database.DatabaseType.GetSreType();
                connectionStringKey.Value = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].ToString();
            }
            else
            {
                connectionStringKey = dataSource.Keys.GetKey(connectionStringKeyName);
                if (connectionStringKey == null)
                    throw new Exception(string.Format("Can not load data table, the connection string was null! Connection string key name was '{0}'"
                        , connectionStringKeyName));
            }
            try
            {
               
                #region Bulk Insert
                bool keepNulls = true;

                SqlCeBulkCopyOptions options = new SqlCeBulkCopyOptions();
                if (keepNulls)
                {
                    options = options |= SqlCeBulkCopyOptions.KeepNulls;
                }

                DatabaseTypes databaseType = connectionStringKey.GetDatabaseType();
                if (databaseType == DatabaseTypes.SqlServer)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionStringKey.Value))
                    {
                        bulkCopy.BulkCopyTimeout = timeOut * 60;
                        bulkCopy.BatchSize = batchSize;
                        for (int c = 0; c < table.Columns.Count; c++)
                        {
                            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(table.Columns[c].ColumnName, table.Columns[c].ColumnName));
                        }
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(table);
                    }
                }

                else
                {                    
                    if (createTable)
                        SqlCeTableCreator.Create(table, new System.Data.SqlServerCe.SqlCeConnection(connectionStringKey.Value), tableName, specificColumnTypes);

                    using (SqlCeBulkCopy bulkCopy = new SqlCeBulkCopy(connectionStringKey.Value, options))
                    {
                        bulkCopy.BulkCopyTimeout = timeOut * 60;
                        bulkCopy.BatchSize = batchSize;
                        for (int c = 0; c < table.Columns.Count; c++)
                        {
                            bulkCopy.ColumnMappings.Add(new SqlCeBulkCopyColumnMapping(table.Columns[c].ColumnName, table.Columns[c].ColumnName));
                        }
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(table);
                    }
                }

                #endregion Bulk Insert
            }
            catch(Exception ex)
            {
                context.SetValue(ErrorMessage, ex.Message);
            }
        }
    }
}



