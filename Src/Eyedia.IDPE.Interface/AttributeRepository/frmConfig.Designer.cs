namespace Eyedia.IDPE.Interface
{
    partial class frmConfig
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpFTPFileSystem = new System.Windows.Forms.GroupBox();
            this.btnFtpTest = new System.Windows.Forms.Button();
            this.cbFilter1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numUpDnInterval = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFtpPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFtpUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFtpRemoteLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpLocalFileSystem = new System.Windows.Forms.GroupBox();
            this.radUseSpecificPull = new System.Windows.Forms.RadioButton();
            this.radUseGlobalPull = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSpecificOutFolder = new System.Windows.Forms.TextBox();
            this.btnFolderBrowser3 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnFolderBrowser2 = new System.Windows.Forms.Button();
            this.txtSpecificPullFolder = new System.Windows.Forms.TextBox();
            this.btnFolderBrowser1 = new System.Windows.Forms.Button();
            this.chkAutoArchive = new System.Windows.Forms.CheckBox();
            this.txtSpecificArchFolder = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cbFilter2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.filterTypeErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtInterfaceNameGeneric = new System.Windows.Forms.TextBox();
            this.chkFirstRowIsHeader2 = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.grpCustom = new System.Windows.Forms.GroupBox();
            this.btnInterface2 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.grpFTPFileSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDnInterval)).BeginInit();
            this.grpLocalFileSystem.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterTypeErrorProvider)).BeginInit();
            this.grpCustom.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 479);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1538, 30);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoToolTip = true;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1515, 25);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Ready";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpFTPFileSystem
            // 
            this.grpFTPFileSystem.Controls.Add(this.btnFtpTest);
            this.grpFTPFileSystem.Controls.Add(this.cbFilter1);
            this.grpFTPFileSystem.Controls.Add(this.label6);
            this.grpFTPFileSystem.Controls.Add(this.label5);
            this.grpFTPFileSystem.Controls.Add(this.numUpDnInterval);
            this.grpFTPFileSystem.Controls.Add(this.label4);
            this.grpFTPFileSystem.Controls.Add(this.txtFtpPassword);
            this.grpFTPFileSystem.Controls.Add(this.label3);
            this.grpFTPFileSystem.Controls.Add(this.txtFtpUserName);
            this.grpFTPFileSystem.Controls.Add(this.label2);
            this.grpFTPFileSystem.Controls.Add(this.txtFtpRemoteLocation);
            this.grpFTPFileSystem.Controls.Add(this.label1);
            this.grpFTPFileSystem.Location = new System.Drawing.Point(8, -2);
            this.grpFTPFileSystem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpFTPFileSystem.Name = "grpFTPFileSystem";
            this.grpFTPFileSystem.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpFTPFileSystem.Size = new System.Drawing.Size(732, 192);
            this.grpFTPFileSystem.TabIndex = 1;
            this.grpFTPFileSystem.TabStop = false;
            // 
            // btnFtpTest
            // 
            this.btnFtpTest.Location = new System.Drawing.Point(555, 132);
            this.btnFtpTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFtpTest.Name = "btnFtpTest";
            this.btnFtpTest.Size = new System.Drawing.Size(168, 35);
            this.btnFtpTest.TabIndex = 11;
            this.btnFtpTest.Text = "&Test Connection";
            this.btnFtpTest.UseVisualStyleBackColor = true;
            this.btnFtpTest.Click += new System.EventHandler(this.btnFtpTest_Click);
            // 
            // cbFilter1
            // 
            this.cbFilter1.FormattingEnabled = true;
            this.cbFilter1.Location = new System.Drawing.Point(555, 18);
            this.cbFilter1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbFilter1.Name = "cbFilter1";
            this.cbFilter1.Size = new System.Drawing.Size(166, 28);
            this.cbFilter1.TabIndex = 10;
            this.toolTip.SetToolTip(this.cbFilter1, "Please include only extension like .txt, for multiple filters use |(pipe) sign, l" +
        "ike .txt|.dat");
            this.cbFilter1.Validated += new System.EventHandler(this.ValidateFilter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(494, 23);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "Filter";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 148);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Minute(s)";
            // 
            // numUpDnInterval
            // 
            this.numUpDnInterval.Location = new System.Drawing.Point(152, 142);
            this.numUpDnInterval.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numUpDnInterval.Name = "numUpDnInterval";
            this.numUpDnInterval.Size = new System.Drawing.Size(102, 26);
            this.numUpDnInterval.TabIndex = 7;
            this.numUpDnInterval.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 148);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Interval";
            // 
            // txtFtpPassword
            // 
            this.txtFtpPassword.Location = new System.Drawing.Point(152, 100);
            this.txtFtpPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFtpPassword.Name = "txtFtpPassword";
            this.txtFtpPassword.Size = new System.Drawing.Size(184, 26);
            this.txtFtpPassword.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password";
            // 
            // txtFtpUserName
            // 
            this.txtFtpUserName.Location = new System.Drawing.Point(152, 60);
            this.txtFtpUserName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFtpUserName.Name = "txtFtpUserName";
            this.txtFtpUserName.Size = new System.Drawing.Size(184, 26);
            this.txtFtpUserName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "User Name";
            // 
            // txtFtpRemoteLocation
            // 
            this.txtFtpRemoteLocation.Location = new System.Drawing.Point(152, 20);
            this.txtFtpRemoteLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFtpRemoteLocation.Name = "txtFtpRemoteLocation";
            this.txtFtpRemoteLocation.Size = new System.Drawing.Size(295, 26);
            this.txtFtpRemoteLocation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ftp Url";
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(237, 225);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 35);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(390, 225);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpLocalFileSystem
            // 
            this.grpLocalFileSystem.Controls.Add(this.radUseSpecificPull);
            this.grpLocalFileSystem.Controls.Add(this.radUseGlobalPull);
            this.grpLocalFileSystem.Controls.Add(this.groupBox1);
            this.grpLocalFileSystem.Controls.Add(this.cbFilter2);
            this.grpLocalFileSystem.Controls.Add(this.label7);
            this.grpLocalFileSystem.Location = new System.Drawing.Point(753, 8);
            this.grpLocalFileSystem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpLocalFileSystem.Name = "grpLocalFileSystem";
            this.grpLocalFileSystem.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpLocalFileSystem.Size = new System.Drawing.Size(732, 203);
            this.grpLocalFileSystem.TabIndex = 4;
            this.grpLocalFileSystem.TabStop = false;
            // 
            // radUseSpecificPull
            // 
            this.radUseSpecificPull.AutoSize = true;
            this.radUseSpecificPull.Location = new System.Drawing.Point(206, 25);
            this.radUseSpecificPull.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radUseSpecificPull.Name = "radUseSpecificPull";
            this.radUseSpecificPull.Size = new System.Drawing.Size(152, 24);
            this.radUseSpecificPull.TabIndex = 27;
            this.radUseSpecificPull.TabStop = true;
            this.radUseSpecificPull.Text = "Use Specific Pull";
            this.radUseSpecificPull.UseVisualStyleBackColor = true;
            this.radUseSpecificPull.CheckedChanged += new System.EventHandler(this.LocalPullTypeChanged);
            // 
            // radUseGlobalPull
            // 
            this.radUseGlobalPull.AutoSize = true;
            this.radUseGlobalPull.Checked = true;
            this.radUseGlobalPull.Location = new System.Drawing.Point(16, 25);
            this.radUseGlobalPull.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radUseGlobalPull.Name = "radUseGlobalPull";
            this.radUseGlobalPull.Size = new System.Drawing.Size(142, 24);
            this.radUseGlobalPull.TabIndex = 26;
            this.radUseGlobalPull.TabStop = true;
            this.radUseGlobalPull.Text = "Use &Global Pull";
            this.radUseGlobalPull.UseVisualStyleBackColor = true;
            this.radUseGlobalPull.CheckedChanged += new System.EventHandler(this.LocalPullTypeChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSpecificOutFolder);
            this.groupBox1.Controls.Add(this.btnFolderBrowser3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnFolderBrowser2);
            this.groupBox1.Controls.Add(this.txtSpecificPullFolder);
            this.groupBox1.Controls.Add(this.btnFolderBrowser1);
            this.groupBox1.Controls.Add(this.chkAutoArchive);
            this.groupBox1.Controls.Add(this.txtSpecificArchFolder);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(4, 57);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(718, 140);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            // 
            // txtSpecificOutFolder
            // 
            this.txtSpecificOutFolder.Location = new System.Drawing.Point(201, 102);
            this.txtSpecificOutFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSpecificOutFolder.Name = "txtSpecificOutFolder";
            this.txtSpecificOutFolder.Size = new System.Drawing.Size(469, 26);
            this.txtSpecificOutFolder.TabIndex = 21;
            // 
            // btnFolderBrowser3
            // 
            this.btnFolderBrowser3.Location = new System.Drawing.Point(675, 98);
            this.btnFolderBrowser3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFolderBrowser3.Name = "btnFolderBrowser3";
            this.btnFolderBrowser3.Size = new System.Drawing.Size(38, 35);
            this.btnFolderBrowser3.TabIndex = 24;
            this.btnFolderBrowser3.Text = "...";
            this.btnFolderBrowser3.UseVisualStyleBackColor = true;
            this.btnFolderBrowser3.Click += new System.EventHandler(this.btnFolderBrowser3_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 23);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "Pull Folder";
            // 
            // btnFolderBrowser2
            // 
            this.btnFolderBrowser2.Location = new System.Drawing.Point(675, 58);
            this.btnFolderBrowser2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFolderBrowser2.Name = "btnFolderBrowser2";
            this.btnFolderBrowser2.Size = new System.Drawing.Size(38, 35);
            this.btnFolderBrowser2.TabIndex = 23;
            this.btnFolderBrowser2.Text = "...";
            this.btnFolderBrowser2.UseVisualStyleBackColor = true;
            this.btnFolderBrowser2.Click += new System.EventHandler(this.btnFolderBrowser2_Click);
            // 
            // txtSpecificPullFolder
            // 
            this.txtSpecificPullFolder.Location = new System.Drawing.Point(201, 18);
            this.txtSpecificPullFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSpecificPullFolder.Name = "txtSpecificPullFolder";
            this.txtSpecificPullFolder.Size = new System.Drawing.Size(469, 26);
            this.txtSpecificPullFolder.TabIndex = 17;
            // 
            // btnFolderBrowser1
            // 
            this.btnFolderBrowser1.Location = new System.Drawing.Point(675, 15);
            this.btnFolderBrowser1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFolderBrowser1.Name = "btnFolderBrowser1";
            this.btnFolderBrowser1.Size = new System.Drawing.Size(38, 35);
            this.btnFolderBrowser1.TabIndex = 22;
            this.btnFolderBrowser1.Text = "...";
            this.btnFolderBrowser1.UseVisualStyleBackColor = true;
            this.btnFolderBrowser1.Click += new System.EventHandler(this.btnFolderBrowser1_Click);
            // 
            // chkAutoArchive
            // 
            this.chkAutoArchive.AutoSize = true;
            this.chkAutoArchive.Checked = true;
            this.chkAutoArchive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoArchive.Location = new System.Drawing.Point(9, 58);
            this.chkAutoArchive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAutoArchive.Name = "chkAutoArchive";
            this.chkAutoArchive.Size = new System.Drawing.Size(174, 24);
            this.chkAutoArchive.TabIndex = 18;
            this.chkAutoArchive.Text = "Auto Archive Folder";
            this.chkAutoArchive.UseVisualStyleBackColor = true;
            this.chkAutoArchive.CheckedChanged += new System.EventHandler(this.chkAutoArchive_CheckedChanged);
            // 
            // txtSpecificArchFolder
            // 
            this.txtSpecificArchFolder.Location = new System.Drawing.Point(201, 58);
            this.txtSpecificArchFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSpecificArchFolder.Name = "txtSpecificArchFolder";
            this.txtSpecificArchFolder.Size = new System.Drawing.Size(469, 26);
            this.txtSpecificArchFolder.TabIndex = 19;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(9, 106);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(84, 20);
            this.label21.TabIndex = 20;
            this.label21.Text = "Out Folder";
            // 
            // cbFilter2
            // 
            this.cbFilter2.FormattingEnabled = true;
            this.cbFilter2.Location = new System.Drawing.Point(552, 22);
            this.cbFilter2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbFilter2.Name = "cbFilter2";
            this.cbFilter2.Size = new System.Drawing.Size(166, 28);
            this.cbFilter2.TabIndex = 14;
            this.toolTip.SetToolTip(this.cbFilter2, "Please include only extension like .txt, for multiple filters use |(pipe) sign, l" +
        "ike .txt|.dat");
            this.cbFilter2.Validated += new System.EventHandler(this.ValidateFilter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(490, 26);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "Filter";
            // 
            // filterTypeErrorProvider
            // 
            this.filterTypeErrorProvider.ContainerControl = this;
            // 
            // txtInterfaceNameGeneric
            // 
            this.txtInterfaceNameGeneric.Location = new System.Drawing.Point(165, 25);
            this.txtInterfaceNameGeneric.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInterfaceNameGeneric.Name = "txtInterfaceNameGeneric";
            this.txtInterfaceNameGeneric.Size = new System.Drawing.Size(510, 26);
            this.txtInterfaceNameGeneric.TabIndex = 26;
            this.toolTip.SetToolTip(this.txtInterfaceNameGeneric, "Implementation of InputFileGenerator");
            this.txtInterfaceNameGeneric.TextChanged += new System.EventHandler(this.txtInterfaceNameGeneric_TextChanged);
            // 
            // chkFirstRowIsHeader2
            // 
            this.chkFirstRowIsHeader2.AutoSize = true;
            this.chkFirstRowIsHeader2.Location = new System.Drawing.Point(165, 65);
            this.chkFirstRowIsHeader2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkFirstRowIsHeader2.Name = "chkFirstRowIsHeader2";
            this.chkFirstRowIsHeader2.Size = new System.Drawing.Size(195, 24);
            this.chkFirstRowIsHeader2.TabIndex = 29;
            this.chkFirstRowIsHeader2.Text = "First row will be header";
            this.toolTip.SetToolTip(this.chkFirstRowIsHeader2, "Even though the interface will take care complete data feed, but you need to let " +
        "IDPE know if the first row contains the header(column names)");
            this.chkFirstRowIsHeader2.UseVisualStyleBackColor = false;
            // 
            // grpCustom
            // 
            this.grpCustom.Controls.Add(this.chkFirstRowIsHeader2);
            this.grpCustom.Controls.Add(this.btnInterface2);
            this.grpCustom.Controls.Add(this.txtInterfaceNameGeneric);
            this.grpCustom.Controls.Add(this.label20);
            this.grpCustom.Location = new System.Drawing.Point(753, 240);
            this.grpCustom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCustom.Name = "grpCustom";
            this.grpCustom.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCustom.Size = new System.Drawing.Size(732, 106);
            this.grpCustom.TabIndex = 7;
            this.grpCustom.TabStop = false;
            // 
            // btnInterface2
            // 
            this.btnInterface2.Location = new System.Drawing.Point(686, 22);
            this.btnInterface2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInterface2.Name = "btnInterface2";
            this.btnInterface2.Size = new System.Drawing.Size(38, 35);
            this.btnInterface2.TabIndex = 27;
            this.btnInterface2.Text = "...";
            this.btnInterface2.UseVisualStyleBackColor = true;
            this.btnInterface2.Click += new System.EventHandler(this.btnInterface_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 29);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(111, 20);
            this.label20.TabIndex = 28;
            this.label20.Text = "Interface Type";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1538, 509);
            this.Controls.Add(this.grpLocalFileSystem);
            this.Controls.Add(this.grpCustom);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpFTPFileSystem);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure FTP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConfigFtp_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpFTPFileSystem.ResumeLayout(false);
            this.grpFTPFileSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDnInterval)).EndInit();
            this.grpLocalFileSystem.ResumeLayout(false);
            this.grpLocalFileSystem.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterTypeErrorProvider)).EndInit();
            this.grpCustom.ResumeLayout(false);
            this.grpCustom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox grpFTPFileSystem;
        private System.Windows.Forms.TextBox txtFtpRemoteLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numUpDnInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFtpUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpLocalFileSystem;
        private System.Windows.Forms.ComboBox cbFilter1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbFilter2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ErrorProvider filterTypeErrorProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox grpCustom;
        private System.Windows.Forms.CheckBox chkFirstRowIsHeader2;
        private System.Windows.Forms.Button btnInterface2;
        private System.Windows.Forms.TextBox txtInterfaceNameGeneric;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtSpecificPullFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnFolderBrowser3;
        private System.Windows.Forms.Button btnFolderBrowser2;
        private System.Windows.Forms.Button btnFolderBrowser1;
        private System.Windows.Forms.TextBox txtSpecificOutFolder;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtSpecificArchFolder;
        private System.Windows.Forms.CheckBox chkAutoArchive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radUseSpecificPull;
        private System.Windows.Forms.RadioButton radUseGlobalPull;
        private System.Windows.Forms.Button btnFtpTest;
    }
}