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
using System.Threading.Tasks;
using System.Data;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using System.Configuration;

namespace Eyedia.IDPE.Services
{
    public class GlobalSearch
    {
        public string ConnectionString { get; private set; }

        public GlobalSearch(string connectionstring = null)
        {
            ConnectionString = connectionstring;
            if (string.IsNullOrEmpty(ConnectionString))
                ConnectionString = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].ToString();
        }
        public List<GSearchResultCube> Search(string searchText, int dataSourceId = 0)
        {
            List<GSearchResultCube> results = new List<GSearchResultCube>();

            DataTable table = new DataTable();
            try
            {
                string sql = "select c.table_name, c.column_name ";
                sql += "from information_schema.columns as c ";
                sql += "inner join information_Schema.tables as t on t.table_name = c.table_name ";
                sql += "where (c.data_Type in ('char', 'nchar', 'varchar', 'nvarchar', 'text', 'ntext')) and (t.table_type = 'table') ";

                bool isErrored = false;
                table = new SqlClientManager(ConnectionString, IdpeKeyTypes.ConnectionStringSqlCe).ExecuteQueryAndGetDataTable(sql, ref isErrored);

                foreach(DataRow dr in table.Rows)
                {
                    string dynSql = "select [" + dr["column_name"] + "] as Value";
                    if (dr["table_name"].ToString() == "IdpeAttribute")
                        dynSql += ", [AttributeId] as ReferenceId, '' as Param1, [Name] as Name";
                    else if (dr["table_name"].ToString() == "IdpeKey")
                        dynSql += ", [KeyId] as ReferenceId, [Type] as Param1, [Name] as Name";
                    else if (dr["table_name"].ToString() == "IdpeRule")
                        dynSql += ", [Id] as ReferenceId, [Name] as Param1, [Name] as Name";
                    else
                        dynSql += ", '0' as ReferenceId, '' as Param1, '' as Name";

                    dynSql += " from [" + dr["table_name"].ToString() + "]";
                    dynSql += " where [" + dr["column_name"].ToString() + "] like '%" + searchText + "%'";

                    DataTable result = new SqlClientManager(ConnectionString, IdpeKeyTypes.ConnectionStringSqlCe).ExecuteQueryAndGetDataTable(dynSql, ref isErrored);

                    foreach(DataRow r in result.Rows)
                    {
                        results.Add(new GSearchResultCube(int.Parse(r["ReferenceId"].ToString()), r["Name"].ToString(), dr["table_name"].ToString(), 
                            dr["column_name"].ToString(), r["Value"].ToString(), r["Param1"].ToString()));
                    }

                }

                return FormatResults(results, dataSourceId);
            }
            catch(Exception ex)
            {

            }
            return results;
        }

        private List<GSearchResultCube> FormatResults(List<GSearchResultCube> results, int dataSourceId)
        {
            List<GSearchResultCube> formattedResults = new List<GSearchResultCube>();
            foreach (GSearchResultCube result in results)
            {
                GSearchResultCube formattedResult = result;
                switch (result.TableName)
                {
                    case "IdpeAttribute":
                        result.DataSource = "0-General";
                        result.Type = "Attribute";
                        result.Where = "Master";
                        formattedResult = FormatResult(result);
                        //formattedResult = FormatResultAttribute(formattedResults, result);
                        break;

                    case "IdpeKey":
                        result.Type = "Key";
                        formattedResult = FormatResultKey(result);
                        break;

                    case "IdpeRule":
                        result.Type = "Rule";
                        formattedResult = FormatResultRule(result);
                        break;

                    default:
                        result.Type = "General";
                        formattedResult = FormatResult(result);
                        break;
                }

                formattedResults.Add(formattedResult);
            }

            formattedResults = formattedResults.OrderBy(r => r.DataSource).ToList();

            if (dataSourceId > 0)
            {
                string dataSourceName = new Manager().GetApplicationName(dataSourceId);
                formattedResults = formattedResults.Where(r => r.DataSource == dataSourceName).ToList();
            }
            return formattedResults;
        }

        private GSearchResultCube FormatResult(GSearchResultCube result)
        {
            return result;
        }

        private GSearchResultCube FormatResultRule(GSearchResultCube result)
        {
            bool isErrored = false;
            string info = new SqlClientManager(ConnectionString, IdpeKeyTypes.ConnectionStringSqlCe)
                .ExecuteQuery("select name, rulesettype from idpedatasource ds inner join idperuledatasource rds on rds.datasourceid = ds.id where ruleid = " + result.ReferenceId
                , ref isErrored);

            if ((info == "NULL") || (string.IsNullOrEmpty(info)))
            {
                result.DataSource = "0-General";
                result.Where = "Master";
            }
            else
            {
                info = info.Replace("NULL", string.Empty);
                result.DataSource = info.Split("|".ToCharArray())[0];
                string strRuleType = info.Split("|".ToCharArray())[1];

                int ruleType = 0;
                if (int.TryParse(strRuleType, out ruleType))
                {
                    RuleSetTypes ruleSetTypes = (RuleSetTypes)ruleType;
                    result.Param1 = ruleSetTypes.ToString();
                    result.Where = "Rule - " + ruleSetTypes.ToString();
                }
                else
                {
                    result.Where = "Unknown";
                }

            }

            result.Value = string.Empty;
            return result;
        }

        private GSearchResultCube FormatResultKey(GSearchResultCube result)
        {           
            bool isErrored = false;
            result.DataSource = new SqlClientManager(ConnectionString, IdpeKeyTypes.ConnectionStringSqlCe)
                .ExecuteQuery("select name from idpedatasource ds inner join idpekeydatasource kds on kds.datasourceid = ds.id where keyid = " + result.ReferenceId
                , ref isErrored);

            int keyType = 0;
            if (int.TryParse(result.Param1, out keyType))
            {
                IdpeKeyTypes idpeKeyType = (IdpeKeyTypes)keyType;
                result.Param1 = idpeKeyType.ToString();

                if (result.Param1.Contains("OutputWriter"))
                    result.Where = "Output Writer";
                else if (result.Param1 == "Custom")
                    result.Where = "Custom - " + result.ReferenceName;
                else
                    result.Where = result.Param1;
            }
            
            return result;
        }

        private GSearchResultCube FormatResultAttribute(List<GSearchResultCube> formattedResults, GSearchResultCube result)
        {
            result.Type = "";
            bool isErrored = false;
            DataTable table = new SqlClientManager(ConnectionString, IdpeKeyTypes.ConnectionStringSqlCe)
                .ExecuteQueryAndGetDataTable("select name, issystem from idpedatasource ds inner join idpeattributedatasource ads on ads.datasourceid = ds.id where attributeId = " + result.ReferenceId
                , ref isErrored);

            foreach(DataRow row in table.Rows)
            {
                GSearchResultCube cube = result;
                cube.DataSource = row[0].ToString();
                cube.Where = row[1].ToString() == "True" ? "System Data Source" : "Data Source";
                cube.Value = string.Empty;
                formattedResults.Add(cube);
            }
            return result;
        }
    }
}


