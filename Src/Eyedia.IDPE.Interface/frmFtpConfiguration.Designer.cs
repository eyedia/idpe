namespace Eyedia.IDPE.Interface
{
    partial class frmFtpConfiguration
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpFTPFileSystem = new System.Windows.Forms.GroupBox();
            this.btnFtpTest = new System.Windows.Forms.Button();
            this.txtFtpPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFtpUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFtpRemoteLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpFTPFileSystem.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(258, 107);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(158, 107);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpFTPFileSystem
            // 
            this.grpFTPFileSystem.Controls.Add(this.btnFtpTest);
            this.grpFTPFileSystem.Controls.Add(this.txtFtpPassword);
            this.grpFTPFileSystem.Controls.Add(this.label3);
            this.grpFTPFileSystem.Controls.Add(this.txtFtpUserName);
            this.grpFTPFileSystem.Controls.Add(this.label2);
            this.grpFTPFileSystem.Controls.Add(this.txtFtpRemoteLocation);
            this.grpFTPFileSystem.Controls.Add(this.label1);
            this.grpFTPFileSystem.Location = new System.Drawing.Point(8, 3);
            this.grpFTPFileSystem.Name = "grpFTPFileSystem";
            this.grpFTPFileSystem.Size = new System.Drawing.Size(488, 98);
            this.grpFTPFileSystem.TabIndex = 4;
            this.grpFTPFileSystem.TabStop = false;
            // 
            // btnFtpTest
            // 
            this.btnFtpTest.Location = new System.Drawing.Point(370, 63);
            this.btnFtpTest.Name = "btnFtpTest";
            this.btnFtpTest.Size = new System.Drawing.Size(112, 23);
            this.btnFtpTest.TabIndex = 11;
            this.btnFtpTest.Text = "&Test Connection";
            this.btnFtpTest.UseVisualStyleBackColor = true;
            this.btnFtpTest.Click += new System.EventHandler(this.btnFtpTest_Click);
            // 
            // txtFtpPassword
            // 
            this.txtFtpPassword.Location = new System.Drawing.Point(101, 65);
            this.txtFtpPassword.Name = "txtFtpPassword";
            this.txtFtpPassword.Size = new System.Drawing.Size(124, 20);
            this.txtFtpPassword.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password";
            // 
            // txtFtpUserName
            // 
            this.txtFtpUserName.Location = new System.Drawing.Point(101, 39);
            this.txtFtpUserName.Name = "txtFtpUserName";
            this.txtFtpUserName.Size = new System.Drawing.Size(124, 20);
            this.txtFtpUserName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User Name";
            // 
            // txtFtpRemoteLocation
            // 
            this.txtFtpRemoteLocation.Location = new System.Drawing.Point(101, 13);
            this.txtFtpRemoteLocation.Name = "txtFtpRemoteLocation";
            this.txtFtpRemoteLocation.Size = new System.Drawing.Size(381, 20);
            this.txtFtpRemoteLocation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ftp Url";
            // 
            // frmFtpConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 136);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpFTPFileSystem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "frmFtpConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure FTP Connection";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmFtpConfiguration_KeyUp);
            this.grpFTPFileSystem.ResumeLayout(false);
            this.grpFTPFileSystem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpFTPFileSystem;
        private System.Windows.Forms.Button btnFtpTest;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFtpUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFtpRemoteLocation;
        private System.Windows.Forms.Label label1;
    }
}