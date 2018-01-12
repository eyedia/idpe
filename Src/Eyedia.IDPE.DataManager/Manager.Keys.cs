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
using System.Data.Linq;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {
        
        public List<SreKey> GetKeys()
        {
            List<SreKey> sreKeys = new List<SreKey>();
            string commandText = "select [KeyId],[Name],[Value],[ValueBinary], [Type],[IsDeployable],[NextKeyId],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [SreKey]";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return sreKeys;

            foreach (DataRow row in table.Rows)
            {                
                sreKeys.Add(RowToSreKey(row));
            }

            return sreKeys;
        }

        public List<SreKey> GetKeys(int dataSourceId = 0)
        {
            string commandText = "select k.[KeyId],[Name],[Value],[ValueBinary],[Type],kds.[IsDeployable],[NextKeyId],k.[CreatedTS],k.[CreatedBy],k.[ModifiedTS],k.[ModifiedBy],k.[Source] ";
            commandText += "from SreKey k ";
            commandText += "inner join SreKeyDataSource kds ON k.KeyId = kds.KeyId";
            if(dataSourceId > 0)
                commandText += " where kds.DataSourceId = " + dataSourceId;

            List<SreKey> sreKeys = new List<SreKey>();
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return sreKeys;

            foreach (DataRow row in table.Rows)
            {
                sreKeys.Add(RowToSreKey(row, dataSourceId));
            }

            return sreKeys;
        }

        public List<SreKey> GetDeployableKeys(int dataSourceId = 0)
        {
            string commandText = "select k.[KeyId],[Name],[Value],[ValueBinary],[Type],kds.[DataSourceId],kds.[IsDeployable],[NextKeyId],k.[CreatedTS],k.[CreatedBy],k.[ModifiedTS],k.[ModifiedBy],k.[Source] ";
            commandText += "from SreKey k ";
            commandText += "inner join SreKeyDataSource kds ON k.KeyId = kds.KeyId";
            commandText += " where kds.IsDeployable = 1";
            if (dataSourceId > 0)
                commandText += " and kds.DataSourceId = " + dataSourceId;

            List<SreKey> sreKeys = new List<SreKey>();
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return sreKeys;

            foreach (DataRow row in table.Rows)
            {                
                sreKeys.Add(RowToSreKey(row, (int)row["DataSourceId"].ToString().ParseInt()));
            }

            return sreKeys;
        }

        public SreKey GetKey(SreKeyTypes keyType)
        {
            string commandText = "select [KeyId],[Name],[Value],[ValueBinary], [Type],[IsDeployable],[NextKeyId],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [SreKey] where [Type] = " + (int)keyType;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return null;

            if (table.Rows.Count == 1)
                return RowToSreKey(table.Rows[0]);

            return null;
        }
       
        public SreKey GetKey(int dataSourceId, string keyName)
        {
            string commandText = "select k.[KeyId],[Name],[Value],[ValueBinary],[Type],kds.[IsDeployable],[NextKeyId],k.[CreatedTS],k.[CreatedBy],k.[ModifiedTS],k.[ModifiedBy],k.[Source] ";
            commandText += "from SreKey k ";
            commandText += "inner join SreKeyDataSource kds ON k.KeyId = kds.KeyId ";
            commandText += "where kds.DataSourceId = " + dataSourceId + " and k.Name = '" + keyName + "'";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if ((table == null)
                || (table.Rows.Count != 1))
            {
                if (dataSourceId != -99)
                    return GetKey(-99, keyName);
                else
                    return null;
            }
            return RowToSreKey(table.Rows[0]);
        }

        public SreKey GetKey(int keyId)
        {
            string commandText = "select [KeyId],[Name],[Value],[ValueBinary],[Type],[IsDeployable],[NextKeyId],[CreatedTS],[CreatedBy],[ModifiedTS],[ModifiedBy],[Source] from [SreKey] where [keyid] = " + keyId;
            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);

            if ((table == null)
                || (table.Rows.Count != 1))
                return null;

            return RowToSreKey(table.Rows[0]);
        }

        private SreKey RowToSreKey(DataRow row, int dataSourceId = 0)
        {
            SreKey key = new SreKey();
            key.KeyId = (int)row["KeyId"].ToString().ParseInt();
            key.Name = row["Name"].ToString();
            key.Value = row["Value"] != DBNull.Value ? row["Value"].ToString() : null;
            key.ValueBinary = row["ValueBinary"] != DBNull.Value ? new Binary((byte[])row["ValueBinary"]) : null;
            key.Type = (int)(row["Type"] != DBNull.Value ? row["Type"].ToString().ParseInt() : 0);
            key.IsDeployable = row["IsDeployable"] != DBNull.Value ? row["IsDeployable"].ToString().ParseBool() : false;
            key.DataSourceId = dataSourceId;
            key.NextKeyId = (int)(row["NextKeyId"] != DBNull.Value ? row["NextKeyId"].ToString().ParseInt() : 0);
            key.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
            key.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
            key.ModifiedTS = (DateTime)(row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : DateTime.MinValue);
            key.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
            key.Source = row["Source"] != DBNull.Value ? row["Source"].ToString() : null;

            return key;
        }

        private List<SreKeyDataSource> GetSreKeyDataSources()
        {
            List<SreKeyDataSource>  sreKeyDataSources = new List<SreKeyDataSource>();
            string commandText = "select [KeyDataSourceId],[KeyId],[DataSourceId],[IsDeployable] from [SreKeyDataSource]";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return sreKeyDataSources;

            foreach (DataRow row in table.Rows)
            {
                SreKeyDataSource sreKeyDataSource = new SreKeyDataSource();
                sreKeyDataSource.KeyDataSourceId = (int)row["KeyDataSourceId"].ToString().ParseInt();
                sreKeyDataSource.KeyId = (int)row["KeyId"].ToString().ParseInt();
                sreKeyDataSource.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                sreKeyDataSource.IsDeployable = row["IsDeployable"].ToString().ParseBool();
                sreKeyDataSources.Add(sreKeyDataSource);
            }
            return sreKeyDataSources;
        }

        public int GetSreKeyId(IDal myDal, IDbConnection conn, IDbTransaction transaction, string keyName)
        {
            int keyId = 0;
            IDbCommand command = myDal.CreateCommand("SELECT KeyId FROM SreKey WHERE Name = @Name", conn);
            command.Transaction = transaction;
            command.AddParameterWithValue("Name", keyName);
            IDataReader reader = command.ExecuteReader();
            if (reader.Read())
                keyId = int.Parse(reader[0].ToString());

            reader.Close();
            reader.Dispose();
            command.Dispose();
            return keyId;
        }


        public int GetSreKeyId(IDal myDal, IDbConnection conn, IDbTransaction transaction, int dataSourceId, string keyName)
        {
            int keyId = 0;
            string commandText = "select k.keyId, [Name], [Value] from srekey k ";
            commandText += "inner join SreKeyDataSource ka on k.keyId = ka.keyId ";
            commandText += "where dataSourceId = @DataSourceId and Name = @Name";

            IDbCommand command = myDal.CreateCommand(commandText, conn);
            command.Transaction = transaction;
            command.AddParameterWithValue("DataSourceId", dataSourceId);
            command.AddParameterWithValue("Name", keyName);
            IDataReader reader = command.ExecuteReader();
            if (reader.Read())
                keyId = int.Parse(reader[0].ToString());

            reader.Close();
            reader.Dispose();
            command.Dispose();
            return keyId;
        }
       
        public List<SreKey> GetApplicationKeys(int dataSourceId, bool includeSystemKeys)
        {            
            List<SreKey> keys = new List<SreKey>();
            if (includeSystemKeys)
            {
                int parentId = GetDataSourceParentId(dataSourceId);
                List<SreKey> syskeys = Cache.Instance.Bag[parentId + ".keys"] as List<SreKey>;
                if ((syskeys == null)
                    || (syskeys.Count == 0))
                {
                    syskeys = GetDataSourceKeys(parentId);
                    if(syskeys.Count > 0)
                        Cache.Instance.Bag.Add(parentId + ".keys", syskeys);
                }
                keys.AddRange(syskeys);
            }

            keys.AddRange(GetDataSourceKeys(dataSourceId));
            return keys;
        }

        public List<SreKey> GetDataSourceKeys(int dataSourceId, bool onlyCustomKeys = false)
        {            
            List<SreKey> keys = new List<SreKey>();
            string commandText = "select k.[KeyId], k.[Name], k.[Value], k.[ValueBinary], k.[Type], k.[NextKeyId], k.[CreatedTS], k.[CreatedBy], k.[ModifiedTS], k.[ModifiedBy], k.[Source], kds.[IsDeployable] ";
            commandText += "from  SreKey k ";
            commandText += "inner join SreKeyDataSource kds on k.KeyId = kds.KeyId ";
            commandText += "where kds.DataSourceId = " + dataSourceId;

            if (onlyCustomKeys)
                commandText += " and k.[Type] = " + (int)SreKeyTypes.Custom;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {                    
                    keys.Add(RowToSreKey(row));
                }
            }
            return keys;
        }

        public List<SreKey> GetDataSourceKeysConnectionStrings(int dataSourceId, bool isSystem)
        {
            List<SreKey> keys = new List<SreKey>();
            string commandText = "select k.[KeyId], k.[Name], k.[Value], k.[ValueBinary], k.[Type], k.[NextKeyId], k.[CreatedTS], k.[CreatedBy], k.[ModifiedTS], k.[ModifiedBy], k.[Source], kds.[IsDeployable] ";
            commandText += "from  SreKey k ";
            commandText += "inner join SreKeyDataSource kds on k.KeyId = kds.KeyId ";
            commandText += "where kds.DataSourceId = " + dataSourceId;            
            commandText += " and k.[Type] in (2,3,4,5)";

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    SreKey key = RowToSreKey(row);

                    if (dataSourceId == -99)
                    {
                        //this is added just to identify global keys
                        SreKeyDataSource kds = new SreKeyDataSource();
                        kds.KeyId = key.KeyId;
                        kds.DataSourceId = -99;
                        key.SreKeyDataSources.Add(kds);
                    }
                    keys.Add(key);
                }
            }

            if ((dataSourceId != -99) && (isSystem == false))
            {
                //adding global
                List<SreKey> gKeys = GetDataSourceKeysConnectionStrings(-99, false);
                foreach (SreKey key in gKeys)
                {
                    if (keys.Where(dKey => dKey.Name == key.Name).SingleOrDefault() == null)
                    {
                        //this is added just to identify global keys
                        SreKeyDataSource kds = new SreKeyDataSource();
                        kds.KeyId = key.KeyId;
                        kds.DataSourceId = -99;
                        key.SreKeyDataSources.Add(kds);
                        keys.Add(key);
                    }
                }
            }
            return keys;
        }      

        public void DeleteKeyFromApplication(int dataSourceId, string key, bool deleteKey)
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
                command.CommandText = "select KeyDataSourceId, ska.KeyId from SreKeyDataSource ska ";
                command.CommandText += "inner join srekey sk on ska.keyId = sk.KeyId ";
                command.CommandText += "where ska.DataSourceId = @DataSourceId and sk.Name = @KeyName";
                command.AddParameterWithValue("DataSourceId", dataSourceId);
                command.AddParameterWithValue("KeyName", key);

                IDataReader reader = command.ExecuteReader();
                int keyId = 0;
                int keyDataSourceId = 0;
                if (reader.Read())
                {
                    keyDataSourceId = int.Parse(reader[0].ToString());
                    keyId = int.Parse(reader[1].ToString());
                }

                reader.Close();
                reader.Dispose();

                if ((keyId == 0) || (keyDataSourceId == 0))
                {
                    transaction.Rollback();
                    return;
                }

                command.Parameters.Clear();
                command.CommandText = "delete from SreKeyDataSource where KeyDataSourceId = @KeyDataSourceId";
                command.AddParameterWithValue("KeyDataSourceId", keyDataSourceId);
                command.ExecuteNonQuery();
                if (deleteKey)
                {
                    command.Parameters.Clear();
                    command.CommandText = "delete from srekey where KeyId = @KeyId";
                    command.AddParameterWithValue("KeyId", keyId);
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

        public int Save(SreKey key, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
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

            int keyId = 0;
            try
            {
                keyId = GetSreKeyId(dal, connection, transaction, key.Name);
                if (keyId == 0)
                {
                    command.CommandText = "INSERT INTO SreKey(Name, Value, ValueBinary, Type, NextKeyId, CreatedTS, CreatedBy, Source) ";
                    command.CommandText += " VALUES (@Name, @Value,  @ValueBinary, @Type, @NextKeyId, @CreatedTS, @CreatedBy, @Source)";
                    command.AddParameterWithValue("Name", key.Name);
                    if (key.ValueBinary != null)
                    {
                        command.AddParameterWithValue("ValueBinary", key.ValueBinary.ToArray());
                        command.AddParameterWithValue("Value", DBNull.Value);
                    }
                    else
                    {
                        command.AddParameterWithValue("ValueBinary", DBNull.Value);
                        if (key.Value != null)
                            command.AddParameterWithValue("Value", key.Value);
                        else
                            command.AddParameterWithValue("Value", DBNull.Value);
                    }

                    command.AddParameterWithValue("Type", key.Type);
                    if ((key.NextKeyId != null) && (key.NextKeyId != 0))
                        command.AddParameterWithValue("NextKeyId", key.NextKeyId);
                    else
                        command.AddParameterWithValue("NextKeyId", DBNull.Value);

                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", GetSource(key.Source));


                    command.ExecuteNonQuery();

                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.Parameters.Clear();
                    command.CommandText = "SELECT max(KeyId) from SreKey";
                    keyId = (Int32)command.ExecuteScalar();

                }
                else
                {
                    command.CommandText = "UPDATE [SreKey] SET [Name] = @Name,[Value] = @Value, [ValueBinary] = @ValueBinary, [Type] = @Type, [ModifiedTS] = @ModifiedTS, [ModifiedBy] = @ModifiedBy,[Source] = @Source, [IsDeployable] = @IsDeployable,[NextKeyId] = @NextKeyId ";
                    command.CommandText += "WHERE [KeyId] = @KeyId";

                    command.AddParameterWithValue("Name", key.Name);
                    if (key.ValueBinary != null)
                    {
                        command.AddParameterWithValue("ValueBinary", key.ValueBinary.ToArray());
                        command.AddParameterWithValue("Value", DBNull.Value);
                    }
                    else
                    {
                        command.AddParameterWithValue("ValueBinary", DBNull.Value);
                        if (key.Value != null)
                            command.AddParameterWithValue("Value", key.Value);
                        else
                            command.AddParameterWithValue("Value", DBNull.Value);
                    }
                    command.AddParameterWithValue("Type", key.Type);
                    command.AddParameterWithValue("IsDeployable", key.IsDeployable == true ? true : false);
                    command.AddParameterWithValue("KeyId", keyId);
                    if ((key.NextKeyId != null) && (key.NextKeyId != 0))
                        command.AddParameterWithValue("NextKeyId", key.NextKeyId);
                    else
                        command.AddParameterWithValue("NextKeyId", DBNull.Value);

                    command.AddParameterWithValue("ModifiedTS", DateTime.Now);
                    command.AddParameterWithValue("ModifiedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", GetSource(key.Source));
                    command.ExecuteNonQuery();


                }
                if (localTransaction)
                    transaction.Commit();
                return keyId;
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
        

        public void Save(SreKey key, int dataSourceId, IDal dal = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (dataSourceId == 0)
                throw new Exception("Data source has not been saved! Save data source first.");

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
                int keyId = GetSreKeyId(dal, connection, transaction, dataSourceId, key.Name);
                if (keyId == 0)
                {
                    command.CommandText = "INSERT INTO SreKey(Name, Value, ValueBinary, Type, NextKeyId, CreatedTS, CreatedBy, Source) ";
                    command.CommandText += " VALUES (@Name, @Value, @ValueBinary, @Type, @NextKeyId, @CreatedTS, @CreatedBy, @Source)";
                    command.AddParameterWithValue("Name", key.Name);
                    command.AddParameterWithValue("Value", key.Value);
                    if (key.ValueBinary != null)
                        command.AddParameterWithValue("ValueBinary", key.ValueBinary.ToArray());
                    else
                        command.AddParameterWithValue("ValueBinary", DBNull.Value);

                    command.AddParameterWithValue("Type", key.Type);
                    if ((key.NextKeyId != null) && (key.NextKeyId != 0))
                        command.AddParameterWithValue("NextKeyId", key.NextKeyId);
                    else
                        command.AddParameterWithValue("NextKeyId", DBNull.Value);

                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", GetSource(key.Source));
                    command.ExecuteNonQuery();

                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.Parameters.Clear();
                    command.CommandText = "SELECT max(KeyId) from SreKey";
                    int newKeyId = (Int32)command.ExecuteScalar();

                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO SreKeyDataSource(KeyId, DataSourceId, IsDeployable, CreatedTS, CreatedBy, Source) ";
                    command.CommandText += " VALUES (@KeyId, @DataSourceId, @IsDeployable, @CreatedTS, @CreatedBy, @Source)";
                    command.AddParameterWithValue("KeyId", newKeyId);
                    command.AddParameterWithValue("DataSourceId", dataSourceId);
                    command.AddParameterWithValue("IsDeployable", key.IsDeployable == true ? true : false);
                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("CreatedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", GetSource(key.Source));

                    command.ExecuteNonQuery();

                }
                else
                {
                    command.CommandText = "UPDATE [SreKey] SET [Name] = @Name, [Value] = @Value, [ValueBinary] = @ValueBinary, [Type] = @Type, [ModifiedTS] = @ModifiedTS, [ModifiedBy] = @ModifiedBy, [Source] = @Source, [IsDeployable] = @IsDeployable,[NextKeyId] = @NextKeyId ";
                    command.CommandText += "WHERE [KeyId] = @KeyId";

                    command.AddParameterWithValue("Name", key.Name);
                    command.AddParameterWithValue("Value", key.Value);
                    if (key.ValueBinary != null)
                        command.AddParameterWithValue("ValueBinary", key.ValueBinary.ToArray());
                    else
                        command.AddParameterWithValue("ValueBinary", DBNull.Value);
                    command.AddParameterWithValue("Type", key.Type);
                    command.AddParameterWithValue("IsDeployable", key.IsDeployable == true ? true : false);
                    if ((key.NextKeyId != null) && (key.NextKeyId != 0))
                        command.AddParameterWithValue("NextKeyId", key.NextKeyId);
                    else
                        command.AddParameterWithValue("NextKeyId", DBNull.Value);

                    command.AddParameterWithValue("ModifiedTS", DateTime.Now);
                    command.AddParameterWithValue("ModifiedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", GetSource(key.Source));
                    command.AddParameterWithValue("KeyId", keyId);
                    command.ExecuteNonQuery();


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
                    if (connection.State == ConnectionState.Open) connection.Close();
                    connection.Dispose();
                    transaction.Dispose();
                }
            }
        }

        public void DeleteKey(int keyId)
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
                command.CommandText = "DELETE FROM [SreKey] WHERE [KeyId] = @KeyId";
                command.AddParameterWithValue("KeyId", keyId);
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

        public void DeleteKey(string key)
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
                command.CommandText = "DELETE FROM [SreKey] WHERE [Name] = @Name";
                command.AddParameterWithValue("Name", key);
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

        

        public void SetIsDeployable(int dataSourceId, List<SreKey> keys)
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
                foreach (SreKey key in keys)
                {
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE SreKeyDataSource SET IsDeployable = @IsDeployable, ModifiedTS = @ModifiedTS, ModifiedBy = @ModifiedBy, Source = @Source WHERE KeyId = @KeyId AND DataSourceId = @DataSourceId";
                    command.AddParameterWithValue("KeyId", key.KeyId);
                    command.AddParameterWithValue("DataSourceId", dataSourceId);
                    command.AddParameterWithValue("IsDeployable", key.IsDeployable == true ? true : false);
                    command.AddParameterWithValue("ModifiedTS", DateTime.Now);
                    command.AddParameterWithValue("ModifiedBy", Information.LoggedInUser == null ? "Debugger" : Information.LoggedInUser.UserName);
                    command.AddParameterWithValue("Source", GetSource(key.Source));
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
    }
}


