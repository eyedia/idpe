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
using Eyedia.Core;
using Eyedia.Core.Data;
using System.Reflection;
using Eyedia.IDPE.Services;
using System.IO;

namespace Eyedia.IDPE.Interface
{
    public partial class ListOfValues : Form
    {
        Manager UtilDataManager;
        List<CodeSet> LOVs;

        List<CodeSet> UniqueCodes
        {
            get
            {
                if (LOVs != null)
                    return LOVs.GroupBy(lov => lov.Code).Select(grp => grp.First()).OrderBy(l => l.Code).ToList();
                else
                    return new List<CodeSet>();
            }
        }
        CodeSet SelectedCode { get; set; }
        CodeSet SelectedSet { get; set; }
        int MaxPosition { get; set; }
        int MaxEnumCode { get; set; }

        public ListOfValues()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Init();
        }

        void Init()
        {
            UtilDataManager = new Manager();
            txtCode.Top = lblCode.Top - 7;
            txtCode.Left = lblCode.Left;

            LOVs = CoreDatabaseObjects.Instance.GetCodeSets();
            lvCodes.Items.Clear();
            foreach (CodeSet lov in UniqueCodes)
            {
                ListViewItem item = new ListViewItem(lov.Code);
                item.Tag = lov;
                lvCodes.Items.Add(item);
            }
            if (SelectedCode != null)
            {
                foreach (ListViewItem item in lvCodes.Items)
                {
                    if (item.Text == SelectedCode.Code)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            else if (lvCodes.Items.Count > 0)
            {
                lvCodes.Items[0].Selected = true;
            }
        }

        private void ListOfValues_Resize(object sender, EventArgs e)
        {
            lvCodes.Columns[0].Width = lvCodes.Width - 5;
            lvSets.Columns[4].Width = (lvSets.Width
                - (lvSets.Columns[0].Width + lvSets.Columns[1].Width + lvSets.Columns[2].Width + lvSets.Columns[3].Width)) - 5;
        }

        private void lvCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCodes.SelectedItems.Count > 0)
                SelectedCode = lvCodes.SelectedItems[0].Tag as CodeSet;
            else
                SelectedCode = null;

            BindLOVList();
        }
        

        private void lvSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSets.SelectedItems.Count > 0)
                SelectedSet = lvSets.SelectedItems[0].Tag as CodeSet;
            else
                SelectedSet = null;

            BindOneItem();
        }

        void BindLOVList()
        {
            if (SelectedCode != null)
            {
                List<CodeSet> lovList = (from cs in LOVs
                                             where cs.Code == SelectedCode.Code
                                             orderby cs.Position
                                             select cs).ToList();

                MaxPosition = lovList.Max(l => l.Position).Value;
                MaxEnumCode = lovList.Max(l => l.EnumCode);

                lvSets.Items.Clear();
                foreach (CodeSet l in lovList)
                {
                    ListViewItem item = new ListViewItem(l.EnumCode.ToString());
                    item.SubItems.Add(l.Value);
                    item.SubItems.Add(l.Position.ToString());
                    item.SubItems.Add(l.ReferenceKey);
                    item.SubItems.Add(l.Description);
                    item.Tag = l;
                    lvSets.Items.Add(item);
                }

                if ((SavedJustNow) && (SelectedSet != null))
                {
                    SavedJustNow = false;
                    foreach (ListViewItem item in lvCodes.Items)
                    {
                        if (item.Text == SelectedSet.EnumCode.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
                else if (lvSets.Items.Count > 0)
                    lvSets.Items[0].Selected = true;
            }
        }

        void BindOneItem()
        {
            if (SelectedSet != null)
            {
                lblId.Text = SelectedSet.CodeSetId.ToString();
                lblCode.Text = SelectedSet.Code;
                lblWarningMsg2.Text = SelectedSet.Code;
                txtEnumCode.Text = SelectedSet.EnumCode.ToString();
                txtValue.Text = SelectedSet.Value;
                txtReferenceKey.Text = SelectedSet.ReferenceKey;
                txtDescription.Text = SelectedSet.Description;
                nmPosition.Value = (decimal)SelectedSet.Position;

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Button clickedOn = sender as Button;
            if (clickedOn.Text.Contains("New"))
            {
                clickedOn.Text = "Ca&ncel";
                lvCodes.Enabled = false;
                lvSets.Enabled = false;
                txtValue.Text = "";
                txtDescription.Text = "";
                if (clickedOn.Name == "btnNewCode")
                {
                    lblCode.Visible = false;
                    txtCode.Visible = true;
                    btnNew.Enabled = false;
                    lblId.Text = "NULL";
                    txtCode.Text = "";
                    txtReferenceKey.Text = "";
                    txtEnumCode.Text = "1";                    
                    nmPosition.Value = 1;
                    txtCode.Focus();
                }
                else
                {
                    lblWarningMsg.Visible = true;
                    timerBlink.Enabled = true;
                    btnNewCode.Enabled = false;
                    txtEnumCode.Text = (MaxEnumCode + 1).ToString();
                    nmPosition.Value = MaxPosition + 1;
                    txtValue.Focus();
                }                
            }
            else
            {
                if (clickedOn.Name != "btnSave")
                {
                    if (clickedOn.Name == "btnNewCode")
                        clickedOn.Text = "&New Code";
                    else
                        clickedOn.Text = "&New";
                }

                if (btnNewCode.Text == "Ca&ncel")
                    btnNewCode.Text = "&New Code";

                if (btnNew.Text == "Ca&ncel")
                    btnNew.Text = "&New";

                lblCode.Visible = true;
                txtCode.Visible = false;
                lvCodes.Enabled = true;
                lvSets.Enabled = true;
                btnNewCode.Enabled = true;
                btnNew.Enabled = true;                
                timerBlink.Enabled = false;
                lblWarningMsg.Visible = false;
                lblWarningMsg2.Visible = false;
                BindOneItem();
            }
        }
        bool SavedJustNow;
        private void btnSave_Click(object sender, EventArgs e)
        {
            CodeSet codeSet = new CodeSet();

            codeSet.CodeSetId = lblId.Text == "NULL" ? 0 : int.Parse(lblId.Text);
            codeSet.Code = txtCode.Visible ? txtCode.Text : lblCode.Text;
            codeSet.EnumCode = int.Parse(txtEnumCode.Text);
            codeSet.Value = txtValue.Text;
            codeSet.ReferenceKey = txtReferenceKey.Text;
            codeSet.Description = txtDescription.Text;
            codeSet.Position = (int)nmPosition.Value;
            CoreDatabaseObjects.Instance.Save(codeSet);
            Cache.Instance.Bag.Remove("codesets.default");            
            SavedJustNow = true;
            SelectedSet = codeSet;
            btnNew_Click(btnSave, e);
            Init();
        }

        private void timerBlink_Tick(object sender, EventArgs e)
        {            
            lblWarningMsg2.Visible = !lblWarningMsg2.Visible;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool itemSelected = lvCodes.SelectedItems.Count > 0 ? true : false;            
            deleteToolStripMenuItem.Enabled = itemSelected;
            exportToolStripMenuItem.Enabled = itemSelected;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvCodes.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Do you want to permanently delete code '" + lvCodes.SelectedItems[0].Text + "'?", "Delete CodeSet",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    CoreDatabaseObjects.Instance.Delete(lvCodes.SelectedItems[0].Text);
                    lvSets.Items.Clear();
                    Init();                    
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvCodes.SelectedItems.Count > 0)
            {
                saveFileDialog.FileName = lvCodes.SelectedItems[0].Text;
                saveFileDialog.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataSourcePatch dsp = new DataSourcePatch();
                    foreach (ListViewItem item in lvSets.Items)
                    {
                        CodeSet codeSet = (CodeSet)item.Tag;
                        codeSet.CodeSetId = 0;
                        dsp.CodeSets.Add(codeSet);
                    }
                    dsp.Export(saveFileDialog.FileName);
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Delimited Files (*.csv)|*.csv|SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog.FileName).ToLower() == ".srep")
                {
                    DataSourcePatch dsp = new DataSourcePatch(openFileDialog.FileName);
                    dsp.Import();                    
                }
                else
                {
                    ImportCurrency(openFileDialog.FileName);
                }

                Init();
                BindLOVList();
            }
        }

        private void ListOfValues_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                deleteToolStripMenuItem_Click(sender, e);
        }

        private void exportImportFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Delimited Files (*.csv)|*.csv|All Files (*.*)|*.*";
            saveFileDialog.FileName = "lov.csv";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.WriteLine("Code,EnumCode,Value,Description");
                sw.WriteLine("Currency,20151,USD,US Dollar");
                sw.WriteLine("Currency,20067,INR,Indian Rupee");
                sw.WriteLine("Sex,101,Male,Male");
                sw.Close();
            }
        }

        private void ImportCurrency(string fileName)
        {
            DataTable table = CsvToDataTable.ReadFile(fileName);

            int position = 1;
            foreach (DataRow row in table.Rows)
            {
                CodeSet codeSet = new CodeSet();

                codeSet.Code = row["Code"].ToString();
                codeSet.EnumCode = (int)row["EnumCode"].ToString().ParseInt();
                codeSet.Value = row["Value"].ToString();
                codeSet.Description = row["Description"].ToString();
                codeSet.ReferenceKey = string.Empty;
                codeSet.Position = position;
                CoreDatabaseObjects.Instance.Save(codeSet);
                position++;
            }
        }

        private void lvCodes_Resize(object sender, EventArgs e)
        {
            lvCodes.Columns[0].Width = lvCodes.Width - 20;
        }
    }
}





