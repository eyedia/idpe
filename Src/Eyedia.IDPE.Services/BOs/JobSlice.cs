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
using System.IO;
using System.Data;

namespace Eyedia.IDPE.Services
{
    public class JobSlice : IDisposable
    {
        public enum JobSliceStatus
        {
            Unknown,
            Processing,
            Processed,
            Failed = 99
        }

        public JobSlice() { }

        public JobSlice(string jobId, int slicePosition, DataTable inputData, List<string> csvRows)
        {
            if (inputData.Rows.Count != csvRows.Count)
                throw new Exception(string.Format("JobSlice initalization failed! Invalid input data {0} & corresponding csv rows {1}",
                    inputData.Rows.Count, csvRows.Count));

            this.JobSliceId = jobId + "." + slicePosition;
            this.SlicePosition = slicePosition;
            this.InputData = inputData;
            this.CSVRows = csvRows;            
        }

        public string JobSliceId { get; private set; }
        public int SlicePosition { get; private set; }

        /// <summary>
        /// Returns list of csv rows(lines
        /// </summary>
        public List<string> CSVRows { get; private set; }
        public DataTable InputData { get; set; }        
        public JobSliceStatus Status { get; set; }
        public Guid WorkflowInstanceId { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            if(InputData != null)
                InputData.Dispose();
            CSVRows = null;
        }

        #endregion
    }
}



