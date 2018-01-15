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
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Common;
using Eyedia.IDPE.Common;
using System.Data;
using Eyedia.Core;
using Eyedia.Core.Data;
using System.Diagnostics;
using System.Globalization;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {
        public List<IdpeAttribute> GetAttributes()
        {
            List<IdpeAttribute> attrbs = new List<IdpeAttribute>();
            string commandText = "select [AttributeId],[Name],[Type],[Minimum],[Maximum],[Formula],[IsAcceptable],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [IdpeAttribute]";

            DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return attrbs;

            foreach (DataRow row in table.Rows)
            {
                IdpeAttribute attrb = new IdpeAttribute();
                attrb.AttributeId = (int)row["AttributeId"].ToString().ParseInt();
                attrb.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                attrb.Type = row["Type"] != DBNull.Value ? new CultureInfo("en-US", false).TextInfo.ToTitleCase(row["Type"].ToString().ToLower()) : null;
                attrb.Minimum = row["Minimum"] != DBNull.Value ? row["Minimum"].ToString() : null;
                attrb.Maximum = row["Maximum"] != DBNull.Value ? row["Maximum"].ToString() : null;
                attrb.Formula = row["Formula"] != DBNull.Value ? row["Formula"].ToString() : null;
                attrb.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
                attrb.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                attrb.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                attrb.ModifiedTS = row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : null;
                attrb.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
                attrb.Source = row["Source"] != DBNull.Value ? row["Source"].ToString() : null;
                attrbs.Add(attrb);
            }
            table.Dispose();
            return attrbs;
        }

        public DataTable GetAttributesAsDataTable(int dataSourceId = 0, bool onlyAcceptable = false)
        {
            string sqlQuery = string.Empty;
            if (dataSourceId == 0)
            {
                sqlQuery = "select [AttributeId],[Name],[Type],[Minimum],[Maximum],[Formula],[IsAcceptable],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [IdpeAttribute] order by [Name]";
            }
            else
            {
                sqlQuery = "select a.[AttributeId],a.[Name],a.[Type],a.[Minimum],a.[Maximum],a.[Formula],aes.[IsAcceptable], aes.[Position], aes.[AttributePrintValueType], aes.[AttributePrintValueCustom],a.[CreatedTS],a.[CreatedBy],a.[ModifiedTS],a.[ModifiedBy],a.[Source] " +
                " from  idpeAttribute a Inner join idpeAttributeDataSource aes  on a.AttributeId = aes.AttributeId " +
                         " inner join idpeDataSource es on aes.DataSourceId = es.Id where es.Id =  " + dataSourceId;
                if (onlyAcceptable)
                    sqlQuery += " and aes.[IsAcceptable] = 1";

                sqlQuery += " order by aes.Position";
            }
            
            DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand(sqlQuery);
            if (dataSourceId > 0)
                table = table.AsEnumerable()
                      .Where(r => r.Field<string>("Name") != "IsValid")
                      .CopyToDataTable();

            foreach (DataRow row in table.Rows)
            {
                row["Type"] = EyediaCoreConfigurationSection.CurrentConfig.CurrentCulture.TextInfo.ToTitleCase(row["Type"].ToString().ToLower());
            }

            return table;
        }

        public IdpeAttribute GetAttribute(int id)
        {
            IdpeAttribute attrb = new IdpeAttribute();
            string commandText = "select [AttributeId],[Name],[Type],[Minimum],[Maximum],[Formula],[IsAcceptable],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [IdpeAttribute] ";
            commandText += "where [AttributeId] = " + id;

            DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if ((table == null)
                || (table.Rows.Count == 0))
                return attrb;

            DataRow row = table.Rows[0];
            
            attrb.AttributeId = (int)row["AttributeId"].ToString().ParseInt();
            attrb.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
            attrb.Type = row["Type"] != DBNull.Value ? new CultureInfo("en-US", false).TextInfo.ToTitleCase(row["Type"].ToString().ToLower()) : null;
            attrb.Minimum = row["Minimum"] != DBNull.Value ? row["Minimum"].ToString() : null;
            attrb.Maximum = row["Maximum"] != DBNull.Value ? row["Maximum"].ToString() : null;
            attrb.Formula = row["Formula"] != DBNull.Value ? row["Formula"].ToString() : null;
            attrb.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
            attrb.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
            attrb.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
            attrb.ModifiedTS = row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : null;
            attrb.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
            attrb.Source = row["Source"] != DBNull.Value ? row["Source"].ToString() : null;
            
            table.Dispose();
            return attrb;
        }

        public IdpeAttribute GetAttribute(string name, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            IdpeAttribute attrb = new IdpeAttribute();
            string commandText = "select [AttributeId],[Name],[Type],[Minimum],[Maximum],[Formula],[IsAcceptable],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [IdpeAttribute] ";
            commandText += "where [Name] = '" + name + "'";

            DataTable table = null;
            if(dal == null)
                table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            else
                table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText, dal, connection, transaction);

            if ((table == null)
                || (table.Rows.Count == 0))
                return null;

            DataRow row = table.Rows[0];

            attrb.AttributeId = (int)row["AttributeId"].ToString().ParseInt();
            attrb.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
            attrb.Type = row["Type"] != DBNull.Value ? new CultureInfo("en-US", false).TextInfo.ToTitleCase(row["Type"].ToString().ToLower()) : null;
            attrb.Minimum = row["Minimum"] != DBNull.Value ? row["Minimum"].ToString() : null;
            attrb.Maximum = row["Maximum"] != DBNull.Value ? row["Maximum"].ToString() : null;
            attrb.Formula = row["Formula"] != DBNull.Value ? row["Formula"].ToString() : null;
            attrb.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
            attrb.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
            attrb.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
            attrb.ModifiedTS = row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : null;
            attrb.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
            attrb.Source = row["Source"] != DBNull.Value ? row["Source"].ToString() : null;

            table.Dispose();
            return attrb;
        }

        public DataTable GetSreAttributeDataSource(int dataSourceId)
        {
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand();
            DataTable dt = new DataTable();
            dt.TableName = "DataSourceAttributes";

            try
            {
                command.CommandText = "select [name] as [Name],position as [Position],aa.IsAcceptable as [IsAcceptable],aa.[AttributePrintValueType], aa.[AttributePrintValueCustom] ";
                command.CommandText += "from IdpeAttribute a ";
                command.CommandText += "inner join IdpeAttributeDataSource aa on a.attributeId = aa.attributeId ";
                command.CommandText += "where aa.dataSourceId = @datasourceId";
                command.AddParameterWithValue("datasourceId", dataSourceId);


                conn.Open();
                command.Connection = conn;
                dt.Load(command.ExecuteReader());

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

            return dt;
        }


        public List<IdpeAttributeDataSource> GetSreAttributeDataSources(int attributeDataSourceId = 0)
        {
            List <IdpeAttributeDataSource> idpeAttributeDataSources = new List<IdpeAttributeDataSource>();
            string commandText = "select [AttributeDataSourceId],[DataSourceId],[AttributeId],[Position],[IsAcceptable],[AttributePrintValueType],[AttributePrintValueCustom],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [IdpeAttributeDataSource]";
            if (attributeDataSourceId > 0)
                commandText += " where [AttributeDataSourceId] = " + attributeDataSourceId;
            


            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return idpeAttributeDataSources;

            foreach (DataRow row in table.Rows)
            {
                IdpeAttributeDataSource idpeAttributeDataSource = new IdpeAttributeDataSource();
                idpeAttributeDataSource.AttributeDataSourceId = (int)row["AttributeDataSourceId"].ToString().ParseInt();
                idpeAttributeDataSource.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                idpeAttributeDataSource.AttributeId = (int)row["AttributeId"].ToString().ParseInt();
                idpeAttributeDataSource.Position = row["Position"] != DBNull.Value ? row["Position"].ToString().ParseInt() : null;
                idpeAttributeDataSource.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
                idpeAttributeDataSource.AttributePrintValueType = row["AttributePrintValueType"] != DBNull.Value ? row["AttributePrintValueType"].ToString().ParseInt() : 0;
                idpeAttributeDataSource.AttributePrintValueCustom = row["AttributePrintValueCustom"] != DBNull.Value ? row["AttributePrintValueCustom"].ToString() : string.Empty;
                idpeAttributeDataSource.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                idpeAttributeDataSource.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                idpeAttributeDataSource.ModifiedTS = row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : null;
                idpeAttributeDataSource.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
                idpeAttributeDataSource.Source = row["Source"] != DBNull.Value ? row["Source"].ToString() : null;
                idpeAttributeDataSources.Add(idpeAttributeDataSource);
            }

            table.Dispose();
            return idpeAttributeDataSources;
        }

        public List<IdpeAttribute> GetAttributes(int dataSourceId)
        {         
            List<IdpeAttribute> attrbs = new List<IdpeAttribute>();
            string commandText = "select a.[AttributeId],a.[Name],a.[Type],a.[Minimum],a.[Maximum],a.[Formula],aes.[IsAcceptable], aes.[Position], aes.[AttributePrintValueType], aes.[AttributePrintValueCustom],a.[CreatedTS],a.[CreatedBy],a.[ModifiedTS],a.[ModifiedBy],a.[Source] " +
                " from  idpeAttribute a Inner join idpeAttributeDataSource aes  on a.AttributeId = aes.AttributeId " +
                         " inner join idpeDataSource es on aes.DataSourceId = es.Id where es.Id =  " + dataSourceId + " order by aes.Position";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return attrbs;

            foreach (DataRow row in table.Rows)
            {
                IdpeAttribute attrb = new IdpeAttribute();
                attrb.AttributeId = (int)row["AttributeId"].ToString().ParseInt();
                attrb.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                attrb.Type = row["Type"] != DBNull.Value ? new CultureInfo("en-US", false).TextInfo.ToTitleCase(row["Type"].ToString().ToLower()) : null;
                attrb.Minimum = row["Minimum"] != DBNull.Value ? row["Minimum"].ToString() : null;
                attrb.Maximum = row["Maximum"] != DBNull.Value ? row["Maximum"].ToString() : null;
                attrb.Formula = row["Formula"] != DBNull.Value ? row["Formula"].ToString() : null;
                attrb.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
                attrb.Position = row["Position"] != DBNull.Value ? row["Position"].ToString().ParseInt() : 0;
                attrb.AttributePrintValueType = row["AttributePrintValueType"] != DBNull.Value ? row["AttributePrintValueType"].ToString().ParseInt() : 0;
                attrb.AttributePrintValueCustom = row["AttributePrintValueCustom"] != DBNull.Value ? row["AttributePrintValueCustom"].ToString() : string.Empty;
                attrb.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                attrb.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                attrb.ModifiedTS = row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : null;
                attrb.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
                attrb.Source = row["Source"] != DBNull.Value ? row["Source"].ToString() : null;
                attrbs.Add(attrb);
            }
            table.Dispose();
            return attrbs;
        }

        public List<IdpeAttribute> GetAttributes(string dataSourceName)
        {

            List<IdpeAttribute> attrbs = new List<IdpeAttribute>();
            string commandText = "select a.[AttributeId],a.[Name],a.[Type],a.[Minimum],a.[Maximum],a.[Formula],a.[IsAcceptable],aes.[AttributePrintValueType], aes.[AttributePrintValueCustom], a.[CreatedTS],a.[CreatedBy],a.[ModifiedTS],a.[ModifiedBy],a.[Source] " + 
                " from  idpeAttribute a Inner join idpeAttributeDataSource aes  on a.AttributeId = aes.AttributeId " +
                         " inner join idpeDataSource es on aes.DataSourceId = es.Id where es.Name =  '" + dataSourceName + "' order by aes.Position";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return attrbs;

            foreach (DataRow row in table.Rows)
            {
                IdpeAttribute attrb = new IdpeAttribute();
                attrb.AttributeId = (int)row["AttributeId"].ToString().ParseInt();
                attrb.Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : null;
                attrb.Type = row["Type"] != DBNull.Value ? new CultureInfo("en-US", false).TextInfo.ToTitleCase(row["Type"].ToString().ToLower()) : null;
                attrb.Minimum = row["Minimum"] != DBNull.Value ? row["Minimum"].ToString() : null;
                attrb.Maximum = row["Maximum"] != DBNull.Value ? row["Maximum"].ToString() : null;
                attrb.Formula = row["Formula"] != DBNull.Value ? row["Formula"].ToString() : null;
                attrb.IsAcceptable = row["IsAcceptable"] != DBNull.Value ? row["IsAcceptable"].ToString().ParseBool() : false;
                attrb.AttributePrintValueType = row["AttributePrintValueType"] != DBNull.Value ? row["AttributePrintValueType"].ToString().ParseInt() : 0;
                attrb.AttributePrintValueCustom = row["AttributePrintValueCustom"] != DBNull.Value ? row["AttributePrintValueCustom"].ToString() : string.Empty;
                attrb.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                attrb.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                attrb.ModifiedTS = row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : null;
                attrb.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
                attrb.Source = row["Source"] != DBNull.Value ? row["Source"].ToString() : null;
                attrbs.Add(attrb);
            }

            table.Dispose();
            return attrbs;

        }
       
        public bool IsAttributeInUse(int attributeId)
        {
            List<IdpeAttributeDataSource> idpeAttributeDataSources = GetSreAttributeDataSources();
            var record = idpeAttributeDataSources.FirstOrDefault(a => a.AttributeId == attributeId);
            return record == null ? false : true;
        }

        public int AttributeExists(string name, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            DataTable table = null;
            string commandText = string.Format("select AttributeId from IdpeAttribute where [Name] = '{0}'", name);
            if (dal == null)
                table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            else
                table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText, dal, connection, transaction);

            if ((table == null)
                ||(table.Rows.Count == 0))
                return 0;

            return int.Parse(table.Rows[0][0].ToString());
        }
      
        public int GetIsValidAttributeId(IDal myDal, IDbConnection conn, IDbTransaction transaction)
        {
            IDbCommand command = myDal.CreateCommand("SELECT AttributeId from IdpeAttribute WHERE [Name] = 'IsValid'", conn);
            command.Transaction = transaction;
            int attributeId = 0;

            IDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                attributeId = int.Parse(reader[0].ToString());
            }
            reader.Close();
            reader.Dispose();
            command.Dispose();

            return attributeId;

        }

        public int Save(IdpeAttribute attribute, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {           
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

            try
            {
                if (attribute.AttributeId == 0)
                    attribute.AttributeId = AttributeExists(attribute.Name, dal, connection, transaction);
                bool inserted = false;
                if (attribute.AttributeId == 0)
                {
                    inserted = true;
                    cmdText = "INSERT INTO [IdpeAttribute] ([Name],[Type],[Minimum],[Maximum],[Formula],[IsAcceptable],[CreatedTS],[CreatedBy],[Source]) VALUES ";
                    cmdText += "(@Name,@Type,@Minimum,@Maximum,@Formula,@IsAcceptable,@CreatedTS,@CreatedBy,@Source)";


                    command.AddParameterWithValue("Name", attribute.Name);
                    command.AddParameterWithValue("Type", attribute.Type.ToString().ToUpper());
                    if (!(string.IsNullOrEmpty(attribute.Minimum)))
                        command.AddParameterWithValue("Minimum", attribute.Minimum);
                    else
                        command.AddParameterWithValue("Minimum", DBNull.Value);

                    if (!(string.IsNullOrEmpty(attribute.Maximum)))
                        command.AddParameterWithValue("Maximum", attribute.Minimum);
                    else
                        command.AddParameterWithValue("Maximum", DBNull.Value);

                    if (!(string.IsNullOrEmpty(attribute.Formula)))
                        command.AddParameterWithValue("Formula", attribute.Formula);
                    else
                        command.AddParameterWithValue("Formula", DBNull.Value);

                    command.AddParameterWithValue("IsAcceptable", attribute.IsAcceptable);                    
                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", "SRE Util");

                }
                else
                {
                    cmdText = "UPDATE [IdpeAttribute] SET [Name] = @Name,[Type] = @Type,[Minimum] = @Minimum,[Maximum] = @Maximum,[Formula] = @Formula ";
                    cmdText += "WHERE [AttributeId] = @AttributeId";


                    command.AddParameterWithValue("Name", attribute.Name);
                    command.AddParameterWithValue("Type", attribute.Type.ToString().ToUpper());
                    if (!(string.IsNullOrEmpty(attribute.Minimum)))
                        command.AddParameterWithValue("Minimum", attribute.Minimum);
                    else
                        command.AddParameterWithValue("Minimum", DBNull.Value);

                    if (!(string.IsNullOrEmpty(attribute.Maximum)))
                        command.AddParameterWithValue("Maximum", attribute.Minimum);
                    else
                        command.AddParameterWithValue("Maximum", DBNull.Value);

                    if (!(string.IsNullOrEmpty(attribute.Formula)))
                        command.AddParameterWithValue("Formula", attribute.Formula);
                    else
                        command.AddParameterWithValue("Formula", DBNull.Value);

                    command.AddParameterWithValue("AttributeId", attribute.AttributeId);
                }


                command.CommandText = cmdText;
                command.ExecuteNonQuery();

                if (inserted)
                {
                    command.CommandText = "SELECT AttributeId from IdpeAttribute where Name = @Name";
                    command.Parameters.Clear();
                    command.AddParameterWithValue("Name", attribute.Name);
                    attribute.AttributeId = (Int32)command.ExecuteScalar();
                }

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
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    transaction.Dispose();
                }
            }
            return attribute.AttributeId;
        }


        public void Save(IdpeAttributeDataSource idpeAttributeDataSource, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            string cmdText = string.Empty;
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
                cmdText = "SELECT [AttributeDataSourceId] FROM IdpeAttributeDataSource WHERE [DataSourceId] = @DataSourceId and [AttributeId] = @AttributeId";
                command.Parameters.Clear();
                command.AddParameterWithValue("DataSourceId", idpeAttributeDataSource.DataSourceId);
                command.AddParameterWithValue("AttributeId", idpeAttributeDataSource.AttributeId);
                command.CommandText = cmdText;
                IDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    cmdText = "UPDATE [IdpeAttributeDataSource] SET [Position] = @Position, [IsAcceptable] = @IsAcceptable, AttributePrintValueType = @AttributePrintValueType, AttributePrintValueCustom = @AttributePrintValueCustom WHERE [AttributeDataSourceId] = @AttributeDataSourceId";
                    command.Parameters.Clear();
                    command.AddParameterWithValue("Position", idpeAttributeDataSource.Position);
                    command.AddParameterWithValue("IsAcceptable", idpeAttributeDataSource.IsAcceptable);
                    command.AddParameterWithValue("AttributeDataSourceId", int.Parse(reader[0].ToString()));
                    if ((idpeAttributeDataSource.AttributePrintValueType == 0)
                        || (idpeAttributeDataSource.AttributePrintValueType == null))
                        command.AddParameterWithValue("AttributePrintValueType", DBNull.Value);
                    else
                        command.AddParameterWithValue("AttributePrintValueType", idpeAttributeDataSource.AttributePrintValueType);

                    if (string.IsNullOrEmpty(idpeAttributeDataSource.AttributePrintValueCustom))
                        command.AddParameterWithValue("AttributePrintValueCustom", DBNull.Value);
                    else
                        command.AddParameterWithValue("AttributePrintValueCustom", idpeAttributeDataSource.AttributePrintValueCustom);

                }
                else
                {
                    cmdText = "INSERT INTO [IdpeAttributeDataSource] ([DataSourceId],[AttributeId],[Position],[IsAcceptable],[AttributePrintValueType],[AttributePrintValueCustom],[CreatedTS],[CreatedBy],[Source]) VALUES ";
                    cmdText += "(@DataSourceId,@AttributeId,@Position,@IsAcceptable,@AttributePrintValueType,@AttributePrintValueCustom,@CreatedTS,@CreatedBy,@Source)";
                    command.Parameters.Clear();
                    command.AddParameterWithValue("DataSourceId", idpeAttributeDataSource.DataSourceId);
                    command.AddParameterWithValue("AttributeId", idpeAttributeDataSource.AttributeId);
                    command.AddParameterWithValue("Position", idpeAttributeDataSource.Position);
                    command.AddParameterWithValue("IsAcceptable", idpeAttributeDataSource.IsAcceptable);
                    if ((idpeAttributeDataSource.AttributePrintValueType == 0)
                        || (idpeAttributeDataSource.AttributePrintValueType == null))
                        command.AddParameterWithValue("AttributePrintValueType", DBNull.Value);
                    else
                        command.AddParameterWithValue("AttributePrintValueType", idpeAttributeDataSource.AttributePrintValueType);

                    if (!string.IsNullOrEmpty(idpeAttributeDataSource.AttributePrintValueCustom))
                        command.AddParameterWithValue("AttributePrintValueCustom", idpeAttributeDataSource.AttributePrintValueCustom);
                    else
                        command.AddParameterWithValue("AttributePrintValueCustom", DBNull.Value);
                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", "SRE Util");

                }
                reader.Close();
                command.CommandText = cmdText;
                command.ExecuteNonQuery();

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
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                    transaction.Dispose();
                }
            }

        }

        //obsolete - dont use
        public bool DisassociateAttributeFromDataSource(int attributeDataSourceId)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);
            conn.Open();

            try
            {
                cmdText = "DELETE FROM [IdpeAttributeDataSource] WHERE [AttributeDataSourceId] = @AttributeDataSourceId";
                command.CommandText = cmdText;                
                command.AddParameterWithValue("AttributeDataSourceId", attributeDataSourceId);
                command.ExecuteNonQuery();

            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return true;

        }

        public bool DisassociateAttributeFromDataSource(int dataSourceId, int attributeId)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);
            conn.Open();

            try
            {
                cmdText = "DELETE FROM [IdpeAttributeDataSource] WHERE [DataSourceId] = @DataSourceId AND [AttributeId] = @AttributeId";
                command.CommandText = cmdText;
                command.AddParameterWithValue("DataSourceId", dataSourceId);
                command.AddParameterWithValue("AttributeId", attributeId);
                command.ExecuteNonQuery();
                
            }          
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return true;

        }

        public bool MakeAttributeAcceptableNew(int dataSourceId, int attributeId, bool isAcceptable)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);
            conn.Open();

            try
            {
                if (isAcceptable)
                    cmdText = "UPDATE [IdpeAttributeDataSource] SET [IsAcceptable] = @IsAcceptable WHERE [DataSourceId] = @DataSourceId and [AttributeId] = @AttributeId";
                else
                    cmdText = "UPDATE [IdpeAttributeDataSource] SET [IsAcceptable] = @IsAcceptable, [Position] = NULL WHERE [DataSourceId] = @DataSourceId and [AttributeId] = @AttributeId";
                

                command.CommandText = cmdText;
                command.AddParameterWithValue("IsAcceptable", isAcceptable);
                command.AddParameterWithValue("DataSourceId", dataSourceId);
                command.AddParameterWithValue("AttributeId", attributeId);
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

            RefreshAttributePositions(dataSourceId);
            return true;
        }

        [ObsoleteAttribute]
        public bool MakeAttributeAcceptable(int dataSourceId, int attributeDataSourceId, bool isAcceptable)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);
            conn.Open();

            try
            {
                if(isAcceptable)
                    cmdText = "UPDATE [IdpeAttributeDataSource] SET [IsAcceptable] = @IsAcceptable WHERE [AttributeDataSourceId] = @AttributeDataSourceId";
                else
                    cmdText = "UPDATE [IdpeAttributeDataSource] SET [IsAcceptable] = @IsAcceptable, [Position] = NULL WHERE [AttributeDataSourceId] = @AttributeDataSourceId";

                command.CommandText = cmdText;
                command.AddParameterWithValue("IsAcceptable", isAcceptable);
                command.AddParameterWithValue("AttributeDataSourceId", attributeDataSourceId);
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

            RefreshAttributePositions(dataSourceId);
            return true;
        }

        public void RefreshAttributePositions(int dataSourceId)
        {
            IDal myDal = null;
            IDbConnection conn = null;
            IDbTransaction transaction = null;
            IDataReader reader = null;
            IDbCommand command = null;

            try
            {
                myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
                conn = myDal.CreateConnection(_ConnectionString);
                conn.Open();
                transaction = myDal.CreateTransaction(conn);
                command = myDal.CreateCommand();
                command.CommandText = "select AttributeDataSourceId, a.[AttributeId],a.[Name],aes.[IsAcceptable],AttributeDataSourceId,Position,es.Name as [DataSourceName] ";
                command.CommandText += "from  idpeAttribute a Inner join idpeAttributeDataSource aes  on a.AttributeId = aes.AttributeId ";
                command.CommandText += "inner join idpeDataSource es on aes.DataSourceId = es.Id where es.Id =  " + dataSourceId + " and aes.IsAcceptable = 1 order by aes.Position";
                command.Connection = conn;
                command.Transaction = transaction;
                reader = command.ExecuteReader();
                List<int> attributeDataSourceIds = new List<int>();
                while (reader.Read())
                {
                    attributeDataSourceIds.Add((int)reader["AttributeDataSourceId"].ToString().ParseInt());
                }
                reader.Close();

                int position = 1;
                foreach(int attributeDataSourceId in attributeDataSourceIds)
                {
                    IDbCommand updateCommand = myDal.CreateCommand();
                    updateCommand.Connection = conn;
                    updateCommand.Transaction = transaction;
                    updateCommand.CommandText = "UPDATE [IdpeAttributeDataSource] SET [Position] = @Position WHERE [AttributeDataSourceId] = @AttributeDataSourceId";
                    updateCommand.AddParameterWithValue("Position", position);
                    updateCommand.AddParameterWithValue("AttributeDataSourceId", attributeDataSourceId);
                    updateCommand.ExecuteNonQuery();

                    position++;
                }
                transaction.Commit();
                conn.Close();
                
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                reader.Dispose();
                command.Dispose();
                conn.Dispose();
            }
        }

        public void DeleteAttribute(int attributeId, bool ignoreException = true)
        {
            string cmdText = string.Empty;
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            IDbCommand command = myDal.CreateCommand(cmdText, conn);
            conn.Open();

            try
            {
                cmdText = "DELETE from [IdpeAttribute] WHERE [AttributeId] = @AttributeId";
                command.CommandText = cmdText;
                command.AddParameterWithValue("AttributeId", attributeId);                
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (!ignoreException)
                {
                    Trace.TraceError(ex.ToString());
                    throw new Exception(ex.Message, ex);
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }            
        }
        
        
    }    
}







