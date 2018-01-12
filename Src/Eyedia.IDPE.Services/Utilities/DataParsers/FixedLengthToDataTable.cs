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
using System.Data.OleDb;
using System.Data;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Microsoft.VisualBasic.FileIO;
using Eyedia.IDPE.DataManager;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    public class FixedLengthToDataTable : DataParser
	{
       
        public string Schema{get;private set;}
        public int RowLength { get; private set; }
        string ColumnNames { get; set; }
        List<int> FieldWidths { get; set; }

        public FixedLengthToDataTable(Job job)
            : base(job)
        {
            IdpeKey schemaKey = Job.DataSource.Key(SreKeyTypes.FixedLengthSchema);
            if (schemaKey == null)
                throw new ApplicationException(string.Format("'FixedLengthSchema' schema is not defined in '{0}'", Job.DataSource.Name));

            this.Schema = schemaKey.Value;            
            GenerateColumnNames();            

        }
        public DataTable ParseData(ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {
            //if (string.IsNullOrEmpty(fileContent)) return new DataTable();
            emptyRowWarnings = new List<string>();
            //string fileName = TempFolder + "\\" + ShortGuid.NewGuid().ToString();
            //lock (_lock)
            //{
            //    //writing file               
            //    using (StreamWriter sw = new StreamWriter(fileName))
            //    {
            //        sw.Write(fileContent);
            //        sw.Close();
            //    }

            //}
            TextFieldParser parser = null;
            if(!Job.IsHavingSpecialHeaderAndOrFooter)
                parser = new TextFieldParser(Job.FileName);
            else
                parser = new TextFieldParser(Job.FileNameWithoutHeaderAndOrFooter);

            parser.TrimWhiteSpace = false;

            parser.TextFieldType = FieldType.FixedWidth;
            parser.SetFieldWidths(FieldWidths.ToArray());
            //parser.SetFieldWidths(new int[] { 3, 10, 5, 10, 8, 8, 14, 3 });
            bool columnInformationTaken = false;
            DataTable table = new DataTable();
            string commaSeparatedList = string.Empty;
            int totLength = 0;
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
                        commaSeparatedList = DataSource.AcceptableAttributes.Select(ax => ax.Name).Aggregate((a, x) => a + "\",\"" + x).ToString();
                        csvRows.Add("\"" + commaSeparatedList + "\"");
                    }
                    else
                    {
                        foreach (IdpeAttribute attribute in DataSource.AcceptableAttributes)
                        {
                            table.Columns.Add(attribute.Name);
                        }                        
                        table.Rows.Add(fields);
                    }
                    columnInformationTaken = true;
                }

                else
                {
                    table.Rows.Add(fields);
                }

                commaSeparatedList = fields.Aggregate((a, x) => a + "\",\"" + x).ToString();
                totLength = fields.Sum((a) => a.Length);
                if (totLength > columnCount)
                    columnCount = totLength;
                csvRows.Add("\"" + commaSeparatedList + "\"");

            }
            parser.Close();
            //File.Delete(fileName);

            if ((Job.DataSource.IsFirstRowHeader)
                && (csvRows.Count > 0))
                csvRows.RemoveAt(0);
            return table;
        }

        public override void Dispose()
        {
            //todo
        }

        #region Private Methods
       
        private static readonly object _lock = new object();


        void GenerateColumnNames()
        {

            FieldWidths = new List<int>();

            List<string> lines = new List<string>(Schema.Split(Environment.NewLine.ToCharArray()));            
            if (lines.Count < 3)
                throw new Exception("Invalid fixed length schema! At least 3 lines expected!");

            this.RowLength = 0;
            foreach (string line in lines)
            {
               
                if ((line.Trim().Length > 0)
                    && (!(line.StartsWith("ColNameHeader")))
                    && (line.Substring(0, 3).Equals("Col", StringComparison.OrdinalIgnoreCase)))
                {

                    string[] words = line.Split(" ".ToCharArray());
                    if (words.Length > 0)
                    {
                        int width = 0;
                        int.TryParse(words[words.Length - 1], out width);
                        this.RowLength += width;

                        string[] columnName = words[0].Split("=".ToCharArray());
                        if (columnName.Length > 1)
                            this.ColumnNames += string.Format("LEFT({0} + SPACE({1}), {1}) as [{0}],", columnName[1], width);

                        FieldWidths.Add(width);
                    }
                }
            }
            
            if ((this.ColumnNames.Length > 0)
                && (this.ColumnNames.Substring(this.ColumnNames.Length - 1, 1) == ","))
                this.ColumnNames = this.ColumnNames.Substring(0, this.ColumnNames.Length - 1);
        }

        #endregion Private Methods
	}
}





