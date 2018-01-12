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
using Symplus.Core;
using Symplus.Core.Data;
using Symplus.RuleEngine.Common;
using Symplus.RuleEngine.DataManager;
using Excel;
using System.Diagnostics;
using System.Threading;

namespace Symplus.RuleEngine.Services
{
    public class MicrosoftExcelToDataTable1 : DataParser
    {
        public enum ExcelVersion
        {
            Unknown,
            Format2003,
            Format2007
        }

        bool _isFirstRowAsColumnNames;
     
        public MicrosoftExcelToDataTable1(DataSource dataSource, bool firstRowIsHeader)
            : base(dataSource)
        {
            _isFirstRowAsColumnNames = firstRowIsHeader;

        }

        public override void Dispose()
        {
            //todo
        }

        private static readonly object _lock = new object();

        public DataTable ParseData(string fileName, ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {
            if (!File.Exists(fileName)) return new DataTable();
            emptyRowWarnings = new List<string>();
            
            lock (_lock)
            {
                ExcelVersion excelVersion = ExcelVersion.Unknown;
                DataTable dataTable = ReadExcelFile(fileName, ref columnCount, ref excelVersion);
                HashSet<int> DateColumnPostions = new HashSet<int>();
                if(excelVersion == ExcelVersion.Format2003)
                    DateColumnPostions = GetDateColumnPostions(dataTable);

                try
                {
                    //trying to remove empty rows
                    dataTable = dataTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                }
                catch { }

                csvRows.Clear();
                if (_isFirstRowAsColumnNames)
                {
                    string header = string.Empty;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        header += "\"" + column.ColumnName + "\",";
                    }
                    csvRows.Add(header);
                }
                

                foreach (DataRow row in dataTable.Rows)
                {
                    //var rowAsString = "\"" + string.Join("\",\"", row.ItemArray.Cast<string>().ToArray()) + "\"";
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
                                colValue = DateTime.FromOADate(double.Parse(colValue)).ToString();
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
            
                return dataTable.RemoveEmptyRows();
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

        #region Complex Code 1 - Start
        //IExcelDataReader excelReader;

        //void OpenExcelReader(object monitorSyncExcelReaderOpenedOrNot, string fileName)
        //{
        //    try
        //    {
        //        if (new FileUtility().WaitTillFileIsFree(fileName))
        //        {
        //            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
        //            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        //        }
        //    }
        //    catch (OutOfMemoryException) { }
        //    lock (monitorSyncExcelReaderOpenedOrNot)
        //    {
        //        Monitor.Pulse(monitorSyncExcelReaderOpenedOrNot);
        //    }
        //}

        //void OpenExcelReaderWithTimeOut(string fileName)
        //{
        //    Action<object, string> openExcelReaderMethod = OpenExcelReader;
        //    object monitorSyncExcelReaderOpenedOrNot = new object();
        //    bool timedOut;
        //    lock (monitorSyncExcelReaderOpenedOrNot)
        //    {
        //        openExcelReaderMethod.BeginInvoke(monitorSyncExcelReaderOpenedOrNot,fileName, null, null);
        //        timedOut = !Monitor.Wait(monitorSyncExcelReaderOpenedOrNot, TimeSpan.FromSeconds(10));
        //    }
        //    if ((timedOut)
        //        || (excelReader == null))
        //    {
        //        throw new Exception(string.Format("The file {0} could not be read or it does not contain any record!", fileName));
        //    }
        //}

        #endregion Complex Code 1 - End

        private DataTable ReadExcelFile(string fileName, ref int columnCount, ref ExcelVersion excelVersion)
        {

            string extension = Path.GetExtension(fileName).ToLower();
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            //For some corrupt .xls file  'ExcelReaderFactory.CreateBinaryReader(stream)' statement goes into infinite loop
            //'excelReader = ExcelReaderFactory.CreateBinaryReader(stream); does not return
            //a new mechanism used to instantiate excelReader with timeout facility
            #region Simple Code - Commented
            
            IExcelDataReader excelReader = null;
            if (extension == ".xls")
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                excelVersion = ExcelVersion.Format2003;
            }
            else if (extension == ".xlsx")
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelVersion = ExcelVersion.Format2007;
            }
            
            #endregion Simple Code - Commented

            #region Complex Code 2 - Start

            //OpenExcelReaderWithTimeOut(fileName);

            #endregion Complex Code 2 - End
            //if (excelReader == null) return new DataTable();
            DataSet result = null;
            if (excelReader != null)
            {
                excelReader.IsFirstRowAsColumnNames = _isFirstRowAsColumnNames;
                result = excelReader.AsDataSet(true);
                excelReader.Close();
            }

            if (result == null)
            {
                throw new Exception(string.Format("The file {0} could not be read or it does not contain any record!", fileName));
            }
            else if (result.Tables.Count > 0)
            {
                columnCount = result.Tables[0].Columns.Count;
                return result.Tables[0];
            }
            else
            {
                //nothing could be retrieved using Excel Lib.
                //Lets try using standard mechanism                
                if (SreConfigurationSection.CurrentConfig.MicrosoftExcelOLEDataReader.Enabled)
                {
                    Trace.TraceInformation("Could not read using default excel library, trying to read using OLE");
                    DataTable table = ReadUsingOLE(fileName);
                    if (table.Rows.Count == 0)
                        Trace.TraceError("Could not read any record from '{0}'.", Path.GetFileName(fileName));
                    columnCount = table.Columns.Count;
                    return table;
                }
                return new DataTable();
            }

        }

        private DataTable ReadUsingOLE(string fileName)
        {            
            DataTable table = new DataTable();
            //string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR={1}\""
            //    , fileName, _isFirstRowAsColumnNames ? "YES" : "NO");
            string connectionString = string.Format("Provider={0};Data Source={1};Extended Properties=\"Excel {2};HDR={3}\"",
                SreConfigurationSection.CurrentConfig.MicrosoftExcelOLEDataReader.Provider, fileName, 
                SreConfigurationSection.CurrentConfig.MicrosoftExcelOLEDataReader.Version, _isFirstRowAsColumnNames ? "YES" : "NO");
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





