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
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;

namespace Eyedia.IDPE.Interface
{
    public partial class FileLog : SREBaseFormNew
    {
        bool _Initialized;
        public FileLog()
        {
            InitializeComponent();
          
            dtPickerFrom.Checked = true;
            dtPickerFrom.Value = DateTime.Now.AddMonths(-1);
            Init();
            InitChart();
            BindData();
            _Initialized = true;
        }
        void Init()
        {
            List<IdpeDataSource> dataSources = new Manager().GetDataSources(1);
            dataSources = dataSources.OrderBy(d => d.Name).ToList();
            cbDataSources.Items.Clear();
            cbDataSources.Items.Add("");
            foreach (IdpeDataSource dataSource in dataSources)
            {
                cbDataSources.Items.Add(dataSource.Name);
            }
        }

        void InitChart()
        {
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

            if (Information.LoggedInUser != null)
                chart1.Titles[1].Text = "Printed By " + Information.LoggedInUser.FullName;

            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd-yy";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;

            cbChartTypes.DataSource = Enum.GetValues(typeof(System.Windows.Forms.DataVisualization.Charting.SeriesChartType));
            cbChartTypes.SelectedItem = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            cbShowAverage.SelectedIndex = 0;
        }

        void BindData()
        {
            this.Cursor = Cursors.WaitCursor;
            if (tabControl1.SelectedIndex == 0)
                BindDataDetailed();
            else
                BindDataSummary();
            this.Cursor = Cursors.Default;
        }

        void BindDataDetailed()
        {
            List<IdpeLog> logs = new Manager().GetLogs(
                dtPickerFrom.Checked ? (DateTime?)dtPickerFrom.Value : null,
                dtPickerTo.Checked ? (DateTime?)dtPickerTo.Value : null,
                string.IsNullOrEmpty(txtFileName.Text) ? null : txtFileName.Text,
                string.IsNullOrEmpty(cbDataSources.Text) ? null : cbDataSources.Text);            
            SetStatusText(logs.Count + " file(s)");
            if (!string.IsNullOrEmpty(exportFileName))
            {
                logs.ToDataTable().ToCsv(exportFileName);
                exportFileName = string.Empty;
            }

            lvSreLog.Items.Clear();
            foreach (IdpeLog log in logs)
            {                
                ListViewItem item = new ListViewItem(log.Started.ToString());
                item.SubItems.Add(Path.GetFileName(log.FileName));
                item.SubItems.Add(log.SubFileName);
                item.SubItems.Add(log.DataSourceName);
                item.SubItems.Add(log.TotalRecords.ToString());
                item.SubItems.Add(log.TotalValidRecords.ToString());
                item.SubItems.Add(log.Started.ToString());
                item.SubItems.Add(log.Finished.ToString());
                string ts = (log.Finished - log.Started).ToString();
                if(ts.Length > 4)
                    item.SubItems.Add(ts.Substring(0, ts.Length - 4));
                else
                    item.SubItems.Add(ts);
                               
                item.SubItems.Add(log.Environment);
                item.SubItems.Add(log.FileName);

                item.ToolTipText = log.Environment;
                if ((log.TotalRecords != log.TotalValidRecords)
                    || (log.TotalRecords == 0))
                    item.ForeColor = Color.Red;
                lvSreLog.Items.Add(item);
            }
        }
        void BindDataSummary()
        {
            DataTable table = new Manager().GetLogsSummary(
                dtPickerFrom.Checked ? (DateTime?)dtPickerFrom.Value : null,
                dtPickerTo.Checked ? (DateTime?)dtPickerTo.Value : null,
                string.IsNullOrEmpty(txtFileName.Text) ? null : txtFileName.Text,
                string.IsNullOrEmpty(cbDataSources.Text) ? null : cbDataSources.Text);

            
            SetStatusText("Day wise summary"); // table.Rows.Count + " day(s)";
            if (!string.IsNullOrEmpty(exportFileName))
            {
                table.ToCsv(exportFileName);
                exportFileName = string.Empty;
            }

            chart1.DataSource = table;
            chart1.DataBind();

            if (table.Rows.Count > 0)
            {
                chart1.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = chart1.DataManipulator.Statistics.Mean("Files");

                int avgFiles = (int)chart1.DataManipulator.Statistics.Mean("Files");
                object avgRecords = table.Compute("avg(TotalRecords)", string.Empty);
                chart1.ChartAreas[0].AxisY.StripLines[0].Text = string.Format("Average - {0} files({1} records)", avgFiles, avgRecords);

                switch (cbShowAverage.Text)
                {
                    case "RightAligned":
                        chart1.ChartAreas[0].AxisY.StripLines[0].TextAlignment = StringAlignment.Far;
                        break;
                    case "CenterAligned":
                        chart1.ChartAreas[0].AxisY.StripLines[0].TextAlignment = StringAlignment.Center;
                        break;
                    case "LeftAligned":
                        chart1.ChartAreas[0].AxisY.StripLines[0].TextAlignment = StringAlignment.Near;
                        break;
                    case "DoNotShow":
                        chart1.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = 0;
                        break;
                }
            }

            SetChartTransparency();
            //this.chart1.Titles[0].Text = string.Format("IDPE - Files per day (average {0} records)", avgRecords);
        }

        private void SetChartTransparency()
        {
            chart1.ApplyPaletteColors();
            foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart1.Series)
                foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint point in series.Points)
                    point.Color = Color.FromArgb(200, point.Color);  
        }

        private void lvSreLog_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            dtPickerFrom.Width = lvSreLog.Columns[0].Width / 2;  // - dtPickerFrom.Width + dtPickerTo.Width;
            dtPickerTo.Width = lvSreLog.Columns[0].Width / 2;
            txtFileName.Width = lvSreLog.Columns[1].Width + lvSreLog.Columns[2].Width;
            cbDataSources.Width = lvSreLog.Columns[3].Width;
        }

        public ListViewColumnSorter ColumnSorter = null;
        private void lvSreLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (ColumnSorter == null)
            {
                ColumnSorter = new ListViewColumnSorter();
                lvSreLog.ListViewItemSorter = ColumnSorter;
                lvSreLog.Sorting = SortOrder.Ascending;
                lvSreLog.AutoArrange = true;
            }

            if (e.Column == ColumnSorter.SortColumn)
            {
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
                ColumnSorter.SortColumn = e.Column;
                ColumnSorter.Order = SortOrder.Ascending;
            }

            lvSreLog.Sort();
        }

        private void Filter(object sender, EventArgs e)
        {
            BindData();
        }

        private void FileLog_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    btnClearFilter_Click(sender, e);
                    break;

                case Keys.F5:
                    BindData();
                    break;

                case Keys.F6:
                    btnExport_Click(sender, e);
                    break;
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dtPickerFrom.Checked = false;
            dtPickerTo.Checked = false;
            txtFileName.Text = "";
            cbDataSources.Text = "";
            BindData();
        }

        string exportFileName;
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                exportFileName = saveFileDialog1.FileName;
                BindData();
            }
        }

        private void FileLog_Load(object sender, EventArgs e)
        {
            MainWindow.SetToolStripStatusLabel(lvSreLog.Items.Count + " file(s)");
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                toolStripMenuItem1.Visible = true;
                openContainingFoldeToolStripMenuItem.Visible = true;
                openContainingFoldeToolStripMenuItem.Enabled = lvSreLog.SelectedItems.Count == 1 ? true : false;
            }
            else
            {
                toolStripMenuItem1.Visible = false;
                openContainingFoldeToolStripMenuItem.Visible = false;
            }
        }

        private void openContainingFoldeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dir = Path.GetDirectoryName(lvSreLog.SelectedItems[0].SubItems[9].Text);
            if ((!string.IsNullOrEmpty(dir))
                && (Directory.Exists(dir)))
                System.Diagnostics.Process.Start(dir);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
            if(tabControl1.SelectedIndex == 1)
            {
                if (chart1.Dock != DockStyle.Fill)
                    chart1.Dock = DockStyle.Fill;
            }
            // btnExport.Enabled = tabControl1.SelectedIndex == 0 ? true : false;
        }

        private void cbChartTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Initialized)
                chart1.Series[0].ChartType = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)cbChartTypes.SelectedItem;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                lvSreLog.FitToPage = true;
                lvSreLog.Title = "Integrated Data Processing Environment - File Log";
                lvSreLog.PrintPreview();
            }
            else
            {
                chart1.Printing.PrintDocument = new PrintDocument();
                chart1.Printing.PrintDocument.PrintPage += new PrintPageEventHandler(PrintGraph);
                chart1.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                chart1.Printing.PrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                chart1.Printing.PrintPreview();
            }
        }

        private void PrintGraph(object sender, PrintPageEventArgs ev)
        {
            Rectangle chartPosition = new Rectangle(ev.MarginBounds.X, ev.MarginBounds.Y, ev.MarginBounds.Width, ev.MarginBounds.Height);            
            chart1.Printing.PrintPaint(ev.Graphics, chartPosition);

        }

        private void cbShowAverage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataSummary();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnPrint_Click(sender, e);

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < lvSreLog.Columns.Count; i++)
            {
                buffer.Append(lvSreLog.Columns[i].Text);
                buffer.Append("\t");
            }

            buffer.Append(Environment.NewLine);

            for (int i = 0; i < lvSreLog.SelectedItems.Count; i++)
            {
                for (int j = 0; j < lvSreLog.Columns.Count; j++)
                {
                    buffer.Append(lvSreLog.SelectedItems[i].SubItems[j].Text);
                    buffer.Append("\t");
                }

                buffer.Append(Environment.NewLine);
            }
            Clipboard.SetText(buffer.ToString());

        }

    }
}


