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
    using System.Security;
    using System.Web;
    using System.Diagnostics;

    #endregion

    internal sealed class HttpModuleRegistry
    {
        private static Hashtable _moduleListByApp;
        private static readonly object _lock = new object();

        public static bool RegisterInPartialTrust(HttpApplication application, IHttpModule module)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            if (module == null)
                throw new ArgumentNullException("module");

            if (IsHighlyTrusted())
                return false;
            
            lock (_lock)
            {
                //
                // On-demand allocate a map of modules per application.
                //

                if (_moduleListByApp == null)
                    _moduleListByApp = new Hashtable();

                //
                // Get the list of modules for the application. If this is
                // the first registration for the supplied application object
                // then setup a new and empty list.
                //

                IList moduleList = (IList) _moduleListByApp[application];
                
                if (moduleList == null)
                {
                    moduleList = new ArrayList(4);
                    _moduleListByApp.Add(application, moduleList);
                }
                else if (moduleList.Contains(module))
                    throw new ApplicationException("Duplicate module registration.");

                //
                // Add the module to list of registered modules for the 
                // given application object.
                //

                moduleList.Add(module);
            }

            //
            // Setup a closure to automatically unregister the module
            // when the application fires its Disposed event.
            //

            Housekeeper housekeeper = new Housekeeper(module);
            application.Disposed += new EventHandler(housekeeper.OnApplicationDisposed);

            return true;
        }

        private static bool UnregisterInPartialTrust(HttpApplication application, IHttpModule module)
        {
            Debug.Assert(application != null);
            Debug.Assert(module != null);

            if (module == null)
                throw new ArgumentNullException("module");

            if (IsHighlyTrusted())
                return false;

            lock (_lock)
            {
                //
                // Get the module list for the given application object.
                //

                if (_moduleListByApp == null)
                    return false;
                
                IList moduleList = (IList) _moduleListByApp[application];
                
                if (moduleList == null)
                    return false;

                //
                // Remove the module from the list if it's in there.
                //

                int index = moduleList.IndexOf(module);

                if (index < 0)
                    return false;

                moduleList.RemoveAt(index);

                //
                // If the list is empty then remove the application entry.
                // If this results in the entire map becoming empty then
                // release it.
                //

                if (moduleList.Count == 0)
                {
                    _moduleListByApp.Remove(application);

                    if (_moduleListByApp.Count == 0)
                        _moduleListByApp = null;
                }
            }

            return true;
        }

        public static ICollection GetModules(HttpApplication application)
        {
            if (application == null)
                throw new ArgumentNullException("application");
            
            try
            {
                IHttpModule[] modules = new IHttpModule[application.Modules.Count];
                application.Modules.CopyTo(modules, 0);
                return modules;
            }
            catch (SecurityException)
            {
                //
                // Pass through because probably this is a partially trusted
                // environment that does not have access to the modules 
                // collection over HttpApplication so we have to resort
                // to our own devices...
                //
            }
            
            lock (_lock)
            {
                if (_moduleListByApp == null)
                    return new IHttpModule[0];

                IList moduleList = (IList) _moduleListByApp[application];

                if (moduleList == null)
                    return new IHttpModule[0];
                
                IHttpModule[] modules = new IHttpModule[moduleList.Count];
                moduleList.CopyTo(modules, 0);
                return modules;
            }
        }
        
        private static bool IsHighlyTrusted() 
        {
#if NET_1_0
            //
            // ASP.NET 1.0 applications always required and ran under full 
            // trust so we just return true here.
            //

            return true;
#else
            try
            {
                AspNetHostingPermission permission = new AspNetHostingPermission(AspNetHostingPermissionLevel.High);
                permission.Demand();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
#endif
        }

        private HttpModuleRegistry()
        {
            throw new NotSupportedException();
        }

        internal sealed class Housekeeper
        {
            private readonly IHttpModule _module;

            public Housekeeper(IHttpModule module)
            {
                _module = module;
            }

            public void OnApplicationDisposed(object sender, EventArgs e)
            {
                UnregisterInPartialTrust((HttpApplication) sender, _module);
            }
        }
    }
}

#endif





