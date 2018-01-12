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
using System.Collections.Generic;
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Tar;
//using System.IO.Compression;
using SharpCompress;
using SharpCompress.Reader;
using SharpCompress.Common;
using SharpCompress.Archive;
using SharpCompress.Archive.Zip;

namespace Eyedia.Core.Windows
{
    /// <summary>
    /// An utility to zip/unzip zip/rar/tar files
    /// </summary>
    public class ZipFileHandler
    {

        /// <summary>
        /// Unzip zip/tar/rar file
        /// </summary>
        /// <param name="sourceDirectory">zip/tar/rar file name</param>
        /// <param name="outFolder">output folder</param>
        /// <returns></returns>
        public static string Zip(string sourceDirectory, string toZipFile, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (!Directory.Exists(sourceDirectory))
                return string.Empty;

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(sourceDirectory, searchPattern, searchOption);
                CompressionInfo compressionInfo = new CompressionInfo();
                compressionInfo.DeflateCompressionLevel = SharpCompress.Compressor.Deflate.CompressionLevel.Default;
                archive.SaveTo(toZipFile, compressionInfo);
                return toZipFile;
            }
        }

        /// <summary>
        /// Unzip zip/tar/rar file
        /// </summary>
        /// <param name="fileName">zip/tar/rar file name</param>
        /// <param name="outFolder">output folder</param>
        /// <returns></returns>
        public static List<string> UnZip(string fileName, string outFolder = null)
        {
            if (outFolder == null)
                outFolder = Path.GetDirectoryName(fileName);

            string extn = Path.GetExtension(fileName).ToLower();
            switch (extn)
            {               
                case ".zip":
                    return UnZipFile(fileName, outFolder);
                case ".rar":
                case ".tar":
                    return UnRarFile(fileName, outFolder);
            }
            return new List<string>();
        }

        static List<string> UnZipFile(string fileName, string outputFolder = null)
        {
            List<string> fileNames = new List<string>();

            using (var archive = ArchiveFactory.Open(fileName))
            {
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        fileNames.Add(Path.Combine(outputFolder, entry.FilePath));
                        entry.WriteToDirectory(outputFolder, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
            return fileNames;
        }

        static List<string> UnRarFile(string fileName, string outputFolder = null)
        {
            List<string> fileNames = new List<string>();
            using (Stream stream = File.OpenRead(fileName))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        fileNames.Add(Path.Combine(outputFolder, reader.Entry.FilePath));
                        reader.WriteEntryToDirectory(outputFolder, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
            return fileNames;
        }     
    }
    
}





