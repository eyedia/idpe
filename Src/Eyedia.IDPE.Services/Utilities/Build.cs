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




using System.Reflection;
using System.Collections.Generic;
namespace Eyedia.IDPE.Services
{
    internal sealed class Build
    {
        private Build() { }

#if DEBUG
        public const bool IsDebug = true;
        public const bool IsRelease = !IsDebug;
        public const string Type = "Debug";
        public const string TypeUppercase = "DEBUG";
        public const string TypeLowercase = "debug";
#else
        public const bool IsDebug = false;
        public const bool IsRelease = !IsDebug;
        public const string Type = "Release";
        public const string TypeUppercase = "RELEASE";
        public const string TypeLowercase = "release";
#endif

#if NET_1_0
        public const string Framework = "net-1.0";
#elif NET_1_1
        public const string Framework = "net-1.1";
#elif NET_2_0
        public const string Framework = "net-2.0";
#elif NET_3_5
        public const string Framework = "net-3.5";
#elif NET_4_0
        public const string Framework = "net-4.0";
#else
        public const string Framework = "unknown";
#endif

        public const string Configuration = TypeLowercase + "; " + Status + "; " + Framework;

        /// <summary>
        /// Gets a string representing the version of the CLR saved in 
        /// the file containing the manifest. Under 1.0, this returns
        /// the hard-wired string "v1.0.3705".
        /// </summary>

        public static string ImageRuntimeVersion
        {
            get
            {
#if NET_1_0
                //
                // As Assembly.ImageRuntimeVersion property was not available
                // under .NET Framework 1.0, we just return the version 
                // hard-coded based on conditional compilation symbol.
                //

                return "v1.0.3705";
#else
                //return typeof(ErrorLog).Assembly.ImageRuntimeVersion;
                return "";
#endif
            }
        }

        /// <summary>
        /// This is the status or milestone of the build. Examples are
        /// M1, M2, ..., Mn, BETA1, BETA2, RC1, RC2, RTM.
        /// </summary>

        public static string[] DLLVersions
        {
            get
            {
                
                List<string> vers = new List<string>();
                vers.Add(Assembly.GetCallingAssembly().GetName().ToString());

                AssemblyName[] assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
                foreach (AssemblyName assemblyName in assemblyNames)
                {
                    string name = assemblyName.ToString();
                    if ((name.Contains("Eyedia"))
                        || (name.Contains("Debjyoti")))
                        vers.Add(name);
                }
                return vers.ToArray();

            }
        }
        public const string Status = "RTM";
    }
}





