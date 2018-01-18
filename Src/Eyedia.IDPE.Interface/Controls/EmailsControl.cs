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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Text.RegularExpressions;
using System.IO;

namespace Eyedia.IDPE.Interface.Controls
{
    public partial class EmailsControl : UserControl
    {
        #region Properties

        int _dataSourceId;
        public int DataSourceId
        {
            get
            {
                return _dataSourceId;
            }
            set
            {
                _dataSourceId = value;
                BindData();
            }
        }

        public ToolStripStatusLabel toolStripStatusLabel1;
        public Button SaveButton { get; set; }

        #endregion Properties

        public EmailsControl()
        {
            InitializeComponent();
        }
      
            
        //private void btnSave_Click(object sender, EventArgs e)
        public void Save()
        {
            if (DataSourceId == 0)
                return;

            try
            {
                if (!string.IsNullOrEmpty(txtCc.Text))
                {
                    IdpeKey keyCc = new IdpeKey();
                    keyCc.Name = IdpeKeyTypes.EmailCc.ToString();
                    keyCc.Type = (int)IdpeKeyTypes.EmailCc;
                    keyCc.Value = txtCc.Text;
                    new Manager().Save(keyCc, DataSourceId);
                }
                else
                {
                    new Manager().DeleteKeyFromApplication(DataSourceId, IdpeKeyTypes.EmailCc.ToString(), true);
                }

                if (!string.IsNullOrEmpty(txtBcc.Text))
                {
                    IdpeKey keyBcc = new IdpeKey();
                    keyBcc.Name = IdpeKeyTypes.EmailBcc.ToString();
                    keyBcc.Type = (int)IdpeKeyTypes.EmailBcc;
                    keyBcc.Value = txtBcc.Text;
                    new Manager().Save(keyBcc, DataSourceId);
                }
                else
                {
                    new Manager().DeleteKeyFromApplication(DataSourceId, IdpeKeyTypes.EmailBcc.ToString(), true);
                }

                if (chkSendEmailAfterFileProcessed.Checked)
                {
                    IdpeKey key = new IdpeKey();
                    key.Name = IdpeKeyTypes.EmailAfterFileProcessed.ToString();
                    key.Type = (int)IdpeKeyTypes.EmailAfterFileProcessed;
                    key.Value = "1";
                    new Manager().Save(key, DataSourceId);
                }
                else
                {
                    new Manager().DeleteKeyFromApplication(DataSourceId, IdpeKeyTypes.EmailAfterFileProcessed.ToString(), true);
                }

                if (chkIncludeAttachmentInput.Checked)
                {
                    IdpeKey key = new IdpeKey();
                    key.Name = IdpeKeyTypes.EmailAfterFileProcessedAttachInputFile.ToString();
                    key.Type = (int)IdpeKeyTypes.EmailAfterFileProcessedAttachInputFile;
                    key.Value = "1";
                    new Manager().Save(key, DataSourceId);
                }
                else
                {
                    new Manager().DeleteKeyFromApplication(DataSourceId, IdpeKeyTypes.EmailAfterFileProcessedAttachInputFile.ToString(), true);
                }

                if (chkIncludeAttachmentOutput.Checked)
                {
                    IdpeKey key = new IdpeKey();
                    key.Name = IdpeKeyTypes.EmailAfterFileProcessedAttachOutputFile.ToString();
                    key.Type = (int)IdpeKeyTypes.EmailAfterFileProcessedAttachOutputFile;
                    key.Value = "1";
                    new Manager().Save(key, DataSourceId);
                }
                else
                {
                    new Manager().DeleteKeyFromApplication(DataSourceId, IdpeKeyTypes.EmailAfterFileProcessedAttachOutputFile.ToString(), true);
                }

                BindData();
            }
            catch (Exception ex)
            {
                if (toolStripStatusLabel1 != null)
                    toolStripStatusLabel1.Text = ex.Message;
                else
                    MessageBox.Show(ex.ToString());
            }
        }

        #region Helper Methods
        
        private void IsValidEmailAddress(object sender, EventArgs e)
        {
            if (SaveButton == null)
                return;

            if (sender is TextBox)
            {
                TextBox tBox = sender as TextBox;
                if (string.IsNullOrEmpty(tBox.Text))
                {
                    SaveButton.Enabled = true;
                    return;
                }

                if (tBox.Text.Contains(";"))
                    SaveButton.Enabled = IsValidEmailIds(tBox.Text, ";");
                else if (tBox.Text.Contains(","))
                    SaveButton.Enabled = IsValidEmailIds(tBox.Text, ",");
                else
                    SaveButton.Enabled = IsValidEmailId(tBox.Text);
            }
        }

        private bool IsValidEmailIds(string emailIds, string delimiter)
        {
            string[] emails = emailIds.Split(delimiter.ToCharArray());
            foreach (string email in emails)
            {
                if (!IsValidEmailId(email))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidEmailId(string emailId)
        {
            Regex reg = new Regex(@"^([a-zA-Z0-9_\-])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$");
            return reg.IsMatch(emailId);
        }

        private void BindData()
        {
            if (DesignMode)
                return;

            BindToEmails();
            BindOtherEmails();
            BindOthers();

        }
        private void BindToEmails()
        {
            string exeName = "idpe.exe";
            if (ConfigurationManager.AppSettings["exeName"] != null)
                exeName = ConfigurationManager.AppSettings["exeName"];

            try
            {
                string exeCompleteFileName = AppDomain.CurrentDomain.BaseDirectory + exeName;
                if (!File.Exists(exeCompleteFileName))
                    return;

                Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeCompleteFileName);
                EyediaCoreConfigurationSection sreSection = (EyediaCoreConfigurationSection)configuration.GetSection("eyediaCoreConfigurationSection");
                txtTo.Text = sreSection.Email.ToEmails;
            }
            catch
            {
                txtTo.Text = "<Could not retrieve! '" + exeName + ".config' not found>";
            }
        }

        private void BindOtherEmails()
        {

            Manager manager = new Manager();
            IdpeKey keyCc = manager.GetKey(DataSourceId, IdpeKeyTypes.EmailCc.ToString());
            if (keyCc != null)
                txtCc.Text = keyCc.Value;
            else
                txtCc.Text = string.Empty;

            IdpeKey keyBcc = manager.GetKey(DataSourceId, IdpeKeyTypes.EmailBcc.ToString());
            if (keyBcc != null)
                txtBcc.Text = keyBcc.Value;
            else
                txtBcc.Text = string.Empty;
        }

        private void BindOthers()
        {

            Manager manager = new Manager();
            IdpeKey key = manager.GetKey(DataSourceId, IdpeKeyTypes.EmailAfterFileProcessed.ToString());
            if (key != null)
                chkSendEmailAfterFileProcessed.Checked = key.Value.ParseBool();
            else
                chkSendEmailAfterFileProcessed.Checked = false;

            key = manager.GetKey(DataSourceId, IdpeKeyTypes.EmailAfterFileProcessedAttachInputFile.ToString());
            if (key != null)
                chkIncludeAttachmentInput.Checked = key.Value.ParseBool();
            else
                chkIncludeAttachmentInput.Checked = false;

            key = manager.GetKey(DataSourceId, IdpeKeyTypes.EmailAfterFileProcessedAttachOutputFile.ToString());
            if (key != null)
                chkIncludeAttachmentOutput.Checked = key.Value.ParseBool();
            else
                chkIncludeAttachmentOutput.Checked = false;

        }

        #endregion Helper Methods

        private void SreEmails_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                BindData();
        }

        private void chkSendEmailAfterFileProcessed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSendEmailAfterFileProcessed.Checked)
            {
                chkIncludeAttachmentInput.Enabled = true;
                chkIncludeAttachmentOutput.Enabled = true;
                chkIncludeAttachmentOthers.Enabled = true;
            }
            else
            {
                chkIncludeAttachmentInput.Checked = false;
                chkIncludeAttachmentInput.Enabled = false;

                chkIncludeAttachmentOutput.Checked = false;
                chkIncludeAttachmentOutput.Enabled = false;

                chkIncludeAttachmentOthers.Checked = false;
                chkIncludeAttachmentOthers.Enabled = false;
            }
            //SaveButton.Enabled = true;
        }

        private void chkIncludeAttachment_CheckedChanged(object sender, EventArgs e)
        {
            //SaveButton.Enabled = true;
        }      

     
    }
}


