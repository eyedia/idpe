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
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace Eyedia.Core.Web
{
    public class Utilities
    {
        public static Control FindControl(Control root, string id)
        {
            Control ctl = root;
            LinkedList<Control> ctls = new LinkedList<Control>();

            while (ctl != null)
            {
                if (ctl.ID == id)
                    return ctl;
                foreach (Control child in ctl.Controls)
                {
                    if (child.ID == id)
                        return child;
                    if (child.HasControls())
                        ctls.AddLast(child);
                }
                ctl = ctls.First.Value;
                ctls.Remove(ctl);
            }
            return null;
        }

        public static bool IsWebHosted() 
        {   
            try   
            {     
                var webAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("System.Web"));     
                foreach(var webAssembly in webAssemblies)     
                {       
                    var hostingEnvironmentType = webAssembly.GetType("System.Web.Hosting.HostingEnvironment");       
                    if (hostingEnvironmentType != null)       
                    {         
                        var isHostedProperty = hostingEnvironmentType.GetProperty("IsHosted",BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public);         
                        if (isHostedProperty != null)         
                        {           
                            object result = isHostedProperty.GetValue(null, null);           
                            if (result is bool)           
                            {             
                                return (bool) result;           
                            }         
                        }       
                    }     
                }   
            }   
            catch (Exception)   
            {     // Failed to find or execute HostingEnvironment.IsHosted; assume false   
            }   
            return false; 
        }
        
    }
}





