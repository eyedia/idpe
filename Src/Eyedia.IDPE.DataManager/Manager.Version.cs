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
using Eyedia.IDPE.Common;
using Eyedia.Core.Data;
using System.Diagnostics;
using System.Data.Linq;
using System.IO;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {                
        public int Save(SreVersion version, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            ValidateVersion(version);
            int versionId = 0;
            bool localTransaction = false;
            if (dal == null)
            {
                dal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
                connection = dal.CreateConnection(_ConnectionString);
                connection.Open();
                transaction = dal.CreateTransaction(connection);
                localTransaction = true;
            }
            IDbCommand command = dal.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandText = "INSERT INTO [SreVersion]([Version], [Type], [ReferenceId], [Data], [CreatedBy], [CreatedTS], [Source]) ";
                command.CommandText += " VALUES (@Version, @Type, @ReferenceId, @Data, @CreatedBy, @CreatedTS, @Source)";
                command.AddParameterWithValue("Version", version.Version);
                command.AddParameterWithValue("Type", version.Type);
                command.AddParameterWithValue("ReferenceId", version.ReferenceId);
                command.AddParameterWithValue("Data", version.Data.ToArray());
                command.AddParameterWithValue("CreatedBy", Information.LoggedInUser != null ? Information.LoggedInUser.UserName : "Debugger");
                command.AddParameterWithValue("CreatedTS", version.CreatedTS);
                command.AddParameterWithValue("Source", GetSource(version.Source));

                command.ExecuteNonQuery();

                //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                command.Parameters.Clear();
                command.CommandText = "SELECT max(Id) from [SreVersion]";
                versionId = (Int32)command.ExecuteScalar();

                if (localTransaction)
                    transaction.Commit();

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
                    if (connection.State == ConnectionState.Open) connection.Close();
                    connection.Dispose();
                    transaction.Dispose();
                }
            }

            return versionId;
        }

        public SreVersion GetVersion(VersionObjectTypes versionType, int referenceId, int versionNumer)
        {         
            string commandText = string.Format("select [Id],[Version],[Type],[ReferenceId],[Data], [CreatedBy], [CreatedTS] from [SreVersion] where [Type] = {0} and [ReferenceId] = {1} and [Version] = {2} order by createdts desc",
                (int)versionType, referenceId, versionNumer);

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if(table.Rows.Count == 1)
                return DataRowToSreVersion(table.Rows[0]);

            return null;
        }

        public List<SreVersion> GetVersions(VersionObjectTypes versionObjectType, int referenceId)
        {
            List<SreVersion> versions = new List<SreVersion>();
            string commandText = string.Format("select [Id],[Version],[Type],[ReferenceId],[Data], [CreatedBy], [CreatedTS], [Source] from [SreVersion] where [Type] = {0} and [ReferenceId] = {1} order by createdts desc",
                (int)versionObjectType, referenceId);

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            for(int i=0;i<table.Rows.Count;i++)
            {                
                versions.Add(DataRowToSreVersion(table.Rows[i]));
            }

            return versions;
        }

        public int GetLatestVersionNumber(VersionObjectTypes versionObjectType, int referenceId)
        {
            string commandText = string.Format("select Max([Version]) from [SreVersion] where [Type] = {0} and [ReferenceId] = {1}",
                (int)versionObjectType, referenceId);

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            return (table.Rows.Count == 1) ? (int)table.Rows[0][0].ToString().ParseInt() : 0;
        }

        #region Helpers

        private void ValidateVersion(SreVersion version)
        {
            string allErrors = string.Empty;

            if (version.Type == (int)VersionObjectTypes.Unknown)
                allErrors += "Version type cannot be unknown!" + Environment.NewLine;

            if (version.ReferenceId == 0)
            {
                allErrors += "Version must have a reference Id!" + Environment.NewLine;
            }
            else
            {
                switch ((VersionObjectTypes)version.Type)
                {
                    case VersionObjectTypes.Attribute:
                        SreAttribute attribute = GetAttribute(version.ReferenceId);
                        if (attribute.AttributeId == 0)
                            allErrors += "The INSERT statement conflicted with the FOREIGN KEY constraint. The conflict occurred in database, table SreAttribute, column 'Id'." + Environment.NewLine;
                        break;

                    case VersionObjectTypes.DataSource:
                        string dataSourceName = GetApplicationName(version.ReferenceId);
                        if (string.IsNullOrEmpty(dataSourceName))
                            allErrors += "The INSERT statement conflicted with the FOREIGN KEY constraint. The conflict occurred in database, table SreDataSource, column 'Id'." + Environment.NewLine;
                        break;

                    case VersionObjectTypes.Rule:
                        SreRule rule = GetRule(version.ReferenceId);
                        if (rule.Id == 0)
                            allErrors += "The INSERT statement conflicted with the FOREIGN KEY constraint. The conflict occurred in database, table SreRule, column 'Id'." + Environment.NewLine;
                        break;

                }
            }

            if (!string.IsNullOrEmpty(allErrors))
                throw new Exception(allErrors);
        }

        private SreVersion DataRowToSreVersion(DataRow row)
        {
            SreVersion version = new SreVersion();
            version.Id = (int)row["Id"].ToString().ParseInt();
            version.Version = (int)row["Version"].ToString().ParseInt();
            version.Type = (int)row["Type"].ToString().ParseInt();
            version.ReferenceId = (int)row["ReferenceId"].ToString().ParseInt();
            version.Data = new Binary((byte[])row["Data"]);
            version.CreatedBy = row["CreatedBy"].ToString();
            version.CreatedTS = (DateTime)row["CreatedTS"].ToString().ParseDateTime();
            version.Source = row["Source"].ToString();
            return version;
        }

        private string GetVersionTempFileName(int referenceId)
        {
            return Path.Combine(Information.TempDirectorySre, referenceId + ".version.xml");
        }        

        #endregion Helpers

    }
}


