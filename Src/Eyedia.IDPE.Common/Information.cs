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
using System.Configuration;
using System.Diagnostics;
using System.Web.Hosting;
using System.Web.UI;

using CultureInfo = System.Globalization.CultureInfo;
using Eyedia.Core;
using Eyedia.Core.Data;
using System.IO;

namespace Eyedia.IDPE.Common
{
    public class Information
    {

        /// <summary>
        /// Dictionary to store common sql queries
        /// </summary>
        public static Dictionary<string, string> SQLQueries { get; set; }

        /// <summary>
        /// Windows event log source
        /// </summary>
        public const string EventLogSource = "IDPE";

        /// <summary>
        /// Windows event log group name
        /// </summary>
        public const string EventLogName = "Application";
        
        /// <summary>
        /// eyediaCoreConfigurationSection from config file
        /// </summary>
        public static EyediaCoreConfigurationSection EyediaCoreConfigurationSection
        {
            get
            {
                return (EyediaCoreConfigurationSection)ConfigurationManager.GetSection("eyediaCoreConfigurationSection");
            }
        }

        /// <summary>
        /// IdpeConfigurationSection from config file
        /// </summary>
        public static IdpeConfigurationSection IdpeConfigurationSection
        {
            get
            {             
                return (IdpeConfigurationSection)ConfigurationManager.GetSection("idpeConfigurationSection");
            }
        }

        public static bool IsInternalRule(string ruleName)
        {
            return ruleName.Equals("DuplicateCheck", StringComparison.OrdinalIgnoreCase);
        }

        public static string TempDirectoryIdpe
        {
            get
            {
                string dir = Path.Combine(EyediaCoreConfigurationSection.CurrentConfig.TempDirectory, Constants.IdpeBaseFolderName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                return dir;
            }
        }

        public static string TempDirectoryTempData
        {
            get
            {
                string dir = Path.Combine(TempDirectoryIdpe, "TempData");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                return dir;
            }
        }

        static User _LoggedInUser;
        public static User LoggedInUser
        {
            get
            {
                if(_LoggedInUser == null)
                {
                    //this happens if this property is accessed from service
                    _LoggedInUser = new User();
                    _LoggedInUser.UserName = "System";
                    _LoggedInUser.FullName = "IDPE Service";                    
                }
                return _LoggedInUser;
            }
            set
            {
                _LoggedInUser = value;
            }
        }

        public static SourceLevels TraceFilter
        {
            get
            {
                if (Trace.Listeners.Count > 0)
                {
                    System.Diagnostics.EventTypeFilter currentFilter = Trace.Listeners[0].Filter as EventTypeFilter;
                    return currentFilter.EventType;
                }
                else
                    return SourceLevels.Off;
            }
        }
        
    }
}




