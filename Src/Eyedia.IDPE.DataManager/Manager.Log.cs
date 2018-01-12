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
using System.Data;
using Eyedia.Core.Data;
using System.Diagnostics;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.DataManager
{
    public partial class Manager
    {
        public List<SreLog> GetLogs(DateTime? fromDate = null, DateTime? toDate = null, string fileName = null, string dataSourceName = null)
        {
            List<SreLog> sreLogs = new List<SreLog>();
            string commandText = "select  l.Id, [FileName],[SubFileName], DataSourceId,ds.Name as [DataSourceName], TotalRecords,TotalValidRecords, Started, l.Finished, [Environment] ";
            commandText += "from SreDataSource ds ";
            commandText += "inner join SreLog l on ds.Id = l.DataSourceId";

            if ((fromDate != null)
             || (toDate != null)
              || (fileName != null)
               || (dataSourceName != null))
                commandText += " where ";

            bool addAnd = false;

            if ((fromDate != null)
                && (toDate != null))
            {
                commandText += "[Started] between @fromDate and @fromDatePlusOneday";
                addAnd = true;
            }
            else if (fromDate != null)
            {
                commandText += "[Started] > @fromDate";
                addAnd = true;
            }
            else if (toDate != null)
            {
                commandText += "[Started] < @toDate";
                addAnd = true;
            }            

            if (fileName != null)
            {
                if (addAnd)
                    commandText += " and ";

                if (fileName != null)
                    commandText += string.Format(" [FileName] like '%{0}%' or [SubFileName] like '%{0}%'", fileName);
            }

            if (dataSourceName != null)
            {

                if (addAnd)
                    commandText += " and ";

                if (dataSourceName != null)
                    commandText += string.Format(" ds.Name like '%{0}%'", dataSourceName);
            }
            
            DataTable table = new DataTable();
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand();
            command.CommandText = commandText + " order by [Started] desc";
            if(commandText.Contains("@fromDate"))
                command.AddParameterWithValue("fromDate", fromDate);
            if (commandText.Contains("@fromDatePlusOneday"))
                command.AddParameterWithValue("fromDatePlusOneday", fromDate.Value.AddDays(1));
            if (commandText.Contains("@toDate"))
                command.AddParameterWithValue("toDate", fromDate);

            command.Connection = conn;
            IDataReader reader = command.ExecuteReader();
            table.Load(reader);
            reader.Close();
            reader.Dispose();
            command.Dispose();

            if (table == null)
                return sreLogs;

            foreach (DataRow row in table.Rows)
            {
                SreLog log = new SreLog();
                log.Id = (int)row["Id"].ToString().ParseInt();
                log.FileName = row["FileName"].ToString();
                log.SubFileName = row["SubFileName"].ToString();
                log.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                log.DataSourceName = row["DataSourceName"].ToString();
                log.TotalRecords = (int)row["TotalRecords"].ToString().ParseInt();
                log.TotalValidRecords = (int)row["TotalValidRecords"].ToString().ParseInt();
                log.Started = (DateTime)row["Started"];
                log.Finished = (DateTime)row["Finished"];
                log.Environment = row["Environment"].ToString();
                sreLogs.Add(log);
            }

            return sreLogs;
        }

        public List<SreLog> GetLogs_old(DateTime? fromDate = null, DateTime? toDate = null, string fileName = null, string dataSourceName = null)
        {            
            List<SreLog> sreLogs = new List<SreLog>();
            string commandText = "select  l.Id, [FileName],[SubFileName], DataSourceId,ds.Name as [DataSourceName], TotalRecords,TotalValidRecords, Started, l.Finished, [Environment] ";
            commandText += "from SreDataSource ds ";
            commandText += "inner join SreLog l on ds.Id = l.DataSourceId";
         
            if ((fromDate != null)
             || (toDate != null)
              || (fileName != null)
               || (dataSourceName != null))
                commandText += " where ";

           
            if ((fromDate != null)
                && (toDate != null))
                commandText += string.Format("[Started] between '{0}' and '{1}'"
                    , fromDate.Value.ToShortDateString(), toDate.Value.AddDays(1).ToShortDateString());
            else if (fromDate != null)
                commandText += string.Format("[Started] > '{0}'", fromDate.Value.ToShortDateString());
            else if (toDate != null)
                commandText += string.Format("[Started] < '{0}'", toDate.Value.ToShortDateString());

            if (fileName != null)
            {
                if ((commandText.Substring(commandText.Length - 1, 1)) == "'")
                    commandText += " and ";

                if (fileName != null)
                    commandText += string.Format(" [FileName] like '%{0}%' or [SubFileName] like '%{0}%'", fileName);
            }

            if (dataSourceName != null)
            {

                if ((commandText.Substring(commandText.Length - 1, 1)) == "'")
                    commandText += " and ";

                if (dataSourceName != null)
                    commandText += string.Format(" ds.Name like '%{0}%'", dataSourceName);
            }
            DataTable table = null;

            try
            {
                table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText + " order by [Started] desc");
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.ToString(), "CommandText = " + commandText + " order by [Started] desc");
                return sreLogs;
            }

            if (table == null)
                return sreLogs;

            foreach (DataRow row in table.Rows)
            {
                SreLog log = new SreLog();
                log.Id = (int)row["Id"].ToString().ParseInt();
                log.FileName = row["FileName"].ToString();
                log.SubFileName = row["SubFileName"].ToString();
                log.DataSourceId = (int)row["DataSourceId"].ToString().ParseInt();
                log.DataSourceName = row["DataSourceName"].ToString();
                log.TotalRecords = (int)row["TotalRecords"].ToString().ParseInt();
                log.TotalValidRecords = (int)row["TotalValidRecords"].ToString().ParseInt();
                log.Started = (DateTime)row["Started"];
                log.Finished = (DateTime)row["Finished"];
                log.Environment = row["Environment"].ToString();
                sreLogs.Add(log);
            }

            return sreLogs;
        }

        public DataTable GetLogsSummary(DateTime? fromDate = null, DateTime? toDate = null, string fileName = null, string dataSourceName = null)
        {
            string commandText = "Select DATEADD(dd, 0, DATEDIFF(dd, 0, started)) as [Date], count(*) as [TotalFiles] , sum(TotalRecords) as [TotalRecords] ";
            commandText += "from SreLog l ";
            commandText += "inner join SreDataSource ds on l.DataSourceId = ds.Id ";

            if ((fromDate != null)
            || (toDate != null)
             || (fileName != null)
              || (dataSourceName != null))
                commandText += " where ";


            bool addAnd = false;

            if ((fromDate != null)
                && (toDate != null))
            {
                commandText += "[Started] between @fromDate and @fromDatePlusOneday";
                addAnd = true;
            }
            else if (fromDate != null)
            {
                commandText += "[Started] > @fromDate";
                addAnd = true;
            }
            else if (toDate != null)
            {
                commandText += "[Started] < @toDate";
                addAnd = true;
            }

            if (fileName != null)
            {
                if (addAnd)
                    commandText += " and ";

                if (fileName != null)
                    commandText += string.Format(" [FileName] like '%{0}%' or [SubFileName] like '%{0}%'", fileName);
            }

            if (dataSourceName != null)
            {

                if (addAnd)
                    commandText += " and ";

                if (dataSourceName != null)
                    commandText += string.Format(" ds.Name like '%{0}%'", dataSourceName);
            }
            
            
            commandText += " group by DATEADD(dd, 0, DATEDIFF(dd, 0, started))";

            DataTable table = new DataTable();
            IDal myDal = new DataAccessLayer(Information.EyediaCoreConfigurationSection.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand();
            command.CommandText = commandText;

            if (commandText.Contains("@fromDate"))
                command.AddParameterWithValue("fromDate", fromDate);
            if (commandText.Contains("@fromDatePlusOneday"))
                command.AddParameterWithValue("fromDatePlusOneday", fromDate.Value.AddDays(1));
            if (commandText.Contains("@toDate"))
                command.AddParameterWithValue("toDate", fromDate);

            command.Connection = conn;
            IDataReader reader = command.ExecuteReader();
            table.Load(reader);
            reader.Close();
            reader.Dispose();
            command.Dispose();
            return table;
        }

        public int SaveLog(string fileName, string subFileName, int dataSourceId, int totalRecords, int totalValidRecords, DateTime started, DateTime finished, string sreEnvironment)
        {
            IDal myDal = new DataAccessLayer(EyediaCoreConfigurationSection.CurrentConfig.Database.DatabaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(_ConnectionString);
            conn.Open();
            IDbCommand command = myDal.CreateCommand();
            command.Connection = conn;
            int id = 0;
            try
            {

                command.CommandText = "insert into [SreLog] ([FileName],[SubFileName],[DataSourceId],[TotalRecords],[TotalValidRecords],[Started],[Finished],[Environment]) ";
                command.CommandText += " values(@FileName, @SubFileName, @DataSourceId,@TotalRecords,@TotalValidRecords,@Started, @Finished, @Environment)";
                command.AddParameterWithValue("FileName", fileName);
                if (string.IsNullOrEmpty(subFileName))
                    command.AddParameterWithValue("SubFileName", DBNull.Value);
                else
                    command.AddParameterWithValue("SubFileName", subFileName);

                command.AddParameterWithValue("DataSourceId", dataSourceId);
                command.AddParameterWithValue("TotalRecords", totalRecords);
                command.AddParameterWithValue("TotalValidRecords", totalValidRecords);
                command.AddParameterWithValue("Started", started);
                command.AddParameterWithValue("Finished", finished);
                command.AddParameterWithValue("Environment", sreEnvironment);
                command.ExecuteNonQuery();

                //Deb: I know dirty coding, need to be changed. OUTPUT INSERTED.Id not working @SQL CE
                command.Parameters.Clear();
                command.CommandText = "SELECT max(Id) from SreLog";
                id = (Int32)command.ExecuteScalar();
            }
            catch (Exception ex)
            {

                Trace.TraceError(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();

            }
            return id;
        }

    }
}


