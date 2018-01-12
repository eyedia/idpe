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
using System.IO;
using Eyedia.IDPE.Common;
using s = Eyedia.IDPE.Services;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{
    public partial class GlobalEventsOnComplete : Form
    {
        public GlobalEventsOnComplete()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Bind();
        }

    
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "&Add")
            {
                txtName.Text = "new global event";
                txtWatchingOn.Text = string.Empty;
                nudTimeOut.Value = 5;
                txtCommands.Text = string.Empty;
                ListViewItem item = new ListViewItem(txtName.Text);
                s.GlobalEventsOnComplete newgec = new s.GlobalEventsOnComplete(new List<int>(), txtName.Text, txtCommands.Text, 5);
                item.Tag = newgec;
                listView.Items.Add(item);
                item.Selected = true;
                txtName.Focus();
            }
            else
            {
                btnAdd.Text = "&Add";
                Bind();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ParseAndSave();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                listView.Items.Remove(listView.SelectedItems[0]);
                ParseAndSave();
            }
        }

        #region Helpers

        private void txtBoxKeyup(object sender, KeyEventArgs e)
        {
            if ((e.Control) && (e.KeyCode == Keys.A))
                ((TextBox)sender).SelectAll();
        }

        private void btnCommandsHelp_Click(object sender, EventArgs e)
        {
            string sreDosHelpCommands = string.Empty;

            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Interface.EmbeddedResources.SreDosCommandsHelp.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                sreDosHelpCommands = reader.ReadToEnd();
            }

            TextArea textAreaHintContent1 = new TextArea("SRE DOS Commands Help");
            textAreaHintContent1.Size = new System.Drawing.Size(691, 363);
            textAreaHintContent1.toolStripStatusLabel1.Text = "";
            textAreaHintContent1.txtContent.Text = sreDosHelpCommands;
            textAreaHintContent1.ShowDialog();
        }

        private void listView_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Control) && (e.KeyCode == Keys.A))
            {
                foreach (ListViewItem item in listView.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void Bind()
        {
            IdpeKey key = new Manager().GetKey(SreKeyTypes.GlobalEventsOnComplete);
            if (key != null)
            {
                s.GlobalEventsOnCompletes gecs = new s.GlobalEventsOnCompletes(key.Value);

                listView.Columns.Clear();
                listView.Columns.Add("Events");
                listView.Columns[0].Width = listView.Width - 10;
                listView.Items.Clear();
                foreach (s.GlobalEventsOnComplete gec in gecs)
                {
                    ListViewItem item = new ListViewItem(gec.Name);
                    item.Tag = gec;
                    listView.Items.Add(item);
                }
            }

            if (listView.Items.Count > 0)
                BindOne(listView.Items[0].Tag);

        }

        private void BindOne(object objTag)
        {
            s.GlobalEventsOnComplete item = objTag as s.GlobalEventsOnComplete;
            if (item != null)
            {
                txtName.Text = item.Name;
                txtWatchingOn.Text = item.CommaSeparatedDataSourceIds;
                nudTimeOut.Value = item.TimeOutInMinutes == 0 ? 5 : item.TimeOutInMinutes;
                txtCommands.Text = item.DosCommands;
            }
        }

        private void Save(string rawString)
        {
            IdpeKey key = new IdpeKey();
            key.Name = SreKeyTypes.GlobalEventsOnComplete.ToString();
            key.Type = (int)SreKeyTypes.GlobalEventsOnComplete;
            key.Value = rawString;
            new Manager().Save(key);
        }

        private void ParseAndSave()
        {
            this.Cursor = Cursors.WaitCursor;
            string rawString = string.Empty;

            foreach (ListViewItem item in listView.Items)
            {
                rawString += ((s.GlobalEventsOnComplete)(item.Tag)).RawString + "°";
            }

            if (rawString.Length > 0)
                rawString = rawString.Substring(0, rawString.Length - 1);

            if (!ValidateData())
            {
                this.Cursor = Cursors.Default;
                return;
            }
            Save(rawString);
            Bind();
            this.Cursor = Cursors.Default;
        }

        private bool ValidateData()
        {
            if(string.IsNullOrEmpty(txtWatchingOn.Text))
            {
                return false;
            }          
            else if (!CommaToList(txtWatchingOn.Text))
            {
                MessageBox.Show("Comma separated data source ids are incorrect!", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool CommaToList(string strCommaSeparated)
        {
            try
            {
                strCommaSeparated.Split(',').Select(int.Parse).ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)          
                BindOne(listView.SelectedItems[0].Tag);            
        }

        private void txtWatchingOn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) 
                && !char.IsDigit(e.KeyChar) 
                && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
    
        }

        private void GlobalEventsOnComplete_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
      
        private void NameChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                ((s.GlobalEventsOnComplete)listView.SelectedItems[0].Tag).Name = txtName.Text;
        }

        private void WatchingOnChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                ((s.GlobalEventsOnComplete)listView.SelectedItems[0].Tag).CommaSeparatedDataSourceIds = txtWatchingOn.Text;
        }

        private void TimeOutChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                ((s.GlobalEventsOnComplete)listView.SelectedItems[0].Tag).TimeOutInMinutes = (int)nudTimeOut.Value;
        }

        private void CommandChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                ((s.GlobalEventsOnComplete)listView.SelectedItems[0].Tag).DosCommands = txtCommands.Text;
        }

        #endregion Helpers

    }
}


