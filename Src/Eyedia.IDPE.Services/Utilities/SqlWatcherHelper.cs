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
using System.Data;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    public class SqlWatcherHelper
    {
        public Job Job { get; private set; }

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
                return Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.PullSqlConnectionString);

            }
        }

        public string ConnectionStringNameRunTime
        {
            get
            {
                return Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.PullSqlConnectionStringRunTime);

            }
        }

        public IdpeKey ConnectionStringKey
        {
            get
            {
                if (!string.IsNullOrEmpty(ConnectionStringNameRunTime))
                {
                    object rtConnectionStringName = Job.GetProcessVariableValue(ConnectionStringNameRunTime);
                    IdpeKey key = Job.DataSource.Keys.GetKey(rtConnectionStringName.ToString());

                    if (key == null)
                        throw new Exception(string.Format("{0} - A database connection '{1}' was not defined!", Job.DataSource.Name, rtConnectionStringName));

                    //DataSource.TraceInformation(__tracePrefix + "Run time connection string '{1}' was set!", DataSource.Name, rtConnectionStringName);
                    return key;
                }
                else
                {
                    return Job.DataSource.Keys.GetKey(ConnectionStringName);
                }

            }
        }

        public SqlWatcherHelper(Job job)
        {
            this.Job = job;
        }

        public void ExecuteRecoveryScript()
        {
            //IdpeKey connectionStringNameKey = Job.DataSource.Keys.GetKey(SreKeyTypes.PullSqlConnectionString.ToString());           
            //IdpeKey connectionStringKey = Job.DataSource.Keys.GetKey(connectionStringNameKey.Value);
            //DatabaseTypes databaseType = connectionStringKey.GetDatabaseType();
           // string actualConnectionString = connectionStringKey.Value;


            IdpeKey recoveryQueryKey = Job.DataSource.Keys.GetKey(IdpeKeyTypes.SqlUpdateQueryRecovery.ToString());
            string recoveryQuery = new CommandParser(Job.DataSource).Parse(recoveryQueryKey.Value);

            ExecuteQuery(recoveryQuery);

            string message = "Pullers - There were error(s) while processing a SQL pull cycle, the pusher was not called. Please study the error(s) and do the needful before next cycle.";
            message += ". The recovery script has been executed.";
            message = PostMan.__warningStartTag + message + PostMan.__warningEndTag;

            new PostMan(this.Job).Send(message, "Recovery Script Executed", true);
        }

        #region Helper Methods

        void ExecuteQuery(string query)
        {
            IDal myDal = new DataAccessLayer(DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(ConnectionStringKey.Value);
            conn.Open();

            IDbTransaction transaction = myDal.CreateTransaction(conn);

            IDbCommand commandUpdate = myDal.CreateCommand();
            commandUpdate.Connection = conn;
            commandUpdate.Transaction = transaction;
            commandUpdate.CommandText = query;

            try
            {
                commandUpdate.ExecuteNonQuery();
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Job.TraceError(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
                commandUpdate.Dispose();
            }
        }

        #endregion Helper Methods
    }
}


