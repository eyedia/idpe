namespace Eyedia.IDPE.Interface
{
    partial class SreEnvironmentsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbEnvs = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPullFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radTcpIp = new System.Windows.Forms.RadioButton();
            this.radFileSystem = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.tlpRemoteServers = new System.Windows.Forms.TableLayoutPanel();
            this.lblInstance2 = new System.Windows.Forms.Label();
            this.lblInstance1 = new System.Windows.Forms.Label();
            this.lblInstance4 = new System.Windows.Forms.Label();
            this.lblInstance3 = new System.Windows.Forms.Label();
            this.txtRemoteServer1 = new System.Windows.Forms.TextBox();
            this.txtRemoteServer2 = new System.Windows.Forms.TextBox();
            this.txtRemoteServer3 = new System.Windows.Forms.TextBox();
            this.txtRemoteServer4 = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnEnsureInstances = new System.Windows.Forms.Button();
            this.lblRemoteServers = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tlpRemoteServers.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbEnvs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.ContextMenuStrip = this.contextMenuStrip1;
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(909, 364);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.TabIndex = 0;
            // 
            // lbEnvs
            // 
            this.lbEnvs.ContextMenuStrip = this.contextMenuStrip1;
            this.lbEnvs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEnvs.FormattingEnabled = true;
            this.lbEnvs.ItemHeight = 20;
            this.lbEnvs.Location = new System.Drawing.Point(0, 0);
            this.lbEnvs.Name = "lbEnvs";
            this.lbEnvs.Size = new System.Drawing.Size(222, 364);
            this.lbEnvs.TabIndex = 0;
            this.lbEnvs.SelectedIndexChanged += new System.EventHandler(this.lbEnvs_SelectedIndexChanged);
            this.lbEnvs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbEnvs_KeyUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripMenuItem1,
            this.refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(157, 100);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPullFolder, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtRootFolder, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tlpRemoteServers, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.64774F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.64774F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.64774F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.64774F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.36556F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.04348F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(683, 319);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 33);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pull Folder";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Location = new System.Drawing.Point(163, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(518, 26);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtPullFolder
            // 
            this.txtPullFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPullFolder.Location = new System.Drawing.Point(163, 69);
            this.txtPullFolder.Name = "txtPullFolder";
            this.txtPullFolder.Size = new System.Drawing.Size(518, 26);
            this.txtPullFolder.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtPullFolder, "Global pull folder root location");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 33);
            this.label4.TabIndex = 6;
            this.label4.Text = "Default Mode";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radTcpIp);
            this.panel2.Controls.Add(this.radFileSystem);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(163, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(518, 27);
            this.panel2.TabIndex = 7;
            // 
            // radTcpIp
            // 
            this.radTcpIp.AutoSize = true;
            this.radTcpIp.Dock = System.Windows.Forms.DockStyle.Left;
            this.radTcpIp.Location = new System.Drawing.Point(116, 0);
            this.radTcpIp.Name = "radTcpIp";
            this.radTcpIp.Size = new System.Drawing.Size(83, 27);
            this.radTcpIp.TabIndex = 6;
            this.radTcpIp.TabStop = true;
            this.radTcpIp.Text = "TCP/IP";
            this.radTcpIp.UseVisualStyleBackColor = true;
            this.radTcpIp.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // radFileSystem
            // 
            this.radFileSystem.AutoSize = true;
            this.radFileSystem.Checked = true;
            this.radFileSystem.Dock = System.Windows.Forms.DockStyle.Left;
            this.radFileSystem.Location = new System.Drawing.Point(0, 0);
            this.radFileSystem.Name = "radFileSystem";
            this.radFileSystem.Size = new System.Drawing.Size(116, 27);
            this.radFileSystem.TabIndex = 5;
            this.radFileSystem.TabStop = true;
            this.radFileSystem.Text = "File System";
            this.radFileSystem.UseVisualStyleBackColor = true;
            this.radFileSystem.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 33);
            this.label5.TabIndex = 9;
            this.label5.Text = "Root Folder";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRootFolder.Location = new System.Drawing.Point(163, 36);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.Size = new System.Drawing.Size(518, 26);
            this.txtRootFolder.TabIndex = 2;
            // 
            // tlpRemoteServers
            // 
            this.tlpRemoteServers.ColumnCount = 2;
            this.tlpRemoteServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpRemoteServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpRemoteServers.Controls.Add(this.lblInstance2, 0, 1);
            this.tlpRemoteServers.Controls.Add(this.lblInstance1, 0, 0);
            this.tlpRemoteServers.Controls.Add(this.lblInstance4, 0, 3);
            this.tlpRemoteServers.Controls.Add(this.lblInstance3, 0, 2);
            this.tlpRemoteServers.Controls.Add(this.txtRemoteServer1, 1, 0);
            this.tlpRemoteServers.Controls.Add(this.txtRemoteServer2, 1, 1);
            this.tlpRemoteServers.Controls.Add(this.txtRemoteServer3, 1, 2);
            this.tlpRemoteServers.Controls.Add(this.txtRemoteServer4, 1, 3);
            this.tlpRemoteServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRemoteServers.Location = new System.Drawing.Point(163, 135);
            this.tlpRemoteServers.Name = "tlpRemoteServers";
            this.tlpRemoteServers.RowCount = 4;
            this.tlpRemoteServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpRemoteServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpRemoteServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpRemoteServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpRemoteServers.Size = new System.Drawing.Size(518, 135);
            this.tlpRemoteServers.TabIndex = 11;
            // 
            // lblInstance2
            // 
            this.lblInstance2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstance2.Location = new System.Drawing.Point(3, 33);
            this.lblInstance2.Name = "lblInstance2";
            this.lblInstance2.Size = new System.Drawing.Size(97, 33);
            this.lblInstance2.TabIndex = 9;
            this.lblInstance2.Text = "Instance 1";
            this.lblInstance2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInstance2.Visible = false;
            // 
            // lblInstance1
            // 
            this.lblInstance1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstance1.Location = new System.Drawing.Point(3, 0);
            this.lblInstance1.Name = "lblInstance1";
            this.lblInstance1.Size = new System.Drawing.Size(97, 33);
            this.lblInstance1.TabIndex = 7;
            this.lblInstance1.Text = "Instance 0";
            this.lblInstance1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInstance1.Visible = false;
            // 
            // lblInstance4
            // 
            this.lblInstance4.Location = new System.Drawing.Point(3, 99);
            this.lblInstance4.Name = "lblInstance4";
            this.lblInstance4.Size = new System.Drawing.Size(97, 33);
            this.lblInstance4.TabIndex = 10;
            this.lblInstance4.Text = "Instance 3";
            this.lblInstance4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInstance4.Visible = false;
            // 
            // lblInstance3
            // 
            this.lblInstance3.Location = new System.Drawing.Point(3, 66);
            this.lblInstance3.Name = "lblInstance3";
            this.lblInstance3.Size = new System.Drawing.Size(97, 33);
            this.lblInstance3.TabIndex = 8;
            this.lblInstance3.Text = "Instance 2";
            this.lblInstance3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInstance3.Visible = false;
            // 
            // txtRemoteServer1
            // 
            this.txtRemoteServer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemoteServer1.Location = new System.Drawing.Point(106, 3);
            this.txtRemoteServer1.Name = "txtRemoteServer1";
            this.txtRemoteServer1.Size = new System.Drawing.Size(409, 26);
            this.txtRemoteServer1.TabIndex = 11;
            this.txtRemoteServer1.Visible = false;
            // 
            // txtRemoteServer2
            // 
            this.txtRemoteServer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemoteServer2.Location = new System.Drawing.Point(106, 36);
            this.txtRemoteServer2.Name = "txtRemoteServer2";
            this.txtRemoteServer2.Size = new System.Drawing.Size(409, 26);
            this.txtRemoteServer2.TabIndex = 12;
            this.txtRemoteServer2.Visible = false;
            // 
            // txtRemoteServer3
            // 
            this.txtRemoteServer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemoteServer3.Location = new System.Drawing.Point(106, 69);
            this.txtRemoteServer3.Name = "txtRemoteServer3";
            this.txtRemoteServer3.Size = new System.Drawing.Size(409, 26);
            this.txtRemoteServer3.TabIndex = 13;
            this.txtRemoteServer3.Visible = false;
            // 
            // txtRemoteServer4
            // 
            this.txtRemoteServer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemoteServer4.Location = new System.Drawing.Point(106, 102);
            this.txtRemoteServer4.Name = "txtRemoteServer4";
            this.txtRemoteServer4.Size = new System.Drawing.Size(409, 26);
            this.txtRemoteServer4.TabIndex = 14;
            this.txtRemoteServer4.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(581, 276);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnEnsureInstances);
            this.panel3.Controls.Add(this.lblRemoteServers);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 135);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(154, 33);
            this.panel3.TabIndex = 12;
            // 
            // btnEnsureInstances
            // 
            this.btnEnsureInstances.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEnsureInstances.Image = global::Eyedia.IDPE.Interface.Properties.Resources.MarkValid;
            this.btnEnsureInstances.Location = new System.Drawing.Point(123, 0);
            this.btnEnsureInstances.Name = "btnEnsureInstances";
            this.btnEnsureInstances.Size = new System.Drawing.Size(31, 33);
            this.btnEnsureInstances.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnEnsureInstances, "Validate number of remote urls with number of instances");
            this.btnEnsureInstances.UseVisualStyleBackColor = true;
            this.btnEnsureInstances.Visible = false;
            this.btnEnsureInstances.Click += new System.EventHandler(this.btnEnsureInstances_Click);
            // 
            // lblRemoteServers
            // 
            this.lblRemoteServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemoteServers.Location = new System.Drawing.Point(0, 0);
            this.lblRemoteServers.Name = "lblRemoteServers";
            this.lblRemoteServers.Size = new System.Drawing.Size(154, 33);
            this.lblRemoteServers.TabIndex = 10;
            this.lblRemoteServers.Text = "Remote Servers";
            this.lblRemoteServers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 364);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(909, 46);
            this.panel1.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 8);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 35);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SreEnvironmentsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "SreEnvironmentsControl";
            this.Size = new System.Drawing.Size(909, 410);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tlpRemoteServers.ResumeLayout(false);
            this.tlpRemoteServers.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbEnvs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPullFolder;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radTcpIp;
        private System.Windows.Forms.RadioButton radFileSystem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblRemoteServers;
        private System.Windows.Forms.TableLayoutPanel tlpRemoteServers;
        private System.Windows.Forms.Label lblInstance2;
        private System.Windows.Forms.Label lblInstance1;
        private System.Windows.Forms.Label lblInstance4;
        private System.Windows.Forms.Label lblInstance3;
        private System.Windows.Forms.TextBox txtRemoteServer1;
        private System.Windows.Forms.TextBox txtRemoteServer2;
        private System.Windows.Forms.TextBox txtRemoteServer3;
        private System.Windows.Forms.TextBox txtRemoteServer4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnEnsureInstances;
    }
}
