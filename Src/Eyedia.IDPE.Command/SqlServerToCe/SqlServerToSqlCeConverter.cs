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
using System.IO;
using System.Data;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Eyedia.Core;
using System.Diagnostics;
using System.Data.SqlServerCe;
using Eyedia.Core.Data;

namespace Eyedia.IDPE.Command
{
    public class SqlServerToSqlCeConverter
    {
        public SqlServerToSqlCeConverter(SqlServerToSqlCeConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public SqlServerToSqlCeConverter(string configurationFileName)
        {
            StreamReader sr = new StreamReader(configurationFileName);
            string xml = sr.ReadToEnd();
            sr.Close();
            SqlServerToSqlCeConfiguration config = xml.Deserialize<SqlServerToSqlCeConfiguration>();
            File.Delete(config.SqlCeFileName);
            this.Configuration = config;
        }

        SqlServerToSqlCeConfiguration Configuration { get; set; }

        private bool ValidateDestination()
        {
            if ((Configuration.SqlCeOverwrite == false)
                && (File.Exists(Configuration.SqlCeFileName)))
            {
                Console.WriteLine("A file with that name already exist! Are you sure you want to overwrite this file?");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Y)
                {
                    File.Delete(Configuration.SqlCeFileName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                File.Delete(Configuration.SqlCeFileName);
            }
            return true;
        }

        public void Convert()
        {
            if (ValidateDestination() == false)
                return;

            Server SourceServer = null;
            if (Configuration.SqlServerIntegratedSecurity)
            {
                SourceServer = new Server(Configuration.SqlServerName);
            }
            else
            {
                ServerConnection svrConn = new ServerConnection(Configuration.SqlServerName);
                svrConn.LoginSecure = false;
                svrConn.Login = Configuration.SqlServerUserName;
                svrConn.Password = Configuration.SqlServerPassword;
                SourceServer = new Server(svrConn);
            }

            Database sourceDb = SourceServer.Databases[Configuration.SqlServerDatabaseName];
            if(sourceDb == null)
            {
                Console.WriteLine("Source db '{0}' not found in '{1}'",
                    Configuration.SqlServerDatabaseName,
                    Configuration.SqlServerName);
                return;
            }
            List<string> schemaNames = new List<string>();
            foreach (Schema schema in sourceDb.Schemas)
            {
                if (schema.Name.Substring(0, 3) != "db_")
                    schemaNames.Add(schema.Name);
            }

            List<string> tableNames = new List<string>();
            foreach (Table tbl in sourceDb.Tables)
            {
                if (!tbl.IsSystemObject)
                    tableNames.Add(tbl.Name);
            }

            string sqlCeConnectionString = string.Empty;
            if (string.IsNullOrEmpty(Configuration.SqlCePassword))
            {
                sqlCeConnectionString = string.Format("Data Source='{0}';Encrypt={1};SSCE:Max Database Size=4091;",
                    Configuration.SqlCeFileName, Configuration.SqlCeIsEncrypted.ToString().ToUpper());
            }
            {
                sqlCeConnectionString = string.Format("Data Source='{0}';Password={1};Encrypt={2};SSCE:Max Database Size=4091;",
                    Configuration.SqlCeFileName, Configuration.SqlCePassword, Configuration.SqlCeIsEncrypted.ToString().ToUpper());
            }

            bool copiedFailed = false;
            sqlCeConnectionString = sqlCeConnectionString.Replace("LCID=sre;", "");
            System.Data.SqlServerCe.SqlCeEngine eng = new System.Data.SqlServerCe.SqlCeEngine(sqlCeConnectionString);
            object engine = eng;
            Type type = engine.GetType();
            Assembly asm = Assembly.GetAssembly(typeof(System.Data.SqlServerCe.SqlCeEngine));

            Console.WriteLine("Sql Ce Version:" + asm.GetName().Version.ToString());
            //Create the database. 
            MethodInfo mi = type.GetMethod("CreateDatabase");
            Console.WriteLine("Creating the SQL Server Compact Edition Database...");
            try
            {
                mi.Invoke(engine, null);
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine("You do not have permissions to save the file to " + Configuration.SqlCeFileName + ". Please select a different destination path and try again.");
                return;
            }
            Console.WriteLine("Connecting to the SQL Server Compact Edition Database...");
            Type connType = asm.GetType("System.Data.SqlServerCe.SqlCeConnection");
            System.Data.IDbConnection conn = (System.Data.IDbConnection)Activator.CreateInstance(connType);
            conn.ConnectionString = sqlCeConnectionString;
            conn.Open();

            //create all the tables                
            int tblCount = 0;

            Type cmdType = asm.GetType("System.Data.SqlServerCe.SqlCeCommand");
            System.Data.IDbCommand cmd = (System.Data.IDbCommand)Activator.CreateInstance(cmdType);
            foreach (string tblName in tableNames)
            {
                Table tbl = sourceDb.Tables[tblName, Configuration.SqlServerSchemaName];
                if (tbl == null)
                {
                    Console.WriteLine("Table '" + tblName + "' was not found in the selected schema.");
                    copiedFailed = true;
                    break;
                }
                if (tbl.IsSystemObject)
                    continue;

                //if (tbl.Name == "SreVersion")
                //    Debugger.Break();

                Console.WriteLine("Scripting table: " + tbl.Name);
                StringBuilder sb = new StringBuilder();
                sb.Append("CREATE TABLE [").Append(tbl.Name).Append("](");
                int colIdx = 0;
                List<string> pKeys = new List<string>();
                foreach (Column col in tbl.Columns)
                {
                    if (colIdx > 0)
                        sb.Append(", ");
                    //if (col.Name == "Data")
                    //    Debugger.Break();
                    sb.Append("[").Append(col.Name).Append("]").Append(" ");
                    int max = 0;
                    switch (col.DataType.SqlDataType)
                    {
                        case SqlDataType.VarChar:
                            max = col.DataType.MaximumLength;
                            col.DataType = new DataType(SqlDataType.NVarChar);
                            col.DataType.MaximumLength = max;
                            break;

                        case SqlDataType.Char:
                            max = col.DataType.MaximumLength;
                            col.DataType = new DataType(SqlDataType.NChar);
                            col.DataType.MaximumLength = max;
                            break;

                        case SqlDataType.Text:  
                        case SqlDataType.VarCharMax:                        
                            col.DataType = new DataType(SqlDataType.NText);
                            break;

                        case SqlDataType.VarBinaryMax:
                            col.DataType = new DataType(SqlDataType.Image);
                            break;

                        case SqlDataType.Decimal:
                            int scale = col.DataType.NumericScale;
                            int precision = col.DataType.NumericPrecision;
                            col.DataType = new DataType(SqlDataType.Numeric);
                            col.DataType.NumericPrecision = precision;
                            col.DataType.NumericScale = scale;
                            break;


                    }

                    sb.Append(col.DataType.SqlDataType.ToString());

                    SqlDataType datatype = col.DataType.SqlDataType;
                    if (datatype == SqlDataType.NVarChar || datatype == SqlDataType.NChar)
                        sb.Append(" (").Append(col.DataType.MaximumLength.ToString()).Append(") ");
                    else if (datatype == SqlDataType.Numeric)
                        sb.Append(" (").Append(col.DataType.NumericPrecision).Append(",").Append(col.DataType.NumericScale).Append(")");


                    if (col.InPrimaryKey)
                        pKeys.Add(col.Name);

                    //if (col.InPrimaryKey)
                    //    sb.Append(" CONSTRAINT PK").Append(col.Name);

                    if (!col.Nullable)
                        sb.Append(" NOT NULL");

                    if (col.DefaultConstraint != null && !String.IsNullOrEmpty(col.DefaultConstraint.Text))
                    {
                        string def = col.DefaultConstraint.Text.Replace("((", "(").Replace("))", ")");

                        sb.Append(" DEFAULT ").Append(col.DefaultConstraint.Text);
                        //sb.Append(" DEFAULT (1) ");
                    }

                    if (col.Identity)
                    {
                        sb.Append(" IDENTITY (").Append(col.IdentitySeed.ToString()).Append(",").Append(col.IdentityIncrement.ToString()).Append(")");
                    }

                    //if (col.InPrimaryKey)
                    //    sb.Append(" PRIMARY KEY");

                    colIdx++;

                }
                sb.Append(")");


                cmd.CommandText = sb.ToString();
                cmd.CommandText = cmd.CommandText.Replace("suser_sname()", "'Manual User'");
                cmd.Connection = conn;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Create table failed! " + ex.Message);
                    copiedFailed = true;
                    break;
                }

                //add the PK constraints
                if (pKeys.Count > 0)
                {
                    sb = new StringBuilder();
                    sb.Append("ALTER TABLE [").Append(tbl.Name).Append("] ADD CONSTRAINT PK_");
                    //create the constraint name
                    for (int k = 0; k < pKeys.Count; k++)
                    {
                        if (k > 0)
                            sb.Append("_");

                        sb.Append(pKeys[k]);
                    }

                    sb.Append(" PRIMARY KEY(");
                    //add the constraint fields
                    for (int k = 0; k < pKeys.Count; k++)
                    {
                        if (k > 0)
                            sb.Append(", ");

                        sb.Append(pKeys[k]);
                    }
                    sb.Append(")");

                    cmd.CommandText = sb.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Create table failed! Failed creating the Primary Key(s).");
                        copiedFailed = true;
                        break;
                    }
                }

                //copy the indexes
                Console.WriteLine("Scripting the indexes for table: " + tbl.Name);
                foreach (Index idx in tbl.Indexes)
                {
                    if (idx.IndexKeyType == IndexKeyType.DriPrimaryKey)
                        continue;

                    sb = new StringBuilder();
                    sb.Append("CREATE");
                    if (idx.IsUnique)
                        sb.Append(" UNIQUE");

                    //if (!idx.IsClustered)
                    //    sb.Append(" CLUSTERED");
                    //else
                    //    sb.Append(" NONCLUSTERED");

                    sb.Append(" INDEX ").Append(idx.Name).Append(" ON [").Append(tbl.Name).Append("](");
                    for (int i = 0; i < idx.IndexedColumns.Count; i++)
                    {
                        if (i > 0)
                            sb.Append(", ");

                        sb.Append("[" + idx.IndexedColumns[i].Name + "]");
                    }
                    sb.Append(")");

                    cmd.CommandText = sb.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Create table failed! Failed creating the indexes." + ex.Message);
                        copiedFailed = true;
                        break;
                    }

                }
                tblCount++;
            }

            if (!copiedFailed)
            {
                //Now copy the data
                bool copyData = true;
                if (copyData)
                {
                    Console.WriteLine("Copying database data.");

                    foreach (string tblName in tableNames)
                    {
                        Table tbl = sourceDb.Tables[tblName];
                        if (tbl.IsSystemObject)
                            continue;

                        Console.WriteLine("Copying " + tbl.RowCount.ToString() + " rows from " + tbl.Name);
                        bool hasIdentity = false;
                        string alterSql = "ALTER TABLE [{0}] ALTER COLUMN [{1}] IDENTITY({2},{3})";
                        string IDColName = "";
                        long increment = 1;
                        //If the table has an Identity column then we need to re-set the seed and increment
                        //This is a hack since SQL Server Compact Edition does not support SET IDENTITY_INSERT <columnname> ON
                        foreach (Column col in tbl.Columns)
                        {
                            if (col.Identity)
                            {
                                hasIdentity = true;
                                IDColName = col.Name;
                                alterSql = String.Format(alterSql, tbl.Name, col.Name, "{0}", "{1}");
                            }
                        }


                        //Select SQL
                        string sql = "SELECT * FROM [{0}]";

                        //Insert Sql
                        string insertSql = "INSERT INTO [{0}] ({1}) VALUES ({2})";
                        StringBuilder sbColums = new StringBuilder();
                        StringBuilder sbValues = new StringBuilder();
                        int idx1 = 0;
                        foreach (Column col in tbl.Columns)
                        {
                            if (col.Name != IDColName)
                            {
                                if (idx1 > 0)
                                {
                                    sbColums.Append(",");
                                    sbValues.Append(",");
                                }

                                sbColums.Append("[").Append(col.Name).Append("]");
                                sbValues.Append("?");
                                idx1++;
                            }
                        }
                        //if (tbl.Name.Contains("SreVersion"))
                        //    Debugger.Break();

                        insertSql = String.Format(insertSql, tbl.Name, sbColums.ToString(), sbValues.ToString());

                        sql = String.Format(sql, tbl.Name);
                        DataSet ds = sourceDb.ExecuteWithResults(sql);
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int rowCnt = 0;
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    rowCnt++;

                                    if (hasIdentity)
                                    {
                                        long seed = long.Parse(row[IDColName].ToString());
                                        //seed--;
                                        string alterTableForIDColumn = String.Format(alterSql, seed.ToString(), increment.ToString());
                                        cmd.CommandText = alterTableForIDColumn;
                                        try
                                        {
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Failed altering the Table for IDENTITY insert.");
                                            copiedFailed = true;
                                            break;
                                        }
                                    }

                                    sbValues = new StringBuilder();
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = insertSql;
                                    for (int i = 0; i < tbl.Columns.Count; i++)
                                    {
                                        if (tbl.Columns[i].Name != IDColName)
                                        {
                                            //if (tbl.Columns[i].Name == "Data")
                                            //    Debugger.Break();

                                            Type type1 = asm.GetType("System.Data.SqlServerCe.SqlCeParameter");
                                            object[] objArray1 = new object[2];
                                            objArray1[0] = tbl.Columns[i].Name;
                                            objArray1[1] = row[tbl.Columns[i].Name];

                                            object p = Activator.CreateInstance(type1, objArray1);
                                            cmd.Parameters.Add(p);

                                        }
                                    }
                                    cmd.CommandText = String.Format(insertSql, sbValues.ToString());
                                    try
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Copy table data failed!");
                                        copiedFailed = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //Now add the FK relationships
                    if (!copiedFailed)
                    {
                        Console.Write("Adding ForeignKeys.");
                        string fkSql = "ALTER TABLE [{0}] ADD CONSTRAINT [{1}] FOREIGN KEY([{2}]) REFERENCES [{3}] ([{4}])";
                        foreach (string tblName in tableNames)
                        {
                            Table tbl = sourceDb.Tables[tblName];
                            if (tbl.IsSystemObject)
                                continue;

                            int fkCnt = tbl.ForeignKeys.Count;
                            int fxIdx = 0;
                            foreach (ForeignKey fk in tbl.ForeignKeys)
                            {
                                if (!tableNames.Contains(fk.ReferencedTable))
                                    continue;

                                fxIdx++;
                                Console.WriteLine(tbl.Name + ": " + fk.Name);
                                string createFKSql = String.Format(fkSql, tbl.Name, fk.Name, "{0}", fk.ReferencedTable, sourceDb.Tables[fk.ReferencedTable].Indexes[fk.ReferencedKey].IndexedColumns[0].Name);
                                StringBuilder sbFk = new StringBuilder();
                                foreach (ForeignKeyColumn col in fk.Columns)
                                {
                                    if (sbFk.Length > 0)
                                        sbFk.Append(",");

                                    sbFk.Append(col.Name);
                                }
                                createFKSql = String.Format(createFKSql, sbFk.ToString());
                                cmd.CommandText = createFKSql;
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Creating ForeignKeys failed!");
                                    //copiedFailed = true;
                                    //break;
                                }
                            }

                        }

                    }

                    Console.WriteLine("Closing the connection to the SQL Server Compact Edition Database...");                    
                    conn.Close();
                    conn.Dispose();

                    if (!copiedFailed)
                    {
                        Console.WriteLine("Completed!");
                    }
                    else
                    {
                        Console.WriteLine("Copy failed!");
                    }
                }
                else
                {
                    Console.WriteLine("Finished!");
                }
            }
            else
            {
                Console.WriteLine("Copy failed!");
            }
        }
    }    
}
