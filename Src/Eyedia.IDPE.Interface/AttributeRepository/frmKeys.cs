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
using Symplus.RuleEngine.Utilities.Data;
using Symplus.RuleEngine.Common;
using Symplus.RuleEngine.DataManager;

namespace Symplus.RuleEngine.Utilities
{
    public partial class frmKeys : Form
    {
        int _ApplicationId;
        string _ApplicationName;
        List<SreKey> _Keys;

        public frmKeys()
        {
            InitData();
            _Keys = new Manager().GetKeys();
            lstvwKeys.CheckBoxes = false;
            BindData();
        }

        public frmKeys(int applicationId, string applicationName)
        {
            InitData();
            _ApplicationId = applicationId;
            _ApplicationName = applicationName;
            _Keys = new Manager().GetApplicationKeys(_ApplicationId, false);
            lstvwKeys.CheckBoxes = true;
            BindData();
        }

        void InitData()
        {
            InitializeComponent();
            lstvwKeys.Columns.Add("Name", 100);
            lstvwKeys.Columns.Add("Value", 450);
            lstvwKeys.Columns.Add("Type", 200);
            cmbType.DataSource = Enum.GetValues(typeof(SreKeyTypes));
        }



        private void btnOK_Click(object sender, EventArgs e)
        {
           
            SreKey key = new SreKey();
            key.Name = txtName.Text;
            key.Value = txtValue.Text;
            key.Type = cmbType.SelectedIndex;
            try
            {
                Manager dm = new Manager();
                if (_ApplicationId > 0)
                {
                    dm.Save(_ApplicationId, key);
                    _Keys = dm.GetApplicationKeys(_ApplicationId, false);
                }
                else
                {
                    dm.Save(key);
                    _Keys = dm.GetKeys();
                }
                BindData();
                if (Form.ModifierKeys != Keys.Control)
                {
                    btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }

        }
        
        void BindData()
        {
            lstvwKeys.Items.Clear();
            foreach (SreKey key in _Keys)
            {
                ListViewItem item = new ListViewItem(key.Name);
                item.SubItems.Add(key.Value);
                SreKeyTypes keytype = (SreKeyTypes) key.Type;
                item.SubItems.Add(keytype.ToString());
                item.SubItems.Add(key.Type.ToString());
                item.Checked = key.IsDefault == true?true:false;                
                lstvwKeys.Items.Add(item);
            }           
            
        }

        private void lstvwKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvwKeys.SelectedItems.Count > 0)
            {
                txtName.Text = lstvwKeys.SelectedItems[0].SubItems[0].Text;
                txtValue.Text = lstvwKeys.SelectedItems[0].SubItems[1].Text;
                cmbType.SelectedIndex = int.Parse(lstvwKeys.SelectedItems[0].SubItems[3].Text);
            }
        }

        private void lstvwKeys_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!e.Item.Checked) return; 
            foreach (ListViewItem item in lstvwKeys.Items) 
            { 
                if (item == e.Item)                    
                    continue; 
                item.Checked = false; 
            }

            if (_FormIntialized) UpdateDefaultKey();
        }

       
        private void btnNew_Click(object sender, EventArgs e)
        {
            cmbType.SelectedIndex = 0;
            txtName.Text = "";
            txtValue.Text = "";            
        }

        bool _FormIntialized;
        private void frmKeys_Activated(object sender, EventArgs e)
        {
            _FormIntialized = true;
        }

        private void UpdateDefaultKey()
        {
            int idx = 0;
            foreach (ListViewItem item in lstvwKeys.Items)
            {
                _Keys[idx].IsDefault = item.Checked;
                idx++;
            }
            new Manager().UpdateDefaultKey(_ApplicationId, _Keys);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = CanSave();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = CanSave();
        }
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = CanSave();
        }

        bool CanSave()
        {

            if ((cmbType.SelectedIndex != 0)
                && (txtName.Text.Length > 0)
                    && (txtValue.Text.Length > 0))
                return true;
            return false;
        }

        private void lstvwKeys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lstvwKeys.SelectedItems.Count > 0)
                {
                    if (_ApplicationId > 0)
                    {
                        if (MessageBox.Show(string.Format("Are you sure you want to disassociate '{0}' from '{1}'? To delete key permanently goto [Main Window]->[Repositories]->[Key Editor]", lstvwKeys.SelectedItems[0].Text, _ApplicationName),
                            "Disassociation of Key", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Manager dm = new Manager();

                            dm.DeleteKeyFromApplication(_ApplicationId, lstvwKeys.SelectedItems[0].Text, false);
                            _Keys = dm.GetApplicationKeys(_ApplicationId, false);
                            BindData();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show(string.Format("Are you sure you want to delete key '{0}'? Note that it can only be deleted if it is not in use.", lstvwKeys.SelectedItems[0].Text),
                            "Delete Key", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Manager dm = new Manager();

                            dm.DeleteKey(lstvwKeys.SelectedItems[0].Text);
                            _Keys = dm.GetKeys();
                            BindData();
                        }
                    }
                }
            }
        }

        

        
    }
}





