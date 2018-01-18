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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
	internal class AttributeParser
	{
        int DataSourceId;
        string DataSourceName;
        Column _SystemAttributes;
        Parameters _Parameters;
        SqlClientManager _SQLClientManager;        
        List<IdpeKey> _DataSourceKeys;        

        Column _OtherAvailableAttributes;
        public Column OtherAvailableAttributes { get { return _OtherAvailableAttributes; } set { _OtherAvailableAttributes = value; } }        

        internal AttributeParser(int dataSourceId, string dataSourceName, Column systemAttributes,
            Parameters parameters, SqlClientManager sqlClientManager, List<IdpeKey> applicationKeys)
        {
            this.DataSourceId = dataSourceId;
            this.DataSourceName = dataSourceName;
            this._SystemAttributes = systemAttributes;
            this._Parameters = parameters;
            this._SQLClientManager = sqlClientManager;           
            this._DataSourceKeys = applicationKeys;           

            //Lets format all SQL Queries with whatever available fields we have
            FormatAllSQLQueries(systemAttributes);  //we are going to format again after parsing all data, we will have more available parameters

        }
        
        /// <summary>
        /// Parses attributes
        /// </summary>
        /// <param name="attributes">Master attribute list</param>
        /// <param name="column">A column(Attributes) to be parsed</param>
        /// <param name="isSystemRow">true if processing system row</param>
        /// <param name="rowPosition">Row position, used only in error information, to identify row. 0(Zero) in case of system attributes.</param>
        /// <param name="doNotWriteErrorInTraceFile">To avoid confusion, if this is true, it wont write sql query formatting errors into trace file.
        /// (in case of first attempt, values may not be ready to replace in queries, which is absolutely a valid scenario)</param>
        internal void Parse(List<IdpeAttribute> attributes, Column column, bool isSystemRow, int rowPosition, bool doNotWriteErrorInTraceFile)
        {
            try
            {
                List<SreType> sqlQueryTypes = new List<SreType>();
                for (int a = 0; a < attributes.Count; a++)
                {
                    string value = string.Empty;
                    if (column[attributes[a].Name].IsNull == true)
                    {
                        value = "NULL";
                        column[a].Error = new IdpeMessage(IdpeMessageCodes.IDPE_SUCCESS);
                    }
                    else
                    {
                        value = column[attributes[a].Name].Value;
                    }


                    //if (attributes[a].Name == "AssetId")
                    //    Debugger.Break();

                    SreType SREType = SreTypeFactory.GetInstance(attributes[a].Name, value, attributes[a].Type,
                        attributes[a].Formula, attributes[a].Minimum, attributes[a].Maximum, isSystemRow, rowPosition, this._SQLClientManager, this._DataSourceKeys);



                    column[a].Type = SREType;

                    if (column[attributes[a].Name].IgnoreParsing)
                    {
                        column[a].Error = new IdpeMessage(IdpeMessageCodes.IDPE_SUCCESS);
                        continue;
                    }

                    if ((SREType.Type != AttributeTypes.Referenced) &&
                        (SREType.Type != AttributeTypes.NotReferenced) &&
                        (SREType.Type != AttributeTypes.Generated))
                    {
                        //parse rightway...We dont have SQL Queries to fire
                        //we can override errors for all except 'IsValid'...because if rule fails, we manually override its error msgs
                        //and if IsValid.err msg already populated, we will lose that...
                        if (attributes[a].Name == "IsValid")
                        {
                            IdpeMessage errMsg = SREType.Parse(false);
                            if (column[a].Error == null)
                                column[a].Error = errMsg;
                            else
                                column[a].Error.Message += column[a].Error.Message;
                        }
                        else
                        {
                            IdpeMessage thisResult = SREType.Parse(false);
                            if (column[a].Error == null)
                            {
                                column[a].Error = thisResult;
                            }
                            else
                            {
                                if (thisResult.Code != IdpeMessageCodes.IDPE_SUCCESS)
                                {
                                    string oldErrorMsg = column[a].Error.Message;
                                    column[a].Error = thisResult;
                                    column[a].Error.Message = string.Format("{0},{1}", column[a].Error.Message, oldErrorMsg);
                                }
                            }
                        }


                        //if codeset type, store enum value and code as well
                        if (SREType.Type == AttributeTypes.Codeset)
                        {
                            SreCodeset sreCodeset = SREType as SreCodeset;

                            //if (string.IsNullOrEmpty(column[attributes[a].Name].ValueEnumValue))
                            //    column[attributes[a].Name].ValueEnumValue = column[attributes[a].Name].Value;                        

                            column[attributes[a].Name].ValueEnumCode = sreCodeset.ValueEnumCode;
                            column[attributes[a].Name].ValueEnumValue = sreCodeset.ValueEnumValue;
                            column[attributes[a].Name].ValueEnumReferenceKey = sreCodeset.ReferenceKey;

                        }
                        else
                        {
                            column[a].Value = SREType.Value;   //in case anything updated after parse(at this moment, formatted datetime)
                        }


                    }
                    else
                    {
                        //to be parsed once all others are parsed.
                        sqlQueryTypes.Add(SREType);
                    }

                    column[a].IsNull = SREType.IsNull;
                }

                //we have parsed all other values except with sql queries, lets parse those.
                foreach (SreType item in sqlQueryTypes)
                {
                    //if (item.ColumnName == "OldInvoiceNumber")
                    //    Debugger.Break();

                    Attribute currentAttribute = column[item.ColumnName]; //efficient, instead of calling string indxr multiple times.
                    if ((item.Type == AttributeTypes.Generated) && (!item.IsHavingSqlQuery))
                    {
                        if (string.IsNullOrEmpty(currentAttribute.Value))
                            currentAttribute.Value = GetFormulaResult(item.Formula);

                        IdpeMessage thisResult = item.Parse(false);
                        if (currentAttribute.Error == null)
                        {
                            currentAttribute.Error = thisResult;
                        }
                        else
                        {
                            if (thisResult.Code != IdpeMessageCodes.IDPE_SUCCESS)
                            {
                                string oldErrorMsg = currentAttribute.Error.Message;
                                currentAttribute.Error = thisResult;
                                currentAttribute.Error.Message = string.Format("{0},{1}", currentAttribute.Error.Message, oldErrorMsg);
                            }
                        }

                    }
                    else if ((item.Type == AttributeTypes.NotReferenced) && (string.IsNullOrEmpty(currentAttribute.Value)))
                    {
                        //NotReferenced can not be empty at least.
                        currentAttribute.Error = new IdpeMessage(IdpeMessageCodes.IDPE_REFERENCED_TYPE_DATA_CAN_NOT_BE_NULL);
                        currentAttribute.Error.Message = string.Format(currentAttribute.Error.Message, PrintRowColPosition(item.RecordPosition, item.ColumnName, isSystemRow), currentAttribute.Value, item.ColumnName);
                        continue;
                    }
                    else
                    {
                        string errorMessage = string.Empty;
                        string value = currentAttribute.Value;
                        SqlCommandTypes sqlCommandTypes = SqlCommandTypes.Unknown;
                        string connectionStringKeyName = string.Empty;
                        string SQLQuery = FormatSQLFormula(item.Formula, ref sqlCommandTypes, ref connectionStringKeyName, ref errorMessage);
                        string FormattedSQLQuery = string.Empty;

                        if (errorMessage == string.Empty)
                        {
                            if (connectionStringKeyName != Constants.DefaultConnectionStringKeyName)
                            {
                                item.ConnectionStringKeyName = connectionStringKeyName;
                                //string k_n_type = _ApplicationKeys.GetKeyValue(connectionStringKeyName);
                                IdpeKey key = _DataSourceKeys.GetKey(connectionStringKeyName);
                                item.ConnectionString = key.Value;
                                item.DatabaseType = (IdpeKeyTypes)key.Type;

                            }

                            if (sqlCommandTypes == SqlCommandTypes.SqlCommand)
                            {
                                FormattedSQLQuery = FormatSQLParameters(SQLQuery, value, column, ref errorMessage);
                            }
                            else if (sqlCommandTypes == SqlCommandTypes.StoreProcedure)
                            {
                                //todo
                                //FormattedSQLQuery = 
                            }
                        }

                        if (errorMessage != string.Empty)
                        {
                            //Prepare return error message (and not the technical exception)
                            IdpeMessage returnMessage = new IdpeMessage(IdpeMessageCodes.IDPE_TYPE_DATA_VALIDATION_FAILED_GENERIC);


                            //Write the error into trace with mapId.
                            Guid detailesMapId = Guid.NewGuid();

                            //Do not write in trace, as it is actually not an error, tried first attempt
                            if (!doNotWriteErrorInTraceFile)
                                ExtensionMethods.TraceError(string.Format("{0}:{1}", detailesMapId.ToString(), errorMessage));
                            

                            //set mapId in client error msg
                            returnMessage.Message = string.Format("{0}. Map Id:{1}", returnMessage.Message, detailesMapId.ToString());

                            if (currentAttribute.Error == null)
                            {
                                //send the generic error with mapId
                                currentAttribute.Error = returnMessage;
                            }
                            else
                            {
                                if (currentAttribute.Error.Code != IdpeMessageCodes.IDPE_SUCCESS)
                                {
                                    string oldErrorMsg = currentAttribute.Error.Message;
                                    currentAttribute.Error = returnMessage;
                                    currentAttribute.Error.Message = string.Format("{0},{1}", currentAttribute.Error.Message, oldErrorMsg);
                                }
                            }
                        }
                        else
                        {
                            item.OverrideFormula(FormattedSQLQuery);
                            currentAttribute.Error = item.Parse(false); //SQL Query results are always overridden.
                            currentAttribute.Value = item.Value;
                        }

                    }
                }

                //Parsing done, as we have more data (SQL Parameters) now, lets just format all queries one more time, there might be few more parameters in context.
                FormatAllSQLQueries(column);
            }
            catch (Exception ex)
            {
                Trace.Write(string.Format("Attribute parsing error, row position = {0}{1}{2}", rowPosition, Environment.NewLine, ex.ToString()));
            }
        }
        

        #region Private Methods

        private string FormatSQLFormula(string sqlFormula, ref SqlCommandTypes sqlCommandType, ref string connectionStringKeyName, ref string errorMessage)
        {
            bool invalidFormula = false;
            if ((!sqlFormula.Contains("(")) || (!sqlFormula.Contains("(")) || (!sqlFormula.Contains("|")))
            {
                errorMessage = string.Format("Error while parasing data, SQL query {0} is invalid. Valid SQL Queries are inside =SQL() or =STOREPROC()", sqlFormula);
                return sqlFormula;
            }
            
            
            int start = sqlFormula.IndexOf("(");
            int end = sqlFormula.LastIndexOf(")");

            if ((end < start) || ((end - start) < 5))
                invalidFormula = true;

            string formula = sqlFormula.Substring(0, start);
            if (formula.Equals("=SQL", StringComparison.OrdinalIgnoreCase))
                sqlCommandType = SqlCommandTypes.SqlCommand;
            else if (!formula.Equals("=STOREPROC", StringComparison.OrdinalIgnoreCase))
                sqlCommandType = SqlCommandTypes.StoreProcedure;
            else
                invalidFormula = true;

            if (invalidFormula)
            {
                errorMessage = string.Format("Error while parasing data, SQL query {0} is invalid. Valid SQL Queries are inside =SQL() or =STOREPROC()", sqlFormula);
                return sqlFormula;
            }
            

            string SQLQueryWithConnectionString = sqlFormula.Substring(start +1 , end - start - 1);
            connectionStringKeyName = SQLQueryWithConnectionString.Split("|".ToCharArray())[0];
            string SQLQuery = SQLQueryWithConnectionString.Split("|".ToCharArray())[1];
           
            return SQLQuery;
        }

        private string FormatSQLParameters(string sqlQuery, string value, Column column, ref string errorMessage)
        {
            if (sqlQuery.IndexOf("{") == -1) return sqlQuery;   //nothing to format

            string pattern = @"{\w+}|{\=\w+\(\w+\)}";   //{APPLICATIONNAME}|{=EnumValue(ProductCode)}
            MatchCollection parameters = Regex.Matches(sqlQuery, pattern);

            bool goNext = false;
            foreach (Match parameter in parameters)
            {
                goNext = false;
                //If SQL query parameter has self value in it.
                if (parameter.Value.ToUpper() == "{VALUE}")
                {
                    sqlQuery = sqlQuery.Replace(parameter.Value, value);
                    goNext = true;
                }

                if (goNext) continue;               

                string overriddenValue = string.Empty;

                //If SQL query has any value from current 'getting processed' attributes
                foreach (Attribute attribute in column)
                {
                    if (CompareParameter(parameter.Value, attribute, ref overriddenValue))
                    {
                        sqlQuery = sqlQuery.Replace(parameter.Value, overriddenValue);
                        goNext = true;
                        break;
                    }
                }
                if (goNext) continue;

                //If SQL query has any value from any other available attributes.
                //Generally these are common attributes, associated with 'System Application'
                if (_SystemAttributes != null)
                {
                    foreach (Attribute attribute in this._SystemAttributes)
                    {
                        if (CompareParameter(parameter.Value, attribute, ref overriddenValue))
                        {
                            sqlQuery = sqlQuery.Replace(parameter.Value, overriddenValue);
                            goNext = true;
                            break;
                        }
                    }
                }
                if (goNext) continue;


                if (_OtherAvailableAttributes != null)
                {
                    foreach (Attribute attribute in this._OtherAvailableAttributes)
                    {
                        if (CompareParameter(parameter.Value, attribute, ref overriddenValue))
                        {
                            sqlQuery = sqlQuery.Replace(parameter.Value, overriddenValue);
                            goNext = true;
                            break;
                        }
                    }
                }

                if (goNext) continue;

                //If SQL query has any parameter from our param list
                PropertyInfo[] propertyInfos = this._Parameters.GetType().GetProperties();
                foreach (PropertyInfo info in propertyInfos)
                {
                    if (info.CanRead)
                    {
                        if (parameter.Value.ToUpper() == "{" + info.Name.ToUpper() + "}")
                        {
                            object propertyValue = info.GetValue(this._Parameters, null);
                            if (propertyValue != null)
                            {
                                sqlQuery = sqlQuery.Replace(parameter.Value, propertyValue.ToString());
                                goNext = true;
                                break;
                            }
                        }
                    }
                }
                if (goNext) continue;                

                //If SQL query has any parameter from AVPair
                IDictionaryEnumerator enumerator = this._Parameters.AttributeValuePair.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string name = (string)enumerator.Key;
                    string paramValue = (string)enumerator.Value;
                    if (parameter.Value.Equals("{" + name + "}",StringComparison.OrdinalIgnoreCase))
                    {
                        sqlQuery = sqlQuery.Replace(parameter.Value, paramValue);
                        goNext = true;
                        break;
                    }
                }
                

            }
            //if still any parameter remains, throw error
            if (sqlQuery.IndexOf("{") > -1)
            {
                errorMessage = string.Format("Error while parasing data, SQL query {0} has some invalid parameters.", sqlQuery);
                sqlQuery = string.Empty;
            }

            return sqlQuery;
        }

        private bool CompareParameter(string param, Attribute attribute, ref string overridenValue)
        {
            string pattern = @"\w+";    //{=EnumValue(ProductCode)}
            string genFormula = string.Empty;
            string genFormulaInputParam = string.Empty;

            MatchCollection parameters = Regex.Matches(param, pattern);
            if (parameters.Count == 1)
            {
                if(param.Length >= 3)
                    genFormulaInputParam = param.Substring(1, param.Length - 2);
            }
            else if (parameters.Count == 2)
            {
                genFormula = parameters[0].Value;
                genFormulaInputParam = parameters[1].Value;
            }            

            if (!(attribute.Name.Equals(genFormulaInputParam, StringComparison.OrdinalIgnoreCase)))
                return false;

            if (genFormula.Equals("EnumValue",StringComparison.OrdinalIgnoreCase))
                overridenValue = attribute.ValueEnumValue;                    
            else if (genFormula.Equals("EnumCode", StringComparison.OrdinalIgnoreCase))
                overridenValue = attribute.ValueEnumCode.ToString();
            else
                overridenValue = attribute.Value;

            return true;
        }

        private string GetFormulaResult(string formula)
        {
            formula = formula.Substring(1);            
            string value = string.Empty;

            if (formula.IndexOf("SPACE",StringComparison.OrdinalIgnoreCase) > -1)
            {
                string strLength = formula.Substring("SPACE".Length + 1, formula.Length - ("SPACE".Length + 2));
                int length = 0;
                int.TryParse(strLength, out length);
                return "".PadRight(length, ' ');
            }
            else if (formula.IndexOf("HARDCODED", StringComparison.OrdinalIgnoreCase) > -1)
            {
                value = formula.Substring("HARDCODED".Length + 1, formula.Length - ("HARDCODED".Length + 2));                
            }
            else
            {
                if (!this._Parameters.AttributeValuePair.ContainsKey(formula)) return string.Empty;
                value = this._Parameters.AttributeValuePair[formula];                
            }

            return value != null ? value : string.Empty;
            
        }
       

        private void FormatAllSQLQueries(Column availableAttributes)
        {
            try
            {
                Assembly assembly = this.GetType().Assembly;
                string resourceName = (from n in assembly.GetManifestResourceNames()
                                       where n.Contains("SQLQueries")
                                       select n).SingleOrDefault();
                if (string.IsNullOrEmpty(resourceName))
                    return;

                resourceName = resourceName.Substring(0, resourceName.LastIndexOf('.'));
                ResourceManager resourceManager = new ResourceManager(resourceName, assembly);
                ResourceSet resources = resourceManager.GetResourceSet(EyediaCoreConfigurationSection.CurrentConfig.CurrentCulture, true, true);
                IDictionaryEnumerator enumerator = resources.GetEnumerator();

                Information.SQLQueries = new Dictionary<string, string>();
                string errorMessage = string.Empty;
                while (enumerator.MoveNext())
                {
                    string name = (string)enumerator.Key;
                    string query = FormatSQLParameters((string)enumerator.Value, string.Empty, availableAttributes, ref errorMessage);
                    Information.SQLQueries.Add(name, query);
                }
            }
            catch { }

        }

        public string PrintRowColPosition(int recordPosition, string columnName, bool isSystemRow = false)
        {
            return string.Format("{0}[{1}][{2}]: ", isSystemRow ? "RowSystem" : "Row", recordPosition, columnName);
        }
        #endregion Private Methods
    }
}





