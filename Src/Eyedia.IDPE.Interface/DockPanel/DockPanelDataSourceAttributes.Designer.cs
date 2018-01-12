namespace Eyedia.IDPE.Interface
{
    partial class DockPanelDataSourceAttributes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockPanelDataSourceAttributes));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.sreListView1 = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDisassociate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miImport = new System.Windows.Forms.ToolStripMenuItem();
            this.miExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.versionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
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
            this.sreListView1.Location = new System.Drawing.Point(0, 3);
            this.sreListView1.Name = "sreListView1";
            this.sreListView1.Rules = null;
            this.sreListView1.ShowAddButton = false;
            this.sreListView1.ShowPosition = true;
            this.sreListView1.ShowPrintButton = false;
            this.sreListView1.ShowRepositionCheckBox = true;
            this.sreListView1.ShowSaveButton = false;
            this.sreListView1.Size = new System.Drawing.Size(808, 283);
            this.sreListView1.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDisassociate,
            this.toolStripMenuItem1,
            this.miImport,
            this.miExport,
            this.toolStripMenuItem2,
            this.versionsToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(212, 169);
            // 
            // mnuDisassociate
            // 
            this.mnuDisassociate.Name = "mnuDisassociate";
            this.mnuDisassociate.Size = new System.Drawing.Size(211, 30);
            this.mnuDisassociate.Text = "Disassociate";
            this.mnuDisassociate.Click += new System.EventHandler(this.mnuDisassociate_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(208, 6);
            // 
            // miImport
            // 
            this.miImport.Name = "miImport";
            this.miImport.Size = new System.Drawing.Size(211, 30);
            this.miImport.Text = "Import";
            this.miImport.Click += new System.EventHandler(this.miImport_Click);
            // 
            // miExport
            // 
            this.miExport.Name = "miExport";
            this.miExport.Size = new System.Drawing.Size(211, 30);
            this.miExport.Text = "Export";
            this.miExport.Click += new System.EventHandler(this.miExport_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(208, 6);
            // 
            // versionsToolStripMenuItem
            // 
            this.versionsToolStripMenuItem.Name = "versionsToolStripMenuItem";
            this.versionsToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
            this.versionsToolStripMenuItem.Text = "Versions";
            this.versionsToolStripMenuItem.Click += new System.EventHandler(this.versionsToolStripMenuItem_Click);
            // 
            // DockPanelDataSourceAttributes
            // 
            this.ClientSize = new System.Drawing.Size(808, 289);
            this.Controls.Add(this.sreListView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DockPanelDataSourceAttributes";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.TabText = "Data Source Attributes";
            this.Text = "Data Source Attributes";
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        internal Controls.SreListView sreListView1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuDisassociate;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miImport;
        private System.Windows.Forms.ToolStripMenuItem miExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem versionsToolStripMenuItem;
    }
}