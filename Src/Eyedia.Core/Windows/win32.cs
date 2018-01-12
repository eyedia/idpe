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
using System.Runtime.InteropServices;

using System.Drawing;


namespace Eyedia.Core.Windows
{
	/// <summary>
	/// Win32 support code.
	/// (C) 2003 Bob Bradley / ZBobb@hotmail.com
	/// </summary>
	public class win32
	{
		 
		public const int  WM_MOUSEMOVE      =              0x0200;
		public const int  WM_LBUTTONDOWN     =             0x0201;
		public const int  WM_LBUTTONUP       =             0x0202;
		public const int  WM_RBUTTONDOWN     =             0x0204;
		public const int  WM_LBUTTONDBLCLK   =             0x0203;

		public const int  WM_MOUSELEAVE      =             0x02A3;


		
		public const int WM_PAINT     =                   0x000F;
		public const int WM_ERASEBKGND   =                0x0014;
		
		public const int WM_PRINT         =               0x0317;

		//const int EN_HSCROLL       =   0x0601;
		//const int EN_VSCROLL       =   0x0602;

		public const int WM_HSCROLL       =              0x0114;
		public const int WM_VSCROLL       =              0x0115;


		public const int EM_GETSEL              = 0x00B0;
		public const int EM_LINEINDEX           = 0x00BB;
		public const int EM_LINEFROMCHAR        = 0x00C9;

		public const int EM_POSFROMCHAR         = 0x00D6;



		[DllImport("USER32.DLL", EntryPoint= "PostMessage")]
		public static extern bool PostMessage(IntPtr hwnd, uint msg,
			IntPtr wParam, IntPtr lParam);

		/*
			BOOL PostMessage(          HWND hWnd,
				UINT Msg,
				WPARAM wParam,
				LPARAM lParam
				);
		*/

		// Put this declaration in your class   //IntPtr
		[DllImport("USER32.DLL", EntryPoint= "SendMessage")]
		public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam,
			IntPtr lParam);



		
		[DllImport("USER32.DLL", EntryPoint= "GetCaretBlinkTime")]
		public static extern uint GetCaretBlinkTime();




		const int WM_PRINTCLIENT	= 0x0318;

		const long PRF_CHECKVISIBLE=0x00000001L;
		const long PRF_NONCLIENT	= 0x00000002L;
		const long PRF_CLIENT		= 0x00000004L;
		const long PRF_ERASEBKGND	= 0x00000008L;
		const long PRF_CHILDREN	= 0x00000010L;
		const long PRF_OWNED		= 0x00000020L;

		/*  Will clean this up later doing something like this
		enum  CaptureOptions : long
		{
			PRF_CHECKVISIBLE= 0x00000001L,
			PRF_NONCLIENT	= 0x00000002L,
			PRF_CLIENT		= 0x00000004L,
			PRF_ERASEBKGND	= 0x00000008L,
			PRF_CHILDREN	= 0x00000010L,
			PRF_OWNED		= 0x00000020L
		}
		*/


		public static bool CaptureWindow(System.Windows.Forms.Control control, 
								ref System.Drawing.Bitmap bitmap)
		{
			//This function captures the contents of a window or control

			Graphics g2 = Graphics.FromImage(bitmap);

			//PRF_CHILDREN // PRF_NONCLIENT
			int meint = (int)(PRF_CLIENT | PRF_ERASEBKGND); //| PRF_OWNED ); //  );
			System.IntPtr meptr = new System.IntPtr(meint);

			System.IntPtr hdc = g2.GetHdc();
			win32.SendMessage(control.Handle,win32.WM_PRINT,hdc,meptr);

			g2.ReleaseHdc(hdc);
			g2.Dispose();

			return true;
			
		}



	}
}





