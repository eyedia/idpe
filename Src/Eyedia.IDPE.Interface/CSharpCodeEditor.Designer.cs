namespace Eyedia.IDPE.Interface
{
    partial class CSharpCodeEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSharpCodeEditor));
            this.cSharpExpression1 = new Eyedia.IDPE.Interface.CSharpExpression();
            this.SuspendLayout();
            // 
            // cSharpExpression1
            // 
            this.cSharpExpression1.Code = "";
            this.cSharpExpression1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSharpExpression1.Location = new System.Drawing.Point(0, 0);
            this.cSharpExpression1.Name = "cSharpExpression1";
            this.cSharpExpression1.Size = new System.Drawing.Size(702, 417);
            this.cSharpExpression1.TabIndex = 0;
            // 
            // CSharpCodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 417);
            this.Controls.Add(this.cSharpExpression1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CSharpCodeEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "C# Code Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private CSharpExpression cSharpExpression1;
    }
}