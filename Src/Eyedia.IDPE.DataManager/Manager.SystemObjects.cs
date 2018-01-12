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
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.Core.Data;
using System.Data;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {
        public void InsertSystemObjects()
        {
            string sdsSqlStatement = "INSERT INTO [SreDataSource]([Id],[Name],[Description],[IsSystem],[IsActive],[CreatedTS],[CreatedBy]) VALUES(-99,'Global Datasource','Global datasource to keep global keys, connectionstrings','TRUE',1,'TRUE', getdate(),'System')";
            string dsSqlStatement = "INSERT INTO [SreDataSource] ([Id],[Name],[Description],[IsSystem],[DataFeederType],[DataFormatType],[Delimiter],[SystemDataSourceId],[OutputType],[IsActive],[CreatedTS],[CreatedBy]) VALUES(100,'System Data Source','System data source to handle deployment restart etc','FALSE',2,99,',',-99,1,'TRUE',getdate(),'System')";

            IDal dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection connection = dal.CreateConnection(ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].ToString());
            connection.Open();
            IDbTransaction transaction = dal.CreateTransaction(connection);
            string sqlStatement = string.Empty;
            try
            {
                SreDataSource sds = GetDataSourceDetails(-99);
                if (sds == null)
                {
                    CoreDatabaseObjects.Instance.ExecuteStatement(sdsSqlStatement, dal, connection, transaction);
                }
                SreDataSource ds = GetDataSourceDetails(100);
                if (ds == null)
                {
                    CoreDatabaseObjects.Instance.ExecuteStatement(dsSqlStatement, dal, connection, transaction);
                }

                sdInsertAttribute(string.Empty, 1, dal, connection, transaction);
                sdInsertAttribute("DeploymentResult", 2, dal, connection, transaction);
                sdInsertAttribute("DeploymentTrace", 3, dal, connection, transaction);
                sdInsertRule(dal, connection, transaction);
                dsInsertKeys(dal, connection, transaction);
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Trace.TraceInformation(ex.ToString());
                Trace.Flush();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
                connection.Dispose();
                transaction.Dispose();
            }
        }

        private void sdInsertAttribute(string name, int position, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            string strType = "STRING";
            if (string.IsNullOrEmpty(name))
            {
                name = "IsValid";
                strType = "BIT";
            }

            SreAttribute attrb = GetAttribute(name);
            if (attrb == null)
            {
                attrb = new SreAttribute();
                attrb.Name = name;
                attrb.Type = strType;
                attrb.IsAcceptable = true;
                attrb.AttributeId = Save(attrb, dal, connection, transaction);
            }

            SreAttributeDataSource sads = new SreAttributeDataSource();
            sads.AttributeId = attrb.AttributeId;
            sads.DataSourceId = -99;
            sads.Position = position;
            sads.IsAcceptable = (name != "IsValid") ? true : false;
            Save(sads, dal, connection, transaction);

            if (name != "IsValid")
            {
                sads = new SreAttributeDataSource();
                sads.AttributeId = attrb.AttributeId;
                sads.DataSourceId = 100;
                sads.Position = position;
                sads.IsAcceptable = true;
                Save(sads, dal, connection, transaction);
            }
        }

        private void sdInsertRule(IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            SreRule rule = new SreRule();
            rule.Name = "System Data Source - Map";
            rule.Description = "System data source rule mapping rule used by system. This rule cannot be deleted.";
            
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.DataManager." + rule.Name + ".xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                rule.Xaml = reader.ReadToEnd();
            }

            rule.Id = Save(rule);

            SreRuleDataSource sreRuleDataSource = new SreRuleDataSource();
            sreRuleDataSource.DataSourceId = 100;
            sreRuleDataSource.RuleId = rule.Id;
            sreRuleDataSource.Priority = 1;
            sreRuleDataSource.RuleSetType = 3;
            Save(sreRuleDataSource, dal, connection,transaction);
        }

        private void dsInsertKeys(IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            /*
            FileInterfaceName Eyedia.IDPE.Services.PacketReceiver, Eyedia.IDPE.Services
            OutputPartialRecordsAllowed False
            OutputIsFirstRowHeader  False
            OutputDelimiter,
            OutputFileExtension	.csv	*/

            SreKey key = new SreKey();
            key.Name = SreKeyTypes.FileInterfaceName.ToString();
            key.Type = (int)SreKeyTypes.FileInterfaceName;
            key.Value = "Eyedia.IDPE.Services.PacketReceiver, Eyedia.IDPE.Services";
            Save(key, 100, dal, connection, transaction);

            key = new SreKey();
            key.Name = SreKeyTypes.OutputPartialRecordsAllowed.ToString();
            key.Type = (int)SreKeyTypes.OutputPartialRecordsAllowed;
            key.Value = "False";
            Save(key, 100, dal, connection, transaction);

            key = new SreKey();
            key.Name = SreKeyTypes.OutputIsFirstRowHeader.ToString();
            key.Type = (int)SreKeyTypes.OutputIsFirstRowHeader;
            key.Value = "True";
            Save(key, 100, dal, connection, transaction);

            key = new SreKey();
            key.Name = SreKeyTypes.OutputDelimiter.ToString();
            key.Type = (int)SreKeyTypes.OutputDelimiter;
            key.Value = ",";
            Save(key, 100, dal, connection, transaction);

            key = new SreKey();
            key.Name = SreKeyTypes.OutputFileExtension.ToString();
            key.Type = (int)SreKeyTypes.OutputFileExtension;
            key.Value = ".csv";
            Save(key, 100, dal, connection, transaction);
        }
    }
}


