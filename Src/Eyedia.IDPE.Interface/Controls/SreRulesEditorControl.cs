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
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.Interface.Controls;
using Eyedia.IDPE.Interface.RuleEditor.Utilities;
using System.Windows.Forms.Integration;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class SreRulesEditorControl : UserControl
    {
        IdpeDataSource mDataSource;
        public IdpeDataSource DataSource
        {
            get { return mDataSource; }
            set
            {
                mDataSource = value;
                Bind();
            }
        }

        bool _ShowSqlInitRules;
        [Browsable(true)]
        public bool ShowSqlInitRules
        {
            get
            {
                return _ShowSqlInitRules;
            }

            set
            {
                _ShowSqlInitRules = value;

                if (_ShowSqlInitRules)
                {
                    if (tabControl.TabPages.Count >= 5)
                    {
                        tabControl.TabPages.Clear();
                        tabControl.TabPages.Remove(tabPage1);
                        tabControl.TabPages.Remove(tabPage2);
                        tabControl.TabPages.Remove(tabPage3);
                        tabControl.TabPages.Remove(tabPage4);
                        tabControl.TabPages.Remove(tabPage5);
                        tabControl.TabPages.Add(tabPage6);
                    }
                }
                else
                {
                    selectedRuleListView = lvPreValidate;
                    selectedRuleSetType = RuleSetTypes.PreValidate;

                    if (tabControl.TabPages.Count != 5)
                    {
                        tabControl.TabPages.Clear();
                        tabControl.TabPages.Add(tabPage1);
                        tabControl.TabPages.Add(tabPage2);
                        tabControl.TabPages.Add(tabPage3);
                        tabControl.TabPages.Add(tabPage4);
                        tabControl.TabPages.Add(tabPage5);
                    }
                }
            }
        }

        public SreRulesEditorControl()
        {
            InitializeComponent();
            tabControl.TabPages.Remove(tabPage6);
            tabControl_SelectedIndexChanged(null, null);
            lvPreValidate.ShowAddButton = true;
            lvPreValidate.ShowPosition = true;
            lvPreValidate.ShowRepositionCheckBox = true;            
            lvPreValidate.AddButtonClick += new EventHandler(OnRuleAddClick);
            lvPreValidate.Repositioned += new EventHandler(OnRepositioned);
            lvPreValidate.ListView.DoubleClick += new EventHandler(OnDoubleClick);
            lvPreValidate.ListView.Resize += new EventHandler(OnResize);

            lvRowPreparing.ShowAddButton = true;
            lvRowPreparing.ShowPosition = true;
            lvRowPreparing.ShowRepositionCheckBox = true;
            lvRowPreparing.AddButtonClick += new EventHandler(OnRuleAddClick);
            lvRowPreparing.Repositioned += new EventHandler(OnRepositioned);
            lvRowPreparing.ListView.DoubleClick += new EventHandler(OnDoubleClick);
            lvRowPreparing.ListView.Resize += new EventHandler(OnResize);

            lvRowPrepared.ShowAddButton = true;
            lvRowPrepared.ShowPosition = true;
            lvRowPrepared.ShowRepositionCheckBox = true;
            lvRowPrepared.AddButtonClick += new EventHandler(OnRuleAddClick);
            lvRowPrepared.Repositioned += new EventHandler(OnRepositioned);
            lvRowPrepared.ListView.DoubleClick += new EventHandler(OnDoubleClick);
            lvRowPrepared.ListView.Resize += new EventHandler(OnResize);

            lvRowValidate.ShowAddButton = true;
            lvRowValidate.ShowPosition = true;
            lvRowValidate.ShowRepositionCheckBox = true;
            lvRowValidate.AddButtonClick += new EventHandler(OnRuleAddClick);
            lvRowValidate.Repositioned += new EventHandler(OnRepositioned);
            lvRowValidate.ListView.DoubleClick += new EventHandler(OnDoubleClick);
            lvRowValidate.ListView.Resize += new EventHandler(OnResize);

            lvPostValidate.ShowAddButton = true;
            lvPostValidate.ShowPosition = true;
            lvPostValidate.ShowRepositionCheckBox = true;
            lvPostValidate.AddButtonClick += new EventHandler(OnRuleAddClick);
            lvPostValidate.Repositioned += new EventHandler(OnRepositioned);
            lvPostValidate.ListView.DoubleClick += new EventHandler(OnDoubleClick);
            lvPostValidate.ListView.Resize += new EventHandler(OnResize);


            lvSqlInit.ShowAddButton = true;
            lvSqlInit.ShowPosition = true;
            lvSqlInit.ShowRepositionCheckBox = true;
            lvSqlInit.AddButtonClick += new EventHandler(OnRuleAddClick);
            lvSqlInit.Repositioned += new EventHandler(OnRepositioned);
            lvSqlInit.ListView.DoubleClick += new EventHandler(OnDoubleClick);
            lvSqlInit.ListView.Resize += new EventHandler(OnResize);

        }

        private void OnResize(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            if(listView.Columns.Count > 0)
            {
                listView.Columns[0].Width = (int)(listView.Width * .80);
            }
        }

        void OnDoubleClick(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
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
                    ruleEditor.Show();
                }
                else
                {
                    MessageBox.Show("Workflow Window already open for rule \"" + idpeRule.Name + "\"", "SRE warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Cursor = Cursors.Default;

            }
        }

        void OnRepositioned(object sender, EventArgs e)
        {
            Manager manager = new Manager();
            for (int i = 0; i < selectedRuleListView.ListView.Items.Count; i++)
            {
                IdpeRuleDataSource idpeRuleDataSource = new IdpeRuleDataSource();
                IdpeRule rule = selectedRuleListView.ListView.Items[i].Tag as IdpeRule;
                if (rule == null)
                    rule = manager.GetRule(selectedRuleListView.ListView.Items[i].Text);

                idpeRuleDataSource.DataSourceId = this.DataSource.Id;
                idpeRuleDataSource.RuleId = rule.Id;
                idpeRuleDataSource.RuleSetType = (int)selectedRuleSetType;
                idpeRuleDataSource.Priority = i + 1;
                manager.Save(idpeRuleDataSource);

                selectedRuleListView.ListView.Items[i].SubItems[1].Text = (i + 1).ToString();
            }
        }

       
        private void Bind()
        {
            if (this.DataSource == null)
                return;

            List<IdpeRule> rules = new Manager().GetRules(this.DataSource.Id);
            if ((!ShowSqlInitRules) && (tabControl.TabPages.Count >= 5))
            {                
                lvPreValidate.Rules = rules.Where(r => r.RuleSetType == (int)RuleSetTypes.PreValidate).ToList();
                lvRowPreparing.Rules = rules.Where(r => r.RuleSetType == (int)RuleSetTypes.RowPreparing).ToList();
                lvRowPrepared.Rules = rules.Where(r => r.RuleSetType == (int)RuleSetTypes.RowPrepared).ToList();
                lvRowValidate.Rules = rules.Where(r => r.RuleSetType == (int)RuleSetTypes.RowValidate).ToList();
                lvPostValidate.Rules = rules.Where(r => r.RuleSetType == (int)RuleSetTypes.PostValidate).ToList();                
            }
            else
            {
                tabControl.TabPages.Remove(tabPage1);
                tabControl.TabPages.Remove(tabPage2);
                tabControl.TabPages.Remove(tabPage3);
                tabControl.TabPages.Remove(tabPage4);
                tabControl.TabPages.Remove(tabPage5);
                lvSqlInit.Rules = rules.Where(r => r.RuleSetType == (int)RuleSetTypes.SqlPullInit).ToList();
                selectedRuleListView = lvSqlInit;
                selectedRuleSetType = RuleSetTypes.SqlPullInit;
            }
        }

        private void OnRuleAddClick(object sender, EventArgs e)
        {

            ListViewItem item = null;
            List<String> doNotIncludeList = selectedRuleListView.ListView.Items.Cast<ListViewItem>().Select(i => i.Text).ToList();

            frmSelectRule frmSelectRuleSet = new frmSelectRule(doNotIncludeList);
            if (frmSelectRuleSet.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                item = new ListViewItem(frmSelectRuleSet.SelectedRule.Name);
                item.Tag = frmSelectRuleSet.SelectedRule;
            }

            if (item == null)
                return;

            int priority = selectedRuleListView.ListView.Items.Count + 1;
            item.SubItems.Add(priority.ToString());
            selectedRuleListView.ListView.Items.Add(item);

            SreDataSourceProperty.KeepVersion(this.DataSource.Id);

            IdpeRuleDataSource idpeRuleDataSource = new IdpeRuleDataSource();
            idpeRuleDataSource.DataSourceId = DataSource.Id;
            idpeRuleDataSource.RuleId = frmSelectRuleSet.SelectedRule.Id;
            idpeRuleDataSource.Priority = priority;
            idpeRuleDataSource.RuleSetType = (int)selectedRuleSetType;
            new DataManager.Manager().Save(idpeRuleDataSource);

        }

        public void SelectTabs(RuleSetTypes ruleType)
        {
            if (ruleType == RuleSetTypes.PreValidate)
                tabControl.SelectedTab = tabPage1;
            else if (ruleType == RuleSetTypes.RowPreparing)
                tabControl.SelectedTab = tabPage2;
            else if (ruleType == RuleSetTypes.RowPrepared)
                tabControl.SelectedTab = tabPage3;
            else if (ruleType == RuleSetTypes.RowValidate)
                tabControl.SelectedTab = tabPage4;
            else if (ruleType == RuleSetTypes.PostValidate)
                tabControl.SelectedTab = tabPage5;
            else if (ruleType == RuleSetTypes.SqlPullInit)
                tabControl.SelectedTab = tabPage6;
        }

        private void miRename_Click(object sender, EventArgs e)
        {           
            if ((selectedRuleListView.ListView != null)
                    && (selectedRuleListView.ListView.SelectedItems.Count > 0))
            {
                IdpeRule rule = selectedRuleListView.ListView.SelectedItems[0].Tag as IdpeRule;
                InputBox inputBox = new InputBox(rule.Name, "New Name", "Rename Rule");
                inputBox.txtInput.TextChanged += new EventHandler(txtInput_TextChanged);
                if (inputBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new Manager().RenameRule(rule.Id, inputBox.TheInput);
                    Bind();
                }
            }

        }

        private void miDisassociate_Click(object sender, EventArgs e)
        {            
            DeleteRule();
        }

        private void miDisassociateDelete_Click(object sender, EventArgs e)
        {
            DeleteRule(true);
        }

        private void miShowDependencies_Click(object sender, EventArgs e)
        {
            if ((selectedRuleListView != null)
                   && (selectedRuleListView.ListView.SelectedItems.Count > 0))
            {
                IdpeRule rule = selectedRuleListView.ListView.SelectedItems[0].Tag as IdpeRule;
                RuleDependencies ruleDependencies = new RuleDependencies(rule.Id);
                ruleDependencies.ShowDialog();
            }
        }

        void txtInput_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            InputBox inputBoxForm = (InputBox)txtBox.FindForm();
            if (string.IsNullOrEmpty(txtBox.Text))
                inputBoxForm.btnOK.Enabled = false;
            else if (new Manager().GetRule(txtBox.Text) != null)
                inputBoxForm.btnOK.Enabled = false;
            else
                inputBoxForm.btnOK.Enabled = true;
        }

        private void DeleteRule(bool deleteRule = false)
        {

            if (selectedRuleListView.ListView.SelectedItems.Count > 0)
            {
                if (deleteRule == false)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to disassociate rule '{0}' from '{1}'? ", selectedRuleListView.ListView.SelectedItems[0].Text, DataSource.Name),
                        "Disassociation of Rule", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        DisassociateRuleFromDatabase(selectedRuleListView.ListView, false);
                    }
                }
                else
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to disassociate rule '{0}' from '{1}'? And also delete the rule paramently? ", selectedRuleListView.ListView.SelectedItems[0].Text, DataSource.Name),
                        "Delete Rule", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        DisassociateRuleFromDatabase(selectedRuleListView.ListView, true);
                    }
                }

            }

        }

        private void DisassociateRuleFromDatabase(ListView listView, bool deleteRule)
        {
            SreDataSourceProperty.KeepVersion(this.DataSource.Id);

            IdpeRule rule = (IdpeRule)listView.SelectedItems[0].Tag;            
            new Manager().DeleteRuleFromDataSource(DataSource.Id, rule.Id, selectedRuleSetType, deleteRule);
            listView.Items.Remove(listView.SelectedItems[0]);
            Bind();
        }

        SreListView selectedRuleListView;
        RuleSetTypes selectedRuleSetType;
      
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ShowSqlInitRules)
            {
                switch (tabControl.SelectedIndex)
                {
                    case 0:
                        selectedRuleListView = lvPreValidate;
                        selectedRuleSetType = RuleSetTypes.PreValidate;
                        break;

                    case 1:
                        selectedRuleListView = lvRowPreparing;
                        selectedRuleSetType = RuleSetTypes.RowPreparing;
                        break;

                    case 2:
                        selectedRuleListView = lvRowPrepared;
                        selectedRuleSetType = RuleSetTypes.RowPrepared;
                        break;

                    case 3:
                        selectedRuleListView = lvRowValidate;
                        selectedRuleSetType = RuleSetTypes.RowValidate;
                        break;

                    case 4:
                        selectedRuleListView = lvPostValidate;
                        selectedRuleSetType = RuleSetTypes.PostValidate;
                        break;

                }
            }
            else
            {
                selectedRuleListView = lvSqlInit;
                selectedRuleSetType = RuleSetTypes.SqlPullInit;

            }
        }

        private void miImport_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch(openFileDialog.FileName);
                dsp.Import();
                OnRuleAddClick(sender, e);
                Bind();
            }
        }

        private void miExport_Click(object sender, EventArgs e)
        {
            if (selectedRuleListView.ListView.SelectedItems.Count > 0)
            {
                saveFileDialog.FileName = selectedRuleListView.ListView.SelectedItems[0].Text;
                saveFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataSourcePatch dsp = new DataSourcePatch();
                    foreach (ListViewItem item in selectedRuleListView.ListView.SelectedItems)
                    {
                        dsp.Rules.Add((IdpeRule)item.Tag);
                    }
                    dsp.Export(saveFileDialog.FileName);
                }
            }
        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}


