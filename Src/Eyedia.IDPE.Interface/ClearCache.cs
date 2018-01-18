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
using System.Configuration;
using System.Reflection;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{
    public partial class ClearCache : Form
    {
        public ClearCache()
        {
            InitializeComponent();
            ;

            lvwDataSources.Columns[0].Width = lvwDataSources.Width - 4;
            BindData();            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearCache_Activated(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }
      
        private void ClearCache_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                btnOK_Click(sender, e);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                lvwDataSources.Focus();
        }

        private void btnClearCache_Click(object sender, EventArgs e)
        {            
            int successCount = 0;
            this.Cursor = Cursors.WaitCursor;
            foreach (ListViewItem item in lvwDataSources.CheckedItems)
            {
                Data.ExternalSystem selectedDataSource = item.Tag as Data.ExternalSystem;

                try
                {
                    ServiceCommunicator.ClearDataSource(selectedDataSource.ExternalSystemId, selectedDataSource.ExternalSystemName);
                    successCount++;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.Cursor = Cursors.Default;

            if (successCount == lvwDataSources.CheckedItems.Count)
                MessageBox.Show("Success!", "Cache Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if ((successCount == 0)
                && (lvwDataSources.CheckedItems.Count > 0))
                MessageBox.Show("All Failed!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Partially Failed!", "Partially Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

     
        private void BindData()
        {
            lvwDataSources.Items.Clear();
            BindingList<Data.ExternalSystem> datasources = new Interface.Data.UtilDataManager().GetAllExternalSystems(null, txtSearch.Text.ToLower());
            foreach (Data.ExternalSystem externalSystem in datasources)
            {
                ListViewItem item = new ListViewItem(externalSystem.ExternalSystemName);
                item.Tag = externalSystem;
                if (externalSystem.IsSystemType)
                    item.ForeColor = Color.DarkRed;

                lvwDataSources.Items.Add(item);
            }

        }

        private void lvwDataSources_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            btnClearCache.Enabled = lvwDataSources.CheckedItems.Count > 0 ? true : false;
        }

      
    }
}





