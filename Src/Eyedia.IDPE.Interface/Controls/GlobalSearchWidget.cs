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
using System.Threading.Tasks;
using System.Windows.Forms;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class GlobalSearchWidget : UserControl
    {
        public GlobalSearchWidget()
        {
            InitializeComponent();       
        }
       
        public bool ShowSearchTextBox
        {
            set
            {
                pnlSearch.Visible = value;
            }
            get
            {
                return pnlSearch.Visible;
            }
        }

        bool _SingleDataSource;
        public bool SingleDataSource
        {
            get
            {
                return _SingleDataSource;
            }
            set
            {
                _SingleDataSource = value;
                lblCaption.Text = _SingleDataSource ? "Search" : "Global Search";
            }
        }

        public int DataSourceId { get; set; }

        private void lvwSearchResult_Resize(object sender, EventArgs e)
        {
            lvwSearchResult.Columns[0].Width = (int)(lvwSearchResult.Width * .25);
            lvwSearchResult.Columns[1].Width = (int)(lvwSearchResult.Width * .25);
            lvwSearchResult.Columns[2].Width = (int)(lvwSearchResult.Width * .40);
        }
       
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length < 3)
            {
                errorProvider1.SetError(lblCaption, "Provide at least 3 or more characters to be searched!");               
            }
            else
            {
                errorProvider1.Clear();
                this.Cursor = Cursors.WaitCursor;
                Services.GlobalSearch globalSearchService = new Services.GlobalSearch();
                List<GSearchResultCube> results = globalSearchService.Search(txtSearch.Text, DataSourceId);

                lvwSearchResult.Items.Clear();
                foreach (GSearchResultCube result in results)
                {
                    ListViewItem item = new ListViewItem(AddGroup(result.DataSource));
                    item.Text = result.Where;
                    //item.SubItems.Add(result.Where);
                    item.SubItems.Add(result.Type);
                    item.SubItems.Add(result.ReferenceName);
                    lvwSearchResult.Items.Add(item);
                }

                this.Cursor = Cursors.Default;
            }
        }

        private ListViewGroup AddGroup(string dataSourceName)
        {
            foreach (ListViewGroup grp in lvwSearchResult.Groups)
            {
                if (grp.Header == dataSourceName)
                    return grp;
            }

            ListViewGroup newgrp = new ListViewGroup(dataSourceName);
            lvwSearchResult.Groups.Add(newgrp);
            return newgrp;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}


