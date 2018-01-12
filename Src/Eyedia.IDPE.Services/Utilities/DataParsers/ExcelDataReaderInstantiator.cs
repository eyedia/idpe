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
using System.IO;
using System.Threading;
using Excel;
using Eyedia.Core;

namespace Eyedia.IDPE.Services
{
    internal sealed class ExcelDataReaderInstantiator
    {
        public string FileName { get; private set; }
        public IExcelDataReader ExcelReader { get; private set; }
        public FileStream FileStream { get; private set; }
        public ExcelDataReaderInstantiator(string fileName)
        {
            FileName = fileName;
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension == ".xlsx")
            {                
                FileStream = File.Open(FileName, FileMode.Open, FileAccess.Read);
                ExcelReader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);                
            }
            else
            {
                InstantiateWithThread();
            }
        }
        public void InstantiateWithThread()
        {
            var thread = new Thread(this.Instantiate);
            thread.Start();

            if (!thread.Join(TimeSpan.FromSeconds(2)))
            {
                thread.Abort();

                try
                {
                    FileStream.Close();
                    FileStream.Dispose();
                }
                catch { }
                
                throw new BusinessException(string.Format("The file {0} was corrupt and ignored!", Path.GetFileName(FileName)));
            }
        }


        private void Instantiate()
        {
            try
            {
                FileStream = File.Open(FileName, FileMode.Open, FileAccess.Read);
                ExcelReader = ExcelReaderFactory.CreateBinaryReader(FileStream);
            }
            catch { }
        }
    }
}


