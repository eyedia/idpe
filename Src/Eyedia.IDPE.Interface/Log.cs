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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Eyedia.Core;

namespace Eyedia.IDPE.Interface
{
    public partial class Log : SREBaseFormNew
    {
        public Log(string logContent, int lineNumber = 0)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            txtLog.Text = logContent;
            if (lineNumber != 0)
            {
                int charCount = SetCaretLine(lineNumber);                
                string currentlinetext = txtLog.Lines[lineNumber - 1];
                txtLog.Select(charCount, currentlinetext.Length);

            }
            else
            {
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.ScrollToCaret();
            }
            
        }

        public Log(string title, string fileName)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            this.Text = title;
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fs);
            LogReader = new StreamReader(sr.BaseStream);
            txtLog.LoadFile(LogReader.BaseStream, RichTextBoxStreamType.PlainText);
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();

            var monitor = new LogFileMonitor(fileName, "\n");

            monitor.OnLine += (s, e) =>
            {
                if (this.IsDisposed)
                    return;
                txtLog.AppendText(e.Line);
            };

            monitor.Start();
        }

        StreamReader LogReader { get; set; }
        

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                txtLog.LoadFile(LogReader.BaseStream, RichTextBoxStreamType.PlainText);
            }
            catch { }
        }

        private void contextMenuLog_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem.Enabled = !string.IsNullOrEmpty(txtLog.SelectedText);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtLog.Copy();
        }

        string findString;
        int findStringLastPosition;
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox ib = new InputBox();
            ib.Icon = this.Icon;
            if (ib.ShowDialog() == DialogResult.OK)
            {
                findString = ib.TheInput;
                findStringLastPosition = txtLog.Find(findString, findStringLastPosition, RichTextBoxFinds.None);
                findStringLastPosition += findString.Length;
            }
        }

        private void Log_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    if (!string.IsNullOrEmpty(findString))
                    {
                        findStringLastPosition = txtLog.Find(findString, findStringLastPosition, RichTextBoxFinds.None);
                        findStringLastPosition += findString.Length;
                    }
                    break;

                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void Log_Shown(object sender, EventArgs e)
        {
            txtLog.Focus();
        }

        int SetCaretLine(int linenr)
        {
            if (linenr > txtLog.Lines.Length)
            {
                MessageBox.Show("Line " + linenr + " is out of range.");
                return 0;
            }

            int row = 1;
            int charCount = 0;
            foreach (string line in txtLog.Lines)
            {
                charCount += line.Length + 1;
                row++;
                if (row == linenr)
                {
                    txtLog.SelectionStart = charCount;
                    break;
                }
            }

            return charCount;
        }

        private void Log_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LogReader != null)
                LogReader.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnChanged(sender, null);
        }
    }
}


