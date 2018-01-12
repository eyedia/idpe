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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eyedia.IDPE.DataManager;
using System.Collections;
using System.Diagnostics;

namespace Eyedia.IDPE.Interface.Controls
{
    public partial class SreListView : UserControl
    {
        public SreListView()
        {
            InitializeComponent();
            ListView.AllowDrop = chkDrag.Checked;
            Debug.AutoFlush = true;
        }

        #region Events

        public event EventHandler ItemSelectedIndexChanged;
        public event EventHandler AddButtonClick;
        public event EventHandler SaveButtonClick;
        public event EventHandler PrintButtonClick;
        public event EventHandler Repositioned;
        public event EventHandler DeleteKeyPressed;
        
        #endregion Events

        #region Properties
        public bool ShowAddButton
        {
            get { return btnAdd.Visible; }
            set
            {
                btnAdd.Visible = value;
                //pnlBottom.Visible = ((btnAdd.Visible == false) && (btnSave.Visible == false) && (btnPrint.Visible == false)) ? false : true;                
                SetButtonPositions();
            }
        }
        
        public bool ShowSaveButton
        {
            get { return btnSave.Visible; }
            set
            {
                btnSave.Visible = value;
                //pnlBottom.Visible = ((btnAdd.Visible == false) && (btnSave.Visible == false) && (btnPrint.Visible == false)) ? false : true;
                SetButtonPositions();
            }
        }


        public bool ShowPrintButton
        {
            get { return btnPrint.Visible; }
            set
            {
                btnPrint.Visible = value;
                //pnlBottom.Visible = ((btnAdd.Visible == false) && (btnSave.Visible == false) && (btnPrint.Visible == false)) ? false : true;
                SetButtonPositions();
            }
        }
       
        private bool _ShowPosition;
        [DefaultValue(false)]
        public bool ShowPosition
        {
            get { return _ShowPosition; }
            set
            {
                _ShowPosition = value;
                this.listView.AllowDrop = _ShowPosition;                
            }
        }

        public bool ShowRepositionCheckBox
        {
            get { return chkDrag.Visible; }
            set { chkDrag.Visible = value; }
        }

        public bool FormatIsPrintable { get; set; }        

        private List<IdpeAttribute> _attributes;
        public List<IdpeAttribute> Attributes 
        {
            get { return _attributes; }
            set
            {
                _attributes = value;    
                if(_attributes != null)            
                    Bind();
            }
        }

        //int BindType;
        private List<IdpeDataSource> _dataSources;
        public List<IdpeDataSource> DataSources
        {
            get { return _dataSources; }
            set
            {
                _dataSources = value; 
                if(_dataSources != null)              
                    Bind(1);               
            }
        }

        private List<IdpeRule> _rules;
        public List<IdpeRule> Rules
        {
            get { return _rules; }
            set
            {
                _rules = value;
                if(_rules != null)
                    Bind(2);
            }
        }

        public bool ShowingDataSource
        {
            get
            {
                return DataSources != null ? true : false;
            }
        }

        public ListView ListView
        {
            get { return this.listView; }
        }

        public int DefaultItemId { get; set; }

        public ListViewItem DefaultItem { get; set; }

        #endregion Properties

        #region Helper Methods
        ListViewItem[] _ListViewItems;

        private void Bind(int bindType = 0)
        {
            //Debug.WriteLine("Binding with {0} - {1}|{2}|{3}", bindType, _attributes == null, _dataSources == null, _rules == null);
            if (bindType == 0)
            {
                if (_attributes == null)
                    return;

                listView.SuspendLayout();
                listView.Columns.Clear();
                listView.Items.Clear();
                listView.Columns.Add("Name");
                listView.Columns.Add("Type");
                if (ShowPosition)
                    listView.Columns.Add("Position");


                listView.Columns[0].Width = column1Width == 0 ? 200 : column1Width;
                listView.Columns[1].Width = column2Width == 0 ? 100 : column2Width;
                foreach (IdpeAttribute attribute in _attributes)
                {
                    ListViewItem item = new ListViewItem(attribute.Name);
                    item.SubItems.Add(attribute.Type);
                    if (ShowPosition)
                        item.SubItems.Add(attribute.Position.ToString());
                    item.ImageKey = attribute.Type.ToString() + ".gif";
                    item.Tag = attribute;

                    if ((FormatIsPrintable) && (attribute.IsAcceptable == false))
                        item.ForeColor = Color.LightGray;
                    if (DefaultItemId == attribute.AttributeId)
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        DefaultItem = item;
                    }

                    listView.Items.Add(item);
                }
                listView.ResumeLayout(true);
                
            }
            else if (bindType == 1)
            {
                if (_dataSources == null)
                    return;

                listView.SuspendLayout();
                listView.Columns.Clear();
                listView.Items.Clear();
                listView.AllowDrop = false;
                listView.Columns.Add("");
                listView.Columns[0].Width = column1Width == 0 ? 200 : column1Width;                
                listView.SmallImageList = null;
                listView.HeaderStyle = ColumnHeaderStyle.None;
                _dataSources = _dataSources.OrderBy(d => d.Name).ToList();
                foreach (IdpeDataSource datasource in _dataSources)
                {
                    if (datasource.Id == 100)
                        continue;
                    ListViewItem item = new ListViewItem(datasource.Name);
                    item.Tag = datasource;

                    if (DefaultItemId == datasource.Id)
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        DefaultItem = item;
                    }

                    listView.Items.Add(item);
                }
                listView.ResumeLayout(true);
            }
            else if (bindType == 2)
            {
                if (_rules == null)
                    return;

                listView.SuspendLayout();
                listView.Columns.Clear();
                listView.Items.Clear();
                listView.AllowDrop = false;
                listView.Columns.Add("Name");
                listView.Columns.Add("Priority");
                listView.Columns[0].Width = column1Width == 0 ? 200 : column1Width;
                listView.Columns[1].Width = column2Width == 0 ? 100 : column2Width;
                listView.SmallImageList = null;
              
                foreach (IdpeRule rule in _rules)
                {
                    ListViewItem item = new ListViewItem(rule.Name);
                    if (ShowPosition)
                        item.SubItems.Add(rule.Priority.ToString());
                    item.Tag = rule;

                    if (DefaultItemId == rule.Id)
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        DefaultItem = item;
                    }

                    listView.Items.Add(item);
                }

                if (ColumnSorter == null)
                {
                    ColumnSorter = new ListViewColumnSorter();
                    listView.ListViewItemSorter = ColumnSorter;
                    listView.Sorting = SortOrder.Ascending;
                    listView.AutoArrange = true;
                }

                ColumnSorter.SortColumn = 1;
                ColumnSorter.Order = SortOrder.Ascending;

                listView.Sort();
                listView.ResumeLayout(true);
            }

            _ListViewItems = new ListViewItem[listView.Items.Count];
            listView.Items.CopyTo(_ListViewItems, 0);
            this.toolStripStatusLabel1.Text = listView.Items.Count.ToString("0000");
        }

        private void Filter()
        {
            if (_ListViewItems == null)
                return; //form is not loaded, bind was not called yet

            listView.BeginUpdate();
            if (txtSearch.Text != "")
            {
                listView.Items.Clear();
                if (_dataSources != null)
                {
                    var filteredOnId = _ListViewItems.Where(l => ((IdpeDataSource)(l.Tag)).Id.ToString().Contains(txtSearch.Text.ToLower())).ToArray();
                    var filteredOnName = _ListViewItems.Where(l => ((IdpeDataSource)(l.Tag)).Name.ToLower().Contains(txtSearch.Text.ToLower())).ToArray();
                    var filtered = filteredOnName.Concat(filteredOnId).Distinct();
                    listView.Items.AddRange(filtered.ToArray());
                }
                else
                {
                    listView.Items.AddRange(_ListViewItems.Where(l => l.SubItems[0].Text.ToLower().Contains(txtSearch.Text.ToLower())).ToArray());
                }
                foreach (ListViewItem item in listView.Items)
                {
                    item.BackColor = SystemColors.Highlight;
                    item.ForeColor = SystemColors.HighlightText;
                }
                if (listView.SelectedItems.Count == 1)
                {
                    listView.Focus();
                }
            }
            else
            {
                listView.Items.Clear();
                listView.Items.AddRange(_ListViewItems);
                foreach (ListViewItem item in listView.Items)
                {
                    item.BackColor = SystemColors.Window;
                    item.ForeColor = SystemColors.WindowText;
                }
            }
            listView.EndUpdate();
        }

        private void SetButtonPositions()
        {            
            List<Button> visibleButtons = new List<Button>();
            if (btnAdd.Visible == true)
                visibleButtons.Add(btnAdd);
            if (btnSave.Visible == true)
                visibleButtons.Add(btnSave);
            if (btnPrint.Visible == true)
                visibleButtons.Add(btnPrint);

            int xposition = 3;
            foreach (Button btn in visibleButtons)
            {
                btn.Location = new Point(xposition, 6);
                xposition = xposition + 105;
            }
        }

        private void CallBind()
        {
            if (this.Attributes != null)
                Bind(0);
            else if (this.DataSources != null)
                Bind(1);
            else if (this.Rules != null)
                Bind(2);
        }

        #endregion Helper Methods

        #region Events

        #region Drag & Drop
        bool privateDrag;
       
        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            privateDrag = true;
            DoDragDrop(e.Item, DragDropEffects.Copy);
            privateDrag = false;
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            if (privateDrag) e.Effect = e.AllowedEffect;
        }

        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            if (!ShowPosition)
                return;

            if (!chkDrag.Checked)
            {
                this.listView.AllowDrop = false;
                return;
            }

            e.Effect = DragDropEffects.None;
            var pos = listView.PointToClient(new Point(e.X, e.Y));
            var hit = listView.HitTest(pos);
            if (hit.Item != null && hit.Item.Tag != null)
            {
                var dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                //Debug.WriteLine("Dragged from " + dragItem.SubItems[1].Text + ", to:" + hit.Item.SubItems[1].Text);

                List<ListViewItem> movedItems = new List<ListViewItem>();
                foreach (ListViewItem item in listView.SelectedItems)
                {
                    movedItems.Add(item);
                    listView.Items.Remove(item);
                }

                int position = hit.Item.Index;
                if (position == -1)
                    position = 0;

         
                foreach (ListViewItem item in movedItems)
                {
                    listView.Items.Insert(position, item);
                    position++;
                }

                position = 1;
                foreach (ListViewItem item in listView.Items)
                {
                    if (Rules != null)
                        item.SubItems[1].Text = position.ToString();
                    else
                        item.SubItems[2].Text = position.ToString();
                    position++;
                }
             
                listView.Enabled = false;
                timerPositionChangeSave.Enabled = true;
            }
        }

       
        #endregion Drag & Drop

        #region Other Events


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            timerFilter.Stop();
            timerFilter.Start();
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            CallBind();
        }        
      

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                txtSearch.Text = "";
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = listView.Items.Count.ToString("0000");
            if (this.ItemSelectedIndexChanged != null)
                this.ItemSelectedIndexChanged(this, e); 
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {            
            if (this.AddButtonClick != null)
                this.AddButtonClick(this, e);  
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.SaveButtonClick != null)
                this.SaveButtonClick(this, e);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {           
            if (this.PrintButtonClick != null)
                this.PrintButtonClick(this, e);  
        }

        private void SreListView_Resize(object sender, EventArgs e)
        {
            
        }

        int column1Width;
        int column2Width;
        private void listView_Resize(object sender, EventArgs e)
        {
            if (!this.IsDisposed)
            {
                var widthChangedThread = new System.Threading.Thread(() => SetNewColumnSizeWithThread()) { IsBackground = true };
                widthChangedThread.Start();
            }
        }

        private void SetNewColumnSizeWithThread()
        {
            try
            {
                if ((listView.IsHandleCreated) && (!this.IsDisposed))
                {
                    Invoke(new MethodInvoker(delegate
                {
                    SetNewColumnSize();
                }));

                }
            }
            catch { }
        }
        private void SetNewColumnSize()
        {
            if (!ShowingDataSource)
            {
                if (listView.Columns.Count > 1)
                {
                    column1Width = (listView.Width / 6) * 3;
                    column2Width = (int)((listView.Width / 6) * 1.2);

                    listView.Columns[0].Width = column1Width;
                    listView.Columns[1].Width = column2Width;
                }

            }
            else
            {
                column1Width = listView.Width - 30;
                if (listView.Columns.Count > 0)
                    listView.Columns[0].Width = column1Width;
            }
        }

        private void listView_Click(object sender, EventArgs e)
        {
            listView.SelectedIndexChanged -= listView_SelectedIndexChanged;
            listView.SuspendLayout();
            try
            {
                for (int i = listView.Items.Count - 1; i >= 0; i--)
                {
                    var item = listView.Items[i];

                    item.BackColor = SystemColors.Window;
                    IdpeAttribute attribute = item.Tag as IdpeAttribute;

                    if ((attribute != null)
                        && (((FormatIsPrintable) && (attribute.IsAcceptable == false))))
                        item.ForeColor = Color.LightGray;
                    else
                        item.ForeColor = SystemColors.ControlText;
                }
            }
            finally
            {
                listView.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
                listView.ResumeLayout();
            }

        }

        private void listView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CallBind();
            }
            else if ((e.Control) && (e.KeyCode == Keys.A))
            {
                listView.SelectedIndexChanged -= listView_SelectedIndexChanged;
                listView.SuspendLayout();
                try
                {
                    for (int i = 0; i < listView.Items.Count; i++)
                    {
                        listView.Items[i].Selected = true;
                    }
                }
                finally
                {
                    listView.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
                    listView.ResumeLayout();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (DeleteKeyPressed != null)
                    DeleteKeyPressed(this, e);
            }
        }

        private void chkDrag_CheckedChanged(object sender, EventArgs e)
        {
            this.listView.AllowDrop = chkDrag.Checked;
        }

        private void timerPositionChangeSave_Tick(object sender, EventArgs e)
        {
            timerPositionChangeSave.Enabled = false;
            listView.Enabled = true;
            if (Repositioned != null)
                Repositioned(this, e);
        }

        #endregion Other Events       

        private ListViewColumnSorter ColumnSorter = null;
        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (ColumnSorter == null)
            {
                ColumnSorter = new ListViewColumnSorter();
                listView.ListViewItemSorter = ColumnSorter;
                listView.Sorting = SortOrder.Ascending;
                listView.AutoArrange = true;
            }

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == ColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (ColumnSorter.Order == SortOrder.Ascending)
                {
                    ColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    ColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                ColumnSorter.SortColumn = e.Column;
                ColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listView.Sort();
        }

        private void timerFilter_Tick(object sender, EventArgs e)
        {
            timerFilter.Stop();
            Filter();
        }

        #endregion Events


    }
}


