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
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Diagnostics;
using System.Data;
using Eyedia.Core.Data;

namespace Eyedia.IDPE.Services
{
	public abstract class OutputWriter
	{
        protected Job _Job;

        public bool AllowPartial { get; private set; }
        public string Delimiter { get; private set; }
        public bool IsFirstRowHeader { get; private set; }
        public bool IsErrored { get; protected set; }

        public OutputWriter()
        {
        }

        public OutputWriter(Job job)
        {
            this._Job = job;
            InitSettings();
        }

        public abstract StringBuilder GetOutput();
        public abstract object GetCustomOutput();

        protected string GetAttributeValue(Eyedia.IDPE.Services.Attribute attribute, bool returnNullWhenNeeded = false)
        {
            string emptyValue = returnNullWhenNeeded == false ? string.Empty : null;

            if (attribute == null)
                return emptyValue;
            if (string.IsNullOrEmpty(attribute.Value))
                return emptyValue;

            string returnValue = string.Empty;
            if (attribute.AttributePrintValueType == AttributePrintValueTypes.PrintNullInCaseOfNull)
            {
                returnValue = attribute.IsNull ? "NULL" : attribute.OriginalValue;
            }
            else if (attribute.Type.Type == AttributeTypes.Datetime)
            {
                if (attribute.AttributePrintValueType == AttributePrintValueTypes.DateTimePrintDate)
                {
                    if ((attribute.Value == DateTime.MinValue.ToString())
                  || (attribute.Value == "01/01/0001 00:00:00"))
                        returnValue = emptyValue;
                    else
                        returnValue = attribute.ValueDate.ToShortDateString();
                }
                else if (attribute.AttributePrintValueType == AttributePrintValueTypes.DateTimePrintMinValue)
                {
                    if ((attribute.Value == DateTime.MinValue.ToString())
                   || (attribute.Value == "01/01/0001 00:00:00"))
                        returnValue = attribute.Value;
                }
                else //default
                {
                    if ((attribute.Value == DateTime.MinValue.ToString())
                    || (attribute.Value == "01/01/0001 00:00:00"))
                        returnValue = emptyValue;
                    else
                        returnValue = attribute.Value;
                }
            }
            else if (attribute.Type.Type == AttributeTypes.Codeset)
            {
                if (attribute.AttributePrintValueType == AttributePrintValueTypes.CodeSetValueEnumCode)
                    returnValue = attribute.ValueEnumCode.ToString();
                else if (attribute.AttributePrintValueType == AttributePrintValueTypes.CodeSetValueEnumReferenceKey)
                    returnValue = attribute.ValueEnumReferenceKey;
                else
                    returnValue = attribute.Value;
            }
            else if ((attribute.Type.Type == AttributeTypes.BigInt)
                || (attribute.Type.Type == AttributeTypes.Int))
            {
                if ((attribute.AttributePrintValueType == AttributePrintValueTypes.NumericPrintNullInCaseOfZero)
                    && (attribute.Value == "0"))
                    returnValue = emptyValue;
                else
                    returnValue = attribute.Value;

            }
            else if (attribute.Type.Type == AttributeTypes.Decimal)
            {
                if ((attribute.AttributePrintValueType == AttributePrintValueTypes.NumericPrintNullInCaseOfZero)
                    && (attribute.Value == "0"))
                    returnValue = emptyValue;

                else if ((attribute.AttributePrintValueType == AttributePrintValueTypes.NumericPrintNullInCaseOfNaN)
                    && double.IsNaN(attribute.ValueDouble))
                    returnValue = emptyValue;
                else if ((attribute.AttributePrintValueType == AttributePrintValueTypes.NumericPrintNullInCaseOfInfinity)
                    && double.IsInfinity(attribute.ValueDouble))
                    returnValue = emptyValue;
                else if ((attribute.AttributePrintValueType == AttributePrintValueTypes.NumericPrintNullInCaseOfNaNOrInfinity)
                    && ((double.IsNaN(attribute.ValueDouble)) || (double.IsInfinity(attribute.ValueDouble))))
                    returnValue = emptyValue;
                else
                    returnValue = attribute.Value;
            }
            else if ((attribute.Type.Type == AttributeTypes.Bit)
                && (attribute.Value != "NULL"))
            {
                if (attribute.AttributePrintValueType == AttributePrintValueTypes.BitPrintZeroOrOne)
                    returnValue = attribute.Value.ToLower().Equals("true") ? "1" : "0";
                else if (attribute.AttributePrintValueType == AttributePrintValueTypes.BitPrintTrueOrFalse)
                    returnValue = (attribute.Value.ToLower().Equals("true") || attribute.Value.Equals("1")) ? "TRUE" : "FALSE";
                else
                    returnValue = attribute.Value;
            }
            else
            {
                returnValue = attribute.Value;
            }

            if ((_Job.DataSource.OutputType == OutputTypes.Delimited)
                && (returnValue.Contains(Delimiter)))
                return "\"" + returnValue + "\"";

            return returnValue;
        }

        //protected bool IsPrintable(string attributeName)
        //{
        //    IdpeAttribute attribute = _Job.DataSource.AttributesSystem.Where(aa => aa.Name == attributeName).SingleOrDefault();
        //    if (attribute == null)
        //        throw new Exception(string.Format("No attribute defined with the name '{0}'", attributeName));

        //    return (bool)attribute.IsAcceptable;
        //}

        private void InitSettings()
        {
            if ((_Job == null)
                || (_Job.DataSource == null))
                return;
            
            string strAllowPartial = _Job.DataSource.Keys.GetKeyValue(SreKeyTypes.OutputPartialRecordsAllowed);
            AllowPartial = string.IsNullOrEmpty(strAllowPartial) ? false : strAllowPartial.ParseBool();

            Delimiter = _Job.DataSource.Keys.GetKeyValue(SreKeyTypes.OutputDelimiter);
            Delimiter = string.IsNullOrEmpty(Delimiter) ? "," : Delimiter;

            string strHasHeader = _Job.DataSource.Keys.GetKeyValue(SreKeyTypes.OutputIsFirstRowHeader);
            IsFirstRowHeader = string.IsNullOrEmpty(strHasHeader) ? false : strHasHeader.ParseBool();

        }

        protected DataTable ToDataTable(List<Row> rows)
        {
            _Job.PerformanceCounter.Start(_Job.JobIdentifier, JobPerformanceTaskNames.ResultToDataTable);

            DataTable table = new DataTable();

            for (int i = 0; i < _Job.DataSource.AcceptableAttributesSystem.Count; i++)
            {
                table.Columns.Add(_Job.DataSource.AcceptableAttributesSystem[i].Name, _Job.DataSource.AcceptableAttributesSystem[i].ConvertSreTypeIntoDotNetType(DatabaseTypes.SqlServer));
            }
           
            foreach (Row row in rows)
            {               
                List<object> oneRow = new List<object>();
                foreach (Services.Attribute attribute in row.ColumnsSystem)
                {
                    if (attribute.IsAcceptableOrPrintable)
                    {
                        string attributeValue = GetAttributeValue(attribute);
                        if ((string.IsNullOrEmpty(attributeValue))
                            && ((attribute.Type is SreDateTime) || (attribute.Type is SreDecimal) || (attribute.Type is SreInt) || (attribute.Type is SreInt)))
                            oneRow.Add(DBNull.Value);
                        else if(attributeValue == "NULL")
                            oneRow.Add(DBNull.Value);
                        else
                            oneRow.Add(attributeValue);
                    }
                }
                DataRow newDataRow = table.NewRow();
                newDataRow.ItemArray = oneRow.ToArray();

                table.Rows.Add(newDataRow);               
            }
            _Job.PerformanceCounter.Stop(_Job.JobIdentifier, JobPerformanceTaskNames.ResultToDataTable);
            return table;
        }

	}
}





