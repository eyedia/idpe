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
using Eyedia.Core.Data;
using Eyedia.IDPE.DataManager;
using Eyedia.Core;
using System.Configuration;
using Eyedia.IDPE.Common;
using System.Diagnostics;
using System.Windows.Forms.Integration;
using Eyedia.IDPE.Interface.RuleEditor.Utilities;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class Rules : DockContent
    {
        public Rules()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            RefreshRules();
          
        }
        public ListViewColumnSorter ColumnSorter = null;
        public void RefreshRules(string keyWords = null)
        {
            List<IdpeRule> rules = new Manager().GetRules();
            rules.Where(r => r.Description == null).ToList().ForEach( r1 => r1.Description = "");
            if(keyWords != null)
                rules = rules.Where(r => (r.Name.ToLower().Contains(keyWords.ToLower())) || (r.Description.ToLower().Contains(keyWords.ToLower()))).ToList();

            listView.Columns.Clear();
            listView.Columns.Add("Name", 400);
            listView.Columns.Add("Description", 500);
            listView.Columns.Add("Date Modified", 140);
            listView.Columns.Add("Modified By", 120);
            listView.Items.Clear();
            foreach (IdpeRule rule in rules)
            {
                ListViewItem item = new ListViewItem(rule.Name);
                item.SubItems.Add(rule.Description);
                item.SubItems.Add(rule.CreatedTS.ToString());
                item.SubItems.Add(rule.CreatedBy);
                item.Tag = rule;
                listView.Items.Add(item);
            }
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            
            if (listView.SelectedItems.Count > 0)
            {
                
                this.Cursor = Cursors.WaitCursor;           
                IdpeRule idpeRule = new Manager().GetRule((listView.SelectedItems[0].Tag as IdpeRule).Id);
                RuleEditor.MainWindow ruleEditor = new RuleEditor.MainWindow(idpeRule, Common.RuleSetTypes.RowPreparing);
                string sname = idpeRule.Name.Replace("-", "").Replace(" ", "");
                ruleEditor.Name = sname;                
                if (!CheckOpenWindow.Helpers.IsWindowOpen(sname))
                {
                    CheckOpenWindow.Helpers.AddWindowToList(sname);
                    ElementHost.EnableModelessKeyboardInterop(ruleEditor);
                    ruleEditor.RuleForm = this;
                    ruleEditor.Show();
                }
                else
                {
                    MessageBox.Show("Workflow Window already open for rule \"" + idpeRule.Name + "\"","IDPE warning",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                this.Cursor = Cursors.Default;
                
                
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            IdpeRule idpeRule = new IdpeRule();
            this.Cursor = Cursors.WaitCursor;
            FormAlreadyOpened(idpeRule);
            RuleEditor.MainWindow ruleEditor = new RuleEditor.MainWindow(idpeRule, Common.RuleSetTypes.RowPrepared);
            ElementHost.EnableModelessKeyboardInterop(ruleEditor);
            ruleEditor.RuleForm = this;
            this.Cursor = Cursors.Default;
            ruleEditor.Show();
            
        }
        private bool FormAlreadyOpened(IdpeRule idperule)
        {
            bool IsAlreadyOpened = false;
            


            return IsAlreadyOpened;
        }
        private void copyToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                frmCopyRule copyRule = new frmCopyRule((listView.SelectedItems[0].Tag as IdpeRule).Id, listView.SelectedItems[0].Text);
                if (copyRule.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {                    
                    try
                    {
                        RefreshRules();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("An error occurred while copying rule!" + Environment.NewLine + ex.ToString(), "Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("If " + listView.SelectedItems[0].Text
                    + " is not associated with any data source, it will be deleted permanently. Are you sure you want to delete this rule?", "Delete Rule",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        new Manager().DeleteRule(listView.SelectedItems[0].Text);
                        RefreshRules();
                    }
                    catch
                    {
                        MessageBox.Show("Can not delete rule, as this rule is associated with one or more data source!", "Delete Rule Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                saveFileDialog.FileName = listView.SelectedItems[0].Text;
                saveFileDialog.Filter = "IDPE Patch Files (*.idpep)|*.idpep|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataSourcePatch dsp = new DataSourcePatch();                   
                    foreach (ListViewItem item in listView.SelectedItems)
                    {
                        dsp.Rules.Add((IdpeRule)item.Tag);
                    }
                    dsp.Export(saveFileDialog.FileName);
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            openFileDialog.Filter = "IDPE Patch Files (*.idpep)|*.idpep|All Files (*.*)|*.*";
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch(openFileDialog.FileName);
                dsp.Import();
                RefreshRules();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool itemSelected = listView.SelectedItems.Count > 0 ? true : false;
            copyToToolStripMenuItem.Enabled = itemSelected;
            deleteToolStripMenuItem.Enabled = itemSelected;
            btnDelete.Enabled = itemSelected;
            exportToolStripMenuItem.Enabled = itemSelected;
            renameToolStripMenuItem.Enabled = itemSelected;

            if (itemSelected)
            {
                if (Information.IsInternalRule(listView.SelectedItems[0].Text))
                {
                    deleteToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    deleteToolStripMenuItem.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }

            if(listView.SelectedItems.Count == 1)
            {
                IdpeRule rule = listView.SelectedItems[0].Tag as IdpeRule;
                versionsToolStripMenuItem.Enabled = new Data.UtilDataManager().IsHavingVersion(VersionObjectTypes.Rule, rule.Id);
                compareWithPreviousVersionToolStripMenuItem.Enabled = versionsToolStripMenuItem.Enabled;
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshRules();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                IdpeRule rule = listView.SelectedItems[0].Tag as IdpeRule;
                InputBox inputBox = new InputBox(rule.Name, "New Name", "Rename Rule", this.Icon);
                if (inputBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new Manager().RenameRule(rule.Id, inputBox.TheInput);
                    RefreshRules();
                }
            }
        }

        private void Rules_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listView.SelectedItems.Count > 0)
                {
                    if (!Information.IsInternalRule(listView.SelectedItems[0].Text))
                        deleteToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteToolStripMenuItem_Click(sender, e);
        }

        private void btnWizards_Click(object sender, EventArgs e)
        {
            RuleWizards ruleWizards = new RuleWizards(this.Icon);
            if (ruleWizards.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                RefreshRules();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshRules(txtSearch.Text);
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (ColumnSorter == null)
            {
                ColumnSorter = new ListViewColumnSorter();
                listView.ListViewItemSorter = ColumnSorter;
                listView.Sorting = SortOrder.Ascending;
                listView.AutoArrange = true;
            }

            if (e.Column == ColumnSorter.SortColumn)
            {
                if (ColumnSorter.Order == SortOrder.Ascending)
                {
                    ColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    ColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                ColumnSorter.SortColumn = e.Column;
                ColumnSorter.Order = SortOrder.Ascending;
            }

            listView.Sort();
        }

        private void showDependenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((listView != null)
                    && (listView.SelectedItems.Count > 0))
            {                
                IdpeRule rule = listView.SelectedItems[0].Tag as IdpeRule;
                RuleDependencies ruleDependencies = new RuleDependencies(rule.Id);
                ruleDependencies.ShowDialog();             
            }
        }

        private void versionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                IdpeRule rule = listView.SelectedItems[0].Tag as IdpeRule;
                frmVersions versions = new frmVersions(rule.Id, VersionObjectTypes.Rule);
                versions.ShowDialog();
            }
        }

        private void compareWithPreviousVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                IdpeRule rule = listView.SelectedItems[0].Tag as IdpeRule;
                List<IdpeVersion> lastVersions = new Manager().GetVersions(VersionObjectTypes.Rule, rule.Id);

                if (lastVersions.Count >= 2)
                    VersionComparer.Compare(VersionObjectTypes.Rule, rule.Name, lastVersions[0], lastVersions[1]);
                else
                    MessageBox.Show("Could not retrieve last 2 versions!", "Comparison Tool", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void validateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                IdpeRule rule = listView.SelectedItems[0].Tag as IdpeRule;
                DataSourceValidator dataSourceValidator = new DataSourceValidator(false, rule.Id,
                    "Attributes & Process Variables Referenced in " + rule.Name, this.Icon);
                dataSourceValidator.Show();
            }
        }
      
    }

    
}



