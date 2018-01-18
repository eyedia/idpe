namespace Eyedia.IDPE.Interface
{
    partial class frmKeys
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
            this.idpeKeys1 = new Eyedia.IDPE.Interface.Controls.IdpeKeysControl();
            this.SuspendLayout();
            // 
            // idpeKeys1
            // 
            this.idpeKeys1.DataSourceId = 0;
            this.idpeKeys1.DataSourceName = null;
            this.idpeKeys1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idpeKeys1.Location = new System.Drawing.Point(0, 0);
            this.idpeKeys1.Name = "idpeKeys1";
            this.idpeKeys1.ShowCaption = false;
            this.idpeKeys1.Size = new System.Drawing.Size(1057, 323);
            this.idpeKeys1.TabIndex = 0;
            // 
            // frmKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 323);
            this.Controls.Add(this.idpeKeys1);
            this.Name = "frmKeys";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DataSource Keys";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.IdpeKeysControl idpeKeys1;

    }
}