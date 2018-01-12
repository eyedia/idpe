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
using System.Runtime.InteropServices;
using System.IO;

namespace Symplus.Core
{
    public class ZipArchive :IDisposable
    {
        /// <summary>
        /// Zips target file or folder.
        /// </summary>
        /// <param name="zipFileName">zip file to be created</param>
        /// <param name="fileOrFolderName">target file or folder</param>
        public void Zip(string zipFileName, string fileOrFolderName)
        {
            bool isFile = true;
            if (File.Exists(fileOrFolderName))
                isFile = true;
            else if (Directory.Exists(fileOrFolderName))
                isFile = false;
            else
                return;

            byte[] emptyzip = new byte[] { 80, 75, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            FileStream fs = File.Create(zipFileName);
            fs.Write(emptyzip, 0, emptyzip.Length);
            fs.Flush();
            fs.Close();
            fs = null;


            //Shell32.Shell sc = new Shell32.Shell();
            //Shell32.Folder DestFlder = sc.NameSpace(zipFileName);

            //if (isFile)
            //{
            //    DestFlder.CopyHere(fileOrFolderName, 20);
            //}
            //else
            //{
            //    Shell32.Folder SrcFlder = sc.NameSpace(fileOrFolderName);
            //    Shell32.FolderItems items = SrcFlder.Items();
            //    DestFlder.CopyHere(items, 20);
            //}

            //System.Threading.Thread.Sleep(1000);
            //try
            //{
            //    Marshal.ReleaseComObject(sc);
            //}
            //catch { }
        }
       

        public void Dispose()
        {
          
        }
 
    }
}



