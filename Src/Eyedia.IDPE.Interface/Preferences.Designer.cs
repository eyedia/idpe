namespace Eyedia.IDPE.Interface
{
    partial class Preferences
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComparisonTool = new System.Windows.Forms.TextBox();
            this.btnComparisonTool = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkEnableServiceCheck = new System.Windows.Forms.CheckBox();
            this.sreDatabases1 = new Eyedia.IDPE.Interface.Controls.SreDatabases();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 276);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(727, 45);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(326, 15);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Comparison tool";
            // 
            // txtComparisonTool
            // 
            this.txtComparisonTool.Location = new System.Drawing.Point(100, 6);
            this.txtComparisonTool.Name = "txtComparisonTool";
            this.txtComparisonTool.Size = new System.Drawing.Size(421, 20);
            this.txtComparisonTool.TabIndex = 3;
            // 
            // btnComparisonTool
            // 
            this.btnComparisonTool.Location = new System.Drawing.Point(525, 6);
            this.btnComparisonTool.Name = "btnComparisonTool";
            this.btnComparisonTool.Size = new System.Drawing.Size(24, 20);
            this.btnComparisonTool.TabIndex = 4;
            this.btnComparisonTool.Text = "...";
            this.btnComparisonTool.UseVisualStyleBackColor = true;
            this.btnComparisonTool.Click += new System.EventHandler(this.btnComparisonTool_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chkEnableServiceCheck
            // 
            this.chkEnableServiceCheck.AutoSize = true;
            this.chkEnableServiceCheck.Checked = true;
            this.chkEnableServiceCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableServiceCheck.Location = new System.Drawing.Point(9, 36);
            this.chkEnableServiceCheck.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkEnableServiceCheck.Name = "chkEnableServiceCheck";
            this.chkEnableServiceCheck.Size = new System.Drawing.Size(151, 21);
            this.chkEnableServiceCheck.TabIndex = 5;
            this.chkEnableServiceCheck.Text = "Clear cache while saving";
            this.chkEnableServiceCheck.UseVisualStyleBackColor = true;
            // 
            // sreDatabases1
            // 
            this.sreDatabases1.DataSourceId = -99;
            this.sreDatabases1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sreDatabases1.EnableGlobalKeys = true;
            this.sreDatabases1.Location = new System.Drawing.Point(2, 2);
            this.sreDatabases1.Name = "sreDatabases1";
            this.sreDatabases1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.sreDatabases1.SaveButton = null;
            this.sreDatabases1.ShowSaveButton = true;
            this.sreDatabases1.Size = new System.Drawing.Size(715, 186);
            this.sreDatabases1.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 60);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(727, 216);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sreDatabases1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(719, 190);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Global Connection Strings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 321);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.chkEnableServiceCheck);
            this.Controls.Add(this.btnComparisonTool);
            this.Controls.Add(this.txtComparisonTool);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "Preferences";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Preferences_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtComparisonTool;
        private System.Windows.Forms.Button btnComparisonTool;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkEnableServiceCheck;
        private Controls.SreDatabases sreDatabases1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
    }
}