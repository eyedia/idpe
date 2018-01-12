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
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Services;
using System.IO;
using Eyedia.IDPE.Common;
using System.Diagnostics;

namespace Eyedia.IDPE.Interface
{
    public partial class DockPanelGlobalAttributes : ToolWindow
    {
        public DockPanelGlobalAttributes()
        {
            InitializeComponent();

            sreListView1.ListView.KeyUp += new KeyEventHandler(ListView_KeyUp);
            sreListView1.ShowPosition = false;
            RefreshData();
        }

        void ListView_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete)
                && (sreListView1.ListView.SelectedItems.Count > 0))
            {
                if (MessageBox.Show("All selected attributes will be deleted if not in use! Are you sure?", "Delete Attributes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Manager manager = new Manager();
                    int total = sreListView1.ListView.SelectedItems.Count;
                    int deleted = 0;
                    foreach (ListViewItem item in sreListView1.ListView.SelectedItems)
                    {
                        SreAttribute attrib = item.Tag as SreAttribute;
                        if (manager.IsAttributeInUse(attrib.AttributeId) == false)
                        {
                            manager.DeleteAttribute(attrib.AttributeId);
                            deleted++;
                        }
                    }
                    MessageBox.Show(string.Format("Total deleted:{0}, Ignored:{1}", deleted, total - deleted), "Delete Attributes",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshData();
                }
            }
        }
     
        private void DockPanelGlobalAttributes_Resize(object sender, EventArgs e)
        {
            pnlRight.Width = (int)((this.Width / 10) * 2);
            sreListView1.Width = (int)((this.Width / 10) * 8);
        }

        public SreDataSource SelectedDataSource { get; set; }

        public DockPanelDataSourceAttributes DockPanelDataSourceAttributes
        {
            get
            {
                foreach (var c in this.DockPanel.Contents)
                {
                    if (c is DockPanelDataSourceAttributes) return c as DockPanelDataSourceAttributes;
                }
                return null;
            }
        }

        public DockPanelDataSourceSystemAttributes DockPanelDataSourceSystemAttributes
        {
            get
            {
                foreach (var c in this.DockPanel.Contents)
                {
                    if (c is DockPanelDataSourceSystemAttributes) return c as DockPanelDataSourceSystemAttributes;
                }
                return null;
            }
        }

        public void RefreshData(int selectedAttributeId = 0)
        {
            this.Cursor = Cursors.WaitCursor;
            sreListView1.Attributes = new Manager().GetAttributes();

            if (selectedAttributeId > 0)
            {
                foreach (ListViewItem item in sreListView1.ListView.Items)
                {
                    if (((SreAttribute)item.Tag).AttributeId == selectedAttributeId)
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        break;
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }

        #region Associate attributes


        private void btnAssociateBoth_Click(object sender, EventArgs e)
        {
            AddToDataSourceButtonClick(sender, e);
            AddToSystemDataSourceButtonClick(sender, e);
        }

        void AddToDataSourceButtonClick(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in sreListView1.ListView.SelectedItems)
            {
                AddAttributeToListView(selectedItem, DockPanelDataSourceAttributes.sreListView1.ListView);
                ((Control)sender).Enabled = false;

            }
            SaveAssociations(DockPanelDataSourceAttributes.sreListView1.ListView, false);
        }

        void AddToSystemDataSourceButtonClick(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in sreListView1.ListView.SelectedItems)
            {
                AddAttributeToListView(selectedItem, DockPanelDataSourceSystemAttributes.sreListView1.ListView);
                ((Control)sender).Enabled = false;
            }
            SaveAssociations(DockPanelDataSourceSystemAttributes.sreListView1.ListView, true);
        }

        void SaveAssociations(ListView listView, bool isSystem)
        {
            Manager manager = new Manager();
            SreDataSourceProperty.KeepVersion(isSystem ? (int)SelectedDataSource.SystemDataSourceId: SelectedDataSource.Id);

            int position = 1;
            foreach (ListViewItem item in listView.Items)
            {
                SreAttribute attribute = item.Tag as SreAttribute;
                SreAttributeDataSource sds = new SreAttributeDataSource();
                sds.DataSourceId = isSystem ? (int)SelectedDataSource.SystemDataSourceId : SelectedDataSource.Id;
                sds.AttributeId = attribute.AttributeId;
                if (isSystem == false)
                    sds.IsAcceptable = true;
                else
                    sds.IsAcceptable = (bool)attribute.IsAcceptable;

                sds.AttributePrintValueType = attribute.AttributePrintValueType;
                sds.AttributePrintValueCustom = attribute.AttributePrintValueCustom;
                sds.Position = position;
                manager.Save(sds);
                position++;
            }
            if (!isSystem)
            {
                DockPanelDataSourceAttributes.DataSourceId = SelectedDataSource.Id;
            }
            else
            {
                DockPanelDataSourceSystemAttributes.SystemDataSourceId = (int)SelectedDataSource.SystemDataSourceId;
            }
        }

        void AddAttributeToListView(ListViewItem selectedItem, ListView listView)
        {
            SreAttribute attribute = selectedItem.Tag as SreAttribute;
            ListViewItem item = new ListViewItem(attribute.Name);
            item.SubItems.Add(attribute.Type);
            item.ImageKey = attribute.Type.ToString() + ".gif";
            item.Tag = attribute;
            if (listView.SelectedIndices.Count > 0)
            {
                item.SubItems.Add(listView.SelectedIndices[0].ToString());
                listView.Items.Insert(listView.SelectedIndices[0], item);
            }
            else
            {
                item.SubItems.Add((listView.Items.Count + 1).ToString());
                listView.Items.Add(item);
            }
        }

        #endregion

        private void DockPanelGlobalAttributes_KeyUp(object sender, KeyEventArgs e)
        {
            bool s= DockPanelDataSourceSystemAttributes.Visible;
            switch (e.KeyCode)
            {
                case Keys.Oemcomma:
                    
                    break;

                case Keys.OemPeriod:
                    //btnAssociateAttributeSystemDataSource(sender, e);
                    break;

            }
        }

        private void miExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "attributes.csv";
            saveFileDialog1.Filter = "Comma Delimited Files (*.csv)|*.csv|All Files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                sw.WriteLine("\"Name\",\"Type\"");

                foreach (ListViewItem item in sreListView1.ListView.Items)
                {
                    SreAttribute attrib = item.Tag as SreAttribute;
                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"", attrib.Name, attrib.Type));
                }
                sw.Close();
            }
        }

        private void miImport_Click(object sender, EventArgs e)
        {
        
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Comma Delimited Files (*.csv)|*.csv|All Files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<SreAttribute> attributes = new List<SreAttribute>();
                string result = ImportAttributes(ReadData(openFileDialog1.FileName), ref attributes);
                MessageBox.Show(result, "Bulk Insert", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
        }

      
        public string ImportAttributes(DataTable table, ref List<SreAttribute> attributes)
        {
            string result = "Total records found = " + table.Rows.Count + Environment.NewLine;
            Manager manager = new Manager();
            int counter = 0;
            attributes = new List<SreAttribute>();
            foreach (DataRow row in table.Rows)
            {
                SreAttribute attrib = new SreAttribute();
                attrib.Name = row[0].ToString();
                attrib.Type = row[1].ToString();
                attrib.Position = counter + 1;
                attrib.IsAcceptable = true;

                string existingDataType = CheckAttributeDataChange(manager, attrib);
                if (!string.IsNullOrEmpty(existingDataType))
                {
                    DialogResult dr = MessageBox.Show(string.Format("You are changing attribute '{0}' data type from '{1}' to '{2}'. This will impact all other data sources where attribute '{0}' is associcated. Are you sure? Press 'No' to skip this attribute and continue, 'Yes' to override, 'Cancel' to abort the operation"
                        , attrib.Name, existingDataType, attrib.Type),
                        "Data Type Change",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (dr == System.Windows.Forms.DialogResult.No)
                        attrib.Type = existingDataType;
                    else if (dr == System.Windows.Forms.DialogResult.Cancel)
                        return "Import operation was aborted!";

                }

                attributes.Add(attrib);
                try
                {
                    manager.Save(attrib);
                    counter++;
                }
                catch { }

            }

            result += "Total inserted = " + counter + Environment.NewLine;
            result += "Total failed = " + (table.Rows.Count - counter) + Environment.NewLine;

            RefreshData();
            return result;

        }

        private string CheckAttributeDataChange(Manager manager, SreAttribute attrib)
        {
            SreAttribute existingAttribute = manager.GetAttribute(attrib.Name);
            if ((existingAttribute != null)
                && (existingAttribute.Type.ToLower() != attrib.Type.ToLower()))
                return existingAttribute.Type;

            return string.Empty;
        }

        
        public DataTable ReadData(string fileName)
        {
            return CsvToDataTable.ReadFile(fileName);
            
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}


