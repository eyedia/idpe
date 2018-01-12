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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{
    public partial class DeployableKeysWindow : Form
    {
        public DeployableKeysWindow()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Bind();
        }
        public List<IdpeKey> SelectedKeys
        {
            get
            {
                return lvwKeys.Items.Cast<ListViewItem>().Where(item => item.Checked).Select(i => (IdpeKey)i.Tag).ToList();
            }
        }
        private void Bind()
        {
            Manager manager = new Manager();
            List<IdpeKey> keys = manager.GetDeployableKeys();
            foreach(IdpeKey key in keys)
            {               
                ListViewItem item = new ListViewItem(manager.GetApplicationName((int)key.DataSourceId));
                item.Tag = key;
                item.Checked = true;                
                item.SubItems.Add(key.Name);
                item.SubItems.Add(key.Value);
                item.SubItems.Add(key.DataSourceId.ToString());
                lvwKeys.Items.Add(item);
            }
        }       

        private void DeployableKeysWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.Close();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip1.Enabled = lvwKeys.SelectedItems.Count > 0 ? true : false;
        }

        private void setValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdpeKey key = lvwKeys.SelectedItems[0].Tag as IdpeKey;
            if (key != null)
            {
                InputBox iBox = new InputBox(key.Value, "Setting new value of '" + key.Name + "' of '" + lvwKeys.SelectedItems[0].Text + "' datasource."
                    , "Set Key Value", this.Icon);
                if (iBox.ShowDialog() == DialogResult.OK)
                {
                    lvwKeys.SelectedItems[0].SubItems[2].Text = iBox.TheInput;
                    key.Value = iBox.TheInput;
                    new Manager().Save(key);
                }
            }
        }

        private void btnDeploy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


