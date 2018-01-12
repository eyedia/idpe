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
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Services
{
    public class ColumnMap : List<ColumnMapInfo>
    {
        #region Properties
        public int DataSourceId { get; private set; }        
        private string _RawString;
        public string RawString
        {
            get { return _RawString; }
            set { _RawString = value; BindFromRawString(); }
        }
        public List<IdpeKey> ConnectionKeys { get; private set; }
        public  IdpeKey ConnectionKey { get; set; }
        public string ConnectionKeyRunTime { get; set; }
        public string HeaderRawString { get; set; }        
        public int NoOfRecordsinBatch = 0;
        public string TargetTableName { get; set; }

        #endregion Properties

        /// <summary>
        /// Used for new mapping information
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <param name="connectionKey"></param>
        /// <param name="connectionKeys"></param>
        /// <param name="tableName"></param>
        /// <param name="totalRecordsInBatch"></param>
        public ColumnMap(int dataSourceId, IdpeKey connectionKey = null, List<IdpeKey> connectionKeys = null, string tableName = null, int totalRecordsInBatch = 0)
        {
            this.DataSourceId = dataSourceId;
            this.ConnectionKey = connectionKey;            
            this.TargetTableName = tableName;
            Init();
            if (totalRecordsInBatch == 0)
                this.NoOfRecordsinBatch = 1;
            else
                this.NoOfRecordsinBatch = totalRecordsInBatch;            
            BindNew();
        }

        /// <summary>
        /// Used for existing mapping information
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <param name="rawString"></param>
        public ColumnMap(int dataSourceId, string rawString)
        {
            this.DataSourceId = dataSourceId;
            Init();
            this.RawString = rawString;

        }
        
        private void Init(List<IdpeKey> connectionKeys = null)
        {
            if (connectionKeys != null)
                ConnectionKeys = connectionKeys;
            else
                ConnectionKeys = new Manager().GetDataSourceKeysConnectionStrings(DataSourceId, false);

        }
       
        private void BindNew()
        {
            DataTable dtColumns = new SqlClientManager(ConnectionKey.Value, (SreKeyTypes)ConnectionKey.Type).GetTableColumnNames(TargetTableName);
            if (dtColumns != null)
            {
                List<string> d = new List<string>();
                foreach (DataRow row in dtColumns.Rows)
                {
                    d.Add(row["Datatype"].ToString());
                    AttributeTypes aType = Eyedia.IDPE.Common.Utility.GetAttributeTypeFromDatabaseType(row["Datatype"].ToString());
                    ColumnMapInfo oneColumn = new ColumnMapInfo(this.DataSourceId, row["ColumnName"].ToString() + "|" + (int)aType + ",");

                    this.Add(oneColumn);
                }
                
            }
        }
      
        private void BindFromRawString()
        {
            MatchCollection matches = GetConfigCollection(_RawString);

            if (matches.Count <= 0)
            {
                ExtensionMethods.TraceInformation("Invalid key value - " + _RawString);
            }
            else
            {
                //extract header
                if (matches.Count >= 1)
                {
                    HeaderRawString = matches[0].Groups[1].Value;
                    string[] DBConDetails = (HeaderRawString).Split(new string[] { "," }, StringSplitOptions.None);
                    this.ConnectionKey = ConnectionKeys.GetKey(DBConDetails[0]);
                    this.ConnectionKeyRunTime = DBConDetails[1];
                    this.TargetTableName = DBConDetails[2].ToString();
                    this.NoOfRecordsinBatch = (int)DBConDetails[3].ToString().ParseInt();
                }
                //extract Column Details
                if (matches.Count > 1)
                {
                    for (int i = 1; i < matches.Count; i++)
                    {
                        ColumnMapInfo oneColumn = new ColumnMapInfo(this.DataSourceId, matches[i].Groups[1].Value);
                        this.Add(oneColumn);
                    }
                }

            }
        }
        static MatchCollection GetConfigCollection(string timerServiceConfig)
        {
            timerServiceConfig = timerServiceConfig.Replace(Environment.NewLine, "");
            var pattern = @"\[(.*?)\]";
            return Regex.Matches(timerServiceConfig, pattern);
        }
    }
}


