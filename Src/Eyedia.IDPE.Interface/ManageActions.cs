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
using System.Windows.Forms;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface
{
    public partial class ManageActions : Form
    {
        public ManageActions(Icon icon)
        {
            InitializeComponent();
            if (icon != null)
                this.Icon = icon;
            Bind();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }

        private void Save()
        {
            string strActions = string.Empty;

            if (chkAction1Enabled.Checked)
                strActions += txtAction1.Text + "|";
            if (chkAction2Enabled.Checked)
                strActions += txtAction2.Text + "|";
            if (chkAction3Enabled.Checked)
                strActions += txtAction3.Text + "|";
            if (chkAction4Enabled.Checked)
                strActions += txtAction4.Text + "|";
            if (chkAction5Enabled.Checked)
                strActions += txtAction5.Text + "|";

            if (strActions.Length > 1)
                strActions = strActions.Substring(0, strActions.Length - 1);

            IdpeKey key = new IdpeKey();
            key.Name = SreKeyTypes.CustomActions.ToString();
            key.Type = (int)SreKeyTypes.CustomActions;
            key.Value = strActions;
            new Manager().Save(key);
            
        }
        private void Bind()
        {
            IdpeKey key = new Manager().GetKey(SreKeyTypes.CustomActions);
            if (key != null)
            {
                if (!key.Value.Contains("|"))
                {
                    chkAction1Enabled.Checked = true;
                    txtAction1.Text = key.Value;
                }
                else
                {
                    string[] actions = key.Value.Split("|".ToCharArray());
                    int counter = 1;
                    foreach (string action in actions)
                    {
                        if (counter == 1)
                        {
                            chkAction1Enabled.Checked = true;
                            txtAction1.Text = action;
                        }
                        else if (counter == 2)
                        {
                            chkAction2Enabled.Checked = true;
                            txtAction2.Text = action;
                        }
                        else if (counter == 3)
                        {
                            chkAction3Enabled.Checked = true;
                            txtAction3.Text = action;
                        }
                        else if (counter == 4)
                        {
                            chkAction4Enabled.Checked = true;
                            txtAction4.Text = action;
                        }
                        else if (counter == 5)
                        {
                            chkAction5Enabled.Checked = true;
                            txtAction5.Text = action;
                        }
                        counter++;

                    }

                }
            }
        }

        private void RunNow(string actions)
        {
            this.Cursor = Cursors.WaitCursor;
            string output = DosCommands.Execute(actions);
            MessageBox.Show(output, "Custom Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Cursor = Cursors.Default;
        }

        #region Events
        private void chkAction1Enabled_CheckedChanged(object sender, EventArgs e)
        {
            txtAction1.Enabled = chkAction1Enabled.Checked;
            btnAction1RunNow.Enabled = chkAction1Enabled.Checked;
            chkAction2Enabled.Enabled = chkAction1Enabled.Checked;           
        }

        private void chkAction2Enabled_CheckedChanged(object sender, EventArgs e)
        {
            txtAction2.Enabled = chkAction2Enabled.Checked;
            btnAction2RunNow.Enabled = chkAction2Enabled.Checked;
            chkAction3Enabled.Enabled = chkAction2Enabled.Checked;
        }

        private void chkAction3Enabled_CheckedChanged(object sender, EventArgs e)
        {
            txtAction3.Enabled = chkAction3Enabled.Checked;
            btnAction3RunNow.Enabled = chkAction3Enabled.Checked;
            chkAction4Enabled.Enabled = chkAction3Enabled.Checked;
        }

        private void chkAction4Enabled_CheckedChanged(object sender, EventArgs e)
        {
            txtAction4.Enabled = chkAction4Enabled.Checked;
            btnAction4RunNow.Enabled = chkAction4Enabled.Checked;
            chkAction5Enabled.Enabled = chkAction4Enabled.Checked;
        }

        private void chkAction5Enabled_CheckedChanged(object sender, EventArgs e)
        {
            txtAction5.Enabled = chkAction5Enabled.Checked;
            btnAction5RunNow.Enabled = chkAction5Enabled.Checked;
        }      

        private void OnTextChanged(object sender, EventArgs e)
        {
            TextBox tBox = sender as TextBox;
            if (tBox.Text.Contains("|"))
                tBox.Text = tBox.Text.Replace("|", "");
        }
       
        private void btnAction1RunNow_Click(object sender, EventArgs e)
        {
            RunNow(txtAction1.Text);
        }

       
        private void btnAction2RunNow_Click(object sender, EventArgs e)
        {
            RunNow(txtAction2.Text);
        }

        private void btnAction3RunNow_Click(object sender, EventArgs e)
        {
            RunNow(txtAction3.Text);
        }

        private void btnAction4RunNow_Click(object sender, EventArgs e)
        {
            RunNow(txtAction4.Text);
        }

        private void btnAction5RunNow_Click(object sender, EventArgs e)
        {
            RunNow(txtAction5.Text);
        }       

        private void ManageActions_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        #endregion Events

    }
}


