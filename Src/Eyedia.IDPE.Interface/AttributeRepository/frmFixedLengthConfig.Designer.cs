namespace Symplus.RuleEngine.Utilities
{
    partial class frmFixedLengthConfig
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fixedLengthSchemaGenerator1 = new Symplus.RuleEngine.Utilities.FixedLengthSchemaGenerator();
            this.grpFooter = new System.Windows.Forms.GroupBox();
            this.chkHasFooter = new System.Windows.Forms.CheckBox();
            this.cmbAttributeFooter = new System.Windows.Forms.ComboBox();
            this.grpHeader = new System.Windows.Forms.GroupBox();
            this.chkHasHeader = new System.Windows.Forms.CheckBox();
            this.cmbAttributeHeader = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpFooter.SuspendLayout();
            this.grpHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 331);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(712, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 285);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 46);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(368, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(258, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fixedLengthSchemaGenerator1);
            this.panel1.Controls.Add(this.grpFooter);
            this.panel1.Controls.Add(this.grpHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(712, 285);
            this.panel1.TabIndex = 3;
            // 
            // fixedLengthSchemaGenerator1
            // 
            this.fixedLengthSchemaGenerator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fixedLengthSchemaGenerator1.Location = new System.Drawing.Point(0, 51);
            this.fixedLengthSchemaGenerator1.Name = "fixedLengthSchemaGenerator1";
            this.fixedLengthSchemaGenerator1.Schema = "[{0}]\r\nColNameHeader=false\r\nFormat=FixedLength\r\nDateTimeFormat=yyyymmdd\r\n";
            this.fixedLengthSchemaGenerator1.Size = new System.Drawing.Size(712, 187);
            this.fixedLengthSchemaGenerator1.TabIndex = 4;
            // 
            // grpFooter
            // 
            this.grpFooter.Controls.Add(this.chkHasFooter);
            this.grpFooter.Controls.Add(this.cmbAttributeFooter);
            this.grpFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpFooter.Location = new System.Drawing.Point(0, 238);
            this.grpFooter.Name = "grpFooter";
            this.grpFooter.Size = new System.Drawing.Size(712, 47);
            this.grpFooter.TabIndex = 3;
            this.grpFooter.TabStop = false;
            // 
            // chkHasFooter
            // 
            this.chkHasFooter.AutoSize = true;
            this.chkHasFooter.Checked = true;
            this.chkHasFooter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHasFooter.Location = new System.Drawing.Point(12, 21);
            this.chkHasFooter.Name = "chkHasFooter";
            this.chkHasFooter.Size = new System.Drawing.Size(152, 17);
            this.chkHasFooter.TabIndex = 4;
            this.chkHasFooter.Text = "Has &Footer and mapped to";
            this.chkHasFooter.UseVisualStyleBackColor = true;
            this.chkHasFooter.CheckedChanged += new System.EventHandler(this.chkHasFooter_CheckedChanged);
            // 
            // cmbAttributeFooter
            // 
            this.cmbAttributeFooter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttributeFooter.FormattingEnabled = true;
            this.cmbAttributeFooter.Location = new System.Drawing.Point(213, 19);
            this.cmbAttributeFooter.Name = "cmbAttributeFooter";
            this.cmbAttributeFooter.Size = new System.Drawing.Size(277, 21);
            this.cmbAttributeFooter.TabIndex = 2;
            // 
            // grpHeader
            // 
            this.grpHeader.Controls.Add(this.chkHasHeader);
            this.grpHeader.Controls.Add(this.cmbAttributeHeader);
            this.grpHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpHeader.Location = new System.Drawing.Point(0, 0);
            this.grpHeader.Name = "grpHeader";
            this.grpHeader.Size = new System.Drawing.Size(712, 51);
            this.grpHeader.TabIndex = 2;
            this.grpHeader.TabStop = false;
            // 
            // chkHasHeader
            // 
            this.chkHasHeader.AutoSize = true;
            this.chkHasHeader.Checked = true;
            this.chkHasHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHasHeader.Location = new System.Drawing.Point(12, 20);
            this.chkHasHeader.Name = "chkHasHeader";
            this.chkHasHeader.Size = new System.Drawing.Size(157, 17);
            this.chkHasHeader.TabIndex = 3;
            this.chkHasHeader.Text = "Has &Header and mapped to";
            this.chkHasHeader.UseVisualStyleBackColor = true;
            this.chkHasHeader.CheckedChanged += new System.EventHandler(this.chkHasHeader_CheckedChanged);
            // 
            // cmbAttributeHeader
            // 
            this.cmbAttributeHeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttributeHeader.FormattingEnabled = true;
            this.cmbAttributeHeader.Location = new System.Drawing.Point(213, 16);
            this.cmbAttributeHeader.Name = "cmbAttributeHeader";
            this.cmbAttributeHeader.Size = new System.Drawing.Size(277, 21);
            this.cmbAttributeHeader.TabIndex = 0;
            // 
            // frmFixedLengthConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 353);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmFixedLengthConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmParam";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grpFooter.ResumeLayout(false);
            this.grpFooter.PerformLayout();
            this.grpHeader.ResumeLayout(false);
            this.grpHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpFooter;
        private System.Windows.Forms.GroupBox grpHeader;
        private System.Windows.Forms.ComboBox cmbAttributeFooter;
        private System.Windows.Forms.ComboBox cmbAttributeHeader;
        private System.Windows.Forms.CheckBox chkHasFooter;
        private System.Windows.Forms.CheckBox chkHasHeader;
        private FixedLengthSchemaGenerator fixedLengthSchemaGenerator1;
    }
}