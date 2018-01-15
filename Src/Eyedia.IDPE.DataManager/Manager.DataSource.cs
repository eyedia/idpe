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
using Eyedia.IDPE.Common;
using System.IO;
using System.Reflection;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {
        public IdpeDataSource GetDataSourceDetails(int dataSourceId)
        {
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand();
            command.CommandText = "SELECT * FROM IdpeDataSource WHERE [Id] = @Id";
            command.AddParameterWithValue("Id", dataSourceId);
            command.Connection = conn;

            IDataReader reader = command.ExecuteReader();
            IdpeDataSource dataSource = null;
            if (reader.Read())
            {
                dataSource = new IdpeDataSource();
                dataSource.Id = int.Parse(reader["Id"].ToString());
                dataSource.Name = reader["Name"].ToString();
                dataSource.Description = reader["Description"].ToString();
                dataSource.IsSystem = reader["IsSystem"] != DBNull.Value ? bool.Parse(reader["IsSystem"].ToString()) : false;
                dataSource.DataFeederType = reader["DataFeederType"].ToString().ParseInt();
                dataSource.DataFormatType = reader["DataFormatType"].ToString().ParseInt();
                dataSource.Delimiter = reader["Delimiter"].ToString();
                dataSource.SystemDataSourceId = reader["SystemDataSourceId"].ToString() == string.Empty ? 0 : int.Parse(reader["SystemDataSourceId"].ToString());
                dataSource.DataContainerValidatorType = reader["DataContainerValidatorType"].ToString();
                dataSource.OutputType = reader["OutputType"].ToString() == string.Empty ? 0 : int.Parse(reader["OutputType"].ToString());
                dataSource.OutputWriterTypeFullName = reader["OutputWriterTypeFullName"].ToString();
                dataSource.PlugInsType = reader["PlugInsType"].ToString();
                dataSource.ProcessingBy = reader["ProcessingBy"].ToString();
                dataSource.PusherType = reader["PusherType"].ToString() == string.Empty ? 0 : int.Parse(reader["PusherType"].ToString());
                dataSource.PusherTypeFullName = reader["PusherTypeFullName"].ToString();
                dataSource.IsActive = reader["IsActive"].ToString() == string.Empty ? false : bool.Parse(reader["IsActive"].ToString());
                dataSource.CreatedTS = (DateTime)(reader["CreatedTS"] != DBNull.Value ? reader["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                dataSource.CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null;
            }
            reader.Close();
            reader.Dispose();
            command.Dispose();
            conn.Close();
            conn.Dispose();
            return dataSource;
        }

        public int GetDataSourceParentId(int dataSourceId)
        {
            string commandText = "select SystemDataSourceId from [IdpeDataSource] where [Id] = " + dataSourceId;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null || table.Rows.Count == 0)
                return 0;

            return (int)table.Rows[0][0].ToString().ParseInt();
        }

        public IdpeDataSource GetApplicationDetails(string applicationName)
        {
            IdpeDataSource dataSource = new IdpeDataSource();
            string commandText = "select top 1 [Id],[Name],[Description],[IsSystem],[DataFeederType],[DataFormatType],[Delimiter],[SystemDataSourceId],[DataContainerValidatorType], ";
            commandText += " [OutputType], [OutputWriterTypeFullName],[PlugInsType],[ProcessingBy],[PusherType],[PusherTypeFullName],[IsActive],[CreatedTS],[CreatedBy]";
            commandText += " from [IdpeDataSource] where Name = '" + applicationName + "'";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null || table.Rows.Count == 0)
                return dataSource;

            foreach (DataRow row in table.Rows)
            {
                dataSource.Id = (int)row["Id"].ToString().ParseInt();
                dataSource.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                dataSource.Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null;
                dataSource.IsSystem = row["IsSystem"] != DBNull.Value ? row["IsSystem"].ToString().ParseBool() : false;
                dataSource.DataFeederType = row["DataFeederType"] != DBNull.Value ? row["DataFeederType"].ToString().ParseInt() : null;
                dataSource.DataFormatType = row["DataFormatType"] != DBNull.Value ? row["DataFormatType"].ToString().ParseInt() : null;
                dataSource.Delimiter = row["Delimiter"] != DBNull.Value ? row["Delimiter"].ToString() : null;
                dataSource.SystemDataSourceId = row["SystemDataSourceId"] != DBNull.Value ? row["SystemDataSourceId"].ToString().ParseInt() : null;
                dataSource.DataContainerValidatorType = row["DataContainerValidatorType"] != DBNull.Value ? row["DataContainerValidatorType"].ToString() : null;
                dataSource.OutputType = row["OutputType"].ToString() == string.Empty ? 0 : int.Parse(row["OutputType"].ToString());
                dataSource.OutputWriterTypeFullName = row["OutputWriterTypeFullName"] != DBNull.Value ? row["OutputWriterTypeFullName"].ToString() : null;
                dataSource.PlugInsType = row["PlugInsType"] != DBNull.Value ? row["PlugInsType"].ToString() : null;
                dataSource.ProcessingBy = row["ProcessingBy"] != DBNull.Value ? row["ProcessingBy"].ToString() : null;
                dataSource.PusherType = row["PusherType"] != DBNull.Value ? 0 : row["PusherType"].ToString().ParseInt();
                dataSource.PusherTypeFullName = row["PusherTypeFullName"] != DBNull.Value ? row["PusherTypeFullName"].ToString() : null;
                dataSource.IsActive = row["IsActive"].ToString() == string.Empty ? false : bool.Parse(row["IsActive"].ToString());
                dataSource.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                dataSource.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
            }
            return dataSource;
        }

        /// <summary>
        /// Retrieves all data sources
        /// </summary>
        /// <param name="type">0 = all, 1 = non system only, 2 = system only</param>
        /// <returns></returns>
        public List<IdpeDataSource> GetDataSources(int type = 0)
        {

            List<IdpeDataSource> idpeDataSources = new List<IdpeDataSource>();

            string commandText = string.Empty;
            if (type == 1)
            {
                commandText = "select [Id],[Name],[Description],[IsSystem],[DataFeederType],[DataFormatType],[Delimiter],[SystemDataSourceId],[DataContainerValidatorType], ";
                commandText += "[OutputType],[OutputWriterTypeFullName],[PlugInsType],[ProcessingBy],[PusherType],[PusherTypeFullName],[IsActive],[CreatedTS],[CreatedBy] ";
                commandText += "from [IdpeDataSource] where [IsSystem] = 0";
            }
            else if (type == 2)
            {
                commandText = "select [Id],[Name],[Description],[IsSystem],[DataFeederType],[DataFormatType],[Delimiter],[SystemDataSourceId],[DataContainerValidatorType], ";
                commandText += "[OutputType],[OutputWriterTypeFullName],[PlugInsType],[ProcessingBy],[PusherType],[PusherTypeFullName],[IsActive],[CreatedTS],[CreatedBy] ";
                commandText += "from [IdpeDataSource] where [IsSystem] = 1";
            }
            else
            {
                commandText = "select [Id],[Name],[Description],[IsSystem],[DataFeederType],[DataFormatType],[Delimiter],[SystemDataSourceId],[DataContainerValidatorType], ";
                commandText += "[OutputType],[OutputWriterTypeFullName],[PlugInsType],[ProcessingBy],[PusherType],[PusherTypeFullName],[IsActive],[CreatedTS],[CreatedBy] ";
                commandText += "from [IdpeDataSource]";
            }

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return idpeDataSources;

            foreach (DataRow row in table.Rows)
            {
                IdpeDataSource dataSource = new IdpeDataSource();
                dataSource.Id = (int)row["Id"].ToString().ParseInt();
                dataSource.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                dataSource.Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null;
                dataSource.IsSystem = row["IsSystem"] != DBNull.Value ? row["IsSystem"].ToString().ParseBool() : false;
                dataSource.DataFeederType = row["DataFeederType"] != DBNull.Value ? row["DataFeederType"].ToString().ParseInt() : null;
                dataSource.DataFormatType = row["DataFormatType"] != DBNull.Value ? row["DataFormatType"].ToString().ParseInt() : null;
                dataSource.Delimiter = row["Delimiter"] != DBNull.Value ? row["Delimiter"].ToString() : null;
                dataSource.SystemDataSourceId = row["SystemDataSourceId"] != DBNull.Value ? row["SystemDataSourceId"].ToString().ParseInt() : null;
                dataSource.DataContainerValidatorType = row["DataContainerValidatorType"] != DBNull.Value ? row["DataContainerValidatorType"].ToString() : null;
                dataSource.OutputType = row["OutputType"].ToString() == string.Empty ? 0 : int.Parse(row["OutputType"].ToString());
                dataSource.OutputWriterTypeFullName = row["OutputWriterTypeFullName"] != DBNull.Value ? row["OutputWriterTypeFullName"].ToString() : null;
                dataSource.PlugInsType = row["PlugInsType"] != DBNull.Value ? row["PlugInsType"].ToString() : null;
                dataSource.ProcessingBy = row["ProcessingBy"] != DBNull.Value ? row["ProcessingBy"].ToString() : null;
                dataSource.PusherType = row["PusherType"] != DBNull.Value ? row["PusherType"].ToString().ParseInt(): 0;
                dataSource.PusherTypeFullName = row["PusherTypeFullName"] != DBNull.Value ? row["PusherTypeFullName"].ToString() : null;
                dataSource.IsActive = row["IsActive"].ToString() == string.Empty ? false : bool.Parse(row["IsActive"].ToString());
                dataSource.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                dataSource.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                idpeDataSources.Add(dataSource);
            }
            return idpeDataSources;
        }

        public List<int> GetAllDataSourceIds(bool onlySystem)
        {
            List<int> ids = new List<int>();
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand("select [Id] from [IdpeDataSource] where [IsSystem] = " + (onlySystem ? 1 : 0).ToString());
            if (table == null)
                return ids;
            foreach (DataRow row in table.Rows)
            {
                ids.Add((int)row["Id"].ToString().ParseInt());
            }
            return ids;
        }

        public string GetApplicationName(int dataSourceId)
        {

            string commandText = "select top 1 [Name] from [IdpeDataSource] where Id = " + dataSourceId;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null || table.Rows.Count == 0)
                return "";
            else
                return table.Rows[0]["Name"].ToString();

        }

        public int GetApplicationId(string dataSourceName)
        {
            DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand("select * from idpedatasource where name = '" + dataSourceName + "'");
            if (table.Rows.Count > 0)
                return (int)table.Rows[0]["Id"].ToString().ParseInt();

            return 0;
        }

        public DateTime? GetDataSourceLastUpdatedTime(int dataSourceId)
        {
            DateTime? DtTm = null;
            DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand("select CreatedTS from IdpeDataSource where Id = " + dataSourceId);
            if (table.Rows.Count > 0)
                DtTm = table.Rows[0][0].ToString().ParseDateTime();

            table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand("select Max(CreatedTS) from IdpeAttributeDataSource where DataSourceId = " + dataSourceId);
            if (table.Rows.Count > 0)
            {
                DateTime? aDtTm = table.Rows[0][0].ToString().ParseDateTime();
                if (aDtTm > DtTm)
                    DtTm = aDtTm;
            }

            table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand("select max(k.ModifiedTS) as dt from idpekey k inner join IdpeKeyDataSource kds on k.Keyid = kds.KeyId where kds.DataSourceID = " 
                + dataSourceId);
            if (table.Rows.Count > 0)
            {
                DateTime? aDtTm = table.Rows[0][0].ToString().ParseDateTime();
                if (aDtTm > DtTm)
                    DtTm = aDtTm;
            }

            return DtTm;
        }


        public bool ApplicationExists(IdpeDataSource datasource)
        {
            int dataSourceId = GetApplicationId(datasource.Name);
            return (dataSourceId == 0) ? false : true;
        }

        public bool ApplicationExists(string name)
        {
            int dataSourceId = GetApplicationId(name);
            return (dataSourceId == 0) ? false : true;

        }

        int GetMaxApplicationId(bool? isSystem)
        {
            string strIssystem = isSystem.ToString();

            if (isSystem == true)
                strIssystem = "1";

            if (isSystem == false)
                strIssystem = "0";

            DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand("select MAX(Id) as Id from idpedatasource where IsSystem = '" + strIssystem + "'");
            int maxId = 0;
            if (table.Rows.Count > 0)
                maxId = (int)table.Rows[0]["Id"].ToString().ParseInt();

            if ((isSystem == false)
                && (maxId == 0))
                maxId = 100;
            else if ((isSystem == true)
                && (maxId < 0))
                maxId = 0;

            return maxId;
        }

        public List<IdpeDataSource> GetChildDataSources(int parentDataSourceId)
        {
            List<IdpeDataSource> dataSources = new List<IdpeDataSource>();

            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand();
            command.CommandText = "SELECT * FROM IdpeDataSource WHERE [SystemDataSourceId] = @SystemDataSourceId";
            command.AddParameterWithValue("SystemDataSourceId", parentDataSourceId);
            command.Connection = conn;

            IDataReader reader = command.ExecuteReader();
            IdpeDataSource dataSource = null;
            while (reader.Read())
            {
                dataSource = new IdpeDataSource();
                dataSource.Id = int.Parse(reader["Id"].ToString());
                dataSource.Name = reader["Name"].ToString();
                dataSource.Description = reader["Description"].ToString();
                dataSource.IsSystem = bool.Parse(reader["IsSystem"].ToString());
                dataSource.DataFeederType = reader["DataFeederType"].ToString().ParseInt();
                dataSource.DataFormatType = reader["DataFormatType"].ToString().ParseInt();
                dataSource.Delimiter = reader["Delimiter"].ToString();
                dataSource.SystemDataSourceId = reader["SystemDataSourceId"].ToString() == string.Empty ? 0 : int.Parse(reader["SystemDataSourceId"].ToString());
                dataSource.DataContainerValidatorType = reader["DataContainerValidatorType"].ToString();
                dataSource.OutputType = reader["OutputType"].ToString() == string.Empty ? 0 : int.Parse(reader["OutputType"].ToString());
                dataSource.OutputWriterTypeFullName = reader["OutputWriterTypeFullName"].ToString();
                dataSource.PlugInsType = reader["PlugInsType"].ToString();
                dataSource.ProcessingBy = reader["ProcessingBy"].ToString();
                dataSource.PusherType = reader["PusherType"].ToString() == string.Empty ? 0 : int.Parse(reader["PusherType"].ToString());
                dataSource.PusherTypeFullName = reader["PusherTypeFullName"].ToString();
                dataSource.IsActive = reader["IsActive"].ToString() == string.Empty ? false : bool.Parse(reader["IsActive"].ToString());
                dataSource.CreatedTS = (DateTime)(reader["CreatedTS"] != DBNull.Value ? reader["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                dataSource.CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null;

                dataSources.Add(dataSource);
            }
            reader.Close();
            reader.Dispose();
            command.Dispose();
            conn.Close();
            conn.Dispose();
            return dataSources;
        }

        public void SaveAssociations(int dataSourceId, List<IdpeAttributeDataSource> appAttributes)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);
            int position = 1;
            conn.Open();

            foreach (IdpeAttributeDataSource attribute in appAttributes)
            {
                cmdText = "SELECT [AttributeDataSourceId] FROM IdpeAttributeDataSource WHERE [DataSourceId] = @DataSourceId and [AttributeId] = @AttributeId";
                command.Parameters.Clear();
                command.AddParameterWithValue("DataSourceId", dataSourceId);
                command.AddParameterWithValue("AttributeId", attribute.AttributeId);
                command.CommandText = cmdText;
                IDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    cmdText = "UPDATE [IdpeAttributeDataSource] SET [Position] = @Position, [IsAcceptable] = @IsAcceptable, AttributePrintValueType = @AttributePrintValueType, AttributePrintValueCustom = @AttributePrintValueCustom WHERE [AttributeDataSourceId] = @AttributeDataSourceId";
                    command.Parameters.Clear();
                    command.AddParameterWithValue("Position", position);
                    command.AddParameterWithValue("IsAcceptable", attribute.IsAcceptable);
                    command.AddParameterWithValue("AttributeDataSourceId", attribute.AttributeDataSourceId);
                    if (attribute.AttributePrintValueType != 0)
                        command.AddParameterWithValue("AttributePrintValueType", attribute.AttributePrintValueType);
                    else
                        command.AddParameterWithValue("AttributePrintValueType", DBNull.Value);

                    if (!string.IsNullOrEmpty(attribute.AttributePrintValueCustom))
                        command.AddParameterWithValue("AttributePrintValueCustom", attribute.AttributePrintValueCustom);
                    else
                        command.AddParameterWithValue("AttributePrintValueCustom", DBNull.Value);
                }
                else
                {
                    cmdText = "INSERT INTO [IdpeAttributeDataSource] ([DataSourceId],[AttributeId],[Position],[IsAcceptable],[AttributePrintValueType],[AttributePrintValueCustom],[CreatedTS],[CreatedBy],[Source]) VALUES ";
                    cmdText += "(@DataSourceId,@AttributeId,@Position,@IsAcceptable,@AttributePrintValueType,@AttributePrintValueCustom,@CreatedTS,@CreatedBy,@Source)";
                    command.Parameters.Clear();
                    command.AddParameterWithValue("DataSourceId", dataSourceId);
                    command.AddParameterWithValue("AttributeId", attribute.AttributeId);
                    command.AddParameterWithValue("Position", position);
                    command.AddParameterWithValue("IsAcceptable", attribute.IsAcceptable);
                    if (attribute.AttributePrintValueType != 0)
                        command.AddParameterWithValue("AttributePrintValueType", attribute.AttributePrintValueType);
                    else
                        command.AddParameterWithValue("AttributePrintValueType", DBNull.Value);

                    if (!string.IsNullOrEmpty(attribute.AttributePrintValueCustom))
                        command.AddParameterWithValue("AttributePrintValueCustom", attribute.AttributePrintValueCustom);
                    else
                        command.AddParameterWithValue("AttributePrintValueCustom", DBNull.Value);
                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null? "Debugger": Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", "SRE Util");

                }
                reader.Close();
                command.CommandText = cmdText;
                command.ExecuteNonQuery();
                position++;
            }

        }

        public void SaveCustomConfig(int dataSourceId, string interfaceName, bool isFirstRowIsHeader)
        {
            IdpeKey key = new IdpeKey();
            if (!string.IsNullOrEmpty(interfaceName))
            {
                key.Name = SreKeyTypes.FileInterfaceName.ToString();
                key.Value = interfaceName;
                key.Type = (int)SreKeyTypes.FileInterfaceName;
                Save(key, dataSourceId);
            }

            key.Name = SreKeyTypes.IsFirstRowHeader.ToString();
            key.Value = isFirstRowIsHeader.ToString();
            key.Type = (int)SreKeyTypes.IsFirstRowHeader;
            Save(key, dataSourceId);

        }
   

        public int Save(IdpeDataSource dataSource)
        {
            bool isInserted = false;
            IDal dal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection connection = dal.CreateConnection(_ConnectionString);
            connection.Open();
            IDbTransaction transaction = dal.CreateTransaction(connection);
            int dataSourceId = 0;

            try
            {
                dataSourceId = Save(dataSource, ref isInserted, dal, connection, transaction);
                dataSource.Id = dataSourceId;

                if (isInserted && dataSource.IsSystem == true)
                {
                    IDbCommand command = dal.CreateCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = "INSERT INTO IdpeAttributeDataSource(DataSourceId, AttributeId, Position, CreatedTS, CreatedBy, Source) ";
                    command.CommandText += " VALUES (@DataSourceId, @AttributeId, @Position, @CreatedTS, @CreatedBy, @Source)";

                    command.AddParameterWithValue("DataSourceId", dataSourceId);
                    command.AddParameterWithValue("AttributeId", GetIsValidAttributeId(dal, connection, transaction));
                    command.AddParameterWithValue("Position", 1);
                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null? "Debugger": Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", "SRE Util");

                    if (connection.State != ConnectionState.Open) connection.Open();
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
                if (connection.State == ConnectionState.Open) connection.Close();
                connection.Dispose();
                transaction.Dispose();
            }

            return dataSourceId;

        }

        public int Save(IdpeDataSource dataSource, ref bool isInserted, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            int dataSourceId = 0;
            string cmdText = string.Empty;
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

            command = dal.CreateCommand(cmdText, connection);
            command.Transaction = transaction;

            try
            {
                if (!ApplicationExists(dataSource))
                {
                    if (dataSource.IsSystem == false)
                    {
                        cmdText = "INSERT INTO [IdpeDataSource] ([Id],[Name],[Description],[IsSystem],[DataFeederType],[DataFormatType],[Delimiter],[SystemDataSourceId],[DataContainerValidatorType],[OutputType],[OutputWriterTypeFullName],[PlugInsType],[ProcessingBy],[PusherType],[PusherTypeFullName], [IsActive],[CreatedTS],[CreatedBy]) ";
                        cmdText += "VALUES (@Id,@Name,@Description,@IsSystem,@DataFeederType,@DataFormatType,@Delimiter,@SystemDataSourceId,@DataContainerValidatorType,@OutputType, @OutputWriterTypeFullName,@PlugInsType,@ProcessingBy,@PusherType,@PusherTypeFullName, @IsActive,@CreatedTS,@CreatedBy)";

                        command.AddParameterWithValue("Id", GetMaxApplicationId(dataSource.IsSystem) + 1);
                        command.AddParameterWithValue("Name", dataSource.Name);
                        command.AddParameterWithValue("Description", string.IsNullOrEmpty(dataSource.Description) ? dataSource.Name : dataSource.Description);
                        command.AddParameterWithValue("IsSystem", dataSource.IsSystem);
                        command.AddParameterWithValue("DataFeederType", dataSource.DataFeederType);
                        command.AddParameterWithValue("DataFormatType", dataSource.DataFormatType);
                        if (!string.IsNullOrEmpty(dataSource.Delimiter))
                        {
                            command.AddParameterWithValue("Delimiter", dataSource.Delimiter);
                        }
                        else
                        {
                            if ((DataFormatTypes)dataSource.DataFormatType == DataFormatTypes.Delimited)
                                command.AddParameterWithValue("Delimiter", ",");
                            else
                                command.AddParameterWithValue("Delimiter", DBNull.Value);
                        }

                        command.AddParameterWithValue("SystemDataSourceId", dataSource.SystemDataSourceId);

                        if (!string.IsNullOrEmpty(dataSource.DataContainerValidatorType))
                            command.AddParameterWithValue("DataContainerValidatorType", dataSource.DataContainerValidatorType);
                        else
                            command.AddParameterWithValue("DataContainerValidatorType", DBNull.Value);

                        if (dataSource.OutputType != null)
                            command.AddParameterWithValue("OutputType", dataSource.OutputType);
                        else
                            command.AddParameterWithValue("OutputType", OutputTypes.Xml);

                        if (!string.IsNullOrEmpty(dataSource.OutputWriterTypeFullName))
                            command.AddParameterWithValue("OutputWriterTypeFullName", dataSource.OutputWriterTypeFullName);
                        else
                            command.AddParameterWithValue("OutputWriterTypeFullName", DBNull.Value);

                        if (!string.IsNullOrEmpty(dataSource.PlugInsType))
                            command.AddParameterWithValue("PlugInsType", dataSource.PlugInsType);
                        else
                            command.AddParameterWithValue("PlugInsType", DBNull.Value);

                        if (!string.IsNullOrEmpty(dataSource.ProcessingBy))
                            command.AddParameterWithValue("ProcessingBy", dataSource.ProcessingBy);
                        else
                            command.AddParameterWithValue("ProcessingBy", DBNull.Value);

                        command.AddParameterWithValue("PusherType", dataSource.PusherType);

                        if (!string.IsNullOrEmpty(dataSource.PusherTypeFullName))
                            command.AddParameterWithValue("PusherTypeFullName", dataSource.PusherTypeFullName);
                        else
                            command.AddParameterWithValue("PusherTypeFullName", DBNull.Value);

                        command.AddParameterWithValue("IsActive", dataSource.IsActive);

                        command.AddParameterWithValue("CreatedTS", DateTime.Now);
                        command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    }
                    else
                    {
                        cmdText = "INSERT INTO [IdpeDataSource] ([Id],[Name],[Description],[IsSystem],[CreatedTS],[CreatedBy]) VALUES ";
                        cmdText += "(@Id,@Name,@Description,@IsSystem,@CreatedTS,@CreatedBy)";

                        command.AddParameterWithValue("Id", GetMaxApplicationId(dataSource.IsSystem) + 1);
                        command.AddParameterWithValue("Name", dataSource.Name);
                        command.AddParameterWithValue("Description", string.IsNullOrEmpty(dataSource.Description) ? dataSource.Name : dataSource.Description);
                        command.AddParameterWithValue("IsSystem", dataSource.IsSystem);
                        command.AddParameterWithValue("CreatedTS", DateTime.Now);
                        command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    }

                    isInserted = true;

                }
                else
                {
                    cmdText = "UPDATE [IdpeDataSource] SET [Name] = @Name,[Description] = @Description, ";
                    cmdText += "[DataFeederType] = @DataFeederType, [DataFormatType] = @DataFormatType, [Delimiter] = @Delimiter, [SystemDataSourceId] = @SystemDataSourceId, ";
                    cmdText += "[DataContainerValidatorType] = @DataContainerValidatorType,[OutputType] = @OutputType,[OutputWriterTypeFullName] = @OutputWriterTypeFullName,[PlugInsType] = @PlugInsType, ";
                    cmdText += "[ProcessingBy] = @ProcessingBy, [PusherType] = @PusherType, [PusherTypeFullName] = @PusherTypeFullName, [IsActive] = @IsActive ";
                    cmdText += "WHERE [Id] = @Id";

                    command.Parameters.Clear();
                    command.AddParameterWithValue("Name", dataSource.Name);
                    command.AddParameterWithValue("Description", string.IsNullOrEmpty(dataSource.Description) ? dataSource.Name : dataSource.Description);
                    command.AddParameterWithValue("DataFeederType", dataSource.DataFeederType);
                    if (dataSource.DataFormatType != null)
                        command.AddParameterWithValue("DataFormatType", dataSource.DataFormatType);
                    else
                        command.AddParameterWithValue("DataFormatType", DBNull.Value);

                    if (!string.IsNullOrEmpty(dataSource.Delimiter))
                        command.AddParameterWithValue("Delimiter", dataSource.Delimiter);
                    else
                        command.AddParameterWithValue("Delimiter", DBNull.Value);

                    if ((dataSource.SystemDataSourceId != null) && (dataSource.SystemDataSourceId != 0))
                        command.AddParameterWithValue("SystemDataSourceId", dataSource.SystemDataSourceId);
                    else
                        command.AddParameterWithValue("SystemDataSourceId", DBNull.Value);

                    if (!string.IsNullOrEmpty(dataSource.DataContainerValidatorType))
                        command.AddParameterWithValue("DataContainerValidatorType", dataSource.DataContainerValidatorType);
                    else
                        command.AddParameterWithValue("DataContainerValidatorType", DBNull.Value);

                    if (dataSource.OutputType != null)
                        command.AddParameterWithValue("OutputType", dataSource.OutputType);
                    else
                        command.AddParameterWithValue("OutputType", OutputTypes.Xml);

                    if (!string.IsNullOrEmpty(dataSource.OutputWriterTypeFullName))
                        command.AddParameterWithValue("OutputWriterTypeFullName", dataSource.OutputWriterTypeFullName);
                    else
                        command.AddParameterWithValue("OutputWriterTypeFullName", DBNull.Value);

                    if (!string.IsNullOrEmpty(dataSource.PlugInsType))
                        command.AddParameterWithValue("PlugInsType", dataSource.PlugInsType);
                    else
                        command.AddParameterWithValue("PlugInsType", DBNull.Value);

                    if (!string.IsNullOrEmpty(dataSource.ProcessingBy))
                        command.AddParameterWithValue("ProcessingBy", dataSource.ProcessingBy);
                    else
                        command.AddParameterWithValue("ProcessingBy", DBNull.Value);

                    command.AddParameterWithValue("PusherType", dataSource.PusherType);

                    if (!string.IsNullOrEmpty(dataSource.PusherTypeFullName))
                        command.AddParameterWithValue("PusherTypeFullName", dataSource.PusherTypeFullName);
                    else
                        command.AddParameterWithValue("PusherTypeFullName", DBNull.Value);

                    command.AddParameterWithValue("IsActive", dataSource.IsActive);

                    command.AddParameterWithValue("Id", dataSource.Id);

                    dataSourceId = dataSource.Id;
                }

                if (connection.State != ConnectionState.Open) connection.Open();
                command.CommandText = cmdText;
                command.ExecuteNonQuery();

                if (localTransaction)
                    transaction.Commit();
                if (isInserted)
                {
                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.CommandText = "SELECT max(Id) from IdpeDataSource where IsSystem = @IsSystem";
                    command.Parameters.Clear();
                    command.AddParameterWithValue("IsSystem", dataSource.IsSystem);
                    dataSourceId = (Int32)command.ExecuteScalar();
                }
               
            }
            catch (Exception ex)
            {
                if (localTransaction)
                    transaction.Rollback();

                Trace.TraceError("Error while saving IdpeRuleDataSource " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
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

            return dataSourceId;

        }

        public void SaveFTPConfig(int dataSourceId, string ftpRemoteLocation, string ftpLocalLocation,
                    string ftpUserName, string ftpPassword, int interval, string filter)
        {
            IdpeKey key = new IdpeKey();
            key.Name = SreKeyTypes.FtpRemoteLocation.ToString();
            key.Value = ftpRemoteLocation;
            key.Type = (int)SreKeyTypes.FtpRemoteLocation;
            Save( key, dataSourceId);

            key.Name = SreKeyTypes.LocalLocation.ToString();
            key.Value = ftpLocalLocation;
            key.Type = (int)SreKeyTypes.LocalLocation;
            Save( key, dataSourceId);

            key.Name = SreKeyTypes.FtpUserName.ToString();
            key.Value = ftpUserName;
            key.Type = (int)SreKeyTypes.FtpUserName;
            Save( key, dataSourceId);

            key.Name = SreKeyTypes.FtpPassword.ToString();
            key.Value = ftpPassword;
            key.Type = (int)SreKeyTypes.FtpPassword;
            Save( key, dataSourceId);

            key.Name = SreKeyTypes.FtpWatchInterval.ToString();
            key.Value = interval.ToString();
            key.Type = (int)SreKeyTypes.FtpWatchInterval;
            Save( key, dataSourceId);

            if (!string.IsNullOrEmpty(filter))
            {
                key.Name = SreKeyTypes.WatchFilter.ToString();
                key.Value = filter;
                key.Type = (int)SreKeyTypes.WatchFilter;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.WatchFilter.ToString(), true);
            }

        }

        public void SaveLocalFSConfig(int dataSourceId, string filter, bool localFileSystemFoldersOverriden,
            bool localFileSystemFolderArchiveAuto, string localFileSystemFolderPullFolder, string localFileSystemFolderArchiveFolder, string localFileSystemFolderOutputFolder)
        {
            IdpeKey key = new IdpeKey();

            if (!string.IsNullOrEmpty(filter))
            {
                key.Name = SreKeyTypes.WatchFilter.ToString();
                key.Value = filter;
                key.Type = (int)SreKeyTypes.WatchFilter;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.WatchFilter.ToString(), true);
            }

            if (localFileSystemFoldersOverriden)
            {
                key.Name = SreKeyTypes.LocalFileSystemFoldersOverriden.ToString();
                key.Value = "true";
                key.Type = (int)SreKeyTypes.LocalFileSystemFoldersOverriden;

                key.Name = SreKeyTypes.LocalFileSystemFolderPull.ToString();
                key.Value = localFileSystemFolderPullFolder;
                key.Type = (int)SreKeyTypes.LocalFileSystemFolderPull;
                Save( key, dataSourceId);

                if (!localFileSystemFolderArchiveAuto)
                {
                    key.Name = SreKeyTypes.LocalFileSystemFolderArchiveAuto.ToString();
                    key.Value = "true";
                    key.Type = (int)SreKeyTypes.LocalFileSystemFolderArchiveAuto;
                    Save( key, dataSourceId);

                    key.Name = SreKeyTypes.LocalFileSystemFolderArchive.ToString();
                    key.Value = localFileSystemFolderArchiveFolder;
                    key.Type = (int)SreKeyTypes.LocalFileSystemFolderArchive;
                    Save( key, dataSourceId);
                }
                else
                {
                    DeleteKeyFromApplication(dataSourceId, SreKeyTypes.LocalFileSystemFolderArchive.ToString(), true);
                }

                key.Name = SreKeyTypes.LocalFileSystemFolderOutput.ToString();
                key.Value = localFileSystemFolderOutputFolder;
                key.Type = (int)SreKeyTypes.LocalFileSystemFolderOutput;
                Save( key, dataSourceId);

            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.LocalFileSystemFoldersOverriden.ToString(), true);
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.LocalFileSystemFolderArchiveAuto.ToString(), true);
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.LocalFileSystemFolderArchive.ToString(), true);
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.LocalFileSystemFolderOutput.ToString(), true);
            }


        }

        public void SaveXmlConfig(int dataSourceId, string interaceName)
        {
            IdpeKey key = new IdpeKey();
            if (!string.IsNullOrEmpty(interaceName))
            {
                key.Name = SreKeyTypes.FileInterfaceName.ToString();
                key.Value = interaceName;
                key.Type = (int)SreKeyTypes.FileInterfaceName;
                Save( key, dataSourceId);
            }

            key.Name = SreKeyTypes.WatchFilter.ToString();
            key.Value = ".xml";
            key.Type = (int)SreKeyTypes.WatchFilter;
            Save( key, dataSourceId);

        }

        public void SavePushConfig(int dataSourceId, bool wcfCallsGenerateStandardOutput)
        {
            if (wcfCallsGenerateStandardOutput)
            {
                IdpeKey key = new IdpeKey();
                key.Name = SreKeyTypes.WcfCallsGenerateStandardOutput.ToString();
                key.Value = "1";
                key.Type = (int)SreKeyTypes.WcfCallsGenerateStandardOutput;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.WcfCallsGenerateStandardOutput.ToString(), true);
            }

        }

        public void SaveZipConfig(int dataSourceId, bool zipDoNotCreateAcknoledgementInOutputFolder, int zipConfigType, SreKeyTypes sortType, string zipSortTypeValue, string zipIgnoreList,
            string zipIgnoreListButCopy, bool isFirstRowIsHeaderInZipFile,
            string zipInteraceName, string interaceName, string overriddenOutputFolder, string pusherTypeFullName)
        {
            IdpeKey key = new IdpeKey();

            if (zipDoNotCreateAcknoledgementInOutputFolder)
            {
                key.Name = SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder.ToString();
                key.Value = "1";
                key.Type = (int)SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder.ToString(), true);
            }



            if (!string.IsNullOrEmpty(zipIgnoreList))
            {
                key.Name = SreKeyTypes.ZipIgnoreFileList.ToString();
                key.Value = zipIgnoreList.ToLower();
                key.Type = (int)SreKeyTypes.ZipIgnoreFileList;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.ZipIgnoreFileList.ToString(), true);
            }

            if (!string.IsNullOrEmpty(zipIgnoreListButCopy))
            {
                key.Name = SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder.ToString();
                key.Value = zipIgnoreListButCopy.ToLower();
                key.Type = (int)SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder.ToString(), true);
            }

            if (isFirstRowIsHeaderInZipFile)
            {
                key.Name = SreKeyTypes.IsFirstRowHeader.ToString();
                key.Value = isFirstRowIsHeaderInZipFile.ToString();
                key.Type = (int)SreKeyTypes.IsFirstRowHeader;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.IsFirstRowHeader.ToString(), true);
            }

            if (!string.IsNullOrEmpty(zipInteraceName))
            {
                key.Name = SreKeyTypes.ZipInterfaceName.ToString();
                key.Value = zipInteraceName;
                key.Type = (int)SreKeyTypes.ZipInterfaceName;
                Save(key, dataSourceId);
            }

            if (!string.IsNullOrEmpty(interaceName))
            {
                key.Name = SreKeyTypes.FileInterfaceName.ToString();
                key.Value = interaceName;
                key.Type = (int)SreKeyTypes.FileInterfaceName;
                Save(key, dataSourceId);
            }

            if (!string.IsNullOrEmpty(pusherTypeFullName))
            {
                IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
                IDbConnection conn = myDal.CreateConnection(_ConnectionString);
                conn.Open();
                IDbCommand command = myDal.CreateCommand();
                command.Connection = conn;

                try
                {

                    command.CommandText = "UPDATE [IdpeDataSource] SET [PusherTypeFullName] = @PusherTypeFullName ";
                    command.CommandText += "WHERE [Id] = @Id";
                    command.AddParameterWithValue("PusherTypeFullName", pusherTypeFullName);
                    command.AddParameterWithValue("Id", dataSourceId);
                    command.ExecuteNonQuery();

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

            }


        }

        public void SavePullSqlConfig(int dataSourceId, string connectionString,
            bool isDirectFeed, string selectQuery, string updateQuery, string recoveryQuery,
            string interfaceName, int interval, bool isFirstRowIsHeader, string pullSqlConnectionStringRunTime)
        {
            IdpeKey key = new IdpeKey();

            if ((dataSourceId == 0)
                || (string.IsNullOrEmpty(connectionString))
                || (string.IsNullOrEmpty(selectQuery)))

                throw new Exception("DataSource id or connection string or query or interface name can not be empty string!");


            key.Name = SreKeyTypes.PullSqlConnectionString.ToString();
            key.Value = connectionString;
            key.Type = (int)SreKeyTypes.PullSqlConnectionString;
            Save(key, dataSourceId);
         

            key.Name = SreKeyTypes.PullSqlReturnType.ToString();
            key.Value = isDirectFeed ? "D" : "I";
            key.Type = (int)SreKeyTypes.PullSqlReturnType;
            Save(key, dataSourceId);

            key.Name = SreKeyTypes.SqlQuery.ToString();
            key.Value = selectQuery;
            key.Type = (int)SreKeyTypes.SqlQuery;
            Save(key, dataSourceId);

            key.Name = SreKeyTypes.SqlUpdateQueryProcessing.ToString();
            key.Value = updateQuery;
            key.Type = (int)SreKeyTypes.SqlUpdateQueryProcessing;
            Save(key, dataSourceId);

            key.Name = SreKeyTypes.SqlUpdateQueryRecovery.ToString();
            key.Value = recoveryQuery;
            key.Type = (int)SreKeyTypes.SqlUpdateQueryRecovery;
            Save(key, dataSourceId);

            key.Name = SreKeyTypes.IsFirstRowHeader.ToString();
            key.Value = isFirstRowIsHeader.ToString();
            key.Type = (int)SreKeyTypes.IsFirstRowHeader;
            Save(key, dataSourceId);

            key.Name = SreKeyTypes.PullSqlConnectionStringRunTime.ToString();
            key.Value = pullSqlConnectionStringRunTime.ToString();
            key.Type = (int)SreKeyTypes.PullSqlConnectionStringRunTime;
            Save(key, dataSourceId);

            if (!isDirectFeed)
            {
                key.Name = SreKeyTypes.PullSqlInterfaceName.ToString();
                key.Value = interfaceName;
                key.Type = (int)SreKeyTypes.PullSqlInterfaceName;
                Save(key, dataSourceId);
            }

            key.Name = SreKeyTypes.SqlWatchInterval.ToString();
            key.Value = interval.ToString();
            key.Type = (int)SreKeyTypes.SqlWatchInterval;
            Save(key, dataSourceId);

        }

        public void SaveConfig(int dataSourceId, List<string> headers, List<string> footers, string fixedLengthSchema = null, string sqlQuery = null)
        {
            int counter = 1;
            IdpeKey key = new IdpeKey();
            #region Delete Existing Headers & Footers

            for (int i = 1; i <= 6; i++)
            {
                string keyName = "HeaderLine" + i + "Attribute";
                DeleteKeyFromApplication(dataSourceId, keyName, true);

            }

            for (int i = 1; i <= 6; i++)
            {
                string keyName = "FooterLine" + i + "Attribute";
                DeleteKeyFromApplication(dataSourceId, keyName, true);
            }

            #endregion Delete Existing Headers & Footers

            foreach (string header in headers)
            {
                string keyName = "HeaderLine" + counter + "Attribute";
                SreKeyTypes keyType = (SreKeyTypes)Enum.Parse(typeof(SreKeyTypes), keyName);
                key = new IdpeKey();
                key.Name = keyType.ToString();
                key.Value = header;
                key.Type = (int)keyType;
                Save(key, dataSourceId);
                counter++;
            }

            counter = 1;
            foreach (string footer in footers)
            {
                string keyName = "FooterLine" + counter + "Attribute";
                SreKeyTypes keyType = (SreKeyTypes)Enum.Parse(typeof(SreKeyTypes), keyName);
                key = new IdpeKey();
                key.Name = keyType.ToString();
                key.Value = footer;
                key.Type = (int)keyType;
                Save(key, dataSourceId);
                counter++;
            }

            if (!string.IsNullOrEmpty(fixedLengthSchema))
            {
                key.Name = SreKeyTypes.FixedLengthSchema.ToString();
                key.Value = fixedLengthSchema;
                key.Type = (int)SreKeyTypes.FixedLengthSchema;
                Save(key, dataSourceId);
            }

            if (!string.IsNullOrEmpty(sqlQuery))
            {
                key.Name = SreKeyTypes.SqlQuery.ToString();
                key.Value = sqlQuery;
                key.Type = (int)SreKeyTypes.SqlQuery;
                Save(key, dataSourceId);
            }


        }

        public void SaveConfigFixedLength(int dataSourceId, string fixedLengthSchema, string fixedLegnthHeaderAttribute, string fixedLegnthFooterAttribute)
        {
            IdpeKey key = new IdpeKey();
            if (!string.IsNullOrEmpty(fixedLengthSchema))
            {
                key.Name = SreKeyTypes.FixedLengthSchema.ToString();
                key.Value = fixedLengthSchema;
                key.Type = (int)SreKeyTypes.FixedLengthSchema;
                Save(key, dataSourceId);
            }

            if (!string.IsNullOrEmpty(fixedLegnthHeaderAttribute))
            {
                key.Name = SreKeyTypes.FixedLengthHeaderAttribute.ToString();
                key.Value = fixedLegnthHeaderAttribute;
                key.Type = (int)SreKeyTypes.FixedLengthHeaderAttribute;
                Save(key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.FixedLengthHeaderAttribute.ToString(), true);
            }

            if (!string.IsNullOrEmpty(fixedLegnthFooterAttribute))
            {
                key.Name = SreKeyTypes.FixedLengthFooterAttribute.ToString();
                key.Value = fixedLegnthFooterAttribute;
                key.Type = (int)SreKeyTypes.FixedLengthFooterAttribute;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.FixedLengthFooterAttribute.ToString(), true);
            }


        }

        public string GenerateDuplicateCheckStoreProcedureCode(int dataSourceId, DatabaseTypes databaseType)
        {
            string strDynamicCode = string.Format("if (@dataSourceId = {0}){1}", dataSourceId, Environment.NewLine);
            strDynamicCode += "\tbegin" + Environment.NewLine;
            strDynamicCode += "\t\tselect " + Environment.NewLine;
            strDynamicCode += "\t\t\t--correct varchar(n) to varchar(n±) if required " + Environment.NewLine;
            strDynamicCode += "\t\t\tinputData.value('(Position)[1]', 'int') as 'Position'," + Environment.NewLine;

            //List<IdpeAttribute> attrbs = GetAttributes(dataSourceId);
            IdpeKey keyUniquenessCriteria = GetKey(dataSourceId, "UniquenessCriteria");
            if (keyUniquenessCriteria == null)
                throw new BusinessException(string.Format("'UniquenessCriteria' is not defined for {1}", dataSourceId));

            string[] uniqueAttributes = keyUniquenessCriteria.Value.Split("+".ToCharArray());
            foreach (string uniqueAttribute in uniqueAttributes)
            {
                IdpeAttribute attrb = GetAttribute(uniqueAttribute);
                strDynamicCode += string.Format("\t\t\tinputData.value('({0})[1]', '{1}') as '{2}', {3}",
                    attrb.Name, ConvertSreTypeIntoDatabaseType(databaseType, attrb.Type), attrb.Name, Environment.NewLine);
            }
            strDynamicCode += "\t\t\tinputData.value('(IsDuplicate)[1]', 'bit') as 'IsDuplicate'" + Environment.NewLine;
            strDynamicCode += "\t\tfrom" + Environment.NewLine;
            strDynamicCode += "\t\t\t@inputData.nodes('/DocumentElement/SreInputData') as DocumentElement(InputData)" + Environment.NewLine;

            strDynamicCode += Environment.NewLine;
            strDynamicCode += Environment.NewLine;

            strDynamicCode += "\t\t\t--loop through the data here & update IsDuplicate to true/false" + Environment.NewLine;
            strDynamicCode += "\t\t\t--OR" + Environment.NewLine;
            strDynamicCode += "\t\t\t--return Sql query result with exact same number of columns" + Environment.NewLine;

            strDynamicCode += "\tend" + Environment.NewLine + Environment.NewLine;

            strDynamicCode += string.Format("\t--elseif (@dataSourceId = {0}){1}", dataSourceId + 1, Environment.NewLine);
            strDynamicCode += "\t--begin" + Environment.NewLine;
            strDynamicCode += Environment.NewLine;
            strDynamicCode += "\t--end" + Environment.NewLine;

            string strCode = strDynamicCode;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.DataManager.SreDuplicateCheck.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                strCode = reader.ReadToEnd();
            }

            return strCode = string.Format(strCode, strDynamicCode);
        }

        private string ConvertSreTypeIntoDatabaseType(DatabaseTypes databaseType, string attributeType)
        {
            switch (databaseType)
            {
                default:
                    switch (attributeType.ToUpper())
                    {
                        case "INT":
                            return "int";

                        case "BIGINT":
                            return "long";

                        case "DECIMAL":
                            return "decimal(20,2)";

                        case "BIT":
                            return "bit";

                        case "GENERATED":
                        case "NOTREFERENCED":
                        case "REFERENCED":
                        case "CODESET":
                        case "STRING":
                            return "varchar(500)";

                        case "DATETIME":
                            return "datetime";

                        default:
                            return "varchar(500)";
                    }
            }
        }

        public DataTable GetDataSourcesAsDataTable()
        {
            string commandText = "select [Id],[Name],[Description],[IsSystem],[DataFeederType],[DataFormatType],[Delimiter],[SystemDataSourceId],[DataContainerValidatorType], ";
            commandText += "[OutputType],[OutputWriterTypeFullName],[PlugInsType],[ProcessingBy],[PusherType],[PusherTypeFullName],[IsActive],[CreatedTS],[CreatedBy] ";
            commandText += "from [IdpeDataSource] where [IsSystem] = 0";

            return CoreDatabaseObjects.Instance.ExecuteCommand(commandText);

        }

        public void SaveOutputWriterCSharpCode(int dataSourceId, string cSharpOutputWriterCode)
        {
            IdpeKey key = new IdpeKey();
            if (!string.IsNullOrEmpty(cSharpOutputWriterCode))
            {
                key.Name = SreKeyTypes.CSharpCodeOutputWriter.ToString();
                key.Value = cSharpOutputWriterCode;
                key.Type = (int)SreKeyTypes.CSharpCodeOutputWriter;
                Save( key, dataSourceId);
            }
            else
            {
                DeleteKeyFromApplication(dataSourceId, SreKeyTypes.CSharpCodeOutputWriter.ToString(), true);
            }

        }

        public void UpdateDelimiter(int dataSourceId, string delimiter)
        {
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand();
            command.Connection = conn;

            try
            {
                command.CommandText = "UPDATE [IdpeDataSource] SET [Delimiter] = @Delimiter ";
                command.CommandText += "WHERE [Id] = @Id";
                command.AddParameterWithValue("Delimiter", delimiter);
                command.AddParameterWithValue("Id", dataSourceId);
                command.ExecuteNonQuery();

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
        }

        public void UpdateDataFormatType(int dataSourceId, DataFormatTypes dataFormatType)
        {
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand();
            command.Connection = conn;

            try
            {
                command.CommandText = "UPDATE [IdpeDataSource] SET [DataFormatType] = @DataFormatType ";
                command.CommandText += "WHERE [Id] = @Id";
                command.AddParameterWithValue("DataFormatType", (int)dataFormatType);
                command.AddParameterWithValue("Id", dataSourceId);
                command.ExecuteNonQuery();

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
        }

        public int GetDataSourceCountUnderSystemDataSource(int systemDataSourceId)
        {
            
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand("select count(*) from IdpeDataSource where SystemDataSourceId = " + systemDataSourceId);
            if (table == null || table.Rows.Count == 0)
                return 0;

            return (int)table.Rows[0][0].ToString().ParseInt();           
        }

        #region Delete datasource
        public void DeleteDataSource(string dataSourceName, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            int dataSourceId = GetApplicationId(dataSourceName);
            DeleteDataSource(dataSourceId, dal, connection, transaction);
        }

        public void DeleteSystemDataSource(int dataSourceId, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {            
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
                IDbCommand command = dal.CreateCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                command.CommandText = "DELETE from IdpeDataSource where Id = " + dataSourceId;
                command.ExecuteNonQuery();
                if (localTransaction)
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

        public void DeleteDataSource(int dataSourceId, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            List<IdpeAttribute> attrbs = GetAttributes(dataSourceId);
            List<IdpeKey> keys = GetKeys(dataSourceId);
            List<IdpeRule> rules = GetRules(dataSourceId);

            #region Delete DataSource & other relations

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
                IDbCommand command = dal.CreateCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                command.CommandText = "DELETE from IdpePersistentVariable where DataSourceId = " + dataSourceId;
                command.ExecuteNonQuery();

                command.CommandText = "DELETE from IdpeAttributeDataSource where DataSourceId = " + dataSourceId;
                command.ExecuteNonQuery();

                command.CommandText = "DELETE from IdpeKeyDataSource where DataSourceId = " + dataSourceId;
                command.ExecuteNonQuery();

                command.CommandText = "DELETE from IdpeRuleDataSource where DataSourceId = " + dataSourceId;
                command.ExecuteNonQuery();

                command.CommandText = "DELETE from IdpeLog where DataSourceId = " + dataSourceId;
                command.ExecuteNonQuery();

                command.CommandText = "DELETE from IdpeDataSource where Id = " + dataSourceId;
                command.ExecuteNonQuery();

                if (localTransaction)
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Rollback started... Deleting failed for dataSource - " + dataSourceId);
                if (localTransaction)
                    transaction.Rollback();
                Trace.TraceInformation("Rollback Completed... Deleting failed for dataSource - " + dataSourceId);
                Trace.TraceInformation(ex.Message);
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

            #endregion Delete DataSource & other relations

            #region trying to delete master records

            foreach (IdpeAttribute attrb in attrbs)
            {
                try
                {
                    //if associated with other data source it will throw error, which we can ignore here
                    DeleteAttribute(attrb.AttributeId);
                }
                catch { }
            }


            foreach (IdpeKey key in keys)
            {
                try
                {
                    //if associated with other data source it will throw error, which we can ignore here
                    DeleteKey(key.KeyId);
                }
                catch { }
            }



            foreach (IdpeRule rule in rules)
            {
                try
                {
                    //if associated with other data source it will throw error, which we can ignore here
                    if (!Information.IsInternalRule(rule.Name))
                        DeleteRule(rule.Name);
                }
                catch { }
            }

            #endregion trying to delete master records
        }

        #endregion  Delete datasource

    }
}


