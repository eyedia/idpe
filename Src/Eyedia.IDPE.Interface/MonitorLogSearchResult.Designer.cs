namespace Eyedia.IDPE.Interface
{
    partial class MonitorLogSearchResult
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
            this.lvSearchResult = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvSearchResult
            // 
            this.lvSearchResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader5});
            this.lvSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSearchResult.FullRowSelect = true;
            this.lvSearchResult.Location = new System.Drawing.Point(0, 0);
            this.lvSearchResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvSearchResult.Name = "lvSearchResult";
            this.lvSearchResult.Size = new System.Drawing.Size(999, 510);
            this.lvSearchResult.TabIndex = 1;
            this.lvSearchResult.UseCompatibleStateImageBehavior = false;
            this.lvSearchResult.View = System.Windows.Forms.View.Details;
            this.lvSearchResult.DoubleClick += new System.EventHandler(this.lvSearchResult_DoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "File Name";
            this.columnHeader4.Width = 200;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Line number";
            this.columnHeader6.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Text";
            this.columnHeader5.Width = 400;
            // 
            // MonitorLogResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 510);
            this.Controls.Add(this.lvSearchResult);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MonitorLogResult";
            this.Text = "Search Result";
            this.Resize += new System.EventHandler(this.MonitorLogResult_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvSearchResult;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}