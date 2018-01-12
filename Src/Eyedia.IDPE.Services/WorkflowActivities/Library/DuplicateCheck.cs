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
using System.Reflection;
using System.Data;
using System.Diagnostics;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.ComponentModel;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
    [Designer(typeof(Eyedia.IDPE.Services.WorkflowActivities.DuplicateCheckDesigner))]
    public sealed class DuplicateCheck : CodeActivity
    {
        #region Properties

        [RequiredArgument]
        [DefaultValue("Job")]
        public InArgument<Job> Job { get; set; }

        [RequiredArgument]
        [DefaultValue("Job.DataSource.Keys.GetKeyValue(\"UniquenessCriteria\"")]
        public InArgument<string> UniquenessCriteriaKeyName { get; set; }

        [RequiredArgument]
        [DefaultValue("Job.DataSource.Keys.GetKeyValue(\"DuplicateCheckCS\"")]
        public InArgument<string> ConnectionStringKeyName { get; set; }

        [DefaultValue(false)]
        public InArgument<bool> CheckWithInTheFileOnly { get; set; }

        #endregion Properties

        protected override void Execute(CodeActivityContext context)
        {
            
            Job job = context.GetValue(this.Job);
            if (job != null)
            {
                job.PreValidationRuleFailed = true;
                string[] uniquenessColumns = GetUniquenessColumns(job, context.GetValue(this.UniquenessCriteriaKeyName));
                ValidateUniquenessColumns(job, uniquenessColumns);

                //generate incoming record table
                DataTable filterTable = GenerateFilterTable(job, uniquenessColumns);
                if (filterTable.Rows.Count == 0)
                    return;

                //check duplicates within the file
                List<string> dupResult = filterTable.CheckDuplicates(uniquenessColumns, false);
                if (dupResult.Count > 0)
                {
                    string errorMessage = "Duplicate records found within the file!";
                    job.Errors.Add(errorMessage);
                    job.Errors.AddRange(dupResult);
                    ExtensionMethods.TraceInformation(errorMessage);
                    job.PreValidationRuleFailed = false;
                    return;
                }

                if (context.GetValue(CheckWithInTheFileOnly))
                    return;

                //check duplicates against consumer's database                
                job.PreValidationRuleFailed = CheckDuplicate(job, filterTable, GetConnectionString(job, context.GetValue(this.ConnectionStringKeyName)));
            }
        }

       
        #region Private Methods

        /// <summary>
        /// returns true if duplicate check failed
        /// </summary>
        /// <param name="job"></param>
        /// <param name="filterTable"></param>
        /// <param name="connectionStringKey"></param>
        /// <returns></returns>
        bool CheckDuplicate(Job job, DataTable filterTable, IdpeKey connectionStringKey)
        {
            string errorMessage = string.Empty;
            int oldCount = filterTable.Rows.Count;
            DataColumnCollection passedColumns = filterTable.Columns;

            filterTable = CallStoreProcedure(job, filterTable, connectionStringKey);

            if (filterTable == null)
            {
                errorMessage = "Row[*][*]:Duplicate check against database was failed. " +
                   "Please analyze the logs/error emails and reprocess the file. " +
                   "'Possible cause the validator was not able to communicate with database.'";
                job.Errors.Add(errorMessage);
                return true;
            }                
         
            if (CheckReturnedColumns(job, passedColumns, filterTable) == true)
                return true;

            if (oldCount != filterTable.Rows.Count)
            {
                errorMessage = string.Format("Row[*][*]:Duplicate check against database was failed. " +
                   "Please analyze the logs/errors and reprocess the file. " +
                   "'SRE was able to communicate with database, but returned incorrect records. '{0}' was sent, but retrieved only '{1}'."
                   , oldCount, filterTable.Rows.Count);

                job.Errors.Add(errorMessage);
                return true;
            }

            bool duplicateFound = false;
            foreach (DataRow row in filterTable.Rows)
            {
                errorMessage = string.Empty;
                if ((row["IsDuplicate"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    || (row["IsDuplicate"].ToString().Equals("1")))
                {
                    duplicateFound = true;
                    errorMessage = string.Format("Duplicate record(s) found. A similar entry with '{0}' was found in the system.", FormatDataRow(row));
                    job.Errors.Add(errorMessage);
                }
            }
            return duplicateFound ? true : false;
        }

        private bool CheckReturnedColumns(Job job, DataColumnCollection passedColumns, DataTable filterTable)
        {
            string errorMessage = string.Empty;

            if (passedColumns.Count != filterTable.Columns.Count)
            {
                errorMessage = string.Format("Row[*][*]:Duplicate check against database was failed. " +
                  "Please analyze the logs/errors and reprocess the file. " +
                  "'SRE was able to communicate with database, but returned incorrect columns. '{0}' was sent, but retrieved only '{1}'."
                  , passedColumns.Count, filterTable.Columns.Count);

                job.Errors.Add(errorMessage);
                return true;
            }


            if (filterTable.Columns.Count < 3)
            {
                errorMessage = "Row[*][*]:Duplicate check against database was failed. " +
                  "Please analyze the logs/errors and reprocess the file. " +
                  "'SRE was able to communicate with database, but returned incorrect columns.";

                job.Errors.Add(errorMessage);
                return true;
            }

            if (filterTable.Columns[0].ColumnName != "Position")
            {
                errorMessage = "Row[*][*]:Duplicate check against database was failed. " +
                 "Please analyze the logs/errors and reprocess the file. " +
                 "'SRE was able to communicate with database, but returned incorrect columns. The very first column should be 'Position'";

                job.Errors.Add(errorMessage);
                return true;
            }

            if (filterTable.Columns[filterTable.Columns.Count - 1].ColumnName != "IsDuplicate")
            {
                errorMessage = "Row[*][*]:Duplicate check against database was failed. " +
                 "Please analyze the logs/errors and reprocess the file. " +
                 "'SRE was able to communicate with database, but returned incorrect columns. The very last column should be 'IsDuplicate'";

                job.Errors.Add(errorMessage);
                return true;
            }

            List<string> errors = new List<string>();
            for (int i = 1; i < passedColumns.Count - 1; i++)
            {
                if (!passedColumns[i].ColumnName.Equals(filterTable.Columns[i].ColumnName, StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = string.Format("Column number {0} was passed as '{1}', but returned as '{2}'",
                    i + 1, passedColumns[i].ColumnName, filterTable.Columns[i].ColumnName);

                    errors.Add(errorMessage);                    
                }
            }

            if (errors.Count > 0)
            {
                job.Errors.Add("Row[*][*]:Duplicate check against database was failed. Please analyze the logs/errors and reprocess the file. 'SRE was able to communicate with database, but returned incorrect columns. ");
                job.Errors.Add("");
                job.Errors.AddRange(errors);
                return true;
            }

            return false;
        }

        DataTable CallStoreProcedure(Job job, DataTable filterTable, IdpeKey connectionStringKey)
        {
            DatabaseTypes databaseType = connectionStringKey.GetDatabaseType();
            string actualConnectionString = connectionStringKey.Value;

            DataTable table = new DataTable();
            string xmlInputData = ToXml(filterTable);
            IDbConnection con = null;
            IDal myDal = new DataAccessLayer(databaseType).Instance;
            try
            {                
                con = myDal.CreateConnection(actualConnectionString);

                StoredProcedure sp = new StoredProcedure(myDal);
                sp.ConnectionString = actualConnectionString;
                IDbDataParameter dp1 = sp.CreateParameter();
                dp1.ParameterName = "@dataSourceId";
                dp1.DbType = DbType.Int16;
                dp1.Value = job.DataSource.Id;

                IDbDataParameter dp2 = sp.CreateParameter();
                dp2.ParameterName = "@inputData";
                dp2.DbType = DbType.Xml;
                dp2.Value = xmlInputData;

                sp.CacheDisconnectData = true;
                sp.Name = "SreDuplicateCheck";

                sp.AddParameter(dp1);
                sp.AddParameter(dp2);
                con.Open();

                IDataReader reader = sp.CreateDataReader();                
                table.Load(reader);


            }
            catch (Exception ex)
            {
                string errorMessage = "Error while checking duplicate (database communication)! " + Environment.NewLine + ex.Message;
                AddErrorMessage(job, errorMessage);               
            }
            finally
            {
                if (con != null)
                {
                    if (con.State != System.Data.ConnectionState.Closed) con.Close();
                }


                con.Close();
                con.Dispose();
            }

            return table;
        }

        private string ToXml(DataTable table)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            table.WriteXml(writer, true);
            return writer.ToString();

        }


        DataTable GenerateFilterTable(Job job, string[] uniquenessColumns)
        {

            DataTable filterTable = new DataTable("SreInputData");            
            filterTable.Columns.Add("Position", typeof(Int32));

            foreach (string uniquenessColumn in uniquenessColumns)
            {
                filterTable.Columns.Add(uniquenessColumn, typeof(String));
            }
            filterTable.Columns.Add("IsDuplicate", typeof(bool));
            int position = 1;
            foreach (DataRow row in job.InputData.Rows)
            {
                DataRow newFilterRow = filterTable.NewRow();                
                newFilterRow["Position"] = position;
                foreach (string uniquenessColumn in uniquenessColumns)
                {
                    if (row[uniquenessColumn] != null)
                    {
                        newFilterRow[uniquenessColumn] = row[uniquenessColumn].ToString();
                    }
                }
                newFilterRow["IsDuplicate"] = false;
                position++;
                filterTable.Rows.Add(newFilterRow);
            }
            return filterTable;
        }

        string FormatDataRow(DataRow row)
        {
            string str = string.Empty;
            string internalColumns = "Position,IsDuplicate";
            int cnt = 0;
            foreach (DataColumn col in row.Table.Columns)
            {
                if (internalColumns.IndexOf(col.ColumnName, StringComparison.OrdinalIgnoreCase) == -1)
                    str += string.Format("{0} = '{1}', ", col.ColumnName, row[cnt].ToString());
                cnt++;
            }
            if (str.Length > 2)
                str = str.Substring(0, str.Length - 2);
            return str;
        }


        string[] GetUniquenessColumns(Job job, string uniquenessCriteriaKeyName)
        {
            string uniquenessCriteria = job.DataSource.Keys.GetKeyValue(uniquenessCriteriaKeyName);
            if (string.IsNullOrEmpty(uniquenessCriteria))
            {
                string errorMessage = string.Format("The key '{0}' was not found! Uniqueness criteria is not defined, duplicate check was aborted!", uniquenessCriteriaKeyName);
                AddErrorMessage(job, errorMessage);
                throw new BusinessException(errorMessage);
            }
            return uniquenessCriteria.Split("+".ToCharArray());
        }

        private void ValidateUniquenessColumns(Job job, string[] uniquenessColumns)
        {
            
            foreach (string column in uniquenessColumns)
            {
                bool found = false;
                foreach (IdpeAttribute attribute in job.DataSource.AcceptableAttributes)
                {
                    if (attribute.Name.Equals(column, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    string errorMessage = string.Format("The attribute '{0}' in uniqueness criteria is not associated with datasource '{1}', duplicate check was aborted!",
                        column, job.DataSource.Name);
                    AddErrorMessage(job, errorMessage);

                    throw new BusinessException(errorMessage);
                }
                
            }
        }

        IdpeKey GetConnectionString(Job job, string connectionStringKeyName)
        {
            IdpeKey connectionStringKey = job.DataSource.Keys.GetKey(connectionStringKeyName);
            if (connectionStringKey == null)
            {
                string errorMessage = string.Format("The connection string '{0}' was not found! Could not connect to database, duplicate check was aborted!", connectionStringKeyName);
                AddErrorMessage(job, errorMessage);
                throw new BusinessException(errorMessage);
            }
            return connectionStringKey;
        }

        void AddErrorMessage(Job job, string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                errorMessage = "Pre-Validation rule(s) failed!";

            if (!errorMessage.Contains("Row[*][*]:"))
                errorMessage = "Row[*][*]:" + errorMessage;

            job.Errors.Add(errorMessage);
        }

        #endregion Private Methods
    }
}


