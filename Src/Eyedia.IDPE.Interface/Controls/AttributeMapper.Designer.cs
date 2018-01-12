namespace Eyedia.IDPE.Interface
{
    partial class AttributeMapper
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmbAttributesSys = new System.Windows.Forms.ComboBox();
            this.txtVBExpression = new System.Windows.Forms.TextBox();
            this.cmbAttributes = new System.Windows.Forms.ComboBox();
            this.radVBExpression = new System.Windows.Forms.RadioButton();
            this.radString = new System.Windows.Forms.RadioButton();
            this.radAttribute = new System.Windows.Forms.RadioButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 8);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cmbAttributesSys);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtVBExpression);
            this.splitContainer1.Panel2.Controls.Add(this.cmbAttributes);
            this.splitContainer1.Panel2.Controls.Add(this.radVBExpression);
            this.splitContainer1.Panel2.Controls.Add(this.radString);
            this.splitContainer1.Panel2.Controls.Add(this.radAttribute);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.splitContainer1.Size = new System.Drawing.Size(772, 23);
            this.splitContainer1.SplitterDistance = 318;
            this.splitContainer1.TabIndex = 0;
            // 
            // cmbAttributesSys
            // 
            this.cmbAttributesSys.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAttributesSys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAttributesSys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttributesSys.FormattingEnabled = true;
            this.cmbAttributesSys.Location = new System.Drawing.Point(20, 0);
            this.cmbAttributesSys.Name = "cmbAttributesSys";
            this.cmbAttributesSys.Size = new System.Drawing.Size(298, 21);
            this.cmbAttributesSys.TabIndex = 0;
            this.toolTip1.SetToolTip(this.cmbAttributesSys, "Internal attribute");
            this.cmbAttributesSys.SelectedIndexChanged += new System.EventHandler(this.ClearError);
            // 
            // txtVBExpression
            // 
            this.txtVBExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVBExpression.Location = new System.Drawing.Point(184, 0);
            this.txtVBExpression.Name = "txtVBExpression";
            this.txtVBExpression.Size = new System.Drawing.Size(246, 20);
            this.txtVBExpression.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtVBExpression, "The constant string");
            this.txtVBExpression.Visible = false;
            this.txtVBExpression.TextChanged += new System.EventHandler(this.ClearError);
            // 
            // cmbAttributes
            // 
            this.cmbAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAttributes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttributes.FormattingEnabled = true;
            this.cmbAttributes.Location = new System.Drawing.Point(184, 0);
            this.cmbAttributes.Name = "cmbAttributes";
            this.cmbAttributes.Size = new System.Drawing.Size(246, 21);
            this.cmbAttributes.TabIndex = 2;
            this.cmbAttributes.SelectedIndexChanged += new System.EventHandler(this.ClearError);
            // 
            // radVBExpression
            // 
            this.radVBExpression.AutoSize = true;
            this.radVBExpression.Dock = System.Windows.Forms.DockStyle.Left;
            this.radVBExpression.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radVBExpression.Location = new System.Drawing.Point(121, 0);
            this.radVBExpression.Name = "radVBExpression";
            this.radVBExpression.Size = new System.Drawing.Size(63, 23);
            this.radVBExpression.TabIndex = 4;
            this.radVBExpression.TabStop = true;
            this.radVBExpression.Text = "VB Expr";
            this.toolTip1.SetToolTip(this.radVBExpression, "Map to VB Expression");
            this.radVBExpression.UseVisualStyleBackColor = true;
            // 
            // radString
            // 
            this.radString.AutoSize = true;
            this.radString.BackColor = System.Drawing.Color.Transparent;
            this.radString.Dock = System.Windows.Forms.DockStyle.Left;
            this.radString.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radString.Location = new System.Drawing.Point(69, 0);
            this.radString.Name = "radString";
            this.radString.Size = new System.Drawing.Size(52, 23);
            this.radString.TabIndex = 1;
            this.radString.Text = "String";
            this.toolTip1.SetToolTip(this.radString, "Map to constant string");
            this.radString.UseVisualStyleBackColor = false;
            this.radString.CheckedChanged += new System.EventHandler(this.AssignTypeChanged);
            // 
            // radAttribute
            // 
            this.radAttribute.AutoSize = true;
            this.radAttribute.Checked = true;
            this.radAttribute.Dock = System.Windows.Forms.DockStyle.Left;
            this.radAttribute.Location = new System.Drawing.Point(5, 0);
            this.radAttribute.Name = "radAttribute";
            this.radAttribute.Size = new System.Drawing.Size(64, 23);
            this.radAttribute.TabIndex = 0;
            this.radAttribute.TabStop = true;
            this.radAttribute.Text = "Attribute";
            this.toolTip1.SetToolTip(this.radAttribute, "Map to external attribute");
            this.radAttribute.UseVisualStyleBackColor = true;
            this.radAttribute.CheckedChanged += new System.EventHandler(this.AssignTypeChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // AttributeMapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "AttributeMapper";
            this.Padding = new System.Windows.Forms.Padding(0, 8, 0, 5);
            this.Size = new System.Drawing.Size(772, 36);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox cmbAttributesSys;
        private System.Windows.Forms.RadioButton radString;
        private System.Windows.Forms.RadioButton radAttribute;
        private System.Windows.Forms.ComboBox cmbAttributes;
        private System.Windows.Forms.TextBox txtVBExpression;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton radVBExpression;
    }
}
