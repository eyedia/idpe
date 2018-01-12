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
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Eyedia.IDPE.Common;
using System.Text;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{

    /// <summary>
    /// Counts perfromance based on unique process name. Enable this by setting 'PerformanceCounter' to 'true' in config file
    /// </summary>
    public class PerformanceCounter : IDisposable
    {

        const int _MaxJobPerformanceTaskNameLength = 30;
        const int _MaxRowPerformanceTaskNameLength = 23;
        const int _MaxPerformanceTasks = 20;

        Dictionary<string, PerformanceTask> WorkSheet;
        public bool IsEverythingOK { get; private set; }
        public PerformanceCounter()
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter)
                return;

            WorkSheet = new Dictionary<string, PerformanceTask>();
        }

        public void StartNew(string jobId)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter)
                return;

            WorkSheet.Add(jobId, new PerformanceTask(jobId));
            IsEverythingOK = true;
           
        }


        public void Start(string jobId, JobPerformanceTaskNames PerformanceTaskName)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter)
                return;

            PerformanceTask thePerformanceTask = GetPerformanceTask(jobId);
            PerformanceTaskInformation PerformanceTaskInfo = GetPerformanceTaskSubPerformanceTask(thePerformanceTask, PerformanceTaskName);

            if (PerformanceTaskInfo == null)
            {
                PerformanceTaskInfo = new PerformanceTaskInformation(PerformanceTaskName);
                thePerformanceTask.ContainerPerformanceTasks.Add(PerformanceTaskName, PerformanceTaskInfo);
                PerformanceTaskInfo.Start();
            }
            else
            {
                throw new Exception(string.Format("The PerformanceTask '{0}' has already been added", PerformanceTaskName));
            }

        }
       

        public void Start(string jobId, RowPerformanceTaskNames PerformanceTaskName, int position)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter)
                return;

            if (position > _MaxPerformanceTasks)
                return;

            PerformanceTask thePerformanceTask = GetPerformanceTask(jobId);
            PerformanceTaskInformation PerformanceTaskInfo = GetPerformanceTaskSubPerformanceTask(thePerformanceTask, PerformanceTaskName, position);
            
            if (PerformanceTaskInfo == null)
            {
                PerformanceTaskInfo = new PerformanceTaskInformation(PerformanceTaskName, position);
                thePerformanceTask.RowLevelPerformanceTasks.Add(PerformanceTaskInfo.PerformanceTaskName, PerformanceTaskInfo);
                PerformanceTaskInfo.Start();
            }
            else
            {
                throw new Exception(string.Format("The PerformanceTask '{0}' has already been added", PerformanceTaskName));
            }
            
        }

        public void Stop(string jobId, JobPerformanceTaskNames PerformanceTaskName)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter)
                return;

            PerformanceTask thePerformanceTask = GetPerformanceTask(jobId);
            PerformanceTaskInformation PerformanceTaskInfo = GetPerformanceTaskSubPerformanceTask(thePerformanceTask, PerformanceTaskName);
            if (PerformanceTaskInfo == null) return;

            PerformanceTaskInfo.Stop();
            if (PerformanceTaskName == JobPerformanceTaskNames.WorkerFinalize)
                thePerformanceTask.Stop();

        }

        public void Stop(string jobId, RowPerformanceTaskNames PerformanceTaskName, int position)
        {
            if (!EyediaCoreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter)
                return;

            PerformanceTaskInformation PerformanceTaskInfo = GetPerformanceTaskSubPerformanceTask(GetPerformanceTask(jobId), PerformanceTaskName, position);
            if (PerformanceTaskInfo == null) return;

            PerformanceTaskInfo.Stop();

        }

        public void PrintTrace(string jobId)
        {

            if (EyediaCoreConfigurationSection.CurrentConfig.Tracking.PerformanceCounter)
                ExtensionMethods.TraceInformation("Printing perfromance counters.");
            else
                return;

            if ((!(WorkSheet.ContainsKey(jobId)))
              || (IsEverythingOK == false))
            {
                ExtensionMethods.TraceInformation("Can not print! Performance counter could not calculate due to some issues. Reset it again for next process.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            PerformanceTask thePerformanceTask = WorkSheet[jobId] as PerformanceTask;
            if (thePerformanceTask.EndedAt == DateTime.MinValue)
                thePerformanceTask.Stop();

            sb.AppendLine(string.Format("Started at:{0}", thePerformanceTask.StartedAt));

            #region PreValidate            

            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.JobInit), sb);
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.FeedData), sb);
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.SliceData), sb);            
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.WorkerInit), sb);            
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.PreValidate), sb);
            
            #endregion PreValidate

            #region Rows

            List<KeyValuePair<int, List<PerformanceTaskNameFormatted>>> aRowList = thePerformanceTask.GetRowLevelPerformanceTaskList();
            sb.AppendLine(GetColumnNames());
            int counter = 0;
            foreach (KeyValuePair<int, List<PerformanceTaskNameFormatted>> item in aRowList)
            {
                List<PerformanceTaskNameFormatted> lst = item.Value.OrderBy(i => i.Position).ToList();

                string row = string.Empty;
                string[] PerformanceTaskNames = Enum.GetNames(typeof(RowPerformanceTaskNames));
                TimeSpan tsTotalForThisRow = TimeSpan.Zero;
                foreach (string PerformanceTaskName in PerformanceTaskNames)
                {
                    RowPerformanceTaskNames thisPerformanceTaskName = (RowPerformanceTaskNames)Enum.Parse(typeof(RowPerformanceTaskNames), PerformanceTaskName);
                    PerformanceTaskNameFormatted tnf = lst.Where(l => l.PerformanceTaskName == thisPerformanceTaskName).SingleOrDefault();
                    row += string.Format("{0},", tnf == null ? "NULL" : tnf.Duration.ToString());
                    if (tnf != null)
                        tsTotalForThisRow += tnf.Duration;
                }
                //if (row.Length > 1)
                //    row = row.Substring(0, row.Length - 1);
                row += tsTotalForThisRow.ToString();
                sb.AppendLine(row);
                counter++;
            }

            #endregion Rows

            #region PostValidate

            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.PostValidate), sb);            
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.WorkerFinalize), sb);
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.ExecuteWorkerManager), sb);
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.ResultToDataTable), sb);
            FormatJobPerformanceTasks(GetPerformanceTaskSubPerformanceTask(thePerformanceTask, JobPerformanceTaskNames.OutputWriter), sb);
            
            #endregion PostValidate

            TimeSpan ts = thePerformanceTask.EndedAt - thePerformanceTask.StartedAt;
            string formatedTs = string.Format("{0} Days,{1} Hours,{2} Minutes,{3} Seconds, {4} Miliseconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

            sb.AppendLine(string.Format("Finished at:{0}. Elapsed time:{1} ({2}).", thePerformanceTask.EndedAt, formatedTs, ts.TotalMilliseconds));

            Trace.WriteLine(Environment.NewLine);
            Trace.Write(sb.ToString());
            Trace.WriteLine(Environment.NewLine);
            ExtensionMethods.TraceInformation("Printing perfromance counters. Done!");
            Trace.Flush();
        }

        private void FormatJobPerformanceTasks(PerformanceTaskInformation info, StringBuilder sb)
        {
            if (info != null)
                sb.AppendLine(string.Format("{0}:{1}", info.PerformanceTaskName.PadRight(_MaxJobPerformanceTaskNameLength, ' '), info.Duration));
        }

        private string GetColumnNames()
        {
            string columnNames = string.Empty;
            string[] PerformanceTaskNames = Enum.GetNames(typeof(RowPerformanceTaskNames));
            foreach (string PerformanceTaskName in PerformanceTaskNames)
            {
                columnNames += PerformanceTaskName + ",";
            }
            columnNames += "TotalDuration";
            return columnNames;
        }
        

        PerformanceTask GetPerformanceTask(string jobId)
        {
            if (WorkSheet == null)
                WorkSheet = new Dictionary<string, PerformanceTask>();

            if (WorkSheet.ContainsKey(jobId))
                return WorkSheet[jobId] as PerformanceTask;
            else
                return null;

        }
       

        PerformanceTaskInformation GetPerformanceTaskSubPerformanceTask(PerformanceTask thePerformanceTask, RowPerformanceTaskNames PerformanceTaskName, int position)
        {

            if ((!(thePerformanceTask.ContainsKey(PerformanceTaskName + "|" + position.ToString())))
               || (IsEverythingOK == false))
            {
                return null;
            }
            return thePerformanceTask[PerformanceTaskName, position];
        }

        private PerformanceTaskInformation GetPerformanceTaskSubPerformanceTask(PerformanceTask thePerformanceTask, JobPerformanceTaskNames PerformanceTaskName)
        {
            if ((!(thePerformanceTask.ContainerContainsKey(PerformanceTaskName)))
               || (IsEverythingOK == false))
            {                
                return null;
            }
            return thePerformanceTask[PerformanceTaskName];
        }



        #region IDisposable Members

        public void Dispose()
        {
            if (WorkSheet == null) return;

            foreach (KeyValuePair<string, PerformanceTask> item in WorkSheet)
            {
                item.Value.Dispose();
            }
        }

        #endregion
    }

    #region Class PerformanceTask
    public class PerformanceTask : IDisposable
    {     
        string Id { get; set; }
        public Hashtable ContainerPerformanceTasks { get; set; }
        public Hashtable RowLevelPerformanceTasks { get; set; }
        public Stopwatch Stopwatch { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime EndedAt { get; private set; }

        public PerformanceTask(string id)
        {
            Id = id;
            ContainerPerformanceTasks = new Hashtable();
            RowLevelPerformanceTasks = new Hashtable();
            Stopwatch = new Stopwatch();
            StartedAt = DateTime.Now;
            Stopwatch.StartNew();            
        }

        public void Stop()
        {
            Stopwatch.Stop();
            EndedAt = DateTime.Now;
        }

        public bool ContainerContainsKey(object key)
        {
            return ContainerPerformanceTasks.ContainsKey(key);
        }

        public bool ContainsKey(object key)
        {
            return RowLevelPerformanceTasks.ContainsKey(key);
        }

        public PerformanceTaskInformation this[RowPerformanceTaskNames PerformanceTaskName, int position]
        {
            get
            {
                return RowLevelPerformanceTasks[PerformanceTaskName + "|" + position] as PerformanceTaskInformation;
            }
        }

        public PerformanceTaskInformation this[JobPerformanceTaskNames PerformanceTaskName]
        {
            get
            {
                return ContainerPerformanceTasks[PerformanceTaskName] as PerformanceTaskInformation;
            }
        }

        public List<KeyValuePair<int, List<PerformanceTaskNameFormatted>>> GetRowLevelPerformanceTaskList()
        {
            Dictionary<int, List<PerformanceTaskNameFormatted>> aRow = new Dictionary<int, List<PerformanceTaskNameFormatted>>();
            IDictionaryEnumerator enumerator = RowLevelPerformanceTasks.GetEnumerator();

            int counter = 0;
            while (enumerator.MoveNext())
            {

                PerformanceTaskInformation info1 = enumerator.Value as PerformanceTaskInformation;
                int workerRowCounter = (int)(info1.PerformanceTaskName.Split("|".ToCharArray())[1]).ParseInt();
                List<PerformanceTaskNameFormatted> lst = GetList(aRow, workerRowCounter);
                PerformanceTaskNameFormatted tnf = new PerformanceTaskNameFormatted(workerRowCounter, counter, info1.PerformanceTaskName.Split("|".ToCharArray())[0], info1.Duration);
                lst.Add(tnf);
                if (aRow.ContainsKey(workerRowCounter))
                    aRow[workerRowCounter] = lst;
                else
                    aRow.Add(workerRowCounter, lst);
                counter++;

            }
            List<KeyValuePair<int, List<PerformanceTaskNameFormatted>>> aRowList = (from r in aRow
                                                                         orderby r.Key
                                                                         select r).ToList();

            return aRowList;
        }
        List<PerformanceTaskNameFormatted> GetList(Dictionary<int, List<PerformanceTaskNameFormatted>> list, int position)
        {
            if (list.ContainsKey(position))
                return list[position];
            else
                return new List<PerformanceTaskNameFormatted>();

        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (DictionaryEntry item in ContainerPerformanceTasks)
            {
                (item.Value as PerformanceTaskInformation).Dispose();
            }

            foreach (DictionaryEntry item in RowLevelPerformanceTasks)
            {
                (item.Value as PerformanceTaskInformation).Dispose();
            }

            Stopwatch = null;            
        }

        #endregion
    }

    #endregion Class PerformanceTask

    #region Class PerformanceTaskInformation

    public class PerformanceTaskInformation : IDisposable
    {
        public PerformanceTaskInformation(JobPerformanceTaskNames performanceTaskName)
        {
            PerformanceTaskName = performanceTaskName.ToString();
            Position = 0;
        }
        public PerformanceTaskInformation(RowPerformanceTaskNames performanceTaskName, int position)
        {
            PerformanceTaskName = string.Format("{0}|{1}", performanceTaskName.ToString(), position);
            _Stopwatch = Stopwatch.StartNew();
            Position = position;
        }


        Stopwatch _Stopwatch;

        public string PerformanceTaskName { get; private set; }
        public bool IsCounterStopped { get; private set; }
        public TimeSpan Duration { get; private set; }
        public long Position { get; private set; }

        public void Start()
        {
            _Stopwatch = Stopwatch.StartNew();
            IsCounterStopped = false;
        }

        public void Stop()
        {
            IsCounterStopped = true;
            _Stopwatch.Stop();
            Duration = _Stopwatch.Elapsed;
        }

        public void Reset()
        {
            _Stopwatch.Reset();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _Stopwatch = null;
        }

        #endregion
    }

    #endregion Class PerformanceTaskInformation

    #region Class PerformanceTaskNameFormatted

    public class PerformanceTaskNameFormatted
    {
        public RowPerformanceTaskNames PerformanceTaskName { get; private set; }
        public int Position { get; private set; }
        public int Counter { get; private set; }
        public TimeSpan Duration { get; private set; }

        public PerformanceTaskNameFormatted(int position, int counter, string columnName, TimeSpan duration)
        {            
            Position = position;
            Counter = counter;
            PerformanceTaskName = (RowPerformanceTaskNames) Enum.Parse(typeof(RowPerformanceTaskNames), columnName);
            Duration = duration;
        }       
    }

    #endregion Class PerformanceTaskNameFormatted

    public enum JobPerformanceTaskNames
    {
        JobInit,
        FeedData,
        SliceData,        
        ExecuteWorkerManager,
        WorkerInit,
        PreValidate,
        PostValidate,
        WorkerFinalize,
        ResultToDataTable,
        OutputWriter,
        OutputWriterOracle,
    }

    public enum RowPerformanceTaskNames
    {        
        LoopPreset,
        RowPreparing,
        ParseDataTypes_ParseInit,
        ParseDataTypes_Attributes,
        ParseDataTypes_AttributesSystem1,
        ParseDataTypes_AttributesSystem2,
        RowPrepared,
        ParseDataTypes_ParseAgain,
        RowValidate,
        RowValidateMandatory,
        StoreBadData,
        LoopReset
    }
}





