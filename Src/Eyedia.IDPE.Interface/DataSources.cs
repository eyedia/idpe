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
    public partial class DataSources : Form
    {
        public DataSources(int selectedDataSourceId = 0, string caption = "Select Data Source")
        {
            InitializeComponent();
            this.Text = caption;
            Bind(selectedDataSourceId);
        }

       
        public IdpeDataSource SelectedDataSource { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbDataSources.SelectedItems.Count > 0)
                SelectedDataSource = (IdpeDataSource)lbDataSources.SelectedItems[0];
            this.Close();
        }
     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedDataSource = null;
            this.Close();
        }

        private void Bind(int selectedDataSourceId = 0)
        {
            lbDataSources.DataSource = new Manager().GetDataSources(1).OrderBy(ds => ds.Name).ToList();
            lbDataSources.DisplayMember = "Name";
            if (selectedDataSourceId > 0)
            {
                int i = 0;
                bool found = false;
                for (i = 0; i < lbDataSources.Items.Count; i++)
                {
                    IdpeDataSource ds = lbDataSources.Items[i] as IdpeDataSource;
                    if ((ds != null) && (ds.Id == selectedDataSourceId))
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    lbDataSources.SelectedIndex = i;
            }
        }

        private void lbDataSources_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK_Click(sender, e);
        }


        private void DataSources_KeyUp(object sender, KeyEventArgs e)
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lbDataSources.Items.Count; i++)
            {
                if (((IdpeDataSource)lbDataSources.Items[i]).Name.ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    lbDataSources.SelectedIndex = i;
                    break;
                }
            }
        }
       
    }
}


