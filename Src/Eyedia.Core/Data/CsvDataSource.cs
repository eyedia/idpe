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
using System.Collections;
using System.Data;
using System.IO;
using System.Security.Permissions;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyedia.Core.Data;


namespace Eyedia.Core.Data
{
    // The CsvDataSource is a data source control that retrieves its
    // data from a comma-separated value file.
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CsvDataSource : DataSourceControl
    {
        public CsvDataSource() : base() { }

        // The comma-separated value file to retrieve data from.
        public string FileName
        {
            get
            {
                return ((CsvDataSourceView)this.GetView(String.Empty)).SourceFile;
            }
            set
            {
                // Only set if it is different.
                if (((CsvDataSourceView)this.GetView(String.Empty)).SourceFile != value)
                {
                    ((CsvDataSourceView)this.GetView(String.Empty)).SourceFile = value;
                    RaiseDataSourceChangedEvent(EventArgs.Empty);
                }
            }
        }        
        
        /// <summary>
        /// Key field name, based on which IUD will be performed. 
        /// </summary>
        public string KeyFieldName
        {
            get
            {
                return ((CsvDataSourceView)this.GetView(String.Empty)).KeyFieldName;
            }
            set
            {
                // Only set if it is different.
                if (((CsvDataSourceView)this.GetView(String.Empty)).KeyFieldName != value)
                {
                    ((CsvDataSourceView)this.GetView(String.Empty)).KeyFieldName = value;                    
                }
            }
        }

        // Key field name, based on which IUD will be performed.
        public string UniqueKeyNameOfThisSetOfData
        {
            get
            {
                return ((CsvDataSourceView)this.GetView(String.Empty)).UniqueKeyNameOfThisSetOfData;
            }
            set
            {
                // Only set if it is different.
                if (((CsvDataSourceView)this.GetView(String.Empty)).UniqueKeyNameOfThisSetOfData != value)
                {
                    ((CsvDataSourceView)this.GetView(String.Empty)).UniqueKeyNameOfThisSetOfData = value;
                }
            }
        }

        // Do not add the column names as a data row. Infer columns if the CSV file does
        // not include column names.
        public bool IncludesColumnNames
        {
            get
            {
                return ((CsvDataSourceView)this.GetView(String.Empty)).IncludesColumnNames;
            }
            set
            {
                // Only set if it is different.
                if (((CsvDataSourceView)this.GetView(String.Empty)).IncludesColumnNames != value)
                {
                    ((CsvDataSourceView)this.GetView(String.Empty)).IncludesColumnNames = value;
                    RaiseDataSourceChangedEvent(EventArgs.Empty);
                }
            }
        }

        // Return a strongly typed view for the current data source control.
        private CsvDataSourceView view = null;
        protected override DataSourceView GetView(string viewName)
        {
            if (null == view)
            {
                view = new CsvDataSourceView(this, String.Empty);
            }
            return view;
        }
        // The ListSourceHelper class calls GetList, which
        // calls the DataSourceControl.GetViewNames method.
        // Override the original implementation to return
        // a collection of one element, the default view name.
        protected override ICollection GetViewNames()
        {
            ArrayList al = new ArrayList(1);
            al.Add(CsvDataSourceView.DefaultViewName);
            return al as ICollection;
        }
        
    }

    // The CsvDataSourceView class encapsulates the
    // capabilities of the CsvDataSource data source control.
    public class CsvDataSourceView : DataSourceView, INotificationManager
    {

        public CsvDataSourceView(IDataSource owner, string name)
            : base(owner, DefaultViewName)
        {

        }

        // The data source view is named. However, the CsvDataSource
        // only supports one view, so the name is ignored, and the
        // default name used instead.
        public static string DefaultViewName = "CommaSeparatedView";

        // The location of the .csv file.
        private string sourceFile = String.Empty;
        internal string SourceFile
        {
            get
            {
                return sourceFile;
            }
            set
            {
                // Use MapPath when the SourceFile is set, so that files local to the
                // current directory can be easily used.
                //string mappedFileName = HttpContext.Current.Server.MapPath(value); todo
                //sourceFile = mappedFileName; todo
                sourceFile = value;
            }
        }

        string keyFieldName = string.Empty;
        public string KeyFieldName
        {
            get { return this.keyFieldName; }
            set { this.keyFieldName = value; }
        }

        string uniqueKeyNameOfThisSetOfData = string.Empty;
        public string UniqueKeyNameOfThisSetOfData
        {
            get { return this.uniqueKeyNameOfThisSetOfData; }
            set { this.uniqueKeyNameOfThisSetOfData = value; }
        }

        // Do not add the column names as a data row. Infer columns if the CSV file does
        // not include column names.
        private bool columns = false;
        internal bool IncludesColumnNames
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        // Get data from the underlying data source.
        // Build and return a DataView, regardless of mode.
        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments selectArgs)
        {
            IEnumerable dataList = null;
            // Open the .csv file.
            if (File.Exists(this.SourceFile))
            {
                DataTable data = new CsvDataTable().FileToDataTable(this.SourceFile, this.IncludesColumnNames);

                #region Commented
                //DataTable data = new DataTable();

                //// Open the file to read from.
                //using (StreamReader sr = File.OpenText(this.SourceFile))
                //{
                //    // Parse the line
                //    string s = "";
                //    string[] dataValues;
                //    DataColumn col;

                //    // Do the following to add schema.
                //    dataValues = sr.ReadLine().Split(',');
                //    // For each token in the comma-delimited string, add a column
                //    // to the DataTable schema.
                //    foreach (string token in dataValues)
                //    {
                //        col = new DataColumn(token, typeof(string));
                //        data.Columns.Add(col);
                //    }

                //    // Do not add the first row as data if the CSV file includes column names.
                //    if (!IncludesColumnNames)
                //        data.Rows.Add(CopyRowData(dataValues, data.NewRow()));

                //    // Do the following to add data.
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        dataValues = s.Split(',');
                //        data.Rows.Add(CopyRowData(dataValues, data.NewRow()));
                //    }
                //}
                #endregion Commented

                data.AcceptChanges();
                DataView dataView = new DataView(data);
                if (selectArgs.SortExpression != String.Empty)
                {
                    dataView.Sort = selectArgs.SortExpression;
                }
                dataList = dataView;
            }
            else
            {
                throw new System.Configuration.ConfigurationErrorsException("File not found, " + this.SourceFile);
            }

            if (null == dataList)
            {
                throw new InvalidOperationException("No data loaded from data source.");
            }

            return dataList;
        }     

        /*
        private DataRow CopyRowData(string[] source, DataRow target)
        {
            try
            {
                for (int i = 0; i < source.Length; i++)
                {
                    target[i] = source[i];
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                // There are more columns in this row than
                // the original schema allows.  Stop copying
                // and return the DataRow.
                return target;
            }
            return target;
        }
         * */
        // The CsvDataSourceView does not currently
        // permit deletion. You can modify or extend
        // this sample to do so.
        public override bool CanDelete
        {
            get
            {
                return false;
            }
        }
        protected override int ExecuteDelete(IDictionary keys, IDictionary values)
        {
            throw new NotSupportedException();
        }
        // The CsvDataSourceView does not currently
        // permit insertion of a new record. You can
        // modify or extend this sample to do so.
        public override bool CanInsert
        {
            get
            {
                return false;
            }
        }
        protected override int ExecuteInsert(IDictionary values)
        {
            throw new NotSupportedException();
        }
        
        public override bool CanUpdate
        {
            get
            {                
                return true;
            }            
        }
        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            DataTable table = new CsvDataTable().FileToDataTable(this.SourceFile, true);
            string originalKey = keys[this.KeyFieldName].ToString();
            DataRow[] foundRows = table.Select(this.KeyFieldName + "=" + originalKey);
            string newKey = string.Empty;

            //we should find only 1 row
            if (foundRows.Length != 1)
                throw new Exception("Invalid update operation. Please make sure that data contains unique key records");
           
            bool raiseKeyFieldUpdate = false;
            foundRows[0].BeginEdit();
            for (int c = 0; c < table.Columns.Count; c++)
            {
                if ((table.Columns[c].Caption == this.KeyFieldName) && (values[table.Columns[c].Caption] != null))
                {
                    if (foundRows[0][c].ToString() != values[table.Columns[c].Caption].ToString())
                    {
                        newKey = values[table.Columns[c].Caption].ToString();
                        raiseKeyFieldUpdate = true;
                    }
                }

                if(values[table.Columns[c].Caption] != null)
                    foundRows[0][c] = values[table.Columns[c].Caption];                
            }
            foundRows[0].EndEdit();


            string operationId = this.FireBeforePerforming(this, "rowupdated", null);    //todo-operationName as property to remove hardcoding 
            string operationIdkfupdated = string.Empty;
            if(raiseKeyFieldUpdate)
                operationIdkfupdated = this.FireBeforePerforming(this, "keyfieldupdated", null);

            new CsvDataTable().DataTableToFile(table, this.sourceFile);

            Dictionary<string, object> Param = new Dictionary<string, object>();
            Param.Add("UniqueKeyNameOfThisSetOfData", this.uniqueKeyNameOfThisSetOfData);
            Param.Add("oldKey", originalKey);
            Param.Add("newKey", newKey == string.Empty? originalKey:newKey);
            this.FireAfterPerforming(this, operationId, Param);

            if (raiseKeyFieldUpdate)                
                this.FireAfterPerforming(this, operationIdkfupdated, Param);

            return 1;
        }

        #region INotificationManager Implementations

        public string GetManagerName()
        {
            return this.GetType().ToString();
        }

        public string GetPoissibleOperationNames()
        {
            return "rowupdated,keyfieldupdated";
        }

        public string FireBeforePerforming(object thisObject, string operationName, Dictionary<string, object> parameters)
        {
            return NotificationManager.Instance.FireBeforePerforming(thisObject, operationName, parameters);
        }

        public void FireAfterPerforming(object thisObject, string operationId, Dictionary<string, object> parameters)
        {
            NotificationManager.Instance.FireAfterPerforming(thisObject, operationId, parameters);
        }

        #endregion INotificationManager Implementations

    }
}





