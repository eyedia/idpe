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

namespace Eyedia.IDPE.Interface
{
    public partial class SystemDataSources : Form
    {
        public SystemDataSources(Icon icon, int selectedSystemDataSourceId = 0)
        {
            InitializeComponent();
            this.Icon = icon;
            Bind(selectedSystemDataSourceId);
        }

       
        public IdpeDataSource SelectedSystemDataSource { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((lbSystemDataSources.Items.Count == 0) || (mAddMode))
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    IdpeDataSource newDs = new IdpeDataSource();
                    newDs.Name = txtSearch.Text;
                    newDs.IsActive = true;
                    newDs.IsSystem = true;
                    int newDsId = new Manager().Save(newDs);
                    newDs.Id = newDsId;
                    SelectedSystemDataSource = newDs;
                    ToggleMode();
                }
            }
            else
            {
                if (lbSystemDataSources.SelectedItems.Count > 0)
                    SelectedSystemDataSource = (IdpeDataSource)lbSystemDataSources.SelectedItems[0];
                this.Close();
            }
        }

        private void ToggleMode(bool defaultMode=true)
        {
            if (defaultMode)
            {
                mAddMode = false;
                txtSearch.Text = "";
                btnAdd.Enabled = true;
                lbSystemDataSources.Visible = true;
                btnOK.Enabled = true;
                this.Text = "System Names";
                Bind();
            }
            else
            {
                mAddMode = true;
                txtSearch.Text = "";
                btnAdd.Enabled = false;
                lbSystemDataSources.Visible = false;
                btnOK.Enabled = false;
                this.Text = "Add New System";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (mAddMode)
            {
                ToggleMode();
            }
            else
            {
                SelectedSystemDataSource = null;
                this.Close();
            }
        }

        private void Bind(int selectedSystemDataSourceId = 0)
        {          
            lbSystemDataSources.DataSource = new Manager().GetDataSources(2).OrderBy(ds => ds.Name).ToList();
            lbSystemDataSources.DisplayMember = "Name";
            if (selectedSystemDataSourceId > 0)
            {
                int i = 0;
                bool found = false;
                for (i = 0; i < lbSystemDataSources.Items.Count; i++)
                {
                    IdpeDataSource ds = lbSystemDataSources.Items[i] as IdpeDataSource;
                    if ((ds != null) && (ds.Id == selectedSystemDataSourceId))
                    {
                        found = true;
                        break;
                    }
                }
                if(found)
                    lbSystemDataSources.SelectedIndex = i;
            }
        }

        private void lbSystemDataSources_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            btnOK_Click(sender, e);
        }

        bool mAddMode;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ToggleMode(false);
        }

        private void SystemDataSources_KeyUp(object sender, KeyEventArgs e)
        {
            if (mAddMode)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    ToggleMode();
                }
                else if (e.KeyCode == Keys.Enter)
                {                   
                    btnOK_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else
            {
                if ((e.KeyCode == Keys.Escape)
                    && (txtSearch.Text.Length > 0))
                {
                    txtSearch.Text = "";
                }
                if ((e.KeyCode == Keys.Escape)
                && (txtSearch.Text == string.Empty))
                {
                    this.Close();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    btnOK_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (mAddMode)
            {
                bool found = false;
                for (int i = 0; i < lbSystemDataSources.Items.Count; i++)
                {
                    if (txtSearch.Text.Equals(((IdpeDataSource)lbSystemDataSources.Items[i]).Name, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }

                btnOK.Enabled = !found;
            }
            else
            {
                for (int i = 0; i < lbSystemDataSources.Items.Count; i++)
                {
                    if (((IdpeDataSource)lbSystemDataSources.Items[i]).Name.ToLower().Contains(txtSearch.Text.ToLower()))
                    {
                        lbSystemDataSources.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void lbSystemDataSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCopy.Enabled = lbSystemDataSources.SelectedIndex > -1 ? true : false;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            frmCopyDataSource copyDataSource = new frmCopyDataSource((IdpeDataSource)lbSystemDataSources.SelectedItem);
            if (copyDataSource.ShowDialog() == DialogResult.OK)
                Bind();
        }

        private void lbSystemDataSources_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                Delete();

        }

        private void Delete()
        {
            try
            {
                if(lbSystemDataSources.SelectedIndex > -1)
                new Manager().DeleteSystemDataSource(((IdpeDataSource)lbSystemDataSources.SelectedItem).Id);
                Bind();
            }
            catch
            {
                MessageBox.Show("Cannot delete the data source, as it is in use!", "Data Source in Use", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbSystemDataSources.SelectedIndex > -1)
            {
                frmExportImport exportUI = new frmExportImport(((IdpeDataSource)lbSystemDataSources.SelectedItem).Id);
                exportUI.ShowDialog();
            }
        }
    }
}


