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
using Eyedia.IDPE.Common;
using Eyedia.IDPE.Services;
using Eyedia.IDPE.DataManager;


namespace Eyedia.IDPE.Interface
{
    public partial class DeploymentArtifacts : Form
    {
        public DeploymentArtifacts()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Bind();
        }

        private void Bind()
        {
            AddToListView(DeploymentService.RequiredFiles.ToArray());
            IdpeKey key = new Manager().GetKey(IdpeKeyTypes.AdditionalDeploymentArtifacts);
            if (key != null)            
                AddToListView(key.GetUnzippedBinaryValue().Split(",".ToCharArray()), true);
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int counter = lvwFiles.Items.Count + 1;
                foreach (string file in openFileDialog1.FileNames)
                {
                    if (!FileAlreadyExist(file))
                    {
                        ListViewItem item = new ListViewItem(counter.ToString());
                        item.Checked = true;
                        item.SubItems.Add(file);
                        item.SubItems.Add("Plugins/Extensions");
                        item.ForeColor = Color.DarkBlue;
                        lvwFiles.Items.Add(item);
                        counter++;
                    }
                }
            }
        }

        private bool FileAlreadyExist(string file)
        {
            foreach(ListViewItem item in lvwFiles.Items)
            {
                if (item.SubItems[1].Text.Contains(file))
                    return true;
            }
            return false;
        }

        private void DeploymentArtifacts_FormClosed(object sender, FormClosedEventArgs e)
        {
            string plugins = GetPlugins();
            if (!string.IsNullOrEmpty(plugins))
            {
                IdpeKey key = new IdpeKey();
                key.Type = (int)IdpeKeyTypes.AdditionalDeploymentArtifacts;
                key.Name = IdpeKeyTypes.AdditionalDeploymentArtifacts.ToString();
                key.SetZippedBinaryValue(plugins);
                new Manager().Save(key);
            }
            else
            {
                new Manager().DeleteKey(IdpeKeyTypes.AdditionalDeploymentArtifacts.ToString());
            }
        }
        private void AddToListView(string[] files, bool isPlugins = false)
        {
            int counter = lvwFiles.Items.Count + 1;
            foreach (string file in files)
            {
                ListViewItem item = new ListViewItem(counter.ToString());
                item.Checked = true;
                item.SubItems.Add(file);
                item.SubItems.Add(isPlugins == false ? "Eydia" : "Plugins/Extensions");
                if (isPlugins)
                    item.ForeColor = Color.DarkBlue;
                lvwFiles.Items.Add(item);
                counter++;
            }
        }

        private string GetPlugins()
        {
            string plugins = string.Empty;
            foreach (ListViewItem item in lvwFiles.Items)
            {
                if (item.SubItems[2].Text == "Plugins/Extensions")
                {
                    plugins += item.SubItems[1].Text + ",";
                }
            }
            if (plugins.Length > 0)
                plugins = plugins.Substring(0, plugins.Length - 1);
            return plugins;
        }

        public List<string> SelectedFiles
        {
            get
            {
                return lvwFiles.Items.Cast<ListViewItem>().Where(item => item.Checked).Select(i => i.SubItems[1].Text).ToList();                
            }
        }

        private void lvwFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                List<ListViewItem> toBeRemovedItems = new List<ListViewItem>();
                foreach (ListViewItem item in lvwFiles.SelectedItems)
                {
                    toBeRemovedItems.Add(item);
                }

                foreach(ListViewItem item in toBeRemovedItems)
                {
                    lvwFiles.Items.Remove(item);
                }                
            }
        }
    }
}


