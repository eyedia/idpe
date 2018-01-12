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
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Web.Hosting;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    public class SreXmlToDataTable : DataParser
    {
        readonly string __validationErrorMsg = "Can not process further as XML validation failed. Log Id '{0}' might have more information.";
        public SreXmlToDataTable(DataSource dataSource)
            : base(dataSource)
        {
            _XMLValidationErrors = new List<string>();
            _XMLValidationWarnings = new List<string>();
        }

        public override void Dispose()
        {
            //todo
        }

        public DataTable ParseData(string xmlInputData, ref List<string> csvRows,
            ref List<string> validationErrors, ref List<string> validationWarnings)
        {

            DataTable dataTable = new DataTable("SRERequest");
            if (string.IsNullOrEmpty(xmlInputData)) return dataTable;

            ValidateXML(xmlInputData);
            validationErrors = _XMLValidationErrors;
            validationWarnings = _XMLValidationWarnings;
            if (validationErrors.Count > 0)
                return dataTable;

            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlInputData));
                XPathDocument docNav = new XPathDocument(ms);
                XPathNavigator nav = docNav.CreateNavigator();
                XPathNodeIterator nodes = nav.Select("/SRERequest/Rows/Row");
                int rowCounter = 0;
                while (nodes.MoveNext())
                {
                    #region Attribute Traverse
                    XPathNodeIterator attributeNodes = nodes.Current.SelectDescendants(XPathNodeType.Element, false);
                    List<object> oneRow = new List<object>();
                    foreach (XPathNavigator attributeNode in attributeNodes)
                    {
                        if (!attributeNode.LocalName.Equals("Attribute", StringComparison.OrdinalIgnoreCase))
                            continue;
                        XPathNodeIterator nodeAVPairs = attributeNode.SelectDescendants(XPathNodeType.Element, false);
                        string columnName = string.Empty;
                        foreach (XPathNavigator nodeAVPair in nodeAVPairs)
                        {
                            if (nodeAVPair.LocalName.Equals("Name", StringComparison.OrdinalIgnoreCase))
                                columnName = nodeAVPair.Value;
                            else if (nodeAVPair.LocalName.Equals("Value", StringComparison.OrdinalIgnoreCase))
                                oneRow.Add(nodeAVPair.Value);
                        }
                        if (rowCounter == 0)
                            dataTable.Columns.Add(columnName);
                    }
                    #endregion Attribute Traverse

                    #region CSVRows
                    string csvRow = string.Empty;
                    if (rowCounter == 0)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            csvRow += "\"" + column.ColumnName + "\",";
                        }
                        csvRows.Add(csvRow);
                        csvRow = string.Empty;
                    }
                    foreach (object cell in oneRow)
                    {
                        csvRow += "\"" + cell.ToString() + "\",";
                    }

                    csvRows.Add(csvRow);
                    #endregion CSVRows

                    dataTable.Rows.Add(oneRow.ToArray());
                    rowCounter++;
                }
            }
            catch (Exception ex)
            {
                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                DataSource.TraceError(errorMessage);
                validationErrors.Add(string.Format(__validationErrorMsg, errorId));
            }

            return dataTable;
        }
        public void ParseDataSourceDetails(string xmlInputData, ref int dataSourceId, ref string dataSourceName,
            ref List<string> validationErrors, ref List<string> validationWarnings)
        {
            if (string.IsNullOrEmpty(xmlInputData)) return;

            ValidateXML(xmlInputData);
            validationErrors = _XMLValidationErrors;
            validationWarnings = _XMLValidationWarnings;

            //xml might be invalid, still lets try if we can get datasource id or name
            try
            {
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlInputData));
                XPathDocument docNav = new XPathDocument(ms);
                XPathNavigator nav = docNav.CreateNavigator();

                XPathNodeIterator nodes = nav.Select("/SreRequest/DataSource/Id"); nodes.MoveNext();
                int.TryParse(nodes.Current.Value, out dataSourceId);
                nodes = nav.Select("/SreRequest/DataSource/Name"); nodes.MoveNext();
                dataSourceName = nodes.Current.Value;
            }
            catch (Exception ex)
            {
                string errorId = Guid.NewGuid().ToString();
                string errorMessage = errorId + ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                DataSource.TraceError(errorMessage);
                validationErrors.Add(string.Format(__validationErrorMsg, errorId));
            }
        }

        #region XML Schema Validation
        List<string> _XMLValidationErrors;
        List<string> _XMLValidationWarnings;

        void ValidateXML(string xmlInputData)
        {
            StringReader stringReader = new StringReader(xmlInputData);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stringReader); stringReader.Close(); stringReader.Dispose();
            string xsdFileLocation = HostingEnvironment.MapPath("~/App_Data/SRERequests.xsd");
            xmlDoc.Schemas.Add(null, xsdFileLocation);
            xmlDoc.Validate(new ValidationEventHandler(XMLValidationEventHandler));
        }

        void XMLValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    _XMLValidationErrors.Add(e.Message);
                    break;
                case XmlSeverityType.Warning:
                    _XMLValidationWarnings.Add(e.Message);
                    break;
            }

        }
        #endregion XML Schema Validation
    }
}





