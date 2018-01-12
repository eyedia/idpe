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
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;


namespace Eyedia.IDPE.Services
{
	public class Parameters
	{
        private int _DataSourceId;
        string _DataSourceName;
        SqlClientManager _SQLClientManager;
        public Parameters(int dataSourceId, string dataSourceName, SreKey generateParamFromDatabase, SqlClientManager sqlClientManager)
        {
            this._DataSourceId = dataSourceId;
            this._DataSourceName = dataSourceName;
            this._SQLClientManager = sqlClientManager;
            this._AttributeValuePair = new Dictionary<string, string>();
            GenerateAttributeValuePair();

            if (generateParamFromDatabase != null)
            {
                if (generateParamFromDatabase.Value.ParseBool())
                    RetrieveFromDB();
            }
        }

        #region Public Read Only Properties

        public int ApplicationId { get { return _DataSourceId; } }
        public string ApplicationName { get { return _DataSourceName; } }
        
        Dictionary<string, string> _AttributeValuePair;
        public Dictionary<string, string> AttributeValuePair { get { return _AttributeValuePair; } }
        #endregion Public Read Only Properties

        #region Private Helper Methods
        private void RetrieveFromDB()
        {
            try
            {
                Dictionary<string, string> thisParams = _SQLClientManager.GenerateParameters(this._DataSourceId);

                foreach ( KeyValuePair<string,string> entry in thisParams)
                {
                    this._AttributeValuePair.Add(entry.Key, entry.Value);
                }
            }
            catch (Exception ex)
            {
                ExtensionMethods.TraceError("Error while parsing generated parameters " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private void GenerateAttributeValuePair()
        {
            this._AttributeValuePair.Add("SPACE", string.Empty);
            this._AttributeValuePair.Add("HARDCODED", string.Empty);
            this._AttributeValuePair.Add("BLANK", string.Empty);
            this._AttributeValuePair.Add("APPLICATIONID", this._DataSourceId.ToString());
            this._AttributeValuePair.Add("APPLICATIONNAME", this._DataSourceName);
            this._AttributeValuePair.Add("CURRENTDATETIME", DateTime.Now.ToDBDateTimeFormat());
            this._AttributeValuePair.Add("CURRENTDATE", DateTime.Now.ToDBDateFormat());
        }

        #endregion Private Helper Methods
    }
}






