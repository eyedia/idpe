namespace Eyedia.IDPE.Interface
{
    partial class frmSreConnections
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
            this.sreDatabases1 = new Eyedia.IDPE.Interface.Controls.ConnectionStringControl();
            this.SuspendLayout();
            // 
            // sreDatabases1
            // 
            this.sreDatabases1.DataSourceId = 0;
            this.sreDatabases1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sreDatabases1.Location = new System.Drawing.Point(0, 0);
            this.sreDatabases1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.sreDatabases1.Name = "sreDatabases1";
            this.sreDatabases1.SaveButton = null;
            this.sreDatabases1.Size = new System.Drawing.Size(841, 517);
            this.sreDatabases1.TabIndex = 0;
            // 
            // frmSreConnections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 517);
            this.Controls.Add(this.sreDatabases1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmSreConnections";
            this.Text = "Create Database Connections";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ConnectionStringControl sreDatabases1;
    }
}