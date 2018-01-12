namespace Eyedia.IDPE.Interface
{
    partial class ConfigCSharpInputGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigCSharpInputGenerator));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cbGenerateDataTableFrom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cSharpExpression1 = new Eyedia.IDPE.Interface.CSharpExpression();
            this.statusStrip1.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 795);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1110, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1087, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.cbGenerateDataTableFrom);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1110, 52);
            this.pnlTop.TabIndex = 1;
            // 
            // cbGenerateDataTableFrom
            // 
            this.cbGenerateDataTableFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGenerateDataTableFrom.FormattingEnabled = true;
            this.cbGenerateDataTableFrom.Items.AddRange(new object[] {
            "File Name",
            "File Content"});
            this.cbGenerateDataTableFrom.Location = new System.Drawing.Point(110, 9);
            this.cbGenerateDataTableFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbGenerateDataTableFrom.Name = "cbGenerateDataTableFrom";
            this.cbGenerateDataTableFrom.Size = new System.Drawing.Size(290, 28);
            this.cbGenerateDataTableFrom.TabIndex = 1;
            this.cbGenerateDataTableFrom.SelectedIndexChanged += new System.EventHandler(this.cbGenerateDataTableFrom_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Using";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 735);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1110, 60);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(573, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 37);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "C&lose";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(424, 11);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 37);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cSharpExpression1
            // 
            this.cSharpExpression1.AdditionalReferences = "";
            this.cSharpExpression1.AdditionalUsingNamespace = "";
            this.cSharpExpression1.Code = "";
            this.cSharpExpression1.CodeType = Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileName;
            this.cSharpExpression1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSharpExpression1.HelperMethods = "";
            this.cSharpExpression1.Location = new System.Drawing.Point(0, 52);
            this.cSharpExpression1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cSharpExpression1.Name = "cSharpExpression1";
            this.cSharpExpression1.RawString = "DataTableGeneratorFromFileName°°°°";
            this.cSharpExpression1.ShowHelperMethods = true;
            this.cSharpExpression1.Size = new System.Drawing.Size(1110, 683);
            this.cSharpExpression1.TabIndex = 3;
            // 
            // ConfigCSharpInputGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 817);
            this.Controls.Add(this.cSharpExpression1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ConfigCSharpInputGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Multi Record Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigCSharpInputGenerator_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private CSharpExpression cSharpExpression1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cbGenerateDataTableFrom;
        private System.Windows.Forms.Label label1;
    }
}