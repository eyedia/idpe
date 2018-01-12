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
using System.Data;
using System.IO;
using System.Linq;
using System.Data.OleDb;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    public class CsvToDataTable : DataParser
    {
        public CsvToDataTable(Job job)
            : base(job)
        {
        }
        
        public CsvToDataTable(DataSource dataSource)
            : base(dataSource)
        {
     
        }

        public override void Dispose()
        {
            //todo
        }

        private static readonly object _lock = new object();
        public DataTable ParseData(ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {
            //if (string.IsNullOrEmpty(fileContent)) return new DataTable();
            emptyRowWarnings = new List<string>();            
            return ReadCSVFileUsingVB(ref csvRows, ref emptyRowWarnings, ref columnCount);            
        }

        #region Private Methods

        private DataTable ReadCSVFileUsingVB(ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {
            string localFileName = Job.FileName;
            if (Job.IsHavingSpecialHeaderAndOrFooter)
                localFileName = Job.FileNameWithoutHeaderAndOrFooter;

            if (!File.Exists(localFileName))
                return new DataTable();
            TextFieldParser parser = new TextFieldParser(localFileName);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(string.IsNullOrEmpty(DataSource.Delimiter) ? "," : DataSource.Delimiter);
            bool columnInformationTaken = false;
            DataTable table = new DataTable();
            bool failed = false;
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                if (!columnInformationTaken)
                {
                    if (DataSource.IsFirstRowHeader)
                    {
                        foreach (string field in fields)
                        {
                            table.Columns.Add(field);
                        }
                        columnCount = table.Columns.Count;
                    }
                    else
                    {
                        foreach (SreAttribute attribute in DataSource.AcceptableAttributes)
                        {
                            table.Columns.Add(attribute.Name);
                        }
                        if (table.Columns.Count != fields.Length)
                        {
                            DataSource.TraceError("1.Data source attribute count and data fed is not equal, could not parse data! Acceptable attributes = {0}, input file columns = {1}",
                                table.Columns.Count, fields.Length);
                            failed = true;
                            break;
                        }
                        table.Rows.Add(fields);
                        columnCount = fields.Length;
                    }
                    columnInformationTaken = true;
                }

                else
                {
                    if (table.Columns.Count != fields.Length)
                    {
                        DataSource.TraceError("2.Data source attribute count and data fed is not equal, could not parse data! Acceptable attributes = {0}, input file columns = {1}",
                                table.Columns.Count, fields.Length);
                        failed = true;
                        break;
                    }
                    table.Rows.Add(fields);
                }

                string commaSeparatedList = fields.Aggregate((a, x) => a + "\",\"" + x).ToString();
                csvRows.Add("\"" + commaSeparatedList + "\"");

            }
            parser.Close();
            if (failed)
            {
                parser.Dispose();
                parser = null;
                table.Dispose();
                table = null;
                 return new DataTable();
            }
            else
            {
                return table;
            }
        }
        #endregion Private Methods

        public static DataTable ReadFile(string fileName, string delimiter = ",", bool firstRowIsHeader = true)
        {

            if (!File.Exists(fileName))
                return new DataTable();
            TextFieldParser parser = new TextFieldParser(fileName);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(delimiter);
            bool columnInformationTaken = false;
            DataTable table = new DataTable();
            bool failed = false;
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                if (!columnInformationTaken)
                {
                    if (firstRowIsHeader)
                    {
                        foreach (string field in fields)
                        {
                            table.Columns.Add(field);
                        }                        
                    }
                    else
                    {
                        for (int c = 1; c <= fields.Length; c++)
                        {
                            table.Columns.Add("Column" + c.ToString());
                        }                        
                        table.Rows.Add(fields);                        
                    }
                    columnInformationTaken = true;
                }

                else
                {
                    if (table.Columns.Count != fields.Length)
                    {
                        ExtensionMethods.TraceError("Data source attribute count and data fed is not equal, could not parse data!");
                        failed = true;
                        break;
                    }
                    table.Rows.Add(fields);
                }

            }
            parser.Close();
            if (failed)
            {
                parser.Dispose();
                parser = null;
                table.Dispose();
                table = null;
                return new DataTable();
            }
            else
            {
                return table;
            }
        }

    }
}





