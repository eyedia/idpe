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
        public List<SymplusAVPair> GetAVPairs()
        {
            List<SymplusAVPair> avPairs = new List<SymplusAVPair>();
            string commandText = "select [Id],[Key],[Value],[CreatedBy],[CreatedTS],[ModifiedBy],[ModifiedTS],[Source] from [SymplusAVPair]";
            DataTable table = ExecuteCommand(commandText);
            if (table == null)
                return avPairs;

            foreach (DataRow row in table.Rows)
            {
                SymplusAVPair avPair = new SymplusAVPair();
                avPair.Id = (int)row["Id"].ToString().ParseInt();
                avPair.Key = row["Key"] != DBNull.Value ? row["Key"].ToString() : null;
                avPair.Value = row["Value"] != DBNull.Value ? row["Value"].ToString() : null;
                avPair.CreatedBy = row["CreatedBy"] != DBNull.Value ? row["CreatedBy"].ToString() : null;
                avPair.CreatedTS = (DateTime)(row["CreatedTS"] != DBNull.Value ? row["CreatedTS"].ToString().ParseDateTime() : DateTime.MinValue);
                avPair.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? row["ModifiedBy"].ToString() : null;
                avPair.ModifiedTS = row["ModifiedTS"] != DBNull.Value ? row["ModifiedTS"].ToString().ParseDateTime() : null;

                avPairs.Add(avPair);
            }

            table.Dispose();
            return avPairs;
        }
       

        public SymplusAVPair GetAVPair(string key)
        {
            return this.SymplusAVPairs.Where(av => av.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
        }

        public string GetAVPairValue(string key)
        {
            SymplusAVPair avPair= this.SymplusAVPairs.Where(av => av.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            return avPair == null ? string.Empty : avPair.Value;
        }


        public void Save(SymplusAVPair avPair)
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
                if (avPair.Id == 0)
                {
                    command.CommandText = "INSERT INTO [SymplusAVPair] ([Key], [Value], [CreatedBy], [CreatedTS], [Source]) ";
                    command.CommandText += " VALUES (@Key, @Value, @CreatedBy, @CreatedTS, @Source)";
                    command.AddParameterWithValue("Key", avPair.Key);
                    command.AddParameterWithValue("Value", avPair.Value);
                    command.AddParameterWithValue("CreatedBy", avPair.CreatedBy);
                    command.AddParameterWithValue("CreatedTS", DateTime.Now);
                    command.AddParameterWithValue("Source", GetSource(avPair.Source));
                    command.ExecuteNonQuery();

                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.Parameters.Clear();
                    command.CommandText = "SELECT max(Id) from [SymplusAVPair]";
                    int newCodeSetId = (Int32)command.ExecuteScalar();

                }
                else
                {
                    command.CommandText = "UPDATE [SymplusAVPair] SET [Key] = @Key, [Value] = @Value, ModifiedBy = @ModifiedBy, ModifiedTS = @ModifiedTS, Source = @Source";
                    command.CommandText += "WHERE [Id] = @Id";

                    command.AddParameterWithValue("Key", avPair.Key);
                    command.AddParameterWithValue("Value", avPair.Value);
                    command.AddParameterWithValue("ModifiedBy", avPair.ModifiedBy);
                    command.AddParameterWithValue("ModifiedTS", DateTime.Now);
                    command.AddParameterWithValue("Source", GetSource(avPair.Source));
                    command.AddParameterWithValue("Id", avPair.Id);

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


