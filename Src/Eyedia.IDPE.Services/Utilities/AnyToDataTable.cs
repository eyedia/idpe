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
using System.Diagnostics;
using System.ComponentModel;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.IO;

namespace Eyedia.IDPE.Services
{

    public class AnyToDataTable
    {
        public AnyToDataTable(Job job)
        {            
            this.Job = job;            
        }
        
        public Job Job { get; private set; }        

        public List<string> Feed()
        {
            if (!File.Exists(Job.FileName)) //when IDPE multiple instances are running
                return new List<string>();

            List<string> errors = new List<string>();

            if (Job.DataSource.DataFormatType == DataFormatTypes.Zipped)
            {
                FeedZipped();
            }
            else if (Job.DataSource.DataFormatType == DataFormatTypes.Xml)
            {
                FeedXml();

            }
            else if ((Job.DataSource.DataFormatType == DataFormatTypes.Delimited)
                || (Job.DataSource.DataFormatType == DataFormatTypes.EDIX12)
                || (Job.DataSource.DataFormatType == DataFormatTypes.Sql))        //sql inputs are always Csv type
            {
                FeedCsv();
            }
            else if (Job.DataSource.DataFormatType == DataFormatTypes.SpreadSheet)
            {
                FeedSpreadSheet();
            }
            else if (Job.DataSource.DataFormatType == DataFormatTypes.FixedLength)
            {
                FeedFixedLength();
            }
            else if (Job.DataSource.DataFormatType == DataFormatTypes.MultiRecord)
            {
                FeedMultiRecord(Job.FileName);
            }
            else if (Job.DataSource.DataFormatType == DataFormatTypes.CSharpCode)
            {
                FeedCSharpCode();
            }
            else if (Job.DataSource.DataFormatType == DataFormatTypes.Custom)
            {
                string fileName = string.Empty;
                if (Registry.Instance.Entries.ContainsKey(Job.JobIdentifier))
                    fileName = ((Job)Registry.Instance.Entries[Job.JobIdentifier]).FileName;

                FeedCustom();
            }
            else
            {
                //todo
                Job.InputData = new DataTable();

            }
            if (Job.DataSource.IsFirstRowHeader)
                RenameColumnNames();
            
            return errors;
        }
       
        /// <summary>
        /// Converts list objects to data table and feeds to DataTable 'InputData' of underlying Job object
        /// </summary>
        /// <param name="inputData">List of objects</param>
        ///<param name="overridenMapping">Comma separated additional mapping override information. For example if "EmpId" from object to be mapped with "EmployeeId" of attribute, then "EmpId=EmployeeId,Ename=EmployeeName"</param>
        public List<string> Feed<T>(IList<T> data, string overridenMapping)
        {
            CollectOverriddenMapping(overridenMapping);

            List<string> errors = new List<string>();    
            string strType = this.Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.DataFeedCustomType);
            Type type = Type.GetType(strType);

            if (type == null)
            {
                errors.Add(String.Format("The data source '{0}' accepts '{1}' type data which was not provided", this.Job.DataSource.Name, strType));
                return errors;
            }
            
            string[] columnNames = this.Job.DataSource.AcceptableAttributes.Select(a => a.Name).ToArray();
            string commaSeparatedList = columnNames.Aggregate((a, x) => a + "," + x).ToString();
            Job.CsvRows.Add(commaSeparatedList);//header
            Job.BadDataInCsvFormat.Add(commaSeparatedList);//header
            this.Job.InputData = new DataTable();


            //Additional columns
            DataColumn dc = new DataColumn(Constants.ColumnNamePosition);
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.AutoIncrementStep = 1;
            dc.DataType = typeof(Int32);
            this.Job.InputData.Columns.Add(dc);

            dc = new DataColumn(Constants.ColumnNameContainerError);
            dc.DataType = typeof(String);
            this.Job.InputData.Columns.Add(dc);            

            foreach (string column in columnNames)
            {
                this.Job.InputData.Columns.Add(column);
            }           

            DataTable table = ToDataTable(data, type);
            foreach (DataRow row in table.Rows)
            {
                DataRow jobDataRow = this.Job.InputData.NewRow();
                commaSeparatedList = string.Empty;

                foreach (string column in columnNames)
                {                    
                    try
                    {
                        jobDataRow[column] = GetRowValue(column, row);
                    }
                    catch
                    {
                        Job.Errors.Add(string.Format("'{0}' property not found in '{1}' type", column, strType));                     
                    }
                    commaSeparatedList = commaSeparatedList + "\"" + jobDataRow[column] + "\",";
                }
                if(commaSeparatedList.Length > 1)
                    commaSeparatedList = commaSeparatedList.Substring(0, commaSeparatedList.Length - 1);             
                this.Job.InputData.Rows.Add(jobDataRow);
                Job.CsvRows.Add(commaSeparatedList);
            }            

            Job.ColumnCount = columnNames.Length;            
            return errors;
        }

        public List<String> OverriddenMapping { get; private set; }

        private void CollectOverriddenMapping(string overridenMapping)
        {
            OverriddenMapping = new List<string>();
            if (string.IsNullOrEmpty(overridenMapping))
                return;
            OverriddenMapping = new List<string>(overridenMapping.Split(",".ToCharArray()));
        }

        private string GetRowValue(string columnName, DataRow row)
        {            
            if (row.Table.Columns.Contains(columnName))
                return row[columnName].ToString();

            if (OverriddenMapping == null)
                return string.Empty;

            columnName = columnName.ToLower();

            foreach (string overriddenMap in OverriddenMapping)
            {
                string[] colMapInfo = overriddenMap.ToLower().Split("=".ToCharArray());
                if (colMapInfo[1] == columnName)
                {
                    if (row.Table.Columns.Contains(colMapInfo[0]))
                        return row[colMapInfo[0]].ToString();
                    else
                        return string.Empty;
                }
            }
            return string.Empty;
        }

        public DataTable ToDataTable<T>(IList<T> data)
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

        public DataTable ToDataTable<T>(IList<T> data, Type type)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(type);
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

        private void FeedCustom()
        {
            List<string> errors = new List<string>();
            IdpeKey fileInterface = Job.DataSource.Key(IdpeKeyTypes.FileInterfaceName);
            if(fileInterface == null)
            {
                //let's do one check for system datasource, keys may be missing for system ds                
                new Manager().InsertSystemObjects();
                Job.DataSource.RefreshKeys();
                fileInterface = Job.DataSource.Key(IdpeKeyTypes.FileInterfaceName);
            }
            if (fileInterface != null)
            {
                if (File.Exists(Job.FileName)) //when IDPE multiple instances are running
                {
                    Job.InputData = GenerateInputDataFromInterface(fileInterface.Value);
                    if ((Job.DataSource.IsFirstRowHeader) && (Job.CsvRows.Count > 0))
                        Job.BadDataInCsvFormat.Add(Job.CsvRows[0]); //storing header information
                }
            }
            else
            {                
                
                throw new Exception(string.Format("Custom interface '{0}' is not defined!", fileInterface != null? fileInterface.Value : "NULL"));
            }

            Job.ColumnCount = Job.InputData.Columns.Count;
        }      

        #region RenameColumnNames
        private void RenameColumnNames()
        {
            try
            {
                string renameColumnHeader = Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.RenameColumnHeader);
                if ((string.IsNullOrEmpty(renameColumnHeader))
                    || (renameColumnHeader.ToLower() == "false")
                    || (Job.CsvRows.Count < 1))
                    return;

                List<string> columnNames = Job.DataSource.AcceptableAttributes.Select(a => a.Name).ToList();

                string newColumns = string.Empty;
                int counter = 0;
                foreach (string column in columnNames)
                {
                    newColumns += column + ",";
                    Job.InputData.Columns[counter].ColumnName = column;
                    counter++;
                }

                if (newColumns.Length > 1)
                    newColumns = newColumns.Substring(0, newColumns.Length - 1);

                if (Job.CsvRows.Count > 0)
                    Job.CsvRows[0] = newColumns;

                //lets not do Job, let the bad data header be the same as original file
                //if(BadDataInCSVFormat.Count > 0)
                //    BadDataInCSVFormat[0] = newColumns;
            }
            catch (Exception ex)
            {
                //here try catch is needed, as we are just feeding data we do not know what format the data is.
                //for example, instead of 10 columns, we might received 7 columns. The case will eventually be handled by IDPE
                //and caller should get notified of the issue.
                //Hence lets keep any rename column related issue as silent issue (if error).
                Job.TraceError(string.Format("Can not rename column! DataSource = {0}", Job.DataSource.Id) + Environment.NewLine + ex.ToString());
            }
        }
        

        #endregion RenameColumnNames
        
        private void FeedFixedLength()
        {
            FixedLengthToDataTable fixedLengthParser = new FixedLengthToDataTable(Job);
            List<string> csvRows = new List<string>();
            List<string> warnings = new List<string>();
            int myColumnCount = 0;
            Job.InputData = fixedLengthParser.ParseData(ref csvRows, ref warnings, ref myColumnCount);

            Job.CsvRows.AddRange(csvRows);
            Job.Warnings.AddRange(warnings);

            if ((Job.DataSource.IsFirstRowHeader) && (Job.CsvRows.Count > 0))
                Job.BadDataInCsvFormat.Add(Job.CsvRows[0]); //storing header information
            Job.ColumnCount = myColumnCount;

        }

        private void FeedSpreadSheet()
        {
            using (SpreadsheetToDataTable MSExcelToDataTable = new SpreadsheetToDataTable(Job))
            {
                int myColumnCount = 0;
                List<string> csvRows = new List<string>();
                List<string> warnings = new List<string>();
                Job.InputData = MSExcelToDataTable.ParseData(ref csvRows, ref warnings, ref myColumnCount);
                Job.CsvRows.AddRange(csvRows);
                Job.Warnings.AddRange(warnings);

                if (Job.CsvRows.Count > 0)
                    Job.BadDataInCsvFormat.Add(Job.CsvRows[0]); //storing header information
                Job.ColumnCount = myColumnCount;
            }
        }

        private void FeedZipped()
        {
            string fileExtension = string.Empty;
            Job currentJob = Registry.Instance.Entries[Job.JobIdentifier] as Job;
            if (currentJob == null)
            {
                currentJob.TraceError("Could not find job information of job id '{0}'. Aborting.", Job.JobIdentifier);
                return;
            }
            fileExtension = currentJob.FileExtension;
            switch (fileExtension)
            {
                case ".CSV":
                case ".csv":
                    FeedCsv();
                    break;
                case ".XML":
                case ".xml":
                    FeedXml();
                    break;

                default:
                    string warnMessage = string.Format("System is not configured to handle a '{0}' type file within a zip file. The file '{1}' was ignored."
                        , fileExtension, ZipFileWatcher.RenameToOriginal(currentJob.FileName));
                    new PostMan(currentJob, false).Send(PostMan.__warningStartTag + warnMessage + PostMan.__warningEndTag, null, true);
                    ExtensionMethods.TraceInformation(warnMessage);
                    Trace.Flush();
                    return;
            }
        }

        private void FeedCsv()
        {         
            using (CsvToDataTable csvToDataTable = new CsvToDataTable(Job))
            {
                int myColumnCount = 0;
                List<string> csvRows = new List<string>();
                List<string> warnings = new List<string>();
                Job.InputData = csvToDataTable.ParseData(ref csvRows, ref warnings, ref myColumnCount);
                Job.CsvRows.AddRange(csvRows);
                Job.Warnings.AddRange(warnings);
                if ((Job.DataSource.IsFirstRowHeader) && (Job.CsvRows.Count > 0))
                    Job.BadDataInCsvFormat.Add(Job.CsvRows[0]); //storing header information
                Job.ColumnCount = myColumnCount;
            }
        }

        private void FeedMultiRecord(string fileName)
        {
            List<string> csvRows = new List<string>();
            List<string> warnings = new List<string>();
            int columnCount = 0;

            Job.InputData = new CSharpCodeToDataTable(Job).Parse(fileName, ref csvRows, ref warnings, ref columnCount);
            Job.CsvRows.AddRange(csvRows);
            Job.Warnings.AddRange(warnings);
            Job.ColumnCount = columnCount;
        }

        private void FeedCSharpCode()
        {
            List<string> csvRows = new List<string>();
            List<string> warnings = new List<string>();
            int columnCount = 0;

            CSharpCodeInformation csharpCodeInformation = CSharpCodeToDataTable.GetCSharpCodeInformation(Job.DataSource.Keys);
            if(csharpCodeInformation.CodeType == CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileName)
                Job.InputData = new CSharpCodeToDataTable(Job).Parse(Job.FileName, ref csvRows, ref warnings, ref columnCount);
            else
                Job.InputData = new CSharpCodeToDataTable(Job).Parse(Job.FileContent, ref csvRows, ref warnings, ref columnCount);

            Job.CsvRows.AddRange(csvRows);
            Job.Warnings.AddRange(warnings);
            Job.ColumnCount = columnCount;
        }
       
        public void FeedXml()
        {
            List<string> csvRows = new List<string>();
            List<string> warnings = new List<string>();
            int columnCount = 0;
            XmlFeedMechanism feedMechanism = (XmlFeedMechanism)Enum.Parse(typeof(XmlFeedMechanism), Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.XmlFeedMechanism), true);

            if (feedMechanism == XmlFeedMechanism.Xslt)
            {
                Job.InputData = new XmlToDataTable(Job).Parse(ref csvRows, ref warnings, ref columnCount);
                Job.CsvRows.AddRange(csvRows);
                Job.Warnings.AddRange(warnings);
            }
            else if (feedMechanism == XmlFeedMechanism.CSharpCode)
            {
                Job.InputData = new CSharpCodeToDataTable(Job).Parse(Job.FileContent, ref csvRows, ref warnings, ref columnCount);
                Job.CsvRows.AddRange(csvRows);
                Job.Warnings.AddRange(warnings);
            }
            else if (feedMechanism == XmlFeedMechanism.Custom)
            {
                Job.InputData = GenerateInputDataFromXml(Job.FileContent, Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.FileInterfaceName));
            }
            
            Job.ColumnCount = Job.InputData.Columns.Count;
        }

        private DataTable GenerateInputDataFromXml(StringBuilder xmlContent, string interfaceName)
        {
            if ((!string.IsNullOrEmpty(interfaceName))
                && (Type.GetType(interfaceName) != null))
            {
                object objInputFileGenerator = Activator.CreateInstance(Type.GetType(interfaceName), Job);
                InputFileGenerator inputFileGenerator = (InputFileGenerator)objInputFileGenerator;
                DataTable table = inputFileGenerator.GenerateFileContent(xmlContent);

                Job.CsvRows.Clear();
                if (Job.DataSource.IsFirstRowHeader)
                {
                    string columnNames = string.Empty;
                    foreach (IdpeAttribute attrib in Job.DataSource.AcceptableAttributes)
                    {
                        columnNames += "\"" + attrib.Name + "\",";
                    }
                    Job.CsvRows.Add(columnNames);
                }

                foreach (DataRow row in table.Rows)
                {                    
                    string rowAsString = string.Empty;
                    foreach (object col in row.ItemArray)
                    {
                        rowAsString += "\"" + (col == DBNull.Value ? string.Empty : col.ToString()) + "\",";
                    }
                    if (rowAsString.Length > 0)
                        rowAsString = rowAsString.Substring(0, rowAsString.Length - 1);
                    Job.CsvRows.Add(rowAsString);
                }
                return table;
            }
            return new DataTable();
        }

        private DataTable GenerateInputDataFromInterface(string interfaceName)
        {          
                if ((!string.IsNullOrEmpty(interfaceName))
                && (Type.GetType(interfaceName) != null))
            {
                object objInputFileGenerator = Activator.CreateInstance(Type.GetType(interfaceName), Job);
                InputFileGenerator inputFileGenerator = (InputFileGenerator)objInputFileGenerator;
                DataTable table = new DataTable();
                try
                {
                    //try 1: with fileName
                    table = inputFileGenerator.GenerateFileContent(Job.FileName);
                }
                catch (NotImplementedException)
                {
                    try
                    {
                        //try 2: with fileContent
                        StringBuilder sb = new StringBuilder(Job.FileContent.ToString());
                        table = inputFileGenerator.GenerateFileContent(sb);
                    }
                    catch (NotImplementedException)
                    {
                        Job.TraceError("The datasource '{0}' has not implemented '{1}' interface."
                            , Job.DataSource.Name, interfaceName);
                    }
                }

                Job.CsvRows.Clear();

                if (Job.DataSource.IsFirstRowHeader)
                {
                    string columnNames = string.Empty;
                    foreach (IdpeAttribute attrib in Job.DataSource.AcceptableAttributes)
                    {
                        columnNames += "\"" + attrib.Name + "\",";
                    }
                    Job.CsvRows.Add(columnNames);
                }

                foreach (DataRow row in table.Rows)
                {
                    string rowAsString = string.Empty;
                    foreach (object col in row.ItemArray)
                    {
                        rowAsString += "\"" + (col == DBNull.Value ? string.Empty : col.ToString()) + "\",";
                    }
                    if (rowAsString.Length > 0)
                        rowAsString = rowAsString.Substring(0, rowAsString.Length - 1);
                    Job.CsvRows.Add(rowAsString);
                }
                return table;
            }
            return new DataTable();
        }
    }
}



