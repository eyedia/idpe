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
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using Eyedia.Core;
using System.Text.RegularExpressions;

namespace Eyedia.Core.Data
{
    public class CsvDataTable
    {        
        public CsvDataTable()
        {
        }

        public DataTable FileContentToDataTable(string fileContent, bool firstRowIsHeader)
        {
            if (string.IsNullOrEmpty(fileContent)) return new DataTable();
            object _objlock = new object();

            lock (_objlock)
            {
                string filename = Path.GetTempFileName();
                StreamWriter sw = new StreamWriter(filename);
                sw.Write(fileContent);
                sw.Close();
                DataTable dataTable = ReadCSVFile(filename, firstRowIsHeader);
                File.Delete(filename);                
                return dataTable;

            }

        }

        public DataTable FileToDataTable(string fileName, bool firstRowIsHeader)
        {

            DataTable dataTable = ReadCSVFile(fileName, firstRowIsHeader);
            return dataTable;
        }

        public void DataTableToFile(DataTable dataTable, string outputFilePath)
        {
            // Create the CSV file to which grid data will be exported.
            StreamWriter sw = new StreamWriter(outputFilePath, false);

            // First we will write the headers.
            int iColCount = dataTable.Columns.Count;

            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dataTable.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dataTable.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {

                    if (!Convert.IsDBNull(dr[i]))
                    {
                        //if (dr[i].ToString().Contains("2011"))
                        //    Debugger.Break();

                        string cellValue = string.Empty;

                        DateTime dtTm = DateTime.Now;
                        DateTime.TryParse(dr[i].ToString(), out dtTm);

                        if (dtTm != DateTime.MinValue)
                            cellValue = string.Format("{0} {1}", dtTm.ToShortDateString(), dtTm.ToShortTimeString());
                        else
                            cellValue = dr[i].ToString();
                       
                        if (cellValue.Contains(",")) //todo
                            cellValue = string.Format("\"{0}\"", cellValue);
                        sw.Write(cellValue);
                    }

                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            

        }

        #region Private Methods
        private DataTable ReadCSVFile(string path, bool firstRowIsHeader)
        {
            if (!File.Exists(path))
                return null;

            string full = Path.GetFullPath(path);
            string file = Path.GetFileName(full);
            string dir = Path.GetDirectoryName(full);

            //create the "database" connection string 
            //string connString = "Provider=Microsoft.Jet.OLEDB.4.0;"
            //  + "Data Source=\"" + dir + "\\\";"
            //  + "Extended Properties=\"text;HDR=Yes;FMT=Delimited\"";

            string connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"{0}\";Extended Properties=\"text;HDR={1};FMT=Delimited\"",dir, firstRowIsHeader?"Yes":"No");

            //create the database query
            string query = "SELECT * FROM " + file;

            //create a DataTable to hold the query results
            DataTable dTable = new DataTable();

            //create an OleDbDataAdapter to execute the query
            OleDbDataAdapter dAdapter = new OleDbDataAdapter(query, connString);

            try
            {
                //fill the DataTable
                dAdapter.Fill(dTable);
            }
            catch (Exception /*e*/)
            { }

            dAdapter.Dispose();

            return dTable;
        }
     
        #endregion Private Methods

    }
}





