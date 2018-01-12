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
using System.Diagnostics;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
    
    public class Row : IDisposable
    {        
        public Column Columns { get; set; }
        public Column ColumnsSystem { get; set; }
        public Attribute IsValidColumn
        {
            get
            {
                return ColumnsSystem["IsValid"];
            }
           
        }

        public string Errors
        {
            get
            {
                List<string> colErrors = Columns.Where(c => string.IsNullOrEmpty(c.Error.Message) == false).Select(c2 => c2.Error.Message).ToList();
                List<string> colSystemErrors = ColumnsSystem.Where(c => string.IsNullOrEmpty(c.Error.Message) == false).Select(c2 => c2.Error.Message).ToList();

                return colErrors.ToLine() + colSystemErrors.ToLine();
            }
        }
        public Row(int originalRowPosition)
        {
            this.OriginalRowPosition = originalRowPosition;
            this.Columns = new Column();
            this.ColumnsSystem = new Column();
            this.TraceLog = new SreTraceLogWriter(this.OriginalRowPosition);
        }

        //Original row position of input data
        public int OriginalRowPosition { get; private set; }


        /// <summary>
        /// This can be called only from RowValidate rules
        /// </summary>
        public bool IsValidForced { get; internal set; }

        /// <summary>
        /// Trace log for each row
        /// </summary>
        public SreTraceLogWriter TraceLog { get; private set; }

        /// <summary>
        /// Adds error message to a specific column
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <param name="columnName">Name of the attribute</param>
        /// <param name="isSystemRow">If passed, error will be formatted as Row[n] or RowSystem[n]</param>
        public void AddError(string errorMessage, string columnName = null, bool isSystemRow = false)
        {
            string msg = string.Format("{0}[{1}][{2}]:", isSystemRow ? "RowSystem" : "Row", OriginalRowPosition, columnName);
            msg = string.Format("{0} {1}", msg, errorMessage);

            if (columnName != null)
            {
                if (!isSystemRow)
                {
                    Columns[columnName].Error = new Common.SreMessage(msg);
                    Columns[columnName].HasBusinessError = true;
                }
                else
                {
                    ColumnsSystem[columnName].Error = new Common.SreMessage(msg);
                    ColumnsSystem[columnName].HasBusinessError = true;
                }
            }
            else
            {
                IsValidColumn.Error = new Common.SreMessage(msg);
                IsValidColumn.HasBusinessError = true;
            }

            IsValidColumn.Value = "false";
        }

        public bool HasAnyError()
        {
            bool itHasError = true;     //default is true, because it has to through at least one 'if' below
            if (this.Columns != null)
            {
                List<Attribute> attributesWithErrors = (from a in Columns
                                                        where !string.IsNullOrEmpty(a.Error.Message)
                                                        select a).ToList<Attribute>();

                itHasError = attributesWithErrors.Count > 0 ? true : false;
                
            }

            if (itHasError)
                return itHasError;

            if (this.ColumnsSystem != null)
            {
                List<Attribute> systemAttributesWithErrors = (from a in ColumnsSystem
                                                              where !string.IsNullOrEmpty(a.Error.Message)
                                                              select a).ToList<Attribute>();

                itHasError = systemAttributesWithErrors.Count > 0 ? true : false;
            }
            return itHasError;
        }

        public bool IsHavingUniqueValues(string currentJobId, string columnName)
        {
            Job currentJob = null;
            List<string> allPreviousDataOfThisColumn = new List<string>();
            string currentDataOfthisColumn = string.Empty;
            int oldCount = 0;
            try
            {
                currentJob = Registry.Instance.Entries[currentJobId] as Job;

                //get all existing values
                allPreviousDataOfThisColumn = (from row in currentJob.Rows
                                               where row.Columns[columnName].Name == columnName
                                               select row.Columns[columnName].Value).ToList<string>();

                //union current value
                currentDataOfthisColumn = (from a in this.Columns
                                           where a.Name.ToLower().Equals(columnName.ToLower())
                                           select a.Value).SingleOrDefault();
                allPreviousDataOfThisColumn.Add(currentDataOfthisColumn);

                //keep track of old count
                oldCount = allPreviousDataOfThisColumn.Count;
                allPreviousDataOfThisColumn.DistinctEx();

                return oldCount == allPreviousDataOfThisColumn.Count ? true : false;

            }

            catch (Exception ex)
            {
                string errMsg = string.Format("An error occurred while checking unique values of {0}{1}", columnName, Environment.NewLine);
                errMsg += string.Format("CurrentJobId = {0}{1}", currentJobId, Environment.NewLine);
                errMsg += string.Format("Current job is null? = {0}{1}", currentJob == null ? true : false, Environment.NewLine);
                errMsg += string.Format("allPreviousDataOfThisColumn.Count = {0}{1}", allPreviousDataOfThisColumn.Count, Environment.NewLine);
                errMsg += string.Format("oldCount = {0}{1}", oldCount, Environment.NewLine);
                errMsg += errMsg + Environment.NewLine + ex.ToString();
                currentJob.TraceError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns comma separated values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string strRow = string.Empty;
            foreach (Attribute attrib in Columns)
            {
                strRow += "\"" + attrib.Value + "\",";
            }           
            foreach (Attribute attrib in ColumnsSystem)
            {
                strRow += "\"" + attrib.Value + "\",";
            }

            if (strRow.Length > 0)
                strRow = strRow.Substring(0, strRow.Length - 1);
            return strRow;
        }


        #region IDisposable Members

        public void Dispose()
        {
            this.TraceLog.Dispose();
            Columns.Dispose();
            ColumnsSystem.Dispose();
        }

        #endregion
    }
    
}



