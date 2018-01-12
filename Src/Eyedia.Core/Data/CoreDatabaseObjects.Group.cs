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

namespace Eyedia.Core.Data
{
    public partial class CoreDatabaseObjects
    {

        public List<Group> GetGroups()
        {
            DataTable table = ExecuteCommand("select [Id],[Name],[Description] from [Group]");
            List<Group> groups = new List<Group>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Group group = new Group();
                group.Id = (int)table.Rows[i]["Id"].ToString().ParseInt();
                group.Name = table.Rows[i]["Name"].ToString();
                group.Description = table.Rows[i]["Description"].ToString();
                groups.Add(group);
            }
            return groups;
        }

        public Group GetAdminGroup()
        {
            return CoreDatabaseObjects.Instance.Groups.Where(g => g.Name.ToLower() == "admins").SingleOrDefault();
        }

        public Group GetGroup(string groupName)
        {
            DataTable table = ExecuteCommand("select [Id],[Name],[Description] from [Group] where [Name] ='" + groupName + "'");
            
            if (table.Rows.Count == 1)
            {
                Group group = new Group();
                group.Id = (int)table.Rows[0]["Id"].ToString().ParseInt();
                group.Name = table.Rows[0]["Name"].ToString();
                group.Description = table.Rows[0]["Description"].ToString();
                return group;
            }
            return null;
        }

        public Group GetGroup(int groupId)
        {
            DataTable table = ExecuteCommand("select [Id],[Name],[Description] from [Group] where [Id] =" + groupId);

            if (table.Rows.Count == 1)
            {
                Group group = new Group();
                group.Id = (int)table.Rows[0]["Id"].ToString().ParseInt();
                group.Name = table.Rows[0]["Name"].ToString();
                group.Description = table.Rows[0]["Description"].ToString();
                return group;
            }
            return null;
        }

        public void Save(Group group)
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
                if (group.Id == 0)
                {
                    command.CommandText = "INSERT INTO [Group] (Name, Description) ";
                    command.CommandText += " VALUES (@Name, @Description)";
                    command.AddParameterWithValue("Name", group.Name);
                    command.AddParameterWithValue("Description", group.Description);                    
                    command.ExecuteNonQuery();

                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.Parameters.Clear();
                    command.CommandText = "SELECT max(Id) from [Group]";
                    int newCodeSetId = (Int32)command.ExecuteScalar();

                }
                else
                {
                    command.CommandText = "UPDATE [Group] SET [Name] = @Name,[Description] = @Description ";
                    command.CommandText += "WHERE [Id] = @Id";

                    command.AddParameterWithValue("Name", group.Name);
                    command.AddParameterWithValue("Description", group.Description);                    
                    command.AddParameterWithValue("Id", group.Id);
                    command.ExecuteNonQuery();

                }

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
        
    }
}


