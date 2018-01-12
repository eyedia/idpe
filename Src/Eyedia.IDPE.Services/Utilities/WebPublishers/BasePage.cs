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
    using System.Web.UI;

    using CultureInfo = System.Globalization.CultureInfo;
    
    #endregion

    /// <summary>
    /// Provides the base implementation and layout for most pages that render 
    /// HTML for the error log.
    /// </summary>

    internal abstract class BasePage : Page
    {
        private string _title;
        //private ErrorLog _log;

        protected string BasePageName
        {
            get { return this.Request.ServerVariables["URL"]; }
        }

        //protected virtual ErrorLog ErrorLog
        //{
        //    get
        //    {
        //        if (_log == null)
        //            _log = ErrorLog.GetDefault(Context);

        //        return _log;
        //    }
        //}

        protected virtual string PageTitle
        {
            get { return Mask.NullString(_title); }
            set { _title = value; }
        }

        //protected virtual string ApplicationName
        //{
        //    get { return this.ErrorLog.ApplicationName; }
        //}

        protected virtual void RenderDocumentStart(HtmlTextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");

            writer.AddAttribute("xmlns", "http://www.w3.org/1999/xhtml");
            writer.RenderBeginTag(HtmlTextWriterTag.Html);  // <html>
            
            writer.RenderBeginTag(HtmlTextWriterTag.Head);  // <head>
            RenderHead(writer);
            writer.RenderEndTag();                          // </head>
            writer.WriteLine();

            writer.RenderBeginTag(HtmlTextWriterTag.Body);  // <body>
        }

        protected virtual void RenderHead(HtmlTextWriter writer)
        {
            //
            // In IE 8 or later, mimic IE 7
            // http://msdn.microsoft.com/en-us/library/cc288325.aspx#DCModes
            //

            writer.AddAttribute("http-equiv", "X-UA-Compatible");
            writer.AddAttribute("content", "IE=EmulateIE7");
            writer.RenderBeginTag(HtmlTextWriterTag.Meta);
            writer.RenderEndTag();
            writer.WriteLine();

            //
            // Write the document title.
            //

            writer.RenderBeginTag(HtmlTextWriterTag.Title);
            Server.HtmlEncode(this.PageTitle, writer);
            writer.RenderEndTag();
            writer.WriteLine();

            //
            // Write a <link> tag to relate the style sheet.
            //

#if NET_1_0 || NET_1_1
            writer.AddAttribute("rel", "stylesheet");
#else
            writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
#endif
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, this.BasePageName + "/sre.css");
            //writer.AddAttribute(HtmlTextWriterAttribute.Href, this.Context.Request.Path + this.BasePageName + "/stylesheet");
            writer.RenderBeginTag(HtmlTextWriterTag.Link);
            writer.RenderEndTag();
            writer.WriteLine();

            //writer.AddAttribute(HtmlTextWriterAttribute.sc, "script");
            //writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");            
            //writer.AddAttribute(HtmlTextWriterAttribute.Href, this.Context.Request.Path + "/js");
            //writer.RenderBeginTag(HtmlTextWriterTag.Link);
            //writer.RenderEndTag();
            //writer.WriteLine();
            writer.Write("<script src=\"{0}/sre.js\" type=\"text/javascript\"></script>", this.BasePageName);
        }

        protected virtual void RenderDocumentEnd(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "Footer");
            writer.RenderBeginTag(HtmlTextWriterTag.P); // <p>

            //
            // Write the powered-by signature, that includes version information.
            //

            //PoweredBy poweredBy = new PoweredBy();
            //poweredBy.RenderControl(writer);

            //
            // Write out server date, time and time zone details.
            //

            DateTime now = DateTime.Now;

            writer.Write("Server date is ");
            this.Server.HtmlEncode(now.ToString("D", CultureInfo.InvariantCulture), writer);

            writer.Write(". Server time is ");
            this.Server.HtmlEncode(now.ToString("T", CultureInfo.InvariantCulture), writer);

            writer.Write(". All dates and times displayed are in the ");
            writer.Write(TimeZone.CurrentTimeZone.IsDaylightSavingTime(now) ?
                TimeZone.CurrentTimeZone.DaylightName : TimeZone.CurrentTimeZone.StandardName);
            writer.Write(" zone. ");
          

            writer.RenderEndTag(); // </p>

            writer.RenderEndTag(); // </body>
            writer.WriteLine();

            writer.RenderEndTag(); // </html>
            writer.WriteLine();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderDocumentStart(writer);
            RenderContents(writer);
            RenderDocumentEnd(writer);
        }

        protected virtual void RenderContents(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}

#endif





