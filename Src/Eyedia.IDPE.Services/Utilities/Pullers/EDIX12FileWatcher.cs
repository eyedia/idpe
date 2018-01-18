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
using Eyedia.Core.Windows;
using Eyedia.IDPE.Common;
using System.IO;
using Eyedia.IDPE.DataManager;


namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// A watcher, which internally turns a EDI X12 file into a Csv Or Flat
    /// </summary>
    public class EDIX12FileWatcher
    {       
        /// <summary>
        /// The datasource which was instantiated based on data source id passed through constructor
        /// </summary>
        public DataSource DataSource { get; private set; }

        /// The file name which was set through constructor
        public string FileName { get; private set; }

        /// <summary>
        /// The Xslt (extracted from data source keys)
        /// </summary>
        public string Xslt
        {
            get
            {
                return DataSource.Keys.GetKeyValue(IdpeKeyTypes.EDIX12Xslt);
            }
        }

        public EDIX12FileWatcher(int dataSourceId, string fileName)
        {            
            this.DataSource = new DataSource(dataSourceId, string.Empty);
            this.FileName = fileName;
        }

        public void Process()
        {
            string onlyFileName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(FileName), ".csv");
            string flatFileName = Path.Combine(DataSource.GetArchiveFolder(DataSource.Id, DataSource.Keys), onlyFileName);
            TransformEDIToFlat(flatFileName);
            Registry.Instance.Pullers._LocalFileSystemWatcher.Process(flatFileName, onlyFileName, DataSource.Id, false);
        }

        private void TransformEDIToFlat(string flatFileName)
        {
            StreamReader sr = new StreamReader(FileName);
            string fileContent = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            EdiX12Parser ediParser = new EdiX12Parser(FileName, fileContent);

            StreamWriter sw = new StreamWriter(flatFileName);
            sw.Write(ediParser.Parse(Xslt).ToString());
            sw.Close();

            ediParser.Dispose();
        }
    }
}


