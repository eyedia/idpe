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
using Microsoft.Win32;
using System.Windows.Forms;
using System.Configuration;

namespace Eyedia.Core.Windows.Utilities
{
	/// <summary>
	/// An useful class to read/write/delete/count registry keys
	/// </summary>
	public class RegistryUtility
	{
        public RegistryUtility()
        {
            _subKey = "SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName;
           
        }

        public RegistryUtility(string subKey)
        {
            _subKey = subKey;
        }
		private bool showError = false;
		/// <summary>
		/// A property to show or hide error messages 
		/// (default = false)
		/// </summary>
		public bool ShowError
		{
			get { return showError; }
			set	{ showError = value; }
		}

        private string _subKey;
		/// <summary>
		/// A property to set the SubKey value
		/// (default = "SOFTWARE\\" + Application.ProductName)
		/// </summary>
		public string SubKey
		{
			get { return _subKey; }
            //set	{ _subKey = value; }
		}

		private RegistryKey baseRegistryKey = Registry.LocalMachine;
		/// <summary>
		/// A property to set the BaseRegistryKey value.
		/// (default = Registry.LocalMachine)
		/// </summary>
		public RegistryKey BaseRegistryKey
		{
			get { return baseRegistryKey; }
			set	{ baseRegistryKey = value; }
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// To read a registry key.
		/// input: KeyName (string)
		/// output: value (string) 
		/// </summary>
		public object Read(string KeyName)
		{
			// Opening the registry key
			RegistryKey rk = baseRegistryKey ;
			// Open a subKey as read-only
			RegistryKey sk1 = rk.OpenSubKey(_subKey);
			// If the RegistrySubKey doesn't exist -> (null)
			if ( sk1 == null )
			{
				return null;
			}
			else
			{
				try 
				{
					// If the RegistryKey exists I get its value
					// or null is returned.
					return sk1.GetValue(KeyName);
				}
				catch (Exception e)
				{
					// AAAAAAAAAAARGH, an error!
					ShowErrorMessage(e, "Reading registry " + KeyName);
					return null;
				}
			}
		}	

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// To write into a registry key.
		/// input: KeyName (string) , Value (object)
		/// output: true or false 
		/// </summary>
		public bool Write(string KeyName, object Value)
		{
			try
			{
				// Setting
				RegistryKey rk = baseRegistryKey ;
				// I have to use CreateSubKey 
				// (create or open it if already exits), 
				// 'cause OpenSubKey open a subKey as read-only
				RegistryKey sk1 = rk.CreateSubKey(_subKey);
				// Save the value
				sk1.SetValue(KeyName, Value);

				return true;
			}
			catch (Exception e)
			{
				// AAAAAAAAAAARGH, an error!
				ShowErrorMessage(e, "Writing registry " + KeyName);
				return false;
			}
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// To delete a registry key.
		/// input: KeyName (string)
		/// output: true or false 
		/// </summary>
		public bool DeleteKey(string KeyName)
		{
			try
			{
				// Setting
				RegistryKey rk = baseRegistryKey ;
				RegistryKey sk1 = rk.CreateSubKey(_subKey);
				// If the RegistrySubKey doesn't exists -> (true)
				if ( sk1 == null )
					return true;
				else
					sk1.DeleteValue(KeyName);

				return true;
			}
			catch (Exception e)
			{
				// AAAAAAAAAAARGH, an error!
				ShowErrorMessage(e, "Deleting SubKey " + _subKey);
				return false;
			}
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// To delete a sub key and any child.
		/// input: void
		/// output: true or false 
		/// </summary>
		public bool DeleteSubKeyTree()
		{
			try
			{
				// Setting
				RegistryKey rk = baseRegistryKey ;
				RegistryKey sk1 = rk.OpenSubKey(_subKey);
				// If the RegistryKey exists, I delete it
				if ( sk1 != null )
					rk.DeleteSubKeyTree(_subKey);

				return true;
			}
			catch (Exception e)
			{
				// AAAAAAAAAAARGH, an error!
				ShowErrorMessage(e, "Deleting SubKey " + _subKey);
				return false;
			}
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// Retrive the count of subkeys at the current key.
		/// input: void
		/// output: number of subkeys
		/// </summary>
		public int SubKeyCount()
		{
			try
			{
				// Setting
				RegistryKey rk = baseRegistryKey ;
				RegistryKey sk1 = rk.OpenSubKey(_subKey);
				// If the RegistryKey exists...
				if ( sk1 != null )
					return sk1.SubKeyCount;
				else
					return 0; 
			}
			catch (Exception e)
			{
				// AAAAAAAAAAARGH, an error!
				ShowErrorMessage(e, "Retriving subkeys of " + _subKey);
				return 0;
			}
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// Retrive the count of values in the key.
		/// input: void
		/// output: number of keys
		/// </summary>
		public int ValueCount()
		{
			try
			{
				// Setting
				RegistryKey rk = baseRegistryKey ;
				RegistryKey sk1 = rk.OpenSubKey(_subKey);
				// If the RegistryKey exists...
				if ( sk1 != null )
					return sk1.ValueCount;
				else
					return 0; 
			}
			catch (Exception e)
			{
				// AAAAAAAAAAARGH, an error!
				ShowErrorMessage(e, "Retriving keys of " + _subKey);
				return 0;
			}
		}

		/* **************************************************************************
		 * **************************************************************************/
		
		private void ShowErrorMessage(Exception e, string Title)
		{
			if (showError == true)
				MessageBox.Show(e.Message,
								Title
								,MessageBoxButtons.OK
								,MessageBoxIcon.Error);
		}
	}
}





