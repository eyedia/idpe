namespace Eyedia.IDPE.Interface.Controls
{
    partial class SreEmails
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlEmailTextBoxes = new System.Windows.Forms.Panel();
            this.txtProcessVariableName = new System.Windows.Forms.TextBox();
            this.chkIncludeAttachmentOthers = new System.Windows.Forms.CheckBox();
            this.chkIncludeAttachmentOutput = new System.Windows.Forms.CheckBox();
            this.chkIncludeAttachmentInput = new System.Windows.Forms.CheckBox();
            this.chkSendEmailAfterFileProcessed = new System.Windows.Forms.CheckBox();
            this.txtBcc = new System.Windows.Forms.TextBox();
            this.txtCc = new System.Windows.Forms.TextBox();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlMain.SuspendLayout();
            this.pnlEmailTextBoxes.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(687, 23);
            this.pnlTop.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 332);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(687, 54);
            this.pnlBottom.TabIndex = 1;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlEmailTextBoxes);
            this.pnlMain.Controls.Add(this.pnlLeft);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 23);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(687, 309);
            this.pnlMain.TabIndex = 2;
            // 
            // pnlEmailTextBoxes
            // 
            this.pnlEmailTextBoxes.Controls.Add(this.txtProcessVariableName);
            this.pnlEmailTextBoxes.Controls.Add(this.chkIncludeAttachmentOthers);
            this.pnlEmailTextBoxes.Controls.Add(this.chkIncludeAttachmentOutput);
            this.pnlEmailTextBoxes.Controls.Add(this.chkIncludeAttachmentInput);
            this.pnlEmailTextBoxes.Controls.Add(this.chkSendEmailAfterFileProcessed);
            this.pnlEmailTextBoxes.Controls.Add(this.txtBcc);
            this.pnlEmailTextBoxes.Controls.Add(this.txtCc);
            this.pnlEmailTextBoxes.Controls.Add(this.txtTo);
            this.pnlEmailTextBoxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEmailTextBoxes.Location = new System.Drawing.Point(62, 0);
            this.pnlEmailTextBoxes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlEmailTextBoxes.Name = "pnlEmailTextBoxes";
            this.pnlEmailTextBoxes.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlEmailTextBoxes.Size = new System.Drawing.Size(625, 309);
            this.pnlEmailTextBoxes.TabIndex = 1;
            // 
            // txtProcessVariableName
            // 
            this.txtProcessVariableName.Enabled = false;
            this.txtProcessVariableName.Location = new System.Drawing.Point(379, 210);
            this.txtProcessVariableName.Name = "txtProcessVariableName";
            this.txtProcessVariableName.Size = new System.Drawing.Size(239, 26);
            this.txtProcessVariableName.TabIndex = 7;
            this.txtProcessVariableName.Visible = false;
            // 
            // chkIncludeAttachmentOthers
            // 
            this.chkIncludeAttachmentOthers.AutoSize = true;
            this.chkIncludeAttachmentOthers.Enabled = false;
            this.chkIncludeAttachmentOthers.Location = new System.Drawing.Point(58, 210);
            this.chkIncludeAttachmentOthers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkIncludeAttachmentOthers.Name = "chkIncludeAttachmentOthers";
            this.chkIncludeAttachmentOthers.Size = new System.Drawing.Size(314, 24);
            this.chkIncludeAttachmentOthers.TabIndex = 6;
            this.chkIncludeAttachmentOthers.Text = "Include other files from process variable";
            this.toolTip1.SetToolTip(this.chkIncludeAttachmentOthers, "Include other files from process variable");
            this.chkIncludeAttachmentOthers.UseVisualStyleBackColor = true;
            this.chkIncludeAttachmentOthers.Visible = false;
            // 
            // chkIncludeAttachmentOutput
            // 
            this.chkIncludeAttachmentOutput.AutoSize = true;
            this.chkIncludeAttachmentOutput.Enabled = false;
            this.chkIncludeAttachmentOutput.Location = new System.Drawing.Point(58, 176);
            this.chkIncludeAttachmentOutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkIncludeAttachmentOutput.Name = "chkIncludeAttachmentOutput";
            this.chkIncludeAttachmentOutput.Size = new System.Drawing.Size(267, 24);
            this.chkIncludeAttachmentOutput.TabIndex = 5;
            this.chkIncludeAttachmentOutput.Text = "Include output file as attachment";
            this.toolTip1.SetToolTip(this.chkIncludeAttachmentOutput, "Include output file as attachment");
            this.chkIncludeAttachmentOutput.UseVisualStyleBackColor = true;
            // 
            // chkIncludeAttachmentInput
            // 
            this.chkIncludeAttachmentInput.AutoSize = true;
            this.chkIncludeAttachmentInput.Enabled = false;
            this.chkIncludeAttachmentInput.Location = new System.Drawing.Point(58, 142);
            this.chkIncludeAttachmentInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkIncludeAttachmentInput.Name = "chkIncludeAttachmentInput";
            this.chkIncludeAttachmentInput.Size = new System.Drawing.Size(256, 24);
            this.chkIncludeAttachmentInput.TabIndex = 4;
            this.chkIncludeAttachmentInput.Text = "Include input file as attachment";
            this.toolTip1.SetToolTip(this.chkIncludeAttachmentInput, "Include input file as attachment");
            this.chkIncludeAttachmentInput.UseVisualStyleBackColor = true;
            this.chkIncludeAttachmentInput.CheckedChanged += new System.EventHandler(this.chkIncludeAttachment_CheckedChanged);
            // 
            // chkSendEmailAfterFileProcessed
            // 
            this.chkSendEmailAfterFileProcessed.AutoSize = true;
            this.chkSendEmailAfterFileProcessed.Location = new System.Drawing.Point(4, 106);
            this.chkSendEmailAfterFileProcessed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSendEmailAfterFileProcessed.Name = "chkSendEmailAfterFileProcessed";
            this.chkSendEmailAfterFileProcessed.Size = new System.Drawing.Size(360, 24);
            this.chkSendEmailAfterFileProcessed.TabIndex = 3;
            this.chkSendEmailAfterFileProcessed.Text = "Send email when a file processed successfully";
            this.toolTip1.SetToolTip(this.chkSendEmailAfterFileProcessed, "Send email when a file processed successfully");
            this.chkSendEmailAfterFileProcessed.UseVisualStyleBackColor = true;
            this.chkSendEmailAfterFileProcessed.CheckedChanged += new System.EventHandler(this.chkSendEmailAfterFileProcessed_CheckedChanged);
            // 
            // txtBcc
            // 
            this.txtBcc.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBcc.Location = new System.Drawing.Point(4, 57);
            this.txtBcc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBcc.Name = "txtBcc";
            this.txtBcc.Size = new System.Drawing.Size(617, 26);
            this.txtBcc.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtBcc, "Comma separated email ids");
            this.txtBcc.TextChanged += new System.EventHandler(this.IsValidEmailAddress);
            this.txtBcc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SreEmails_KeyUp);
            // 
            // txtCc
            // 
            this.txtCc.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCc.Location = new System.Drawing.Point(4, 31);
            this.txtCc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCc.Name = "txtCc";
            this.txtCc.Size = new System.Drawing.Size(617, 26);
            this.txtCc.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtCc, "Comma separated email ids");
            this.txtCc.TextChanged += new System.EventHandler(this.IsValidEmailAddress);
            this.txtCc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SreEmails_KeyUp);
            // 
            // txtTo
            // 
            this.txtTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTo.Location = new System.Drawing.Point(4, 5);
            this.txtTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTo.Name = "txtTo";
            this.txtTo.ReadOnly = true;
            this.txtTo.Size = new System.Drawing.Size(617, 26);
            this.txtTo.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.label3);
            this.pnlLeft.Controls.Add(this.label2);
            this.pnlLeft.Controls.Add(this.label1);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(62, 309);
            this.pnlLeft.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 74);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bcc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cc";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "To";
            // 
            // SreEmails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SreEmails";
            this.Size = new System.Drawing.Size(687, 386);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SreEmails_KeyUp);
            this.pnlMain.ResumeLayout(false);
            this.pnlEmailTextBoxes.ResumeLayout(false);
            this.pnlEmailTextBoxes.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlEmailTextBoxes;
        private System.Windows.Forms.TextBox txtBcc;
        private System.Windows.Forms.TextBox txtCc;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSendEmailAfterFileProcessed;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkIncludeAttachmentInput;
        private System.Windows.Forms.CheckBox chkIncludeAttachmentOutput;
        private System.Windows.Forms.CheckBox chkIncludeAttachmentOthers;
        private System.Windows.Forms.TextBox txtProcessVariableName;

    }
}
