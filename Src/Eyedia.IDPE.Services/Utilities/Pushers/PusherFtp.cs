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
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Eyedia.IDPE.Services
{
    /// <summary>
    /// FTP type pusher - uploads file into FTP when processed
    /// </summary>
    public class PusherFtp: Pushers
    {
        /// <summary>
        /// Default instance of PusherFtp 
        /// </summary>
        public PusherFtp() { }
        
        /// <summary>
        /// Uploads processed file into FTP
        /// </summary>
        /// <param name="e">PullerEventArgs</param>
        public override void FileProcessed(PullersEventArgs e)
        {
            string ftpRemoteLocation = string.Empty;
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;
            string onlyFileName = Path.GetFileName(e.OutputFileName);

            if (e.Job.DataSource.PusherTypeFullName.Contains("|"))
            {
                if (e.Job.DataSource.PusherTypeFullName.Split("|".ToCharArray()).Length == 3)
                {
                    ftpRemoteLocation = e.Job.DataSource.PusherTypeFullName.Split("|".ToCharArray())[0];
                    ftpUserName = e.Job.DataSource.PusherTypeFullName.Split("|".ToCharArray())[1];
                    ftpPassword = e.Job.DataSource.PusherTypeFullName.Split("|".ToCharArray())[2];
                }
            }

            if ((string.IsNullOrEmpty(ftpRemoteLocation))
                || (string.IsNullOrEmpty(ftpRemoteLocation))
            || (string.IsNullOrEmpty(ftpRemoteLocation)))
            {
                e.Job.TraceError("Ftp pusher was not configured correctly! Upload failed for " + e.OutputFileName);
                return;
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpRemoteLocation + "/" + onlyFileName);
            request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            StreamReader sourceStream = new StreamReader(e.OutputFileName);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();

        }

        
    }
}


