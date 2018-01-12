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
using System.IO;
using System.Data.SqlServerCe;

namespace Eyedia.IDPE.Command
{

    public static class SqlCe
    {
        public static void Ensure40(string fileName, string password = null)
        {
            string connectionString = GetConnectionString(fileName, password);
            var engine = new SqlCeEngine(connectionString);
            SQLCEVersion fileversion = DetermineVersion(fileName);
            if (fileversion == SQLCEVersion.SQLCE20)
                throw new ApplicationException("Unable to upgrade from 2.0 to 4.0");
            if (SQLCEVersion.SQLCE40 > fileversion)
            {
                engine.Upgrade();
            }
        }

        public static void ExecuteDDL(string statementFileName, string sqlCeFileName, string sqlCeFilePassword = null)
        {
            string connectionString = GetConnectionString(sqlCeFileName, sqlCeFilePassword);

            SqlCeConnection connection = null;
            SqlCeTransaction transaction = null;
            string statement = string.Empty;

            try
            {
                connection = new SqlCeConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
                SqlCeCommand alterTable = new SqlCeCommand();
                alterTable.Connection = connection;
                alterTable.Transaction = transaction;

                using (StreamReader sr = new StreamReader(statementFileName))
                {                    
                    while ((statement = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(statement))
                            continue;
                        Console.WriteLine("Executing '{0}'", statement);
                        alterTable.CommandText = statement;
                        alterTable.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("An error occurred!" + Environment.NewLine);
                Console.Write(ex.ToString());
                Console.WriteLine("Transaction rollbacked!");
            }
            finally
            {
                if ((connection != null)
                    && (connection.State == System.Data.ConnectionState.Open))
                    connection.Close();
            }


        }

        static string GetConnectionString(string fileName, string password = null)
        {
            string connectionString = string.Empty;
            if (password != null)
                connectionString = string.Format("Data Source={0};password={1}", fileName, password);
            else
                connectionString = string.Format("Data Source={0}", fileName);

            return connectionString;
        }

        private enum SQLCEVersion
        {
            SQLCE20 = 0,
            SQLCE30 = 1,
            SQLCE35 = 2,
            SQLCE40 = 3
        }

        private static SQLCEVersion DetermineVersion(string filename)
        {
            var versionDictionary = new Dictionary<int, SQLCEVersion> 
                { 
                { 0x73616261, SQLCEVersion.SQLCE20 }, 
                { 0x002dd714, SQLCEVersion.SQLCE30 }, 
                { 0x00357b9d, SQLCEVersion.SQLCE35 }, 
                { 0x003d0900, SQLCEVersion.SQLCE40 }        
                };
            int versionLONGWORD = 0;
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                fs.Seek(16, SeekOrigin.Begin);
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    versionLONGWORD = reader.ReadInt32();
                }
            }

            if (versionDictionary.ContainsKey(versionLONGWORD))
            {
                return versionDictionary[versionLONGWORD];
            }
            else
            {
                throw new ApplicationException("Unable to determine database file version");
            }
        }
    }


}


