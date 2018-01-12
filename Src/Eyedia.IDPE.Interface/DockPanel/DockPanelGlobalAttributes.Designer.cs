namespace Eyedia.IDPE.Interface
{
    partial class DockPanelGlobalAttributes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockPanelGlobalAttributes));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.pnlRight = new System.Windows.Forms.Panel();
            this.btnAssociateBoth = new System.Windows.Forms.Button();
            this.btnAssociateAttributeSystemDataSource = new System.Windows.Forms.Button();
            this.btnAssociateAttributeDataSource = new System.Windows.Forms.Button();
            this.sreListView1 = new Eyedia.IDPE.Interface.Controls.SreListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miExport = new System.Windows.Forms.ToolStripMenuItem();
            this.miImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlRight.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.btnAssociateBoth);
            this.pnlRight.Controls.Add(this.btnAssociateAttributeSystemDataSource);
            this.pnlRight.Controls.Add(this.btnAssociateAttributeDataSource);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(397, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(127, 607);
            this.pnlRight.TabIndex = 0;
            // 
            // btnAssociateBoth
            // 
            this.btnAssociateBoth.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAssociateBoth.Image = ((System.Drawing.Image)(resources.GetObject("btnAssociateBoth.Image")));
            this.btnAssociateBoth.Location = new System.Drawing.Point(30, 169);
            this.btnAssociateBoth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAssociateBoth.Name = "btnAssociateBoth";
            this.btnAssociateBoth.Size = new System.Drawing.Size(69, 58);
            this.btnAssociateBoth.TabIndex = 3;
            this.btnAssociateBoth.UseVisualStyleBackColor = true;
            this.btnAssociateBoth.Click += new System.EventHandler(this.btnAssociateBoth_Click);
            // 
            // btnAssociateAttributeSystemDataSource
            // 
            this.btnAssociateAttributeSystemDataSource.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAssociateAttributeSystemDataSource.Image = global::Eyedia.IDPE.Interface.Properties.Resources._112_RightArrowShort_Red_32x32_72;
            this.btnAssociateAttributeSystemDataSource.Location = new System.Drawing.Point(30, 336);
            this.btnAssociateAttributeSystemDataSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAssociateAttributeSystemDataSource.Name = "btnAssociateAttributeSystemDataSource";
            this.btnAssociateAttributeSystemDataSource.Size = new System.Drawing.Size(69, 58);
            this.btnAssociateAttributeSystemDataSource.TabIndex = 2;
            this.btnAssociateAttributeSystemDataSource.UseVisualStyleBackColor = true;
            this.btnAssociateAttributeSystemDataSource.Click += new System.EventHandler(this.AddToSystemDataSourceButtonClick);
            // 
            // btnAssociateAttributeDataSource
            // 
            this.btnAssociateAttributeDataSource.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAssociateAttributeDataSource.Image = ((System.Drawing.Image)(resources.GetObject("btnAssociateAttributeDataSource.Image")));
            this.btnAssociateAttributeDataSource.Location = new System.Drawing.Point(30, 252);
            this.btnAssociateAttributeDataSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAssociateAttributeDataSource.Name = "btnAssociateAttributeDataSource";
            this.btnAssociateAttributeDataSource.Size = new System.Drawing.Size(69, 58);
            this.btnAssociateAttributeDataSource.TabIndex = 1;
            this.btnAssociateAttributeDataSource.UseVisualStyleBackColor = true;
            this.btnAssociateAttributeDataSource.Click += new System.EventHandler(this.AddToDataSourceButtonClick);
            // 
            // sreListView1
            // 
            this.sreListView1.Attributes = null;
            this.sreListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.sreListView1.DataSources = null;
            this.sreListView1.DefaultItem = null;
            this.sreListView1.DefaultItemId = 0;
            this.sreListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sreListView1.FormatIsPrintable = false;
            this.sreListView1.Location = new System.Drawing.Point(0, 3);
            this.sreListView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sreListView1.Name = "sreListView1";
            this.sreListView1.Rules = null;
            this.sreListView1.ShowAddButton = true;
            this.sreListView1.ShowPosition = true;
            this.sreListView1.ShowPrintButton = true;
            this.sreListView1.ShowRepositionCheckBox = false;
            this.sreListView1.ShowSaveButton = false;
            this.sreListView1.Size = new System.Drawing.Size(397, 607);
            this.sreListView1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExport,
            this.miImport,
            this.toolStripMenuItem1,
            this.refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(157, 100);
            // 
            // miExport
            // 
            this.miExport.Name = "miExport";
            this.miExport.Size = new System.Drawing.Size(156, 30);
            this.miExport.Text = "Export";
            this.miExport.Click += new System.EventHandler(this.miExport_Click);
            // 
            // miImport
            // 
            this.miImport.Name = "miImport";
            this.miImport.Size = new System.Drawing.Size(156, 30);
            this.miImport.Text = "Import";
            this.miImport.Click += new System.EventHandler(this.miImport_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DockPanelGlobalAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.ClientSize = new System.Drawing.Size(524, 613);
            this.CloseButton = false;
            this.Controls.Add(this.sreListView1);
            this.Controls.Add(this.pnlRight);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "DockPanelGlobalAttributes";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.TabText = "Global Attributes";
            this.Text = "Global Attributes";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DockPanelGlobalAttributes_KeyUp);
            this.Resize += new System.EventHandler(this.DockPanelGlobalAttributes_Resize);
            this.pnlRight.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.Panel pnlRight;
        internal System.Windows.Forms.Button btnAssociateAttributeSystemDataSource;
        internal System.Windows.Forms.Button btnAssociateAttributeDataSource;
        internal Controls.SreListView sreListView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miExport;
        private System.Windows.Forms.ToolStripMenuItem miImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        internal System.Windows.Forms.Button btnAssociateBoth;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}