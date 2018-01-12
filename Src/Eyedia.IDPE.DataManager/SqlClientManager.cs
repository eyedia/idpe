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
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using Eyedia.IDPE.Common;
using Eyedia.Core.Data;
using Eyedia.Core;


namespace Eyedia.IDPE.DataManager
{
    public class SqlClientManager : IDisposable
    {        
        string _ConnectionString;
        SreKeyTypes _DatabaseType;
        IDal _Dal = null;
        private bool disposed = false;


        public SqlClientManager(string defaultConnectionString, SreKeyTypes databaseType)
        {
            _ConnectionString = defaultConnectionString;
            _DatabaseType = databaseType;            
            _Dal = new DataAccessLayer(GetDalDBType(databaseType)).Instance;
        }

        ~SqlClientManager()
        {
            Dispose(false);
        }


        #region IDisposable

       
        public void Dispose()
        {
            Dispose(true);            
            GC.SuppressFinalize(this);
        }

     
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {              
                if (disposing)
                {   
                    //component.Dispose();
                }
                
                disposed = true;

            }
        }


        #endregion IDisposable

        #region Private Methods
        DatabaseTypes GetDalDBType(SreKeyTypes keytype)
        {
            DatabaseTypes dalDbType = DatabaseTypes.SqlCe;
            switch (keytype)
            {
                case SreKeyTypes.ConnectionStringSqlCe:
                    dalDbType = DatabaseTypes.SqlCe;
                    break;                
                case SreKeyTypes.ConnectionStringSqlServer:
                    dalDbType = DatabaseTypes.SqlServer;
                    break;
                case SreKeyTypes.ConnectionStringOracle:
                    dalDbType = DatabaseTypes.Oracle;
                    break;
                case SreKeyTypes.ConnectionStringDB2iSeries:
                    dalDbType = DatabaseTypes.DB2iSeries;
                    break;
            }
            return dalDbType;
        }
        #endregion Private Methods
        
        public void CheckCodeSet(string connectionString, SreKeyTypes databaseType, string tableName, string code, ref string value, ref int enumCode, ref string referenceKey, ref string errorMessage)
        {
            errorMessage = "ERROR";
            bool DBReturnedNothing = true;
            IDbConnection con = null;
            IDbCommand command = null;
            IDataReader reader = null;


            IDal myDal = null;
            string myConnectionString = string.Empty;

            if (!string.IsNullOrEmpty(connectionString))
            {
                myDal = new DataAccessLayer(GetDalDBType(databaseType)).Instance;   //use current
                myConnectionString = connectionString;                              //use current                
            }
            else
            {
                myDal = _Dal;     //use default
                myConnectionString = _ConnectionString;     //use default
            }

            string commandText = string.Empty;
            try
            {
                con = myDal.CreateConnection(myConnectionString);
                con.Open();
                commandText = string.Format("select [value],[enumcode],isnull([ReferenceKey],'') from {0} with(nolock) where Code = '{1}'", tableName, code);
                command = myDal.CreateCommand(commandText, con);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DBReturnedNothing = false;
                    if (reader.GetInt32(1) == enumCode)
                    {
                        enumCode = reader.GetInt32(1);
                        referenceKey = reader.GetString(2);
                        errorMessage = string.Empty;
                        break;
                    }
                    else if (reader.GetString(0).ToUpper().Equals(value.ToUpper()))
                    {
                        enumCode = int.Parse(reader.GetInt32(1).ToString());
                        referenceKey = reader.GetString(2);
                        errorMessage = string.Empty;
                        break;
                    }
                }


                if (DBReturnedNothing)
                {
                    //lets trace more information, error is anyway returned back to caller
                    string errMsg = string.Format("Error: Nothing retrieved from DB while checking codeset with code:'{0}', value:'{1}',table name:'{2}', SQL Query ='{3}'",
                        code, value, tableName, commandText);
                    Trace.TraceError(errMsg);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while checking codeset." + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                if (con != null)
                {
                    if(con.State != System.Data.ConnectionState.Closed) con.Close();
                }
                reader.Dispose();
                command.Dispose();
                con.Close();
                con.Dispose();
            }            
        }

        public bool CheckReferenceKey(string connectionString, SreKeyTypes databaseType, string query, string data)
        {
            bool found = false;
            IDbConnection con = null;
            IDbCommand command = null;
            IDataReader reader = null;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;

            string myConnectionString = string.Empty;
            if (!string.IsNullOrEmpty(connectionString))
            {
                myDal = new DataAccessLayer(GetDalDBType(databaseType)).Instance;   //use current
                myConnectionString = connectionString;
            }
            else
            {
                myDal = _Dal;                           //use default
                myConnectionString = _ConnectionString; //use default
            }

            try
            {
                con = myDal.CreateConnection(myConnectionString);
                con.Open();
                command = myDal.CreateCommand(query, con);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetString(0).ToUpper().Equals(data.ToUpper()))
                    {
                        found = true;
                        break;
                    }
                }
               
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while checking reference key." + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State != System.Data.ConnectionState.Closed) con.Close();
                }
                reader.Dispose();
                command.Dispose();
                con.Close();
                con.Dispose();
            }
            return found;
        }

        public string ExecuteQuery(string query, ref bool isErrored)
        {
            return ExecuteQuery(string.Empty,SreKeyTypes.Unknown, query, ref isErrored);
        }
        

        public string ExecuteQuery(string connectionString, SreKeyTypes databaseType, string query, ref bool isErrored)
        {
            string returnString = "NULL";
            isErrored = false;
            IDbConnection con = null;
            IDbCommand command = null;
            IDataReader reader = null;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            string myConnectionString = string.Empty;
            if (!string.IsNullOrEmpty(connectionString))
            {
                myDal = new DataAccessLayer(GetDalDBType(databaseType)).Instance;   //use current
                myConnectionString = connectionString;
            }
            else
            {
                myDal = _Dal;                               //use default
                myConnectionString = _ConnectionString;     //use default
            }

            try
            {
                con = myDal.CreateConnection(myConnectionString);
                con.Open();
                command = myDal.CreateCommand(query, con);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.FieldCount == 1)
                    {
                        returnString = reader[0].ToString();
                    }
                    else
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            returnString += reader[i] == null ? "NULL" : reader[i].ToString() + "|";
                        }
                        if (returnString.Length > 0)
                            returnString = returnString.Substring(0, returnString.Length - 1);
                    }
                }


            }
            catch (Exception ex)
            {
                isErrored = true;
                Trace.TraceError("Error while executing query '{0}'{1}{2} on '{3}'.", query, Environment.NewLine, ex.Message, GetDatabaseName(myConnectionString));
               
            }
            finally
            {
                if (con != null)
                {
                    if (con.State != System.Data.ConnectionState.Closed) con.Close();
                }
                reader.Dispose();
                command.Dispose();
                con.Close();
                con.Dispose();
            }
            return returnString;
        }

        private string GetDatabaseName(string connectionString)
        {
            try
            {
                System.Data.Common.DbConnectionStringBuilder csBuilder = new System.Data.Common.DbConnectionStringBuilder();
                csBuilder.ConnectionString = connectionString;
                string server = csBuilder["server"] as string;
                string database = csBuilder["database"] as string;
                return string.Format("{0};Database={1}", server, database);
            }
            catch(Exception ex)
            {
                return string.Format("<DB Name>-{0}", ex.ToString());
            }
        }

        public int ExecuteNonQuery(string query, bool silent = false, string connectionString = null, SreKeyTypes databaseType = SreKeyTypes.ConnectionStringSqlCe)
        {
            string returnString = string.Empty;

            IDbConnection con = null;
            IDbCommand command = null;
            int noOfRowsAffected = 0;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            string myConnectionString = string.Empty;
            if (!string.IsNullOrEmpty(connectionString))
            {
                myDal = new DataAccessLayer(GetDalDBType(databaseType)).Instance;   //use current
                myConnectionString = connectionString;
            }
            else
            {
                myDal = _Dal;                               //use default
                myConnectionString = _ConnectionString;     //use default
            }

            try
            {
                con = myDal.CreateConnection(myConnectionString);
                con.Open();
                command = myDal.CreateCommand(query, con);
                noOfRowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if(!silent)
                    Trace.TraceError("Error while executing query '{0}'{1}{2} on '{3}'.", query, Environment.NewLine, ex.Message, GetDatabaseName(myConnectionString));
            }
            finally
            {
                if (con != null)
                {
                    if (con.State != System.Data.ConnectionState.Closed) con.Close();
                }
                command.Dispose();
                con.Close();
                con.Dispose();
            }
            return noOfRowsAffected;
        }

        public DataTable ExecuteQueryAndGetDataTable(string query, ref bool isErrored)
        {
            return ExecuteQueryAndGetDataTable(string.Empty, SreKeyTypes.Unknown, query, ref isErrored);
        }

        public DataTable ExecuteQueryAndGetDataTable(string connectionString, SreKeyTypes databaseType, string query, ref bool isErrored)
        {
            string returnString = string.Empty;
            isErrored = false;
            IDbConnection con = null;
            IDbCommand command = null;
            DataTable dataTable = new DataTable();

            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            string myConnectionString = string.Empty;
            
            if (!string.IsNullOrEmpty(connectionString))
            {
                myDal = new DataAccessLayer(GetDalDBType(databaseType)).Instance;   //use current
                myConnectionString = connectionString;
            }
            else
            {
                myDal = _Dal;                               //use default
                myConnectionString = _ConnectionString;     //use default
            }

            try
            {
                con = myDal.CreateConnection(myConnectionString);
                con.Open();
                command = myDal.CreateCommand(query, con);
                dataTable.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                isErrored = true;
                Trace.TraceError("Error while executing query '{0}'{1}{2} on '{3}'.", query, Environment.NewLine, ex.Message, GetDatabaseName(myConnectionString));
            }
            finally
            {
                if (con != null)
                {
                    if (con.State != System.Data.ConnectionState.Closed) con.Close();
                }                
                if(command != null) command.Dispose();                
                if(con != null) con.Dispose();
            }
            return dataTable;
        }

        public DataTable ExecuteQueryAndGetDataTable(string connectionString, SreKeyTypes databaseType, string query, ref string errorMessage, int timeOut = 5, bool silent = false)
        {            
            string returnString = string.Empty;            
            IDbConnection con = null;
            IDbCommand command = null;
            DataTable dataTable = new DataTable();

            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            string myConnectionString = string.Empty;

            if (!string.IsNullOrEmpty(connectionString))
            {
                myDal = new DataAccessLayer(GetDalDBType(databaseType)).Instance;   //use current
                myConnectionString = connectionString;
            }
            else
            {
                myDal = _Dal;                               //use default
                myConnectionString = _ConnectionString;     //use default
            }

            try
            {
                con = myDal.CreateConnection(myConnectionString);
                con.Open();
                command = myDal.CreateCommand(query, con);
                command.CommandTimeout = 60 * timeOut;    //5 Minutes
                dataTable.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if(!silent)
                    Trace.TraceError("Error while executing query '{0}'{1}{2} on '{3}'.", query, Environment.NewLine, ex.Message, GetDatabaseName(myConnectionString));
            }
            finally
            {
                if (con != null)
                {
                    if (con.State != System.Data.ConnectionState.Closed) con.Close();
                }
                if (command != null) command.Dispose();
                if (con != null) con.Dispose();
            }
            return dataTable;
        }

        public List<string> GetTableNames()
        {
            List<string> listOfTables = new List<string>();
            string commandText = string.Empty;          
            switch (this._DatabaseType)
            {
                case SreKeyTypes.ConnectionStringSqlCe:
                    commandText = "select table_name from information_schema.tables where TABLE_TYPE <> 'VIEW'";
                    break;

                case SreKeyTypes.ConnectionStringSqlServer:
                    commandText = "select [Name] from sys.tables where type = 'U' order by name";                   
                    break;

                case SreKeyTypes.ConnectionStringOracle:
                    commandText = "select tname from tab order by tname";                   
                    break;
            }            
            bool isErrored = false;
            DataTable table = ExecuteQueryAndGetDataTable(commandText, ref isErrored);
            if (table == null || table.Rows.Count == 0)
            {
                return listOfTables;
            }
            else
            {
                foreach (DataRow row in table.Rows)
                {
                    listOfTables.Add(row[0].ToString());
                }
            }
            return listOfTables;
        }

        public DataTable GetTableColumnNames(string tableName)
        {
            List<string> TableList = new List<string>();
            string commandText = string.Empty;
            switch (this._DatabaseType)
            {
                case SreKeyTypes.ConnectionStringSqlCe:
                    commandText = string.Format("select ordinal_position ColumnId, column_name ColumnName, data_type DataType from information_schema.columns where TABLE_NAME = '{0}' order by column_name",
                     tableName);
                    break;

                case SreKeyTypes.ConnectionStringSqlServer:
                    commandText = string.Format("SELECT Column_Id, c.name 'ColumnName',t.Name 'Datatype' FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id and object_id = object_id('{0}') and c.is_identity = 0 order by Column_id",
                        tableName);
                    break;

                case SreKeyTypes.ConnectionStringOracle:
                    commandText = string.Format("Select COLUMN_ID ColumnId , COLUMN_NAME ColumnName, data_type DataType from all_tab_columns where table_name = '{0}'",  
                        tableName);
                    break;
            }
            bool isErrored = false;
            DataTable table = ExecuteQueryAndGetDataTable(commandText, ref isErrored);
            return table;
        }

        public Dictionary<string, string> GenerateParameters(int applicationId)
        {            
            Dictionary<string, string> returnParams = new Dictionary<string, string>();
            IDbConnection con = null;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            try
            {
                //this will be called using default connectionstring of the application
                con = myDal.CreateConnection(_ConnectionString);

                StoredProcedure sp = new StoredProcedure(myDal);
                sp.ConnectionString = _ConnectionString;
                IDbDataParameter dp1 = sp.CreateParameter();
                dp1.ParameterName = "@applicationId";
                dp1.DbType = DbType.Int16;
                dp1.Value = applicationId;

                sp.CacheDisconnectData = true;
                sp.Name = "SRE_GENERATE_PARAMETERS";
                
                sp.AddParameter(dp1);
                con.Open();

                IDataReader reader = sp.CreateDataReader();
                reader.Read();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    returnParams.Add(reader.GetName(i), reader.GetValue(i).ToString());
                }                    
               
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while generating parameters " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                
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
            return returnParams;

        }

        public string GenerateParameter(string connectionString, SreKeyTypes databaseType, string paramName, string query)
        {
            bool isErrored = false;
            return ExecuteQuery(connectionString,databaseType, query, ref isErrored);
        }

      
    }
}







