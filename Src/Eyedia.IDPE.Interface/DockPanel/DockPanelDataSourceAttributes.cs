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
    public partial class DockPanelDataSourceAttributes : DockContent
    {
        public DockPanelDataSourceAttributes()
        {
            InitializeComponent();
            this.TabText = "Attributes";
            this.Text = "Attributes";
        }
        private int _DataSourceId;
        public int DataSourceId
        {
            get { return _DataSourceId; }
            set
            {
                _DataSourceId = value;
                sreListView1.Attributes = new Manager().GetAttributes(_DataSourceId);
                if (SelectedDataSource != null)
                {
                    this.TabText = SelectedDataSource.Name;
                    this.Text = this.TabText;
                }
            }
        }
        public IdpeDataSource SelectedDataSource { get; set; }

        public MainWindow MainWindow { get; set; }

        private void mnuDisassociate_Click(object sender, EventArgs e)
        {
            MainWindow.DisassociateAttributeFromDataSource(sreListView1);

        }

        private void miImport_Click(object sender, EventArgs e)
        {
            MainWindow.ImportAttributes(sreListView1);
        }

        private void miExport_Click(object sender, EventArgs e)
        {
            MainWindow.ExportAttributes(sreListView1);
        }

        private void versionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVersions versions = new frmVersions(SelectedDataSource.Id, VersionObjectTypes.DataSource, false, this.Icon);
            if(versions.ShowDialog() == DialogResult.OK)            
                MainWindow.RefreshData(SelectedDataSource.Id);
        }
    }
}


