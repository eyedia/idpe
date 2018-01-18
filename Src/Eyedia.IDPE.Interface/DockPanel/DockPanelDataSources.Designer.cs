namespace Eyedia.IDPE.Interface
{
    partial class DockPanelDataSources
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockPanelDataSources));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.sreListView1 = new Eyedia.IDPE.Interface.Controls.ListViewControl();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setAsDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuValidate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCompareWithPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.mcnuShowVersions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImportKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.globalSearchWidget1 = new Eyedia.IDPE.Interface.GlobalSearchWidget();
            this.dataSourceSummary1 = new Eyedia.IDPE.Interface.Controls.DataSourceSummary();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.deployToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qa1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sreListView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.globalSearchWidget1);
            this.splitContainer1.Panel2.Controls.Add(this.dataSourceSummary1);
            this.splitContainer1.Size = new System.Drawing.Size(808, 283);
            this.splitContainer1.SplitterDistance = 346;
            this.splitContainer1.TabIndex = 0;
            // 
            // sreListView1
            // 
            this.sreListView1.Attributes = null;
            this.sreListView1.ContextMenuStrip = this.contextMenu;
            this.sreListView1.DataSources = null;
            this.sreListView1.DefaultItem = null;
            this.sreListView1.DefaultItemId = 0;
            this.sreListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sreListView1.FormatIsPrintable = false;
            this.sreListView1.Location = new System.Drawing.Point(0, 0);
            this.sreListView1.Name = "sreListView1";
            this.sreListView1.Rules = null;
            this.sreListView1.ShowAddButton = true;
            this.sreListView1.ShowPrintButton = true;
            this.sreListView1.ShowRepositionCheckBox = false;
            this.sreListView1.ShowSaveButton = false;
            this.sreListView1.Size = new System.Drawing.Size(346, 283);
            this.sreListView1.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAsDefaultToolStripMenuItem,
            this.toolStripMenuItem6,
            this.mnuValidate,
            this.toolStripMenuItem1,
            this.mnuCompareWithPrevious,
            this.mcnuShowVersions,
            this.deployToolStripMenuItem,
            this.toolStripMenuItem2,
            this.mnuCopy,
            this.mnuDelete,
            this.toolStripMenuItem3,
            this.mnuExport,
            this.mnuExportKeys,
            this.toolStripMenuItem4,
            this.mnuImport,
            this.mnuImportKeys,
            this.toolStripMenuItem5,
            this.mnuRefresh});
            this.contextMenu.Name = "copyToMenu";
            this.contextMenu.Size = new System.Drawing.Size(345, 400);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // setAsDefaultToolStripMenuItem
            // 
            this.setAsDefaultToolStripMenuItem.Name = "setAsDefaultToolStripMenuItem";
            this.setAsDefaultToolStripMenuItem.Size = new System.Drawing.Size(344, 30);
            this.setAsDefaultToolStripMenuItem.Text = "Set as default";
            this.setAsDefaultToolStripMenuItem.Click += new System.EventHandler(this.setAsDefaultToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(341, 6);
            // 
            // mnuValidate
            // 
            this.mnuValidate.Name = "mnuValidate";
            this.mnuValidate.Size = new System.Drawing.Size(344, 30);
            this.mnuValidate.Text = "Validate";
            this.mnuValidate.Click += new System.EventHandler(this.mnuValidate_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(341, 6);
            // 
            // mnuCompareWithPrevious
            // 
            this.mnuCompareWithPrevious.Name = "mnuCompareWithPrevious";
            this.mnuCompareWithPrevious.Size = new System.Drawing.Size(344, 30);
            this.mnuCompareWithPrevious.Text = "Compare with Previous Version";
            this.mnuCompareWithPrevious.Click += new System.EventHandler(this.mnuCompareWithPrevious_Click);
            // 
            // mcnuShowVersions
            // 
            this.mcnuShowVersions.Name = "mcnuShowVersions";
            this.mcnuShowVersions.Size = new System.Drawing.Size(344, 30);
            this.mcnuShowVersions.Text = "Versions";
            this.mcnuShowVersions.Click += new System.EventHandler(this.mcnuShowVersions_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(341, 6);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(344, 30);
            this.mnuCopy.Text = "Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(344, 30);
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(341, 6);
            // 
            // mnuExport
            // 
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.Size = new System.Drawing.Size(344, 30);
            this.mnuExport.Text = "Export";
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
            // 
            // mnuExportKeys
            // 
            this.mnuExportKeys.Name = "mnuExportKeys";
            this.mnuExportKeys.Size = new System.Drawing.Size(344, 30);
            this.mnuExportKeys.Text = "Export Keys";
            this.mnuExportKeys.Click += new System.EventHandler(this.mnuExportKeys_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(341, 6);
            // 
            // mnuImport
            // 
            this.mnuImport.Name = "mnuImport";
            this.mnuImport.Size = new System.Drawing.Size(344, 30);
            this.mnuImport.Text = "Import";
            this.mnuImport.Click += new System.EventHandler(this.mnuImport_Click);
            // 
            // mnuImportKeys
            // 
            this.mnuImportKeys.Name = "mnuImportKeys";
            this.mnuImportKeys.Size = new System.Drawing.Size(344, 30);
            this.mnuImportKeys.Text = "Import Keys";
            this.mnuImportKeys.Click += new System.EventHandler(this.mnuImportKeys_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(341, 6);
            // 
            // mnuRefresh
            // 
            this.mnuRefresh.Name = "mnuRefresh";
            this.mnuRefresh.Size = new System.Drawing.Size(344, 30);
            this.mnuRefresh.Text = "Refresh";
            this.mnuRefresh.Click += new System.EventHandler(this.mnuRefresh_Click);
            // 
            // globalSearchWidget1
            // 
            this.globalSearchWidget1.DataSourceId = 0;
            this.globalSearchWidget1.Location = new System.Drawing.Point(0, 0);
            this.globalSearchWidget1.Name = "globalSearchWidget1";
            this.globalSearchWidget1.ShowSearchTextBox = true;
            this.globalSearchWidget1.SingleDataSource = false;
            this.globalSearchWidget1.Size = new System.Drawing.Size(424, 158);
            this.globalSearchWidget1.TabIndex = 0;
            this.globalSearchWidget1.Visible = false;
            // 
            // dataSourceSummary1
            // 
            this.dataSourceSummary1.DataSourceId = 0;
            this.dataSourceSummary1.DockPanelProperty = null;
            this.dataSourceSummary1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataSourceSummary1.Location = new System.Drawing.Point(15, 173);
            this.dataSourceSummary1.Name = "dataSourceSummary1";
            this.dataSourceSummary1.Size = new System.Drawing.Size(423, 98);
            this.dataSourceSummary1.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // deployToolStripMenuItem
            // 
            this.deployToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qa1ToolStripMenuItem});
            this.deployToolStripMenuItem.Name = "deployToolStripMenuItem";
            this.deployToolStripMenuItem.Size = new System.Drawing.Size(344, 30);
            this.deployToolStripMenuItem.Text = "Deploy";
            // 
            // qa1ToolStripMenuItem
            // 
            this.qa1ToolStripMenuItem.Name = "qa1ToolStripMenuItem";
            this.qa1ToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
            this.qa1ToolStripMenuItem.Text = "qa1";
            // 
            // DockPanelDataSources
            // 
            this.ClientSize = new System.Drawing.Size(808, 289);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockPanelDataSources";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.TabText = "Data Source Attributes";
            this.Text = "Data Source Attributes";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DockPanelDataSources_KeyUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        internal Controls.ListViewControl sreListView1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuValidate;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuCompareWithPrevious;
        private System.Windows.Forms.ToolStripMenuItem mcnuShowVersions;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnuExport;
        private System.Windows.Forms.ToolStripMenuItem mnuExportKeys;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mnuImport;
        private System.Windows.Forms.ToolStripMenuItem mnuImportKeys;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem mnuRefresh;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem setAsDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        internal GlobalSearchWidget globalSearchWidget1;
        private Controls.DataSourceSummary dataSourceSummary1;
        private System.Windows.Forms.ToolStripMenuItem deployToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qa1ToolStripMenuItem;
    }
}