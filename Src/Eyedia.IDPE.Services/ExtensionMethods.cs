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
using System.Data;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using System.Diagnostics;
using Eyedia.IDPE.DataManager;
using System.Data.Linq;

namespace Eyedia.IDPE.Services
{
    public static class ExtensionMethods
    {
        public static SreFileSystemWatcher Get(this Dictionary<int, SreFileSystemWatcher> sreFileSystemWatchers, int dataSourceId)
        {
            SreFileSystemWatcher sreFileSystemWatcher = null;
            sreFileSystemWatchers.TryGetValue(dataSourceId, out sreFileSystemWatcher);
            return sreFileSystemWatcher;
        }

        public static void ThrowErrorIfNull(this WorkerData data, string activityName = null)
        {
            if (data == null)
            {
                throw new Exception(string.Format("A valid instance of 'Data' object has not been passed! Please make sure that a valid instance of 'WorkerData' has been passed as argument to this activity '{0}'"
                    , string.IsNullOrEmpty(activityName) ? string.Empty : activityName));
            }
        }

        public static void ThrowErrorIfNull(this Job job, string activityName = null)
        {
            if (job == null)
            {
                throw new Exception(string.Format("A valid instance of 'Job' object was not passed! Please make sure that a valid instance of 'Job' has been passed as argument to this activity '{0}'"
                    , string.IsNullOrEmpty(activityName) ? string.Empty : activityName));
            }
        }

        public static void WriteVerboseLine(this SreTraceLogWriter traceLog, string message)
        {
            if (Information.TraceFilter.HasFlag(SourceLevels.Verbose))
            {
                traceLog.WriteLine(message);
            }
        }

        public static void WriteVerbose(this SreTraceLogWriter traceLog, string message)
        {
            if (Information.TraceFilter.HasFlag(SourceLevels.Verbose))
            {
                traceLog.Write(message);
            }
        }

        public static string Lookup(this DataSource dataSource, string connectionStringKeyName, string query)
        {
            IdpeKey connectionStringKey = dataSource.Keys.GetKey(connectionStringKeyName);
            if (connectionStringKey == null)
                throw new Exception(string.Format("Can not perform lookup operation, the connection string was null! Connection string key name was '{0}'"
                    , connectionStringKeyName));

            bool isErrored = false;

            string result = new SqlClientManager(string.Empty, IdpeKeyTypes.ConnectionStringSqlServer)
                .ExecuteQuery(connectionStringKey.Value, (IdpeKeyTypes)connectionStringKey.Type, query, ref isErrored);
            TraceInformation("Lookup:'{0}', Result:{1}", query, result);
            return result;
        }

        public static int ExecuteNonQuery(this DataSource dataSource, string connectionStringKeyName, string query, bool silent = false)
        {
            IdpeKey connectionStringKey = dataSource.Keys.GetKey(connectionStringKeyName);
            if (connectionStringKey == null)
                throw new Exception(string.Format("Can not execute non query, the connection string was null! Connection string key name was '{0}'"
                    , connectionStringKeyName));

            int result = new SqlClientManager(string.Empty, IdpeKeyTypes.ConnectionStringSqlServer)
                .ExecuteNonQuery(query, silent, connectionStringKey.Value, (IdpeKeyTypes)connectionStringKey.Type);
            if (!silent)
                TraceInformation("ExecuteNonQuery:'{0}', Result:{1}", query, result);
            return result;
        }

        public static DataTable LoadDataTable(this DataSource dataSource, string connectionStringKeyName, string query)
        {
            IdpeKey connectionStringKey = dataSource.Keys.GetKey(connectionStringKeyName);
            if (connectionStringKey == null)
                throw new Exception(string.Format("Can not load data table, the connection string was null! Connection string key name was '{0}'"
                    , connectionStringKeyName));

            bool isErrored = false;

            DataTable table = new SqlClientManager(string.Empty, IdpeKeyTypes.ConnectionStringSqlServer)
                .ExecuteQueryAndGetDataTable(connectionStringKey.Value, (IdpeKeyTypes)connectionStringKey.Type, query, ref isErrored);
            return table;
        }

        public static DataTable LoadDataTable(this DataSource dataSource, string connectionStringKeyName, string query, ref bool isErrored)
        {
            IdpeKey connectionStringKey = dataSource.Keys.GetKey(connectionStringKeyName);
            if (connectionStringKey == null)
                throw new Exception(string.Format("Can not load data table, the connection string was null! Connection string key name was '{0}'"
                    , connectionStringKeyName));

            DataTable table = new SqlClientManager(string.Empty, IdpeKeyTypes.ConnectionStringSqlServer)
                .ExecuteQueryAndGetDataTable(connectionStringKey.Value, (IdpeKeyTypes)connectionStringKey.Type, query, ref isErrored);
            return table;
        }

        public static DataTable LoadDataTable(this DataSource dataSource, string connectionStringKeyName, string query, ref string errorMessage, int timeOut = 5, bool silent = false)
        {
            IdpeKey connectionStringKey = dataSource.Keys.GetKey(connectionStringKeyName);
            if (connectionStringKey == null)
                throw new Exception(string.Format("Can not load data table, the connection string was null! Connection string key name was '{0}'"
                    , connectionStringKeyName));

            DataTable table = new SqlClientManager(string.Empty, IdpeKeyTypes.ConnectionStringSqlServer)
                .ExecuteQueryAndGetDataTable(connectionStringKey.Value, (IdpeKeyTypes)connectionStringKey.Type, query, ref errorMessage, timeOut, silent);
            return table;
        }

        public static object GetProcessVariableValue(this Job job, string name, bool nullAsEmptyString = true)
        {
            object obj = job.ProcessVariables.GetValueOrDefault(name, null);
            if (obj == null)
            {
                string errorMessage = string.Format("The process variable '{0}' is not defined!", name);
                job.TraceError(errorMessage);
                job.AddContainerError(-1, errorMessage);

                if (nullAsEmptyString)
                    return string.Empty;
            }

            return obj;
        }

        public static bool AllowPartial(this DataSource dataSource)
        {
            string strAllowPartial = dataSource.Keys.GetKeyValue(IdpeKeyTypes.OutputPartialRecordsAllowed);
            return string.IsNullOrEmpty(strAllowPartial) ? false : strAllowPartial.ParseBool();
        }

        public static bool IsCacheInitialized(this DataSource dataSource, string name)
        {
            object obj = Registry.Instance.CachedTables.GetValueOrDefault(name, null);
            return obj == null ? false : true;
        }

        public static bool IsCacheInitialized(this Job job, string name)
        {
            object obj = Registry.Instance.CachedTables.GetValueOrDefault(name, null);
            return obj == null ? false : true;
        }

        public static DataTable GetCache(this Job job, string name)
        {
            object obj = Registry.Instance.CachedTables.GetValueOrDefault(name, null);
            return obj == null ? null : obj as DataTable;
        }

        public static DataTable GetCache(this DataSource dataSource, string name)
        {
            object obj = Registry.Instance.CachedTables.GetValueOrDefault(name, null);
            return obj == null ? null : obj as DataTable;
        }

        public static void TraceInformation(this Job job, string format, params object[] args)
        {
            if (job != null)
                Trace.TraceInformation(GetTracePrefix(job) + format, args);
            else
                TraceInformation(format, args);
        }

        public static void TraceInformation(this Job job, string message)
        {
            if (job != null)
                Trace.TraceInformation(GetTracePrefix(job) + message);
            else
                TraceInformation(message);
        }

        public static void TraceError(this Job job, string message)
        {
            if (job != null)
                Trace.TraceError(GetTracePrefix(job) + message);
            else
                TraceError(message);
        }

        public static void TraceError(this Job job, string format, params object[] args)
        {
            if (job != null)
                Trace.TraceError(GetTracePrefix(job) + format, args);
            else
                TraceError(format, args);
        }

        public static void TraceInformation(this WorkerData data, string message)
        {
            if (data != null)
                Trace.TraceInformation(GetTracePrefix(data) + message);
            else
                TraceInformation(message);
        }

        public static void TraceError(this WorkerData data, string message)
        {
            if (data != null)
                Trace.TraceError(GetTracePrefix(data) + message);
            else
                TraceError(message);
        }

        public static void TraceInformation(this DataSource dataSource, string message)
        {
            if (dataSource != null)
                Trace.TraceInformation(GetTracePrefix(dataSource) + message);
            else
                TraceInformation(message);
        }

        public static void TraceInformation(this DataSource dataSource, string format, params object[] args)
        {
            if (dataSource != null)
                Trace.TraceInformation(GetTracePrefix(dataSource) + format, args);
            else
                TraceInformation(format, args);
        }

        public static void TraceError(this DataSource dataSource, string message)
        {
            if (dataSource != null)
                Trace.TraceError(GetTracePrefix(dataSource) + message);
            else
                TraceError(message);
        }

        public static void TraceError(this DataSource dataSource, string format, params object[] args)
        {
            if (dataSource != null)
                Trace.TraceError(GetTracePrefix(dataSource) + format, args);
            else
                TraceError(format, args);
        }

        public static string GetTracePrefix(Job job)
        {
            string prefix = string.Format("{0}. . :", EyediaCoreConfigurationSection.CurrentConfig.InstanceName);
            if (job != null)
                prefix = string.Format("{0}.{1}.{2}:", EyediaCoreConfigurationSection.CurrentConfig.InstanceName, TrimDataSourceName(job.DataSource.Name), job.FileNameOnly);
            return prefix;
        }

        public static string GetTracePrefix(WorkerData data)
        {
            string prefix = string.Format("{0}. . :", EyediaCoreConfigurationSection.CurrentConfig.InstanceName);
            if (data != null)
                prefix = string.Format("{0}.{1}.{2}:", EyediaCoreConfigurationSection.CurrentConfig.InstanceName, TrimDataSourceName(data.Job.DataSource.Name), data.Job.FileNameOnly);
            return prefix;
        }

        public static string GetTracePrefix(DataSource dataSource)
        {
            string prefix = string.Format("{0}. . :", EyediaCoreConfigurationSection.CurrentConfig.InstanceName);
            if (dataSource != null)
                prefix = string.Format("{0}.{1}.{2}:", EyediaCoreConfigurationSection.CurrentConfig.InstanceName, TrimDataSourceName(dataSource.Name), " ");
            return prefix;
        }

        public static string GetTracePrefix()
        {
            return EyediaCoreConfigurationSection.CurrentConfig!= null? EyediaCoreConfigurationSection.CurrentConfig.InstanceName + "." : string.Empty;
        }

        private static object TrimDataSourceName(string name)
        {
            return name.Length > 10 ? name.Substring(0, 10) : name;
        }

        public static void TraceInformation(string message)
        {
            Trace.TraceInformation(GetTracePrefix() + message);
        }

        public static void TraceInformation(string format, params object[] args)
        {
            Trace.TraceInformation(GetTracePrefix() + format, args);
        }

        public static void TraceError(string message)
        {
            Trace.TraceError(GetTracePrefix() + message);
        }

        public static void TraceError(string format, params object[] args)
        {
            Trace.TraceError(GetTracePrefix() + format, args);
        }

        public static void SetZippedBinaryValue(this IdpeKey key, string str)
        {
            key.ValueBinary = new Binary(GZipArchive.Compress(str.GetByteArray()));            
        }

        public static string GetUnzippedBinaryValue(this IdpeKey key)
        {            
            return GZipArchive.Decompress(key.ValueBinary.ToArray()).GetString();
        }
    }

}



