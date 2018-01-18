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
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Executes defined CSharp code to generate dattaable for the datasource
    /// </summary>
    public class CSharpCodeToDataTable : DataParser
    {
        /// <summary>
        /// Instantiate CSharpCodeToDataTable with job
        /// </summary>
        /// <param name="job">The job object</param>        
        public CSharpCodeToDataTable(Job job)
            : base(job)
        {

        }

        /// <summary>
        /// Instantiate CSharpCodeToDataTable with job
        /// </summary>
        /// <param name="dataSource">The DataSource object</param>        
        public CSharpCodeToDataTable(DataSource dataSource)
            : base(dataSource)
        {

        }


        /// <summary>
        /// Converts xml file into data table
        /// </summary>
        /// <param name="fileContent">Xml file content</param>
        /// <param name="csvRows">List of string csv rows</param>
        /// <param name="emptyRowWarnings">List of warnings</param>
        /// <param name="columnCount">Total column count</param>
        /// <returns></returns>
        public DataTable Parse(StringBuilder fileContent, ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {
            DataTable table = Parse(fileContent);
            columnCount = table.Columns.Count;

            AddCommaSeparatedColumnNames(table, ref csvRows);
            foreach (DataRow row in table.Rows)
            {
                string rowAsString = string.Empty;
                int pos = 0;
                foreach (object col in row.ItemArray)
                {
                    string colValue = col.ToString();

                    rowAsString += "\"" + (col == DBNull.Value ? string.Empty : col) + "\",";
                    pos++;
                }
                if (rowAsString.Length > 0)
                    rowAsString = rowAsString.Substring(0, rowAsString.Length - 1);
                csvRows.Add(rowAsString);
            }

            return table;
        }

        /// <summary>
        /// Converts xml file into data table
        /// </summary>
        /// <param name="fileName">Input file name</param>
        /// <param name="csvRows">List of string csv rows</param>
        /// <param name="emptyRowWarnings">List of warnings</param>
        /// <param name="columnCount">Total column count</param>
        /// <returns></returns>
        public DataTable Parse(string fileName, ref List<string> csvRows, ref List<string> emptyRowWarnings, ref int columnCount)
        {
            DataTable table = Parse(fileName);
            columnCount = table.Columns.Count;

            AddCommaSeparatedColumnNames(table, ref csvRows);
            foreach (DataRow row in table.Rows)
            {
                string rowAsString = string.Empty;
                int pos = 0;
                foreach (object col in row.ItemArray)
                {
                    string colValue = col.ToString();

                    rowAsString += "\"" + (col == DBNull.Value ? string.Empty : col) + "\",";
                    pos++;
                }
                if (rowAsString.Length > 0)
                    rowAsString = rowAsString.Substring(0, rowAsString.Length - 1);
                csvRows.Add(rowAsString);
            }

            return table;
        }

        /// <summary>
        /// Executes defined CSharp code and return DataTable
        /// </summary>
        /// <param name="fileContent">file content</param>
        /// <param name="csharpCodeInformation">Csharp code information, if not passed, will be taken from Datasource key</param>
        /// <returns></returns>
        public DataTable Parse(StringBuilder fileContent, CSharpCodeInformation csharpCodeInformation = null)
        {
            csharpCodeInformation = GetCSharpCodeInformation(DataSource.Keys, csharpCodeInformation);
            return (DataTable)GetCSharpCodeExecutor(csharpCodeInformation).Execute(csharpCodeInformation.CompleteCode, "GenerateFileContent", new object[] { fileContent });
        }

        /// <summary>
        /// Executes defined CSharp code and return DataTable
        /// </summary>
        /// <param name="fileName">file content</param>
        /// <param name="csharpCodeInformation">Csharp code information, if not passed, will be taken from Datasource key</param>
        /// <returns></returns>
        public DataTable Parse(string fileName, CSharpCodeInformation csharpCodeInformation = null)
        {
            csharpCodeInformation = GetCSharpCodeInformation(DataSource.Keys, csharpCodeInformation);
            return (DataTable)GetCSharpCodeExecutor(csharpCodeInformation).Execute(csharpCodeInformation.CompleteCode, "GenerateFileContent", new object[] { fileName });
        }

        #region Helpers

        public static CSharpCodeInformation GetCSharpCodeInformation(List<IdpeKey> dataSourceKeys, CSharpCodeInformation csharpCodeInformation = null)
        {
            if (csharpCodeInformation == null)
                csharpCodeInformation = new CSharpCodeInformation(dataSourceKeys.GetKeyValue(IdpeKeyTypes.CSharpCodeGenerateTable));

            if (string.IsNullOrEmpty(csharpCodeInformation.RawString))
                throw new Exception("C# Code was not defined!");

            return csharpCodeInformation;
        }

        private ExecuteCSharpCode GetCSharpCodeExecutor(CSharpCodeInformation csharpCodeInformation)
        {
            return new ExecuteCSharpCode("Eyedia.IDPE.Common.CSharpCodeInputFileGenerator", new object[] {this.Job},
                    ExecuteCSharpCode.CompilerVersions.v40, csharpCodeInformation.GetReferencedAssemblies());
        }

        private void AddCommaSeparatedColumnNames(DataTable table, ref List<string> csvRows)
        {
            if (this.DataSource.IsFirstRowHeader)
            {
                csvRows.Add(string.Join(",", table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray()));
            }
        }

        #endregion Helpers
    }
}


