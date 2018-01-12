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
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Eyedia.Core;
using System.IO;
using System.IO.Packaging;


namespace Eyedia.IDPE.Common
{
    public class SourceCodeManager
    {
        string _DestinationPath;
        List<string> _ExcludeList;
        List<string> ExcludedDirs;
        const string _ExcludeListCSV = "bin,obj,Debug,Release,InBound,OutBound";

        public SourceCodeManager(string destinationPath)
        {
            ExcludedDirs = new List<string>();
            _ExcludeList = new List<string>(_ExcludeListCSV.Split(",".ToCharArray()));
            _DestinationPath = destinationPath;
            if (!Directory.Exists(_DestinationPath))
            {
                Directory.CreateDirectory(_DestinationPath);
            }
            else
            {
                DeleteDir(new DirectoryInfo(_DestinationPath));
            }
        }

        public void BundleCode(string sDir)
        {
            CleanDir(sDir);
            CopyDir(sDir);
        }

        void DeleteDir(DirectoryInfo directory)
        {
            if (!directory.Exists) return;
            foreach (System.IO.FileInfo file in directory.GetFiles())
                file.Delete();

            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
                subDirectory.Delete(true);
        } 

        void CopyDir(string sDir)
        {
            string dName = string.Empty;
            if (sDir.LastIndexOf("\\") > -1)
                dName = sDir.Substring(sDir.LastIndexOf("\\"), sDir.Length - sDir.LastIndexOf("\\"));

            dName = _DestinationPath + dName;
            if (!Directory.Exists(dName))
                Directory.CreateDirectory(dName);

            //Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sDir, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sDir, dName));
            //Copy all the files 
            foreach (string newPath in Directory.GetFiles(sDir, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sDir, dName));
        }

        void CleanDir(string sDir)
        {
            DirSearch(sDir);

            foreach (string dir in ExcludedDirs)
            {                
                DeleteDir(new DirectoryInfo(dir));
            }
        }
            
    

        
        List<string> DirSearch(string sDir)
        {

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    string dName = string.Empty;
                    if (d.LastIndexOf("\\") > -1)
                        dName = d.Substring(d.LastIndexOf("\\") + 1, (d.Length - d.LastIndexOf("\\")) - 1);
                    if (IsExcluded(dName))
                        ExcludedDirs.Add(d);

                    DirSearch(d);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return ExcludedDirs;
        }

        bool IsExcluded(string pattern)
        {

            List<string> result = _ExcludeList.Where(p => p.ToLower() == pattern.ToLower()).ToList();
            return result.Count > 0 ? true : false;

        }

        string documentPath = "";
        string resourcePath = "";
        string packagePath = "";
        string PackageRelationshipType = "";
        string ResourceRelationshipType = "";
        private void CreatePackage()
        {
            // Convert system path and file names to Part URIs. In this example 
            // Uri partUriDocument /* /Content/Document.xml */ =
            //     PackUriHelper.CreatePartUri( 
            //         new Uri("Content\Document.xml", UriKind.Relative));
            // Uri partUriResource /* /Resources/Image1.jpg */ =
            //     PackUriHelper.CreatePartUri( 
            //         new Uri("Resources\Image1.jpg", UriKind.Relative));
            Uri partUriDocument = PackUriHelper.CreatePartUri(
                                      new Uri(documentPath, UriKind.Relative));
            Uri partUriResource = PackUriHelper.CreatePartUri(
                                      new Uri(resourcePath, UriKind.Relative));

            // Create the Package 
            // (If the package file already exists, FileMode.Create will 
            //  automatically delete it first before creating a new one. 
            //  The 'using' statement insures that 'package' is 
            //  closed and disposed when it goes out of scope.) 
            using (Package package =
                Package.Open(packagePath, FileMode.Create))
            {
                // Add the Document part to the Package
                PackagePart packagePartDocument =
                    package.CreatePart(partUriDocument,
                                   System.Net.Mime.MediaTypeNames.Text.Xml);

                // Copy the data to the Document Part 
                using (FileStream fileStream = new FileStream(
                       documentPath, FileMode.Open, FileAccess.Read))
                {
                    CopyStream(fileStream, packagePartDocument.GetStream());
                }// end:using(fileStream) - Close and dispose fileStream. 

                // Add a Package Relationship to the Document Part
                package.CreateRelationship(packagePartDocument.Uri,
                                           TargetMode.Internal,
                                           PackageRelationshipType);

                // Add a Resource Part to the Package
                PackagePart packagePartResource =
                    package.CreatePart(partUriResource,
                                   System.Net.Mime.MediaTypeNames.Image.Jpeg);

                // Copy the data to the Resource Part 
                using (FileStream fileStream = new FileStream(
                       resourcePath, FileMode.Open, FileAccess.Read))
                {
                    CopyStream(fileStream, packagePartResource.GetStream());
                }// end:using(fileStream) - Close and dispose fileStream. 

                // Add Relationship from the Document part to the Resource part
                packagePartDocument.CreateRelationship(
                                        new Uri(@"../resources/image1.jpg",
                                        UriKind.Relative),
                                        TargetMode.Internal,
                                        ResourceRelationshipType);

            }// end:using (Package package) - Close and dispose package.

        }// end:CreatePackage() 

        /// <summary> 
        ///   Copies data from a source stream to a target stream.</summary> 
        /// <param name="source">
        ///   The source stream to copy from.</param> 
        /// <param name="target">
        ///   The destination stream to copy to.</param> 
        private void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }// end:CopyStream()




      
    }
}




