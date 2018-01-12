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
using System.Runtime.InteropServices;

namespace Eyedia.Core.Data
{
    public partial class CoreDatabaseObjects
    {
        [DllImport(@"Symplus.Security.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SymplusAuthenticate(string user, string password);

        [DllImport(@"Symplus.Security.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Encrypt(StringBuilder plaintext, int length);

        [DllImport(@"Symplus.Security.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int IsEqual(StringBuilder plaintext, int plaintextlength, StringBuilder enctext, int enctextlength);

        private List<User> GetUsers()
        {
            List<User> users = new List<User>();
            DataTable table = ExecuteCommand("SELECT [Id],[FullName],[UserName],[Password],[EmailId],[Password],[Preferences],[GroupId], [IsDebugUser], [IsSystemUser] FROM [User]");            
            if (table == null)
                return users;

            foreach (DataRow row in table.Rows)
            {
                User user = new User();
                user.Id = (int)row["Id"].ToString().ParseInt();
                user.FullName = row["FullName"].ToString();
                user.UserName = row["UserName"].ToString();
                user.Password = row["Password"].ToString();
                user.EmailId = row["EmailId"].ToString();
                user.Preferences = row["Preferences"].ToString();
                user.IsDebugUser = row["IsDebugUser"].ToString().ParseBool();
                user.IsSystemUser = row["IsSystemUser"].ToString().ParseBool();
                user.GroupId = (int)row["GroupId"].ToString().ParseInt();

                users.Add(user);
            }

            table.Dispose();
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

        public User GetUser(string userName)
        {
            userName = userName.Replace("'", "''");
            DataTable table = ExecuteCommand("SELECT [Id],[FullName],[UserName],[Password],[EmailId],[Password],[Preferences],[GroupId], [IsDebugUser], [IsSystemUser] FROM [User] WHERE UserName = '" + userName + "'");

             if (table.Rows.Count == 1)
             {
                 User user = new User();
                 user.Id = (int)table.Rows[0]["Id"].ToString().ParseInt();
                 user.FullName = table.Rows[0]["FullName"].ToString();
                 user.UserName = table.Rows[0]["UserName"].ToString();
                 user.Password = table.Rows[0]["Password"].ToString();
                 user.EmailId = table.Rows[0]["EmailId"].ToString();
                 user.Preferences = table.Rows[0]["Preferences"].ToString();
                 user.IsDebugUser = table.Rows[0]["IsDebugUser"].ToString().ParseBool();
                 user.IsSystemUser = table.Rows[0]["IsSystemUser"].ToString().ParseBool();
                 user.GroupId = (int)table.Rows[0]["GroupId"].ToString().ParseInt();

                 return user;
             }
            return null;
        }

        private string EncryptPassword(string password)
        {            
            StringBuilder sbPassword = new StringBuilder(password);
            Encrypt(sbPassword, sbPassword.Length);
            return sbPassword.ToString();
        }

        public int Save(User user)
        {
            if ((user.IsDebugUser != true) && (string.IsNullOrEmpty(user.Password)))
                throw new Exception("Password cannot be blank!");

            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbTransaction transaction = myDal.CreateTransaction(conn);
            IDbCommand command = myDal.CreateCommand();
            command.Connection = conn;
            command.Transaction = transaction;
            int userId = 0;
            try
            {
                userId = GetUserId(myDal, conn, transaction, user.UserName);
                if (userId == 0)
                {
                    command.CommandText = "INSERT INTO [User] (FullName, UserName, Password, EmailId, [Preferences], GroupId, IsDebugUser, IsSystemUser) ";
                    command.CommandText += " VALUES (@FullName, @UserName, @Password, @EmailId, @Preferences, @GroupId, @IsDebugUser, @IsSystemUser)";
                    command.AddParameterWithValue("FullName", user.FullName);
                    command.AddParameterWithValue("UserName", user.UserName);
                    if(user.IsDebugUser != true)
                        command.AddParameterWithValue("Password", EncryptPassword(user.Password));
                    else
                        command.AddParameterWithValue("Password", "");      //only in case of debug

                    if (!string.IsNullOrEmpty(user.EmailId))
                        command.AddParameterWithValue("EmailId", user.EmailId);
                    else
                        command.AddParameterWithValue("EmailId", DBNull.Value);

                    if (!string.IsNullOrEmpty(user.Preferences))
                        command.AddParameterWithValue("Preferences", user.Preferences);
                    else
                        command.AddParameterWithValue("Preferences", DBNull.Value);

                    if (user.GroupId != null)
                        command.AddParameterWithValue("GroupId", user.GroupId);
                    else
                        command.AddParameterWithValue("GroupId", DBNull.Value);

                    if (user.IsDebugUser != null)
                        command.AddParameterWithValue("IsDebugUser", user.IsDebugUser);
                    else
                        command.AddParameterWithValue("IsDebugUser", DBNull.Value);

                    if (user.IsSystemUser != null)
                        command.AddParameterWithValue("IsSystemUser", user.IsSystemUser);
                    else
                        command.AddParameterWithValue("IsSystemUser", DBNull.Value);

                    command.ExecuteNonQuery();

                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.Parameters.Clear();
                    command.CommandText = "SELECT max(Id) from [User]";
                    userId = (Int32)command.ExecuteScalar();

                }
                else
                {
                    command.CommandText = "UPDATE [User] SET [FullName] = @FullName,[UserName] = @UserName,[EmailId] = @EmailId,[Preferences] = @Preferences, [GroupId] = @GroupId, [IsDebugUser] = @IsDebugUser, [IsSystemUser] = @IsSystemUser ";
                    command.CommandText += "WHERE [Id] = @Id";

                    command.AddParameterWithValue("FullName", user.FullName);
                    command.AddParameterWithValue("UserName", user.UserName);                    

                    if (!string.IsNullOrEmpty(user.EmailId))
                        command.AddParameterWithValue("EmailId", user.EmailId);
                    else
                        command.AddParameterWithValue("EmailId", DBNull.Value);

                    if (!string.IsNullOrEmpty(user.Preferences))
                        command.AddParameterWithValue("Preferences", user.Preferences);
                    else
                        command.AddParameterWithValue("Preferences", DBNull.Value);

                    if (user.GroupId != null)
                        command.AddParameterWithValue("GroupId", user.GroupId);
                    else
                        command.AddParameterWithValue("GroupId", DBNull.Value);

                    if (user.IsDebugUser != null)
                        command.AddParameterWithValue("IsDebugUser", user.IsDebugUser);
                    else
                        command.AddParameterWithValue("IsDebugUser", DBNull.Value);

                    if (user.IsSystemUser != null)
                        command.AddParameterWithValue("IsSystemUser", user.IsSystemUser);
                    else
                        command.AddParameterWithValue("IsSystemUser", DBNull.Value);

                    command.AddParameterWithValue("Id", userId);
                    command.ExecuteNonQuery();


                }

                transaction.Commit();
                CoreDatabaseObjects.Instance.Refresh();
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
                transaction.Dispose();
            }
            return userId;
        }

        public void ChangePassword(User user, string newPassword)
        {           
            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();

            IDbCommand command = myDal.CreateCommand();
            command.Connection = conn;
            
            command.CommandText = "UPDATE [User] SET [Password] = @Password ";
            command.CommandText += "WHERE [Id] = @Id";

            command.AddParameterWithValue("Password", EncryptPassword(newPassword));
            command.AddParameterWithValue("Id", user.Id);
            command.ExecuteNonQuery();

            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Dispose();

        }

        public void UpdateUserPreferences(User user)
        {
            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
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
                    userId = Save(user);    //debugger only

                command.CommandText = "UPDATE [User] SET [Preferences] = @Preferences ";
                command.CommandText += "WHERE [Id] = @Id";

                command.AddParameterWithValue("Preferences", user.Preferences);
                command.AddParameterWithValue("Id", userId);
                command.ExecuteNonQuery();
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
                transaction.Dispose();
            }

        }

        public User Authenticate(string userName, string password, string connectionString = null)
        {
            if (connectionString == null)
                connectionString = _ConnectionString;

            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(connectionString);
            IDbCommand command = myDal.CreateCommand();
            string dbPassword = string.Empty;
            bool foundInDb = false; //root user's password has been changed
            try
            {
                command.CommandText = "SELECT [Password] FROM [User] WHERE [UserName] = @UserName";
                command.AddParameterWithValue("UserName", userName);


                conn.Open();
                command.Connection = conn;
                dbPassword = (string)command.ExecuteScalar();
                if(dbPassword != null)
                    foundInDb = true;
            }           
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }


            StringBuilder sbPassword = new StringBuilder(password);
            StringBuilder sbEncryptedPassword = new StringBuilder(dbPassword);

            if (IsEqual(sbPassword, sbPassword.Length, sbEncryptedPassword, sbEncryptedPassword.Length) == 1)
            {
                return GetUser(userName);
            }
            else if(foundInDb == false)
            {
                //default hardcoded users
                if (CoreDatabaseObjects.SymplusAuthenticate(userName, password) > 0)
                {
                    User user = new User();
                    user.UserName = userName;
                    user.Password = password;
                    user.FullName = "Copied Root User";
                    user.UserName = user.UserName;
                    user.IsDebugUser = false;
                    user.IsSystemUser = true;
                    Group adminGroup = CoreDatabaseObjects.Instance.GetAdminGroup();
                    if (adminGroup != null)
                        user.GroupId = adminGroup.Id;
                    CoreDatabaseObjects.Instance.Save(user);
                    return user;
                }

            }
            return null;
        }
    }
}


