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
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Eyedia.Core.Net;

namespace Eyedia.Core.Data
{
    public partial class CoreDatabaseObjects
    {
        private string _ConnectionString;
        public static CoreDatabaseObjects instance;
        private static object syncRoot = new Object();

        CoreDatabaseObjects()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["cs"].ToString();
            Refresh();
        }

        CoreDatabaseObjects(string connectionString)
        {
            _ConnectionString = connectionString;
            Refresh();
        }

        public static CoreDatabaseObjects Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CoreDatabaseObjects();
                        }
                    }
                }
                return instance;
            }

        }

        public List<SymplusAVPair> SymplusAVPairs { get; private set; }
        public List<SymplusCodeSet> SymplusCodeSets { get; private set; }
        public List<SymplusGroup> SymplusGroups { get; private set; }        
        public List<SymplusUser> SymplusUsers { get; private set; }



        public bool TestConnection(string connectionString = null)
        {
            if (connectionString == null)
                connectionString = _ConnectionString;

            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(connectionString);
            IDbCommand command = myDal.CreateCommand();

            try
            {
                command.CommandText = "SELECT count(*) FROM [SymplusUser]";

                conn.Open();
                command.Connection = conn;
                command.ExecuteScalar();

            }            
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }

            return true;
        }

        public void Refresh()
        {
            this.SymplusAVPairs = GetAVPairs();
            this.SymplusCodeSets = GetCodeSets();
            this.SymplusGroups = GetGroups();
            this.SymplusUsers = GetUsers();
        }

        public int GetMaxId(string tableName)
        {
            object objMaxId = ExecuteScalar(string.Format("select max(id) from [{0}] ", tableName));
            int maxId = 0;
            int.TryParse(objMaxId.ToString(), out maxId);
            return maxId;

        }

        public DataTable ExecuteCommand(string commandText, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            DataTable table = new DataTable();
            if (dal == null)
            {
                dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
                connection = dal.CreateConnection(_ConnectionString);
                connection.Open();
                IDbCommand command = dal.CreateCommand(commandText, connection);
                IDataReader reader = command.ExecuteReader();
                table.Load(reader);
                reader.Close();
                reader.Dispose();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else
            {
                IDbCommand command = dal.CreateCommand(commandText, connection, transaction);
                IDataReader reader = command.ExecuteReader();
                table.Load(reader);
                reader.Close();
                reader.Dispose();
                command.Dispose();
            }
            return table;
        }

        public object ExecuteScalar(string commandText, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            object returnObject = null;
            if (dal == null)
            {
                dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
                connection = dal.CreateConnection(_ConnectionString);
                connection.Open();
                IDbCommand command = dal.CreateCommand(commandText, connection);
                returnObject = command.ExecuteScalar();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else
            {
                if (connection.State != ConnectionState.Open) 
                    connection.Open();
                IDbCommand command = dal.CreateCommand(commandText, connection);
                returnObject = command.ExecuteScalar();
                command.Dispose();
            }
            return returnObject;
        }

        public void ExecuteStatement(string statement, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            bool localTransaction = false;
            IDbCommand command = null;
            if (dal == null)
            {
                dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
                connection = dal.CreateConnection(_ConnectionString);
                connection.Open();
                transaction = dal.CreateTransaction(connection);
                localTransaction = true;
            }
          
            try
            {
                command = dal.CreateCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = statement;
                if (connection.State != ConnectionState.Open) connection.Open();
                command.ExecuteNonQuery();
                if(localTransaction)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                if (localTransaction)
                    transaction.Rollback();               
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (localTransaction)
                {
                    if (connection.State == ConnectionState.Open) connection.Close();
                    connection.Dispose();
                    transaction.Dispose();
                }
            }

        }

        private string GetSource(string source = null)
        {
            return source == null ? Dns.GetLocalIpAddress() : source;
        }
    }
}


