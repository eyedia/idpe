namespace Eyedia.IDPE.Interface
{
    partial class frmConfig2
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
            this.headerFooterConfiguration1 = new Eyedia.IDPE.Interface.HeaderFooterConfiguration();
            this.SuspendLayout();
            // 
            // headerFooterConfiguration1
            // 
            this.headerFooterConfiguration1.DataFormatType = Eyedia.IDPE.Common.DataFormatTypes.Delimited;
            this.headerFooterConfiguration1.DataSourceId = 0;
            this.headerFooterConfiguration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerFooterConfiguration1.DoNotCloseAtSave = false;
            this.headerFooterConfiguration1.Location = new System.Drawing.Point(0, 0);
            this.headerFooterConfiguration1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.headerFooterConfiguration1.Name = "headerFooterConfiguration1";
            this.headerFooterConfiguration1.Size = new System.Drawing.Size(682, 710);
            this.headerFooterConfiguration1.TabIndex = 0;
            // 
            // frmConfig2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(682, 710);
            this.Controls.Add(this.headerFooterConfiguration1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmConfig2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmParam";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfig2_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private HeaderFooterConfiguration headerFooterConfiguration1;

    }
}