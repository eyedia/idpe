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
using System.Reflection;
using System.Data.Linq.Mapping;
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic.FileIO;


namespace Eyedia.Core.Data
{
    public static class DataExtensionMethods
    {
        /// <summary>
        /// Gets the length limit for a given field on a LINQ object ... or zero if not known
        /// </summary>        
        /// <param name="table">the linq table name</param>
        /// <param name="columnName">the column name on that table</param>
        /// <returns></returns>
        public static int LinqObjectGetLengthLimit(this object table, string columnName)
        {
            int dblenint = 0;   // default value = we can't determine the length

            Type type = table.GetType();
            PropertyInfo prop = type.GetProperty(columnName);
            // Find the Linq 'Column' attribute
            // e.g. [Column(Storage="_FileName", DbType="NChar(256) NOT NULL", CanBeNull=false)]
            object[] info = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
            // Assume there is just one
            if (info.Length == 1)
            {
                ColumnAttribute ca = (ColumnAttribute)info[0];
                string dbtype = ca.DbType;

                if (dbtype.StartsWith("NChar") || dbtype.StartsWith("NVarChar"))
                {
                    int index1 = dbtype.IndexOf("(");
                    int index2 = dbtype.IndexOf(")");
                    string dblen = dbtype.Substring(index1 + 1, index2 - index1 - 1);
                    int.TryParse(dblen, out dblenint);
                }
            }
            return dblenint;
        }

        /// <summary>
        /// Truncates column value of a table to its max allowed value...
        /// </summary>
        public static void LinqObjectAutoTruncate(this object table, string columnName, string columnValue)
        {
            int len = LinqObjectGetLengthLimit(table, columnName);
            if (len == 0) return;

            Type type = table.GetType();
            PropertyInfo prop = type.GetProperty(columnName);
            if (columnValue.Length > len)
            {
                prop.SetValue(table, columnValue.Substring(0, len), null);
            }
            else
                prop.SetValue(table, columnValue, null);
        }

        /// <summary>
        /// Truncates all column values to its max allowed value...
        /// </summary>
        public static void LinqObjectAutoTruncate(this object table)
        {
            PropertyInfo[] columns = table.GetType().GetProperties();

            foreach (PropertyInfo column in columns)
            {
                int len = LinqObjectGetLengthLimit(table, column.Name);
                if (len == 0) continue;
                Type type = table.GetType();
                PropertyInfo prop = type.GetProperty(column.Name);
                string columnValue = column.GetValue(table, null).ToString();
                if (columnValue.Length > len)
                {
                    prop.SetValue(table, columnValue.Substring(0, len), null);
                }
                else
                    prop.SetValue(table, columnValue, null);
            }
        }


        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="parameterName">
        /// Name of the parameter.
        /// </param>
        /// <param name="parameterValue">
        /// The parameter value.
        /// </param>
        /// <param name="setDbType">pass 'true' if you would like to set parameter.DbType</param>
        /// <remarks>
        /// </remarks>
        public static void AddParameterWithValue(this IDbCommand command, string parameterName, object parameterValue, bool setDbType = false)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            if (setDbType)
                SetDbType(parameter, parameterValue);
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Set DbType to the passed parameter
        /// </summary>
        /// <param name="parameter">The Parameter</param>
        /// <param name="parameterValue">The parameter value</param>
        public static void SetDbType(this IDbDataParameter parameter, object parameterValue)
        {
            Type parameterValueType = parameterValue.GetType().IsArray ? parameterValue.GetType().GetElementType() : parameterValue.GetType();

            if (parameterValueType == typeof(string))
                parameter.DbType = DbType.String;
            else if ((parameterValueType == typeof(Int16))
                || (parameterValueType == typeof(Int16?)))
                parameter.DbType = DbType.Int16;
            else if ((parameterValueType == typeof(Int32))
                || (parameterValueType == typeof(Int32?)))
                parameter.DbType = DbType.Int32;
            else if ((parameterValueType == typeof(long))
                || (parameterValueType == typeof(long?)))
                parameter.DbType = DbType.Int64;
            else if ((parameterValueType == typeof(double))
                || (parameterValueType == typeof(double?)))
                parameter.DbType = DbType.Decimal;
            else if ((parameterValueType == typeof(decimal))
                || (parameterValueType == typeof(decimal?)))
                parameter.DbType = DbType.Decimal;
            else if ((parameterValueType == typeof(bool))
                || (parameterValueType == typeof(bool?)))
                parameter.DbType = DbType.Boolean;
            else if ((parameterValueType == typeof(DateTime))
                || (parameterValueType == typeof(DateTime?)))
                parameter.DbType = DbType.DateTime;
            else if (parameterValueType == typeof(byte))
                parameter.DbType = DbType.Byte;
        }

        /// <summary>
        /// Writes data table into comma separated Csv file
        /// </summary>
        /// <param name="dataTable">The data table</param>
        /// <param name="fileName">The file name</param>
        /// <param name="delimiter">The delimiter, default is comma</param>
        public static long ToCsv(this DataTable dataTable, string fileName, string delimiter = ",")
        {
            //StringBuilder sb = new StringBuilder();
            //var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            //sb.AppendLine(string.Join(delimiter, columnNames));
            //foreach (DataRow row in dataTable.Rows)
            //{
            //    var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
            //    sb.AppendLine("\"" + string.Join("\"" + delimiter + "\"", fields) + "\"");
            //}
            
            File.WriteAllText(fileName, dataTable.ToCsvStringBuilder(delimiter).ToString());
            return dataTable.Rows.Count;
        }

        /// <summary>
        /// Writes data table into comma separated Csv file
        /// </summary>
        /// <param name="dataTable">The data table</param>       
        /// <param name="delimiter">The delimiter, default is comma</param>
        public static StringBuilder ToCsvStringBuilder(this DataTable dataTable, string delimiter = ",")
        {
            StringBuilder sb = new StringBuilder();
            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            sb.AppendLine(string.Join(delimiter, columnNames));
            foreach (DataRow row in dataTable.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                sb.AppendLine("\"" + string.Join("\"" + delimiter + "\"", fields) + "\"");
            }

            return sb;
        }

        /// <summary>
        /// Splits data table into number of data tables
        /// </summary>
        /// <param name="table">The data table</param>
        /// <param name="noOfRows">Number of rows per chunk(data table)</param>
        /// <returns>List of data tables</returns>
        public static List<DataTable> Split(this DataTable table, int noOfRows)
        {
            List<DataTable> tables = new List<DataTable>();
            int count = 0;
            DataTable copyTable = null;

            foreach (DataRow dr in table.Rows)
            {
                if ((count++ % noOfRows) == 0)
                {
                    copyTable = new DataTable();
                    // Clone the structure of the table.
                    copyTable = table.Clone();
                    // Add the new DataTable to the list.
                    tables.Add(copyTable);
                }
                // Import the current row.
                copyTable.ImportRow(dr);
            }
            return tables;
        }

        /// <summary>
        /// Checks duplicate entries based on key columns. Optinally removes column
        /// </summary>
        /// <param name="table">input data table</param>
        /// <param name="keyColumns">List of key columns to check duplicate</param>
        /// <param name="doNotRemove">if true, then it will not remove duplicate entries</param>
        /// <returns>List of string containing error messages</returns>
        public static List<string> CheckDuplicates(this DataTable table, string[] keyColumns, bool doNotRemove)
        {
            List<string> result = new List<string>();
            Dictionary<string, string> uniquenessDict = new Dictionary<string, string>(table.Rows.Count);
            StringBuilder sb = null;
            int rowIndex = 0;
            DataRow row;
            DataRowCollection rows = table.Rows;
            while (rowIndex < rows.Count)
            {
                row = rows[rowIndex];
                sb = new StringBuilder();
                foreach (string colname in keyColumns)
                {
                    sb.Append(((string)row[colname]));
                }

                if (uniquenessDict.ContainsKey(sb.ToString()))
                {
                    if (!doNotRemove)
                        rows.Remove(row);
                    result.Add(string.Format("Row[{0}]: is duplicate.", rowIndex + 1));
                }
                else
                {
                    uniquenessDict.Add(sb.ToString(), string.Empty);
                    rowIndex++;
                }
            }
            return result;
        }

        /// <summary>
        /// Converts an object to data table
        /// </summary>
        /// <param name="dataObject">the object to be converted to data table</param>
        /// <returns>Converted data table</returns>
        public static DataTable ToDataTable(this object dataObject)
        {
            var tpDataObject = dataObject.GetType();

            DataTable tbl = new DataTable();
            DataRow dataRow = tbl.NewRow();
            PropertyInfo[] allProperties = tpDataObject.GetProperties();
            foreach (var property in allProperties)
            {
                if (property.CanRead)
                {
                    object value = property.GetValue(dataObject, null);
                    DataColumn clm = tbl.Columns.Add(property.Name, property.PropertyType);
                    dataRow[clm] = value;
                }
            }
            tbl.Rows.Add(dataRow);
            tbl.AcceptChanges();
            return tbl;
        }

        /// <summary>
        /// Converts string builder to data table
        /// </summary>
        /// <param name="delimitedData">the delimited string to be converted to data table</param>
        /// <param name="header">pass true if the delimited string contains header</param>
        /// <param name="delimiter">the delimiter</param>
        /// <returns>Converted data table</returns>
        public static DataTable ToDataTable(this StringBuilder delimitedData, bool header = false, string delimiter = ",")
        {
            string fileName = Path.GetTempFileName();
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(delimitedData);
            sw.Close();
            DataTable table = new DataTable();
            try
            {
                table = ToDataTable(fileName, header, delimiter);
            }
            finally { File.Delete(fileName); }

            return table;
        }

        /// <summary>
        /// Read file to data table
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="header">pass true if the delimited string contains header</param>
        /// <param name="delimiter">the delimiter</param>
        /// <returns></returns>
        public static DataTable ToDataTable(string fileName, bool header = false, string delimiter = ",")
        {            
            if (!File.Exists(fileName))
                return new DataTable();
            TextFieldParser parser = new TextFieldParser(fileName);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(delimiter);
            bool columnInformationTaken = false;
            DataTable table = new DataTable();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                if (!columnInformationTaken)
                {
                    if (header)
                    {
                        foreach (string field in fields)
                        {
                            table.Columns.Add(field);
                        }                        
                    }
                    else
                    {
                        for (int i = 1; i <= fields.Length; i++)
                        {
                            table.Columns.Add("Column " + i);
                        }

                        table.Rows.Add(fields);                        
                    }
                    columnInformationTaken = true;
                }

                else
                {
                    table.Rows.Add(fields);
                }
            }
            parser.Close();

            return table;
        }

        /// <summary>
        /// Converts a list of objecst to data table
        /// </summary>
        /// <param name="data">the list of objects to be converted to data table</param>
        /// <returns>Converted data table</returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// Removes empty rows from data table
        /// </summary>        
        /// <param name="dataTable">The dirty data table</param>
        /// <param name="onlyFromEnd">Pass false if you want empty rows to be deleted from anywhere</param>
        /// <returns>Cleaned data table</returns>
        public static DataTable RemoveEmptyRows(this DataTable dataTable, bool onlyFromEnd = true)
        {            
            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {               
                bool isEmpty = true;
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (!string.IsNullOrEmpty(dataTable.Rows[i][column].ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }
                if (isEmpty)
                {
                    dataTable.Rows[i].Delete();
                }
                else
                {
                    if (!onlyFromEnd)
                        break;
                }
            }
            dataTable.AcceptChanges();
            return dataTable;
        }

        /// <summary>
        /// Returns code set list based on code
        /// </summary>
        /// <param name="codeSets">List of codesets</param>
        /// <param name="code">The code</param>
        /// <returns></returns>
        public static List<SymplusCodeSet> Find(this List<SymplusCodeSet> codeSets, string code)
        {
            return codeSets.Where(cs => cs.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Returns code set list based on code
        /// </summary>
        /// <param name="codeSets">List of codesets</param>
        /// <param name="code">The code</param>
        /// <returns></returns>
        public static List<SymplusCodeSet> Find(this ReadOnlyCollection<SymplusCodeSet> codeSets, string code)
        {
            return codeSets.Where(cs => cs.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).ToList();
        }


        /// <summary>
        /// Returns code value list based on code
        /// </summary>
        /// <param name="codeSets">List of codesets</param>
        /// <param name="code">The code</param>
        /// <returns></returns>
        public static List<string> GetCodeValues(this List<SymplusCodeSet> codeSets)
        {
            return codeSets.Select(cs => cs.Value).ToList();
        }

        /// <summary>
        /// Returns code value list based on code
        /// </summary>
        /// <param name="codeSets">List of codesets</param>
        /// <param name="code">The code</param>
        /// <returns></returns>
        public static List<string> GetCodeValues(this ReadOnlyCollection<SymplusCodeSet> codeSets)
        {
            return codeSets.Select(cs => cs.Value).ToList();
        }

        /// <summary>
        /// Checks if user is an admin user - will return true if the user is system user or an admin user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns></returns>
        public static bool IsAdmin(this SymplusUser user)
        {
            if (user == null) return false;

            if (user.IsSystemUser == true)
            {
                return true;
            }
            else
            {
                SymplusGroup adminGroup = CoreDatabaseObjects.Instance.GetAdminGroup();
                if (user.GroupId == adminGroup.Id)
                    return true;
            }
            return false;
        }

        public static string LookUp(this DataTable table, string criteria, string returnColumnName = null)
        {
            string result = "NULL";
            DataRow[] dataRow = table.Select(criteria);
            if (dataRow.Length > 0)
            {
                if (!string.IsNullOrEmpty(returnColumnName))
                {
                    result = dataRow[0][returnColumnName].ToString();
                }
                else
                {
                    if (dataRow[0].Table.Columns.Count == 1)
                    {
                        result = dataRow[0][0].ToString();
                    }
                    else
                    {
                        for (int i = 0; i < dataRow[0].Table.Columns.Count; i++)
                        {
                            result = result + (dataRow[0][i] == null ? "NULL" : dataRow[0][i].ToString()) + "|";
                        }
                        if (result.Length > 0)
                            result = result.Substring(0, result.Length - 1);
                    }
                }
            }

            return result;
        }
       
       

    }
}



