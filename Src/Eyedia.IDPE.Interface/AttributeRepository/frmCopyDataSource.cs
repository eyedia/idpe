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
using Eyedia.IDPE.Interface.Data;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface
{
    public partial class frmCopyDataSource : Form
    {
        bool Copying;
        IdpeDataSource FromDataSource;
        Manager DataManager;
        public frmCopyDataSource(IdpeDataSource fromDataSource)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DataManager = new Manager();
            FromDataSource = fromDataSource;
            lblFrom.Text = FromDataSource.Name;
            txtApplicationName.Text = FromDataSource.Name + "_New";
            Bind();
            
        }

        private void Bind()
        {
            treeView.Nodes.Clear();
            treeView.Nodes.Add("Attributes");
            treeView.Nodes.Add("Keys");
            treeView.Nodes.Add("Rules");
            BindAttributes();
            BindKeys();
            BindRules();

            treeView.Nodes[0].Checked = true;
            treeView.Nodes[1].Checked = true;
            treeView.Nodes[2].Checked = true;
        }

        private void BindAttributes()
        {
            Manager manager = new Manager();
            List<IdpeAttribute> attributes = DataManager.GetAttributes(FromDataSource.Id);
            attributes = attributes.Where(a => a.AttributeId != 1).ToList();
            List<IdpeAttributeDataSource> sadss = new List<IdpeAttributeDataSource>();
            foreach (IdpeAttribute a in attributes)
            {
                IdpeAttributeDataSource sads = new IdpeAttributeDataSource();
                sads.AttributeId = a.AttributeId;
                sads.DataSourceId = FromDataSource.Id;
                sads.IsAcceptable = true;
                sads.AttributeDataSourceId = 0;
                sads.AttributePrintValueType = a.AttributePrintValueType;
                sads.AttributePrintValueCustom = a.AttributePrintValueCustom;

                TreeNode node = new TreeNode(manager.GetAttribute(a.AttributeId).Name);
                node.Tag = sads;
                treeView.Nodes[0].Nodes.Add(node);
                sadss.Add(sads);
            }
            treeView.Nodes[0].Tag = sadss;
            //DataManager.SaveAssociations(NewDataSourceId, sadss);
        }

        private void BindKeys()
        {
            Manager manager = new Manager();
            List<IdpeKey> keys = DataManager.GetApplicationKeys(FromDataSource.Id, false);

            if (keys != null)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    if (((IdpeKeyTypes)keys[i].Type) == IdpeKeyTypes.Custom)
                    {
                        IdpeKey newKey = new IdpeKey();

                        newKey.Name = keys[i].Name;
                        newKey.Value = keys[i].Value;
                        newKey.Type = keys[i].Type;
                        newKey.IsDeployable = keys[i].IsDeployable;

                        TreeNode node = new TreeNode(newKey.Name);
                        node.Tag = newKey;
                        treeView.Nodes[1].Nodes.Add(node);
                    }

                    //DataManager.Save(newKey, newDataSource.Id);
                }

            }
        }

        private void BindRules()
        {
            Manager manager = new Manager();            
            List<IdpeRule> rules = DataManager.GetRules(FromDataSource.Id);
            foreach (IdpeRule rule in rules)
            {
                TreeNode node = new TreeNode(manager.GetRule(rule.Id).Name);
                node.Tag = rule;
                treeView.Nodes[2].Nodes.Add(node);
            }
        }

        private void frmCopyApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Copying)
                e.Cancel = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                CopyDataSource();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
                Copying = false;
            }
            this.Cursor = Cursors.Default;
            this.Close();
        }

        private void txtApplicationName_TextChanged(object sender, EventArgs e)
        {
            if (txtApplicationName.Text.Length > 0)
            {
                if (DataManager.ApplicationExists(txtApplicationName.Text))
                {
                    toolStripStatusLabel1.Text = "Data source exists";
                    btnOK.Enabled = false;
                }
                else
                {
                    toolStripStatusLabel1.Text = "Ready";
                    btnOK.Enabled = true;
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Enter a valid data source name";
                btnOK.Enabled = false;
            }
        }

        private void txtApplicationName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (btnOK.Enabled))
            {
                btnOK_Click(null, null);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        public int NewDataSourceId { get; private set; }
        private void CopyDataSource()
        {
            Copying = true;
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Increment(10);
            toolStripStatusLabel1.Text = "Copying datsource...";
            Application.DoEvents();
            IdpeDataSource newDataSource = new IdpeDataSource();
            newDataSource.IsActive = true;
            newDataSource.Name = txtApplicationName.Text;
            newDataSource.Description = FromDataSource.Description;
            newDataSource.IsSystem = FromDataSource.IsSystem;
            newDataSource.DataFeederType = FromDataSource.DataFeederType;
            newDataSource.DataFormatType = FromDataSource.DataFormatType;
            newDataSource.Delimiter = FromDataSource.Delimiter;
            newDataSource.SystemDataSourceId = FromDataSource.SystemDataSourceId;
            newDataSource.DataContainerValidatorType = FromDataSource.DataContainerValidatorType;
            newDataSource.OutputType = FromDataSource.OutputType;
            newDataSource.OutputWriterTypeFullName = FromDataSource.OutputWriterTypeFullName;
            newDataSource.PlugInsType = FromDataSource.PlugInsType;
            newDataSource.ProcessingBy = FromDataSource.ProcessingBy;
            newDataSource.PusherType = FromDataSource.PusherType;
            newDataSource.PusherTypeFullName = FromDataSource.PusherTypeFullName;
            newDataSource.Id = DataManager.Save(newDataSource);
            NewDataSourceId = newDataSource.Id;

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Copying attributes...";
            Application.DoEvents();

            List<IdpeAttributeDataSource> sadss = new List<IdpeAttributeDataSource>();
            foreach (TreeNode node in treeView.Nodes[0].Nodes)
            {
                if (node.Checked)
                {
                    sadss.Add((IdpeAttributeDataSource)node.Tag);
                    toolStripStatusLabel1.Text = "Copying attribute..." + node.Text;
                    Application.DoEvents();
                }
            }

            DataManager.SaveAssociations(NewDataSourceId, sadss);
            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Copying keys...";
            Application.DoEvents();

            #region Copy Internal Keys
            //we have to copy internal keys anyways...

            List<IdpeKey> keys = DataManager.GetApplicationKeys(FromDataSource.Id, false);
            foreach (IdpeKey key in keys)
            {
                if (((IdpeKeyTypes)key.Type) != IdpeKeyTypes.Custom)
                {
                    DataManager.Save(key, newDataSource.Id);
                    toolStripStatusLabel1.Text = "Copying key..." + key.Name;
                    Application.DoEvents();
                }
            }
            #endregion Copy Internal Keys

            #region Copy External Keys
            foreach (TreeNode node in treeView.Nodes[1].Nodes)
            {
                if (node.Checked)
                {
                    IdpeKey ckey = (IdpeKey)node.Tag;
                    DataManager.Save(ckey, newDataSource.Id);
                    toolStripStatusLabel1.Text = "Copying key..." + ckey.Name;
                    Application.DoEvents();
                }
            }
            #endregion Copy External Keys

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Copying rules...";
            Application.DoEvents();
            string newRuleName = string.Empty;


            foreach (TreeNode node in treeView.Nodes[2].Nodes)
            {
                if (node.Checked)
                {
                    IdpeRule rule = node.Tag as IdpeRule;
                    rule.Name = rule.Name + "_" + newDataSource.Id;
                    rule.Description = rule.Description == null ? string.Empty : rule.Description;
                    rule.Id = 0;
                    int newRuleId = DataManager.Save(rule);

                    IdpeRuleDataSource idpeRuleDataSource = new IdpeRuleDataSource();
                    idpeRuleDataSource.Priority = (int)rule.Priority;
                    idpeRuleDataSource.RuleSetType = (int)rule.RuleSetType;
                    idpeRuleDataSource.IsActive = true;
                    idpeRuleDataSource.RuleId = newRuleId;
                    idpeRuleDataSource.DataSourceId = newDataSource.Id;
                    DataManager.SaveRuleAssociationDuringCopy(idpeRuleDataSource);

                    toolStripStatusLabel1.Text = "Copying rule..." + rule.Name;
                    Application.DoEvents();
                }
            }

            toolStripProgressBar1.Increment(30);
            toolStripStatusLabel1.Text = "Done";
            txtApplicationName.Enabled = false;
            btnOK.Enabled = true;
            Copying = false;
            Application.DoEvents();

            toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
            Application.DoEvents();

        }

        private void frmCopyApplication_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Escape)
                && (Copying == false))
                this.Close();
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }
    }
}





