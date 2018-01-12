namespace Symplus.RuleEngine.Utilities
{
    partial class frmWizMapping
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWizMapping));
            this.grpTop = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTips = new System.Windows.Forms.Label();
            this.cbDataSources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpBottom = new System.Windows.Forms.GroupBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pnlr2c1 = new System.Windows.Forms.Panel();
            this.cbr1c1 = new System.Windows.Forms.ComboBox();
            this.rbr1c1 = new System.Windows.Forms.RadioButton();
            this.pnlr1c1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pnlr2c2 = new System.Windows.Forms.Panel();
            this.txtr1c2 = new System.Windows.Forms.TextBox();
            this.rbr1c2 = new System.Windows.Forms.RadioButton();
            this.pnlr1c2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlr2c3 = new System.Windows.Forms.Panel();
            this.txtr1c3 = new System.Windows.Forms.TextBox();
            this.pnlr1c3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.grpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.pnlr2c1.SuspendLayout();
            this.pnlr1c1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.pnlr2c2.SuspendLayout();
            this.pnlr1c2.SuspendLayout();
            this.pnlr2c3.SuspendLayout();
            this.pnlr1c3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTop
            // 
            this.grpTop.Controls.Add(this.button1);
            this.grpTop.Controls.Add(this.pictureBox1);
            this.grpTop.Controls.Add(this.lblTips);
            this.grpTop.Controls.Add(this.cbDataSources);
            this.grpTop.Controls.Add(this.label1);
            this.grpTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTop.Location = new System.Drawing.Point(0, 0);
            this.grpTop.Name = "grpTop";
            this.grpTop.Size = new System.Drawing.Size(1122, 73);
            this.grpTop.TabIndex = 0;
            this.grpTop.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(390, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Symplus.RuleEngine.Utilities.Properties.Resources.info;
            this.pictureBox1.Location = new System.Drawing.Point(411, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblTips
            // 
            this.lblTips.BackColor = System.Drawing.SystemColors.Info;
            this.lblTips.Location = new System.Drawing.Point(459, 16);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(657, 46);
            this.lblTips.TabIndex = 2;
            this.lblTips.Text = resources.GetString("lblTips.Text");
            // 
            // cbDataSources
            // 
            this.cbDataSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSources.FormattingEnabled = true;
            this.cbDataSources.Location = new System.Drawing.Point(84, 32);
            this.cbDataSources.Name = "cbDataSources";
            this.cbDataSources.Size = new System.Drawing.Size(297, 21);
            this.cbDataSources.TabIndex = 1;
            this.cbDataSources.SelectedIndexChanged += new System.EventHandler(this.cbDataSources_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DataSources";
            // 
            // grpBottom
            // 
            this.grpBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpBottom.Location = new System.Drawing.Point(0, 391);
            this.grpBottom.Name = "grpBottom";
            this.grpBottom.Size = new System.Drawing.Size(1122, 82);
            this.grpBottom.TabIndex = 1;
            this.grpBottom.TabStop = false;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 73);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.pnlr2c1);
            this.splitContainerMain.Panel1.Controls.Add(this.pnlr1c1);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainerMain.Size = new System.Drawing.Size(1122, 318);
            this.splitContainerMain.SplitterDistance = 373;
            this.splitContainerMain.TabIndex = 2;
            // 
            // pnlr2c1
            // 
            this.pnlr2c1.Controls.Add(this.cbr1c1);
            this.pnlr2c1.Controls.Add(this.rbr1c1);
            this.pnlr2c1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlr2c1.Location = new System.Drawing.Point(0, 23);
            this.pnlr2c1.Name = "pnlr2c1";
            this.pnlr2c1.Padding = new System.Windows.Forms.Padding(4);
            this.pnlr2c1.Size = new System.Drawing.Size(373, 30);
            this.pnlr2c1.TabIndex = 1;
            // 
            // cbr1c1
            // 
            this.cbr1c1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbr1c1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbr1c1.FormattingEnabled = true;
            this.cbr1c1.Location = new System.Drawing.Point(94, 4);
            this.cbr1c1.Name = "cbr1c1";
            this.cbr1c1.Size = new System.Drawing.Size(275, 21);
            this.cbr1c1.TabIndex = 1;
            // 
            // rbr1c1
            // 
            this.rbr1c1.AutoSize = true;
            this.rbr1c1.Checked = true;
            this.rbr1c1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbr1c1.Location = new System.Drawing.Point(4, 4);
            this.rbr1c1.Name = "rbr1c1";
            this.rbr1c1.Size = new System.Drawing.Size(90, 22);
            this.rbr1c1.TabIndex = 0;
            this.rbr1c1.TabStop = true;
            this.rbr1c1.Text = "From Attribute";
            this.rbr1c1.UseVisualStyleBackColor = true;
            this.rbr1c1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // pnlr1c1
            // 
            this.pnlr1c1.Controls.Add(this.label3);
            this.pnlr1c1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlr1c1.Location = new System.Drawing.Point(0, 0);
            this.pnlr1c1.Name = "pnlr1c1";
            this.pnlr1c1.Size = new System.Drawing.Size(373, 23);
            this.pnlr1c1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(373, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "From Attribute";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pnlr2c2);
            this.splitContainer2.Panel1.Controls.Add(this.pnlr1c2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pnlr2c3);
            this.splitContainer2.Panel2.Controls.Add(this.pnlr1c3);
            this.splitContainer2.Size = new System.Drawing.Size(745, 318);
            this.splitContainer2.SplitterDistance = 358;
            this.splitContainer2.TabIndex = 0;
            // 
            // pnlr2c2
            // 
            this.pnlr2c2.Controls.Add(this.txtr1c2);
            this.pnlr2c2.Controls.Add(this.rbr1c2);
            this.pnlr2c2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlr2c2.Location = new System.Drawing.Point(0, 23);
            this.pnlr2c2.Name = "pnlr2c2";
            this.pnlr2c2.Padding = new System.Windows.Forms.Padding(4);
            this.pnlr2c2.Size = new System.Drawing.Size(358, 30);
            this.pnlr2c2.TabIndex = 2;
            // 
            // txtr1c2
            // 
            this.txtr1c2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtr1c2.Enabled = false;
            this.txtr1c2.Location = new System.Drawing.Point(82, 4);
            this.txtr1c2.Name = "txtr1c2";
            this.txtr1c2.Size = new System.Drawing.Size(272, 20);
            this.txtr1c2.TabIndex = 2;
            // 
            // rbr1c2
            // 
            this.rbr1c2.AutoSize = true;
            this.rbr1c2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbr1c2.Location = new System.Drawing.Point(4, 4);
            this.rbr1c2.Name = "rbr1c2";
            this.rbr1c2.Size = new System.Drawing.Size(78, 22);
            this.rbr1c2.TabIndex = 1;
            this.rbr1c2.Text = "From Value";
            this.rbr1c2.UseVisualStyleBackColor = true;
            this.rbr1c2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // pnlr1c2
            // 
            this.pnlr1c2.Controls.Add(this.label4);
            this.pnlr1c2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlr1c2.Location = new System.Drawing.Point(0, 0);
            this.pnlr1c2.Name = "pnlr1c2";
            this.pnlr1c2.Size = new System.Drawing.Size(358, 23);
            this.pnlr1c2.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(358, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "From Value Or VB Expression";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlr2c3
            // 
            this.pnlr2c3.Controls.Add(this.txtr1c3);
            this.pnlr2c3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlr2c3.Location = new System.Drawing.Point(0, 23);
            this.pnlr2c3.Name = "pnlr2c3";
            this.pnlr2c3.Padding = new System.Windows.Forms.Padding(4);
            this.pnlr2c3.Size = new System.Drawing.Size(383, 30);
            this.pnlr2c3.TabIndex = 2;
            // 
            // txtr1c3
            // 
            this.txtr1c3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtr1c3.Enabled = false;
            this.txtr1c3.Location = new System.Drawing.Point(4, 4);
            this.txtr1c3.Name = "txtr1c3";
            this.txtr1c3.ReadOnly = true;
            this.txtr1c3.Size = new System.Drawing.Size(375, 20);
            this.txtr1c3.TabIndex = 3;
            // 
            // pnlr1c3
            // 
            this.pnlr1c3.Controls.Add(this.label5);
            this.pnlr1c3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlr1c3.Location = new System.Drawing.Point(0, 0);
            this.pnlr1c3.Name = "pnlr1c3";
            this.pnlr1c3.Size = new System.Drawing.Size(383, 23);
            this.pnlr1c3.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(383, 23);
            this.label5.TabIndex = 1;
            this.label5.Text = "To System Attribute";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmWizMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 473);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.grpBottom);
            this.Controls.Add(this.grpTop);
            this.Name = "frmWizMapping";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rule Wizard - Internal External Mapping";
            this.grpTop.ResumeLayout(false);
            this.grpTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.pnlr2c1.ResumeLayout(false);
            this.pnlr2c1.PerformLayout();
            this.pnlr1c1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.pnlr2c2.ResumeLayout(false);
            this.pnlr2c2.PerformLayout();
            this.pnlr1c2.ResumeLayout(false);
            this.pnlr2c3.ResumeLayout(false);
            this.pnlr2c3.PerformLayout();
            this.pnlr1c3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTop;
        private System.Windows.Forms.GroupBox grpBottom;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDataSources;
        private System.Windows.Forms.Label lblTips;
        private System.Windows.Forms.Panel pnlr2c1;
        private System.Windows.Forms.Panel pnlr1c1;
        private System.Windows.Forms.Panel pnlr2c2;
        private System.Windows.Forms.Panel pnlr1c2;
        private System.Windows.Forms.Panel pnlr2c3;
        private System.Windows.Forms.Panel pnlr1c3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbr1c1;
        private System.Windows.Forms.RadioButton rbr1c1;
        private System.Windows.Forms.TextBox txtr1c2;
        private System.Windows.Forms.RadioButton rbr1c2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtr1c3;
        private System.Windows.Forms.Button button1;
    }
}