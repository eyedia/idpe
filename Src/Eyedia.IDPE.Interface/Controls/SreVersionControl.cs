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
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.IO;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface.Controls
{
    public partial class SreVersionControl : UserControl
    {
        public SreVersionControl()
        {
            InitializeComponent();
        }

        public string ObjectName { get; set; }

        int _ReferenceId;
        public int ReferenceId
        {
            get
            {
                return _ReferenceId;
            }
            set
            {
                _ReferenceId = value;
                BindData();
            }
        }
        public VersionObjectTypes VersionObjectType { get; set; }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (lvwVersions.SelectedItems.Count == 2)
            {
                compareToolStripMenuItem.Text = __Compare;
                contextMenuStrip1.Enabled = true;
            }
            else if (lvwVersions.SelectedItems.Count == 1)
            {
                compareToolStripMenuItem.Text = __SetToTheVersion;
                IdpeVersion version = lvwVersions.SelectedItems[0].Tag as IdpeVersion;
                contextMenuStrip1.Enabled = version.Version == 0 ? false : true;
            }
            else
            {
                e.Cancel = true;
            }

        }
        public bool Reverted { get; private set; }
        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compareToolStripMenuItem.Text == __Compare)
            {
                if (lvwVersions.SelectedItems.Count == 2)
                    SreVersionComparer.Compare(VersionObjectType, ObjectName, (IdpeVersion)lvwVersions.SelectedItems[0].Tag, (IdpeVersion)lvwVersions.SelectedItems[1].Tag);
            }
            else if (compareToolStripMenuItem.Text == __SetToTheVersion)
            {
                if (lvwVersions.SelectedItems.Count == 1)
                {
                    IdpeVersion version = lvwVersions.SelectedItems[0].Tag as IdpeVersion;
                    if (MessageBox.Show(string.Format("Are you sure you want to revert '{0}' to Version {1}?", ObjectName, version.Version)
                        , "Revert Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            this.Cursor = Cursors.WaitCursor;
                            switch (VersionObjectType)
                            {
                                case VersionObjectTypes.Attribute:
                                    break;

                                case VersionObjectTypes.DataSource:
                                    DataSourceBundle dataSourceBundle = SreVersionComparer.ConvertToSreVersionObject(VersionObjectTypes.DataSource, version) as DataSourceBundle;
                                    dataSourceBundle.Import();
                                    Reverted = true;
                                    break;

                                case VersionObjectTypes.Rule:
                                    DataSourcePatch dataSourcePatch = SreVersionComparer.ConvertToSreVersionObject(VersionObjectTypes.Rule, version) as DataSourcePatch;
                                    dataSourcePatch.Import();
                                    Reverted = true;
                                    break;
                            }
                            //this.Close();
                        }
                        catch (Exception ex)
                        {
                            //toolStripStatusLabel1.Text = ex.Message;
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
            }

            if (Reverted)
            {
                this.ParentForm.DialogResult = DialogResult.OK;
                this.ParentForm.Close();
            }
        }

        #region Helpers
        private void BindData()
        {
            BindTitle();
            BindVersions();
        }
        private void BindVersions()
        {
            if (ReferenceId == 0)
                return;

            Manager manager = new Manager();
            string myIpAddress = Eyedia.Core.Net.Dns.GetLocalIpAddress();
            ListViewItem currentItem = new ListViewItem("current");
            IdpeVersion currentVersion = new SreVersionManager().GetCurrentVersion(VersionObjectType, ReferenceId);
            currentItem.SubItems.Add(currentVersion.CreatedBy == Information.LoggedInUser.UserName ? "Me" : currentVersion.CreatedBy);
            currentItem.SubItems.Add(currentVersion.CreatedTS.ToString());
            currentItem.SubItems.Add(currentVersion.Source == myIpAddress? "This Machine": currentVersion.Source);
            currentItem.Tag = currentVersion;
            lvwVersions.Items.Add(currentItem);

            List<IdpeVersion> versions = manager.GetVersions(VersionObjectType, ReferenceId).OrderByDescending(v=> v.Version).ToList();

            foreach (IdpeVersion version in versions)
            {
                ListViewItem item = new ListViewItem(version.Version.ToString());
                item.SubItems.Add(version.CreatedBy == Information.LoggedInUser.UserName ? "Me" : version.CreatedBy);
                item.SubItems.Add(version.CreatedTS.ToString());
                item.SubItems.Add(version.Source == myIpAddress ? "This Machine" : version.Source);
                item.Tag = version;

                lvwVersions.Items.Add(item);
            }
           
        }

        private void BindTitle()
        {
            switch (VersionObjectType)
            {
                case VersionObjectTypes.Attribute:
                    IdpeAttribute attribute = new Manager().GetAttribute(ReferenceId);
                    if (attribute != null)
                        ObjectName = attribute.Name;
                    break;

                case VersionObjectTypes.DataSource:
                    ObjectName = new Manager().GetApplicationName(ReferenceId);
                    break;

                case VersionObjectTypes.Rule:
                    IdpeRule rule = new Manager().GetRule(ReferenceId);
                    if (rule != null)
                        ObjectName = rule.Name;
                    break;
            }
            this.Text = "Previous versions of " + ObjectName;
        }

        const string __Compare = "Compare";
        const string __SetToTheVersion = "Revert to this Version";
        #endregion Helpers
    }

}


