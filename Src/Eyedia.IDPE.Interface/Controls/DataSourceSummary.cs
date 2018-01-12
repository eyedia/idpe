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
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Services;
using Eyedia.IDPE.Common;
using System.Diagnostics;
using System.IO;

namespace Eyedia.IDPE.Interface.Controls
{
    public partial class DataSourceSummary : UserControl
    {
        public DataSourceSummary()
        {
            InitializeComponent();
        }

        int mDataSourceId;
        public int DataSourceId
        {
            get { return mDataSourceId; }
            set
            {
                mDataSourceId = value;
                if (!DesignMode)
                {
                    DataSource = new Services.DataSource(mDataSourceId, string.Empty);
                    Bind();
                }
            }
        }

        public DockPanelProperty DockPanelProperty { get; set; }
        
        public DataSource DataSource { get; private set; }

        private void Bind()
        {
            if (DataSource == null)
            {
                tableLayoutPanel1.Visible = false;
                return;
            }
            Manager manager = new Manager();

            SreDataSource sysDs = manager.GetDataSourceDetails((int)DataSource.SystemDataSourceId);
            tableLayoutPanel1.Visible = true;
            switch (DataSource.DataFeederType)
            {
                case DataFeederTypes.PullLocalFileSystem:
                case DataFeederTypes.PullFtp:
                case DataFeederTypes.PullSql:
                    lblDataFeeder.Text = "Pulls from";
                    break;

                case DataFeederTypes.Push:
                    lblDataFeeder.Text = "Hosted as Service";
                    break;
            }
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = DataSource.LocalFileSystemFolderPullFolder;
            lblDataFeederValue.Links.Clear();
            lblDataFeederValue.Links.Add(link);
            List<string> folders = new List<string>(DataSource.LocalFileSystemFolderPullFolder.Split("\\".ToCharArray()));
            if (folders.Count >= 3)
                folders = folders.Skip(Math.Max(0, folders.Count() - 3)).ToList();
            else if (folders.Count >= 2)
                folders = folders.Skip(Math.Max(0, folders.Count() - 2)).ToList();
            else if (folders.Count >= 1)
                folders = folders.Skip(Math.Max(0, folders.Count() - 1)).ToList();

            lblDataFeederValue.Text = string.Empty;
            foreach (string folder in folders)
            {
                lblDataFeederValue.Text += folder + "\\";
            }

            if (DataSource.BusinessRules != null)
            {
                llPreValidate.Text = DataSource.BusinessRules.Where(r => r.RuleSetType == RuleSetTypes.PreValidate).ToList().Count.ToString();
                llRowPreparing.Text = DataSource.BusinessRules.Where(r => r.RuleSetType == RuleSetTypes.RowPreparing).ToList().Count.ToString();
                llRowPrepared.Text = DataSource.BusinessRules.Where(r => r.RuleSetType == RuleSetTypes.RowPrepared).ToList().Count.ToString();
                llRowValidate.Text = DataSource.BusinessRules.Where(r => r.RuleSetType == RuleSetTypes.RowValidate).ToList().Count.ToString();
                llPostValidate.Text = DataSource.BusinessRules.Where(r => r.RuleSetType == RuleSetTypes.PostValidate).ToList().Count.ToString();
            }
            else
            {
                llPreValidate.Text = "0";
                llRowPreparing.Text = "0";
                llRowPrepared.Text = "0";
                llRowValidate.Text = "0";
                llPostValidate.Text = "0";
            }

            llKeys.Text = manager.GetDataSourceKeys(DataSourceId, true).Count.ToString();
            llPostEvents.Text = DataSource.PusherType.ToString();
            lblSystemName.Text = sysDs != null ? sysDs.Name : string.Empty;
        }

       
        private void lblDataFeederValue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(Directory.Exists(e.Link.LinkData.ToString()))
                Process.Start(e.Link.LinkData.ToString());
        }
       
        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DockPanelProperty != null)
            {
                if (((Control)sender).Name == llPreValidate.Name)
                    DockPanelProperty.SelectTabs(1, RuleSetTypes.PreValidate);
                else if (((Control)sender).Name == llRowPreparing.Name)
                    DockPanelProperty.SelectTabs(1, RuleSetTypes.RowPreparing);
                else if (((Control)sender).Name == llRowPrepared.Name)
                    DockPanelProperty.SelectTabs(1, RuleSetTypes.RowPrepared);
                else if (((Control)sender).Name == llRowValidate.Name)
                    DockPanelProperty.SelectTabs(1, RuleSetTypes.RowValidate);
                else if (((Control)sender).Name == llPostValidate.Name)
                    DockPanelProperty.SelectTabs(1, RuleSetTypes.PostValidate);
                else if (((Control)sender).Name == llKeys.Name)
                    DockPanelProperty.SelectTabs(2);
                else if (((Control)sender).Name == llDatabase.Name)
                    DockPanelProperty.SelectTabs(4);
                else if (((Control)sender).Name == llPostEvents.Name)
                    DockPanelProperty.SelectTabs(0);

            }
        }
    }
}


