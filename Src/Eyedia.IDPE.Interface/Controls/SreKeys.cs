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
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Interface.Data;
using Eyedia.IDPE.Common;
using Eyedia.Core;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface.Controls
{
    public partial class SreKeys : UserControl
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
                Init(); 
            } 
        }

        public string DataSourceName { get; set; }
        public bool ShowCaption
        {
            get
            {
                return lblCaption.Visible;
            }
            set
            {
                lblCaption.Visible = value;
            }
        }

        public ToolStripStatusLabel ToolStripStatusLabel { get; set; }
        List<IdpeKey> _SystemKeys;
        List<IdpeKey> _Keys;

        public Button SaveButton { get; set; }

        private IdpeKey SelectedKey
        {
            get
            {
                return lstvwKeys.SelectedItems.Count > 0 ? (IdpeKey)lstvwKeys.SelectedItems[0].Tag : null;
            }
        }
        #endregion Properties

        public SreKeys()
        {
            InitializeComponent();
        }

       
        private void SreKeys_Load(object sender, EventArgs e)
        {          
            lstvwKeys.Columns.Add("Name", 300);                 
        }

        private void Init()
        {
            if ((this.DesignMode)
                || (DataSourceId == 0))
                return;

            Manager manager = new Manager();
            _Keys = manager.GetDataSourceKeys(DataSourceId, true);
            _SystemKeys = manager.GetDataSourceKeys(manager.GetDataSourceParentId(DataSourceId), true);
            if (string.IsNullOrEmpty(DataSourceName))
                DataSourceName = new Manager().GetApplicationName(DataSourceId);
            lblCaption.Text = (DataSourceId != 0) ? "Keys of " + DataSourceName : "All Keys";
            BindData();
        }
       
        public void Save()
        {
            IdpeKey key = new IdpeKey();
            key.Name = txtName.Text;
            key.Value = txtValue.Text;
            key.Type = (int)SreKeyTypes.Custom;
            try
            {
                Manager dm = new Manager();
                if (DataSourceId > 0)
                {
                    ValidateKeyValue();
                    dm.Save(key, DataSourceId);
                    _Keys = dm.GetDataSourceKeys(DataSourceId, true);
                }
                BindData();
                if (Form.ModifierKeys != System.Windows.Forms.Keys.Control)
                {
                    if(SaveButton != null)
                        SaveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
      
        void BindData(IdpeKey selectedKey = null)
        {
            lstvwKeys.Items.Clear();
            foreach (IdpeKey key in _SystemKeys)
            {
                ListViewItem item = new ListViewItem(key.Name);
                item.Tag = key;               
                item.ForeColor = Color.DarkRed;               
                lstvwKeys.Items.Add(item);
            }

            foreach (IdpeKey key in _Keys)
            {
                ListViewItem item = new ListViewItem(key.Name);
                item.Tag = key;   
                if(key.IsDeployable == true)
                    item.ForeColor = Color.DarkBlue;

                lstvwKeys.Items.Add(item);
            }
            if (lstvwKeys.Items.Count == 0)
            {
                txtName.Text = "";
                txtValue.Text = "";
            }

            if(selectedKey != null)
            {
                foreach(ListViewItem item in lstvwKeys.Items)
                {
                    if(((IdpeKey)item.Tag).Name == selectedKey.Name)
                    {
                        item.Selected = true;
                        lstvwKeys.Select();
                        break;
                    }
                }
            }

        }

        private void lstvwKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvwKeys.SelectedItems.Count > 0)
            {                
                txtName.Text = lstvwKeys.SelectedItems[0].SubItems[0].Text;
                txtValue.Text = ((IdpeKey)lstvwKeys.SelectedItems[0].Tag).Value;
                bool isSystemKey = lstvwKeys.SelectedItems[0].ForeColor != Color.DarkRed ? true : false;
                txtName.ReadOnly = !isSystemKey;
                txtValue.ReadOnly = !isSystemKey;                
            }
        }

        private void ValidateKeyValue()
        {
            if (txtName.Text.Equals("UniquenessCriteria", StringComparison.OrdinalIgnoreCase))
            {
                List<IdpeAttribute> attributes = new Manager().GetAttributes(DataSourceId);
                attributes = attributes.Where(a => a.IsAcceptable == true).ToList();

                string[] uniquenessAttributeNames = txtValue.Text.Split("+".ToCharArray());
                foreach (string column in uniquenessAttributeNames)
                {
                    bool found = false;
                    foreach (IdpeAttribute attribute in attributes)
                    {
                        if (attribute.Name.Equals(column, StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        string errorMessage = string.Format("The attribute '{0}' is not associated with datasource '{1}'! Uniqueness criteria should be defined as attribute1+attribute2+attributeN",
                            column, DataSourceName);
                        throw new BusinessException(errorMessage);
                    }

                }
            }
        }

        private void lstvwKeys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                deleteToolStripMenuItem_Click(sender, e);
            }
            else if ((e.KeyCode == Keys.A) && (e.Control))
            {
                foreach (ListViewItem item in lstvwKeys.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Init();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            saveFileDialog.FileName = DataSourceName;
            saveFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch();
                dsp.Name = DataSourceName;
                Manager mgr = new Manager();
                List<IdpeKey> keys = mgr.GetApplicationKeys(mgr.GetApplicationId(dsp.Name), false);
                foreach (IdpeKey key in keys)
                {
                    dsp.Keys.Add(key);
                }
                dsp.Export(saveFileDialog.FileName);
            }

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = DataSourceName;
            openFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch(openFileDialog.FileName);
                if (dsp.Name != DataSourceName)
                {
                    if (MessageBox.Show(string.Format("The keys are from different data source {0}. Do you want to import those into {1}?", dsp.Name, DataSourceName),
                        "Import Keys", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dsp.Name = DataSourceName;
                        dsp.Import();
                        Init();
                    }

                }
                else
                {
                    dsp.Import();
                    Init();
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (DataSourceId == 0)
            {
                exportToolStripMenuItem.Visible = false;
                importToolStripMenuItem.Visible = false;
            }
            else
            {
                exportToolStripMenuItem.Visible = true;
                importToolStripMenuItem.Visible = true;
            }

            deleteToolStripMenuItem.Enabled = ((lstvwKeys.SelectedItems.Count > 0)
                && (lstvwKeys.SelectedItems[0].ForeColor == Color.DarkRed)) ? false : true;

            if ((SelectedKey != null) && (SelectedKey.IsDeployable == true))
                deployableToolStripMenuItem.Text = "Make it not Deployable";
            else
                deployableToolStripMenuItem.Text = "Make it Deployable";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manager dm = new Manager();
            if ((lstvwKeys.SelectedItems.Count == 1)
                && (lstvwKeys.SelectedItems[0].ForeColor != Color.DarkRed))
            {
                if (MessageBox.Show(string.Format("Are you sure you want to delete '{0}'? The system will try to disassociate the key from '{1}' and then try to delete the key permanently!", lstvwKeys.SelectedItems[0].Text, DataSourceName),
                    "Delete Key", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {                    
                    IdpeKey key = dm.GetKey(DataSourceId, lstvwKeys.SelectedItems[0].Text);
                    dm.DeleteKeyFromApplication(DataSourceId, lstvwKeys.SelectedItems[0].Text, false);
                    try
                    {
                        dm.DeleteKey(key.KeyId);
                    }
                    catch
                    {
                        MessageBox.Show(string.Format("The key has been disassociated from '{0}', but master entry could not be deleted as the same key is associated with other data sources!", DataSourceName),
                            "Partially Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }                    
                }
            }
            else if ((lstvwKeys.SelectedItems.Count > 1)
                && (lstvwKeys.SelectedItems[0].ForeColor != Color.DarkRed))
            {
                foreach (ListViewItem item in lstvwKeys.SelectedItems)
                {
                    IdpeKey key = dm.GetKey(DataSourceId, item.Text);
                    dm.DeleteKeyFromApplication(DataSourceId, item.Text, false);
                    try
                    {
                        dm.DeleteKey(key.KeyId);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(string.Format("Error deleting key '{0}'! {1}{2}", item.Text,Environment.NewLine, ex.Message),
                            "Error Deleting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            _Keys = dm.GetDataSourceKeys(DataSourceId, true);
            BindData();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSources ds = new DataSources();
            Manager manager = new Manager();
            if (ds.ShowDialog() == DialogResult.OK)
            {
                int counter = 0;
                foreach (ListViewItem item in lstvwKeys.SelectedItems)
                {
                    IdpeKey key = item.Tag as IdpeKey;
                    manager.Save(key, ds.SelectedDataSource.Id);
                    counter++;
                }
                if(counter > 0)
                    MessageBox.Show(string.Format("{0} keys copied to {1}", counter, ds.SelectedDataSource.Name), "Copy Keys", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SreKeys_Resize(object sender, EventArgs e)
        {
            if (lstvwKeys.Columns.Count > 0)
                lstvwKeys.Columns[0].Width = lstvwKeys.Width - 5;
        }

        private void txtValue_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Control) && (e.KeyCode == Keys.A))
                txtValue.SelectAll();
        }

        private void deployableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<IdpeKey> keys = new List<IdpeKey>();
            SelectedKey.IsDeployable = deployableToolStripMenuItem.Text.Contains("not") ? false : true;
            keys.Add(SelectedKey);
            new Manager().SetIsDeployable(DataSourceId, keys);
            BindData(SelectedKey);
        }
    }
}


