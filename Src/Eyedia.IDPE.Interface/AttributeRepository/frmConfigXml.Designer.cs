namespace Eyedia.IDPE.Interface
{
    partial class frmConfigXml
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigXml));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnTransform = new System.Windows.Forms.Button();
            this.btnExportSampleFile = new System.Windows.Forms.Button();
            this.btnShowXml1 = new System.Windows.Forms.Button();
            this.radConfigureMode = new System.Windows.Forms.RadioButton();
            this.radViewMode = new System.Windows.Forms.RadioButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timerIdle = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.pnlXsltAndCSharpCode = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cSharpExpression1 = new Eyedia.IDPE.Interface.CSharpExpression();
            this.rtbXslt = new System.Windows.Forms.RichTextBox();
            this.groupBoxXsltTop = new System.Windows.Forms.GroupBox();
            this.lblParseMechanism = new System.Windows.Forms.Label();
            this.rtbSampleOutput = new System.Windows.Forms.RichTextBox();
            this.pnlTransformButton = new System.Windows.Forms.Panel();
            this.groupBoxDelimitedFileTop = new System.Windows.Forms.GroupBox();
            this.lblResultCaption = new System.Windows.Forms.Label();
            this.groupBoxXsltMainTop = new System.Windows.Forms.GroupBox();
            this.lblTips = new System.Windows.Forms.Label();
            this.btnBrowseSampleFile1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSampleFile = new System.Windows.Forms.TextBox();
            this.groupBoxMainBottom = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpCustom = new System.Windows.Forms.GroupBox();
            this.btnInterface2 = new System.Windows.Forms.Button();
            this.txtInterfaceName = new System.Windows.Forms.TextBox();
            this.pnlCustom = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.groupBoxTransformType = new System.Windows.Forms.GroupBox();
            this.radCustomInterface = new System.Windows.Forms.RadioButton();
            this.radCSharpCode = new System.Windows.Forms.RadioButton();
            this.radXslt = new System.Windows.Forms.RadioButton();
            this.groupBoxViewMode = new System.Windows.Forms.GroupBox();
            this.btnSamples = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.pnlXsltAndCSharpCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxXsltTop.SuspendLayout();
            this.pnlTransformButton.SuspendLayout();
            this.groupBoxDelimitedFileTop.SuspendLayout();
            this.groupBoxXsltMainTop.SuspendLayout();
            this.groupBoxMainBottom.SuspendLayout();
            this.grpCustom.SuspendLayout();
            this.pnlCustom.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.groupBoxTransformType.SuspendLayout();
            this.groupBoxViewMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 843);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1206, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1183, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(250, 18);
            this.btnNew.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(112, 35);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "&New";
            this.toolTip1.SetToolTip(this.btnNew, "New XSLT");
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(129, 18);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(112, 35);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "&Export";
            this.toolTip1.SetToolTip(this.btnExport, "Saves the XSLT file into your local disk");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(8, 18);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(112, 35);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "&Import";
            this.toolTip1.SetToolTip(this.btnImport, "Import XSLT file");
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnTransform
            // 
            this.btnTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransform.Enabled = false;
            this.btnTransform.Location = new System.Drawing.Point(14, 303);
            this.btnTransform.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(134, 35);
            this.btnTransform.TabIndex = 3;
            this.btnTransform.Text = "&Transform";
            this.toolTip1.SetToolTip(this.btnTransform, "Transforms sample EDI X12 file into delimited file");
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Click += new System.EventHandler(this.btnTransform_Click);
            // 
            // btnExportSampleFile
            // 
            this.btnExportSampleFile.Enabled = false;
            this.btnExportSampleFile.Location = new System.Drawing.Point(9, 18);
            this.btnExportSampleFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportSampleFile.Name = "btnExportSampleFile";
            this.btnExportSampleFile.Size = new System.Drawing.Size(112, 35);
            this.btnExportSampleFile.TabIndex = 7;
            this.btnExportSampleFile.Text = "&Export";
            this.toolTip1.SetToolTip(this.btnExportSampleFile, "Saves the sample output file into your local disk");
            this.btnExportSampleFile.UseVisualStyleBackColor = true;
            this.btnExportSampleFile.Click += new System.EventHandler(this.btnExportSampleFile_Click);
            // 
            // btnShowXml1
            // 
            this.btnShowXml1.Enabled = false;
            this.btnShowXml1.Location = new System.Drawing.Point(897, 23);
            this.btnShowXml1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnShowXml1.Name = "btnShowXml1";
            this.btnShowXml1.Size = new System.Drawing.Size(112, 35);
            this.btnShowXml1.TabIndex = 14;
            this.btnShowXml1.Text = "Show XML";
            this.toolTip1.SetToolTip(this.btnShowXml1, "Opens xml in default browser");
            this.btnShowXml1.UseVisualStyleBackColor = true;
            this.btnShowXml1.Click += new System.EventHandler(this.btnShowXml_Click);
            // 
            // radConfigureMode
            // 
            this.radConfigureMode.AutoSize = true;
            this.radConfigureMode.Location = new System.Drawing.Point(93, 31);
            this.radConfigureMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radConfigureMode.Name = "radConfigureMode";
            this.radConfigureMode.Size = new System.Drawing.Size(103, 24);
            this.radConfigureMode.TabIndex = 14;
            this.radConfigureMode.TabStop = true;
            this.radConfigureMode.Text = "&Configure";
            this.toolTip1.SetToolTip(this.radConfigureMode, "Configure Mode [F2]");
            this.radConfigureMode.UseVisualStyleBackColor = true;
            this.radConfigureMode.CheckedChanged += new System.EventHandler(this.ViewModeChanged);
            // 
            // radViewMode
            // 
            this.radViewMode.AutoSize = true;
            this.radViewMode.Checked = true;
            this.radViewMode.Location = new System.Drawing.Point(12, 31);
            this.radViewMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radViewMode.Name = "radViewMode";
            this.radViewMode.Size = new System.Drawing.Size(68, 24);
            this.radViewMode.TabIndex = 13;
            this.radViewMode.TabStop = true;
            this.radViewMode.Text = "&View";
            this.toolTip1.SetToolTip(this.radViewMode, "View Mode [F2]");
            this.radViewMode.UseVisualStyleBackColor = true;
            this.radViewMode.CheckedChanged += new System.EventHandler(this.ViewModeChanged);
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
            // pnlXsltAndCSharpCode
            // 
            this.pnlXsltAndCSharpCode.Controls.Add(this.splitContainer1);
            this.pnlXsltAndCSharpCode.Controls.Add(this.groupBoxXsltMainTop);
            this.pnlXsltAndCSharpCode.Location = new System.Drawing.Point(4, 189);
            this.pnlXsltAndCSharpCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlXsltAndCSharpCode.Name = "pnlXsltAndCSharpCode";
            this.pnlXsltAndCSharpCode.Size = new System.Drawing.Size(1017, 562);
            this.pnlXsltAndCSharpCode.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 100);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cSharpExpression1);
            this.splitContainer1.Panel1.Controls.Add(this.rtbXslt);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxXsltTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtbSampleOutput);
            this.splitContainer1.Panel2.Controls.Add(this.pnlTransformButton);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxDelimitedFileTop);
            this.splitContainer1.Size = new System.Drawing.Size(1017, 462);
            this.splitContainer1.SplitterDistance = 465;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 4;
            // 
            // cSharpExpression1
            // 
            this.cSharpExpression1.AdditionalReferences = "";
            this.cSharpExpression1.AdditionalUsingNamespace = "";
            this.cSharpExpression1.Code = "";
            this.cSharpExpression1.HelperMethods = "";
            this.cSharpExpression1.Location = new System.Drawing.Point(-14, 206);
            this.cSharpExpression1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cSharpExpression1.Name = "cSharpExpression1";
            this.cSharpExpression1.RawString = "DataTableGeneratorFromFileContent°°°°";
            this.cSharpExpression1.ShowHelperMethods = true;
            this.cSharpExpression1.Size = new System.Drawing.Size(478, 255);
            this.cSharpExpression1.TabIndex = 13;
            this.cSharpExpression1.Visible = false;
            this.cSharpExpression1.SourceCodeChanged += new System.EventHandler(this.txtSampleFile1_TextChanged);
            // 
            // rtbXslt
            // 
            this.rtbXslt.BackColor = System.Drawing.SystemColors.Window;
            this.rtbXslt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbXslt.EnableAutoDragDrop = true;
            this.rtbXslt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbXslt.Location = new System.Drawing.Point(0, 89);
            this.rtbXslt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbXslt.MaxLength = 3995;
            this.rtbXslt.Name = "rtbXslt";
            this.rtbXslt.Size = new System.Drawing.Size(242, 95);
            this.rtbXslt.TabIndex = 1;
            this.rtbXslt.Text = "";
            this.rtbXslt.TextChanged += new System.EventHandler(this.rtbXslt_TextChanged);
            // 
            // groupBoxXsltTop
            // 
            this.groupBoxXsltTop.Controls.Add(this.btnNew);
            this.groupBoxXsltTop.Controls.Add(this.btnExport);
            this.groupBoxXsltTop.Controls.Add(this.lblParseMechanism);
            this.groupBoxXsltTop.Controls.Add(this.btnImport);
            this.groupBoxXsltTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxXsltTop.Location = new System.Drawing.Point(0, 0);
            this.groupBoxXsltTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxXsltTop.Name = "groupBoxXsltTop";
            this.groupBoxXsltTop.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxXsltTop.Size = new System.Drawing.Size(465, 89);
            this.groupBoxXsltTop.TabIndex = 0;
            this.groupBoxXsltTop.TabStop = false;
            // 
            // lblParseMechanism
            // 
            this.lblParseMechanism.BackColor = System.Drawing.Color.Silver;
            this.lblParseMechanism.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblParseMechanism.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParseMechanism.Location = new System.Drawing.Point(4, 64);
            this.lblParseMechanism.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParseMechanism.Name = "lblParseMechanism";
            this.lblParseMechanism.Size = new System.Drawing.Size(457, 20);
            this.lblParseMechanism.TabIndex = 1;
            this.lblParseMechanism.Text = "XSLT";
            this.lblParseMechanism.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbSampleOutput
            // 
            this.rtbSampleOutput.BackColor = System.Drawing.SystemColors.Control;
            this.rtbSampleOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbSampleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSampleOutput.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.rtbSampleOutput.Location = new System.Drawing.Point(160, 89);
            this.rtbSampleOutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbSampleOutput.Name = "rtbSampleOutput";
            this.rtbSampleOutput.ReadOnly = true;
            this.rtbSampleOutput.Size = new System.Drawing.Size(386, 373);
            this.rtbSampleOutput.TabIndex = 2;
            this.rtbSampleOutput.Text = "";
            this.rtbSampleOutput.TextChanged += new System.EventHandler(this.rtbSampleOutput_TextChanged);
            // 
            // pnlTransformButton
            // 
            this.pnlTransformButton.Controls.Add(this.btnTransform);
            this.pnlTransformButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTransformButton.Location = new System.Drawing.Point(0, 89);
            this.pnlTransformButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTransformButton.Name = "pnlTransformButton";
            this.pnlTransformButton.Size = new System.Drawing.Size(160, 373);
            this.pnlTransformButton.TabIndex = 3;
            // 
            // groupBoxDelimitedFileTop
            // 
            this.groupBoxDelimitedFileTop.Controls.Add(this.btnExportSampleFile);
            this.groupBoxDelimitedFileTop.Controls.Add(this.lblResultCaption);
            this.groupBoxDelimitedFileTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxDelimitedFileTop.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDelimitedFileTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxDelimitedFileTop.Name = "groupBoxDelimitedFileTop";
            this.groupBoxDelimitedFileTop.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxDelimitedFileTop.Size = new System.Drawing.Size(546, 89);
            this.groupBoxDelimitedFileTop.TabIndex = 1;
            this.groupBoxDelimitedFileTop.TabStop = false;
            // 
            // lblResultCaption
            // 
            this.lblResultCaption.BackColor = System.Drawing.Color.Silver;
            this.lblResultCaption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResultCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultCaption.Location = new System.Drawing.Point(4, 64);
            this.lblResultCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResultCaption.Name = "lblResultCaption";
            this.lblResultCaption.Size = new System.Drawing.Size(538, 20);
            this.lblResultCaption.TabIndex = 6;
            this.lblResultCaption.Text = "Sample XML to Delimited";
            this.lblResultCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxXsltMainTop
            // 
            this.groupBoxXsltMainTop.Controls.Add(this.btnShowXml1);
            this.groupBoxXsltMainTop.Controls.Add(this.lblTips);
            this.groupBoxXsltMainTop.Controls.Add(this.btnBrowseSampleFile1);
            this.groupBoxXsltMainTop.Controls.Add(this.label2);
            this.groupBoxXsltMainTop.Controls.Add(this.txtSampleFile);
            this.groupBoxXsltMainTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxXsltMainTop.Location = new System.Drawing.Point(0, 0);
            this.groupBoxXsltMainTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxXsltMainTop.Name = "groupBoxXsltMainTop";
            this.groupBoxXsltMainTop.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxXsltMainTop.Size = new System.Drawing.Size(1017, 100);
            this.groupBoxXsltMainTop.TabIndex = 10;
            this.groupBoxXsltMainTop.TabStop = false;
            // 
            // lblTips
            // 
            this.lblTips.BackColor = System.Drawing.SystemColors.Info;
            this.lblTips.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTips.Location = new System.Drawing.Point(4, 66);
            this.lblTips.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(1009, 29);
            this.lblTips.TabIndex = 13;
            this.lblTips.Text = "Write the XSLT to transform XML to pipe(|) delimited data, if you have sample fil" +
    "e, you can see transform result";
            // 
            // btnBrowseSampleFile1
            // 
            this.btnBrowseSampleFile1.Location = new System.Drawing.Point(848, 25);
            this.btnBrowseSampleFile1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowseSampleFile1.Name = "btnBrowseSampleFile1";
            this.btnBrowseSampleFile1.Size = new System.Drawing.Size(40, 34);
            this.btnBrowseSampleFile1.TabIndex = 2;
            this.btnBrowseSampleFile1.Text = "...";
            this.btnBrowseSampleFile1.UseVisualStyleBackColor = true;
            this.btnBrowseSampleFile1.Click += new System.EventHandler(this.btnBrowseSampleFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sample XML file";
            // 
            // txtSampleFile
            // 
            this.txtSampleFile.Location = new System.Drawing.Point(136, 26);
            this.txtSampleFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSampleFile.Name = "txtSampleFile";
            this.txtSampleFile.Size = new System.Drawing.Size(702, 26);
            this.txtSampleFile.TabIndex = 1;
            this.txtSampleFile.TextChanged += new System.EventHandler(this.txtSampleFile1_TextChanged);
            // 
            // groupBoxMainBottom
            // 
            this.groupBoxMainBottom.Controls.Add(this.btnCancel);
            this.groupBoxMainBottom.Controls.Add(this.btnSave);
            this.groupBoxMainBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxMainBottom.Location = new System.Drawing.Point(0, 766);
            this.groupBoxMainBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxMainBottom.Name = "groupBoxMainBottom";
            this.groupBoxMainBottom.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxMainBottom.Size = new System.Drawing.Size(1206, 77);
            this.groupBoxMainBottom.TabIndex = 3;
            this.groupBoxMainBottom.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(612, 25);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 37);
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "C&lose";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(464, 25);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 37);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpCustom
            // 
            this.grpCustom.Controls.Add(this.btnInterface2);
            this.grpCustom.Controls.Add(this.txtInterfaceName);
            this.grpCustom.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpCustom.Location = new System.Drawing.Point(0, 0);
            this.grpCustom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCustom.Name = "grpCustom";
            this.grpCustom.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCustom.Size = new System.Drawing.Size(909, 86);
            this.grpCustom.TabIndex = 8;
            this.grpCustom.TabStop = false;
            this.grpCustom.Text = "Interface Type";
            // 
            // btnInterface2
            // 
            this.btnInterface2.Location = new System.Drawing.Point(854, 37);
            this.btnInterface2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInterface2.Name = "btnInterface2";
            this.btnInterface2.Size = new System.Drawing.Size(38, 35);
            this.btnInterface2.TabIndex = 27;
            this.btnInterface2.Text = "...";
            this.btnInterface2.UseVisualStyleBackColor = true;
            this.btnInterface2.Click += new System.EventHandler(this.btnInterface2_Click);
            // 
            // txtInterfaceName
            // 
            this.txtInterfaceName.Location = new System.Drawing.Point(9, 42);
            this.txtInterfaceName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInterfaceName.Name = "txtInterfaceName";
            this.txtInterfaceName.Size = new System.Drawing.Size(834, 26);
            this.txtInterfaceName.TabIndex = 26;
            // 
            // pnlCustom
            // 
            this.pnlCustom.Controls.Add(this.grpCustom);
            this.pnlCustom.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCustom.Location = new System.Drawing.Point(0, 87);
            this.pnlCustom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCustom.Name = "pnlCustom";
            this.pnlCustom.Size = new System.Drawing.Size(1206, 86);
            this.pnlCustom.TabIndex = 9;
            this.pnlCustom.Visible = false;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.groupBoxTransformType);
            this.pnlTop.Controls.Add(this.groupBoxViewMode);
            this.pnlTop.Controls.Add(this.btnSamples);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 15);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1206, 72);
            this.pnlTop.TabIndex = 10;
            // 
            // groupBoxTransformType
            // 
            this.groupBoxTransformType.Controls.Add(this.radCustomInterface);
            this.groupBoxTransformType.Controls.Add(this.radCSharpCode);
            this.groupBoxTransformType.Controls.Add(this.radXslt);
            this.groupBoxTransformType.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxTransformType.Location = new System.Drawing.Point(218, 0);
            this.groupBoxTransformType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxTransformType.Name = "groupBoxTransformType";
            this.groupBoxTransformType.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxTransformType.Size = new System.Drawing.Size(465, 72);
            this.groupBoxTransformType.TabIndex = 5;
            this.groupBoxTransformType.TabStop = false;
            this.groupBoxTransformType.Text = "Transform XML using";
            // 
            // radCustomInterface
            // 
            this.radCustomInterface.AutoSize = true;
            this.radCustomInterface.Location = new System.Drawing.Point(196, 31);
            this.radCustomInterface.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radCustomInterface.Name = "radCustomInterface";
            this.radCustomInterface.Size = new System.Drawing.Size(157, 24);
            this.radCustomInterface.TabIndex = 2;
            this.radCustomInterface.Text = "Custom Interface";
            this.radCustomInterface.UseVisualStyleBackColor = true;
            this.radCustomInterface.CheckedChanged += new System.EventHandler(this.TransformMechanismChanged);
            // 
            // radCSharpCode
            // 
            this.radCSharpCode.AutoSize = true;
            this.radCSharpCode.Location = new System.Drawing.Point(87, 31);
            this.radCSharpCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radCSharpCode.Name = "radCSharpCode";
            this.radCSharpCode.Size = new System.Drawing.Size(96, 24);
            this.radCSharpCode.TabIndex = 1;
            this.radCSharpCode.Text = "C# Code";
            this.radCSharpCode.UseVisualStyleBackColor = true;
            this.radCSharpCode.CheckedChanged += new System.EventHandler(this.TransformMechanismChanged);
            // 
            // radXslt
            // 
            this.radXslt.AutoSize = true;
            this.radXslt.Checked = true;
            this.radXslt.Location = new System.Drawing.Point(18, 29);
            this.radXslt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radXslt.Name = "radXslt";
            this.radXslt.Size = new System.Drawing.Size(61, 24);
            this.radXslt.TabIndex = 0;
            this.radXslt.TabStop = true;
            this.radXslt.Text = "&Xslt";
            this.radXslt.UseVisualStyleBackColor = true;
            this.radXslt.CheckedChanged += new System.EventHandler(this.TransformMechanismChanged);
            // 
            // groupBoxViewMode
            // 
            this.groupBoxViewMode.Controls.Add(this.radConfigureMode);
            this.groupBoxViewMode.Controls.Add(this.radViewMode);
            this.groupBoxViewMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxViewMode.Location = new System.Drawing.Point(0, 0);
            this.groupBoxViewMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxViewMode.Name = "groupBoxViewMode";
            this.groupBoxViewMode.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxViewMode.Size = new System.Drawing.Size(218, 72);
            this.groupBoxViewMode.TabIndex = 30;
            this.groupBoxViewMode.TabStop = false;
            this.groupBoxViewMode.Text = "View Mode";
            // 
            // btnSamples
            // 
            this.btnSamples.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSamples.Location = new System.Drawing.Point(1089, 20);
            this.btnSamples.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSamples.Name = "btnSamples";
            this.btnSamples.Size = new System.Drawing.Size(112, 37);
            this.btnSamples.TabIndex = 29;
            this.btnSamples.Text = "Samples";
            this.btnSamples.UseVisualStyleBackColor = true;
            this.btnSamples.Click += new System.EventHandler(this.btnSamples_Click);
            // 
            // frmConfigXml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 865);
            this.Controls.Add(this.pnlCustom);
            this.Controls.Add(this.pnlXsltAndCSharpCode);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.groupBoxMainBottom);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmConfigXml";
            this.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XML Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfigXml_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmConfigXml_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlXsltAndCSharpCode.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxXsltTop.ResumeLayout(false);
            this.pnlTransformButton.ResumeLayout(false);
            this.groupBoxDelimitedFileTop.ResumeLayout(false);
            this.groupBoxXsltMainTop.ResumeLayout(false);
            this.groupBoxXsltMainTop.PerformLayout();
            this.groupBoxMainBottom.ResumeLayout(false);
            this.grpCustom.ResumeLayout(false);
            this.grpCustom.PerformLayout();
            this.pnlCustom.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.groupBoxTransformType.ResumeLayout(false);
            this.groupBoxTransformType.PerformLayout();
            this.groupBoxViewMode.ResumeLayout(false);
            this.groupBoxViewMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timerIdle;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Panel pnlXsltAndCSharpCode;
        private System.Windows.Forms.GroupBox groupBoxXsltMainTop;
        private System.Windows.Forms.Button btnBrowseSampleFile1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSampleFile;
        private System.Windows.Forms.GroupBox groupBoxMainBottom;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtbXslt;
        private System.Windows.Forms.GroupBox groupBoxXsltTop;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblParseMechanism;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.RichTextBox rtbSampleOutput;
        private System.Windows.Forms.Panel pnlTransformButton;
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.GroupBox groupBoxDelimitedFileTop;
        private System.Windows.Forms.Button btnExportSampleFile;
        private System.Windows.Forms.Label lblResultCaption;
        private CSharpExpression cSharpExpression1;
        private System.Windows.Forms.GroupBox grpCustom;
        private System.Windows.Forms.Button btnInterface2;
        private System.Windows.Forms.TextBox txtInterfaceName;
        private System.Windows.Forms.Panel pnlCustom;
        private System.Windows.Forms.Label lblTips;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.GroupBox groupBoxTransformType;
        private System.Windows.Forms.RadioButton radCustomInterface;
        private System.Windows.Forms.RadioButton radCSharpCode;
        private System.Windows.Forms.RadioButton radXslt;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnShowXml1;
        private System.Windows.Forms.Button btnSamples;
        private System.Windows.Forms.GroupBox groupBoxViewMode;
        private System.Windows.Forms.RadioButton radConfigureMode;
        private System.Windows.Forms.RadioButton radViewMode;
    }
}