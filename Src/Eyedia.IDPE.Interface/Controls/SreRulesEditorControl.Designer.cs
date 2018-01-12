namespace Eyedia.IDPE.Interface
{
    partial class SreRulesEditorControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.contextMenuStripRules = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miRename = new System.Windows.Forms.ToolStripMenuItem();
            this.miDisassociate = new System.Windows.Forms.ToolStripMenuItem();
            this.miDisassociateDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miShowDependencies = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miExport = new System.Windows.Forms.ToolStripMenuItem();
            this.miImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvPreValidate = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lvRowPreparing = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lvRowPrepared = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lvRowValidate = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lvPostValidate = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.lvSqlInit = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.contextMenuStripRules.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.ContextMenuStrip = this.contextMenuStripRules;
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Controls.Add(this.tabPage5);
            this.tabControl.Controls.Add(this.tabPage6);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(817, 481);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // contextMenuStripRules
            // 
            this.contextMenuStripRules.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripRules.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRename,
            this.miDisassociate,
            this.miDisassociateDelete,
            this.toolStripMenuItem1,
            this.miShowDependencies,
            this.toolStripMenuItem2,
            this.miExport,
            this.miImport,
            this.toolStripMenuItem3,
            this.miRefresh});
            this.contextMenuStripRules.Name = "contextMenuStripRules";
            this.contextMenuStripRules.Size = new System.Drawing.Size(355, 232);
            // 
            // miRename
            // 
            this.miRename.Name = "miRename";
            this.miRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.miRename.Size = new System.Drawing.Size(354, 30);
            this.miRename.Text = "Rename";
            this.miRename.Click += new System.EventHandler(this.miRename_Click);
            // 
            // miDisassociate
            // 
            this.miDisassociate.Name = "miDisassociate";
            this.miDisassociate.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.miDisassociate.Size = new System.Drawing.Size(354, 30);
            this.miDisassociate.Text = "Disassociate";
            this.miDisassociate.Click += new System.EventHandler(this.miDisassociate_Click);
            // 
            // miDisassociateDelete
            // 
            this.miDisassociateDelete.Name = "miDisassociateDelete";
            this.miDisassociateDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.miDisassociateDelete.Size = new System.Drawing.Size(354, 30);
            this.miDisassociateDelete.Text = "Disassociate && Delete";
            this.miDisassociateDelete.Click += new System.EventHandler(this.miDisassociateDelete_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(351, 6);
            // 
            // miShowDependencies
            // 
            this.miShowDependencies.Name = "miShowDependencies";
            this.miShowDependencies.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.miShowDependencies.Size = new System.Drawing.Size(354, 30);
            this.miShowDependencies.Text = "Show Dependencies";
            this.miShowDependencies.Click += new System.EventHandler(this.miShowDependencies_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(351, 6);
            // 
            // miExport
            // 
            this.miExport.Name = "miExport";
            this.miExport.Size = new System.Drawing.Size(354, 30);
            this.miExport.Text = "Export";
            this.miExport.Click += new System.EventHandler(this.miExport_Click);
            // 
            // miImport
            // 
            this.miImport.Name = "miImport";
            this.miImport.Size = new System.Drawing.Size(354, 30);
            this.miImport.Text = "Import";
            this.miImport.Click += new System.EventHandler(this.miImport_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(351, 6);
            // 
            // miRefresh
            // 
            this.miRefresh.Name = "miRefresh";
            this.miRefresh.Size = new System.Drawing.Size(354, 30);
            this.miRefresh.Text = "Refresh";
            this.miRefresh.Click += new System.EventHandler(this.miRefresh_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvPreValidate);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(809, 448);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pre Validate";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvPreValidate
            // 
            this.lvPreValidate.Attributes = null;
            this.lvPreValidate.ContextMenuStrip = this.contextMenuStripRules;
            this.lvPreValidate.DataSources = null;
            this.lvPreValidate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPreValidate.FormatIsPrintable = false;
            this.lvPreValidate.Location = new System.Drawing.Point(0, 0);
            this.lvPreValidate.Name = "lvPreValidate";
            this.lvPreValidate.Rules = null;
            this.lvPreValidate.ShowAddButton = false;
            this.lvPreValidate.ShowPrintButton = false;
            this.lvPreValidate.ShowRepositionCheckBox = false;
            this.lvPreValidate.ShowSaveButton = false;
            this.lvPreValidate.Size = new System.Drawing.Size(809, 448);
            this.lvPreValidate.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lvRowPreparing);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(809, 448);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Row Preparing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lvRowPreparing
            // 
            this.lvRowPreparing.Attributes = null;
            this.lvRowPreparing.ContextMenuStrip = this.contextMenuStripRules;
            this.lvRowPreparing.DataSources = null;
            this.lvRowPreparing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRowPreparing.FormatIsPrintable = false;
            this.lvRowPreparing.Location = new System.Drawing.Point(0, 0);
            this.lvRowPreparing.Name = "lvRowPreparing";
            this.lvRowPreparing.Rules = null;
            this.lvRowPreparing.ShowAddButton = false;
            this.lvRowPreparing.ShowPrintButton = false;
            this.lvRowPreparing.ShowRepositionCheckBox = false;
            this.lvRowPreparing.ShowSaveButton = false;
            this.lvRowPreparing.Size = new System.Drawing.Size(809, 448);
            this.lvRowPreparing.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lvRowPrepared);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(809, 448);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Row Prepared";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lvRowPrepared
            // 
            this.lvRowPrepared.Attributes = null;
            this.lvRowPrepared.ContextMenuStrip = this.contextMenuStripRules;
            this.lvRowPrepared.DataSources = null;
            this.lvRowPrepared.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRowPrepared.FormatIsPrintable = false;
            this.lvRowPrepared.Location = new System.Drawing.Point(0, 0);
            this.lvRowPrepared.Name = "lvRowPrepared";
            this.lvRowPrepared.Rules = null;
            this.lvRowPrepared.ShowAddButton = false;
            this.lvRowPrepared.ShowPrintButton = false;
            this.lvRowPrepared.ShowRepositionCheckBox = false;
            this.lvRowPrepared.ShowSaveButton = false;
            this.lvRowPrepared.Size = new System.Drawing.Size(809, 448);
            this.lvRowPrepared.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lvRowValidate);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(809, 448);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Row Validate";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lvRowValidate
            // 
            this.lvRowValidate.Attributes = null;
            this.lvRowValidate.ContextMenuStrip = this.contextMenuStripRules;
            this.lvRowValidate.DataSources = null;
            this.lvRowValidate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRowValidate.FormatIsPrintable = false;
            this.lvRowValidate.Location = new System.Drawing.Point(0, 0);
            this.lvRowValidate.Name = "lvRowValidate";
            this.lvRowValidate.Rules = null;
            this.lvRowValidate.ShowAddButton = false;
            this.lvRowValidate.ShowPrintButton = false;
            this.lvRowValidate.ShowRepositionCheckBox = false;
            this.lvRowValidate.ShowSaveButton = false;
            this.lvRowValidate.Size = new System.Drawing.Size(809, 448);
            this.lvRowValidate.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lvPostValidate);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(809, 448);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Post Validate";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lvPostValidate
            // 
            this.lvPostValidate.Attributes = null;
            this.lvPostValidate.DataSources = null;
            this.lvPostValidate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPostValidate.FormatIsPrintable = false;
            this.lvPostValidate.Location = new System.Drawing.Point(0, 0);
            this.lvPostValidate.Name = "lvPostValidate";
            this.lvPostValidate.Rules = null;
            this.lvPostValidate.ShowAddButton = false;
            this.lvPostValidate.ShowPrintButton = false;
            this.lvPostValidate.ShowRepositionCheckBox = false;
            this.lvPostValidate.ShowSaveButton = false;
            this.lvPostValidate.Size = new System.Drawing.Size(809, 448);
            this.lvPostValidate.TabIndex = 1;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.lvSqlInit);
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(809, 448);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Sql Init";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // lvSqlInit
            // 
            this.lvSqlInit.Attributes = null;
            this.lvSqlInit.DataSources = null;
            this.lvSqlInit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSqlInit.FormatIsPrintable = false;
            this.lvSqlInit.Location = new System.Drawing.Point(0, 0);
            this.lvSqlInit.Name = "lvSqlInit";
            this.lvSqlInit.Rules = null;
            this.lvSqlInit.ShowAddButton = false;
            this.lvSqlInit.ShowPosition = true;
            this.lvSqlInit.ShowPrintButton = false;
            this.lvSqlInit.ShowRepositionCheckBox = false;
            this.lvSqlInit.ShowSaveButton = false;
            this.lvSqlInit.Size = new System.Drawing.Size(809, 448);
            this.lvSqlInit.TabIndex = 2;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // SreRulesEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "SreRulesEditorControl";
            this.Size = new System.Drawing.Size(817, 481);
            this.tabControl.ResumeLayout(false);
            this.contextMenuStripRules.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private Controls.SreListView lvPreValidate;
        private Controls.SreListView lvRowPreparing;
        private Controls.SreListView lvRowPrepared;
        private Controls.SreListView lvRowValidate;
        private Controls.SreListView lvPostValidate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRules;
        private System.Windows.Forms.ToolStripMenuItem miRename;
        private System.Windows.Forms.ToolStripMenuItem miDisassociate;
        private System.Windows.Forms.ToolStripMenuItem miDisassociateDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miShowDependencies;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miImport;
        private System.Windows.Forms.ToolStripMenuItem miExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miRefresh;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tabPage6;
        private Controls.SreListView lvSqlInit;
    }
}
