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
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface
{
    public partial class DockPanelDataSourceSystemAttributes : DockContent
    {
        public DockPanelDataSourceSystemAttributes()
        {
            InitializeComponent();
            this.TabText = "System Attributes";
            this.Text = this.TabText;
        }

        public IdpeDataSource SelectedDataSource { get; set; }

        private int _SystemDataSourceId;
        public int SystemDataSourceId
        {
            get { return _SystemDataSourceId; }
            set
            {
                _SystemDataSourceId = value;
                RefreshData();
            }
        }

        public void RefreshData()
        {
            Manager manager = new Manager();
            sreListView1.Attributes = manager.GetAttributes(SystemDataSourceId);
            if (SelectedDataSource != null)
            {
                int count = manager.GetDataSourceCountUnderSystemDataSource((int)SelectedDataSource.SystemDataSourceId);
                MultipleDataSources = count > 1;
                SystemDataSourceName = manager.GetApplicationName((int)SelectedDataSource.SystemDataSourceId);
                this.TabText = MultipleDataSources ? "* " + SystemDataSourceName : SystemDataSourceName;
                this.Text = this.TabText;
            }
        }

        public string SystemDataSourceName { get; set; }

        public bool MultipleDataSources { get; set; }
        public MainWindow MainWindow { get; set; }

        private void mnuDisassociate_Click(object sender, EventArgs e)
        {
            if (CheckIfMultipleDataSources())               
                MainWindow.DisassociateAttributeFromDataSource(sreListView1, true);
        }

       
        private void mnuMakePrintable_Click(object sender, EventArgs e)
        {
            if (CheckIfMultipleDataSources())
            {
                foreach (ListViewItem selectedItem in sreListView1.ListView.SelectedItems)
                {
                    IdpeAttribute attribute = selectedItem.Tag as IdpeAttribute;

                    bool acceptable = true;
                    ToolStripItem tItem = sender as ToolStripItem;
                    acceptable = !tItem.Text.Contains("Not Printable");
                    new Manager().MakeAttributeAcceptableNew((int)SelectedDataSource.SystemDataSourceId, attribute.AttributeId, acceptable);
                }
                RefreshData();
            }
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {            
            if (sreListView1.ListView.SelectedItems.Count == 1)
            {
                mnuDisassociate.Enabled = true;
                IdpeAttribute attribute = sreListView1.ListView.SelectedItems[0].Tag as IdpeAttribute;
                if (attribute.Name != "IsValid")
                {
                    if (attribute.IsAcceptable == false)
                        mnuMakePrintable.Text = string.Format("Make [{0}] Printable", attribute.Name);
                    else
                        mnuMakePrintable.Text = string.Format("Make [{0}] Not Printable", attribute.Name);
                }
                else
                {
                    mnuDisassociate.Enabled = false;
                }
            }
        }

        private void miImport_Click(object sender, EventArgs e)
        {
            if (CheckIfMultipleDataSources())
                MainWindow.ImportAttributes(sreListView1, true);
        }

        private void miExport_Click(object sender, EventArgs e)
        {
            MainWindow.ExportAttributes(sreListView1, true);
        }

        private void versionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVersions versions = new frmVersions((int)SelectedDataSource.SystemDataSourceId, VersionObjectTypes.DataSource, false, this.Icon);           
            if (versions.ShowDialog() == DialogResult.OK)
                MainWindow.RefreshData(SelectedDataSource.Id);
        }
        private bool CheckIfMultipleDataSources()
        {
            if (MultipleDataSources)
            {
                string question = string.Format("The system data source {0} is associated with more than 1 data source. Updating this will impact other related data sources as well. Do you still want to make the changes?",
                    SystemDataSourceName);
                if (MessageBox.Show(question, "System Data Source", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2
                    ) == DialogResult.Yes)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}


