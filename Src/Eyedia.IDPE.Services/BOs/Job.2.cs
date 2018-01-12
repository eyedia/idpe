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
using Eyedia.IDPE.Common;
using System.Data;

namespace Eyedia.IDPE.Services
{
    public partial class Job
    {
        /// <summary>
        /// Thread safe agreegate function, should be called from Pre/Post validate only. The result will be stored in container data
        /// </summary>
        /// <param name="columnName">Name of the column which is to be agreegated</param>                
        /// <param name="isSytemColumn">True if the system columns to be searched</param>
        /// <param name="operationType">Aggregate operation type; default is 'Sum'</param>
        /// <param name="containerDataKeyName">Name of the container data key where the result will be added or updated</param>
        /// <returns>calculated value as double</returns>
        public double Aggregate(string columnName, bool isSytemColumn = false, AggregateOperationTypes operationType = AggregateOperationTypes.Sum, string containerDataKeyName = null)
        {
            double calculatedValue = 0;
            lock (_lock)
            {
                
                List<double> values = new List<double>();
                if (isSytemColumn)
                {
                    values = (from r in Rows
                              select r.ColumnsSystem[columnName].ValueDouble).ToList();
                }
                else
                {
                    values = (from r in Rows
                              select r.Columns[columnName].ValueDouble).ToList();
                }

                switch (operationType)
                {
                    case AggregateOperationTypes.Sum:
                        calculatedValue = values.Sum();
                        break;

                    case AggregateOperationTypes.Average:
                        calculatedValue = values.Average();
                        break;

                    case AggregateOperationTypes.Min:
                        calculatedValue = values.Min();
                        break;

                    case AggregateOperationTypes.Max:
                        calculatedValue = values.Max();
                        break;

                }
                calculatedValue = Math.Round(calculatedValue, 2);
                if (containerDataKeyName != null)
                {
                    ProcessVariables.AddOrUpdate(containerDataKeyName, calculatedValue, (keyx, oldValue) => calculatedValue);
                    ExtensionMethods.TraceInformation("Aggregate method '{0}' has been called on '{1}' and calculated value '{2}' has been assigned to ContainerData with key as '{3}'"
                        , operationType.ToString(), calculatedValue, containerDataKeyName, containerDataKeyName);
                }
                else
                {
                    ExtensionMethods.TraceInformation("Aggregate method '{0}' has been called on '{1}' and calculated value '{2}' was returned."
                      , operationType.ToString(), calculatedValue, containerDataKeyName);
                }
                return calculatedValue;
            }

        }

        /// <summary>
        /// Thread safe group by function, should be called from Pre-validate only. Returns max(count()) group by column names
        /// </summary>        
        /// <param name="commaSeparatedColumnNames">Comma separated column names</param>        
        /// <returns>count</returns>
        public int PreValidateGroupByMaxCount(string commaSeparatedColumnNames)
        {
            List<string> columns = new List<string>(commaSeparatedColumnNames.Split(",".ToCharArray()));
            
            lock (_lock)
            {              
                var result = from row in InputData.AsEnumerable()
                             group row by columns into grp
                             select new
                             {
                                 Id = grp.Key,
                                 Count = grp.Count()
                             };

                int max = 0;
                foreach (var r in result)
                {
                    if (r.Count > max)
                        max = r.Count;
                }                
                
                return max;
            }
        }

        /// <summary>
        /// Thread safe group by function, should be called from Pre-validate only. Returns max(count()) group by column names
        /// </summary>        
        /// <param name="commaSeparatedColumnNames">Comma separated column names</param>
        /// <param name="isSytemColumn">True if the system columns to be searched</param>        
        /// <returns>count</returns>
        public int PostValidateGroupByMaxCount(string commaSeparatedColumnNames, bool isSytemColumn = false)
        {
            List<string> columns = new List<string>(commaSeparatedColumnNames.Split(",".ToCharArray()));
            DataTable table = new DataTable();

            foreach (string column in columns)
            {
                table.Columns.Add(column);
            }

            lock (_lock)
            {
                foreach (Row row in Rows)
                {
                    DataRow dtRow = table.NewRow();
                    foreach (string column in columns)
                    {
                        if (isSytemColumn)
                        {
                            dtRow[column] = row.ColumnsSystem[column];
                        }
                        else
                        {
                            dtRow[column] = row.Columns[column];
                        }
                    }
                    table.Rows.Add(dtRow);
                }

                var result = from row in InputData.AsEnumerable()
                             group row by columns into grp
                             select new
                             {
                                 Id = grp.Key,
                                 Count = grp.Count()
                             };

                int max = 0;
                foreach (var r in result)
                {
                    if (r.Count > max)
                        max = r.Count;
                }

                return max;
            }
        }
        /// <summary>
        /// Converts all values column to 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="useDoubleQuote"></param>
        /// <param name="isSystemColumn"></param>
        /// <returns></returns>
        public string ColumnToString(string columnName, bool useDoubleQuote = true, bool isSystemColumn = false)
        {
            List<string> columnValues = new List<string>();
            if (!isSystemColumn)
                columnValues = (from r in this.Rows select r.Columns[columnName].Value).ToList();
            else
                columnValues = (from r in this.Rows select r.ColumnsSystem[columnName].Value).ToList();

            return useDoubleQuote ? "\"" + columnValues.Aggregate((a, x) => a + "\",\"" + x).ToString() + "\""
                : "'" + columnValues.Aggregate((a, x) => a + "','" + x).ToString() + "'";
        }
       
    }
}


