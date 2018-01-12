namespace Eyedia.IDPE.Interface
{
    partial class EnvironmentWindow
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbParam2 = new System.Windows.Forms.ComboBox();
            this.cbParam1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCommands = new System.Windows.Forms.ComboBox();
            this.cbDataSources = new System.Windows.Forms.ComboBox();
            this.cbRules = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDataSources = new System.Windows.Forms.Label();
            this.lblParam1 = new System.Windows.Forms.Label();
            this.btnAction = new System.Windows.Forms.Button();
            this.btnDeployDataSource = new System.Windows.Forms.Button();
            this.btnDeployRule = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.chkIncludeSystemDS = new System.Windows.Forms.CheckBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkAllEnvs = new System.Windows.Forms.CheckBox();
            this.cbServiceInstances = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbEnvs = new System.Windows.Forms.ComboBox();
            this.lblInstances = new System.Windows.Forms.Label();
            this.nudInstances = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radTcpIp = new System.Windows.Forms.RadioButton();
            this.radFileSystem = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenSreEnv = new System.Windows.Forms.Button();
            this.rtfLog = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pgSymplus = new System.Windows.Forms.PropertyGrid();
            this.pnlConfigPropTop = new System.Windows.Forms.Panel();
            this.cbEnvConfigs = new System.Windows.Forms.ComboBox();
            this.btnRefreshConfig = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.picServerStatus = new System.Windows.Forms.PictureBox();
            this.pnlConfigPropBottom = new System.Windows.Forms.Panel();
            this.lblConfigFileName = new System.Windows.Forms.Label();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.environmentsControl1 = new Eyedia.IDPE.Interface.SreEnvironmentsControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timerServerStatus = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timerDelayedCommand = new System.Windows.Forms.Timer(this.components);
            this.pnlBottom.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInstances)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlConfigPropTop.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picServerStatus)).BeginInit();
            this.pnlConfigPropBottom.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(659, 32);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.Visible = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 351);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(659, 35);
            this.pnlBottom.TabIndex = 1;
            this.pnlBottom.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(269, 6);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(67, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbCommands, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbDataSources, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbRules, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDataSources, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblParam1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnAction, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDeployDataSource, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDeployRule, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnExecute, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkIncludeSystemDS, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenFile, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(342, 101);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbParam2);
            this.panel2.Controls.Add(this.cbParam1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(79, 77);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(103, 22);
            this.panel2.TabIndex = 13;
            // 
            // cbParam2
            // 
            this.cbParam2.FormattingEnabled = true;
            this.cbParam2.Location = new System.Drawing.Point(83, 4);
            this.cbParam2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbParam2.Name = "cbParam2";
            this.cbParam2.Size = new System.Drawing.Size(82, 21);
            this.cbParam2.TabIndex = 13;
            // 
            // cbParam1
            // 
            this.cbParam1.FormattingEnabled = true;
            this.cbParam1.Location = new System.Drawing.Point(2, 4);
            this.cbParam1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbParam1.Name = "cbParam1";
            this.cbParam1.Size = new System.Drawing.Size(89, 21);
            this.cbParam1.TabIndex = 12;
            this.cbParam1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(2, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label2.Size = new System.Drawing.Size(73, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Command";
            // 
            // cbCommands
            // 
            this.cbCommands.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommands.DropDownWidth = 300;
            this.cbCommands.FormattingEnabled = true;
            this.cbCommands.Location = new System.Drawing.Point(79, 2);
            this.cbCommands.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbCommands.Name = "cbCommands";
            this.cbCommands.Size = new System.Drawing.Size(103, 21);
            this.cbCommands.TabIndex = 3;
            this.cbCommands.SelectedIndexChanged += new System.EventHandler(this.cbCommands_SelectedIndexChanged);
            // 
            // cbDataSources
            // 
            this.cbDataSources.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbDataSources.FormattingEnabled = true;
            this.cbDataSources.Location = new System.Drawing.Point(79, 27);
            this.cbDataSources.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbDataSources.Name = "cbDataSources";
            this.cbDataSources.Size = new System.Drawing.Size(103, 21);
            this.cbDataSources.TabIndex = 5;
            // 
            // cbRules
            // 
            this.cbRules.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbRules.FormattingEnabled = true;
            this.cbRules.Location = new System.Drawing.Point(79, 52);
            this.cbRules.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbRules.Name = "cbRules";
            this.cbRules.Size = new System.Drawing.Size(103, 21);
            this.cbRules.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(2, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label4.Size = new System.Drawing.Size(73, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Rules";
            // 
            // lblDataSources
            // 
            this.lblDataSources.AutoSize = true;
            this.lblDataSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDataSources.Location = new System.Drawing.Point(2, 25);
            this.lblDataSources.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDataSources.Name = "lblDataSources";
            this.lblDataSources.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblDataSources.Size = new System.Drawing.Size(73, 25);
            this.lblDataSources.TabIndex = 7;
            this.lblDataSources.Text = "Data Sources";
            // 
            // lblParam1
            // 
            this.lblParam1.AutoSize = true;
            this.lblParam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblParam1.Location = new System.Drawing.Point(2, 75);
            this.lblParam1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblParam1.Name = "lblParam1";
            this.lblParam1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblParam1.Size = new System.Drawing.Size(73, 26);
            this.lblParam1.TabIndex = 11;
            this.lblParam1.Text = "Command";
            this.toolTip1.SetToolTip(this.lblParam1, "Click to clear history");
            this.lblParam1.Visible = false;
            this.lblParam1.Click += new System.EventHandler(this.lblParam1_Click);
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(271, 2);
            this.btnAction.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(67, 19);
            this.btnAction.TabIndex = 4;
            this.btnAction.Text = "Action";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // btnDeployDataSource
            // 
            this.btnDeployDataSource.Location = new System.Drawing.Point(271, 27);
            this.btnDeployDataSource.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDeployDataSource.Name = "btnDeployDataSource";
            this.btnDeployDataSource.Size = new System.Drawing.Size(67, 19);
            this.btnDeployDataSource.TabIndex = 9;
            this.btnDeployDataSource.Text = "Deploy";
            this.btnDeployDataSource.UseVisualStyleBackColor = true;
            this.btnDeployDataSource.Click += new System.EventHandler(this.btnDeployDataSource_Click);
            // 
            // btnDeployRule
            // 
            this.btnDeployRule.Location = new System.Drawing.Point(271, 52);
            this.btnDeployRule.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDeployRule.Name = "btnDeployRule";
            this.btnDeployRule.Size = new System.Drawing.Size(67, 19);
            this.btnDeployRule.TabIndex = 10;
            this.btnDeployRule.Text = "Deploy";
            this.btnDeployRule.UseVisualStyleBackColor = true;
            this.btnDeployRule.Click += new System.EventHandler(this.btnDeployRule_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(271, 77);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(67, 19);
            this.btnExecute.TabIndex = 13;
            this.btnExecute.Text = "&Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Visible = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // chkIncludeSystemDS
            // 
            this.chkIncludeSystemDS.AutoSize = true;
            this.chkIncludeSystemDS.Checked = true;
            this.chkIncludeSystemDS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeSystemDS.Enabled = false;
            this.chkIncludeSystemDS.Location = new System.Drawing.Point(186, 27);
            this.chkIncludeSystemDS.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkIncludeSystemDS.Name = "chkIncludeSystemDS";
            this.chkIncludeSystemDS.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.chkIncludeSystemDS.Size = new System.Drawing.Size(81, 21);
            this.chkIncludeSystemDS.TabIndex = 14;
            this.chkIncludeSystemDS.Text = "Include &Sys";
            this.toolTip1.SetToolTip(this.chkIncludeSystemDS, "Includes system side attributes along with regular attributes");
            this.chkIncludeSystemDS.UseVisualStyleBackColor = true;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(186, 77);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(27, 19);
            this.btnOpenFile.TabIndex = 18;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Visible = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkAllEnvs);
            this.panel3.Controls.Add(this.cbServiceInstances);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(186, 2);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(81, 21);
            this.panel3.TabIndex = 19;
            // 
            // chkAllEnvs
            // 
            this.chkAllEnvs.AutoSize = true;
            this.chkAllEnvs.Location = new System.Drawing.Point(41, 6);
            this.chkAllEnvs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkAllEnvs.Name = "chkAllEnvs";
            this.chkAllEnvs.Size = new System.Drawing.Size(71, 21);
            this.chkAllEnvs.TabIndex = 23;
            this.chkAllEnvs.Text = "All Envs";
            this.toolTip1.SetToolTip(this.chkAllEnvs, "Apply to all environments");
            this.chkAllEnvs.UseVisualStyleBackColor = true;
            this.chkAllEnvs.Visible = false;
            // 
            // cbServiceInstances
            // 
            this.cbServiceInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServiceInstances.FormattingEnabled = true;
            this.cbServiceInstances.Location = new System.Drawing.Point(2, 3);
            this.cbServiceInstances.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbServiceInstances.Name = "cbServiceInstances";
            this.cbServiceInstances.Size = new System.Drawing.Size(26, 21);
            this.cbServiceInstances.TabIndex = 22;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.cbEnvs, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblInstances, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.nudInstances, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(82, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(173, 21);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // cbEnvs
            // 
            this.cbEnvs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnvs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnvs.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.cbEnvs.FormattingEnabled = true;
            this.cbEnvs.Location = new System.Drawing.Point(2, 2);
            this.cbEnvs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbEnvs.Name = "cbEnvs";
            this.cbEnvs.Size = new System.Drawing.Size(81, 21);
            this.cbEnvs.TabIndex = 1;
            this.cbEnvs.SelectedIndexChanged += new System.EventHandler(this.cbEnvs_SelectedIndexChanged);
            // 
            // lblInstances
            // 
            this.lblInstances.AutoSize = true;
            this.lblInstances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstances.Location = new System.Drawing.Point(87, 0);
            this.lblInstances.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInstances.Name = "lblInstances";
            this.lblInstances.Size = new System.Drawing.Size(53, 21);
            this.lblInstances.TabIndex = 2;
            this.lblInstances.Text = "Instances";
            this.lblInstances.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInstances.Visible = false;
            // 
            // nudInstances
            // 
            this.nudInstances.Location = new System.Drawing.Point(144, 2);
            this.nudInstances.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudInstances.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudInstances.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInstances.Name = "nudInstances";
            this.nudInstances.Size = new System.Drawing.Size(27, 20);
            this.nudInstances.TabIndex = 3;
            this.nudInstances.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInstances.Visible = false;
            this.nudInstances.ValueChanged += new System.EventHandler(this.nudInstances_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radTcpIp);
            this.panel1.Controls.Add(this.radFileSystem);
            this.panel1.Location = new System.Drawing.Point(82, 27);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(128, 21);
            this.panel1.TabIndex = 0;
            // 
            // radTcpIp
            // 
            this.radTcpIp.AutoSize = true;
            this.radTcpIp.BackColor = System.Drawing.Color.Transparent;
            this.radTcpIp.Dock = System.Windows.Forms.DockStyle.Left;
            this.radTcpIp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.radTcpIp.Location = new System.Drawing.Point(85, 0);
            this.radTcpIp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radTcpIp.Name = "radTcpIp";
            this.radTcpIp.Size = new System.Drawing.Size(68, 21);
            this.radTcpIp.TabIndex = 1;
            this.radTcpIp.TabStop = true;
            this.radTcpIp.Text = "TCP/IP";
            this.radTcpIp.UseVisualStyleBackColor = false;
            this.radTcpIp.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // radFileSystem
            // 
            this.radFileSystem.AutoSize = true;
            this.radFileSystem.BackColor = System.Drawing.Color.Transparent;
            this.radFileSystem.Checked = true;
            this.radFileSystem.Dock = System.Windows.Forms.DockStyle.Left;
            this.radFileSystem.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.radFileSystem.Location = new System.Drawing.Point(0, 0);
            this.radFileSystem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radFileSystem.Name = "radFileSystem";
            this.radFileSystem.Size = new System.Drawing.Size(85, 21);
            this.radFileSystem.TabIndex = 0;
            this.radFileSystem.TabStop = true;
            this.radFileSystem.Text = "File System";
            this.radFileSystem.UseVisualStyleBackColor = false;
            this.radFileSystem.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label1.Size = new System.Drawing.Size(76, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Environment";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(2, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 25);
            this.label5.TabIndex = 15;
            this.label5.Text = "Mode";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStatus.Location = new System.Drawing.Point(2, 50);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(76, 25);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(2, 75);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 26);
            this.label3.TabIndex = 19;
            this.label3.Text = "sre.env.xml";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpenSreEnv
            // 
            this.btnOpenSreEnv.Location = new System.Drawing.Point(82, 77);
            this.btnOpenSreEnv.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOpenSreEnv.Name = "btnOpenSreEnv";
            this.btnOpenSreEnv.Size = new System.Drawing.Size(67, 19);
            this.btnOpenSreEnv.TabIndex = 20;
            this.btnOpenSreEnv.Text = "&Open";
            this.btnOpenSreEnv.UseVisualStyleBackColor = true;
            this.btnOpenSreEnv.Click += new System.EventHandler(this.btnOpenSreEnv_Click);
            // 
            // rtfLog
            // 
            this.rtfLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfLog.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfLog.Location = new System.Drawing.Point(0, 101);
            this.rtfLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtfLog.Name = "rtfLog";
            this.rtfLog.ReadOnly = true;
            this.rtfLog.Size = new System.Drawing.Size(342, 188);
            this.rtfLog.TabIndex = 3;
            this.rtfLog.Text = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 32);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(659, 319);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(651, 293);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Environment";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pgSymplus);
            this.splitContainer1.Panel1.Controls.Add(this.pnlConfigPropTop);
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer1.Panel1.Controls.Add(this.pnlConfigPropBottom);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtfLog);
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(647, 289);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 5;
            // 
            // pgSymplus
            // 
            this.pgSymplus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSymplus.Location = new System.Drawing.Point(0, 120);
            this.pgSymplus.Name = "pgSymplus";
            this.pgSymplus.Size = new System.Drawing.Size(302, 143);
            this.pgSymplus.TabIndex = 6;
            // 
            // pnlConfigPropTop
            // 
            this.pnlConfigPropTop.Controls.Add(this.cbEnvConfigs);
            this.pnlConfigPropTop.Controls.Add(this.btnRefreshConfig);
            this.pnlConfigPropTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConfigPropTop.Location = new System.Drawing.Point(0, 101);
            this.pnlConfigPropTop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlConfigPropTop.Name = "pnlConfigPropTop";
            this.pnlConfigPropTop.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pnlConfigPropTop.Size = new System.Drawing.Size(302, 19);
            this.pnlConfigPropTop.TabIndex = 0;
            // 
            // cbEnvConfigs
            // 
            this.cbEnvConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnvConfigs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnvConfigs.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.cbEnvConfigs.FormattingEnabled = true;
            this.cbEnvConfigs.Location = new System.Drawing.Point(0, 1);
            this.cbEnvConfigs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbEnvConfigs.Name = "cbEnvConfigs";
            this.cbEnvConfigs.Size = new System.Drawing.Size(285, 21);
            this.cbEnvConfigs.TabIndex = 0;
            this.cbEnvConfigs.SelectedIndexChanged += new System.EventHandler(this.cbEnvConfigs_SelectedIndexChanged);
            // 
            // btnRefreshConfig
            // 
            this.btnRefreshConfig.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefreshConfig.Image = global::Eyedia.IDPE.Interface.Properties.Resources.refresh;
            this.btnRefreshConfig.Location = new System.Drawing.Point(285, 1);
            this.btnRefreshConfig.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRefreshConfig.Name = "btnRefreshConfig";
            this.btnRefreshConfig.Size = new System.Drawing.Size(17, 18);
            this.btnRefreshConfig.TabIndex = 1;
            this.btnRefreshConfig.UseVisualStyleBackColor = true;
            this.btnRefreshConfig.Click += new System.EventHandler(this.btnRefreshConfig_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.11527F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.88473F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblStatus, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.picServerStatus, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnOpenSreEnv, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(302, 101);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // picServerStatus
            // 
            this.picServerStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.picServerStatus.Location = new System.Drawing.Point(82, 52);
            this.picServerStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picServerStatus.Name = "picServerStatus";
            this.picServerStatus.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.picServerStatus.Size = new System.Drawing.Size(46, 21);
            this.picServerStatus.TabIndex = 17;
            this.picServerStatus.TabStop = false;
            this.toolTip1.SetToolTip(this.picServerStatus, "Click to Refresh");
            this.picServerStatus.Visible = false;
            this.picServerStatus.Click += new System.EventHandler(this.picServerStatus_Click);
            // 
            // pnlConfigPropBottom
            // 
            this.pnlConfigPropBottom.Controls.Add(this.lblConfigFileName);
            this.pnlConfigPropBottom.Controls.Add(this.btnSaveConfig);
            this.pnlConfigPropBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlConfigPropBottom.Location = new System.Drawing.Point(0, 263);
            this.pnlConfigPropBottom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlConfigPropBottom.Name = "pnlConfigPropBottom";
            this.pnlConfigPropBottom.Size = new System.Drawing.Size(302, 26);
            this.pnlConfigPropBottom.TabIndex = 1;
            // 
            // lblConfigFileName
            // 
            this.lblConfigFileName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblConfigFileName.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblConfigFileName.Location = new System.Drawing.Point(97, 0);
            this.lblConfigFileName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConfigFileName.Name = "lblConfigFileName";
            this.lblConfigFileName.Size = new System.Drawing.Size(205, 26);
            this.lblConfigFileName.TabIndex = 1;
            this.lblConfigFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(2, 2);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(67, 19);
            this.btnSaveConfig.TabIndex = 22;
            this.btnSaveConfig.Text = "&Save";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.environmentsControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Size = new System.Drawing.Size(651, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Manage";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // environmentsControl1
            // 
            this.environmentsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.environmentsControl1.Location = new System.Drawing.Point(2, 2);
            this.environmentsControl1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.environmentsControl1.Name = "environmentsControl1";
            this.environmentsControl1.Size = new System.Drawing.Size(647, 288);
            this.environmentsControl1.TabIndex = 0;
            // 
            // timerServerStatus
            // 
            this.timerServerStatus.Interval = 2000;
            this.timerServerStatus.Tick += new System.EventHandler(this.timerServerStatus_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timerDelayedCommand
            // 
            this.timerDelayedCommand.Interval = 1000;
            this.timerDelayedCommand.Tick += new System.EventHandler(this.timerDelayedCommand_Tick);
            // 
            // SreEnvironmentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 386);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SreEnvironmentWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Environments";
            this.Resize += new System.EventHandler(this.SreEnvironments_Resize);
            this.pnlBottom.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInstances)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlConfigPropTop.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picServerStatus)).EndInit();
            this.pnlConfigPropBottom.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ComboBox cbEnvs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCommands;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.ComboBox cbDataSources;
        private System.Windows.Forms.ComboBox cbRules;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDataSources;
        private System.Windows.Forms.Button btnDeployDataSource;
        private System.Windows.Forms.Button btnDeployRule;
        private System.Windows.Forms.RichTextBox rtfLog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private SreEnvironmentsControl environmentsControl1;
        private System.Windows.Forms.Label lblParam1;
        private System.Windows.Forms.ComboBox cbParam1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkIncludeSystemDS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radTcpIp;
        private System.Windows.Forms.RadioButton radFileSystem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox picServerStatus;
        private System.Windows.Forms.Timer timerServerStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbParam2;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpenSreEnv;
        private System.Windows.Forms.Panel pnlConfigPropBottom;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Panel pnlConfigPropTop;
        private System.Windows.Forms.PropertyGrid pgSymplus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox cbEnvConfigs;
        private System.Windows.Forms.Label lblConfigFileName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblInstances;
        private System.Windows.Forms.NumericUpDown nudInstances;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnRefreshConfig;
        private System.Windows.Forms.Timer timerDelayedCommand;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbServiceInstances;
        private System.Windows.Forms.CheckBox chkAllEnvs;
    }
}