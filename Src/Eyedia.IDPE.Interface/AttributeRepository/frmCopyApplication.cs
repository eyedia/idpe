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
using Symplus.RuleEngine.DataManager;
using Symplus.RuleEngine.Utilities.Data;

namespace Symplus.RuleEngine.Utilities
{
    public partial class frmCopyApplication : Form
    {
        bool _Copying;
        SreDataSource _FromApplication;
        UtilDataManager _UtilDataManager;
        Manager _AttributeManager;
        public frmCopyApplication(int fromApplicationId)
        {
            InitializeComponent();
            _UtilDataManager = new UtilDataManager();
            _AttributeManager = new Manager();
            _FromApplication = _AttributeManager.GetApplicationDetails(fromApplicationId);
            lblFrom.Text = _FromApplication.Name;
        }

        private void frmCopyApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Copying)
                e.Cancel = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                CopyApplication();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
                _Copying = false;
            }
            this.Close();
        }

        private void txtApplicationName_TextChanged(object sender, EventArgs e)
        {
            if (txtApplicationName.Text.Length > 0)
            {
                if (_AttributeManager.ApplicationExists(txtApplicationName.Text))
                {
                    toolStripStatusLabel1.Text = "Application name exists";
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
                toolStripStatusLabel1.Text = "Enter a valid application name";
                btnOK.Enabled = false;
            }
        }

        private void txtApplicationName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (btnOK.Enabled))
                btnOK_Click(null, null);
        }

        int _NewApplicationId;
        public int NewApplicationId
        {
            get { return _NewApplicationId; }
            set { _NewApplicationId = value; }
        }

        private void CopyApplication()
        {
            _Copying = true;
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Increment(10);
            toolStripStatusLabel1.Text = "Copying application...";
            Application.DoEvents();
            SreDataSource newApp = new SreDataSource();
            newApp.Name = txtApplicationName.Text;
            newApp.Description = txtApplicationName.Text;
            newApp.IsSystem = _FromApplication.IsSystem;
            newApp.DataFeederType = _FromApplication.DataFeederType;
            newApp.DataFormatType = _FromApplication.DataFormatType;
            newApp.Delimiter = _FromApplication.Delimiter;
            newApp.SystemDataSourceId = _FromApplication.SystemDataSourceId;
            newApp.DataContainerValidatorType = _FromApplication.DataContainerValidatorType;
            newApp.OutputWriterType = _FromApplication.OutputWriterType;
            newApp.PlugInsType = _FromApplication.PlugInsType;
            newApp.ProcessingBy = _FromApplication.ProcessingBy;
            newApp.PusherType = _FromApplication.PusherType;
            List<SreRuleDataSource> ruleSets = new List<SreRuleDataSource>();
            newApp.Id = _AttributeManager.Save(newApp, ruleSets);
            NewApplicationId = newApp.Id;

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Copying attributes...";
            Application.DoEvents();

            BindingList<Symplus.RuleEngine.Utilities.Data.Attribute> attributes = _UtilDataManager.GetAttributes(_FromApplication.Id);
            List<object> esAttributes = new List<object>();
            foreach (Symplus.RuleEngine.Utilities.Data.Attribute a in attributes)
            {
                a.AttributeExternalSystemId = 0;
                esAttributes.Add(a);
            }
            _UtilDataManager.SaveAssociations(newApp.Id, esAttributes);

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Copying keys...";
            Application.DoEvents();

            List<SreKey> fromAppKeys = _AttributeManager.GetApplicationKeys(_FromApplication.Id, false);

            if (fromAppKeys != null)
            {
                for (int i = 0; i < fromAppKeys.Count; i++)
                {
                    SreKey newKey = new SreKey();

                    newKey.Name = fromAppKeys[i].Name;
                    newKey.Value = fromAppKeys[i].Value;
                    newKey.Type = fromAppKeys[i].Type;
                    newKey.IsDefault = fromAppKeys[i].IsDefault;

                    _AttributeManager.Save(newApp.Id, newKey);
                }

            }

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Copying rules...";
            Application.DoEvents();
            string newRuleName = string.Empty;
            int newRuleId = 0;
            List<SreRule> fromRuleSets = _AttributeManager.GetRules(_FromApplication.Id);
            foreach (SreRule sr in fromRuleSets)
            {
                newRuleName = (sr.Name).Replace(_FromApplication.Name, "");
                newRuleName = newRuleName + "_" + newApp.Name;
                sr.Id = 0;
                newRuleId = 0;
                newRuleId = _AttributeManager.Save(sr);
                SreRuleDataSource ruleSetApp = new SreRuleDataSource();
                ruleSetApp.DataSourceId = 0;
                ruleSetApp.RuleId = newRuleId;
                ruleSetApp.DataSourceId = newApp.Id;
                ruleSetApp.IsActive = true;
                _AttributeManager.Save(null, null, null, ruleSetApp);
            }
            //for (int i = 0; i < fromRuleSets.Count; i++)
            //{
               

            //    fromRuleSets[i].Id = 0;
            //    if (fromRuleSets[i].Name.IndexOf("precontainer", StringComparison.OrdinalIgnoreCase) >= 0)
            //        fromRuleSets[i].Name = newApp.Name + "PreContainer";
            //    else if (fromRuleSets[i].Name.IndexOf("preparse", StringComparison.OrdinalIgnoreCase) >= 0)
            //        fromRuleSets[i].Name = newApp.Name + "PreParse";
            //    else if (fromRuleSets[i].Name.IndexOf("postparse", StringComparison.OrdinalIgnoreCase) >= 0)
            //        fromRuleSets[i].Name = newApp.Name + "PostParse";
            //    else if (fromRuleSets[i].Name.IndexOf("postcontainer", StringComparison.OrdinalIgnoreCase) >= 0)
            //        fromRuleSets[i].Name = newApp.Name + "PreContainer";
            //    else
            //    {
            //        newRuleName = (fromRuleSets[i].Name).Replace(_FromApplication.Name, "");

            //        fromRuleSets[i].Name = newRuleName.Replace("_","") + "_" + newApp.Name;
            //    }
            //}
            //_AttributeManager.Save(fromRuleSets);

            //ruleSets = _AttributeManager.GetRuleSetApplication(_FromApplication.Id);
            //foreach (SreRuleDataSource ruleSet in ruleSets)
            //{
            //    SreRuleDataSource ruleSetApp = new SreRuleDataSource();
            //    ruleSetApp.DataSourceId = 0;
            //    ruleSetApp.RuleId = 0;
            //    ruleSetApp.DataSourceId = newApp.Id;
            //    //ruleSetApp.RuleName = ruleSet.RulesetName + "_" + newApp.Id;
            //    //ruleSetApp.RulesetType = ruleSet.RulesetType;
            //    _AttributeManager.Save(null, null, null, ruleSetApp);
            //}

            toolStripProgressBar1.Increment(30);
            toolStripStatusLabel1.Text = "Done";
            txtApplicationName.Enabled = false;
            btnOK.Enabled = false;
            _Copying = false;
            Application.DoEvents();

            toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
            Application.DoEvents();

        }

        private void frmCopyApplication_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Escape)
                && (_Copying == false))
                this.Close();
        }
    }
}





