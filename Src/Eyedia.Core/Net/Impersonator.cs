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
using System.ComponentModel;

namespace Eyedia.Core.Net
{

	public class Impersonator : IDisposable
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// Please note that the account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public Impersonator(
			string userName,
			string domainName,
			string password )
		{
			ImpersonateValidUser( userName, domainName, password );
		}

		// ------------------------------------------------------------------
		#endregion

		#region IDisposable member.
		// ------------------------------------------------------------------

		public void Dispose()
		{
			UndoImpersonation();
		}

		// ------------------------------------------------------------------
		#endregion

		#region P/Invoke.
		// ------------------------------------------------------------------

		[DllImport("advapi32.dll", SetLastError=true)]
		private static extern int LogonUser(
			string lpszUserName,
			string lpszDomain,
			string lpszPassword,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken);
		
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int DuplicateToken(
			IntPtr hToken,
			int impersonationLevel,
			ref IntPtr hNewToken);

		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern bool RevertToSelf();

		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		private static extern  bool CloseHandle(
			IntPtr handle);

		private const int LOGON32_LOGON_INTERACTIVE = 2;
		private const int LOGON32_PROVIDER_DEFAULT = 0;

		// ------------------------------------------------------------------
		#endregion

		#region Private member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		private void ImpersonateValidUser(
			string userName, 
			string domain, 
			string password )
		{
			WindowsIdentity tempWindowsIdentity = null;
			IntPtr token = IntPtr.Zero;
			IntPtr tokenDuplicate = IntPtr.Zero;

			try
			{
				if ( RevertToSelf() )
				{
					if ( LogonUser(
						userName, 
						domain, 
						password, 
						LOGON32_LOGON_INTERACTIVE,
						LOGON32_PROVIDER_DEFAULT, 
						ref token ) != 0 )
					{
						if ( DuplicateToken( token, 2, ref tokenDuplicate ) != 0 )
						{
							tempWindowsIdentity = new WindowsIdentity( tokenDuplicate );
							impersonationContext = tempWindowsIdentity.Impersonate();
						}
						else
						{
							throw new Win32Exception( Marshal.GetLastWin32Error() );
						}
					}
					else
					{
						throw new Win32Exception( Marshal.GetLastWin32Error() );
					}
				}
				else
				{
					throw new Win32Exception( Marshal.GetLastWin32Error() );
				}
			}
			finally
			{
				if ( token!= IntPtr.Zero )
				{
					CloseHandle( token );
				}
				if ( tokenDuplicate!=IntPtr.Zero )
				{
					CloseHandle( tokenDuplicate );
				}
			}
		}

		/// <summary>
		/// Reverts the impersonation.
		/// </summary>
		private void UndoImpersonation()
		{
			if ( impersonationContext!=null )
			{
				impersonationContext.Undo();
			}	
		}

		private WindowsImpersonationContext impersonationContext = null;

		// ------------------------------------------------------------------
		#endregion
	}
	
}


