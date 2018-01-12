namespace Symplus.RuleEngine.Utilities
{
    partial class frmConfig3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfig3));
            this.MainTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtbXslt = new System.Windows.Forms.RichTextBox();
            this.groupBoxXsltTop = new System.Windows.Forms.GroupBox();
            this.btnExportXslt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.rtbSampleOutput = new System.Windows.Forms.RichTextBox();
            this.pnlTransformButton = new System.Windows.Forms.Panel();
            this.btnTransform = new System.Windows.Forms.Button();
            this.groupBoxEDISampleFile = new System.Windows.Forms.GroupBox();
            this.btnExportSampleFile = new System.Windows.Forms.Button();
            this.lblResultCaption = new System.Windows.Forms.Label();
            this.groupBoxMainBottom = new System.Windows.Forms.GroupBox();
            this.pnlMainTop = new System.Windows.Forms.Panel();
            this.radConfigureMode = new System.Windows.Forms.RadioButton();
            this.radViewMode = new System.Windows.Forms.RadioButton();
            this.groupBoxTop = new System.Windows.Forms.GroupBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnBrowseSampleFile = new System.Windows.Forms.Button();
            this.pnlTips = new System.Windows.Forms.Panel();
            this.lblTips = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnShowXml = new System.Windows.Forms.Button();
            this.txtSampleFile = new System.Windows.Forms.TextBox();
            this.btnLoadSamples = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBoxDelimiter = new System.Windows.Forms.GroupBox();
            this.chkRenameHeaders = new System.Windows.Forms.CheckBox();
            this.cmbDelmiter = new System.Windows.Forms.ComboBox();
            this.chkFileHasHeader = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timerIdle = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnDefaultXslt = new System.Windows.Forms.Button();
            this.headerFooterConfiguration1 = new Symplus.RuleEngine.Utilities.HeaderFooterConfiguration();
            this.MainTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxXsltTop.SuspendLayout();
            this.pnlTransformButton.SuspendLayout();
            this.groupBoxEDISampleFile.SuspendLayout();
            this.pnlMainTop.SuspendLayout();
            this.groupBoxTop.SuspendLayout();
            this.pnlTips.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBoxDelimiter.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabs
            // 
            this.MainTabs.Controls.Add(this.tabPage1);
            this.MainTabs.Controls.Add(this.tabPage2);
            this.MainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabs.Location = new System.Drawing.Point(0, 0);
            this.MainTabs.Name = "MainTabs";
            this.MainTabs.SelectedIndex = 0;
            this.MainTabs.Size = new System.Drawing.Size(992, 637);
            this.MainTabs.TabIndex = 0;
            this.MainTabs.SelectedIndexChanged += new System.EventHandler(this.MainTabs_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.groupBoxMainBottom);
            this.tabPage1.Controls.Add(this.pnlMainTop);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(984, 611);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "EDI > Delimited";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 118);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbXslt);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxXsltTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtbSampleOutput);
            this.splitContainer1.Panel2.Controls.Add(this.pnlTransformButton);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxEDISampleFile);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(978, 426);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.TabIndex = 2;
            // 
            // rtbXslt
            // 
            this.rtbXslt.BackColor = System.Drawing.SystemColors.Control;
            this.rtbXslt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbXslt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbXslt.EnableAutoDragDrop = true;
            this.rtbXslt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbXslt.Location = new System.Drawing.Point(0, 58);
            this.rtbXslt.MaxLength = 3995;
            this.rtbXslt.Name = "rtbXslt";
            this.rtbXslt.Size = new System.Drawing.Size(978, 368);
            this.rtbXslt.TabIndex = 1;
            this.rtbXslt.Text = "";
            this.rtbXslt.TextChanged += new System.EventHandler(this.rtbXslt_TextChanged);
            // 
            // groupBoxXsltTop
            // 
            this.groupBoxXsltTop.Controls.Add(this.btnDefaultXslt);
            this.groupBoxXsltTop.Controls.Add(this.btnExportXslt);
            this.groupBoxXsltTop.Controls.Add(this.label3);
            this.groupBoxXsltTop.Controls.Add(this.btnImport);
            this.groupBoxXsltTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxXsltTop.Location = new System.Drawing.Point(0, 0);
            this.groupBoxXsltTop.Name = "groupBoxXsltTop";
            this.groupBoxXsltTop.Size = new System.Drawing.Size(978, 58);
            this.groupBoxXsltTop.TabIndex = 0;
            this.groupBoxXsltTop.TabStop = false;
            // 
            // btnExportXslt
            // 
            this.btnExportXslt.Enabled = false;
            this.btnExportXslt.Location = new System.Drawing.Point(86, 12);
            this.btnExportXslt.Name = "btnExportXslt";
            this.btnExportXslt.Size = new System.Drawing.Size(75, 23);
            this.btnExportXslt.TabIndex = 2;
            this.btnExportXslt.Text = "&Export";
            this.toolTip1.SetToolTip(this.btnExportXslt, "Saves the XSLT file into your local disk");
            this.btnExportXslt.UseVisualStyleBackColor = true;
            this.btnExportXslt.Click += new System.EventHandler(this.btnExportXslt_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Silver;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(972, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "XSLT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(5, 12);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "&Import";
            this.toolTip1.SetToolTip(this.btnImport, "Import XSLT file");
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // rtbSampleOutput
            // 
            this.rtbSampleOutput.BackColor = System.Drawing.SystemColors.Control;
            this.rtbSampleOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbSampleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSampleOutput.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.rtbSampleOutput.Location = new System.Drawing.Point(107, 58);
            this.rtbSampleOutput.Name = "rtbSampleOutput";
            this.rtbSampleOutput.ReadOnly = true;
            this.rtbSampleOutput.Size = new System.Drawing.Size(0, 42);
            this.rtbSampleOutput.TabIndex = 2;
            this.rtbSampleOutput.Text = "";
            this.rtbSampleOutput.TextChanged += new System.EventHandler(this.rtbSampleOutput_TextChanged);
            // 
            // pnlTransformButton
            // 
            this.pnlTransformButton.Controls.Add(this.btnTransform);
            this.pnlTransformButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTransformButton.Location = new System.Drawing.Point(0, 58);
            this.pnlTransformButton.Name = "pnlTransformButton";
            this.pnlTransformButton.Size = new System.Drawing.Size(107, 42);
            this.pnlTransformButton.TabIndex = 3;
            // 
            // btnTransform
            // 
            this.btnTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransform.Enabled = false;
            this.btnTransform.Location = new System.Drawing.Point(9, 197);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(89, 23);
            this.btnTransform.TabIndex = 3;
            this.btnTransform.Text = "&Transform";
            this.toolTip1.SetToolTip(this.btnTransform, "Transforms sample EDI X12 file into delimited file");
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Click += new System.EventHandler(this.btnTransform_Click);
            // 
            // groupBoxEDISampleFile
            // 
            this.groupBoxEDISampleFile.Controls.Add(this.btnExportSampleFile);
            this.groupBoxEDISampleFile.Controls.Add(this.lblResultCaption);
            this.groupBoxEDISampleFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxEDISampleFile.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEDISampleFile.Name = "groupBoxEDISampleFile";
            this.groupBoxEDISampleFile.Size = new System.Drawing.Size(96, 58);
            this.groupBoxEDISampleFile.TabIndex = 1;
            this.groupBoxEDISampleFile.TabStop = false;
            // 
            // btnExportSampleFile
            // 
            this.btnExportSampleFile.Enabled = false;
            this.btnExportSampleFile.Location = new System.Drawing.Point(6, 12);
            this.btnExportSampleFile.Name = "btnExportSampleFile";
            this.btnExportSampleFile.Size = new System.Drawing.Size(75, 23);
            this.btnExportSampleFile.TabIndex = 7;
            this.btnExportSampleFile.Text = "&Export";
            this.toolTip1.SetToolTip(this.btnExportSampleFile, "Saves the sample output file into your local disk");
            this.btnExportSampleFile.UseVisualStyleBackColor = true;
            this.btnExportSampleFile.Click += new System.EventHandler(this.btnExportSampleFile_Click);
            // 
            // lblResultCaption
            // 
            this.lblResultCaption.BackColor = System.Drawing.Color.Silver;
            this.lblResultCaption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResultCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultCaption.Location = new System.Drawing.Point(3, 42);
            this.lblResultCaption.Name = "lblResultCaption";
            this.lblResultCaption.Size = new System.Drawing.Size(90, 13);
            this.lblResultCaption.TabIndex = 6;
            this.lblResultCaption.Text = "EDI to Delimited";
            this.lblResultCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxMainBottom
            // 
            this.groupBoxMainBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxMainBottom.Location = new System.Drawing.Point(3, 544);
            this.groupBoxMainBottom.Name = "groupBoxMainBottom";
            this.groupBoxMainBottom.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxMainBottom.Size = new System.Drawing.Size(978, 64);
            this.groupBoxMainBottom.TabIndex = 1;
            this.groupBoxMainBottom.TabStop = false;
            // 
            // pnlMainTop
            // 
            this.pnlMainTop.Controls.Add(this.radConfigureMode);
            this.pnlMainTop.Controls.Add(this.radViewMode);
            this.pnlMainTop.Controls.Add(this.groupBoxTop);
            this.pnlMainTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMainTop.Location = new System.Drawing.Point(3, 3);
            this.pnlMainTop.Name = "pnlMainTop";
            this.pnlMainTop.Size = new System.Drawing.Size(978, 115);
            this.pnlMainTop.TabIndex = 0;
            // 
            // radConfigureMode
            // 
            this.radConfigureMode.AutoSize = true;
            this.radConfigureMode.Location = new System.Drawing.Point(92, 3);
            this.radConfigureMode.Name = "radConfigureMode";
            this.radConfigureMode.Size = new System.Drawing.Size(100, 17);
            this.radConfigureMode.TabIndex = 12;
            this.radConfigureMode.TabStop = true;
            this.radConfigureMode.Text = "&Configure Mode";
            this.toolTip1.SetToolTip(this.radConfigureMode, "Configure Mode [F2]");
            this.radConfigureMode.UseVisualStyleBackColor = true;
            this.radConfigureMode.CheckedChanged += new System.EventHandler(this.ViewModeChanged);
            // 
            // radViewMode
            // 
            this.radViewMode.AutoSize = true;
            this.radViewMode.Checked = true;
            this.radViewMode.Location = new System.Drawing.Point(8, 3);
            this.radViewMode.Name = "radViewMode";
            this.radViewMode.Size = new System.Drawing.Size(78, 17);
            this.radViewMode.TabIndex = 11;
            this.radViewMode.TabStop = true;
            this.radViewMode.Text = "&View Mode";
            this.toolTip1.SetToolTip(this.radViewMode, "View Mode [F2]");
            this.radViewMode.UseVisualStyleBackColor = true;
            this.radViewMode.CheckedChanged += new System.EventHandler(this.ViewModeChanged);
            // 
            // groupBoxTop
            // 
            this.groupBoxTop.Controls.Add(this.btnHelp);
            this.groupBoxTop.Controls.Add(this.btnBrowseSampleFile);
            this.groupBoxTop.Controls.Add(this.pnlTips);
            this.groupBoxTop.Controls.Add(this.label2);
            this.groupBoxTop.Controls.Add(this.btnShowXml);
            this.groupBoxTop.Controls.Add(this.txtSampleFile);
            this.groupBoxTop.Controls.Add(this.btnLoadSamples);
            this.groupBoxTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxTop.Location = new System.Drawing.Point(0, 24);
            this.groupBoxTop.Name = "groupBoxTop";
            this.groupBoxTop.Size = new System.Drawing.Size(978, 91);
            this.groupBoxTop.TabIndex = 10;
            this.groupBoxTop.TabStop = false;
            this.groupBoxTop.Visible = false;
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(191, 43);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(49, 23);
            this.btnHelp.TabIndex = 9;
            this.btnHelp.Text = "&Help";
            this.toolTip1.SetToolTip(this.btnHelp, "Shows help");
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnBrowseSampleFile
            // 
            this.btnBrowseSampleFile.Location = new System.Drawing.Point(565, 16);
            this.btnBrowseSampleFile.Name = "btnBrowseSampleFile";
            this.btnBrowseSampleFile.Size = new System.Drawing.Size(27, 22);
            this.btnBrowseSampleFile.TabIndex = 2;
            this.btnBrowseSampleFile.Text = "...";
            this.btnBrowseSampleFile.UseVisualStyleBackColor = true;
            this.btnBrowseSampleFile.Click += new System.EventHandler(this.btnBrowseSampleFile_Click);
            // 
            // pnlTips
            // 
            this.pnlTips.Controls.Add(this.lblTips);
            this.pnlTips.Controls.Add(this.pictureBox1);
            this.pnlTips.Location = new System.Drawing.Point(246, 43);
            this.pnlTips.Name = "pnlTips";
            this.pnlTips.Size = new System.Drawing.Size(702, 42);
            this.pnlTips.TabIndex = 8;
            this.pnlTips.Visible = false;
            // 
            // lblTips
            // 
            this.lblTips.BackColor = System.Drawing.SystemColors.Info;
            this.lblTips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTips.Location = new System.Drawing.Point(42, 0);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(660, 42);
            this.lblTips.TabIndex = 11;
            this.lblTips.Text = resources.GetString("lblTips.Text");
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Symplus.RuleEngine.Utilities.Properties.Resources.info;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sample EDI X12 file";
            // 
            // btnShowXml
            // 
            this.btnShowXml.Enabled = false;
            this.btnShowXml.Location = new System.Drawing.Point(112, 43);
            this.btnShowXml.Name = "btnShowXml";
            this.btnShowXml.Size = new System.Drawing.Size(73, 23);
            this.btnShowXml.TabIndex = 7;
            this.btnShowXml.Text = "&Show XML";
            this.toolTip1.SetToolTip(this.btnShowXml, "Generates XML from the EDI file & Opens it in default browser");
            this.btnShowXml.UseVisualStyleBackColor = true;
            this.btnShowXml.Click += new System.EventHandler(this.btnShowXml_Click);
            // 
            // txtSampleFile
            // 
            this.txtSampleFile.Location = new System.Drawing.Point(112, 17);
            this.txtSampleFile.Name = "txtSampleFile";
            this.txtSampleFile.Size = new System.Drawing.Size(448, 20);
            this.txtSampleFile.TabIndex = 1;
            this.txtSampleFile.TextChanged += new System.EventHandler(this.ValidateTransformReady);
            // 
            // btnLoadSamples
            // 
            this.btnLoadSamples.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSamples.Location = new System.Drawing.Point(869, 14);
            this.btnLoadSamples.Name = "btnLoadSamples";
            this.btnLoadSamples.Size = new System.Drawing.Size(105, 23);
            this.btnLoadSamples.TabIndex = 5;
            this.btnLoadSamples.Text = "Built-in Samples";
            this.toolTip1.SetToolTip(this.btnLoadSamples, "Loads sample EDI, along with XSLT and demonstrates the configuration");
            this.btnLoadSamples.UseVisualStyleBackColor = true;
            this.btnLoadSamples.Click += new System.EventHandler(this.btnLoadSamples_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.headerFooterConfiguration1);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBoxDelimiter);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(984, 611);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Delimited > SRE";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 544);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(978, 64);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Visible = false;
            // 
            // groupBoxDelimiter
            // 
            this.groupBoxDelimiter.Controls.Add(this.chkRenameHeaders);
            this.groupBoxDelimiter.Controls.Add(this.cmbDelmiter);
            this.groupBoxDelimiter.Controls.Add(this.chkFileHasHeader);
            this.groupBoxDelimiter.Controls.Add(this.label4);
            this.groupBoxDelimiter.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxDelimiter.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDelimiter.Name = "groupBoxDelimiter";
            this.groupBoxDelimiter.Size = new System.Drawing.Size(978, 43);
            this.groupBoxDelimiter.TabIndex = 1;
            this.groupBoxDelimiter.TabStop = false;
            // 
            // chkRenameHeaders
            // 
            this.chkRenameHeaders.AutoSize = true;
            this.chkRenameHeaders.Location = new System.Drawing.Point(206, 19);
            this.chkRenameHeaders.Name = "chkRenameHeaders";
            this.chkRenameHeaders.Size = new System.Drawing.Size(109, 17);
            this.chkRenameHeaders.TabIndex = 14;
            this.chkRenameHeaders.Text = "&Rename Headers";
            this.chkRenameHeaders.UseVisualStyleBackColor = true;
            // 
            // cmbDelmiter
            // 
            this.cmbDelmiter.FormattingEnabled = true;
            this.cmbDelmiter.Location = new System.Drawing.Point(62, 15);
            this.cmbDelmiter.Name = "cmbDelmiter";
            this.cmbDelmiter.Size = new System.Drawing.Size(44, 21);
            this.cmbDelmiter.TabIndex = 12;
            // 
            // chkFileHasHeader
            // 
            this.chkFileHasHeader.AutoSize = true;
            this.chkFileHasHeader.Location = new System.Drawing.Point(112, 19);
            this.chkFileHasHeader.Name = "chkFileHasHeader";
            this.chkFileHasHeader.Size = new System.Drawing.Size(83, 17);
            this.chkFileHasHeader.TabIndex = 13;
            this.chkFileHasHeader.Text = "Has Header";
            this.chkFileHasHeader.UseVisualStyleBackColor = true;
            this.chkFileHasHeader.CheckedChanged += new System.EventHandler(this.chkFileHasHeader_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Delimiter";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 615);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(992, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(977, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timerIdle
            // 
            this.timerIdle.Interval = 5000;
            this.timerIdle.Tick += new System.EventHandler(this.timerIdle_Tick);
            // 
            // btnDefaultXslt
            // 
            this.btnDefaultXslt.Location = new System.Drawing.Point(167, 12);
            this.btnDefaultXslt.Name = "btnDefaultXslt";
            this.btnDefaultXslt.Size = new System.Drawing.Size(75, 23);
            this.btnDefaultXslt.TabIndex = 3;
            this.btnDefaultXslt.Text = "&New";
            this.toolTip1.SetToolTip(this.btnDefaultXslt, "Saves the XSLT file into your local disk");
            this.btnDefaultXslt.UseVisualStyleBackColor = true;
            this.btnDefaultXslt.Visible = false;
            this.btnDefaultXslt.Click += new System.EventHandler(this.btnDefaultXslt_Click);
            // 
            // headerFooterConfiguration1
            // 
            this.headerFooterConfiguration1.DataFormatType = Symplus.RuleEngine.Common.DataFormatTypes.Delimited;
            this.headerFooterConfiguration1.DataSourceId = 0;
            this.headerFooterConfiguration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerFooterConfiguration1.DoNotCloseAtSave = true;
            this.headerFooterConfiguration1.Location = new System.Drawing.Point(3, 46);
            this.headerFooterConfiguration1.Name = "headerFooterConfiguration1";
            this.headerFooterConfiguration1.Size = new System.Drawing.Size(978, 498);
            this.headerFooterConfiguration1.TabIndex = 3;
            this.headerFooterConfiguration1.Saved += new System.EventHandler(this.headerFooterConfiguration1_Saved);
            // 
            // frmConfig3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 637);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainTabs);
            this.KeyPreview = true;
            this.Name = "frmConfig3";
            this.Text = "EDI X12 Configuration";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmConfig3_KeyUp);
            this.MainTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxXsltTop.ResumeLayout(false);
            this.pnlTransformButton.ResumeLayout(false);
            this.groupBoxEDISampleFile.ResumeLayout(false);
            this.pnlMainTop.ResumeLayout(false);
            this.pnlMainTop.PerformLayout();
            this.groupBoxTop.ResumeLayout(false);
            this.groupBoxTop.PerformLayout();
            this.pnlTips.ResumeLayout(false);
            this.pnlTips.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBoxDelimiter.ResumeLayout(false);
            this.groupBoxDelimiter.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabs;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pnlMainTop;
        private System.Windows.Forms.GroupBox groupBoxMainBottom;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxXsltTop;
        private System.Windows.Forms.RichTextBox rtbXslt;
        private System.Windows.Forms.GroupBox groupBoxEDISampleFile;
        private System.Windows.Forms.RichTextBox rtbSampleOutput;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox txtSampleFile;
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.Button btnBrowseSampleFile;
        private System.Windows.Forms.Button btnLoadSamples;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlTransformButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblResultCaption;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxDelimiter;        
        private System.Windows.Forms.CheckBox chkRenameHeaders;
        private System.Windows.Forms.ComboBox cmbDelmiter;
        private System.Windows.Forms.CheckBox chkFileHasHeader;
        private System.Windows.Forms.Label label4;
        private HeaderFooterConfiguration headerFooterConfiguration1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timerIdle;
        private System.Windows.Forms.Button btnShowXml;
        private System.Windows.Forms.Panel pnlTips;
        private System.Windows.Forms.Label lblTips;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnExportXslt;
        private System.Windows.Forms.Button btnExportSampleFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.GroupBox groupBoxTop;
        private System.Windows.Forms.RadioButton radConfigureMode;
        private System.Windows.Forms.RadioButton radViewMode;
        private System.Windows.Forms.Button btnDefaultXslt;
    }
}