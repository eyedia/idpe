namespace Eyedia.IDPE.Interface
{
    partial class Test
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
            this.globalSearchWidget1 = new Eyedia.IDPE.Interface.GlobalSearchWidget();
            this.SuspendLayout();
            // 
            // globalSearchWidget1
            // 
            this.globalSearchWidget1.Location = new System.Drawing.Point(178, 56);
            this.globalSearchWidget1.Name = "globalSearchWidget1";
            this.globalSearchWidget1.ShowSearchTextBox = true;
            this.globalSearchWidget1.Size = new System.Drawing.Size(736, 327);
            this.globalSearchWidget1.TabIndex = 0;
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1113, 496);
            this.Controls.Add(this.globalSearchWidget1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Test";
            this.Text = "Test";
            this.ResumeLayout(false);

        }



























        #endregion

        private GlobalSearchWidget globalSearchWidget1;
    }
}