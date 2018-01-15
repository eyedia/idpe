namespace Eyedia.IDPE.Interface
{
    partial class DockPanelProperty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockPanelProperty));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbShowDataSource = new System.Windows.Forms.ToolStripButton();
            this.tsbClearCache = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tbPrint = new System.Windows.Forms.ToolStripButton();
            this.tsbSqlStopRequest = new System.Windows.Forms.ToolStripButton();
            this.tsbTest = new System.Windows.Forms.ToolStripButton();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblGap = new System.Windows.Forms.Label();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.idpeRulesEditorControl1 = new Eyedia.IDPE.Interface.SreRulesEditorControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.idpeKeys1 = new Eyedia.IDPE.Interface.Controls.SreKeys();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.sreEmails1 = new Eyedia.IDPE.Interface.Controls.SreEmails();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.sreDatabases1 = new Eyedia.IDPE.Interface.Controls.SreDatabases();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.globalSearchWidget1 = new Eyedia.IDPE.Interface.GlobalSearchWidget();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timerBlank = new System.Windows.Forms.Timer(this.components);
            this.pnlTop.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 448);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(858, 48);
            this.pnlBottom.TabIndex = 3;
            this.pnlBottom.Visible = false;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(844, 347);
            this.propertyGrid.TabIndex = 4;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.toolStrip1);
            this.pnlTop.Controls.Add(this.lblCaption);
            this.pnlTop.Controls.Add(this.lblGap);
            this.pnlTop.Controls.Add(this.picIcon);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(858, 59);
            this.pnlTop.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbShowDataSource,
            this.tsbClearCache,
            this.tsbSave,
            this.tbPrint,
            this.tsbSqlStopRequest,
            this.tsbTest});
            this.toolStrip1.Location = new System.Drawing.Point(662, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(189, 32);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbShowDataSource
            // 
            this.tsbShowDataSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShowDataSource.Image = global::Eyedia.IDPE.Interface.Properties.Resources.DataSource;
            this.tsbShowDataSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowDataSource.Name = "tsbShowDataSource";
            this.tsbShowDataSource.Size = new System.Drawing.Size(28, 29);
            this.tsbShowDataSource.Text = "Data Source Property (F4)";
            this.tsbShowDataSource.Click += new System.EventHandler(this.tsbShowDataSource_Click);
            // 
            // tsbClearCache
            // 
            this.tsbClearCache.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClearCache.Image = global::Eyedia.IDPE.Interface.Properties.Resources.ClearCache;
            this.tsbClearCache.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearCache.Name = "tsbClearCache";
            this.tsbClearCache.Size = new System.Drawing.Size(28, 29);
            this.tsbClearCache.Text = "Clear Cache (F3)";
            this.tsbClearCache.Click += new System.EventHandler(this.tsbClearCache_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::Eyedia.IDPE.Interface.Properties.Resources.save;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(28, 29);
            this.tsbSave.Text = "Save (F6)";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tbPrint
            // 
            this.tbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPrint.Image = global::Eyedia.IDPE.Interface.Properties.Resources.printer_tool;
            this.tbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPrint.Name = "tbPrint";
            this.tbPrint.Size = new System.Drawing.Size(28, 29);
            this.tbPrint.Text = "Print";
            this.tbPrint.Click += new System.EventHandler(this.tbPrint_Click);
            // 
            // tsbSqlStopRequest
            // 
            this.tsbSqlStopRequest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSqlStopRequest.Image = global::Eyedia.IDPE.Interface.Properties.Resources.warning45;
            this.tsbSqlStopRequest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSqlStopRequest.Name = "tsbSqlStopRequest";
            this.tsbSqlStopRequest.Size = new System.Drawing.Size(28, 29);
            this.tsbSqlStopRequest.Text = "Request Stop";
            this.tsbSqlStopRequest.ToolTipText = "Request stop";
            this.tsbSqlStopRequest.Visible = false;
            this.tsbSqlStopRequest.Click += new System.EventHandler(this.tsbSqlStopRequest_Click);
            // 
            // tsbTest
            // 
            this.tsbTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTest.Image = global::Eyedia.IDPE.Interface.Properties.Resources.test;
            this.tsbTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTest.Name = "tsbTest";
            this.tsbTest.Size = new System.Drawing.Size(28, 28);
            this.tsbTest.Text = "Test";
            this.tsbTest.Click += new System.EventHandler(this.tsbTest_Click);
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorProvider1.SetIconAlignment(this.lblCaption, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.lblCaption.Location = new System.Drawing.Point(79, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(779, 59);
            this.lblCaption.TabIndex = 1;
            this.lblCaption.Text = "Data source Name";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGap
            // 
            this.lblGap.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblGap.Location = new System.Drawing.Point(61, 0);
            this.lblGap.Name = "lblGap";
            this.lblGap.Size = new System.Drawing.Size(18, 59);
            this.lblGap.TabIndex = 3;
            // 
            // picIcon
            // 
            this.picIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.errorProvider1.SetIconAlignment(this.picIcon, System.Windows.Forms.ErrorIconAlignment.BottomLeft);
            this.picIcon.Image = global::Eyedia.IDPE.Interface.Properties.Resources.SystemDataSource;
            this.picIcon.Location = new System.Drawing.Point(0, 0);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(61, 59);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picIcon.TabIndex = 0;
            this.picIcon.TabStop = false;
            this.picIcon.Click += new System.EventHandler(this.picIcon_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 62);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(858, 386);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(850, 353);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.idpeRulesEditorControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(850, 353);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rules";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // idpeRulesEditorControl1
            // 
            this.idpeRulesEditorControl1.DataSource = null;
            this.idpeRulesEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idpeRulesEditorControl1.Location = new System.Drawing.Point(3, 3);
            this.idpeRulesEditorControl1.Name = "idpeRulesEditorControl1";
            this.idpeRulesEditorControl1.ShowSqlInitRules = false;
            this.idpeRulesEditorControl1.Size = new System.Drawing.Size(844, 347);
            this.idpeRulesEditorControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.idpeKeys1);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(850, 353);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Keys";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // idpeKeys1
            // 
            this.idpeKeys1.DataSourceId = 0;
            this.idpeKeys1.DataSourceName = null;
            this.idpeKeys1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idpeKeys1.Location = new System.Drawing.Point(0, 0);
            this.idpeKeys1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.idpeKeys1.Name = "idpeKeys1";
            this.idpeKeys1.SaveButton = null;
            this.idpeKeys1.ShowCaption = false;
            this.idpeKeys1.Size = new System.Drawing.Size(850, 353);
            this.idpeKeys1.TabIndex = 0;
            this.idpeKeys1.ToolStripStatusLabel = null;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.sreEmails1);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(850, 353);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Emails";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // sreEmails1
            // 
            this.sreEmails1.DataSourceId = 0;
            this.sreEmails1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sreEmails1.Location = new System.Drawing.Point(0, 0);
            this.sreEmails1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sreEmails1.Name = "sreEmails1";
            this.sreEmails1.SaveButton = null;
            this.sreEmails1.Size = new System.Drawing.Size(850, 353);
            this.sreEmails1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.sreDatabases1);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(850, 353);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Database";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // sreDatabases1
            // 
            this.sreDatabases1.DataSourceId = 0;
            this.sreDatabases1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sreDatabases1.EnableGlobalKeys = false;
            this.sreDatabases1.Location = new System.Drawing.Point(0, 0);
            this.sreDatabases1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sreDatabases1.Name = "sreDatabases1";
            this.sreDatabases1.SaveButton = null;
            this.sreDatabases1.ShowSaveButton = false;
            this.sreDatabases1.Size = new System.Drawing.Size(850, 353);
            this.sreDatabases1.TabIndex = 1;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.globalSearchWidget1);
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(850, 353);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Search";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // globalSearchWidget1
            // 
            this.globalSearchWidget1.DataSourceId = 0;
            this.globalSearchWidget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalSearchWidget1.Location = new System.Drawing.Point(0, 0);
            this.globalSearchWidget1.Name = "globalSearchWidget1";
            this.globalSearchWidget1.ShowSearchTextBox = false;
            this.globalSearchWidget1.SingleDataSource = true;
            this.globalSearchWidget1.Size = new System.Drawing.Size(850, 353);
            this.globalSearchWidget1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "DataSource.png");
            // 
            // timerBlank
            // 
            this.timerBlank.Interval = 1000;
            this.timerBlank.Tick += new System.EventHandler(this.timerBlank_Tick);
            // 
            // DockPanelProperty
            // 
            this.ClientSize = new System.Drawing.Size(858, 499);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "DockPanelProperty";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.TabText = "Properties";
            this.Text = "Properties";
            this.pnlTop.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.PictureBox picIcon;
        internal System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private SreRulesEditorControl idpeRulesEditorControl1;
        private Controls.SreKeys idpeKeys1;
        private Controls.SreEmails sreEmails1;
        private System.Windows.Forms.TabPage tabPage5;
        private Controls.SreDatabases sreDatabases1;
        private System.Windows.Forms.Label lblGap;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbShowDataSource;
        private System.Windows.Forms.ToolStripButton tsbClearCache;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton tsbSqlStopRequest;
        private System.Windows.Forms.Timer timerBlank;
        private System.Windows.Forms.ToolStripButton tbPrint;
        private System.Windows.Forms.TabPage tabPage6;
        private GlobalSearchWidget globalSearchWidget1;
        private System.Windows.Forms.ToolStripButton tsbTest;
    }
}