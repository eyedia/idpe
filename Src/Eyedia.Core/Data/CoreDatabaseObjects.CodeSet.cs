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
        public List<CodeSet> GetCodeSets()
        {
            List<CodeSet> codeSets = new List<CodeSet>();
            string commandText = "select [CodeSetId],[Code],[EnumCode],[Value],[ReferenceKey],[Description],[Position] from [CodeSet]";
            DataTable table = ExecuteCommand(commandText);
            if (table == null)
                return codeSets;

            foreach (DataRow row in table.Rows)
            {
                CodeSet codeSet = new CodeSet();
                codeSet.CodeSetId = (int)row["CodeSetId"].ToString().ParseInt();
                codeSet.Code = row["Code"].ToString();
                codeSet.EnumCode = (int)row["EnumCode"].ToString().ParseInt();
                codeSet.Value = row["Value"].ToString();
                codeSet.ReferenceKey = row["ReferenceKey"].ToString();
                codeSet.Description = row["Description"].ToString();
                codeSet.Position = row["Position"].ToString().ParseInt();

                codeSets.Add(codeSet);
            }

            table.Dispose();
            return codeSets;
        }

        public int GetCodeSetId(IDal myDal, IDbConnection conn, string code, int enumCode, IDbTransaction transaction = null)
        {
            int codesetId = 0;
            IDbCommand command = myDal.CreateCommand("SELECT CodeSetId FROM CodeSet WHERE Code = @Code and EnumCode = @EnumCode", conn);
            if (transaction != null)
                command.Transaction = transaction;

            command.AddParameterWithValue("Code", code);
            command.AddParameterWithValue("EnumCode", enumCode);
            IDataReader reader = command.ExecuteReader();
            if (reader.Read())
                codesetId = int.Parse(reader[0].ToString());

            reader.Close();
            reader.Dispose();
            command.Dispose();
            return codesetId;
        }

        public void Save(CodeSet codeset)
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
                int CodeSetId = GetCodeSetId(myDal, conn, codeset.Code, codeset.EnumCode, transaction);
                if (CodeSetId == 0)
                {
                    command.CommandText = "INSERT INTO CodeSet(Code, EnumCode, Value, ReferenceKey, Description, Position) ";
                    command.CommandText += " VALUES (@Code, @EnumCode, @Value, @ReferenceKey, @Description, @Position)";
                    command.AddParameterWithValue("Code", codeset.Code);
                    command.AddParameterWithValue("EnumCode", codeset.EnumCode);
                    command.AddParameterWithValue("Value", codeset.Value);
                    command.AddParameterWithValue("ReferenceKey", codeset.ReferenceKey);
                    command.AddParameterWithValue("Description", codeset.Description);
                    command.AddParameterWithValue("Position", codeset.Position);
                    command.ExecuteNonQuery();

                    //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                    command.Parameters.Clear();
                    command.CommandText = "SELECT max(CodeSetId) from CodeSet";
                    int newCodeSetId = (Int32)command.ExecuteScalar();

                }
                else
                {
                    command.CommandText = "UPDATE [CodeSet] SET [EnumCode] = @EnumCode,[Value] = @Value,[ReferenceKey] = @ReferenceKey, [Description] = @Description,[Position] = @Position ";
                    command.CommandText += "WHERE [CodeSetId] = @CodeSetId";

                    command.AddParameterWithValue("EnumCode", codeset.EnumCode);
                    command.AddParameterWithValue("Value", codeset.Value);
                    command.AddParameterWithValue("ReferenceKey", codeset.ReferenceKey);
                    command.AddParameterWithValue("Description", codeset.Description);
                    command.AddParameterWithValue("Position", codeset.Position);
                    command.AddParameterWithValue("CodeSetId", CodeSetId);
                    command.ExecuteNonQuery();


                }

                transaction.Commit();
                Refresh();
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

        public void Delete(string code)
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
                command.CommandText = "DELETE from [CodeSet] WHERE [Code] = @Code";
                command.AddParameterWithValue("Code", code);                
                command.ExecuteNonQuery();
                transaction.Commit();
                Refresh();
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


