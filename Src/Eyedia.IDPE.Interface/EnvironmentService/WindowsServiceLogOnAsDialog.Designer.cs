namespace Eyedia.IDPE.Interface
{
    partial class WindowsServiceLogOnAsDialog
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
            this.radLocalSystemAccount = new System.Windows.Forms.RadioButton();
            this.radThisAccount = new System.Windows.Forms.RadioButton();
            this.chkAllowServiceToInteractWithDesktop = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPasswordConfirm = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radLocalSystemAccount
            // 
            this.radLocalSystemAccount.AutoSize = true;
            this.radLocalSystemAccount.Enabled = false;
            this.radLocalSystemAccount.Location = new System.Drawing.Point(33, 12);
            this.radLocalSystemAccount.Name = "radLocalSystemAccount";
            this.radLocalSystemAccount.Size = new System.Drawing.Size(192, 24);
            this.radLocalSystemAccount.TabIndex = 0;
            this.radLocalSystemAccount.Text = "Local System Account";
            this.radLocalSystemAccount.UseVisualStyleBackColor = true;
            // 
            // radThisAccount
            // 
            this.radThisAccount.AutoSize = true;
            this.radThisAccount.Checked = true;
            this.radThisAccount.Location = new System.Drawing.Point(33, 87);
            this.radThisAccount.Name = "radThisAccount";
            this.radThisAccount.Size = new System.Drawing.Size(130, 24);
            this.radThisAccount.TabIndex = 2;
            this.radThisAccount.TabStop = true;
            this.radThisAccount.Text = "This Account:";
            this.radThisAccount.UseVisualStyleBackColor = true;
            // 
            // chkAllowServiceToInteractWithDesktop
            // 
            this.chkAllowServiceToInteractWithDesktop.AutoSize = true;
            this.chkAllowServiceToInteractWithDesktop.Enabled = false;
            this.chkAllowServiceToInteractWithDesktop.Location = new System.Drawing.Point(63, 42);
            this.chkAllowServiceToInteractWithDesktop.Name = "chkAllowServiceToInteractWithDesktop";
            this.chkAllowServiceToInteractWithDesktop.Size = new System.Drawing.Size(293, 24);
            this.chkAllowServiceToInteractWithDesktop.TabIndex = 1;
            this.chkAllowServiceToInteractWithDesktop.Text = "Allow service to interact with desktop";
            this.chkAllowServiceToInteractWithDesktop.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Confirm Password:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(200, 87);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(323, 26);
            this.txtUserName.TabIndex = 3;
            // 
            // txtPasswordConfirm
            // 
            this.txtPasswordConfirm.Location = new System.Drawing.Point(200, 161);
            this.txtPasswordConfirm.Name = "txtPasswordConfirm";
            this.txtPasswordConfirm.PasswordChar = '*';
            this.txtPasswordConfirm.Size = new System.Drawing.Size(323, 26);
            this.txtPasswordConfirm.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(200, 123);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(323, 26);
            this.txtPassword.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOK.Location = new System.Drawing.Point(150, 19);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&Generate";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnOK.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnOK_KeyUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(538, 61);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(271, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // WindowsServiceLogOnAsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 284);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtPasswordConfirm);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkAllowServiceToInteractWithDesktop);
            this.Controls.Add(this.radThisAccount);
            this.Controls.Add(this.radLocalSystemAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WindowsServiceLogOnAsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Service Log On Properties";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radLocalSystemAccount;
        private System.Windows.Forms.RadioButton radThisAccount;
        private System.Windows.Forms.CheckBox chkAllowServiceToInteractWithDesktop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPasswordConfirm;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
    }
}