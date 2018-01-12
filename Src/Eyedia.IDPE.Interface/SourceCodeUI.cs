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
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface
{
    public partial class SourceCodeUI : Form
    {             
       int CloseCounter;
       const int CloseCounterLimit = 3;
        public SourceCodeUI()
        {            
            InitializeComponent();
            ;
            Init();

        }

        void Init()
        {
            CloseCounter = CloseCounterLimit;
            lvwFolders.Columns.Clear();
            lvwFolders.Columns.Add("Folders");
            lvwFolders.Columns[0].Width = lvwFolders.Width - 25;

            string[] dirs = Directory.GetDirectories(txtSourceCodeLocation.Text);
            foreach (string dir in dirs)
            {
                ListViewItem item = new ListViewItem(dir);
                item.Checked = true;
                lvwFolders.Items.Add(item);
            }
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {

                if (!Directory.Exists(txtSourceCodeLocation.Text))
                {
                    MessageBox.Show("Directory does not exist", "Directory Does Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;                    
                }
                SourceCodeManager sourceCodeManager = new SourceCodeManager(txtDestinationFolder.Text);
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Increment(10);
                toolStripStatusLabel1.Text = "Initializing...";
                Application.DoEvents();

                foreach (ListViewItem item in lvwFolders.Items)
                {
                    if (item.Checked)
                    {
                        sourceCodeManager.BundleCode(item.Text);
                        toolStripProgressBar1.Increment(10);
                        toolStripStatusLabel1.Text = "Bundling..." + item.Text;
                        Application.DoEvents();
                    }

                    
                }                
                toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
                toolStripStatusLabel1.Text = "Done";
                
                toolStripProgressBar1.Visible = false;
                pnlTop.Enabled = false;
                btnOK.Text = string.Format("Close {0}", CloseCounter);
                Application.DoEvents();
                timerClose.Enabled = true;
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
                toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
                toolStripProgressBar1.Visible = false;             
            }
            
        }       

        private void SourceCodeUI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)                
                this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtSourceCodeLocation.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                txtSourceCodeLocation.Text = saveFileDialog1.FileName;
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            if (CloseCounter <= 0)
            {
                timerClose.Enabled = false;
                this.Close();
            }
            CloseCounter--;
            btnOK.Text = string.Format("Close {0}", CloseCounter);
            Application.DoEvents();

        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvwFolders.Items)
            {
                item.Checked = chkSelectAll.Checked;
            }
        }

      
    }
   
}





