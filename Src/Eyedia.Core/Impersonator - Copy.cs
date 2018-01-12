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
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace Symplus.Core
{

    /// <summary>
    /// Impersonate a windows logon.
    /// </summary>
    public class Impersonator
    {

        /// <summary>
        /// Impersonate given logon information.
        /// </summary>
        /// <param name="logon">Windows logon name.</param>
        /// <param name="password">password</param>
        /// <param name="domain">domain name</param>
        /// <returns></returns>
        public static bool Impersonate(string logon, string password, string
        domain)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (LogonUser(logon, domain, password, LOGON32_LOGON_INTERACTIVE,
            LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {

                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (null != impersonationContext) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Unimpersonate.
        /// </summary>
        public static void UnImpersonate()
        {
            impersonationContext.Undo();
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int LogonUser(
        string lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);

        [DllImport("advapi32.dll",
        CharSet = System.Runtime.InteropServices.CharSet.Auto,
        SetLastError = true)]
        public extern static int DuplicateToken(
        IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_LOGON_NETWORK_CLEARTEXT = 4;
        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private static WindowsImpersonationContext impersonationContext;
    }
}



