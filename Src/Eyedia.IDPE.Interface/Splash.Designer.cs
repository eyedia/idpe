namespace Eyedia.IDPE.Interface
{
    partial class Splash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.lblMessage = new System.Windows.Forms.Label();
            this.timerLoad = new System.Windows.Forms.Timer(this.components);
            this.lblVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbSdfs = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.timerReset = new System.Windows.Forms.Timer(this.components);
            this.lblRecoveryMode = new System.Windows.Forms.Label();
            this.timerBlink = new System.Windows.Forms.Timer(this.components);
            this.pnlLoginBox = new System.Windows.Forms.TableLayoutPanel();
            this.cbUserName = new System.Windows.Forms.ComboBox();
            this.timerExit = new System.Windows.Forms.Timer(this.components);
            this.pnlLoginBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblMessage.Location = new System.Drawing.Point(490, 517);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(90, 20);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Initializing...";
            this.lblMessage.Visible = false;
            // 
            // timerLoad
            // 
            this.timerLoad.Interval = 200;
            this.timerLoad.Tick += new System.EventHandler(this.timerLoad_Tick);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblVersion.Location = new System.Drawing.Point(711, 15);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(72, 20);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Version -";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(4, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 37);
            this.label1.TabIndex = 3;
            this.label1.Text = "User";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPassword.Location = new System.Drawing.Point(96, 78);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(261, 26);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.TextChanged += new System.EventHandler(this.Validate);
            this.txtPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbUserName_KeyUp);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(4, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 37);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnOK.Location = new System.Drawing.Point(96, 115);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 31);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbSdfs
            // 
            this.cbSdfs.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbSdfs.DropDownWidth = 150;
            this.cbSdfs.FormattingEnabled = true;
            this.cbSdfs.Location = new System.Drawing.Point(96, 4);
            this.cbSdfs.Margin = new System.Windows.Forms.Padding(4);
            this.cbSdfs.Name = "cbSdfs";
            this.cbSdfs.Size = new System.Drawing.Size(261, 28);
            this.cbSdfs.TabIndex = 0;
            this.cbSdfs.Visible = false;
            this.cbSdfs.SelectedIndexChanged += new System.EventHandler(this.cmbInstances_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(4, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 37);
            this.label3.TabIndex = 6;
            this.label3.Text = "Instance";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Visible = false;
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorMessage.Location = new System.Drawing.Point(13, 379);
            this.lblErrorMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(355, 134);
            this.lblErrorMessage.TabIndex = 7;
            // 
            // timerReset
            // 
            this.timerReset.Interval = 7000;
            this.timerReset.Tick += new System.EventHandler(this.timerReset_Tick);
            // 
            // lblRecoveryMode
            // 
            this.lblRecoveryMode.AutoSize = true;
            this.lblRecoveryMode.BackColor = System.Drawing.Color.Transparent;
            this.lblRecoveryMode.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblRecoveryMode.Location = new System.Drawing.Point(801, 517);
            this.lblRecoveryMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecoveryMode.Name = "lblRecoveryMode";
            this.lblRecoveryMode.Size = new System.Drawing.Size(52, 20);
            this.lblRecoveryMode.TabIndex = 8;
            this.lblRecoveryMode.Text = "Done!";
            this.lblRecoveryMode.Visible = false;
            // 
            // timerBlink
            // 
            this.timerBlink.Interval = 500;
            this.timerBlink.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // pnlLoginBox
            // 
            this.pnlLoginBox.BackColor = System.Drawing.Color.Transparent;
            this.pnlLoginBox.ColumnCount = 2;
            this.pnlLoginBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.66138F));
            this.pnlLoginBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.33862F));
            this.pnlLoginBox.Controls.Add(this.btnOK, 1, 3);
            this.pnlLoginBox.Controls.Add(this.txtPassword, 1, 2);
            this.pnlLoginBox.Controls.Add(this.cbSdfs, 1, 0);
            this.pnlLoginBox.Controls.Add(this.label3, 0, 0);
            this.pnlLoginBox.Controls.Add(this.label1, 0, 1);
            this.pnlLoginBox.Controls.Add(this.label2, 0, 2);
            this.pnlLoginBox.Controls.Add(this.cbUserName, 1, 1);
            this.pnlLoginBox.Location = new System.Drawing.Point(492, 355);
            this.pnlLoginBox.Name = "pnlLoginBox";
            this.pnlLoginBox.RowCount = 4;
            this.pnlLoginBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlLoginBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlLoginBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlLoginBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlLoginBox.Size = new System.Drawing.Size(361, 150);
            this.pnlLoginBox.TabIndex = 10;
            // 
            // cbUserName
            // 
            this.cbUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbUserName.FormattingEnabled = true;
            this.cbUserName.Location = new System.Drawing.Point(95, 40);
            this.cbUserName.Name = "cbUserName";
            this.cbUserName.Size = new System.Drawing.Size(263, 28);
            this.cbUserName.TabIndex = 1;
            this.cbUserName.TextChanged += new System.EventHandler(this.Validate);
            this.cbUserName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbUserName_KeyUp);
            // 
            // timerExit
            // 
            this.timerExit.Interval = 1000;
            this.timerExit.Tick += new System.EventHandler(this.timerExit_Tick);
            // 
            // Splash
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(882, 542);
            this.Controls.Add(this.pnlLoginBox);
            this.Controls.Add(this.lblRecoveryMode);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblErrorMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Integrated Data Processing Environment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Splash_FormClosing);
            this.Load += new System.EventHandler(this.Splash_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Splash_KeyUp);
            this.pnlLoginBox.ResumeLayout(false);
            this.pnlLoginBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Timer timerLoad;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblErrorMessage;
        private System.Windows.Forms.Timer timerReset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSdfs;
        private System.Windows.Forms.Label lblRecoveryMode;
        private System.Windows.Forms.Timer timerBlink;
        private System.Windows.Forms.TableLayoutPanel pnlLoginBox;
        private System.Windows.Forms.ComboBox cbUserName;
        private System.Windows.Forms.Timer timerExit;
    }
}