namespace Eyedia.IDPE.Interface
{
    partial class GlobalSearchWidget
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
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.pnlSearchResult = new System.Windows.Forms.Panel();
            this.lvwSearchResult = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlSearch.SuspendLayout();
            this.pnlSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblCaption);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(550, 106);
            this.pnlSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSearch.Location = new System.Drawing.Point(222, 64);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 35);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.SystemColors.Control;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.errorProvider1.SetIconAlignment(this.txtSearch, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.errorProvider1.SetIconPadding(this.txtSearch, 10);
            this.txtSearch.Location = new System.Drawing.Point(0, 33);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(550, 26);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // lblCaption
            // 
            this.lblCaption.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCaption.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(550, 33);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "Global Search";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSearchResult
            // 
            this.pnlSearchResult.Controls.Add(this.lvwSearchResult);
            this.pnlSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearchResult.Location = new System.Drawing.Point(0, 106);
            this.pnlSearchResult.Name = "pnlSearchResult";
            this.pnlSearchResult.Size = new System.Drawing.Size(550, 244);
            this.pnlSearchResult.TabIndex = 1;
            // 
            // lvwSearchResult
            // 
            this.lvwSearchResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvwSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwSearchResult.FullRowSelect = true;
            this.lvwSearchResult.Location = new System.Drawing.Point(0, 0);
            this.lvwSearchResult.Name = "lvwSearchResult";
            this.lvwSearchResult.Size = new System.Drawing.Size(550, 244);
            this.lvwSearchResult.TabIndex = 2;
            this.lvwSearchResult.UseCompatibleStateImageBehavior = false;
            this.lvwSearchResult.View = System.Windows.Forms.View.Details;
            this.lvwSearchResult.Resize += new System.EventHandler(this.lvwSearchResult_Resize);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Where";
            this.columnHeader2.Width = 180;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 100;
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // GlobalSearchWidget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSearchResult);
            this.Controls.Add(this.pnlSearch);
            this.Name = "GlobalSearchWidget";
            this.Size = new System.Drawing.Size(550, 350);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel pnlSearchResult;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ListView lvwSearchResult;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
