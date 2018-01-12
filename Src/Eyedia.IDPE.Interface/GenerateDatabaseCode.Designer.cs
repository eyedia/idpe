namespace Eyedia.IDPE.Interface
{
    partial class GenerateDatabaseCode
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
            this.btnShowInstructions1 = new System.Windows.Forms.Button();
            this.btnGenerateDuplicateCheck = new System.Windows.Forms.Button();
            this.cbDatabaseTypes = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnShowInstructions1
            // 
            this.btnShowInstructions1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowInstructions1.Location = new System.Drawing.Point(363, 14);
            this.btnShowInstructions1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnShowInstructions1.Name = "btnShowInstructions1";
            this.btnShowInstructions1.Size = new System.Drawing.Size(202, 37);
            this.btnShowInstructions1.TabIndex = 6;
            this.btnShowInstructions1.Text = "&Show Instructions";
            this.btnShowInstructions1.UseVisualStyleBackColor = true;
            this.btnShowInstructions1.Click += new System.EventHandler(this.btnShowInstructions1_Click);
            // 
            // btnGenerateDuplicateCheck
            // 
            this.btnGenerateDuplicateCheck.Location = new System.Drawing.Point(8, 52);
            this.btnGenerateDuplicateCheck.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGenerateDuplicateCheck.Name = "btnGenerateDuplicateCheck";
            this.btnGenerateDuplicateCheck.Size = new System.Drawing.Size(338, 37);
            this.btnGenerateDuplicateCheck.TabIndex = 5;
            this.btnGenerateDuplicateCheck.Text = "Generate Duplicate Check Store Procedure Code";
            this.btnGenerateDuplicateCheck.UseVisualStyleBackColor = true;
            this.btnGenerateDuplicateCheck.Click += new System.EventHandler(this.btnGenerateDuplicateCheck_Click);
            // 
            // cbDatabaseTypes
            // 
            this.cbDatabaseTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabaseTypes.FormattingEnabled = true;
            this.cbDatabaseTypes.Location = new System.Drawing.Point(8, 14);
            this.cbDatabaseTypes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbDatabaseTypes.Name = "cbDatabaseTypes";
            this.cbDatabaseTypes.Size = new System.Drawing.Size(338, 28);
            this.cbDatabaseTypes.TabIndex = 7;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GenerateDatabaseCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 205);
            this.Controls.Add(this.btnShowInstructions1);
            this.Controls.Add(this.cbDatabaseTypes);
            this.Controls.Add(this.btnGenerateDuplicateCheck);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GenerateDatabaseCode";
            this.Text = "Generate Database Code";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnShowInstructions1;
        private System.Windows.Forms.Button btnGenerateDuplicateCheck;
        private System.Windows.Forms.ComboBox cbDatabaseTypes;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}