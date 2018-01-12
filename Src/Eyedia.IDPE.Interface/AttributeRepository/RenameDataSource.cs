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
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using System.Text.RegularExpressions;


namespace Eyedia.IDPE.Interface
{
    public partial class RenameDataSource : Form
    {
        internal IdpeDataSource DataSource;
        public RenameDataSource(IdpeDataSource dataSource)
        {
            InitializeComponent();
            ;
            DataSource = dataSource;
            this.Text = "Rename DataSource - " + dataSource.Name;
            pictureBox1.Image = System.Drawing.SystemIcons.Warning.ToBitmap();
        }

        private void RenameDataSource_Load(object sender, EventArgs e)
        {
            BindData();
        }
       
        private void RenameDataSource_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void lvw_Resize(object sender, EventArgs e)
        {
            lvw.Columns[1].Width = lvw.Width - (lvw.Columns[0].Width + lvw.Columns[2].Width) - 5;
        }

        #region Private Methods

        private void BindData()
        {
            ListViewItem item = new ListViewItem("Name");
            item.SubItems.Add(DataSource.Name);
            lvw.Items.Add(item);

            Manager manager = new Manager();
            List<IdpeRule> rules = manager.GetRules();
            string[] words = DataSource.Name.Split(" ".ToCharArray());
            for(int i=0;i<words.Length;i++)
            {
                if (i == 0)
                    rules = rules.Where(r => Regex.IsMatch(r.Name, words[i], RegexOptions.IgnoreCase)).ToList();
                else
                    rules.AddRange(rules.Where(r => Regex.IsMatch(r.Name, words[i], RegexOptions.IgnoreCase)).ToList());
            }
            rules = rules.Distinct().ToList();
            
            foreach (IdpeRule rule in rules)
            {
                int usedBy = manager.GetRuleDependencies(rule.Id).Rows.Count;
                item = new ListViewItem("Rule Name");
                item.SubItems.Add(rule.Name);
                item.SubItems.Add("Used by " + usedBy.ToString());
                if (usedBy > 0)
                    item.ForeColor = Color.Gray;
                lvw.Items.Add(item);
            }
        }

        #endregion Private Methods
    }
}


