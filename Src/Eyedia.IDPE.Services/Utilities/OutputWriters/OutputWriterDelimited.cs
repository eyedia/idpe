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
using System.Xml;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    public class OutputWriterDelimited  : OutputWriter
	{
        public OutputWriterDelimited() : base() { }
        public OutputWriterDelimited(Job job) : base(job) { }

        /// <summary>
        /// Generates XML string based on pre-determined format
        /// </summary>
        /// <returns></returns>
        public override StringBuilder GetOutput()
        {
            
            if (_Job != null)
            {
                if (_Job.IsErrored)
                    return new StringBuilder();

                if (!AllowPartial)
                {
                    if (_Job.Errors.Count == 0)
                        return WriteOutput(_Job.Rows);
                    else
                        return new StringBuilder();
                }
                else
                {
                    return WriteOutput(_Job.Rows.Where( r=> r.IsValidColumn.ValueBoolean == true).ToList());
                }
            }

            return new StringBuilder();
        }

        public override object GetCustomOutput()
        {
            return null;
        }

        #region Private Methods
       
        private StringBuilder WriteOutput(List<Row> rows)
        {
            StringBuilder sb = new StringBuilder();
            if (IsFirstRowHeader)
                sb.AppendLine(CreateHeader());

            bool doNotEncloseWithDoubleQuotes = _Job.DataSource.Keys.GetKeyValue(IdpeKeyTypes.OutputDelimiterDoNotEncloseWithDoubleQuote).ParseBool();           

            foreach (Row row in rows)
            {
                string oneLine = string.Empty;
                foreach (Services.Attribute attribute in row.ColumnsSystem)
                {
                    if (attribute.IsAcceptableOrPrintable)
                    {
                        if (!doNotEncloseWithDoubleQuotes)
                            oneLine += "\"" + GetAttributeValue(attribute) + "\"" + Delimiter;
                        else
                            oneLine += GetAttributeValue(attribute) + Delimiter;
                    }
                }
                if (oneLine.Length > 1)
                    oneLine = oneLine.Substring(0, oneLine.Length - 1);

                sb.AppendLine(oneLine);
            }

            return sb;
        }

        private string CreateHeader()
        {
            string header = string.Empty;
            if (_Job.Rows.Count > 0)
            {
                foreach (Services.Attribute attribute in _Job.Rows[0].ColumnsSystem)
                {
                    if (attribute.IsAcceptableOrPrintable)
                    {
                        header += attribute.Name + Delimiter;
                    }
                }
                if (header.Length > 1)
                    header = header.Substring(0, header.Length - 1);
            }
            return header;
        }

        #endregion Private Methods
    }
}


