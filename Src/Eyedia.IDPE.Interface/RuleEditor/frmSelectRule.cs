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
using Eyedia.IDPE.Interface.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Reflection;

namespace Eyedia.IDPE.Interface
{
    public partial class frmSelectRule : Form
    {
        public List<string> DoNotIncludeList { get; private set; }

        public frmSelectRule(List<string> doNotIncludeList)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            if (doNotIncludeList != null)
            {
                this.DoNotIncludeList = doNotIncludeList;
                RefreshRules();
                lstBoxRuleSet.DisplayMember = "Name";
            }
        }

        private void RefreshRules(string keyWords = null)
        {
            List<IdpeRule> rules = new Manager().GetRules();

            #region Excluding DONOTINCLUDE
            List<int> ruleIdToBeExcluded = new List<int>();

            for (int i = 0; i < rules.Count; i++)
            {
                foreach (string donotInclude in DoNotIncludeList)
                {
                    if (donotInclude == rules[i].Name)
                    {
                        ruleIdToBeExcluded.Add(rules[i].Id);
                        break;
                    }
                }
            }
            foreach (int ruleId in ruleIdToBeExcluded)
            {
                rules.RemoveAll(r => r.Id == ruleId);
            }
            #endregion Excluding DONOTINCLUDE

            rules.Where(r => r.Description == null).ToList().ForEach(r1 => r1.Description = "");
            if (keyWords != null)
                rules = rules.Where(r => (r.Name.ToLower().Contains(keyWords.ToLower())) || (r.Description.ToLower().Contains(keyWords.ToLower()))).ToList();

            lstBoxRuleSet.DataSource = rules;
        }

        private void lstBoxRuleSet_DoubleClick(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
            this.DialogResult = DialogResult.OK;
        }

        public IdpeRule SelectedRule;
       
        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedRule = (IdpeRule)lstBoxRuleSet.SelectedItem;
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshRules(txtSearch.Text);
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                btnOK_Click(sender, e);
                this.DialogResult = DialogResult.OK;
            }
        }
      
        
    }
}





