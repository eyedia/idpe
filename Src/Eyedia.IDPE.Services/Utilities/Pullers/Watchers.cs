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
	public class Watchers : IDisposable
	{
        protected Dictionary<string, object> DataSourceParameters
        {
            get;
            set;
        }
        public virtual bool IsRunning { get; set; }

        public delegate void FileSystemWatcherEventHandler(FileSystemWatcherEventArgs e);
        public event FileSystemWatcherEventHandler FileDownloaded;

        protected void InvokeFileDownloaded(FileSystemWatcherEventArgs e)
        {
            if (FileDownloaded != null)
            {
                FileDownloaded(e);
            }
        }

        public delegate void SqlWatcherEventHandler(SqlWatcherEventArgs e);        
        public event SqlWatcherEventHandler AfterExecutingSelect;

        protected void InvokeAfterExecutingSelect(SqlWatcherEventArgs e)
        {
            if (AfterExecutingSelect != null)
            {
                AfterExecutingSelect(e);
            }
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            
        }

        #endregion
    }

    public class FileSystemWatcherEventArgs
    {
        public FileSystemWatcherEventArgs(Dictionary<string, object> applicationParameters, string fileName, string renamedToIdentifier)
        {
            ApplicationParameters = applicationParameters;
            FileName = fileName;
            RenamedToIdentifier = renamedToIdentifier;
            FileExtension = Path.GetExtension(fileName);
        }

        public FileSystemWatcherEventArgs(Dictionary<string, object> applicationParameters, string fileName)
        {
            ApplicationParameters = applicationParameters;
            FileName = fileName;            
            FileExtension = Path.GetExtension(fileName);
        }
        
        public Dictionary<string, object> ApplicationParameters
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            private set;
        }

        public string RenamedToIdentifier
        {
            get;
            private set;
        }
      
        public string FileExtension { get; set; }

    }

    public class SqlWatcherEventArgs
    {
        public SqlWatcherEventArgs(DataTable data)
        {
            this.Data = data;
        }

        public DataTable Data { get; set; }
    }
}





