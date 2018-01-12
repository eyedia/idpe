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




#if WEB
namespace Eyedia.IDPE.Services
{
    #region Imports

    using System;
    using System.IO;
    using System.Globalization;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Configuration;
    using System.Web.UI.HtmlControls;
    using System.Collections;
    using System.Web;    

    #endregion

    /// <summary>
    /// Renders an HTML page that presents information about the version,
    /// build configuration, source files as well as a method to check
    /// for updates.
    /// </summary>

    internal sealed class LogArchivePage : BasePage
    {
        string _logRelativePath;
        public LogArchivePage(string logFileRelativePath)
        {
            PageTitle = "SRE Monitor - Archived Logs";
            _logRelativePath = logFileRelativePath;
        }
       
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
          
            //
            // Title
            //

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "PageTitle");
            writer.RenderBeginTag(HtmlTextWriterTag.H1);
            writer.Write(PageTitle);
            writer.RenderEndTag(); // </h1>
            writer.WriteLine();

          
            //
            // Content...
            //
         
            WriteLogs(writer);            
          
        }

        #region WriteLogs

        void WriteLogs(HtmlTextWriter writer)
        {

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "hilite");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hilite");

            string logContent = string.Empty;
            DateTime dtModifiedAt = DateTime.Now;
            try
            {
                using (StreamReader sr = new StreamReader(_logRelativePath))
                {
                    logContent = sr.ReadToEnd();
                    sr.Close();
                }
                dtModifiedAt = File.GetLastWriteTime(_logRelativePath);
            }
            catch { }

            if (!string.IsNullOrEmpty(logContent))
            {                
                writer.Write("<strong>");
                writer.Write("<tr><td>File Name</td><td>{0}</td></tr>", _logRelativePath);
                writer.Write("<tr><td>Archived on </td><td>{0}</td></tr>", dtModifiedAt.ToString());
                writer.Write("</strong>");
                writer.Write("</table>");
                writer.Write("<table style=\"width:100%;table-layout:fixed\"><tr><td><textarea  style=\"width:99%;height:500px;background-color:#E6E6E6;\">");
                writer.Write(logContent);
                writer.Write("</textarea></td></tr></table>");
            }
            writer.RenderEndTag(); //</table>
        }
        
        #endregion WriteLogs
        
    }
}
#endif





