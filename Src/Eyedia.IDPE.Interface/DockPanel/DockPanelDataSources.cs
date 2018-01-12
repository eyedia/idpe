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
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Eyedia.IDPE.DataManager;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using System.Linq;
using System.IO;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class DockPanelDataSources : DockContent
    {
        public DockPanelDataSources()
        {           
            InitializeComponent();
            this.TabText = "Data Sources";
            this.Text = TabText;           
            this.sreListView1.DefaultItemId = Information.LoggedInUser.GetUserPreferences().DefaultDataSourceId;
            this.sreListView1.DataSources = new Manager().GetDataSources(1);
            dataSourceSummary1.Dock = DockStyle.Fill;
            globalSearchWidget1.Dock = DockStyle.Fill;        
        }
     
        public Rules RuleExplorer
        {
            get
            {
                foreach (var c in this.DockPanel.Contents)
                {
                    if (c is Rules) return c as Rules;
                }
                return null;
            }
        }

        public void RefreshData(int selectedDataSourceId = 0)
        {
            this.sreListView1.DefaultItemId = Information.LoggedInUser.GetUserPreferences().DefaultDataSourceId;
            this.sreListView1.DataSources = new Manager().GetDataSources(1);

            if (selectedDataSourceId > 0)
            {
                foreach(ListViewItem item in this.sreListView1.ListView.Items)
                {
                    if (((SreDataSource)item.Tag).Id == selectedDataSourceId)
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        break;
                    }
                }
            }
        }

        public DockPanelProperty DockPanelProperty
        {
            get
            {
                if (this.DockPanel.Contents != null)
                {
                    foreach (var c in this.DockPanel.Contents)
                    {
                        if (c is DockPanelProperty) return c as DockPanelProperty;
                    }
                }
                return null;
            }
        }

        SreDataSource mSelectedDataSource;
        internal SreDataSource SelectedDataSource
        { 
            get
            {
                return mSelectedDataSource;
            }
            set
            {
                mSelectedDataSource = value;
                dataSourceSummary1.DataSourceId = mSelectedDataSource.Id;
                dataSourceSummary1.DockPanelProperty = DockPanelProperty;
            }
        }

        private void mnuValidate_Click(object sender, EventArgs e)
        {
            DataSourceValidator dataSourceValidator = new DataSourceValidator(true, SelectedDataSource.Id, 
                "Missing Attributes & Process Variables Referenced in " + SelectedDataSource.Name, this.Icon);
            dataSourceValidator.Show();
        }

        private void mnuCompareWithPrevious_Click(object sender, EventArgs e)
        {
            List<SreVersion> lastVersions = new Manager().GetVersions(VersionObjectTypes.DataSource, SelectedDataSource.Id);

            if (lastVersions.Count >= 2)
                SreVersionComparer.Compare(VersionObjectTypes.DataSource, SelectedDataSource.Name, lastVersions[0], lastVersions[1]);
            else
                MessageBox.Show("Could not retrieve last 2 versions!", "Comparison Tool", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void mcnuShowVersions_Click(object sender, EventArgs e)
        {
            frmVersions versions = new frmVersions(SelectedDataSource.Id, VersionObjectTypes.DataSource, true, this.Icon);
            versions.ShowDialog();
            if ((versions.sreVersionControl1.Reverted)
                || (versions.sreVersionControl1.Reverted))
                RefreshData(SelectedDataSource.Id);
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {            
            frmCopyDataSource copyDataSource = new frmCopyDataSource(SelectedDataSource);
            if (copyDataSource.ShowDialog() == DialogResult.OK)
            {
                if (copyDataSource.NewDataSourceId > 0)
                    RefreshData();
            }
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            if (sreListView1.ListView.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete '" + sreListView1.ListView.SelectedItems[0].SubItems[0].Text + "'?", "Delete Data Source",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        new Manager().DeleteDataSource(SelectedDataSource.Id);
                        RefreshData();
                        if (RuleExplorer != null)
                            RuleExplorer.RefreshRules();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void mnuExport_Click(object sender, EventArgs e)
        {
            frmExportImport exportUI = new frmExportImport(SelectedDataSource.Id);
            exportUI.ShowDialog();
        }

        private void mnuExportKeys_Click(object sender, EventArgs e)
        {
            if (sreListView1.ListView.SelectedItems.Count > 0)
            {
                saveFileDialog.FileName = sreListView1.ListView.SelectedItems[0].SubItems[0].Text;
                saveFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataSourcePatch dsp = new DataSourcePatch();
                    dsp.Name = sreListView1.ListView.SelectedItems[0].SubItems[0].Text;
                    Manager mgr = new Manager();
                    List<SreKey> keys = mgr.GetApplicationKeys(mgr.GetApplicationId(dsp.Name), false);
                    foreach (SreKey key in keys)
                    {
                        dsp.Keys.Add(key);
                    }
                    dsp.Export(saveFileDialog.FileName);
                }
            }
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            frmExportImport importUI = new frmExportImport();
            if (importUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {                
                RefreshData();
                if (!string.IsNullOrEmpty(importUI.ApplicationName))
                {
                    foreach (ListViewItem item in sreListView1.ListView.Items)
                    {
                        if (item.SubItems[1].Text == importUI.ApplicationName)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void mnuImportKeys_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch(openFileDialog1.FileName);
                dsp.Import();
            }
        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void DockPanelDataSources_KeyUp(object sender, KeyEventArgs e)
        {
           // MessageBox.Show(e.KeyCode.ToString());
        }

        private void setAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sreListView1.ListView.SelectedItems.Count == 1)
            {
                UserPreferences up = Information.LoggedInUser.GetUserPreferences();
                up.DefaultDataSourceId = ((SreDataSource)sreListView1.ListView.SelectedItems[0].Tag).Id;
                Information.LoggedInUser.Preferences = up.Serialize();
                CoreDatabaseObjects.Instance.UpdateUserPreferences(Information.LoggedInUser);
            }
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            //SreEnvironments envs = SreEnvironmentServiceDispatcherFactory.GetEnvironments();
            //if (envs != null)
            //{
            //    deployToolStripMenuItem.DropDownItems.Clear();                
            //    foreach (SreEnvironment env in envs)
            //    {
            //        ToolStripMenuItem item = new ToolStripMenuItem(env.Name);
            //        item.Tag = env;
            //        item.Click += new System.EventHandler(OnDeploymentClick);
            //        deployToolStripMenuItem.DropDownItems.Add(item);
            //    }
            //}        
        }

        private void OnDeploymentClick(object sender, EventArgs e)
        {
            
        }
    }
}


