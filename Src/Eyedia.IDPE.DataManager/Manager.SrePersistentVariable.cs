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
using Eyedia.Core;
using System.Data;
using Eyedia.Core.Data;
using System.Diagnostics;
using Eyedia.Core;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {
        public List<SrePersistentVariable> GetPersistentVariables()
        {
            List<SrePersistentVariable> variables = new List<SrePersistentVariable>();
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand("SELECT * FROM SrePersistentVariable");
            if (table == null)
                return variables;

            foreach (DataRow row in table.Rows)
            {
                SrePersistentVariable variable = new SrePersistentVariable();
                variable.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                variable.Name = row["Name"].ToString();
                variable.Value = row["Value"].ToString();
                variable.CreatedTS = (DateTime)row["CreatedTS"].ToString().ParseDateTime();

                variables.Add(variable);
            }

            return variables;
        }

        public SrePersistentVariable GetPersistentVariable(int dataSourceId, string name, IDal dal = null, IDbConnection connection = null)
        {
            bool isLocalConnection = false;
            if (dal == null)
            {
                dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
                connection = dal.CreateConnection(_ConnectionString);
                connection.Open();
                isLocalConnection = true;
            }

            SrePersistentVariable variable = null;

            IDbCommand command = dal.CreateCommand("SELECT [Value] FROM SrePersistentVariable WHERE DataSourceId = @DataSourceId AND Name = @Name", connection);
            command.AddParameterWithValue("DataSourceId", dataSourceId);
            command.AddParameterWithValue("Name", name);
            IDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                variable = new SrePersistentVariable();
                variable.DataSourceId = dataSourceId;
                variable.Name = name;
                variable.Value = reader[0].ToString();
            }


            reader.Close();
            reader.Dispose();
            command.Dispose();

            if(isLocalConnection)
            {
                if(connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return variable;
        }

         public SrePersistentVariable SavePersistentVariable(int dataSourceId, string variableName, string variableValue)
         {
             if (string.IsNullOrEmpty(variableName))
                 throw new Exception("Variable name can not be blank!");

             IDal dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
             IDbConnection connection = dal.CreateConnection(_ConnectionString);
             connection.Open();

             IDbCommand command = dal.CreateCommand();
             command.Connection = connection;

             SrePersistentVariable variable = GetPersistentVariable(dataSourceId, variableName, dal, connection);
             if (variable == null)
             {
                 command.CommandText = "INSERT INTO SrePersistentVariable(DataSourceId, Name, Value, CreatedTS) ";
                 command.CommandText += " VALUES (@DataSourceId, @Name, @Value, @CreatedTS)";
                 command.AddParameterWithValue("DataSourceId", dataSourceId);
                 command.AddParameterWithValue("Name", variableName);
                 command.AddParameterWithValue("Value", variableValue);
                 command.AddParameterWithValue("CreatedTS", DateTime.Now);
                 command.ExecuteNonQuery();
             }
             else
             {
                 command.CommandText = "UPDATE [SrePersistentVariable] SET [Value] = @Value, [CreatedTS] = @CreatedTS ";
                 command.CommandText += "WHERE [DataSourceId] = @DataSourceId AND [Name] = @Name";
                 command.AddParameterWithValue("DataSourceId", dataSourceId);
                 command.AddParameterWithValue("Name", variableName);
                 command.AddParameterWithValue("Value", variableValue);
                 command.AddParameterWithValue("CreatedTS", DateTime.Now);

                 command.ExecuteNonQuery();
             }
             if (variable == null)
             {
                 //if we are here, then we have inserted or updated, lets just fill the object and return it
                 variable = new SrePersistentVariable();
                 variable.DataSourceId = dataSourceId;
                 variable.Name = variableName;
                 variable.Value = variableValue;
                 variable.CreatedTS = DateTime.Now;
             }
             else
             {
                 variable.Value = variableValue;
                 variable.CreatedTS = DateTime.Now;
             }
             return variable;

         }

         public void DeletePersistentVariable(int dataSourceId, string variableName = null)
         {
             IDal dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
             IDbConnection connection = dal.CreateConnection(_ConnectionString);
             connection.Open();

             IDbCommand command = dal.CreateCommand();
             command.Connection = connection;

             if (!string.IsNullOrEmpty(variableName))
             {
                 command.CommandText = "DELETE from [SrePersistentVariable] WHERE [DataSourceId] = @DataSourceId AND [Name] = @Name";
                 command.AddParameterWithValue("DataSourceId", dataSourceId);
                 command.AddParameterWithValue("Name", variableName);
             }
             else
             {
                 command.CommandText = "DELETE from [SrePersistentVariable] WHERE [DataSourceId] = @DataSourceId";
                 command.AddParameterWithValue("DataSourceId", dataSourceId);                 
             }

             command.ExecuteNonQuery();

         }

    }
}


