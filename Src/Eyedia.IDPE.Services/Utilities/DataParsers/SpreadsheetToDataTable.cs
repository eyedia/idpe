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
using System.Data;
using System.IO;
using System.Data.OleDb;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Excel;
using System.Diagnostics;
using System.Threading;

namespace Eyedia.IDPE.Services
{
    public class SpreadsheetToDataTable : DataParser
    {
        public enum ExcelVersion
        {
            Unknown,
            Format2003,
            Format2007
        }

        public SpreadsheetToDataTable(Job job)
            : base(job){}

        public SpreadsheetToDataTable(DataSource dataSource)
            : base(dataSource){}

        public override void Dispose()
        {
            //todo
        }

        private static readonly object _lock = new object();

        public DataTable ParseData(ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {   
            emptyRowWarnings = new List<string>();
            int spreadSheetNumber = (int)Job.DataSource.Keys.GetKeyValue(SreKeyTypes.SpreadSheetNumber).ParseInt();

            lock (_lock)
            {
                ExcelVersion excelVersion = ExcelVersion.Unknown; //excel version is not in use
                DataTable dataTable = ReadExcelFile(Job.FileName, Job.DataSource.IsFirstRowHeader, spreadSheetNumber, ref columnCount);
                HashSet<int> DateColumnPostions = new HashSet<int>();
                if (excelVersion == ExcelVersion.Format2003)
                    DateColumnPostions = GetDateColumnPostions(dataTable);

                try
                {
                    //trying to remove empty rows
                    dataTable = dataTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                }
                catch { }

                csvRows.Clear();
                if (Job.DataSource.IsFirstRowHeader)
                {
                    string header = string.Empty;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        header += "\"" + column.ColumnName + "\",";
                    }
                    csvRows.Add(header);
                }

                dataTable = dataTable.RemoveEmptyRows();
                foreach (DataRow row in dataTable.Rows)
                {                    
                    string rowAsString = string.Empty;
                    int pos = 0;
                    foreach (object col in row.ItemArray)
                    {
                        string colValue = col.ToString();
                        if ((excelVersion == ExcelVersion.Format2003)
                            && (DateColumnPostions.Contains(pos)))
                        {
                            try
                            {
                                colValue = DateTime.FromOADate((double)colValue.ParseDouble()).ToString();
                                row[pos] = colValue;
                            }
                            catch { }
                        }

                        rowAsString += "\"" + (col == DBNull.Value ? string.Empty : colValue) + "\",";
                        pos++;
                    }
                    if (rowAsString.Length > 0)
                        rowAsString = rowAsString.Substring(0, rowAsString.Length - 1);
                    csvRows.Add(rowAsString);
                }

                return dataTable;
            }
        }


        private HashSet<int> GetDateColumnPostions(DataTable dataTable)
        {
            HashSet<int> positionSet = new HashSet<int>();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Columns[i].Caption.ToLower().Contains("date"))
                    positionSet.Add(i);
            }
            return positionSet;
        }

        #region Private Methods

        public static DataTable ReadExcelFile(string fileName, bool isFirstRowHeader, int spreadSheetNumber, ref int columnCount)
        {
            try
            {
                ExcelDataReaderInstantiator excelDataReaderInstantiator = null;
                try
                {
                    excelDataReaderInstantiator = new ExcelDataReaderInstantiator(fileName);
                }
                catch(IOException ioe)
                {
                    //we can eat this exception, as during multi instance scenario the file might have already been processed by other instance
                }
                DataSet result = null;
                if ((excelDataReaderInstantiator != null) 
                    && (excelDataReaderInstantiator.ExcelReader != null))
                {
                    excelDataReaderInstantiator.ExcelReader.IsFirstRowAsColumnNames = isFirstRowHeader;
                    result = excelDataReaderInstantiator.ExcelReader.AsDataSet(true);
                    excelDataReaderInstantiator.ExcelReader.Close();
                }

                if (result == null)
                {
                    new DataTable();
                }
                else if (result.Tables.Count > 0)
                {
                    if (spreadSheetNumber < result.Tables.Count)
                    {
                        columnCount = result.Tables[spreadSheetNumber].Columns.Count;
                        return result.Tables[spreadSheetNumber];
                    }
                    else
                    {
                        new DataTable();
                    }
                }
                else
                {
                    //nothing could be retrieved using Excel Lib.
                    //Lets try using standard mechanism                
                    if (SreConfigurationSection.CurrentConfig.MicrosoftExcelOLEDataReader.Enabled)
                    {
                        ExtensionMethods.TraceInformation("Could not read using default excel library, trying to read using OLE");
                        DataTable table = ReadUsingOLE(fileName, isFirstRowHeader);
                        if (table.Rows.Count == 0)
                            ExtensionMethods.TraceError("Could not read any record from '{0}'.", Path.GetFileName(fileName));
                        columnCount = table.Columns.Count;
                        return table;
                    }
                }

                return new DataTable();
            }
            catch (FileNotFoundException fnfe)
            {
                //this try/catch is placed when multi instances of SRE runs on same machine
                return new DataTable();
            }

        }

        public static DataTable ReadUsingOLE(string fileName, bool isFirstRowHeader)
        {
            DataTable table = new DataTable();
            //string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR={1}\""
            //    , fileName, _isFirstRowAsColumnNames ? "YES" : "NO");
            string connectionString = string.Format("Provider={0};Data Source={1};Extended Properties=\"Excel {2};HDR={3}\"",
                SreConfigurationSection.CurrentConfig.MicrosoftExcelOLEDataReader.Provider, fileName,
                SreConfigurationSection.CurrentConfig.MicrosoftExcelOLEDataReader.Version, isFirstRowHeader ? "YES" : "NO");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DataTable dtSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if ((null == dtSchema) || (dtSchema.Rows.Count <= 0))
                    {
                        //raise exception if needed
                    }

                    string sheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                    DataSet dataSet = new DataSet();
                    OleDbCommand command = new OleDbCommand(string.Format("SELECT * FROM [{0}]", sheetName), connection);
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(dataSet);
                    table = dataSet.Tables[0];
                    connection.Close();
                }
                finally
                {
                    if (connection != null && (connection.State != ConnectionState.Broken || connection.State != ConnectionState.Closed))
                    {
                        connection.Close();
                    }
                }
            }

            return table;
        }

        #endregion Private Methods

    }
}





