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
    using System.Collections;
    using System.Web;
    using System.Diagnostics;

    using CultureInfo = System.Globalization.CultureInfo;
    using Encoding = System.Text.Encoding;
    using System.Web.Configuration;
    using System.Configuration;
    using Eyedia.Core;
    using Eyedia.IDPE.Common;

    #endregion

    /// <summary>
    /// HTTP handler factory that dispenses handlers for rendering views and 
    /// resources needed to display the error log.
    /// </summary>

    public class PageFactory : IHttpHandlerFactory
    {
        private static readonly object _authorizationHandlersKey = new object();
        private static readonly IRequestAuthorizationHandler[] _zeroAuthorizationHandlers = new IRequestAuthorizationHandler[0];

        /// <summary>
        /// Returns an object that implements the <see cref="IHttpHandler"/> 
        /// interface and which is responsible for serving the request.
        /// </summary>
        /// <returns>
        /// A new <see cref="IHttpHandler"/> object that processes the request.
        /// </returns>

        public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            //
            // The request resource is determined by the looking up the
            // value of the PATH_INFO server variable.
            //

            string resource = context.Request.PathInfo.Length == 0 ? string.Empty :
                context.Request.PathInfo.Substring(1).ToLower(CultureInfo.InvariantCulture);

            if(string.IsNullOrEmpty(resource))
                resource = context.Request.CurrentExecutionFilePath.Replace(context.Request.ApplicationPath, "");

            HandleQueryParams(context);
           
            if ((resource.Contains(".png"))
                || (resource.Contains(".jpg"))
                || (resource.Contains(".js"))
                || (resource.Contains(".css")))
            {
                if (resource.LastIndexOf("/") > -1)
                    resource = resource.Substring(resource.LastIndexOf("/"));
            }
            
            if (resource.StartsWith("/"))
                resource = resource.Substring(1);

         
            IHttpHandler handler = FindHandler(resource);

            if (handler == null)
                throw new HttpException(404, "Resource not found.");

            //
            // Check if authorized then grant or deny request.
            //

            int authorized = IsAuthorized(context);
            //if (authorized == 0
            //    || (authorized < 0 // Compatibility case...
            //        && !HttpRequestSecurity.IsLocal(context.Request)
            //        && !SecurityConfiguration.Default.AllowRemoteAccess))
            if(authorized == 0)
            {
                //(new ManifestResourceHandler("RemoteAccessError.htm", "text/html")).ProcessRequest(context);
                HttpResponse response = context.Response;
                response.Status = "403 Forbidden";
                response.End();

                //
                // HttpResponse.End docs say that it throws 
                // ThreadAbortException and so should never end up here but
                // that's not been the observation in the debugger. So as a
                // precautionary measure, bail out anyway.
                //

                return null;
            }

            return handler;
        }

        void HandleQueryParams(HttpContext context)
        {
            if (context.Request.QueryString["pullers"] != null)
            {

                switch (context.Request.QueryString["pullers"])
                {
                    case "0":
                        new Idpe().StartPullers();
                        break;

                    case "1":
                        new Idpe().StopPullers();
                        break;

                    case "2":
                        Idpe psc = new Idpe();
                        psc.StopPullers();
                        psc.StartPullers();
                        break;
                }

            }
            if (context.Request.QueryString["log"] != null)
            {

                switch (context.Request.QueryString["log"])
                {
                    case "1":
                        //Registry.Instance.ClearLog();
                        SetupTrace.Clear(Information.EventLogSource, Information.EventLogName);
                        break;

                }

            }

            if (context.Request.QueryString["cache"] != null)
            {
                var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                EyediaCoreConfigurationSection coreConfig = (EyediaCoreConfigurationSection)configuration.GetSection("eyediaCoreConfigurationSection");
                coreConfig.Cache = context.Request.QueryString["cache"] == "1" ? true : false;
                configuration.Save();                
            }

            if (context.Request.QueryString["logname"] != null)
            {
                _ArchiveLogRelativePath = context.Request.QueryString["logname"];
            }
        }
        static string _ArchiveLogRelativePath;

        private static IHttpHandler FindHandler(string name)
        {
            Debug.Assert(name != null);

            switch (name)
            {
            
                case "sre.js":
                    return new ManifestResourceHandler("sre.js",
                        "text/css", Encoding.GetEncoding("Windows-1252"));

                case "running.png":
                    return new ManifestResourceHandler("running.png",
                        "text/css", Encoding.GetEncoding("Windows-1252"));

                case "stopped.png":
                    return new ManifestResourceHandler("stopped.png",
                        "text/css", Encoding.GetEncoding("Windows-1252"));
                    
                case "sre.css":
                    return new ManifestResourceHandler("sre.css",
                        "text/css", Encoding.GetEncoding("Windows-1252"));

                case "archivelog":
                    return new LogArchivePage(_ArchiveLogRelativePath);

                case "monitor":
                default:
                    return new MonitorPage();
          
            }
        }

        /// <summary>
        /// Enables the factory to reuse an existing handler instance.
        /// </summary>

        public virtual void ReleaseHandler(IHttpHandler handler)
        {
        }

        /// <summary>
        /// Determines if the request is authorized by objects implementing
        /// <see cref="IRequestAuthorizationHandler" />.
        /// </summary>
        /// <returns>
        /// Returns zero if unauthorized, a value greater than zero if 
        /// authorized otherwise a value less than zero if no handlers
        /// were available to answer.
        /// </returns>

        private static int IsAuthorized(HttpContext context)
        {
            Debug.Assert(context != null);

            int authorized = /* uninitialized */ -1;
            IEnumerator authorizationHandlers = GetAuthorizationHandlers(context).GetEnumerator();
            while (authorized != 0 && authorizationHandlers.MoveNext())
            {
                IRequestAuthorizationHandler authorizationHandler = (IRequestAuthorizationHandler)authorizationHandlers.Current;
                authorized = authorizationHandler.Authorize(context) ? 1 : 0;
            }
            return authorized;
        }

        private static IList GetAuthorizationHandlers(HttpContext context)
        {
            Debug.Assert(context != null);

            object key = _authorizationHandlersKey;
            IList handlers = (IList)context.Items[key];

            if (handlers == null)
            {
                const int capacity = 4;
                ArrayList list = null;

                HttpApplication application = context.ApplicationInstance;
                if (application is IRequestAuthorizationHandler)
                {
                    list = new ArrayList(capacity);
                    list.Add(application);
                }

                foreach (IHttpModule module in HttpModuleRegistry.GetModules(application))
                {
                    if (module is IRequestAuthorizationHandler)
                    {
                        if (list == null)
                            list = new ArrayList(capacity);
                        list.Add(module);
                    }
                }

                context.Items[key] = handlers = ArrayList.ReadOnly(
                    list != null
                    ? list.ToArray(typeof(IRequestAuthorizationHandler))
                    : _zeroAuthorizationHandlers);
            }

            return handlers;
        }

        internal static Uri GetRequestUrl(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Uri url = context.Items["SRE_REQUEST_URL"] as Uri;
            return url != null ? url : context.Request.Url;
        }
    }

    public interface IRequestAuthorizationHandler
    {
        bool Authorize(HttpContext context);
    }
}
#endif





