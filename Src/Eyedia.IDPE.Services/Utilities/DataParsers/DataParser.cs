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
using Eyedia.IDPE.Common;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
    public class DataParser : IDisposable
    {
        /// <summary>
        /// Creates new data parser object in demo mode
        /// </summary>
        public DataParser() { }

        /// <summary>
        /// Creates new data parser object within IDPE context
        /// </summary>
        /// <param name="job"></param>
        public DataParser(Job job)
        {
            this.Job = job;
            this.DataSource = job.DataSource;
            Init();
        }

        /// <summary>
        /// Creates new data parser object within IDPE context
        /// </summary>
        /// <param name="dataSource"></param>
        public DataParser(DataSource dataSource)
        {
            this.DataSource = dataSource;            
            Init();
        }

        private void Init()
        {
            this.TempFolder = Path.Combine(Information.TempDirectoryTempData, DataSource.Id.ToString());
            if (!Directory.Exists(this.TempFolder))
                Directory.CreateDirectory(this.TempFolder);
        }

        /// <summary>
        /// The job object
        /// </summary>
        public Job Job { get; private set; }

        /// <summary>
        /// The data source object
        /// </summary>
        public DataSource DataSource { get; private set; }

        /// <summary>
        /// Temp folder for each data source
        /// </summary>
        public string TempFolder { get; private set; }

        public virtual void Dispose() { }
     
    }
}




