namespace Eyedia.IDPE.Interface
{
    partial class FileLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.StripLine stripLine1 = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileLog));
            this.lvSreLog = new Eyedia.IDPE.Interface.Controls.PrintableListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openContainingFoldeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.cbDataSources = new System.Windows.Forms.ComboBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.dtPickerTo = new System.Windows.Forms.DateTimePicker();
            this.dtPickerFrom = new System.Windows.Forms.DateTimePicker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbShowAverage = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbChartTypes = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvSreLog
            // 
            this.lvSreLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader7,
            this.columnHeader2,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader3,
            this.columnHeader4});
            this.lvSreLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lvSreLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSreLog.FitToPage = false;
            this.lvSreLog.FullRowSelect = true;
            this.lvSreLog.Location = new System.Drawing.Point(4, 5);
            this.lvSreLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvSreLog.Name = "lvSreLog";
            this.lvSreLog.ShowItemToolTips = true;
            this.lvSreLog.Size = new System.Drawing.Size(1757, 273);
            this.lvSreLog.TabIndex = 0;
            this.lvSreLog.Title = "";
            this.lvSreLog.UseCompatibleStateImageBehavior = false;
            this.lvSreLog.View = System.Windows.Forms.View.Details;
            this.lvSreLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSreLog_ColumnClick);
            this.lvSreLog.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lvSreLog_ColumnWidthChanging);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Date and Time";
            this.columnHeader1.Width = 234;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "File Name";
            this.columnHeader7.Width = 230;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Sub File Name";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Data Source";
            this.columnHeader8.Width = 190;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Total Records";
            this.columnHeader9.Width = 80;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Total Valid";
            this.columnHeader10.Width = 80;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Started";
            this.columnHeader11.Width = 125;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Finished";
            this.columnHeader12.Width = 130;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Time Taken (hh:mm:ss:ms)";
            this.columnHeader13.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Environment";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Full File Name";
            this.columnHeader4.Width = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.clearFiltersToolStripMenuItem,
            this.toolStripMenuItem1,
            this.openContainingFoldeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exportToolStripMenuItem,
            this.printToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(288, 196);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.Filter);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // clearFiltersToolStripMenuItem
            // 
            this.clearFiltersToolStripMenuItem.Name = "clearFiltersToolStripMenuItem";
            this.clearFiltersToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.clearFiltersToolStripMenuItem.Text = "Clear Filters";
            this.clearFiltersToolStripMenuItem.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(284, 6);
            // 
            // openContainingFoldeToolStripMenuItem
            // 
            this.openContainingFoldeToolStripMenuItem.Name = "openContainingFoldeToolStripMenuItem";
            this.openContainingFoldeToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.openContainingFoldeToolStripMenuItem.Text = "Open Containing Folder";
            this.openContainingFoldeToolStripMenuItem.Click += new System.EventHandler(this.openContainingFoldeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(284, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(287, 30);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1773, 111);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnExport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 490);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1773, 65);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(1645, 16);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(112, 35);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(1525, 16);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(112, 35);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "&Export to Csv";
            this.toolTip1.SetToolTip(this.btnExport, "Export log to CSV file [F6]");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRefresh);
            this.groupBox3.Controls.Add(this.btnClearFilter);
            this.groupBox3.Controls.Add(this.cbDataSources);
            this.groupBox3.Controls.Add(this.txtFileName);
            this.groupBox3.Controls.Add(this.dtPickerTo);
            this.groupBox3.Controls.Add(this.dtPickerFrom);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 111);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(1773, 63);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRefresh.Location = new System.Drawing.Point(1378, 24);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(112, 34);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "&Refresh";
            this.toolTip1.SetToolTip(this.btnRefresh, "Refreshes data on screen [F5]");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Filter);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClearFilter.Location = new System.Drawing.Point(1266, 24);
            this.btnClearFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(112, 34);
            this.btnClearFilter.TabIndex = 5;
            this.btnClearFilter.Text = "&Clear Filters";
            this.toolTip1.SetToolTip(this.btnClearFilter, "Clear filters [F3]");
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // cbDataSources
            // 
            this.cbDataSources.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbDataSources.FormattingEnabled = true;
            this.cbDataSources.Location = new System.Drawing.Point(882, 24);
            this.cbDataSources.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbDataSources.Name = "cbDataSources";
            this.cbDataSources.Size = new System.Drawing.Size(384, 28);
            this.cbDataSources.TabIndex = 4;
            this.cbDataSources.TextChanged += new System.EventHandler(this.Filter);
            // 
            // txtFileName
            // 
            this.txtFileName.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtFileName.Location = new System.Drawing.Point(360, 24);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(522, 26);
            this.txtFileName.TabIndex = 3;
            this.txtFileName.TextChanged += new System.EventHandler(this.Filter);
            // 
            // dtPickerTo
            // 
            this.dtPickerTo.Checked = false;
            this.dtPickerTo.CustomFormat = "dd-MMM-yyyy";
            this.dtPickerTo.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtPickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerTo.Location = new System.Drawing.Point(182, 24);
            this.dtPickerTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtPickerTo.Name = "dtPickerTo";
            this.dtPickerTo.ShowCheckBox = true;
            this.dtPickerTo.Size = new System.Drawing.Size(178, 26);
            this.dtPickerTo.TabIndex = 2;
            this.dtPickerTo.ValueChanged += new System.EventHandler(this.Filter);
            // 
            // dtPickerFrom
            // 
            this.dtPickerFrom.Checked = false;
            this.dtPickerFrom.CustomFormat = "dd-MMM-yyyy";
            this.dtPickerFrom.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtPickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerFrom.Location = new System.Drawing.Point(4, 24);
            this.dtPickerFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtPickerFrom.Name = "dtPickerFrom";
            this.dtPickerFrom.ShowCheckBox = true;
            this.dtPickerFrom.Size = new System.Drawing.Size(178, 26);
            this.dtPickerFrom.TabIndex = 1;
            this.dtPickerFrom.ValueChanged += new System.EventHandler(this.Filter);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "csv";
            this.saveFileDialog1.FileName = "SreLog.csv";
            this.saveFileDialog1.Filter = "Csv Files|*.csv|All Files|*.*";
            this.saveFileDialog1.Title = "Export SRE File Log";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 174);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1773, 316);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvSreLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(1765, 283);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Detailed";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(1765, 283);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Summary";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            stripLine1.BorderColor = System.Drawing.Color.DimGray;
            stripLine1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            stripLine1.BorderWidth = 2;
            chartArea1.AxisY.StripLines.Add(stripLine1);
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.contextMenuStrip1;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(4, 68);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            this.chart1.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))))};
            series1.ChartArea = "ChartArea1";
            series1.IsValueShownAsLabel = true;
            series1.Legend = "Legend1";
            series1.Name = "Files";
            series1.XValueMember = "Date";
            series1.YValueMembers = "TotalFiles";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1752, 205);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            title1.ForeColor = System.Drawing.Color.Gray;
            title1.Name = "TitleHeader";
            title1.Text = "Integrated Data Processing Environment - Files per day";
            title2.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title2.ForeColor = System.Drawing.Color.Gray;
            title2.Name = "TitleFooter";
            title2.Text = "Printed By";
            this.chart1.Titles.Add(title1);
            this.chart1.Titles.Add(title2);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbShowAverage);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cbChartTypes);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(4, 5);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(1757, 63);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // cbShowAverage
            // 
            this.cbShowAverage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShowAverage.FormattingEnabled = true;
            this.cbShowAverage.Items.AddRange(new object[] {
            "RightAligned",
            "CenterAligned",
            "LeftAligned",
            "DoNotShow"});
            this.cbShowAverage.Location = new System.Drawing.Point(416, 20);
            this.cbShowAverage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbShowAverage.Name = "cbShowAverage";
            this.cbShowAverage.Size = new System.Drawing.Size(180, 28);
            this.cbShowAverage.TabIndex = 3;
            this.cbShowAverage.SelectedIndexChanged += new System.EventHandler(this.cbShowAverage_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Average";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chart Type";
            // 
            // cbChartTypes
            // 
            this.cbChartTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChartTypes.FormattingEnabled = true;
            this.cbChartTypes.Location = new System.Drawing.Point(102, 20);
            this.cbChartTypes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbChartTypes.Name = "cbChartTypes";
            this.cbChartTypes.Size = new System.Drawing.Size(190, 28);
            this.cbChartTypes.TabIndex = 0;
            this.cbChartTypes.SelectedIndexChanged += new System.EventHandler(this.cbChartTypes_SelectedIndexChanged);
            // 
            // FileLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1773, 555);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FileLog";
            this.Text = "Log";
            this.Load += new System.EventHandler(this.FileLog_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FileLog_KeyUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Eyedia.IDPE.Interface.Controls.PrintableListView lvSreLog;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbDataSources;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.DateTimePicker dtPickerFrom;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DateTimePicker dtPickerTo;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem openContainingFoldeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbChartTypes;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ComboBox cbShowAverage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader4;        
    }
}