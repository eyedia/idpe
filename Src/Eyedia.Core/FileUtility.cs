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
using System.Diagnostics;
using System.Threading;

namespace Eyedia.Core
{
    public class FileUtility
    {
        /// <summary>
        /// Only for logging purpose
        /// </summary>
        string _operation;

        /// <summary>
        /// Copies file, waits until copy completes
        /// </summary>
        /// <param name="fromFileName">Name of the source file</param>
        /// <param name="toFileName">Name of the destination file</param>
        /// <param name="move">true, if to be moved</param>
        public void FileCopy(string fromFileName, string toFileName, bool move)
        {
            _operation = move ? "MOVE" : "COPY";
            if (WaitTillFileIsFree(fromFileName))
            {
                File.Copy(fromFileName, toFileName);
                if (WaitTillFileIsFree(toFileName))
                {
                    if (move)
                        Delete(fromFileName);
                }
            }

        }

        /// <summary>
        /// Deletes file, waits if file is in use
        /// </summary>
        /// <param name="fileName">Name of the file to be deleted</param>        
        public void Delete(string fileName)
        {
            _operation = "DELETE";
            if(WaitTillFileIsFree(fileName))
                File.Delete(fileName);
        }

        /// <summary>
        /// Renames a file to unique name if exists
        /// </summary>
        /// <param name="fileName">Source fileName</param>
        /// <returns>if backed up, then new file name else original file name</returns>
        public string Backup(string fileName, bool useShortGuid = true)
        {
            _operation = "BACKUP";
            string postfix = useShortGuid ? ShortGuid.NewGuid().Value : DateTime.Now.ToString("yyyyMMddHHmmss");
            if (File.Exists(fileName))
            {
                string newFileName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + "_" + postfix + Path.GetExtension(fileName));
                FileCopy(fileName, newFileName, true);
                return newFileName;
            }
            return fileName;
        }

        private static readonly object _lock = new object();
        /// <summary>
        /// Waits until file is free or timed out
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <returns>true if file could be freed</returns>
        public bool WaitTillFileIsFree(string filename)
        {
            lock (_lock)
            {
                DateTime fileReceived = DateTime.Now;
                while (true)
                {
                    if (!File.Exists(filename))
                        return false;

                    TimeSpan timeElapsed = DateTime.Now - fileReceived;                   
                    if (timeElapsed.TotalMinutes > 2)
                    {
                        if (string.IsNullOrEmpty(_operation))
                            _operation = "WaitTillFileIsFree";
                        Trace.TraceError("The file \"{0}\" could not be freed. The operation was '{1}'", filename, _operation);
                        Trace.Flush();
                        return false;
                    }

                    try
                    {
                        using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            return true;
                        }
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(300);
                        continue;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Checks if file exist or not, supports wild card (e.g c:\temp\filename_*.txt)
        /// </summary>
        /// <param name="FileNameOrDirectoryName">file path or directory path</param>
        /// <returns></returns>
        public static bool FileExists(string FileNameOrDirectoryName)
        {
            string dir = Path.GetDirectoryName(FileNameOrDirectoryName);
            string fileName = Path.GetFileName(FileNameOrDirectoryName);
            if (!Directory.Exists(dir))
                return false;

            string[] files = System.IO.Directory.GetFiles(dir, fileName, System.IO.SearchOption.TopDirectoryOnly);
            return files.Length > 0;
        }

        public string GetFileSize(string fileName)
        {
            if (!File.Exists(fileName))
                return string.Empty;

            return new FileInfo(fileName).Length.FormatMemorySize();
        }

        public static byte[] FileToByteArray(string fileName)
        {
            using (FileStream fs = new System.IO.FileStream(fileName,
                                           System.IO.FileMode.Open,
                                           System.IO.FileAccess.Read))
            {
                var binaryData = new Byte[fileName.Length];
                long bytesRead = fs.Read(binaryData, 0,
                                     (int)fileName.Length);
                fs.Close();
                return binaryData;
            }
        }

        public static string FileToToBase64String(string fileName)
        {
            using (FileStream fs = new System.IO.FileStream(fileName,
                                           System.IO.FileMode.Open,
                                           System.IO.FileAccess.Read))
            {
                var binaryData = new Byte[fileName.Length];
                long bytesRead = fs.Read(binaryData, 0,
                                     (int)fileName.Length);
                fs.Close();
                return Convert.ToBase64String(binaryData, 0, binaryData.Length);
            }
        }

       
    }
}





