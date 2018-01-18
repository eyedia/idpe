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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Eyedia.Core;
using Eyedia.Core.Windows;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Services
{
    public class DeploymentService
    {
        public DeploymentService() { }

        public delegate void CallbackEventHandler(string info, bool isError);
        public event CallbackEventHandler Callback;

        public string PickupDlls(string fromLocation = null)
        {
            string toZipFile = Path.Combine(Information.TempDirectoryTempData, "idpe.zip");
            if (File.Exists(toZipFile))
                File.Delete(toZipFile);
            
            if (fromLocation == null)
                fromLocation = AppDomain.CurrentDomain.BaseDirectory;

            //create temp repository
            string tempRepository = Path.Combine(Information.TempDirectoryTempData, "sredlls");

            if (Directory.Exists(tempRepository))
                Directory.Delete(tempRepository, true);
            Directory.CreateDirectory(tempRepository);

            //copy neccessary files into temp repository
            string[] files = Directory.GetFiles(fromLocation);
            List<string> artifacts = DeploymentArtifacts;
            foreach (string file in files)
            {
                if (artifacts.Contains(Path.GetFileName(file)))                
                    File.Copy(file, Path.Combine(tempRepository, Path.GetFileName(file)));                
            }

            //couple of artifacts could be with relative path, lets try
            foreach(string file in artifacts)
            {               
                if (File.Exists(Path.Combine(tempRepository, file)))
                {
                    if(!File.Exists(Path.Combine(tempRepository, Path.GetFileName(file))))
                        File.Copy(file, Path.Combine(tempRepository, Path.GetFileName(file)));
                }
            }

            return EnsureFiles(artifacts, tempRepository) ? ZipFileHandler.Zip(tempRepository, toZipFile): string.Empty;
        }
      
        public List<string> DeployArtifacts(FileTransferPacket packet)
        {
            if (packet.Content == null)
                return new List<string>();

            //store file into a temp folder
            string directoryName = Path.Combine(Information.TempDirectoryTempData, DateTime.Now.ToString("yyyyMMddHHmmss") + "_d");
            Directory.CreateDirectory(directoryName);
            string fileName = Path.Combine(directoryName, DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");
            SaveFileStream(fileName, new MemoryStream(packet.Content));
            List<string> files = ZipFileHandler.UnZip(fileName, directoryName);
            File.Delete(fileName);

            //pass over to batch file
            string batchFile = ExtractBatchFile(directoryName);
            Trace.TraceInformation("The system is going to update. Waiting for stop signal...");
            Trace.Flush();

            Eyedia.Core.Windows.Utilities.WindowsUtility.ExecuteBatchFile(batchFile, true);
            //List<string> output = new List<string>();
            //List<string> error = new List<string>();
            //Eyedia.Core.Windows.Utilities.WindowsUtility.ExecuteDosCommand(batchFile, true, ref output, ref error);
            //Trace.TraceInformation("srecmd: Batch file '{0}' was exeucted, output was:{1},{2}Error:{3}{4}",
            //    batchFile, output.ToLine(), Environment.NewLine, error.ToLine(), Environment.NewLine);
            return files;
        }

        #region Helpers
        private string ExtractBatchFile(string tempRepository)
        {            
            string batchFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sreupdate.cmd");
            StreamWriter sw = new StreamWriter(batchFile);
            sw.Write(string.Format(BatchContent, tempRepository, AppDomain.CurrentDomain.BaseDirectory));
            sw.Close();
            return batchFile;
        }
        private void SaveFileStream(string filePath, Stream stream)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Dispose();
        }

        private FileTransferPacket CreatePacket(string zipFile)
        {
            FileTransferPacket packet = new FileTransferPacket();
            packet.FileName = zipFile;
            packet.Content = File.ReadAllBytes(zipFile);
            return packet;
        }

        private string BatchContent
        {
            get
            {
                string strContent = string.Empty;
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Services.EnvironmentService.sreupdate.cmd"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    strContent = reader.ReadToEnd();
                }

                return strContent;
            }
        }

        public static List<string> RequiredFiles
        {
            get
            {
                return new List<string>(__requireddlls.Split(",".ToCharArray()));
            }
        }

        public static List<string> DeploymentArtifacts
        {
            get
            {
                List<string> allFiles = RequiredFiles;

                IdpeKey key = new Manager().GetKey(IdpeKeyTypes.AdditionalDeploymentArtifacts);
                if (key != null)
                    allFiles.AddRange(key.GetUnzippedBinaryValue().Split(",".ToCharArray()));

                return allFiles;
            }
        }


        private bool EnsureFiles(List<string> artifacts, string tempRepository)
        {
            List<string> found = new List<string>(Directory.GetFiles(tempRepository));
            found = found.Select(f => { f = Path.GetFileName(f); return f; }).ToList();
            List<string> artifactsOnlyFiles = artifacts.Select(f => { f = Path.GetFileName(f); return f; }).ToList();
            List<string> notFound = artifactsOnlyFiles.Except(found).ToList();
            if (notFound.Count != 0)
            {
                CallCallBack(string.Format("Could not find {0} files to deploy. Please click 'Artifact' to validate files, ensure that all required files exist in the repository!",
                    notFound.ToLine(",")), true);
                return false;
            }
            else
            {
                return true;
            }            
        }

        private void CallCallBack(string info, bool isError = false)
        {
            if (Callback != null)
                Callback(info, isError);
        }

        private const string __requireddlls = "Eyedia.Security.dll,Eyedia.Core.dll,Eyedia.IDPE.Services.dll,Eyedia.IDPE.Common.dll,Eyedia.IDPE.DataManager.dll,Eyedia.IDPE.Services.WorkflowActivities.dll,idpe.exe,idped.exe,idpec.exe";
        #endregion Helpers

    }
}


