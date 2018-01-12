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
using System.Timers;
using System.Net;
using System.Web.Hosting;

namespace Eyedia.IDPE.Services
{
    public class FtpFileSystemWatcher : Watchers
    {
        #region Public Properties

        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string FtpLocationToWatch { get; set; }
        public string DownloadTo { get; set; }
        public bool KeepOrignal { get; set; }
        public bool OverwriteExisting { get; set; }
        public int IntervalInMinutes { get; set; }
        public DateTime LastRunTime { get; set; }
        //private bool DownloadInprogress { get; set; }

        public string FtpLocationToWatchArchive
        {
            get { return string.Format("{0}{1}/{2}/", FtpLocationToWatch, "Archive", DateTime.Now.ToString("yyyyMMdd")); }

        }
        #endregion Public Properties

        //#region private Properties
        //Timer JobProcessor;
        //#endregion private Properties
        
        public FtpFileSystemWatcher(Dictionary<string, object> datasourceParameters, string ftpLocationToWatch, string downloadTo, int intervalInMinutes, string userName, string password, bool keepOrignal, bool overwriteExisting)
        {
            this.DataSourceParameters = datasourceParameters;
            this.FtpUserName = userName;
            this.FtpPassword = password;
            this.FtpLocationToWatch = ftpLocationToWatch;
            this.DownloadTo = downloadTo;
            this.KeepOrignal = keepOrignal;
            this.IntervalInMinutes = intervalInMinutes;
            this.OverwriteExisting = overwriteExisting;
            if (this.IntervalInMinutes < 1)
                this.IntervalInMinutes = 1;
        }
        
        public void Download()
        {           
            if ((DateTime.Now - LastRunTime).TotalMinutes < this.IntervalInMinutes)
                return;

            if (DataSourceIsDisabled())
                return;

            ExtensionMethods.TraceInformation("Ftp downloading T:{0}:{1}", this.DataSourceParameters["DataSourceId"], this.FtpLocationToWatch);
            this.IsRunning = true;
            string[] FilesList = GetFilesList(this.FtpLocationToWatch, this.FtpUserName, this.FtpPassword, this.DataSourceParameters["DataSourceId"].ToString());
            if (FilesList == null || FilesList.Length < 1)
            {
                return;
            }
            foreach (string FileName in FilesList)
            {
                if (!string.IsNullOrEmpty(FileName))
                {                                  
                    DownloadFile(FileName.Trim());
                    if (!this.KeepOrignal)
                    {
                        DeleteFile(FileName.Trim());                     
                    }
                }
            }
            this.LastRunTime = DateTime.Now;
            this.IsRunning = false;
           
        }

        #region Private Methods
        private bool DataSourceIsDisabled()
        {
            int dataSourceId = 0;
            int.TryParse(DataSourceParameters["DataSourceId"].ToString(), out dataSourceId);
            if (dataSourceId == 0)
                return true;

            DataSource dataSource = new DataSource(dataSourceId, string.Empty);
            if (dataSource.IsActive == false)
            {
                ExtensionMethods.TraceInformation("Ftp downloading T:{0} was ignored as the data source was disabled", dataSource.Name);
                return true;
            }
            return false;
        }

        //private void RenameFile(string fileName)
        //{
        //    FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(Path.Combine(FtpLocationToWatch, fileName)));
        //    ftpRequest.Method = System.Net.WebRequestMethods.Ftp.Rename;
        //    ftpRequest.RenameTo = "Archive\\" + fileName;

        //    //CreateFTPDirectory(FtpLocationToWatchArchive);
        //    ftpRequest.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
        //    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
        //    response.Close();
        //}

        private bool CreateFtpDirectory(string directory)
        {
            try
            {
                //create the directory                
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                requestDir.UsePassive = true;
                //requestDir.UseBinary = true;         
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }


        private void DeleteFile(string fileName)
        {
            FtpWebRequest FtpRequest;
            FtpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(Path.Combine(FtpLocationToWatch, fileName)));
            FtpRequest.UseBinary = true;
            FtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpRequest.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
            FtpWebResponse response = (FtpWebResponse)FtpRequest.GetResponse();
            response.Close();
        }

        private void DownloadFile(string fileName)
        {
            string completeFileName = Path.Combine(DownloadTo, fileName);
            string renamedToIdentifier = string.Empty;
            if (File.Exists(completeFileName))
            {
                if (OverwriteExisting)
                {
                    renamedToIdentifier = Guid.NewGuid().ToString();
                    File.Copy(completeFileName, Path.Combine(DownloadTo, string.Format("{0}_{1}", renamedToIdentifier, fileName)));
                    File.Delete(completeFileName);
                }
                else
                {
                    Console.WriteLine(string.Format("File {0} already exist.", fileName));
                    return;
                }
            }

            FtpWebRequest reqFTP;

            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(DownloadTo, fileName)));
            FileStream outputStream = new FileStream(Path.Combine(DownloadTo, fileName), FileMode.CreateNew, FileAccess.ReadWrite);


            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(Path.Combine(FtpLocationToWatch, fileName)));
            reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            long cl = response.ContentLength;
            int bufferSize = 2048;
            int readCount;
            byte[] buffer = new byte[bufferSize];

            //readCount = ftpStream.Read(buffer, 0, bufferSize);
            //while (readCount > 0)
            while(true)
            {
                //outputStream.Write(buffer, 0, readCount);
                //readCount = ftpStream.Read(buffer, 0, bufferSize);

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                if (readCount == 0)
                    break;

                outputStream.Write(buffer, 0, readCount);
                
            }

            ftpStream.Close();
            outputStream.Close();
            response.Close();

            InvokeFileDownloaded(new FileSystemWatcherEventArgs(DataSourceParameters, completeFileName, renamedToIdentifier));

        }

        private string[] GetFilesList(string ftpFolderPath, string userName, string password, string dataSourceId)
        {

            try
            {
                FtpWebRequest Request;
                FtpWebResponse Response;
                Request = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpFolderPath));
                Request.Credentials = new NetworkCredential(userName, password);
                Request.Proxy = null;
                Request.Method = WebRequestMethods.Ftp.ListDirectory;
                Request.UseBinary = true;
                Response = (FtpWebResponse)Request.GetResponse();
                StreamReader reader = new StreamReader(Response.GetResponseStream());
                string Data = reader.ReadToEnd();
                return Data.Split('\n');
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error while pulling data from FTP! DataSource Id= {0} URL = {1}, User Name= {2}, Password = ******", 
                    dataSourceId, ftpFolderPath, userName);
                ExtensionMethods.TraceError(errorMessage + Environment.NewLine + ex.ToString());
                return new List<string>().ToArray();
            }

        }

        #endregion Private Methods
    }

    
}





