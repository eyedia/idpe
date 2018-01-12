namespace Eyedia.IDPE.Interface
{
    partial class frmConfigXmlExtension
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
            this.groupBoxTop = new System.Windows.Forms.GroupBox();
            this.lvwSamples = new System.Windows.Forms.ListView();
            this.pnlTopRight = new System.Windows.Forms.Panel();
            this.radCSharpCode = new System.Windows.Forms.RadioButton();
            this.radXslt = new System.Windows.Forms.RadioButton();
            this.pnlTips = new System.Windows.Forms.Panel();
            this.lblTips = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.groupBoxBottom = new System.Windows.Forms.GroupBox();
            this.btnExportAll = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.webBrowser2 = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExport1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnExport2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.rtbCSharpCode = new System.Windows.Forms.RichTextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnExport3 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbFlatFile = new System.Windows.Forms.RichTextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnExport4 = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxTop.SuspendLayout();
            this.pnlTopRight.SuspendLayout();
            this.pnlTips.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxBottom.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTop
            // 
            this.groupBoxTop.Controls.Add(this.lvwSamples);
            this.groupBoxTop.Controls.Add(this.pnlTopRight);
            this.groupBoxTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTop.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTop.Name = "groupBoxTop";
            this.groupBoxTop.Size = new System.Drawing.Size(1069, 77);
            this.groupBoxTop.TabIndex = 0;
            this.groupBoxTop.TabStop = false;
            // 
            // lvwSamples
            // 
            this.lvwSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwSamples.Location = new System.Drawing.Point(3, 16);
            this.lvwSamples.Name = "lvwSamples";
            this.lvwSamples.Size = new System.Drawing.Size(393, 58);
            this.lvwSamples.TabIndex = 0;
            this.lvwSamples.UseCompatibleStateImageBehavior = false;
            this.lvwSamples.View = System.Windows.Forms.View.List;
            this.lvwSamples.SelectedIndexChanged += new System.EventHandler(this.lvwSamples_SelectedIndexChanged);
            // 
            // pnlTopRight
            // 
            this.pnlTopRight.Controls.Add(this.radCSharpCode);
            this.pnlTopRight.Controls.Add(this.radXslt);
            this.pnlTopRight.Controls.Add(this.pnlTips);
            this.pnlTopRight.Controls.Add(this.btnSelect);
            this.pnlTopRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTopRight.Location = new System.Drawing.Point(396, 16);
            this.pnlTopRight.Name = "pnlTopRight";
            this.pnlTopRight.Size = new System.Drawing.Size(670, 58);
            this.pnlTopRight.TabIndex = 1;
            // 
            // radCSharpCode
            // 
            this.radCSharpCode.AutoSize = true;
            this.radCSharpCode.Location = new System.Drawing.Point(57, 9);
            this.radCSharpCode.Name = "radCSharpCode";
            this.radCSharpCode.Size = new System.Drawing.Size(67, 17);
            this.radCSharpCode.TabIndex = 11;
            this.radCSharpCode.Text = "C# Code";
            this.radCSharpCode.UseVisualStyleBackColor = true;
            // 
            // radXslt
            // 
            this.radXslt.AutoSize = true;
            this.radXslt.Checked = true;
            this.radXslt.Location = new System.Drawing.Point(9, 9);
            this.radXslt.Name = "radXslt";
            this.radXslt.Size = new System.Drawing.Size(42, 17);
            this.radXslt.TabIndex = 10;
            this.radXslt.TabStop = true;
            this.radXslt.Text = "Xslt";
            this.radXslt.UseVisualStyleBackColor = true;
            // 
            // pnlTips
            // 
            this.pnlTips.Controls.Add(this.lblTips);
            this.pnlTips.Controls.Add(this.pictureBox1);
            this.pnlTips.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTips.Location = new System.Drawing.Point(220, 0);
            this.pnlTips.Name = "pnlTips";
            this.pnlTips.Size = new System.Drawing.Size(450, 58);
            this.pnlTips.TabIndex = 9;
            // 
            // lblTips
            // 
            this.lblTips.BackColor = System.Drawing.SystemColors.Info;
            this.lblTips.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTips.Location = new System.Drawing.Point(42, 0);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(408, 29);
            this.lblTips.TabIndex = 11;
            this.lblTips.Text = "Each sample XML has \"XSLT\" and \"C# Code\" to parse. You need either of one. Xslt i" +
    "s recommended.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Eyedia.IDPE.Interface.Properties.Resources.info;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(139, 6);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "&Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // groupBoxBottom
            // 
            this.groupBoxBottom.Controls.Add(this.btnExportAll);
            this.groupBoxBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxBottom.Location = new System.Drawing.Point(0, 504);
            this.groupBoxBottom.Name = "groupBoxBottom";
            this.groupBoxBottom.Size = new System.Drawing.Size(1069, 44);
            this.groupBoxBottom.TabIndex = 1;
            this.groupBoxBottom.TabStop = false;
            // 
            // btnExportAll
            // 
            this.btnExportAll.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExportAll.Enabled = false;
            this.btnExportAll.Location = new System.Drawing.Point(503, 15);
            this.btnExportAll.Name = "btnExportAll";
            this.btnExportAll.Size = new System.Drawing.Size(75, 23);
            this.btnExportAll.TabIndex = 2;
            this.btnExportAll.Text = "&Export All";
            this.btnExportAll.UseVisualStyleBackColor = true;
            this.btnExportAll.Click += new System.EventHandler(this.btnExportAll_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splitContainer1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 77);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1069, 427);
            this.pnlMain.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.webBrowser2);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1069, 427);
            this.splitContainer1.SplitterDistance = 282;
            this.splitContainer1.TabIndex = 0;
            // 
            // webBrowser2
            // 
            this.webBrowser2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser2.Location = new System.Drawing.Point(0, 15);
            this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser2.Name = "webBrowser2";
            this.webBrowser2.Size = new System.Drawing.Size(282, 377);
            this.webBrowser2.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnExport1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 392);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(282, 35);
            this.panel2.TabIndex = 2;
            // 
            // btnExport1
            // 
            this.btnExport1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExport1.Enabled = false;
            this.btnExport1.Location = new System.Drawing.Point(114, 6);
            this.btnExport1.Name = "btnExport1";
            this.btnExport1.Size = new System.Drawing.Size(75, 23);
            this.btnExport1.TabIndex = 0;
            this.btnExport1.Text = "Export";
            this.btnExport1.UseVisualStyleBackColor = true;
            this.btnExport1.Click += new System.EventHandler(this.btnExport1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 15);
            this.panel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Silver;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Sample XML (Input)";
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
            this.splitContainer2.Panel1.Controls.Add(this.webBrowser1);
            this.splitContainer2.Panel1.Controls.Add(this.panel4);
            this.splitContainer2.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(783, 427);
            this.splitContainer2.SplitterDistance = 260;
            this.splitContainer2.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 15);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(260, 377);
            this.webBrowser1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnExport2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 392);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(260, 35);
            this.panel4.TabIndex = 2;
            // 
            // btnExport2
            // 
            this.btnExport2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExport2.Enabled = false;
            this.btnExport2.Location = new System.Drawing.Point(93, 6);
            this.btnExport2.Name = "btnExport2";
            this.btnExport2.Size = new System.Drawing.Size(75, 23);
            this.btnExport2.TabIndex = 1;
            this.btnExport2.Text = "Export";
            this.btnExport2.UseVisualStyleBackColor = true;
            this.btnExport2.Click += new System.EventHandler(this.btnExport2_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(260, 15);
            this.panel3.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "XSLT (Input)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.rtbCSharpCode);
            this.splitContainer3.Panel1.Controls.Add(this.panel6);
            this.splitContainer3.Panel1.Controls.Add(this.panel5);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.rtbFlatFile);
            this.splitContainer3.Panel2.Controls.Add(this.panel8);
            this.splitContainer3.Panel2.Controls.Add(this.panel7);
            this.splitContainer3.Size = new System.Drawing.Size(519, 427);
            this.splitContainer3.SplitterDistance = 256;
            this.splitContainer3.TabIndex = 0;
            // 
            // rtbCSharpCode
            // 
            this.rtbCSharpCode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rtbCSharpCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbCSharpCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCSharpCode.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.rtbCSharpCode.Location = new System.Drawing.Point(0, 15);
            this.rtbCSharpCode.Name = "rtbCSharpCode";
            this.rtbCSharpCode.ReadOnly = true;
            this.rtbCSharpCode.Size = new System.Drawing.Size(256, 377);
            this.rtbCSharpCode.TabIndex = 3;
            this.rtbCSharpCode.Text = "";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnExport3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 392);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(256, 35);
            this.panel6.TabIndex = 2;
            // 
            // btnExport3
            // 
            this.btnExport3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExport3.Enabled = false;
            this.btnExport3.Location = new System.Drawing.Point(91, 6);
            this.btnExport3.Name = "btnExport3";
            this.btnExport3.Size = new System.Drawing.Size(75, 23);
            this.btnExport3.TabIndex = 1;
            this.btnExport3.Text = "Export";
            this.btnExport3.UseVisualStyleBackColor = true;
            this.btnExport3.Click += new System.EventHandler(this.btnExport3_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(256, 15);
            this.panel5.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Silver;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "C# Code (Input)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbFlatFile
            // 
            this.rtbFlatFile.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rtbFlatFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbFlatFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbFlatFile.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.rtbFlatFile.Location = new System.Drawing.Point(0, 15);
            this.rtbFlatFile.Name = "rtbFlatFile";
            this.rtbFlatFile.ReadOnly = true;
            this.rtbFlatFile.Size = new System.Drawing.Size(259, 377);
            this.rtbFlatFile.TabIndex = 1;
            this.rtbFlatFile.Text = "";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnExport4);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 392);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(259, 35);
            this.panel8.TabIndex = 3;
            // 
            // btnExport4
            // 
            this.btnExport4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExport4.Enabled = false;
            this.btnExport4.Location = new System.Drawing.Point(92, 6);
            this.btnExport4.Name = "btnExport4";
            this.btnExport4.Size = new System.Drawing.Size(75, 23);
            this.btnExport4.TabIndex = 1;
            this.btnExport4.Text = "Export";
            this.btnExport4.UseVisualStyleBackColor = true;
            this.btnExport4.Click += new System.EventHandler(this.btnExport4_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(259, 15);
            this.panel7.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Silver;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(259, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Delimited Output (Output)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmConfigXmlExtension
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 548);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.groupBoxBottom);
            this.Controls.Add(this.groupBoxTop);
            this.Name = "frmConfigXmlExtension";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sample XML Configurations";
            this.groupBoxTop.ResumeLayout(false);
            this.pnlTopRight.ResumeLayout(false);
            this.pnlTopRight.PerformLayout();
            this.pnlTips.ResumeLayout(false);
            this.pnlTips.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBoxBottom.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTop;
        private System.Windows.Forms.GroupBox groupBoxBottom;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RichTextBox rtbFlatFile;
        private System.Windows.Forms.ListView lvwSamples;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnExportAll;
        private System.Windows.Forms.Button btnExport1;
        private System.Windows.Forms.Button btnExport2;
        private System.Windows.Forms.Button btnExport3;
        private System.Windows.Forms.Button btnExport4;
        private System.Windows.Forms.Panel pnlTopRight;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.RichTextBox rtbCSharpCode;
        private System.Windows.Forms.WebBrowser webBrowser2;
        private System.Windows.Forms.Panel pnlTips;
        private System.Windows.Forms.Label lblTips;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radCSharpCode;
        private System.Windows.Forms.RadioButton radXslt;
    }
}