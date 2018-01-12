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





namespace Eyedia.IDPE.Services
{
    #region Imports

    using System;
    using System.Web;
    //using System.Reflection;
    using Stream = System.IO.Stream;
    using Encoding = System.Text.Encoding;
    using System.Diagnostics;
    using System.Reflection;

    #endregion

    /// <summary>
    /// Reads a resource from the assembly manifest and returns its contents
    /// as the response entity.
    /// </summary>

    internal sealed class ManifestResourceHandler : IHttpHandler
    {
        private readonly string _resourceName;
        private readonly string _contentType;
        private readonly Encoding _responseEncoding;

        public ManifestResourceHandler(string resourceName, string contentType) :
            this(resourceName, contentType, null) {}

        public ManifestResourceHandler(string resourceName, string contentType, Encoding responseEncoding)
        {

            _resourceName = resourceName;
            _contentType = contentType;
            _responseEncoding = responseEncoding;
        }

        public void ProcessRequest(HttpContext context)
        {
            //
            // Set the response headers for indicating the content type 
            // and encoding (if specified).
            //

            HttpResponse response = context.Response;
            response.ContentType = _contentType;

            if (_responseEncoding != null)
                response.ContentEncoding = _responseEncoding;

            WriteResourceToStream(response.OutputStream, _resourceName);
        }

        public void WriteResourceToStream(Stream outputStream, string resourceName)
        {
            //
            // Grab the resource stream from the manifest.
            //
          
            string[] resourceNames = Assembly.GetCallingAssembly().GetManifestResourceNames();
            foreach (string rn in resourceNames)
            {
                if(rn.Contains(resourceName))
                {
                    resourceName = rn;
                    break;
                }
            }

            using (Stream inputStream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName))
            {

                //
                // Allocate a buffer for reading the stream. The maximum size
                // of this buffer is fixed to 4 KB.
                //

                byte[] buffer = new byte[Math.Min(inputStream.Length, 4096)];

                //
                // Finally, write out the bytes!
                //

                int readLength = inputStream.Read(buffer, 0, buffer.Length);

                while (readLength > 0)
                {
                    outputStream.Write(buffer, 0, readLength);
                    readLength = inputStream.Read(buffer, 0, buffer.Length);
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}





