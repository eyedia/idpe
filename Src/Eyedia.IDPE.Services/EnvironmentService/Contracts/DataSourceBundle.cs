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
using Eyedia.IDPE.DataManager;
using System.IO;
using Eyedia.Core;
using System.Diagnostics;
using Eyedia.Core.Data;
using System.Configuration;
using Eyedia.IDPE.Common;
using System.Data;
using System.Globalization;
using System.Runtime.Serialization;

namespace Eyedia.IDPE.Services
{
    [DataContract]
    /// <summary>
    /// Used for import export data sources
    /// </summary>

    public class DataSourceBundle
    {
        public DataSourceBundle() 
        {
            Init();
        }

        public DataSourceBundle(string fileName = null, string serializedObject = null, int version = 0)
        {
            if ((fileName == null) && (serializedObject == null))
            {
                Init();
            }
            else if (fileName != null)
            {
                LoadFromFile(fileName);
                IsImporting = true;
            }
            else if (serializedObject != null)
            {
                LoadFromSerializedObject(serializedObject);
                IsImporting = true;
            }

            Version = version;
        }

        public DataSourceBundle(bool bundledWithDeploymentPacket)
        {
            IsImporting = true;            
        }

        void Init()
        {
            this.DataSource = new IdpeDataSource();
            this.Attributes = new List<IdpeAttribute>();
            this.Keys = new List<IdpeKey>();           
            this.Rules = new List<IdpeRule>();
            this.RuleNames = new List<string>();
        }

        [DataMember]
        public int Version { get; private set; }

        [DataMember]
        public bool IsImporting { get; set; }

        [DataMember]
        public string SystemDataSourceName { get; set; }

        [DataMember]
        public IdpeDataSource DataSource { get; set; }

        [DataMember]
        public List<IdpeAttribute> Attributes { get; set; }

        [DataMember]
        public List<IdpeRule> Rules { get; set; }

        [DataMember]
        public List<string> RuleNames { get; set; }

        [DataMember]
        public List<IdpeKey> Keys { get; set; }        

        #region Export
        public void Export(int dataSourceId, string fileName, bool includeRules = true)
        {            
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(Export(dataSourceId, includeRules));
                sw.Close();
            }
        }

        public string Export(int dataSourceId, bool includeRules = true)
        {
            ExportDataSource(dataSourceId);
            if (includeRules)
                ExportRules(dataSourceId);
            else
                ExportRuleNames(dataSourceId);

            ExportKeys(dataSourceId);
            ExportAttributes(dataSourceId);
            return this.Serialize();           
        }

        private void ExportDataSource(int dataSourceId)
        {
            Manager mgr = new Manager();
            this.DataSource = mgr.GetDataSourceDetails(dataSourceId);
            this.SystemDataSourceName = mgr.GetApplicationName((int)this.DataSource.SystemDataSourceId);
        }

        void ExportRules(int dataSourceId)
        {
            string sqlStatement = "select r.Name, r.Description, r.Xaml, rds.Priority, rds.RuleSetType ";
            sqlStatement += "from IdpeRule r ";
            sqlStatement += "inner join IdpeRuleDataSource rds on r.Id = rds.RuleId ";
            sqlStatement += "where rds.DataSourceId = " + dataSourceId;
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(sqlStatement);

            foreach (DataRow row in table.Rows)
            {
                IdpeRule rule = new IdpeRule();                
                rule.Name = row["Name"].ToString();
                rule.Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null;
                rule.Xaml = row["Xaml"] != DBNull.Value ? row["Xaml"].ToString() : null;
                rule.Priority = row["Priority"] != DBNull.Value ? row["Priority"].ToString().ParseInt() : null;
                rule.RuleSetType = row["RuleSetType"] != DBNull.Value ? row["RuleSetType"].ToString().ParseInt() : null;                
                Rules.Add(rule);
            }            
        }

        void ExportRuleNames(int dataSourceId)
        {
            string sqlStatement = "select r.Name, rds.Priority, rds.RuleSetType ";
            sqlStatement += "from IdpeRule r ";
            sqlStatement += "inner join IdpeRuleDataSource rds on r.Id = rds.RuleId ";
            sqlStatement += "where rds.DataSourceId = " + dataSourceId;
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(sqlStatement);

            List<IdpeRule> rules = new List<IdpeRule>();
            foreach (DataRow row in table.Rows)
            {
                IdpeRule rule = new IdpeRule();
                rule.Name = row["Name"].ToString();
                rule.Priority = row["Priority"] != DBNull.Value ? row["Priority"].ToString().ParseInt() : null;
                rule.RuleSetType = row["RuleSetType"] != DBNull.Value ? row["RuleSetType"].ToString().ParseInt() : null;
                rules.Add(rule);
            }
            //following logic implemented to have rule names sequentially
            ExportRuleNames(rules, RuleSetTypes.PreValidate);
            ExportRuleNames(rules, RuleSetTypes.RowPreparing);
            ExportRuleNames(rules, RuleSetTypes.RowPrepared);
            ExportRuleNames(rules, RuleSetTypes.RowValidate);
            ExportRuleNames(rules, RuleSetTypes.PostValidate);
        }

        void ExportRuleNames(List<IdpeRule> allRules, RuleSetTypes ruleSetType)
        {
            List<IdpeRule> rules = allRules.Where(r => r.RuleSetType == (int)ruleSetType).ToList();
            foreach (IdpeRule rule in rules)
            {
                RuleNames.Add(string.Format("{0}({1}):{2}", ruleSetType.ToString().PadRight(12), rule.Priority, rule.Name));
            }
        }

        void ExportKeys(int dataSourceId)
        {
            string sqlStatement = "select k.Name, k.Value, k.Type, kds.IsDeployable, k.NextKeyId ";
            sqlStatement += "from  IdpeKey k ";
            sqlStatement += "inner join IdpeKeyDataSource kds on k.KeyId = kds.KeyId ";
            sqlStatement += "where kds.DataSourceId = " + dataSourceId;
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(sqlStatement);

            foreach (DataRow row in table.Rows)
            {
                IdpeKey key = new IdpeKey();                
                key.Name = row["Name"].ToString();
                key.Value = row["Value"] != DBNull.Value ? row["Value"].ToString() : null;
                key.Type = (int)(row["Type"] != DBNull.Value ? row["Type"].ToString().ParseInt() : 0);
                key.IsDeployable = row["IsDeployable"] != DBNull.Value ? row["IsDeployable"].ToString().ParseBool() : false;
                key.NextKeyId = row["NextKeyId"] != DBNull.Value ? row["NextKeyId"].ToString().ParseInt() : 0;
                Keys.Add(key);
            }
        }


        void ExportAttributes(int dataSourceId)
        {
            string sqlStatement = "select a.Name, a.Type, a.Minimum, a.Maximum, a.Formula, ads.IsAcceptable,ads.[AttributePrintValueType],ads.[AttributePrintValueCustom] ";
            sqlStatement += "from IdpeAttribute a ";
            sqlStatement += "inner join IdpeAttributeDataSource ads on a.AttributeId = ads.AttributeId ";
            sqlStatement += "where ads.DataSourceId = " + dataSourceId;
            sqlStatement += "order by ads.position ";
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(sqlStatement);

            foreach (DataRow row in table.Rows)
            {
                IdpeAttribute attrb = new IdpeAttribute();                
                attrb.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                attrb.Type = row["Type"] != DBNull.Value ? row["Type"].ToString() : null;
                attrb.Minimum = row["Minimum"] != DBNull.Value ? row["Minimum"].ToString() : null;
                attrb.Maximum = row["Maximum"] != DBNull.Value ? row["Maximum"].ToString() : null;
                attrb.Formula = row["Formula"] != DBNull.Value ? row["Formula"].ToString() : null;
                attrb.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
                attrb.AttributePrintValueType = row["AttributePrintValueType"] != DBNull.Value ? row["AttributePrintValueType"].ToString().ParseInt() : 0;
                attrb.AttributePrintValueCustom = row["AttributePrintValueCustom"] != DBNull.Value ? row["AttributePrintValueCustom"].ToString() : string.Empty;
                Attributes.Add(attrb);
            }
        }

        #endregion Export

        #region Import
        
        public void Import(string remoteIpAddress = null, string remoteMachineName = null)
        {
            if (string.IsNullOrEmpty(remoteIpAddress))
                remoteIpAddress = Eyedia.Core.Net.Dns.GetLocalIpAddress();

            if (string.IsNullOrEmpty(remoteMachineName))
                remoteMachineName = Environment.MachineName;

            this.DataSource.Source = remoteIpAddress;
            foreach(IdpeAttribute attrib in this.Attributes)
            {
                attrib.Source = remoteIpAddress;
            }

            foreach (IdpeRule rule in this.Rules)
            {
                rule.Source = remoteIpAddress;
            }

            foreach (IdpeKey key in this.Keys)
            {
                key.Source = remoteIpAddress;
            }

            //if (!IsImporting)
            //    throw new BusinessException("You can not call import function with this instance. Try creating instance with a different constructor.");

            //if (string.IsNullOrEmpty(this.DataSource.Name))
            //    throw new BusinessException("The file was invalid! Can not continue, operation aborted!");

            //if (((this.DataSource.IsSystem == false))
            //   && (string.IsNullOrEmpty(this.SystemDataSourceName)))
            //    throw new BusinessException("The file was invalid! Can not continue, operation aborted!");

            Manager manager = new Manager();
            
            //if ((this.DataSource.IsSystem == false)
            //    && (!manager.ApplicationExists(SystemDataSourceName)))
            //    throw new BusinessException(string.Format("System data source '{0}' does not exist! Please import system data source first and then try to import this one.", SystemDataSourceName));

            ImportInternal(manager);
        }

        void ImportInternal(Manager manager)
        {            
            IDal dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection connection = dal.CreateConnection(ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].ToString());
            connection.Open();
            IDbTransaction transaction = dal.CreateTransaction(connection);
            string sqlStatement = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(this.DataSource.Name))
                {
                    new VersionManager().KeepVersion(VersionObjectTypes.DataSource, this.DataSource.Id);
                    bool isInserted = false;
                    manager.Save(this.DataSource, ref isInserted, dal, connection, transaction);
                    ImportAttributes(dal, connection, transaction, manager);
                }
                ImportKeys(dal, connection, transaction, manager);
                ImportRules(dal, connection, transaction, manager);
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                connection.Dispose();
                transaction.Dispose();
            }            
        }

        void ImportAttributes(IDal dal, IDbConnection connection, IDbTransaction transaction, Manager manager)
        {
            //delete all existing association
            string sqlStatement = "delete from [IdpeAttributeDataSource] where [DataSourceId] = " + this.DataSource.Id;
            CoreDatabaseObjects.Instance.ExecuteStatement(sqlStatement, dal, connection, transaction);
            
            int position = 1;
            foreach (IdpeAttribute attribute in this.Attributes)
            {
                int attributeId = manager.AttributeExists(attribute.Name);
                if (attributeId == 0)
                    attributeId = manager.Save(attribute, dal, connection, transaction);
         
                sqlStatement = "insert into [IdpeAttributeDataSource]([DataSourceId],[AttributeId],[Position],[IsAcceptable],[AttributePrintValueType],[AttributePrintValueCustom],[CreatedTS],[CreatedBy],[Source]) ";
                sqlStatement += "values ({0},{1},{2},{3},{4},'{5}','{6}','{7}' ,'Imported')";

                sqlStatement = string.Format(sqlStatement,
                    this.DataSource.Id, attributeId, position, attribute.IsAcceptable == true ? 1 : 0, attribute.AttributePrintValueType, 
                    attribute.AttributePrintValueCustom, DateTime.Now, 
                    Information.LoggedInUser.UserName.Replace("'",""));
                CoreDatabaseObjects.Instance.ExecuteStatement(sqlStatement, dal, connection, transaction);
                position++;
            }
        }

        void ImportKeys(IDal dal, IDbConnection connection, IDbTransaction transaction, Manager manager)
        {
            foreach (IdpeKey key in this.Keys)
            {
                int dataSourceId = DataSource.Id;
                if ((key.DataSourceId != null) && (key.DataSourceId != 0))
                    dataSourceId = (int)key.DataSourceId;
                manager.Save(key, dataSourceId, dal, connection, transaction);
            }
        }

        void ImportRules(IDal dal, IDbConnection connection, IDbTransaction transaction, Manager manager)
        {
            string sqlStatement = string.Empty;
            foreach (IdpeRule rule in this.Rules)
            {
                if (!rule.Name.Equals("DuplicateCheck", StringComparison.OrdinalIgnoreCase))
                {
                    string userName = Information.LoggedInUser.UserName.Contains("'") ? Information.LoggedInUser.UserName.Replace("'", "''") : Information.LoggedInUser.UserName;                
                    rule.Id = manager.Save(rule, dal, connection, transaction);
                }

                if (this.DataSource.Id > 0)
                {
                    IdpeRuleDataSource sra = new IdpeRuleDataSource();
                    sra.DataSourceId = this.DataSource.Id;
                    sra.RuleId = rule.Id;
                    sra.Priority = (int)rule.Priority;
                    sra.RuleSetType = (int)rule.RuleSetType;
                    sra.IsActive = true;
                    manager.Save(sra, dal, connection, transaction);
                }
            }
        }

        void LoadFromFile(string fileName)
        {
            string fileContent = string.Empty;
            using (StreamReader sr = new StreamReader(fileName))
            {
                fileContent = sr.ReadToEnd();
                sr.Close();
            }
            LoadFromSerializedObject(fileContent);
        }

        void LoadFromSerializedObject(string serializedObject)
        {
            DataSourceBundle dsb = serializedObject.Deserialize<DataSourceBundle>();
            this.DataSource = dsb.DataSource;
            this.SystemDataSourceName = dsb.SystemDataSourceName;
            this.Attributes = dsb.Attributes;
            this.Rules = dsb.Rules;
            this.RuleNames = dsb.RuleNames;
            this.Keys = dsb.Keys;
        }

        #endregion Import
    }

    
}



