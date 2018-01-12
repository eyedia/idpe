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
using Eyedia.Core.Data;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using System.IO;
using System.Reflection;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface.Controls
{
    public partial class SreDatabases : UserControl
    {
        public SreDatabases()
        {
            InitializeComponent();
            cbDatabaseTypes.Items.AddRange(new string[] { "Microsoft SQL Server", "Oracle", "DB2", "Microsoft SQL Compact Edition" });
            if (DesignMode)
            {
                Manager manager = new Manager();
                Keys = manager.GetDataSourceKeysConnectionStrings(DataSourceId, false);
                SystemKeys = manager.GetDataSourceKeysConnectionStrings(manager.GetDataSourceParentId(DataSourceId), true);                
                BindData();
            }
        }

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

        public bool EnableGlobalKeys { get; set; }

        [Browsable(false)]
        public List<IdpeKey> Keys { get; private set; }

        [Browsable(false)]
        public List<IdpeKey> SystemKeys { get; private set; }

        IdpeKey SelectedKey { get; set; }

        [Browsable(false)]
        public Button SaveButton { get; set; }

        public bool ShowSaveButton { get { return btnSave.Visible; } set { btnSave.Visible = value; } }

        private void Init()
        {
            if ((this.DesignMode)
                || (DataSourceId == 0))
                return;

            Manager manager = new Manager();
            Keys = manager.GetDataSourceKeysConnectionStrings(DataSourceId, false);
            if (DataSourceId != -99)
                SystemKeys = manager.GetDataSourceKeysConnectionStrings(manager.GetDataSourceParentId(DataSourceId), true);
            else
                SystemKeys = new List<IdpeKey>();

            BindData();
        }

        public void BindData()
        {
            lvwKeys.Items.Clear();

            //system keys are deprecated
            foreach (IdpeKey key in SystemKeys)
            {
                ListViewItem item = new ListViewItem(key.Name);
                item.Tag = key;
                item.ForeColor = Color.DarkRed;
                lvwKeys.Items.Add(item);
            }

            foreach (IdpeKey key in Keys)
            {
                ListViewItem item = new ListViewItem(key.Name);
                if (key.IdpeKeyDataSources.Count > 0)
                    item.ForeColor = Color.DarkRed;
                item.Tag = key;                
                lvwKeys.Items.Add(item);
            }
          
        }

        private SreKeyTypes GetSreKeyType()
        {
            switch (cbDatabaseTypes.Text)
            {
                case "Microsoft SQL Server":
                    return SreKeyTypes.ConnectionStringSqlServer;
                case "Oracle":
                    return SreKeyTypes.ConnectionStringOracle;
                case "DB2":
                    return SreKeyTypes.ConnectionStringDB2iSeries;
                case "Microsoft SQL Compact Edition":
                    return SreKeyTypes.ConnectionStringSqlCe;

                default:
                    return SreKeyTypes.Unknown;
            }
        }

        //private void btnSave_Click(object sender, EventArgs e)
        public void Save()
        {
            IdpeKey key = new IdpeKey();
            SreKeyTypes selectedType = GetSreKeyType();
            key.Name = txtConnectionStringName.Text;
            key.Value = txtConnectionString.Text;
            key.Type = (int)selectedType;
            try
            {
                Manager dm = new Manager();
                dm.Save(key, DataSourceId);
                refreshToolStripMenuItem_Click(null, null);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            IDal dal = null;
            IDbConnection connection = null;

            switch (cbDatabaseTypes.Text)
            {
                case "Microsoft SQL Server":
                    dal = new DataAccessLayer(DatabaseTypes.SqlServer).Instance;
                    break;

                case "Oracle":
                    dal = new DataAccessLayer(DatabaseTypes.Oracle).Instance;
                    break;

                case "DB2":
                    dal = new DataAccessLayer(DatabaseTypes.DB2iSeries).Instance;
                    break;

                case "Microsoft SQL Compact Edition":
                    dal = new DataAccessLayer(DatabaseTypes.SqlCe).Instance;
                    break;

                default:
                    return;
            }

            try
            {
                connection = dal.CreateConnection(txtConnectionString.Text);
                connection.Open();
                MessageBox.Show("Successful!",
                    "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed!" + Environment.NewLine + ex.Message,
                    "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                    connection.Close();
                }
            }
        }

        private void lvwKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwKeys.SelectedItems.Count > 0)
            {
                SelectedKey = lvwKeys.SelectedItems[0].Tag as IdpeKey;
                switch ((SreKeyTypes)SelectedKey.Type)
                {
                    case SreKeyTypes.ConnectionStringSqlServer:
                        cbDatabaseTypes.Text = "Microsoft SQL Server";
                        break;

                    case SreKeyTypes.ConnectionStringOracle:
                        cbDatabaseTypes.Text = "Oracle";
                        break;

                    case SreKeyTypes.ConnectionStringDB2iSeries:
                        cbDatabaseTypes.Text = "DB2";
                        break;

                    case SreKeyTypes.ConnectionStringSqlCe:
                        cbDatabaseTypes.Text = "Microsoft SQL Compact Edition";
                        break;

                    default:
                        cbDatabaseTypes.Text = "";
                        break;
                }

                txtConnectionStringName.Text = SelectedKey.Name;
                txtConnectionString.Text = SelectedKey.Value;


                bool isSystemKey = lvwKeys.SelectedItems[0].ForeColor != Color.DarkRed ? true : false;

                if (EnableGlobalKeys == false)
                {
                    cbDatabaseTypes.Enabled = isSystemKey;
                    txtConnectionStringName.ReadOnly = !isSystemKey;
                    txtConnectionString.ReadOnly = !isSystemKey;
                }             
            }
            else
            {
                txtConnectionStringName.Text = string.Empty;
                txtConnectionString.Text = string.Empty;
                cbDatabaseTypes.SelectedIndex = -1;
            }
        }

        private void lvwKeys_Resize(object sender, EventArgs e)
        {
            lvwKeys.Columns[0].Width = lvwKeys.Width - 1;            
        }

       

        public void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Keys = new Manager().GetDataSourceKeysConnectionStrings(DataSourceId, false);
            BindData();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dataSourceName = new Manager().GetApplicationName(DataSourceId);
            saveFileDialog.FileName = dataSourceName;
            saveFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch();
                dsp.Name = dataSourceName;
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
            string dataSourceName = new Manager().GetApplicationName(DataSourceId);
            openFileDialog.FileName = dataSourceName;
            openFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch(openFileDialog.FileName);
                if (dsp.Name != dataSourceName)
                {
                    if (MessageBox.Show(string.Format("The connection strings are from different data source {0}. Do you want to import those into {1}?", dsp.Name, dataSourceName),
                        "Import Keys", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dsp.Name = dataSourceName;
                        dsp.Import();
                        refreshToolStripMenuItem_Click(sender, e);
                    }

                }
                else
                {
                    dsp.Import();
                    refreshToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void lvwKeys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                deleteToolStripMenuItem_Click(sender, e);
            }
            else if ((e.Control) && (e.KeyCode == System.Windows.Forms.Keys.A))
            {
                foreach (ListViewItem item in lvwKeys.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manager dm = new Manager();
            string dataSourceName = dm.GetApplicationName(DataSourceId);

            if ((lvwKeys.SelectedItems.Count == 1)
                && (lvwKeys.SelectedItems[0].ForeColor != Color.DarkRed)
                && (EnableGlobalKeys == false))
            {
                
                if (MessageBox.Show(string.Format("Are you sure you want to delete the connection string '{0}'?", lvwKeys.SelectedItems[0].Text, dataSourceName),
                    "Delete Connection String", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {                    
                    IdpeKey key = dm.GetKey(DataSourceId, lvwKeys.SelectedItems[0].Text);
                    dm.DeleteKeyFromApplication(DataSourceId, lvwKeys.SelectedItems[0].Text, false);
                    try
                    {
                        dm.DeleteKey(key.KeyId);
                    }
                    catch
                    {
                        MessageBox.Show(string.Format("The connection string has been disassociated from '{0}', but master entry could not be deleted as the same key is associated with other data sources!", dataSourceName),
                            "Partially Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                 
                }

            }
            else if ((lvwKeys.SelectedItems.Count > 1)
               && (lvwKeys.SelectedItems[0].ForeColor != Color.DarkRed)
               && (EnableGlobalKeys == false))
            {
               foreach(ListViewItem item in lvwKeys.SelectedItems)
                {
                    IdpeKey key = dm.GetKey(DataSourceId, item.Text);
                    dm.DeleteKeyFromApplication(DataSourceId, item.Text, false);
                    try
                    {
                        dm.DeleteKey(key.KeyId);
                    }
                    catch
                    {
                        MessageBox.Show(string.Format("The connection string has been disassociated from '{0}', but master entry could not be deleted as the same key is associated with other data sources!", dataSourceName),
                            "Partially Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }

            Keys = dm.GetDataSourceKeysConnectionStrings(DataSourceId, false);
            BindData();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            deleteToolStripMenuItem.Enabled = ((lvwKeys.SelectedItems.Count > 0)
                && (lvwKeys.SelectedItems[0].ForeColor == Color.DarkRed)
                && (EnableGlobalKeys == false)) ? false : true;

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSources ds = new DataSources();
            Manager manager = new Manager();
            if (ds.ShowDialog() == DialogResult.OK)
            {
                int counter = 0;
                foreach (ListViewItem item in lvwKeys.SelectedItems)
                {
                    IdpeKey key = item.Tag as IdpeKey;
                    manager.Save(key, ds.SelectedDataSource.Id);
                    counter++;
                }
                if (counter > 0)
                    MessageBox.Show(string.Format("{0} database connections copied to {1}", counter, ds.SelectedDataSource.Name), 
                        "Copy Database Connections", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtConnectionString_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Control) && (e.KeyCode == System.Windows.Forms.Keys.A))
            {
                txtConnectionString.SelectAll();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cbDatabaseTypes.Enabled = true;
            txtConnectionStringName.ReadOnly = true;
            txtConnectionString.ReadOnly = true;

            txtConnectionStringName.Text = string.Empty;
            txtConnectionString.Text = string.Empty;
            cbDatabaseTypes.SelectedIndex = -1;

            cbDatabaseTypes.Focus();
        }
    }
}


