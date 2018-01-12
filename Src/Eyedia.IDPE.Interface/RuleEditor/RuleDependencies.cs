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
using Eyedia.IDPE.Common;
using System.Reflection;

namespace Eyedia.IDPE.Interface
{
    public partial class RuleDependencies : Form
    {        
        public int RuleId { get; private set; }

        public RuleDependencies(int ruleId)
        {
            InitializeComponent();
            RuleId = ruleId;
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);  
        }

        private void RuleDependencies_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            Manager manager = new Manager();
            DataTable table = manager.GetRuleDependencies(RuleId);
            this.Text = "Rule Dependencies - " + manager.GetRule(RuleId).Name;

            lvwDataSources.Items.Clear();
            foreach (DataRow row in table.Rows)
            {
                string dataSourceName = row[1].ToString();
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    if (!(dataSourceName.ToLower().Contains(txtSearch.Text.ToLower())))
                        continue;
                }

                ListViewItem item = new ListViewItem(dataSourceName);
                RuleSetTypes ruleSetType = (RuleSetTypes)int.Parse(row[2].ToString());

                item.SubItems.Add(ruleSetType.ToString());
                item.Tag = row[0].ToString();
                lvwDataSources.Items.Add(item);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {           
            this.Close();
        }

        private void RuleDependencies_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        

    }
}


