namespace Eyedia.IDPE.Interface
{
    partial class RuleWizards
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
            this.grpTop = new System.Windows.Forms.GroupBox();
            this.cbWizards = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.cbDataSources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpBottom = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.attributeMapperRuleWizard1 = new Eyedia.IDPE.Interface.AttributeMapperRuleWizard();
            this.grpTop.SuspendLayout();
            this.grpBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTop
            // 
            this.grpTop.Controls.Add(this.cbWizards);
            this.grpTop.Controls.Add(this.label2);
            this.grpTop.Controls.Add(this.btnHelp);
            this.grpTop.Controls.Add(this.cbDataSources);
            this.grpTop.Controls.Add(this.label1);
            this.grpTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTop.Location = new System.Drawing.Point(0, 0);
            this.grpTop.Name = "grpTop";
            this.grpTop.Size = new System.Drawing.Size(1122, 47);
            this.grpTop.TabIndex = 0;
            this.grpTop.TabStop = false;
            // 
            // cbWizards
            // 
            this.cbWizards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWizards.FormattingEnabled = true;
            this.cbWizards.Items.AddRange(new object[] {
            "Internal External Mapping"});
            this.cbWizards.Location = new System.Drawing.Point(57, 17);
            this.cbWizards.Name = "cbWizards";
            this.cbWizards.Size = new System.Drawing.Size(300, 21);
            this.cbWizards.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Wizards";
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Location = new System.Drawing.Point(1041, 17);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "&Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // cbDataSources
            // 
            this.cbDataSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSources.FormattingEnabled = true;
            this.cbDataSources.Location = new System.Drawing.Point(461, 17);
            this.cbDataSources.Name = "cbDataSources";
            this.cbDataSources.Size = new System.Drawing.Size(300, 21);
            this.cbDataSources.TabIndex = 1;
            this.cbDataSources.SelectedIndexChanged += new System.EventHandler(this.cbDataSources_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(388, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Source";
            // 
            // grpBottom
            // 
            this.grpBottom.Controls.Add(this.btnClose);
            this.grpBottom.Controls.Add(this.btnSave);
            this.grpBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpBottom.Location = new System.Drawing.Point(0, 427);
            this.grpBottom.Name = "grpBottom";
            this.grpBottom.Size = new System.Drawing.Size(1122, 46);
            this.grpBottom.TabIndex = 1;
            this.grpBottom.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(611, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(513, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // attributeMapperRuleWizard1
            // 
            this.attributeMapperRuleWizard1.Location = new System.Drawing.Point(30, 81);
            this.attributeMapperRuleWizard1.Name = "attributeMapperRuleWizard1";
            this.attributeMapperRuleWizard1.Size = new System.Drawing.Size(232, 100);
            this.attributeMapperRuleWizard1.TabIndex = 2;
            // 
            // RuleWizards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 473);
            this.Controls.Add(this.attributeMapperRuleWizard1);
            this.Controls.Add(this.grpBottom);
            this.Controls.Add(this.grpTop);
            this.Name = "RuleWizards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rule Wizard - Internal External Mapping";
            this.Activated += new System.EventHandler(this.frmWizMapping_Activated);
            this.grpTop.ResumeLayout(false);
            this.grpTop.PerformLayout();
            this.grpBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTop;
        private System.Windows.Forms.GroupBox grpBottom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDataSources;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnHelp;
        private AttributeMapperRuleWizard attributeMapperRuleWizard1;
        private System.Windows.Forms.ComboBox cbWizards;
        private System.Windows.Forms.Label label2;
    }
}