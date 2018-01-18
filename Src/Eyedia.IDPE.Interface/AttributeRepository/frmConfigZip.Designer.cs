namespace Eyedia.IDPE.Interface
{
    partial class frmConfigZip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigZip));
            this.groupBoxHandler = new System.Windows.Forms.GroupBox();
            this.pnlZipHandlerCustom = new System.Windows.Forms.Panel();
            this.txtInterfaceNameZip = new System.Windows.Forms.TextBox();
            this.btnInterface = new System.Windows.Forms.Button();
            this.lblZipFileHandler = new System.Windows.Forms.Label();
            this.pnlZipHandlerDefault = new System.Windows.Forms.Panel();
            this.txtZipIgnoreFileList = new System.Windows.Forms.TextBox();
            this.chkZipIgnoreFiles = new System.Windows.Forms.CheckBox();
            this.chkZipIgnoreFilesButCopy = new System.Windows.Forms.CheckBox();
            this.txtZipIgnoreFileListButCopy = new System.Windows.Forms.TextBox();
            this.radZipHandlerCustom = new System.Windows.Forms.RadioButton();
            this.radZipHandlerDefault = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkZipDoNotCreateAcknoledgementInOutputFolder = new System.Windows.Forms.CheckBox();
            this.txtSortCustom = new System.Windows.Forms.TextBox();
            this.chkZipSort = new System.Windows.Forms.CheckBox();
            this.grpZipSort = new System.Windows.Forms.GroupBox();
            this.cmbZipSortType = new System.Windows.Forms.ComboBox();
            this.radZipSortCstm = new System.Windows.Forms.RadioButton();
            this.radZipSortStd = new System.Windows.Forms.RadioButton();
            this.groupBoxBasic = new System.Windows.Forms.GroupBox();
            this.pnlGap1 = new System.Windows.Forms.Panel();
            this.pnlGap2 = new System.Windows.Forms.Panel();
            this.groupBoxFile = new System.Windows.Forms.GroupBox();
            this.pnlCustomInterface = new System.Windows.Forms.Panel();
            this.txtFileInterfaceName = new System.Windows.Forms.TextBox();
            this.btnInterface2 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.chkRenameHeaders = new System.Windows.Forms.CheckBox();
            this.cmbDelmiter = new System.Windows.Forms.ComboBox();
            this.chkFileHasHeader = new System.Windows.Forms.CheckBox();
            this.btnConfigureDataFormat = new System.Windows.Forms.Button();
            this.cmbDataFormatTypes = new System.Windows.Forms.ComboBox();
            this.lblDelimiter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxMainBottom = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxHandler.SuspendLayout();
            this.pnlZipHandlerCustom.SuspendLayout();
            this.pnlZipHandlerDefault.SuspendLayout();
            this.grpZipSort.SuspendLayout();
            this.groupBoxBasic.SuspendLayout();
            this.groupBoxFile.SuspendLayout();
            this.pnlCustomInterface.SuspendLayout();
            this.groupBoxMainBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxHandler
            // 
            this.groupBoxHandler.Controls.Add(this.pnlZipHandlerCustom);
            this.groupBoxHandler.Controls.Add(this.pnlZipHandlerDefault);
            this.groupBoxHandler.Controls.Add(this.radZipHandlerCustom);
            this.groupBoxHandler.Controls.Add(this.radZipHandlerDefault);
            this.groupBoxHandler.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxHandler.Location = new System.Drawing.Point(0, 168);
            this.groupBoxHandler.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxHandler.Name = "groupBoxHandler";
            this.groupBoxHandler.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxHandler.Size = new System.Drawing.Size(1101, 168);
            this.groupBoxHandler.TabIndex = 0;
            this.groupBoxHandler.TabStop = false;
            this.groupBoxHandler.Text = "Zip Handler";
            // 
            // pnlZipHandlerCustom
            // 
            this.pnlZipHandlerCustom.Controls.Add(this.txtInterfaceNameZip);
            this.pnlZipHandlerCustom.Controls.Add(this.btnInterface);
            this.pnlZipHandlerCustom.Controls.Add(this.lblZipFileHandler);
            this.pnlZipHandlerCustom.Location = new System.Drawing.Point(14, 175);
            this.pnlZipHandlerCustom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlZipHandlerCustom.Name = "pnlZipHandlerCustom";
            this.pnlZipHandlerCustom.Size = new System.Drawing.Size(1072, 48);
            this.pnlZipHandlerCustom.TabIndex = 32;
            this.pnlZipHandlerCustom.Visible = false;
            // 
            // txtInterfaceNameZip
            // 
            this.txtInterfaceNameZip.Location = new System.Drawing.Point(126, 9);
            this.txtInterfaceNameZip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInterfaceNameZip.Name = "txtInterfaceNameZip";
            this.txtInterfaceNameZip.Size = new System.Drawing.Size(890, 26);
            this.txtInterfaceNameZip.TabIndex = 28;
            // 
            // btnInterface
            // 
            this.btnInterface.Location = new System.Drawing.Point(1032, 6);
            this.btnInterface.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInterface.Name = "btnInterface";
            this.btnInterface.Size = new System.Drawing.Size(38, 35);
            this.btnInterface.TabIndex = 29;
            this.btnInterface.Text = "...";
            this.btnInterface.UseVisualStyleBackColor = true;
            this.btnInterface.Click += new System.EventHandler(this.btnInterface_Click);
            // 
            // lblZipFileHandler
            // 
            this.lblZipFileHandler.AutoSize = true;
            this.lblZipFileHandler.Location = new System.Drawing.Point(2, 14);
            this.lblZipFileHandler.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZipFileHandler.Name = "lblZipFileHandler";
            this.lblZipFileHandler.Size = new System.Drawing.Size(115, 20);
            this.lblZipFileHandler.TabIndex = 30;
            this.lblZipFileHandler.Text = "Zip file Handler";
            // 
            // pnlZipHandlerDefault
            // 
            this.pnlZipHandlerDefault.Controls.Add(this.txtZipIgnoreFileList);
            this.pnlZipHandlerDefault.Controls.Add(this.chkZipIgnoreFiles);
            this.pnlZipHandlerDefault.Controls.Add(this.chkZipIgnoreFilesButCopy);
            this.pnlZipHandlerDefault.Controls.Add(this.txtZipIgnoreFileListButCopy);
            this.pnlZipHandlerDefault.Location = new System.Drawing.Point(14, 65);
            this.pnlZipHandlerDefault.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlZipHandlerDefault.Name = "pnlZipHandlerDefault";
            this.pnlZipHandlerDefault.Size = new System.Drawing.Size(1072, 102);
            this.pnlZipHandlerDefault.TabIndex = 31;
            // 
            // txtZipIgnoreFileList
            // 
            this.txtZipIgnoreFileList.Location = new System.Drawing.Point(393, 54);
            this.txtZipIgnoreFileList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtZipIgnoreFileList.Name = "txtZipIgnoreFileList";
            this.txtZipIgnoreFileList.Size = new System.Drawing.Size(646, 26);
            this.txtZipIgnoreFileList.TabIndex = 6;
            this.toolTip1.SetToolTip(this.txtZipIgnoreFileList, "Define extensions here to ignore (e.g. .xml|.txt)");
            this.txtZipIgnoreFileList.Visible = false;
            // 
            // chkZipIgnoreFiles
            // 
            this.chkZipIgnoreFiles.AutoSize = true;
            this.chkZipIgnoreFiles.Location = new System.Drawing.Point(0, 49);
            this.chkZipIgnoreFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkZipIgnoreFiles.Name = "chkZipIgnoreFiles";
            this.chkZipIgnoreFiles.Size = new System.Drawing.Size(361, 24);
            this.chkZipIgnoreFiles.TabIndex = 5;
            this.chkZipIgnoreFiles.Text = "&Ignore these file type(s)........................................";
            this.toolTip1.SetToolTip(this.chkZipIgnoreFiles, "IDPE will ignore these extensions (e.g. .dat|.txt)");
            this.chkZipIgnoreFiles.UseVisualStyleBackColor = true;
            this.chkZipIgnoreFiles.CheckedChanged += new System.EventHandler(this.chkZipIgnoreFiles_CheckedChanged);
            // 
            // chkZipIgnoreFilesButCopy
            // 
            this.chkZipIgnoreFilesButCopy.AutoSize = true;
            this.chkZipIgnoreFilesButCopy.Location = new System.Drawing.Point(0, 14);
            this.chkZipIgnoreFilesButCopy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkZipIgnoreFilesButCopy.Name = "chkZipIgnoreFilesButCopy";
            this.chkZipIgnoreFilesButCopy.Size = new System.Drawing.Size(381, 24);
            this.chkZipIgnoreFilesButCopy.TabIndex = 7;
            this.chkZipIgnoreFilesButCopy.Text = "Ignore these file type(s), but &copy to output folder";
            this.toolTip1.SetToolTip(this.chkZipIgnoreFilesButCopy, "IDPE will ignore these extensions (e.g. .dat|.txt), but will be copied to output f" +
        "older");
            this.chkZipIgnoreFilesButCopy.UseVisualStyleBackColor = true;
            this.chkZipIgnoreFilesButCopy.CheckedChanged += new System.EventHandler(this.chkZipIgnoreFilesButCopy_CheckedChanged);
            // 
            // txtZipIgnoreFileListButCopy
            // 
            this.txtZipIgnoreFileListButCopy.Location = new System.Drawing.Point(393, 14);
            this.txtZipIgnoreFileListButCopy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtZipIgnoreFileListButCopy.Name = "txtZipIgnoreFileListButCopy";
            this.txtZipIgnoreFileListButCopy.Size = new System.Drawing.Size(646, 26);
            this.txtZipIgnoreFileListButCopy.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txtZipIgnoreFileListButCopy, "Define extensions here to ignore but copy to output folder (e.g. .xml|.txt)");
            this.txtZipIgnoreFileListButCopy.Visible = false;
            // 
            // radZipHandlerCustom
            // 
            this.radZipHandlerCustom.AutoSize = true;
            this.radZipHandlerCustom.Location = new System.Drawing.Point(132, 29);
            this.radZipHandlerCustom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radZipHandlerCustom.Name = "radZipHandlerCustom";
            this.radZipHandlerCustom.Size = new System.Drawing.Size(89, 24);
            this.radZipHandlerCustom.TabIndex = 1;
            this.radZipHandlerCustom.TabStop = true;
            this.radZipHandlerCustom.Text = "&Custom";
            this.toolTip1.SetToolTip(this.radZipHandlerCustom, "Custom interface to handle all files, accept/reject file, make your own decision " +
        "to process files");
            this.radZipHandlerCustom.UseVisualStyleBackColor = true;
            this.radZipHandlerCustom.CheckedChanged += new System.EventHandler(this.ZipHandlerChanged);
            // 
            // radZipHandlerDefault
            // 
            this.radZipHandlerDefault.AutoSize = true;
            this.radZipHandlerDefault.Checked = true;
            this.radZipHandlerDefault.Location = new System.Drawing.Point(14, 29);
            this.radZipHandlerDefault.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radZipHandlerDefault.Name = "radZipHandlerDefault";
            this.radZipHandlerDefault.Size = new System.Drawing.Size(86, 24);
            this.radZipHandlerDefault.TabIndex = 0;
            this.radZipHandlerDefault.TabStop = true;
            this.radZipHandlerDefault.Text = "Default";
            this.toolTip1.SetToolTip(this.radZipHandlerDefault, "Built-in zip handler");
            this.radZipHandlerDefault.UseVisualStyleBackColor = true;
            this.radZipHandlerDefault.CheckedChanged += new System.EventHandler(this.ZipHandlerChanged);
            // 
            // chkZipDoNotCreateAcknoledgementInOutputFolder
            // 
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.AutoSize = true;
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.Location = new System.Drawing.Point(9, 108);
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.Name = "chkZipDoNotCreateAcknoledgementInOutputFolder";
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.Size = new System.Drawing.Size(278, 24);
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.TabIndex = 45;
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.Text = "Do not create acknoledgement file";
            this.toolTip1.SetToolTip(this.chkZipDoNotCreateAcknoledgementInOutputFolder, "IDPE by default creates a blank file same name as zip file to keep track of origin" +
        "al zip file in the output folder");
            this.chkZipDoNotCreateAcknoledgementInOutputFolder.UseVisualStyleBackColor = true;
            // 
            // txtSortCustom
            // 
            this.txtSortCustom.Enabled = false;
            this.txtSortCustom.Location = new System.Drawing.Point(352, 28);
            this.txtSortCustom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSortCustom.Name = "txtSortCustom";
            this.txtSortCustom.Size = new System.Drawing.Size(592, 26);
            this.txtSortCustom.TabIndex = 44;
            this.toolTip1.SetToolTip(this.txtSortCustom, "Define extensions here to sort (e.g. .xml|.txt)");
            // 
            // chkZipSort
            // 
            this.chkZipSort.AutoSize = true;
            this.chkZipSort.Location = new System.Drawing.Point(12, 48);
            this.chkZipSort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkZipSort.Name = "chkZipSort";
            this.chkZipSort.Size = new System.Drawing.Size(102, 24);
            this.chkZipSort.TabIndex = 46;
            this.chkZipSort.Text = "&Sort Files";
            this.toolTip1.SetToolTip(this.chkZipSort, "Sort zipped files before processing (based on extension)");
            this.chkZipSort.UseVisualStyleBackColor = true;
            this.chkZipSort.CheckedChanged += new System.EventHandler(this.chkZipSort_CheckedChanged);
            // 
            // grpZipSort
            // 
            this.grpZipSort.Controls.Add(this.txtSortCustom);
            this.grpZipSort.Controls.Add(this.cmbZipSortType);
            this.grpZipSort.Controls.Add(this.radZipSortCstm);
            this.grpZipSort.Controls.Add(this.radZipSortStd);
            this.grpZipSort.Location = new System.Drawing.Point(111, 14);
            this.grpZipSort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpZipSort.Name = "grpZipSort";
            this.grpZipSort.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpZipSort.Size = new System.Drawing.Size(972, 85);
            this.grpZipSort.TabIndex = 47;
            this.grpZipSort.TabStop = false;
            this.grpZipSort.Visible = false;
            // 
            // cmbZipSortType
            // 
            this.cmbZipSortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZipSortType.FormattingEnabled = true;
            this.cmbZipSortType.Items.AddRange(new object[] {
            "Ascending ",
            "Descending"});
            this.cmbZipSortType.Location = new System.Drawing.Point(106, 29);
            this.cmbZipSortType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbZipSortType.Name = "cmbZipSortType";
            this.cmbZipSortType.Size = new System.Drawing.Size(121, 28);
            this.cmbZipSortType.TabIndex = 41;
            // 
            // radZipSortCstm
            // 
            this.radZipSortCstm.AutoSize = true;
            this.radZipSortCstm.Location = new System.Drawing.Point(260, 31);
            this.radZipSortCstm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radZipSortCstm.Name = "radZipSortCstm";
            this.radZipSortCstm.Size = new System.Drawing.Size(89, 24);
            this.radZipSortCstm.TabIndex = 43;
            this.radZipSortCstm.Text = "Custom";
            this.radZipSortCstm.UseVisualStyleBackColor = true;
            this.radZipSortCstm.CheckedChanged += new System.EventHandler(this.zipSortTypeChanged);
            // 
            // radZipSortStd
            // 
            this.radZipSortStd.AutoSize = true;
            this.radZipSortStd.Checked = true;
            this.radZipSortStd.Location = new System.Drawing.Point(9, 32);
            this.radZipSortStd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radZipSortStd.Name = "radZipSortStd";
            this.radZipSortStd.Size = new System.Drawing.Size(100, 24);
            this.radZipSortStd.TabIndex = 42;
            this.radZipSortStd.TabStop = true;
            this.radZipSortStd.Text = "Standard";
            this.radZipSortStd.UseVisualStyleBackColor = true;
            this.radZipSortStd.CheckedChanged += new System.EventHandler(this.zipSortTypeChanged);
            // 
            // groupBoxBasic
            // 
            this.groupBoxBasic.Controls.Add(this.grpZipSort);
            this.groupBoxBasic.Controls.Add(this.chkZipDoNotCreateAcknoledgementInOutputFolder);
            this.groupBoxBasic.Controls.Add(this.chkZipSort);
            this.groupBoxBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasic.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxBasic.Location = new System.Drawing.Point(0, 0);
            this.groupBoxBasic.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBasic.Name = "groupBoxBasic";
            this.groupBoxBasic.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxBasic.Size = new System.Drawing.Size(1101, 145);
            this.groupBoxBasic.TabIndex = 48;
            this.groupBoxBasic.TabStop = false;
            this.groupBoxBasic.Text = "Basic";
            // 
            // pnlGap1
            // 
            this.pnlGap1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGap1.Location = new System.Drawing.Point(0, 145);
            this.pnlGap1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlGap1.Name = "pnlGap1";
            this.pnlGap1.Size = new System.Drawing.Size(1101, 23);
            this.pnlGap1.TabIndex = 49;
            // 
            // pnlGap2
            // 
            this.pnlGap2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGap2.Location = new System.Drawing.Point(0, 336);
            this.pnlGap2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlGap2.Name = "pnlGap2";
            this.pnlGap2.Size = new System.Drawing.Size(1101, 23);
            this.pnlGap2.TabIndex = 50;
            // 
            // groupBoxFile
            // 
            this.groupBoxFile.Controls.Add(this.pnlCustomInterface);
            this.groupBoxFile.Controls.Add(this.chkRenameHeaders);
            this.groupBoxFile.Controls.Add(this.cmbDelmiter);
            this.groupBoxFile.Controls.Add(this.chkFileHasHeader);
            this.groupBoxFile.Controls.Add(this.btnConfigureDataFormat);
            this.groupBoxFile.Controls.Add(this.cmbDataFormatTypes);
            this.groupBoxFile.Controls.Add(this.lblDelimiter);
            this.groupBoxFile.Controls.Add(this.label1);
            this.groupBoxFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxFile.Location = new System.Drawing.Point(0, 359);
            this.groupBoxFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFile.Name = "groupBoxFile";
            this.groupBoxFile.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFile.Size = new System.Drawing.Size(1101, 163);
            this.groupBoxFile.TabIndex = 51;
            this.groupBoxFile.TabStop = false;
            this.groupBoxFile.Text = "Data Files";
            // 
            // pnlCustomInterface
            // 
            this.pnlCustomInterface.Controls.Add(this.txtFileInterfaceName);
            this.pnlCustomInterface.Controls.Add(this.btnInterface2);
            this.pnlCustomInterface.Controls.Add(this.label20);
            this.pnlCustomInterface.Location = new System.Drawing.Point(14, 68);
            this.pnlCustomInterface.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCustomInterface.Name = "pnlCustomInterface";
            this.pnlCustomInterface.Size = new System.Drawing.Size(1080, 46);
            this.pnlCustomInterface.TabIndex = 32;
            this.pnlCustomInterface.Visible = false;
            // 
            // txtFileInterfaceName
            // 
            this.txtFileInterfaceName.Location = new System.Drawing.Point(176, 9);
            this.txtFileInterfaceName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFileInterfaceName.Name = "txtFileInterfaceName";
            this.txtFileInterfaceName.Size = new System.Drawing.Size(842, 26);
            this.txtFileInterfaceName.TabIndex = 29;
            // 
            // btnInterface2
            // 
            this.btnInterface2.Location = new System.Drawing.Point(1032, 6);
            this.btnInterface2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInterface2.Name = "btnInterface2";
            this.btnInterface2.Size = new System.Drawing.Size(38, 35);
            this.btnInterface2.TabIndex = 30;
            this.btnInterface2.Text = "...";
            this.btnInterface2.UseVisualStyleBackColor = true;
            this.btnInterface2.Click += new System.EventHandler(this.btnInterface2_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(-3, 12);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(111, 20);
            this.label20.TabIndex = 31;
            this.label20.Text = "Interface Type";
            // 
            // chkRenameHeaders
            // 
            this.chkRenameHeaders.AutoSize = true;
            this.chkRenameHeaders.Enabled = false;
            this.chkRenameHeaders.Location = new System.Drawing.Point(922, 37);
            this.chkRenameHeaders.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkRenameHeaders.Name = "chkRenameHeaders";
            this.chkRenameHeaders.Size = new System.Drawing.Size(161, 24);
            this.chkRenameHeaders.TabIndex = 16;
            this.chkRenameHeaders.Text = "&Rename Headers";
            this.chkRenameHeaders.UseVisualStyleBackColor = true;
            this.chkRenameHeaders.Visible = false;
            // 
            // cmbDelmiter
            // 
            this.cmbDelmiter.FormattingEnabled = true;
            this.cmbDelmiter.Location = new System.Drawing.Point(696, 31);
            this.cmbDelmiter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDelmiter.Name = "cmbDelmiter";
            this.cmbDelmiter.Size = new System.Drawing.Size(64, 28);
            this.cmbDelmiter.TabIndex = 14;
            this.cmbDelmiter.Visible = false;
            // 
            // chkFileHasHeader
            // 
            this.chkFileHasHeader.AutoSize = true;
            this.chkFileHasHeader.Location = new System.Drawing.Point(784, 37);
            this.chkFileHasHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkFileHasHeader.Name = "chkFileHasHeader";
            this.chkFileHasHeader.Size = new System.Drawing.Size(121, 24);
            this.chkFileHasHeader.TabIndex = 15;
            this.chkFileHasHeader.Text = "Has Header";
            this.chkFileHasHeader.UseVisualStyleBackColor = true;
            this.chkFileHasHeader.Visible = false;
            this.chkFileHasHeader.CheckedChanged += new System.EventHandler(this.chkFileHasHeader_CheckedChanged);
            // 
            // btnConfigureDataFormat
            // 
            this.btnConfigureDataFormat.Location = new System.Drawing.Point(405, 28);
            this.btnConfigureDataFormat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConfigureDataFormat.Name = "btnConfigureDataFormat";
            this.btnConfigureDataFormat.Size = new System.Drawing.Size(156, 37);
            this.btnConfigureDataFormat.TabIndex = 12;
            this.btnConfigureDataFormat.Text = "C&onfigure";
            this.btnConfigureDataFormat.UseVisualStyleBackColor = true;
            this.btnConfigureDataFormat.Visible = false;
            this.btnConfigureDataFormat.Click += new System.EventHandler(this.btnConfigureDataFormat_Click);
            // 
            // cmbDataFormatTypes
            // 
            this.cmbDataFormatTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataFormatTypes.FormattingEnabled = true;
            this.cmbDataFormatTypes.Location = new System.Drawing.Point(189, 31);
            this.cmbDataFormatTypes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDataFormatTypes.Name = "cmbDataFormatTypes";
            this.cmbDataFormatTypes.Size = new System.Drawing.Size(205, 28);
            this.cmbDataFormatTypes.TabIndex = 11;
            this.cmbDataFormatTypes.SelectedIndexChanged += new System.EventHandler(this.cmbDataFormatTypes_SelectedIndexChanged);
            // 
            // lblDelimiter
            // 
            this.lblDelimiter.AutoSize = true;
            this.lblDelimiter.Location = new System.Drawing.Point(610, 37);
            this.lblDelimiter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDelimiter.Name = "lblDelimiter";
            this.lblDelimiter.Size = new System.Drawing.Size(71, 20);
            this.lblDelimiter.TabIndex = 13;
            this.lblDelimiter.Text = "Delimiter";
            this.lblDelimiter.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Each file format will be";
            // 
            // groupBoxMainBottom
            // 
            this.groupBoxMainBottom.Controls.Add(this.btnCancel);
            this.groupBoxMainBottom.Controls.Add(this.btnSave);
            this.groupBoxMainBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxMainBottom.Location = new System.Drawing.Point(0, 474);
            this.groupBoxMainBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxMainBottom.Name = "groupBoxMainBottom";
            this.groupBoxMainBottom.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxMainBottom.Size = new System.Drawing.Size(1101, 77);
            this.groupBoxMainBottom.TabIndex = 52;
            this.groupBoxMainBottom.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(560, 25);
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
            this.btnSave.Location = new System.Drawing.Point(411, 25);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 37);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmConfigZip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 551);
            this.Controls.Add(this.groupBoxMainBottom);
            this.Controls.Add(this.groupBoxFile);
            this.Controls.Add(this.pnlGap2);
            this.Controls.Add(this.groupBoxHandler);
            this.Controls.Add(this.pnlGap1);
            this.Controls.Add(this.groupBoxBasic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmConfigZip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Zip Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfigZip_FormClosed);
            this.groupBoxHandler.ResumeLayout(false);
            this.groupBoxHandler.PerformLayout();
            this.pnlZipHandlerCustom.ResumeLayout(false);
            this.pnlZipHandlerCustom.PerformLayout();
            this.pnlZipHandlerDefault.ResumeLayout(false);
            this.pnlZipHandlerDefault.PerformLayout();
            this.grpZipSort.ResumeLayout(false);
            this.grpZipSort.PerformLayout();
            this.groupBoxBasic.ResumeLayout(false);
            this.groupBoxBasic.PerformLayout();
            this.groupBoxFile.ResumeLayout(false);
            this.groupBoxFile.PerformLayout();
            this.pnlCustomInterface.ResumeLayout(false);
            this.pnlCustomInterface.PerformLayout();
            this.groupBoxMainBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxHandler;
        private System.Windows.Forms.TextBox txtZipIgnoreFileListButCopy;
        private System.Windows.Forms.CheckBox chkZipIgnoreFilesButCopy;
        private System.Windows.Forms.TextBox txtZipIgnoreFileList;
        private System.Windows.Forms.CheckBox chkZipIgnoreFiles;
        private System.Windows.Forms.RadioButton radZipHandlerCustom;
        private System.Windows.Forms.RadioButton radZipHandlerDefault;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkZipDoNotCreateAcknoledgementInOutputFolder;
        private System.Windows.Forms.GroupBox grpZipSort;
        private System.Windows.Forms.TextBox txtSortCustom;
        private System.Windows.Forms.ComboBox cmbZipSortType;
        private System.Windows.Forms.RadioButton radZipSortCstm;
        private System.Windows.Forms.RadioButton radZipSortStd;
        private System.Windows.Forms.CheckBox chkZipSort;
        private System.Windows.Forms.GroupBox groupBoxBasic;
        private System.Windows.Forms.Label lblZipFileHandler;
        private System.Windows.Forms.TextBox txtInterfaceNameZip;
        private System.Windows.Forms.Button btnInterface;
        private System.Windows.Forms.Panel pnlZipHandlerCustom;
        private System.Windows.Forms.Panel pnlZipHandlerDefault;
        private System.Windows.Forms.Panel pnlGap1;
        private System.Windows.Forms.Panel pnlGap2;
        private System.Windows.Forms.GroupBox groupBoxFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxMainBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkRenameHeaders;
        private System.Windows.Forms.ComboBox cmbDelmiter;
        private System.Windows.Forms.CheckBox chkFileHasHeader;
        private System.Windows.Forms.Button btnConfigureDataFormat;
        private System.Windows.Forms.ComboBox cmbDataFormatTypes;
        private System.Windows.Forms.Label lblDelimiter;
        private System.Windows.Forms.Button btnInterface2;
        private System.Windows.Forms.TextBox txtFileInterfaceName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Panel pnlCustomInterface;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}