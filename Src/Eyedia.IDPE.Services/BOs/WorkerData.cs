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
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// WorkerData stores information(job slice) of a worker
    /// A job instance used by Worker
    /// </summary>
    public class WorkerData : IDisposable
    {
        public WorkerData(Job job, int slicePosition)
        {
            this.Job = job;            
            this.SlicePosition = slicePosition;
            this.Rows = new List<Row>();               
            this.BadDataInCsvFormat = new List<string>();
            this.Errors = new List<string>();
            this.Warnings = new List<string>();
            this.ProcessVariable = new Dictionary<string, object>();            
        }

        public Job Job { get; private set; }       

        /// <summary>
        /// Index of splitted data tables
        /// </summary>
        public int SlicePosition { get; private set; }       
        
        /// <summary>
        /// Current row position, while processing
        /// </summary>
        public int RowPosition { get; set;}
        
        /// <summary>
        /// Any errors during the process. This gets accumulated into job.Errors
        /// </summary>
        public List<string> Errors { get; set;}                
       
        /// <summary>
        /// One Row = List of attributes. This one returns all the rows of current process
        /// </summary>
        public List<Row> Rows { get;set;}

        /// <summary>
        /// returns current row based on RowPosition
        /// </summary>
        public Row CurrentRow 
        { 
            get 
            {
                if(this.RowPosition < this.Rows.Count)
                    return this.Rows[this.RowPosition];
                return null;
            } 
        }             
       
        /// <summary>
        /// Returns bad data in csv file format
        /// </summary>
        public List<string> BadDataInCsvFormat { get; internal set;}

        /// <summary>
        /// Stores all warnnings
        /// </summary>
        public List<string> Warnings { get; set; }

        /// <summary>
        /// Returns column value from Columns from the particular row (based on RowPosition)
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string ColumnValue(string columnName)
        {
            try
            {
                return (from val in this.CurrentRow.Columns
                        where val.Name == columnName
                        select val.Value).SingleOrDefault();
            }
            catch { return string.Empty; }
        }      

        /// <summary>
        /// Process variable is a dictionary which can be used to store values for one process
        /// Useful when u want to store something while executing rules.
        /// </summary>
        public Dictionary<string, object> ProcessVariable { get; set;}

        bool _ValueUpdationNotPermitted;

        /// <summary>
        /// Only IsValid is a permissible attribute after PostParse, no other attribute value can not be updated        
        /// </summary>
        public bool ValueUpdationNotPermitted
        {
            get { return _ValueUpdationNotPermitted; }
            internal set 
            { 
                _ValueUpdationNotPermitted = value;
                for (int c = 0; c < this.CurrentRow.Columns.Count; c++)
                {
                    this.CurrentRow.Columns[c].ValueUpdationNotPermitted = value;
                }
                for (int c = 0; c < this.CurrentRow.ColumnsSystem.Count; c++)
                {
                    this.CurrentRow.ColumnsSystem[c].ValueUpdationNotPermitted = value;
                }
            }
        }

        public string PrintRowColPosition(int originalRowPosition, string columnName = "", bool isSystemRow = false)
        {
            return columnName == string.Empty ? string.Format("{0}[{1}][*]: ", isSystemRow ? "RowSystem" : "Row", originalRowPosition) : string.Format("{0}[{1}][{2}]: ", isSystemRow ? "RowSystem" : "Row", originalRowPosition, columnName);
        }

        public StringBuilder TraceLog
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (Row row in this.Rows)
                {
                    sb.Append(row.TraceLog.ToString() + Environment.NewLine);
                }
                return sb;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            
            foreach (Row row in this.Rows)
            {
                row.Dispose();
            }
        }

        #endregion
    }

}






