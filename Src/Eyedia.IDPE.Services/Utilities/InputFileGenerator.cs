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

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// Generates input file content or actual data as System.Data.DataTable.
    /// </summary>
    public abstract class InputFileGenerator
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InputFileGenerator() { }

        /// <summary>
        /// When implemented by a 'PullSQL' type data source, this is the recommended constructor
        /// </summary>
        /// <param name="sqlWatcher">The SQL watcher will be passed from SRE context</param>
        public InputFileGenerator(Watchers sqlWatcher)
        {
            this.SqlWatcher = (SqlWatcher)sqlWatcher;
        }

        /// <summary>
        /// When implemented for scheduler type
        /// </summary>
        /// <param name="dataSource">The data source will be passed from SRE context</param>
        public InputFileGenerator(DataSource dataSource)
        {
            this.DataSource = dataSource;
        }

        /// <summary>
        /// When implemented by 'Non-PullSQL' type data source, this is the recommended constructor
        /// </summary>
        /// <param name="data">The data will be passed from SRE context</param>
        public InputFileGenerator(WorkerData data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Most commonly used constructor
        /// </summary>
        /// <param name="job">The job will be passed from SRE context</param>
        public InputFileGenerator(Job job)
        {
            this.Job = job;
        }

        /// <summary>
        /// The SqlWatcher, If appropriate constructor is used, else null
        /// </summary>
        protected SqlWatcher SqlWatcher { get; set; }

        /// <summary>
        /// The Data, If appropriate constructor is used, else null
        /// </summary>
        protected WorkerData Data { get; set; }

        /// <summary>
        /// The Data dource, If appropriate constructor is used, else null
        /// </summary>
        protected DataSource DataSource { get; set; }

        /// <summary>
        /// The Job, If appropriate constructor is used, else null
        /// </summary>
        protected Job Job { get; set; }

        /// <summary>
        /// Generate input file content from the SQL output(the 'data')
        /// </summary>
        /// <param name="data">SQL output will be loaded into this data table</param>
        /// <exception cref="NotImplementedException">If actual code is implemented in other GenerateFileContent(string or StringBuilder), then throw NotImplementedException from this.</exception>
        /// <returns>Input file content as StringBuilder</returns>
        public abstract StringBuilder GenerateFileContent(DataTable data);


        /// <summary>
        /// Generate input data from any file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <exception cref="NotImplementedException">If actual code is implemented in other GenerateFileContent(DataTable or StringBuilder), then throw NotImplementedException from this.</exception>
        /// <returns>Actual data as DataTable</returns>
        public abstract DataTable GenerateFileContent(string fileName);

        /// <summary>
        /// Generate input data from file content of any readable file(.txt|.csv|.dat|etc)
        /// </summary>
        /// <param name="fileContent">File content</param>
        /// <exception cref="NotImplementedException">If actual code is implemented in other GenerateFileContent(DataTable or string), then throw NotImplementedException from this.</exception>
        /// <returns>Actual data as DataTable</returns>
        public abstract DataTable GenerateFileContent(StringBuilder fileContent);
    }
    
}





