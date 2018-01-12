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
using Eyedia.Core.Data;
using System.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Sql Query type pusher - executes sql query when file processed
    /// </summary>
    public class PusherSqlQuery : Pushers
    {
        /// <summary>
        /// Default instance of PusherSqlQuery 
        /// </summary>
        public PusherSqlQuery() { }

        /// <summary>
        /// Executes SQL query
        /// </summary>
        /// <param name="e">PullerEventArgs</param>
        public override void FileProcessed(PullersEventArgs e)
        {

            if (e.Job.DataSource.PusherTypeFullName.Contains("|"))
            {
                string[] info = e.Job.DataSource.PusherTypeFullName.Split("|".ToCharArray());

                string connectionStringKeyNameDesign = info[0];
                string connectionStringKeyName = info[1];
                string updateQuery = info[2];

                IdpeKey connectionStringKey = null;
                if (!string.IsNullOrEmpty(connectionStringKeyName))
                {
                    string connectionKeyNameRuntime = e.Job.GetProcessVariableValue(connectionStringKeyName).ToString();
                    if (string.IsNullOrEmpty(connectionKeyNameRuntime))
                        throw new Exception(string.Format("Database writer run time connection string '{0}' was not defined!", connectionStringKeyName));
                    connectionStringKey = e.Job.DataSource.Keys.GetKey(connectionKeyNameRuntime);
                }
                else
                {
                    connectionStringKey = e.Job.DataSource.Keys.GetKey(connectionStringKeyNameDesign);
                }

                if (connectionStringKey == null)
                    throw new Exception("Database writer connection string was not defined!");

                if (!string.IsNullOrEmpty(updateQuery))
                    ExecuteQuery(e.Job.DataSource, connectionStringKey, updateQuery);
            }

        }

        #region Private Methods

        void ExecuteQuery(DataSource dataSource, IdpeKey connectionStringKey, string updateQuery)
        {
            //SreKey connectionStringKey = dataSource.Keys.GetKey(connectionStringKeyName);
            //if (connectionStringKey == null)
            //    throw new KeyNotFoundException(string.Format("The connection string '{0}' was not defined!", connectionStringKeyName));

            DatabaseTypes databaseType = connectionStringKey.GetDatabaseType();
            string actualConnectionString = connectionStringKey.Value;

            IDal myDal = new DataAccessLayer(databaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(actualConnectionString);
            conn.Open();

            IDbTransaction transaction = myDal.CreateTransaction(conn);

            IDbCommand commandUpdate = myDal.CreateCommand();
            commandUpdate.Connection = conn;
            commandUpdate.Transaction = transaction;
            commandUpdate.CommandText = new SreCommandParser(dataSource).Parse(updateQuery);


            try
            {
                commandUpdate.ExecuteNonQuery();
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ExtensionMethods.TraceError(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
                commandUpdate.Dispose();
            }
        }

        #endregion Private Methods
    }
}


