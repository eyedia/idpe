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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Eyedia.Core.Data;
using WeifenLuo.WinFormsUI.Docking;

namespace Eyedia.IDPE.Interface
{
    public class SREBaseFormNew : DockContent
    {
        protected StatusStrip statusStrip1 { get { return MainWindow.statusStrip1; } }
        protected ToolStripStatusLabel toolStripStatusLabel1 { get { return MainWindow == null ? new ToolStripStatusLabel() : MainWindow.toolStripStatusLabel1; } }
        protected ToolStripStatusLabel toolStripStatusLabel2 { get { return MainWindow == null ? new ToolStripStatusLabel() : MainWindow.toolStripStatusLabel2; } }
        protected ToolStripStatusLabel toolStripStatusLabel3 { get { return MainWindow == null ? new ToolStripStatusLabel() : MainWindow.toolStripStatusLabel3; } }
        protected ToolStripStatusLabel toolStripStatusLabel4 { get { return MainWindow == null ? new ToolStripStatusLabel() : MainWindow.toolStripStatusLabel4; } }
        protected ToolStripProgressBar toolStripProgressBar1 { get { return MainWindow == null ? new ToolStripProgressBar() : MainWindow.toolStripProgressBar1; } }        

        protected MainWindow MainWindow
        {
            get
            {
                return this.ParentForm as MainWindow;
            }
        }

        protected bool CancelPressed
        {
            get { return MainWindow == null ? false : MainWindow.CancelPressed; }
            set
            {
                if (MainWindow != null)
                    MainWindow.CancelPressed = value;
            }
        }

        protected void SetStatusText(string text,bool redFont = false, bool clear = false)
        {
            if (MainWindow != null)
                MainWindow.SetToolStripStatusLabel(text,redFont, clear);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();     
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "SREBaseFormNew";            
            this.ResumeLayout(false);

        }

       
    }
}





