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
using Eyedia.Core.Data;

namespace Eyedia.Core.Windows.Forms
{
    public partial class UserManager : Form
    {
        public UserManager()
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            Init();
        }
        
        void Init()
        {            
            lvwUsers.Columns.Add("Name");
            lvwUsers.Columns.Add("User Name");
            lvwUsers.Columns.Add("Email Id");
            lvwUsers.Columns[0].Width = 150;
            lvwUsers.Columns[1].Width = 100;
            lvwUsers.Columns[2].Width = 150;
            cbGroups.DataSource = CoreDatabaseObjects.Instance.Groups;
            cbGroups.DisplayMember = "Name";
            RefreshUserList();
        }

        void RefreshUserList()
        {
            lvwUsers.Items.Clear();
            foreach (User user in CoreDatabaseObjects.Instance.Users)
            {
                ListViewItem item = new ListViewItem(user.FullName);
                item.SubItems.Add(user.UserName);
                item.SubItems.Add(user.EmailId);
                if ((user.IsDebugUser == true)
                    || (user.IsSystemUser == true))
                    item.ForeColor = Color.DarkGray;

                item.Tag = user;
                lvwUsers.Items.Add(item);
            }
            if (lvwUsers.Items.Count > 0)
            {
                SelectedUser = (User)lvwUsers.Items[0].Tag;
                BindData();
            }
        }

        void BindData()
        {
            if (SelectedUser != null)
            {
                if ((SelectedUser.IsDebugUser == true)
                    || (SelectedUser.IsSystemUser == true))
                {
                    EnableControls(false);
                }
                else
                {
                   EnableControls(true);
                    txtFullName.Text = SelectedUser.FullName;
                    txtUserName.Text = SelectedUser.UserName;
                    lblPassword.Text = SelectedUser.Password;
                    txtEmailId.Text = SelectedUser.EmailId;
                    cbGroups.SelectedItem = CoreDatabaseObjects.Instance.Groups.Where(g => g.Id == SelectedUser.GroupId).SingleOrDefault();
                }
            }
            else
            {
               EnableControls(false);
                txtFullName.Text = string.Empty;
                txtUserName.Text = string.Empty;
                lblPassword.Text = string.Empty;
                txtEmailId.Text = string.Empty;
                if(cbGroups.Items.Count > 0) cbGroups.SelectedIndex = 0;
            }
        }

        User SelectedUser { get; set; }

        private void lvwUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwUsers.SelectedItems.Count > 0)
                SelectedUser = (User)lvwUsers.SelectedItems[0].Tag;
            else
                SelectedUser = null;

            BindData();
        }

        private void lvwUsers_Resize(object sender, EventArgs e)
        {
            if (lvwUsers.Columns.Count >= 3)
            {
                lvwUsers.Columns[0].Width = lvwUsers.Width / 4;
                lvwUsers.Columns[1].Width = lvwUsers.Width / 4;
                lvwUsers.Columns[2].Width = lvwUsers.Width / 4;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            User newOrOldUser = new User();

            newOrOldUser.FullName = txtFullName.Text;
            newOrOldUser.UserName = txtUserName.Text;
            newOrOldUser.EmailId = txtEmailId.Text;
            newOrOldUser.GroupId = ((Group)cbGroups.SelectedItem).Id;

            try
            {
                if (CoreDatabaseObjects.Instance.GetUser(newOrOldUser.UserName) == null)
                {
                    ChangePassword setPassword = new ChangePassword(SelectedUser, false, 4, "Set Password");
                    if (setPassword.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        newOrOldUser.Password = setPassword.Password;                    
                }
                else
                {
                    newOrOldUser.Password = lblPassword.Text;
                }
                CoreDatabaseObjects.Instance.Save(newOrOldUser);
                CoreDatabaseObjects.Instance.Refresh();
                RefreshUserList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedUser != null)
                contextMenuStrip1.Enabled = SelectedUser.IsDebugUser != true && SelectedUser.IsSystemUser != true;
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword(SelectedUser);
            changePassword.ShowDialog();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableControls(true);
            txtFullName.Text = "";
            txtUserName.Text = "";
            txtEmailId.Text = "";
            lblPassword.Text = "";
            cbGroups.SelectedItem = CoreDatabaseObjects.Instance.Groups.Where(g => g.Name.ToLower() == "users").SingleOrDefault();
            txtFullName.Focus();
        }

        private void EnableControls(bool enable)
        {
            txtFullName.Enabled = enable;
            txtUserName.Enabled = enable;
            txtEmailId.Enabled = enable;
            cbGroups.Enabled = enable;
            btnSave.Enabled = enable;
        }

       
    }
}





