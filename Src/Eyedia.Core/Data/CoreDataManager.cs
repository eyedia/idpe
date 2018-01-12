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

namespace Symplus.Core.Data
{
    public class CoreDataManager
    {
        private string _ConnectionString;

        public CoreDataManager()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["cs"].ToString();         
        }

        public CoreDataManager(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public bool TestConnection(string connectionString = null)
        {
            if (connectionString == null)
                connectionString = _ConnectionString;

            IDal myDal = new DataAccessLayer(SymplusCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(connectionString);
            IDbCommand command = myDal.CreateCommand();

            try
            {
                command.CommandText = "SELECT count(*) FROM [User]";

                conn.Open();
                command.Connection = conn;
                command.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }

            return true;
        }

        public List<User> GetUsers()
        {           
            List<User> users = new List<User>();
            IDal myDal = new DataAccessLayer(SymplusCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand("SELECT * FROM [User]", conn);          
            IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User();
                user.Id = int.Parse(reader["Id"].ToString());
                user.FullName = reader["FullName"].ToString();
                user.UserName = reader["UserName"].ToString();
                user.Password = reader["Password"].ToString();
                user.EmailId = reader["EmailId"].ToString();
                user.GroupId = int.Parse(reader["GroupId"].ToString());

                users.Add(user);
            }
            return users;
        }

        public int GetUserId(IDal myDal, IDbConnection conn, IDbTransaction transaction, string userName)
        {
            int userId = 0;
            IDbCommand command = myDal.CreateCommand("SELECT Id FROM [User] WHERE UserName = @UserName", conn);
            command.AddParameterWithValue("UserName", userName);
            command.Transaction = transaction;
            IDataReader reader = command.ExecuteReader();
            if (reader.Read())
                userId = int.Parse(reader[0].ToString());
            reader.Close();
            reader.Dispose();
            command.Dispose();
            return userId;
        }


        public void Save(User user)
        {
            IDal myDal = new DataAccessLayer(SymplusCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbTransaction transaction = myDal.CreateTransaction(conn);
            IDbCommand command = myDal.CreateCommand();
            command.Connection = conn;
            command.Transaction = transaction;

            try
            {
                int userId = GetUserId(myDal, conn, transaction, user.UserName);
                if (userId == 0)
                {
                    command.CommandText = "INSERT INTO [User] (FullName, UserName, Password, EmailId, GroupId) ";
                    command.CommandText += " VALUES (@FullName, @UserName, @Password, @EmailId, @GroupId)";
                    command.AddParameterWithValue("FullName", user.FullName);
                    command.AddParameterWithValue("UserName", user.UserName);
                    command.AddParameterWithValue("Password", user.Password);
                    command.AddParameterWithValue("EmailId", user.EmailId);
                    command.AddParameterWithValue("GroupId", user.GroupId);
                    command.ExecuteNonQuery();

                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.Parameters.Clear();
                    command.CommandText = "SELECT max(Id) from [User]";
                    int newCodeSetId = (Int32)command.ExecuteScalar();

                }
                else
                {
                    command.CommandText = "UPDATE [User] SET [FullName] = @FullName,[UserName] = @UserName,[Password] = @Password,[EmailId] = @EmailId ";
                    command.CommandText += "WHERE [Id] = @Id";

                    command.AddParameterWithValue("FullName", user.FullName);
                    command.AddParameterWithValue("UserName", user.UserName);
                    command.AddParameterWithValue("Password", user.Password);
                    command.AddParameterWithValue("EmailId", user.EmailId);
                    command.AddParameterWithValue("Id", userId);
                    command.ExecuteNonQuery();


                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Trace.TraceError(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
                transaction.Dispose();
            }

        }

        public bool Authenticate(string userName, string password)
        {
            IDal myDal = new DataAccessLayer(SymplusCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand();
            string dbPassword = string.Empty;

            try
            {
                command.CommandText = "SELECT [Password] FROM [User] WHERE [UserName] = @UserName";
                command.AddParameterWithValue("UserName", userName);


                conn.Open();
                command.Connection = conn;
                dbPassword = (string)command.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }

            return password == dbPassword;
        }
    }
}


