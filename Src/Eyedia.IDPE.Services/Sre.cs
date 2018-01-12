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
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Workflow.Runtime;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Collections;
using System.Text;
using Eyedia.Core.Data;
using System.Configuration;

namespace Eyedia.IDPE.Services
{
    #region IIdpe

    [ServiceContract]
	public interface IIdpe
	{
        [OperationContract]
        string StartPullers();

        [OperationContract]
        string StopPullers();

        [OperationContract]
        string GetPullersStatus();

        //[OperationContract]
        //string InitializeJob(int dataSourceId, string dataSourceName, string processingBy);

        [OperationContract]
        string ProcessJob(int dataSourceId, string dataSourceName, string processingBy, string inputData);

        [OperationContract]
        object ProcessObjects(int dataSourceId, string dataSourceName, string processingBy, List<object> inputData, string overridenMapping);

        [OperationContract]
        string ProcessXml(string xmlInput, string processingBy);     

        [OperationContract]
        string GetActiveJobs();

        [OperationContract]
        string ClearCache();
        
        [OperationContract]
        string ClearCacheDataSource(int dataSourceId, string dataSourceName);

        [OperationContract]
        string ClearCacheRule(int ruleId, string ruleName);

        [OperationContract]
        string RefreshGlobalEvents();

        [OperationContract]
        string ClearLog();

        [OperationContract]
        string StopSqlPuller(int dataSourceId);

        [OperationContract]
        string StartSqlPuller(int dataSourceId);

        [OperationContract]
        string IsTemporarilyStopped(int dataSourceId);
    }

    #endregion IIdpe

    #region Idpe


    public class Idpe : IIdpe
    {
        /// <summary>
        /// This is mainly to handle instantiation of custom trace listner (if possible). Incorrect file path or permission issue may fail
        /// instantiating a custom trace listner if instantiated from web.config.system.diagonstic settings. Most of the methods will throw error
        /// as they have Trace statements in it, and no operation will be performed. This place is to intialize trace listner silently, 
        /// else eat the exception and keep quite.
        /// </summary>
        static Idpe()
        {
            try
            {
#if WEB
                AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);
#endif
                Init();
                Trace.Flush();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.ToString() + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                ExtensionMethods.TraceError(errorMessage);
                Trace.Flush();
            }
        }


        /// <summary>
        /// This method initializes all 'pull' type applications
        /// </summary>
        /// <returns></returns>
        public string StartPullers()
        {

            Registry.Instance.Pullers = new Pullers();
            //Registry.Instance.Pullers.Start(); moved to delayed init timer
            Trace.Flush();
            return "1";
        }

        /// <summary>
        /// This method stops all 'pull' type applications
        /// </summary>
        /// <returns></returns>
        public string StopPullers()
        {
            if (Registry.Instance.Pullers != null)
                Registry.Instance.Pullers.Stop();
            Trace.Flush();
            return "1";
        }

        /// <summary>
        /// Gets Pullers status
        /// </summary>
        /// <returns></returns>
        public string GetPullersStatus()
        {
            if (Registry.Instance.Pullers != null)
                return Registry.Instance.Pullers.LocalFileSystemWatcherIsRunning.ToString();
            Trace.Flush();
            return "false";
        }

        /// <summary>
        /// Start called when 'Caller' wants to parse one row at a time. Initiate a new job based on input parameters.
        /// Either dataSourceId should contain a valid application id or dataSourceName should be a valid application name.
        /// </summary>
        /// <param name="dataSourceId">Should be a valid application id. If this is filled, then dataSourceName is optional.</param>
        /// <param name="dataSourceName">Should be a valid application name. If this is filled, then dataSourceId is optional.</param>
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like 'a126042, Deb'jyoti Das', let caller decide the user name.</param>
        /// <returns>Retruns process identifier as GUID or string.empty (fail condition)</returns>
        //public string InitializeJob(int dataSourceId, string dataSourceName, string processingBy)
        //{            
        //    return new JobProcessor().InitializeJob(dataSourceId, dataSourceName, processingBy);
        //}


        /// <summary>
        /// This method is used when 'Caller' wants to parse complete file at a time.
        /// </summary>
        /// <param name="dataSourceId">Data source id (either id or name is needed)</param>
        /// <param name="dataSourceName">Data source name (either id or name is needed)</param>
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like 'a126042, Deb'jyoti Das', let caller decide the user name.</param>
        /// <param name="inputData">File content.</param>
        /// <returns>Output xml. for details, read documentation.</returns>
        public string ProcessJob(int dataSourceId, string dataSourceName, string processingBy, string inputData)
        {
            //todo - with inputData

            return new JobProcessor().ProcessJob(dataSourceId, dataSourceName, processingBy).ToString();
        }


        /// <summary>
        /// This method is used when 'Caller' wants to parse complete file at a time.
        /// </summary>
        /// <param name="dataSourceId">Data source id (either id or name is needed)</param>
        /// <param name="dataSourceName">Data source name (either id or name is needed)</param>      
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like 'a126042, Deb'jyoti Das', let caller decide the user name.</param>
        ///<param name="inputData">List of custom objects</param>
        ///<param name="overridenMapping">Comma separated additional mapping override information. For example if "EmpId" from object to be mapped with "EmployeeId" of attribute, then "EmpId=EmployeeId,Ename=EmployeeName"</param>
        /// <returns>Output object. for details, read documentation.</returns>
        public object ProcessObjects(int dataSourceId, string dataSourceName, string processingBy, List<object> inputData, string overridenMapping)
        {
            JobProcessor jobProcessor = new JobProcessor();
            string jobId = jobProcessor.InitializeJob(dataSourceId, dataSourceName, processingBy);
            return new JobProcessor().ProcessJob(jobId, processingBy, inputData, overridenMapping);
        }


        /// <summary>
        /// This method is used when 'Caller' wants to parse complete file at a time.
        /// </summary>        
        /// <param name="xmlInput">Input data in xml format.</param>        
        /// <param name="processingBy">user name who is processing, we could get this from context, but for scenario like 'a126042, Deb'jyoti Das', let caller decide the user name.</param>
        /// <returns>Output xml. for details, read documentation.</returns>
        public string ProcessXml(string xmlInput, string processingBy)
        {
            return new JobProcessor().ProcessXml(xmlInput, processingBy).ToString();
        }

        public string GetActiveJobs()
        {
            return new JobProcessor().GetActiveJobs();
        }

        #region Private Methods


        static void Init()
        {
            SetupTrace.SetupTraceListners(Information.EventLogSource, Information.EventLogName);
            LoadCodeSets();
        }

        static void LoadCodeSets()
        {
            string connectedTo = "Unknown";
            if (EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType == DatabaseTypes.SqlCe)
                connectedTo = ConfigurationManager.ConnectionStrings["cs"].GetSdfFileName();
            else if (EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType == DatabaseTypes.SqlCe)
                connectedTo = string.Format("{0}\\{1}", ConfigurationManager.ConnectionStrings["cs"].GetSqlServerName(),
                    ConfigurationManager.ConnectionStrings["cs"].GetSqlServerDatabaseName());

                ExtensionMethods.TraceInformation("Connected to = {0}, Database type = '{1}'", 
                connectedTo,
                EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType.ToString());
            ExtensionMethods.TraceInformation("Initializing code sets.");
            Registry.Instance.CodeSets = Cache.Instance.Bag["codesets.default"] as List<CodeSet>;
            if (Registry.Instance.CodeSets == null)
            {
                Registry.Instance.CodeSets = CoreDatabaseObjects.Instance.GetCodeSets();
                Cache.Instance.Bag.Add("codesets.default", Registry.Instance.CodeSets);
            }

            ExtensionMethods.TraceInformation("Done. {0} code sets found.", Registry.Instance.CodeSets.Count);
        }

        public string ClearCache()
        {
            Cache.Instance.Bag.Clear();
            return "true";
        }

        public string ClearCacheDataSource(int dataSourceId, string dataSourceName)
        {
            if (Cache.Instance.Bag[dataSourceId] != null)
                Cache.Instance.Bag.Remove(dataSourceId);

            if (Cache.Instance.Bag[dataSourceName] != null)
                Cache.Instance.Bag.Remove(dataSourceName);

            if (Cache.Instance.Bag[dataSourceId + ".attributes"] != null)
                Cache.Instance.Bag.Remove(dataSourceId + ".attributes");

            if (Cache.Instance.Bag[dataSourceId + ".attributessystem"] != null)
                Cache.Instance.Bag.Remove(dataSourceId + ".attributessystem");

            if (Cache.Instance.Bag[dataSourceId + ".keys"] != null)
                Cache.Instance.Bag.Remove(dataSourceId + ".keys");

            if (Cache.Instance.Bag[dataSourceId + ".rules"] != null)
                Cache.Instance.Bag.Remove(dataSourceId + ".rules");

            return "true";
        }

        public string ClearCacheRule(int ruleId, string ruleName)
        {
            List<IdpeDataSource> dataSources = new Manager().GetDataSources(1);
            foreach (IdpeDataSource ds in dataSources)
            {
                if (Cache.Instance.Bag[ds.Id + ".rules"] != null)
                    Cache.Instance.Bag.Remove(ds.Id + ".rules");
            }
            return "true";
        }

        public string RefreshGlobalEvents()
        {
            Registry.Instance.RefreshGlobalEvents();
            return "true";
        }

        public string ClearLog()
        {
            ExtensionMethods.TraceInformation("Cleaing log!");
            Trace.Flush();
            SetupTrace.Clear(Information.EventLogSource, Information.EventLogName);
            ExtensionMethods.TraceInformation("New log created.");
            Trace.Flush();
            return "true";
        }

        public string StopSqlPuller(int dataSourceId)
        {
            if (Registry.Instance.Pullers.SqlWatchers.ContainsKey(dataSourceId))
                Registry.Instance.Pullers.SqlWatchers[dataSourceId].IsTemporarilyStopped = true;

            return "true";
        }

        public string StartSqlPuller(int dataSourceId)
        {
            if (Registry.Instance.Pullers.SqlWatchers.ContainsKey(dataSourceId))
                Registry.Instance.Pullers.SqlWatchers[dataSourceId].IsTemporarilyStopped = false;

            return "true";
        }

        public string IsTemporarilyStopped(int dataSourceId)
        {
            if (Registry.Instance.Pullers.SqlWatchers.ContainsKey(dataSourceId))
                return Registry.Instance.Pullers.SqlWatchers[dataSourceId].IsTemporarilyStopped ? "1" : "0";

            return "0";
        }

        #endregion Private Methods
    }

    #endregion Idpe
}






