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
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Text.RegularExpressions;
using Eyedia.Core;
using Eyedia.Core.Data;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Reflection;
using ErikEJ.SqlCe;
using System.Data.SqlServerCe;

namespace Eyedia.IDPE.Services
{
    class OutputWriterDatabase : OutputWriter
    {
        public OutputWriterDatabase() : base() { }
        public OutputWriterDatabase(Job job) : base(job) { }

        #region private properties

        IdpeKey ConnectionStringKey;
        ColumnMap DatabaseMap;

        #endregion Private Properties
        
        public override StringBuilder GetOutput()
        {
            if (_Job != null)
            {
                if (_Job.IsErrored)
                    return new StringBuilder();

                if (!AllowPartial)
                {
                    if (_Job.Errors.Count == 0)
                        return WriteOutput(_Job.Rows);
                    else
                        return new StringBuilder();
                }
                else
                {
                    return WriteOutput(_Job.Rows.Where(r => r.IsValidColumn.ValueBoolean == true).ToList());
                }
            }

            return new StringBuilder();
        }

        public override object GetCustomOutput()
        {
            return null;
        }

        #region PrivateMethods

        #region WriteOutputToDatabaseSqlCe
        private StringBuilder WriteOutputToDatabaseSqlCe(List<Row> rows)
        {
            StringBuilder sb = new StringBuilder();

            DateTime StartTime = DateTime.Now;
            ExtensionMethods.TraceInformation("Converting to DataTable - " + StartTime.ToString());
            DataTable outDataTable = ToDataTable(rows);
            sb = outDataTable.ToCsvStringBuilder();
            DateTime EndTime = DateTime.Now;
            ExtensionMethods.TraceInformation("Converted - {0}. Time taken: {1}", EndTime, (EndTime - StartTime).ToString());

            bool keepNulls = true;
            SqlCeBulkCopyOptions options = new SqlCeBulkCopyOptions();
            if (keepNulls)
            {
                options = options |= SqlCeBulkCopyOptions.KeepNulls;
            }
            using (SqlCeBulkCopy bulkCopy = new SqlCeBulkCopy(ConnectionStringKey.Value, options))
            {
                bulkCopy.DestinationTableName = DatabaseMap.TargetTableName;
                foreach (ColumnMapInfo columnMapInfo in DatabaseMap)
                {
                    if ((!string.IsNullOrEmpty(columnMapInfo.InputColumn))
                    && (columnMapInfo.InputColumn != ColumnMapInfo.DatabaseDefault))
                    {
                        SqlCeBulkCopyColumnMapping mapInfo = new SqlCeBulkCopyColumnMapping(columnMapInfo.InputColumn, columnMapInfo.OutputColumn);
                        bulkCopy.ColumnMappings.Add(mapInfo);
                    }
                }

                try
                {
                    bulkCopy.WriteToServer(outDataTable);
                }
                catch (Exception ex)
                {
                    IsErrored = true;
                    _Job.TraceError(GetBulkCopyFailedData(outDataTable, bulkCopy.ColumnMappings));

                }
                finally
                {
                    if (outDataTable != null)
                        outDataTable.Dispose();
                }
            }           

            return sb;
        }

        #endregion WriteOutputToDatabaseSqlCe

        #region WriteOutputToDatabaseSql
        private StringBuilder WriteOutputToDatabaseSql(List<Row> rows)
        {
            StringBuilder sb = new StringBuilder();
            using (SqlConnection sourceConnection = new SqlConnection(ConnectionStringKey.Value))
            {
                DateTime StartTime = DateTime.Now;
                ExtensionMethods.TraceInformation("Converting to DataTable - " + StartTime.ToString());
                DataTable outDataTable = ToDataTable(rows);
                sb = outDataTable.ToCsvStringBuilder();
                DateTime EndTime = DateTime.Now;
                ExtensionMethods.TraceInformation("Converted - {0}. Time taken: {1}", EndTime, (EndTime - StartTime).ToString());
                
                sourceConnection.Open();
                
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionStringKey.Value))
                {
                    bulkCopy.BulkCopyTimeout = 15 * 60; //15 minutes
                    bulkCopy.DestinationTableName = DatabaseMap.TargetTableName;

                    foreach (ColumnMapInfo columnMapInfo in DatabaseMap)
                    {
                        if ((!string.IsNullOrEmpty(columnMapInfo.InputColumn))
                    && (columnMapInfo.InputColumn != ColumnMapInfo.DatabaseDefault))
                        {
                            SqlBulkCopyColumnMapping mapInfo = new SqlBulkCopyColumnMapping(columnMapInfo.InputColumn, columnMapInfo.OutputColumn);
                            bulkCopy.ColumnMappings.Add(mapInfo);
                        }
                    }

                    try
                    {
                        bulkCopy.WriteToServer(outDataTable);                        
                    }
                    catch(Exception ex)
                    {
                        IsErrored = true;
                        //we are not throwing as we need sb to return as it may useful for debugging
                        _Job.TraceError(GetBulkCopyFailedData(outDataTable, bulkCopy.ColumnMappings));
                        
                    }
                    finally
                    {
                        if(outDataTable != null)
                            outDataTable.Dispose();
                    }
                }              
            }
            return sb;
        }
        #endregion WriteOutputToDatabaseSql

        #region WriteOutputToDatabaseOracle
        private StringBuilder WriteOutputToDatabaseOracle(List<Row> rows)
        {
            _Job.PerformanceCounter.Start(_Job.JobIdentifier, JobPerformanceTaskNames.OutputWriterOracle);

            ExtensionMethods.TraceInformation("Starting oracle bulk insert...");
            IDal dal = new DataAccessLayer(DatabaseTypes.Oracle).Instance;
            IDbConnection connection = dal.CreateConnection(ConnectionStringKey.Value);
            connection.Open();
            IDbCommand command = dal.CreateCommand();
            command.GetType().GetProperty("ArrayBindCount").SetValue(command, rows.Count, null);            
            command.Connection = connection;
            ExtensionMethods.TraceInformation("Communication objects initialized. Creating arrays...");
        
            int counter = 0;
            //creating arrays & insert statement
            string insertStatementPart1 = string.Format("insert into {0} (", DatabaseMap.TargetTableName);
            string insertStatementPart2 = "values (";
            foreach (ColumnMapInfo columnMapInfo in DatabaseMap)
            {
                if ((!string.IsNullOrEmpty(columnMapInfo.InputColumn))
                    && (columnMapInfo.InputColumn != ColumnMapInfo.DatabaseDefault))
                {
                    counter++;

                    insertStatementPart1 += "\"" + columnMapInfo.OutputColumn + "\",";

                    if (columnMapInfo.InputColumn == ColumnMapInfo.CustomDefined)
                    {
                        insertStatementPart2 += string.Format("\"Customer_Id_Seq\".nextval,", counter);                        
                        continue;
                    }
                    else
                    {
                        insertStatementPart2 += string.Format(":{0},", counter);
                    }

                    IdpeAttribute attributeAvailable = _Job.DataSource.AcceptableAttributesSystem.Where(a => a.Name == columnMapInfo.InputColumn).SingleOrDefault();
                    if (attributeAvailable == null)
                        continue;

                    Type attributeType = attributeAvailable.ConvertSreTypeIntoDotNetType(DatabaseTypes.Oracle);

                    string[] strList = rows.Select(row => GetAttributeValue(row.ColumnsSystem[columnMapInfo.InputColumn], true)).ToArray();

                    if (attributeType == typeof(int))
                    {
                        List<int?> list = strList.Select(s => s.GetValueOrNull<int>()).ToList();
                        command.AddParameterWithValue(columnMapInfo.OutputColumn, list.ToArray(), true);
                    }
                    else if (attributeType == typeof(long))
                    {
                        List<long?> list = strList.Select(s => s.GetValueOrNull<long>()).ToList();
                        command.AddParameterWithValue(columnMapInfo.OutputColumn, list.ToArray(), true);
                    }
                    else if (attributeType == typeof(double))
                    {
                        List<double?> list = strList.Select(s => s.GetValueOrNull<double>()).ToList();
                        command.AddParameterWithValue(columnMapInfo.OutputColumn, list.ToArray(), true);
                    }
                    else if (attributeType == typeof(bool))
                    {
                        List<bool?> list = strList.Select(s => s.GetValueOrNull<bool>()).ToList();
                        command.AddParameterWithValue(columnMapInfo.OutputColumn, list.ToArray(), true);
                    }
                    else if (attributeType == typeof(DateTime))
                    {
                        List<DateTime?> list = strList.Select(s => s.GetValueOrNull<DateTime>()).ToList();
                        command.AddParameterWithValue(columnMapInfo.OutputColumn, list.ToArray(), true);
                    }
                    else
                    {
                        command.AddParameterWithValue(columnMapInfo.OutputColumn, strList, true);
                    }                    
                }                
            }

            if (insertStatementPart1.Length > 1)
                insertStatementPart1 = insertStatementPart1.Substring(0, insertStatementPart1.Length - 1);
            if (insertStatementPart2.Length > 1)
                insertStatementPart2 = insertStatementPart2.Substring(0, insertStatementPart2.Length - 1);

            command.CommandText = string.Format("{0}) {1})", insertStatementPart1, insertStatementPart2);
            ExtensionMethods.TraceInformation("Arrays created. Executing statement - '{0}'", command.CommandText);
            command.ExecuteNonQuery();

            connection.Close();
            connection.Dispose();

            ExtensionMethods.TraceInformation("Oracle bulk insert finished.");
            Trace.Flush();
            _Job.PerformanceCounter.Stop(_Job.JobIdentifier, JobPerformanceTaskNames.OutputWriterOracle);

            return new StringBuilder();
        }
        #endregion WriteOutputToDatabaseOracle

        #region Helpers
        private void InitConfig()
        {           
            IdpeKey OutputWriterDatabaseConfiguration = new Manager().GetKey(_Job.DataSource.Id, SreKeyTypes.OutputWriterDatabaseConfiguration.ToString());

            if ((OutputWriterDatabaseConfiguration != null)
                && (!string.IsNullOrEmpty(OutputWriterDatabaseConfiguration.Name)))
            {
                DatabaseMap = new ColumnMap(_Job.DataSource.Id, OutputWriterDatabaseConfiguration.Value);
                ConnectionStringKey = DatabaseMap.ConnectionKey;

                if (!string.IsNullOrEmpty(DatabaseMap.ConnectionKeyRunTime))
                {
                    string connectionKeyNameRuntime = _Job.GetProcessVariableValue(DatabaseMap.ConnectionKeyRunTime).ToString();
                    if (string.IsNullOrEmpty(connectionKeyNameRuntime))
                        throw new Exception(string.Format("Database writer run time connection string '{0}' was not defined!", DatabaseMap.ConnectionKeyRunTime));
                    ConnectionStringKey = _Job.DataSource.Keys.GetKey(connectionKeyNameRuntime);
                }           
            }            
        }

        private StringBuilder WriteOutput(List<Row> rows)
        {
            StringBuilder sb = new StringBuilder();
            if (rows != null)
            {
                DateTime StartTime = DateTime.Now;
                ExtensionMethods.TraceInformation("Outputwriter Database started - " + StartTime.ToString());
                InitConfig();
                sb = WriteOutputToDatabase(rows);
                DateTime EndTime = DateTime.Now;
                ExtensionMethods.TraceInformation("Outputwriter Database Completed - {0}. Time taken: {1}", EndTime, (EndTime - StartTime).ToString());
            }

            return sb;
        }

        private StringBuilder WriteOutputToDatabase(List<Row> row)
        {
            DatabaseTypes databaseType = ConnectionStringKey.GetDatabaseType();
            switch (databaseType)
            {
                case DatabaseTypes.SqlServer:
                    return WriteOutputToDatabaseSql(row);

                case DatabaseTypes.SqlCe:
                    return WriteOutputToDatabaseSqlCe(row);

                case DatabaseTypes.Oracle:
                    return WriteOutputToDatabaseOracle(row);

                default:
                    throw new Exception(string.Format("The '{0}' type database output writer has not been implemented yet!", databaseType));

            }
        }
        #endregion Helpers

        private string GetBulkCopyFailedData(DataTable table, SqlBulkCopyColumnMappingCollection columnMappings)
        {
            StringBuilder errorMessage = new StringBuilder("Bulk copy failures:" + Environment.NewLine);
            SqlConnection connection = null;
            SqlTransaction transaction = null;
            SqlBulkCopy bulkCopy = null;
            int failedRecords = 0;
            try
            {
                connection = new SqlConnection(ConnectionStringKey.Value);
                connection.Open();
                transaction = connection.BeginTransaction();
                bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
                bulkCopy.DestinationTableName = DatabaseMap.TargetTableName;
                foreach (SqlBulkCopyColumnMapping mapping in columnMappings)
                {
                    bulkCopy.ColumnMappings.Add(mapping);
                }
                DataTable oneRecordTable = table.Clone();
                
               foreach(DataRow row in table.Rows)
                {
                    oneRecordTable.Rows.Clear();
                    oneRecordTable.ImportRow(row);
                   
                    try
                    {
                        bulkCopy.WriteToServer(oneRecordTable);
                    }
                    catch (Exception ex)
                    {
                        failedRecords++;
                        DataRow faultyDataRow = oneRecordTable.Rows[0];
                        errorMessage.AppendFormat("Error: {0}{1}", ex.Message, Environment.NewLine);
                        errorMessage.AppendFormat("Row data: {0}", Environment.NewLine);
                        foreach (DataColumn column in oneRecordTable.Columns)
                        {
                            errorMessage.AppendFormat(
                               "\tColumn {0} - [{1}]{2}",
                               column.ColumnName,
                               faultyDataRow[column.ColumnName].ToString(),
                               Environment.NewLine);
                        }

                        if (failedRecords >= 100)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Append("Unable to document SqlBulkCopy errors. See inner exceptions for details.");
                errorMessage.Append(ex.ToString());
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return errorMessage.ToString();
        }

        private string GetBulkCopyFailedData(DataTable table, SqlCeBulkCopyColumnMappingCollection columnMappings)
        {
            StringBuilder errorMessage = new StringBuilder("Bulk copy failures:" + Environment.NewLine);
            SqlCeConnection connection = null;
            SqlCeTransaction transaction = null;
            SqlCeBulkCopy bulkCopy = null;

            try
            {
                connection = new SqlCeConnection(ConnectionStringKey.Value);
                connection.Open();
                transaction = connection.BeginTransaction();
                SqlCeBulkCopyOptions options = new SqlCeBulkCopyOptions();
                options = options |= SqlCeBulkCopyOptions.KeepNulls;
                bulkCopy = new SqlCeBulkCopy(connection, options, transaction);
                bulkCopy.DestinationTableName = DatabaseMap.TargetTableName;
                foreach (SqlCeBulkCopyColumnMapping mapping in columnMappings)
                {
                    bulkCopy.ColumnMappings.Add(mapping);
                }
                DataTable oneRecordTable = table.Clone();

                foreach (DataRow row in table.Rows)
                {
                    oneRecordTable.Rows.Clear();
                    oneRecordTable.ImportRow(row);

                    try
                    {
                        bulkCopy.WriteToServer(oneRecordTable);
                    }
                    catch (Exception ex)
                    {
                        DataRow faultyDataRow = oneRecordTable.Rows[0];
                        errorMessage.AppendFormat("Error: {0}{1}", ex.Message, Environment.NewLine);
                        errorMessage.AppendFormat("Row data: {0}", Environment.NewLine);
                        foreach (DataColumn column in oneRecordTable.Columns)
                        {
                            errorMessage.AppendFormat(
                               "\tColumn {0} - [{1}]{2}",
                               column.ColumnName,
                               faultyDataRow[column.ColumnName].ToString(),
                               Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Append("Unable to document SqlBulkCopy errors. See inner exceptions for details.");
                errorMessage.Append(ex.ToString());
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return errorMessage.ToString();
        }
        #endregion privateMethods
    }

}


