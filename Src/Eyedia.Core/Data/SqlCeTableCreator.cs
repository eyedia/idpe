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
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Core.Data
{
    public class SqlCeTableCreator
    {
        /// <summary>
        /// Gets the correct SqlDBType for a given .NET type. Useful for working with SQL CE.
        /// </summary>
        /// <param name="type">The .Net Type used to find the SqlDBType.</param>
        /// <returns>The correct SqlDbType for the .Net type passed in.</returns>
        private static SqlDbType GetSqlDBTypeFromType(Type type)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(DbType));
                if (!type.IsArray)
                {
                    SqlCeParameter param = new SqlCeParameter();
                    param.DbType = (DbType)tc.ConvertFrom(type.Name);
                    return param.SqlDbType;
                }
                else
                {
                    return SqlDbType.Binary;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Warning: Couldn't find SqlDbType of '{0}' because of '{1}'. The column was set to default NVarChar."
                    , type.ToString(), ex.Message);
                return SqlDbType.NVarChar;
            }
        }

        private static string GetSpecificColumn(string specificColumnTypes, int columnIndex)
        {
            string[] specificColumns = specificColumnTypes.Split("|".ToCharArray());          
            foreach (string specificColumn in specificColumns)
            {
                string[] info = specificColumn.Split(":".ToCharArray());
                if (info.Length != 2)
                    break;
                if (info[0] == columnIndex.ToString())
                {
                    return info[1];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// The method gets the SQL CE type name for use in SQL Statements such as CREATE TABLE
        /// </summary>
        /// <param name="dbType">The SqlDbType to get the type name for</param>
        /// <param name="size">The size where applicable e.g. to create a nchar(n) type where n is the size passed in.</param>
        /// <returns>The SQL CE compatible type for use in SQL Statements</returns>
        private static string GetSqlServerCETypeName(string columnName, SqlDbType dbType, int size)
        {
            try
            {
                // Conversions according to: http://msdn.microsoft.com/en-us/library/ms173018.aspx
                bool max = (size == int.MaxValue) ? true : false;
                bool over4k = (size > 4000) ? true : false;

                if (size > 0)
                {
                    return string.Format(Enum.GetName(typeof(SqlDbType), dbType) + " ({0})", size);
                }
                else
                {
                    return Enum.GetName(typeof(SqlDbType), dbType);
                }

            }
            catch(Exception ex)
            {
                Trace.TraceInformation("Warning: Couldn't find SqlDbType of '{0}-{1}' because of '{2}'. The column was set to default NVarChar."
                    , columnName, dbType, ex.Message);
                return SqlDbType.NVarChar + " (4000)";
            }
        }

        /// <summary>
        /// Genenerates a SQL CE compatible CREATE TABLE statement based on a schema obtained from
        /// a SqlDataReader or a SqlCeDataReader.
        /// </summary>
        /// <param name="table">The existing table.</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="specificColumnTypes">Specific column types with</param>
        /// <returns>The CREATE TABLE... Statement for the given schema.</returns>
        public static string GetCreateTableStatement(DataTable table, string tableName = null, string specificColumnTypes = null)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = table.TableName;

            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("CREATE TABLE [{0}] (", tableName));
            int columnIndex = 0;

            foreach (DataColumn col in table.Columns)
            {
                SqlDbType dbType = GetSqlDBTypeFromType(col.DataType);
                int size = col.MaxLength;
                if ((dbType == SqlDbType.NVarChar) && (size == -1))
                    size = 4000;

                builder.Append("[");
                builder.Append(col.ColumnName);
                builder.Append("]");
                builder.Append(" ");
                string specificColumnType = GetSpecificColumn(specificColumnTypes, columnIndex);
                if (!string.IsNullOrEmpty(specificColumnType))
                {
                    //specific
                    builder.Append(specificColumnType);
                    builder.Append(", ");
                }
                else
                {
                    //standard
                    builder.Append(GetSqlServerCETypeName(col.ColumnName, dbType, size));
                    builder.Append(", ");
                }
                columnIndex++;
            }

            if (table.Columns.Count > 0) builder.Length = builder.Length - 2;

            builder.Append(")");
            return builder.ToString();
        }

        public static void Create(DataSet dataSet, SqlCeConnection conn)
        {
            conn.Open();
            SqlCeCommand cmd;
            foreach (DataTable table in dataSet.Tables)
            {
                string createSql = GetCreateTableStatement(table);
                Console.WriteLine(createSql);

                cmd = new SqlCeCommand(createSql, conn);
                Console.WriteLine(cmd.ExecuteNonQuery());
            }
            conn.Close();
        }

        public static void Create(DataTable table, SqlCeConnection conn, string tableName = null, string specificColumnTypes = null)
        {
            conn.Open();
            SqlCeCommand cmd;
            DropTable(table, conn, tableName);
            string createSql = GetCreateTableStatement(table, tableName, specificColumnTypes);            
            cmd = new SqlCeCommand(createSql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private static void DropTable(DataTable table, SqlCeConnection conn, string tableName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    tableName = table.TableName;
                SqlCeCommand cmd = new SqlCeCommand("drop table " + tableName, conn);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

    }
}


