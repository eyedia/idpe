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
using System.IO.Compression;

namespace Eyedia.Core
{
    public class GZipArchive
    {
        public static byte[] Compress(byte[] data)
        {
            var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress, true))
            {
                gzip.Write(data, 0, data.Length);
                gzip.Close();
            }
            return output.ToArray();
        }

        public static string Compress(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            string outFileName = fi.FullName + ".gz";
            using (FileStream inFile = fi.OpenRead())
            {              
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {                   
                    using (FileStream outFile =
                                File.Create(outFileName))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                            CompressionMode.Compress))
                        {                       
                            inFile.CopyTo(Compress);
                        }
                    }
                }
            }

            return outFileName;
        }

        public static void Decompress(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            using (FileStream inFile = fi.OpenRead())
            {              
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length -
                        fi.Extension.Length);
               
                using (FileStream outFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile,
                            CompressionMode.Decompress))
                    {                       
                        Decompress.CopyTo(outFile);
                      
                    }
                }
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            var output = new MemoryStream();
            var input = new MemoryStream();
            input.Write(data, 0, data.Length);
            input.Position = 0;

            using (var gzip = new GZipStream(input, CompressionMode.Decompress, true))
            {
                var buff = new byte[64];
                var read = gzip.Read(buff, 0, buff.Length);

                while (read > 0)
                {
                    output.Write(buff, 0, read);
                    read = gzip.Read(buff, 0, buff.Length);
                }

                gzip.Close();
            }
            return output.ToArray();
        }
 
    }
}



