namespace Eyedia.IDPE.Interface
{
    partial class ListOfValues
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvCodes = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportImportFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvSets = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblId = new System.Windows.Forms.Label();
            this.txtReferenceKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWarningMsg2 = new System.Windows.Forms.Label();
            this.lblWarningMsg = new System.Windows.Forms.Label();
            this.btnNewCode = new System.Windows.Forms.Button();
            this.nmPosition = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEnumCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timerBlink = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 781);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1592, 30);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(61, 25);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1592, 58);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 58);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvCodes);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvSets);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1592, 723);
            this.splitContainer1.SplitterDistance = 624;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.TabStop = false;
            // 
            // lvCodes
            // 
            this.lvCodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvCodes.ContextMenuStrip = this.contextMenuStrip1;
            this.lvCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvCodes.FullRowSelect = true;
            this.lvCodes.Location = new System.Drawing.Point(0, 0);
            this.lvCodes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvCodes.Name = "lvCodes";
            this.lvCodes.Size = new System.Drawing.Size(624, 723);
            this.lvCodes.TabIndex = 8;
            this.lvCodes.UseCompatibleStateImageBehavior = false;
            this.lvCodes.View = System.Windows.Forms.View.Details;
            this.lvCodes.SelectedIndexChanged += new System.EventHandler(this.lvCodes_SelectedIndexChanged);
            this.lvCodes.Resize += new System.EventHandler(this.lvCodes_Resize);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Codes";
            this.columnHeader1.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exportImportFormatToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(258, 124);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(257, 30);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(257, 30);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(257, 30);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportImportFormatToolStripMenuItem
            // 
            this.exportImportFormatToolStripMenuItem.Name = "exportImportFormatToolStripMenuItem";
            this.exportImportFormatToolStripMenuItem.Size = new System.Drawing.Size(257, 30);
            this.exportImportFormatToolStripMenuItem.Text = "Export Import Format";
            this.exportImportFormatToolStripMenuItem.Click += new System.EventHandler(this.exportImportFormatToolStripMenuItem_Click);
            // 
            // lvSets
            // 
            this.lvSets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader5});
            this.lvSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSets.FullRowSelect = true;
            this.lvSets.Location = new System.Drawing.Point(0, 0);
            this.lvSets.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvSets.Name = "lvSets";
            this.lvSets.Size = new System.Drawing.Size(962, 392);
            this.lvSets.TabIndex = 9;
            this.lvSets.UseCompatibleStateImageBehavior = false;
            this.lvSets.View = System.Windows.Forms.View.Details;
            this.lvSets.SelectedIndexChanged += new System.EventHandler(this.lvSets_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Enum Code";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            this.columnHeader3.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Position";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Reference Key";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Description";
            this.columnHeader5.Width = 200;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblId);
            this.panel1.Controls.Add(this.txtReferenceKey);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblWarningMsg2);
            this.panel1.Controls.Add(this.lblWarningMsg);
            this.panel1.Controls.Add(this.btnNewCode);
            this.panel1.Controls.Add(this.nmPosition);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtValue);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtEnumCode);
            this.panel1.Controls.Add(this.lblCode);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 392);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(962, 331);
            this.panel1.TabIndex = 0;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(862, 198);
            this.lblId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(23, 20);
            this.lblId.TabIndex = 19;
            this.lblId.Text = "Id";
            this.lblId.Visible = false;
            // 
            // txtReferenceKey
            // 
            this.txtReferenceKey.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtReferenceKey.Location = new System.Drawing.Point(140, 126);
            this.txtReferenceKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtReferenceKey.Name = "txtReferenceKey";
            this.txtReferenceKey.Size = new System.Drawing.Size(326, 26);
            this.txtReferenceKey.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 129);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 18;
            this.label1.Text = "Reference Key";
            // 
            // lblWarningMsg2
            // 
            this.lblWarningMsg2.AutoSize = true;
            this.lblWarningMsg2.BackColor = System.Drawing.SystemColors.Info;
            this.lblWarningMsg2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarningMsg2.Location = new System.Drawing.Point(681, 88);
            this.lblWarningMsg2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWarningMsg2.Name = "lblWarningMsg2";
            this.lblWarningMsg2.Size = new System.Drawing.Size(110, 20);
            this.lblWarningMsg2.TabIndex = 16;
            this.lblWarningMsg2.Text = "CURRENCY";
            this.lblWarningMsg2.Visible = false;
            // 
            // lblWarningMsg
            // 
            this.lblWarningMsg.AutoSize = true;
            this.lblWarningMsg.BackColor = System.Drawing.SystemColors.Info;
            this.lblWarningMsg.Location = new System.Drawing.Point(508, 88);
            this.lblWarningMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWarningMsg.Name = "lblWarningMsg";
            this.lblWarningMsg.Size = new System.Drawing.Size(173, 20);
            this.lblWarningMsg.TabIndex = 15;
            this.lblWarningMsg.Text = "You are entering a new";
            this.lblWarningMsg.Visible = false;
            // 
            // btnNewCode
            // 
            this.btnNewCode.Location = new System.Drawing.Point(490, 124);
            this.btnNewCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNewCode.Name = "btnNewCode";
            this.btnNewCode.Size = new System.Drawing.Size(100, 35);
            this.btnNewCode.TabIndex = 5;
            this.btnNewCode.Text = "&New Code";
            this.btnNewCode.UseVisualStyleBackColor = true;
            this.btnNewCode.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // nmPosition
            // 
            this.nmPosition.Location = new System.Drawing.Point(140, 283);
            this.nmPosition.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nmPosition.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nmPosition.Name = "nmPosition";
            this.nmPosition.Size = new System.Drawing.Size(68, 26);
            this.nmPosition.TabIndex = 4;
            this.nmPosition.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 282);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Position";
            // 
            // txtCode
            // 
            this.txtCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCode.Location = new System.Drawing.Point(490, 43);
            this.txtCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(310, 26);
            this.txtCode.TabIndex = 0;
            this.txtCode.Visible = false;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(595, 124);
            this.btnNew.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(100, 35);
            this.btnNew.TabIndex = 6;
            this.btnNew.Text = "&New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(700, 124);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtDescription.Location = new System.Drawing.Point(140, 169);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDescription.MaxLength = 255;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(660, 101);
            this.txtDescription.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 166);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "Description";
            // 
            // txtValue
            // 
            this.txtValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtValue.Location = new System.Drawing.Point(140, 86);
            this.txtValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(326, 26);
            this.txtValue.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 89);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Value";
            // 
            // txtEnumCode
            // 
            this.txtEnumCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtEnumCode.Location = new System.Drawing.Point(140, 43);
            this.txtEnumCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtEnumCode.Name = "txtEnumCode";
            this.txtEnumCode.Size = new System.Drawing.Size(326, 26);
            this.txtEnumCode.TabIndex = 1;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCode.Location = new System.Drawing.Point(140, 15);
            this.lblCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(100, 20);
            this.lblCode.TabIndex = 5;
            this.lblCode.Text = "CURRENCY";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 52);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Enum Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Code";
            // 
            // timerBlink
            // 
            this.timerBlink.Interval = 500;
            this.timerBlink.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // ListOfValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1592, 811);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ListOfValues";
            this.Text = "List of Values(LOV)";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ListOfValues_KeyUp);
            this.Resize += new System.EventHandler(this.ListOfValues_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPosition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvCodes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lvSets;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEnumCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.NumericUpDown nmPosition;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnNewCode;
        private System.Windows.Forms.Label lblWarningMsg2;
        private System.Windows.Forms.Label lblWarningMsg;
        private System.Windows.Forms.Timer timerBlink;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox txtReferenceKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ToolStripMenuItem exportImportFormatToolStripMenuItem;
    }
}