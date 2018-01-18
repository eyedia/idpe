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
using Eyedia.IDPE.DataManager;
using System.IO;
using Eyedia.Core;
using System.Diagnostics;
using Eyedia.Core.Data;
using System.Configuration;
using Eyedia.IDPE.Common;
using System.Data;
using System.Globalization;
using System.Runtime.Serialization;

namespace Eyedia.IDPE.Services
{
    [DataContract]
    /// <summary>
    /// This class is used to facilitate different key values for different envrionments, e.g. DEV, UAT or PROD
    /// </summary>
    public class DataSourcePatch
    {
        public DataSourcePatch()
        {
            this.Keys = new List<IdpeKey>();
            this.Rules = new List<IdpeRule>();
            this.CodeSets = new List<CodeSet>();
        }

        public DataSourcePatch(string fileName = null, string serializedObject = null, int version = 0)
        {
            if ((fileName == null) && (serializedObject == null))
            {
                this.Keys = new List<IdpeKey>();
                this.Rules = new List<IdpeRule>();
                this.CodeSets = new List<CodeSet>();
            }
            else if (fileName != null)
            {
                this.FileName = fileName;
                LoadFromFile(fileName);             
            }
            else if (serializedObject != null)
            {
                LoadFromSerializedObject(serializedObject);             
            }

            Version = version;
        }

        [DataMember]
        public int Version { get; private set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FileName { get; private set; }

        [DataMember]
        public List<IdpeKey> Keys { get; set; }

        [DataMember]
        public List<IdpeRule> Rules { get; set; }

        [DataMember]
        public List<CodeSet> CodeSets { get; set; }

        void LoadFromFile(string fileName)
        {
            string serializedObject = string.Empty;
            using (StreamReader sr = new StreamReader(fileName))
            {
                serializedObject = sr.ReadToEnd();
                sr.Close();
            }

            LoadFromSerializedObject(serializedObject);
        }

        void LoadFromSerializedObject(string serializedObject)
        {            
            DataSourcePatch dsp = serializedObject.Deserialize<DataSourcePatch>();
            this.Name = dsp.Name;
            this.FileName = FileName;
            this.Keys = dsp.Keys;
            this.Rules = dsp.Rules;
            this.CodeSets = dsp.CodeSets;
        }


        public void Export(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(Export());
                sw.Close();
            }
        }

        public string Export()
        {            
            return this.Serialize();                
        }

        public void Import()
        {
            Trace.TraceInformation("Initializing data manager...");
            Manager dbMgr = new Manager();
            if ((this.Keys.Count > 0)
                && (!string.IsNullOrEmpty(this.Name)))
            {
                #region Key with DataSource

                int dataSourceId = dbMgr.GetApplicationId(this.Name);
                Trace.TraceInformation("Done. The id of {0} in target database is {1}", this.Name, dataSourceId);
                if (dataSourceId == 0)
                {
                    Trace.TraceInformation("Error! No data source exist in target database with name {0}. Aborting...", this.Name);
                    return;
                }

                foreach (var key in this.Keys)
                {
                    Trace.TraceInformation("Inserting:Key.Name={0}, Key.Value={1}, Key.Type={2}"
                        , key.Name, key.Value, key.Type);
                    dbMgr.Save(key, dataSourceId);
                    Trace.TraceInformation("Done!");
                }

                #endregion Key with DataSource
            }
            else if ((this.Keys.Count > 0)
            && (string.IsNullOrEmpty(this.Name)))
            {
                #region Only Keys

                foreach (var key in this.Keys)
                {
                    Trace.TraceInformation("Inserting:Key.Name={0}, Key.Value={1}, Key.Type={2}"
                        , key.Name, key.Value, key.Type);
                    dbMgr.Save(key);
                    Trace.TraceInformation("Done!");
                }

                #endregion Only Keys
            }

            #region Rules

            foreach (IdpeRule rule in this.Rules)
            {
                Trace.TraceInformation("Inserting:Rule.Name={0}", rule.Name);
                new VersionManager().KeepVersion(VersionObjectTypes.Rule, rule.Id);
                dbMgr.Save(rule);
                Trace.TraceInformation("Done!");
            }

            #endregion Rules

            #region CodeSets

            foreach (CodeSet codeSet in this.CodeSets)
            {
                Trace.TraceInformation("Inserting:CodeSet.Name={0}", codeSet.Code);
                CoreDatabaseObjects.Instance.Save(codeSet);
                Trace.TraceInformation("Done!");
            }

            #endregion CodeSets

        }       


    }
}


