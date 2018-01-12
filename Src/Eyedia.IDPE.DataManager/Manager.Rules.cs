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
using Eyedia.Core;
using System.Globalization;
using System.Diagnostics;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {
        public List<IdpeRule> GetRules()
        {
            List<IdpeRule> sreRules = new List<IdpeRule>();
            string commandText = "select [Id],[Name],[Description],[Xaml],[Priority],[RuleSetType],[CreatedTS],[CreatedBy] from [SreRule] order by [Name]";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return sreRules;

            foreach (DataRow row in table.Rows)
            {
                IdpeRule rule = new IdpeRule();
                rule.Id = (int)row["Id"].ToString().ParseInt();
                rule.Name = row["Name"].ToString();
                rule.Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null;
                rule.Xaml = row["Xaml"] != DBNull.Value ? row["Xaml"].ToString() : null;
                rule.Priority = row["Priority"] != DBNull.Value ? row["Priority"].ToString().ParseInt() : null;
                rule.RuleSetType = row["RuleSetType"] != DBNull.Value ? row["RuleSetType"].ToString().ParseInt() : null;
                rule.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                rule.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                
                sreRules.Add(rule);
            }
            return sreRules;
        }

        public IdpeRule GetRule(int ruleId)
        {
            
            string commandText = "select TOP 1 [Id],[Name],[Description],[Xaml],[Priority],[RuleSetType],[CreatedTS],[CreatedBy] from [SreRule] where [Id] = " + ruleId;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null || table.Rows.Count == 0)
                return null;

            foreach (DataRow row in table.Rows)
            {
                IdpeRule rule = new IdpeRule();
                rule.Id = (int)row["Id"].ToString().ParseInt();
                rule.Name = row["Name"].ToString();
                rule.Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null;
                rule.Xaml = row["Xaml"] != DBNull.Value ? row["Xaml"].ToString() : null;
                rule.Priority = row["Priority"] != DBNull.Value ? row["Priority"].ToString().ParseInt() : null;
                rule.RuleSetType = row["RuleSetType"] != DBNull.Value ? row["RuleSetType"].ToString().ParseInt() : null;
                rule.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                rule.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                return rule;
            }
            return null;

        }

        public IdpeRule GetRule(string ruleName)
        {

            string commandText = "select TOP 1 [Id],[Name],[Description],[Xaml],[Priority],[RuleSetType],[CreatedTS],[CreatedBy] from [SreRule] where [Name] = '" + ruleName + "'";
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null || table.Rows.Count == 0)
                return null;

            IdpeRule rule = new IdpeRule();
            DataRow row = table.Rows[0];
            rule.Id = (int)row["Id"].ToString().ParseInt();
            rule.Name = row["Name"].ToString();
            rule.Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null;
            rule.Xaml = row["Xaml"] != DBNull.Value ? row["Xaml"].ToString() : null;
            rule.Priority = row["Priority"] != DBNull.Value ? row["Priority"].ToString().ParseInt() : null;
            rule.RuleSetType = row["RuleSetType"] != DBNull.Value ? row["RuleSetType"].ToString().ParseInt() : null;
            rule.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
            rule.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;            

            return rule;

        }
      

        private List<IdpeRuleDataSource> GetSreRuleDataSources()
        {
            List<IdpeRuleDataSource> sreRuleDataSources = new List<IdpeRuleDataSource>();
            string commandText = "select [RuleDataSourceId],[RuleId],[DataSourceId],[Priority],[RuleSetType],[IsActive] from [SreRuleDataSource]";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return sreRuleDataSources;

            foreach (DataRow row in table.Rows)
            {
                IdpeRuleDataSource sreRuleDataSource = new IdpeRuleDataSource();
                sreRuleDataSource.RuleDataSourceId = (int)row["RuleDataSourceId"].ToString().ParseInt();
                sreRuleDataSource.RuleId = (int)row["RuleId"].ToString().ParseInt();
                sreRuleDataSource.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                sreRuleDataSource.Priority = (int)row["Priority"].ToString().ParseInt();
                sreRuleDataSource.RuleSetType = (int)row["RuleSetType"].ToString().ParseInt();
                sreRuleDataSource.IsActive = row["IsActive"].ToString().ParseBool();
                sreRuleDataSources.Add(sreRuleDataSource);
            }
            return sreRuleDataSources;
        }

        private IdpeRuleDataSource GetSreRuleDataSource(int dataSourceId, int ruleId, int ruleSetType)
        {
            IdpeRuleDataSource sreRuleDataSource = null;
            string commandText = string.Format("select [RuleDataSourceId],[RuleId],[DataSourceId],[Priority],[RuleSetType],[IsActive] from [SreRuleDataSource] where [DataSourceId] = {0} and [RuleId] = {1} and [RuleSetType] = {2}"
                ,dataSourceId, ruleId, ruleSetType);

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return sreRuleDataSource;

            foreach (DataRow row in table.Rows)
            {
                sreRuleDataSource = new IdpeRuleDataSource();
                sreRuleDataSource.RuleDataSourceId = (int)row["RuleDataSourceId"].ToString().ParseInt();
                sreRuleDataSource.RuleId = (int)row["RuleId"].ToString().ParseInt();
                sreRuleDataSource.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                sreRuleDataSource.Priority = (int)row["Priority"].ToString().ParseInt();
                sreRuleDataSource.RuleSetType = (int)row["RuleSetType"].ToString().ParseInt();
                sreRuleDataSource.IsActive = row["IsActive"].ToString().ParseBool();
                
            }
            return sreRuleDataSource;
        }
             


        public List<IdpeRule> GetRules(int dataSourceId)
        {
            List<IdpeRule> rules = new List<IdpeRule>();

            string qry = "select r.Id,name,description,xaml,rds.[priority],rds.rulesettype from srerule r ";
            qry += "inner join SreRuleDataSource rds on rds.ruleId = r.id ";
            qry += "where datasourceId = " + dataSourceId;
            DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand(qry);

            foreach (DataRow row in table.Rows)
            {
                IdpeRule rule = new IdpeRule();
                rule.Id = (int)row["Id"].ToString().ParseInt();
                rule.Name = row["name"].ToString();
                rule.Description = row["description"].ToString();
                rule.Xaml = row["xaml"].ToString();
                rule.Priority = row["priority"].ToString().ParseInt();
                rule.RuleSetType = row["rulesettype"].ToString().ParseInt();

                rules.Add(rule);
            }

            return rules;
        }

        public List<string> GetRuleNames(int dataSourceId)
        {
            List<IdpeRule> rules = new List<IdpeRule>();

            if (dataSourceId == 0)
            {
                rules = GetRules();
                return rules.Select(r => r.Name).ToList();
            }
            else
            {
                rules = GetRules();
                List<IdpeRuleDataSource> sreRuleDataSources = GetSreRuleDataSources();

                return (from r in rules
                        join rd in sreRuleDataSources on r.Id equals rd.RuleId
                        where rd.DataSourceId == dataSourceId
                        select r.Name).ToList();
            }
        }

        public List<IdpeRuleDataSource> GetApplicationRuleSetNames(int dataSourceId)
        {
            return GetRuleSetApplication(dataSourceId);
        }

        public List<IdpeRuleDataSource> GetRuleSetApplication(int dataSourceId)
        {
            List<IdpeRuleDataSource> sreRuleDataSources = new List<IdpeRuleDataSource>();
            string commandText = "select [RuleDataSourceId],[RuleId],[DataSourceId],[Priority],[RuleSetType],[IsActive] from [SreRuleDataSource] where DataSourceId = " + dataSourceId;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null || table.Rows.Count == 0)
                return sreRuleDataSources;

            foreach (DataRow row in table.Rows)
            {
                IdpeRuleDataSource sreRuleDataSource = new IdpeRuleDataSource();
                sreRuleDataSource.RuleDataSourceId = (int)row["RuleDataSourceId"].ToString().ParseInt();
                sreRuleDataSource.RuleId = (int)row["RuleId"].ToString().ParseInt();
                sreRuleDataSource.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                sreRuleDataSource.Priority = (int)row["Priority"].ToString().ParseInt();
                sreRuleDataSource.RuleSetType = (int)row["RuleSetType"].ToString().ParseInt();
                sreRuleDataSource.IsActive = row["IsActive"].ToString().ParseBool();
                sreRuleDataSources.Add(sreRuleDataSource);
            }
            return sreRuleDataSources;
        }

        public List<IdpeRule> GetApplicationRuleSets(int dataSourceId)
        {
            List<IdpeRule> sreRules = new List<IdpeRule>();
            string commandText = "select rs.[Id],rs.[Name],rs.[Description],rs.[Xaml],rsa.[Priority],rsa.[RuleSetType],rs.[CreatedTS],rs.[CreatedBy] from [SreRule] rs " +
                        " inner join SreRuleDataSource rsa on rs.Id = rsa.RuleId  and DataSourceId = " + dataSourceId;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null || table.Rows.Count == 0)
                return sreRules;

            foreach (DataRow row in table.Rows)
            {
                IdpeRule rule = new IdpeRule();
                rule.Id = (int)row["Id"].ToString().ParseInt();
                rule.Name = row["Name"].ToString();
                rule.Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null;
                rule.Xaml = row["Xaml"] != DBNull.Value ? row["Xaml"].ToString() : null;
                rule.Priority = row["Priority"] != DBNull.Value ? row["Priority"].ToString().ParseInt() : null;
                rule.RuleSetType = row["RuleSetType"] != DBNull.Value ? row["RuleSetType"].ToString().ParseInt() : null;
                rule.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                rule.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;                
                sreRules.Add(rule);
            }
            return sreRules;
        }

      

        public void Save(List<IdpeRule> rules)
        {
            foreach (IdpeRule rule in rules)
            {
                Save(rule);
            }
        }

        public int Save(IdpeRule rule, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            bool isInserted = false;

            string cmdText = string.Empty;
            IDbCommand command = null;
            bool localTransaction = false;
            if (dal == null)
            {
                dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
                connection = dal.CreateConnection(_ConnectionString);
                connection.Open();
                transaction = dal.CreateTransaction(connection);
                localTransaction = true;
            }

            command = dal.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            IdpeRule existingRule = GetRule(rule.Name);
            string commandText = string.Empty;
            int ruleSetId = 0;
            try
            {
                string userName = Information.LoggedInUser == null? "Debugger":Information.LoggedInUser.UserName;
                if(userName.Contains("'"))
                    userName = userName.Replace("'", "''");
                //handling single quote in rule
                if (rule.Xaml.Contains("'"))
                    rule.Xaml = rule.Xaml.Replace("'", "''");
                if (userName.Contains("'"))
                    userName = userName.Replace("'", "''");
                if (rule.Description.Contains("'"))
                    rule.Description = rule.Description.Replace("'", "''");

                if (existingRule == null)
                {
                    commandText = string.Format(CultureInfo.InvariantCulture, "INSERT SreRule (Name, Description, Xaml, CreatedTS, CreatedBy) VALUES('{0}','{1}','{2}','{3}','{4}')"
                        , rule.Name, rule.Description, rule.Xaml, DateTime.Now, userName);
                    isInserted = true;
                }
                else
                {
                    commandText = string.Format(CultureInfo.InvariantCulture, "UPDATE SreRule  SET Name = '{0}', Description = '{1}', Xaml = '{2}' where id = {3}"
                       , rule.Name, rule.Description, rule.Xaml, existingRule.Id);
                }              
                command.CommandText = commandText;                
                command.ExecuteNonQuery();

                if (localTransaction)
                    transaction.Commit();

                if (isInserted)
                {
                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.CommandText = "SELECT max(Id) from SreRule";
                    ruleSetId = (Int32)command.ExecuteScalar();
                }
                else
                {
                    ruleSetId = existingRule.Id;
                }
                
            }
            catch (Exception ex)
            {
                if (localTransaction)
                    transaction.Rollback();
                Trace.TraceError(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (localTransaction)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    transaction.Dispose();
                }
            }

            return ruleSetId;

        }

        public void SaveRuleAssociationDuringCopy(IdpeRuleDataSource ruleDataSource)
        {
            IDal dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection con = dal.CreateConnection(_ConnectionString);
            con.Open();

            string query = string.Format("INSERT INTO [SreRuleDataSource] ([RuleId],[DataSourceId],[Priority],[RulesetType],[IsActive]) VALUES ({0},{1},{2},{3},{4})",
                       ruleDataSource.RuleId,
                       ruleDataSource.DataSourceId, ruleDataSource.Priority, ruleDataSource.RuleSetType, ruleDataSource.IsActive == true ? 1 : 0);

            using (IDbCommand command = dal.CreateCommand(query, con))
            {
                command.ExecuteNonQuery();
            }

        }

        public int Save(IdpeRuleDataSource ruleDataSource, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            int ruleDataSourceId = 0;
            bool localTransaction = false;
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

                bool inserted = false;
                string query = string.Empty;
                IdpeRuleDataSource existingObject = GetSreRuleDataSource(ruleDataSource.DataSourceId, ruleDataSource.RuleId, ruleDataSource.RuleSetType);

                if (existingObject == null)
                {
                    query = string.Format("INSERT INTO [SreRuleDataSource] ([RuleId],[DataSourceId],[Priority],[RulesetType],[IsActive]) VALUES ({0},{1},{2},{3},{4})",
                        ruleDataSource.RuleId,
                        ruleDataSource.DataSourceId, ruleDataSource.Priority, ruleDataSource.RuleSetType, ruleDataSource.IsActive == true ? 1 : 0);
                    inserted = true;
                }
                else
                {
                    ruleDataSourceId = existingObject.RuleDataSourceId;
                    query = string.Format("update [SreRuleDataSource] set [Priority] = {0},[RulesetType] = {1},[IsActive] ={2} where RuleDataSourceId = {3}",
                        ruleDataSource.Priority, ruleDataSource.RuleSetType, ruleDataSource.IsActive == true ? 1 : 0, ruleDataSourceId);

                }

                using (IDbCommand command = dal.CreateCommand(query, connection, transaction))
                {
                    command.ExecuteNonQuery();
                }

                if (localTransaction)
                    transaction.Commit();

                if (inserted)
                {
                    query = string.Format("select RuleDataSourceId from SreRuleDataSource where [RuleId] = {0} and [DataSourceId] = {1} and [Priority] = {2} and [RulesetType] = {3}",
                        ruleDataSource.RuleId, ruleDataSource.DataSourceId, ruleDataSource.Priority, (int)ruleDataSource.RuleSetType);
                    
                    using (IDbCommand command = localTransaction ? dal.CreateCommand(query, connection) : dal.CreateCommand(query, connection, transaction))
                    {

                        IDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            ruleDataSourceId = int.Parse(reader[0].ToString());
                        }
                        reader.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                if (localTransaction)
                    transaction.Rollback();

                Trace.TraceError("Error while saving SreRuleDataSource " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                throw new Exception(ex.Message, ex);

            }
            finally
            {
                if (localTransaction)
                {
                    if (connection != null)
                    {
                        if (connection.State != System.Data.ConnectionState.Closed) connection.Close();
                        connection.Dispose();
                    }
                    if (transaction != null)
                        transaction.Dispose();
                }
            }
            return ruleDataSourceId;
        }

        public void DeleteRule(string ruleName)
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
                command.CommandText = "DELETE FROM [SreRule] WHERE [Name] = @Name";
                command.AddParameterWithValue("Name", ruleName);
                command.ExecuteNonQuery();
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

        public void RenameRule(int id, string ruleName)
        {
            if ((id == 0)
                || (string.IsNullOrEmpty(ruleName)))
                return;

            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbTransaction transaction = myDal.CreateTransaction(conn);
            IDbCommand command = myDal.CreateCommand();
            command.Connection = conn;
            command.Transaction = transaction;

            try
            {
                command.CommandText = "update [SreRule] set [Name] = @Name WHERE [Id] = @Id";
                command.AddParameterWithValue("Name", ruleName);
                command.AddParameterWithValue("Id", id);
                command.ExecuteNonQuery();
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

        public void DeleteRuleFromDataSource(int dataSourceId, int ruleId, RuleSetTypes ruleSetType, bool deleteRule = false)
        {
            string sqlStatement = "delete from [SreRuleDataSource] ";
            sqlStatement += string.Format("where (DataSourceId = {0}) and (RuleId = {1}) and (RuleSetType = {2})", dataSourceId, ruleId, (int)ruleSetType);
            CoreDatabaseObjects.Instance.ExecuteStatement(sqlStatement);

            if (deleteRule)
            {
                sqlStatement = "delete from [SreRule] ";
                sqlStatement += string.Format("where Id = {0}", ruleId);
                CoreDatabaseObjects.Instance.ExecuteStatement(sqlStatement);
            }
        }

        public void CopyRule(int fromRuleId, string newRuleName)
        {
            IdpeRule rule = GetRule(fromRuleId);
            rule.Id = 0;
            rule.Name = newRuleName;
            Save(rule); 
        }

        public DataTable GetRuleDependencies(int ruleId)
        {
            string commandText = "select ds.Id, ds.Name, rds.RuleSetType from SreDataSource ds ";
            commandText += "inner join SreRuleDataSource rds on ds.Id = rds.DataSourceId ";
            commandText += "where rds.RuleId = " + ruleId;

            return CoreDatabaseObjects.Instance.ExecuteCommand(commandText);

        }
    }
}


