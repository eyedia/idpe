namespace Eyedia.IDPE.Interface
{
    partial class CSharpExpression
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
            this.gbBottom = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtEditor = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtHelperMethods = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtUsing = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbReferences = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddReference = new System.Windows.Forms.Button();
            this.pnlTools = new System.Windows.Forms.Panel();
            this.rtbCompileResult = new System.Windows.Forms.RichTextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gbBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbBottom
            // 
            this.gbBottom.Controls.Add(this.btnCancel);
            this.gbBottom.Controls.Add(this.btnCompile);
            this.gbBottom.Controls.Add(this.btnOK);
            this.gbBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbBottom.Location = new System.Drawing.Point(0, 373);
            this.gbBottom.Name = "gbBottom";
            this.gbBottom.Size = new System.Drawing.Size(650, 57);
            this.gbBottom.TabIndex = 0;
            this.gbBottom.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(369, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cl&ose";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCompile.Enabled = false;
            this.btnCompile.Location = new System.Drawing.Point(207, 17);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(75, 23);
            this.btnCompile.TabIndex = 1;
            this.btnCompile.Text = "&Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(288, 17);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.pnlTools);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtbCompileResult);
            this.splitContainer1.Size = new System.Drawing.Size(650, 373);
            this.splitContainer1.SplitterDistance = 303;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(650, 277);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtEditor);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(642, 251);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source Code";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtEditor
            // 
            this.txtEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditor.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEditor.Location = new System.Drawing.Point(3, 3);
            this.txtEditor.MaxLength = 3995;
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.Size = new System.Drawing.Size(636, 245);
            this.txtEditor.TabIndex = 0;
            this.txtEditor.Text = "";
            this.txtEditor.TextChanged += new System.EventHandler(this.txtEditor_TextChanged);
            this.txtEditor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CSharpExpression_KeyUp);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtHelperMethods);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(642, 251);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Helper Methods";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtHelperMethods
            // 
            this.txtHelperMethods.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHelperMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelperMethods.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHelperMethods.Location = new System.Drawing.Point(0, 0);
            this.txtHelperMethods.MaxLength = 3995;
            this.txtHelperMethods.Name = "txtHelperMethods";
            this.txtHelperMethods.Size = new System.Drawing.Size(642, 251);
            this.txtHelperMethods.TabIndex = 2;
            this.txtHelperMethods.Text = "";
            this.txtHelperMethods.TextChanged += new System.EventHandler(this.txtHelperMethods_TextChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtUsing);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(642, 251);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Using";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtUsing
            // 
            this.txtUsing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsing.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsing.Location = new System.Drawing.Point(0, 0);
            this.txtUsing.MaxLength = 3995;
            this.txtUsing.Name = "txtUsing";
            this.txtUsing.Size = new System.Drawing.Size(642, 251);
            this.txtUsing.TabIndex = 1;
            this.txtUsing.Text = "";
            this.txtUsing.TextChanged += new System.EventHandler(this.txtUsing_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lbReferences);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(642, 251);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "References";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbReferences
            // 
            this.lbReferences.BackColor = System.Drawing.SystemColors.Window;
            this.lbReferences.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbReferences.ContextMenuStrip = this.contextMenuStrip1;
            this.lbReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbReferences.FormattingEnabled = true;
            this.lbReferences.Location = new System.Drawing.Point(3, 3);
            this.lbReferences.Name = "lbReferences";
            this.lbReferences.Size = new System.Drawing.Size(636, 217);
            this.lbReferences.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "&Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAddReference);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 220);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 28);
            this.panel1.TabIndex = 0;
            // 
            // btnAddReference
            // 
            this.btnAddReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddReference.Location = new System.Drawing.Point(561, 2);
            this.btnAddReference.Name = "btnAddReference";
            this.btnAddReference.Size = new System.Drawing.Size(75, 23);
            this.btnAddReference.TabIndex = 0;
            this.btnAddReference.Text = "&Add";
            this.btnAddReference.UseVisualStyleBackColor = true;
            this.btnAddReference.Click += new System.EventHandler(this.btnAddReference_Click);
            // 
            // pnlTools
            // 
            this.pnlTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTools.Location = new System.Drawing.Point(0, 277);
            this.pnlTools.Name = "pnlTools";
            this.pnlTools.Size = new System.Drawing.Size(650, 26);
            this.pnlTools.TabIndex = 0;
            this.pnlTools.Visible = false;
            // 
            // rtbCompileResult
            // 
            this.rtbCompileResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbCompileResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCompileResult.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.rtbCompileResult.Location = new System.Drawing.Point(0, 0);
            this.rtbCompileResult.Name = "rtbCompileResult";
            this.rtbCompileResult.Size = new System.Drawing.Size(650, 66);
            this.rtbCompileResult.TabIndex = 4;
            this.rtbCompileResult.Text = "";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "dll";
            this.openFileDialog.Filter = ".Net Assemblies|*.dll";
            this.openFileDialog.Title = "Add Reference";
            // 
            // CSharpExpression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gbBottom);
            this.Name = "CSharpExpression";
            this.Size = new System.Drawing.Size(650, 430);
            this.Load += new System.EventHandler(this.CSharpExpression_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CSharpExpression_KeyUp);
            this.gbBottom.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox txtEditor;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lbReferences;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddReference;
        private System.Windows.Forms.Panel pnlTools;
        private System.Windows.Forms.RichTextBox rtbCompileResult;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox txtUsing;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox txtHelperMethods;
    }
}
