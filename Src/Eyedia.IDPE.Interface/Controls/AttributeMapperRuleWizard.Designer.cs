namespace Eyedia.IDPE.Interface
{
    partial class AttributeMapperRuleWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttributeMapperRuleWizard));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlPreparing = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnlHelp = new System.Windows.Forms.Panel();
            this.lblTips = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlPreparing.SuspendLayout();
            this.pnlHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(232, 104);
            this.pnlMain.TabIndex = 3;
            // 
            // pnlPreparing
            // 
            this.pnlPreparing.AutoScroll = true;
            this.pnlPreparing.Controls.Add(this.lblMessage);
            this.pnlPreparing.Location = new System.Drawing.Point(339, 5);
            this.pnlPreparing.Name = "pnlPreparing";
            this.pnlPreparing.Size = new System.Drawing.Size(212, 116);
            this.pnlPreparing.TabIndex = 4;
            this.pnlPreparing.Visible = false;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.Control;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(212, 116);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Preparing...";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlHelp
            // 
            this.pnlHelp.AutoScroll = true;
            this.pnlHelp.Controls.Add(this.lblTips);
            this.pnlHelp.Controls.Add(this.pictureBox1);
            this.pnlHelp.Location = new System.Drawing.Point(3, 127);
            this.pnlHelp.Name = "pnlHelp";
            this.pnlHelp.Padding = new System.Windows.Forms.Padding(0, 10, 30, 0);
            this.pnlHelp.Size = new System.Drawing.Size(422, 89);
            this.pnlHelp.TabIndex = 5;
            this.pnlHelp.Visible = false;
            // 
            // lblTips
            // 
            this.lblTips.BackColor = System.Drawing.SystemColors.Info;
            this.lblTips.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTips.Location = new System.Drawing.Point(42, 10);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(350, 46);
            this.lblTips.TabIndex = 2;
            this.lblTips.Text = resources.GetString("lblTips.Text");
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Eyedia.IDPE.Interface.Properties.Resources.info;
            this.pictureBox1.Location = new System.Drawing.Point(0, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // AttributeMapperRuleWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlHelp);
            this.Controls.Add(this.pnlPreparing);
            this.Controls.Add(this.pnlMain);
            this.Name = "AttributeMapperRuleWizard";
            this.Size = new System.Drawing.Size(780, 255);
            this.pnlPreparing.ResumeLayout(false);
            this.pnlHelp.ResumeLayout(false);
            this.pnlHelp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlPreparing;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Panel pnlHelp;
        private System.Windows.Forms.Label lblTips;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
