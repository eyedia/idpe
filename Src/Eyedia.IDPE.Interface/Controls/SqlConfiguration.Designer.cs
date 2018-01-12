namespace Eyedia.IDPE.Interface.Controls
{
    partial class SqlConfiguration
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
            this.gbTop = new System.Windows.Forms.GroupBox();
            this.txtRunTimeCononectionStringName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbConnectionString = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCreateConnections = new System.Windows.Forms.Button();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numUpDnIntervalSql = new System.Windows.Forms.NumericUpDown();
            this.lblMinutes = new System.Windows.Forms.Label();
            this.chkTestQueries = new System.Windows.Forms.CheckBox();
            this.gbBottom = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkFirstRowIsHeader = new System.Windows.Forms.CheckBox();
            this.btnInterface = new System.Windows.Forms.Button();
            this.txtInterfaceName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.radInterface = new System.Windows.Forms.RadioButton();
            this.radInputData = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTestRecoveryQuery = new System.Windows.Forms.Button();
            this.btnTestUpdateQuery = new System.Windows.Forms.Button();
            this.pnlQuerySelect = new System.Windows.Forms.Panel();
            this.txtQueryUpdate = new System.Windows.Forms.RichTextBox();
            this.lblUpdateQuery = new System.Windows.Forms.Label();
            this.txtQuerySelect = new System.Windows.Forms.RichTextBox();
            this.pnlQueryProcessing = new System.Windows.Forms.Panel();
            this.btnTestSqlQuery = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.pnlTestButton2 = new System.Windows.Forms.Panel();
            this.pnlQueryRecovery = new System.Windows.Forms.Panel();
            this.txtQueryRecovery = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlTestButton3 = new System.Windows.Forms.Panel();
            this.pnlOthers = new System.Windows.Forms.Panel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlTestButton1 = new System.Windows.Forms.Panel();
            this.gbTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDnIntervalSql)).BeginInit();
            this.gbBottom.SuspendLayout();
            this.pnlQuerySelect.SuspendLayout();
            this.pnlQueryProcessing.SuspendLayout();
            this.pnlQueryRecovery.SuspendLayout();
            this.pnlOthers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbTop
            // 
            this.gbTop.Controls.Add(this.txtRunTimeCononectionStringName);
            this.gbTop.Controls.Add(this.label1);
            this.gbTop.Controls.Add(this.cmbConnectionString);
            this.gbTop.Controls.Add(this.label9);
            this.gbTop.Controls.Add(this.btnCreateConnections);
            this.gbTop.Controls.Add(this.lblInterval);
            this.gbTop.Controls.Add(this.numUpDnIntervalSql);
            this.gbTop.Controls.Add(this.lblMinutes);
            this.gbTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTop.Location = new System.Drawing.Point(0, 0);
            this.gbTop.Name = "gbTop";
            this.gbTop.Size = new System.Drawing.Size(725, 43);
            this.gbTop.TabIndex = 0;
            this.gbTop.TabStop = false;
            // 
            // txtRunTimeCononectionStringName
            // 
            this.txtRunTimeCononectionStringName.Location = new System.Drawing.Point(421, 16);
            this.txtRunTimeCononectionStringName.Margin = new System.Windows.Forms.Padding(2);
            this.txtRunTimeCononectionStringName.Name = "txtRunTimeCononectionStringName";
            this.txtRunTimeCononectionStringName.Size = new System.Drawing.Size(161, 20);
            this.txtRunTimeCononectionStringName.TabIndex = 2;
            this.toolTip.SetToolTip(this.txtRunTimeCononectionStringName, "Run time connection string name");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(369, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Run time";
            // 
            // cmbConnectionString
            // 
            this.cmbConnectionString.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnectionString.FormattingEnabled = true;
            this.cmbConnectionString.Location = new System.Drawing.Point(107, 14);
            this.cmbConnectionString.Name = "cmbConnectionString";
            this.cmbConnectionString.Size = new System.Drawing.Size(212, 21);
            this.cmbConnectionString.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Connection String";
            // 
            // btnCreateConnections
            // 
            this.btnCreateConnections.Location = new System.Drawing.Point(323, 14);
            this.btnCreateConnections.Name = "btnCreateConnections";
            this.btnCreateConnections.Size = new System.Drawing.Size(26, 19);
            this.btnCreateConnections.TabIndex = 1;
            this.btnCreateConnections.Text = "...";
            this.btnCreateConnections.UseVisualStyleBackColor = true;
            this.btnCreateConnections.Click += new System.EventHandler(this.btnCreateConnections_Click);
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(587, 18);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(42, 13);
            this.lblInterval.TabIndex = 37;
            this.lblInterval.Text = "Interval";
            // 
            // numUpDnIntervalSql
            // 
            this.numUpDnIntervalSql.Location = new System.Drawing.Point(633, 16);
            this.numUpDnIntervalSql.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numUpDnIntervalSql.Name = "numUpDnIntervalSql";
            this.numUpDnIntervalSql.Size = new System.Drawing.Size(37, 20);
            this.numUpDnIntervalSql.TabIndex = 3;
            this.numUpDnIntervalSql.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lblMinutes
            // 
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Location = new System.Drawing.Point(671, 18);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new System.Drawing.Size(50, 13);
            this.lblMinutes.TabIndex = 39;
            this.lblMinutes.Text = "Minute(s)";
            // 
            // chkTestQueries
            // 
            this.chkTestQueries.AutoSize = true;
            this.chkTestQueries.Checked = true;
            this.chkTestQueries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTestQueries.Location = new System.Drawing.Point(5, 20);
            this.chkTestQueries.Margin = new System.Windows.Forms.Padding(2);
            this.chkTestQueries.Name = "chkTestQueries";
            this.chkTestQueries.Size = new System.Drawing.Size(93, 21);
            this.chkTestQueries.TabIndex = 15;
            this.chkTestQueries.Text = "&Test Queries";
            this.chkTestQueries.UseVisualStyleBackColor = true;
            // 
            // gbBottom
            // 
            this.gbBottom.Controls.Add(this.chkTestQueries);
            this.gbBottom.Controls.Add(this.btnCancel);
            this.gbBottom.Controls.Add(this.btnSave);
            this.gbBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbBottom.Location = new System.Drawing.Point(0, 426);
            this.gbBottom.Name = "gbBottom";
            this.gbBottom.Size = new System.Drawing.Size(725, 48);
            this.gbBottom.TabIndex = 1;
            this.gbBottom.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(377, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "C&lose";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(274, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkFirstRowIsHeader
            // 
            this.chkFirstRowIsHeader.AutoSize = true;
            this.chkFirstRowIsHeader.Location = new System.Drawing.Point(108, 52);
            this.chkFirstRowIsHeader.Name = "chkFirstRowIsHeader";
            this.chkFirstRowIsHeader.Size = new System.Drawing.Size(140, 21);
            this.chkFirstRowIsHeader.TabIndex = 14;
            this.chkFirstRowIsHeader.Text = "First row will be header";
            this.chkFirstRowIsHeader.UseVisualStyleBackColor = true;
            this.chkFirstRowIsHeader.Visible = false;
            // 
            // btnInterface
            // 
            this.btnInterface.Location = new System.Drawing.Point(597, 25);
            this.btnInterface.Name = "btnInterface";
            this.btnInterface.Size = new System.Drawing.Size(25, 23);
            this.btnInterface.TabIndex = 13;
            this.btnInterface.Text = "...";
            this.btnInterface.UseVisualStyleBackColor = true;
            this.btnInterface.Visible = false;
            this.btnInterface.Click += new System.EventHandler(this.btnInterface_Click);
            // 
            // txtInterfaceName
            // 
            this.txtInterfaceName.Location = new System.Drawing.Point(106, 26);
            this.txtInterfaceName.Name = "txtInterfaceName";
            this.txtInterfaceName.Size = new System.Drawing.Size(485, 20);
            this.txtInterfaceName.TabIndex = 12;
            this.txtInterfaceName.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(0, 29);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 13);
            this.label15.TabIndex = 40;
            this.label15.Text = "Interface Type";
            this.label15.Visible = false;
            // 
            // radInterface
            // 
            this.radInterface.AutoSize = true;
            this.radInterface.Location = new System.Drawing.Point(262, 3);
            this.radInterface.Name = "radInterface";
            this.radInterface.Size = new System.Drawing.Size(116, 20);
            this.radInterface.TabIndex = 11;
            this.radInterface.TabStop = true;
            this.radInterface.Text = "Interface (&Plug-In)";
            this.radInterface.UseVisualStyleBackColor = true;
            this.radInterface.CheckedChanged += new System.EventHandler(this.radInputData_CheckedChanged);
            // 
            // radInputData
            // 
            this.radInputData.AutoSize = true;
            this.radInputData.Checked = true;
            this.radInputData.Location = new System.Drawing.Point(108, 3);
            this.radInputData.Name = "radInputData";
            this.radInputData.Size = new System.Drawing.Size(146, 20);
            this.radInputData.TabIndex = 10;
            this.radInputData.TabStop = true;
            this.radInputData.Text = "&Direct Feed (Input Data)";
            this.radInputData.UseVisualStyleBackColor = true;
            this.radInputData.CheckedChanged += new System.EventHandler(this.radInputData_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(0, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Return Type";
            // 
            // btnTestRecoveryQuery
            // 
            this.btnTestRecoveryQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestRecoveryQuery.Enabled = false;
            this.btnTestRecoveryQuery.Location = new System.Drawing.Point(15, 76);
            this.btnTestRecoveryQuery.Name = "btnTestRecoveryQuery";
            this.btnTestRecoveryQuery.Size = new System.Drawing.Size(73, 23);
            this.btnTestRecoveryQuery.TabIndex = 8;
            this.btnTestRecoveryQuery.Text = "&Test";
            this.toolTip.SetToolTip(this.btnTestRecoveryQuery, "Test the update query. This will not commit anything to the database");
            this.btnTestRecoveryQuery.UseVisualStyleBackColor = true;
            this.btnTestRecoveryQuery.Click += new System.EventHandler(this.btnTestRecoveryQuery_Click);
            // 
            // btnTestUpdateQuery
            // 
            this.btnTestUpdateQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestUpdateQuery.Enabled = false;
            this.btnTestUpdateQuery.Location = new System.Drawing.Point(15, 70);
            this.btnTestUpdateQuery.Name = "btnTestUpdateQuery";
            this.btnTestUpdateQuery.Size = new System.Drawing.Size(73, 23);
            this.btnTestUpdateQuery.TabIndex = 4;
            this.btnTestUpdateQuery.Text = "&Test";
            this.toolTip.SetToolTip(this.btnTestUpdateQuery, "Test the update query. This will not commit anything to the database");
            this.btnTestUpdateQuery.UseVisualStyleBackColor = true;
            // 
            // pnlQuerySelect
            // 
            this.pnlQuerySelect.Controls.Add(this.btnTestUpdateQuery);
            this.pnlQuerySelect.Controls.Add(this.txtQueryUpdate);
            this.pnlQuerySelect.Controls.Add(this.lblUpdateQuery);
            this.pnlQuerySelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuerySelect.Location = new System.Drawing.Point(0, 43);
            this.pnlQuerySelect.Name = "pnlQuerySelect";
            this.pnlQuerySelect.Size = new System.Drawing.Size(725, 99);
            this.pnlQuerySelect.TabIndex = 46;
            // 
            // txtQueryUpdate
            // 
            this.txtQueryUpdate.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtQueryUpdate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtQueryUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQueryUpdate.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.errorProvider1.SetIconAlignment(this.txtQueryUpdate, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.txtQueryUpdate.Location = new System.Drawing.Point(106, 0);
            this.txtQueryUpdate.Name = "txtQueryUpdate";
            this.txtQueryUpdate.Size = new System.Drawing.Size(619, 99);
            this.txtQueryUpdate.TabIndex = 5;
            this.txtQueryUpdate.Text = "";
            this.txtQueryUpdate.TextChanged += new System.EventHandler(this.txtQueryUpdate_TextChanged);
            this.txtQueryUpdate.Enter += new System.EventHandler(this.txtQuery_Enter);
            this.txtQueryUpdate.Validated += new System.EventHandler(this.txtQueryUpdate_Validated);
            // 
            // lblUpdateQuery
            // 
            this.lblUpdateQuery.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblUpdateQuery.Location = new System.Drawing.Point(0, 0);
            this.lblUpdateQuery.Name = "lblUpdateQuery";
            this.lblUpdateQuery.Size = new System.Drawing.Size(106, 99);
            this.lblUpdateQuery.TabIndex = 46;
            this.lblUpdateQuery.Text = "Update Query \r\n(to mark data to \'Processing\' state)\r\n\r\n";
            this.lblUpdateQuery.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtQuerySelect
            // 
            this.txtQuerySelect.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtQuerySelect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtQuerySelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuerySelect.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.errorProvider1.SetIconAlignment(this.txtQuerySelect, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.txtQuerySelect.Location = new System.Drawing.Point(106, 0);
            this.txtQuerySelect.Name = "txtQuerySelect";
            this.txtQuerySelect.Size = new System.Drawing.Size(619, 99);
            this.txtQuerySelect.TabIndex = 7;
            this.txtQuerySelect.Text = "";
            this.txtQuerySelect.TextChanged += new System.EventHandler(this.txtQuerySelect_TextChanged);
            this.txtQuerySelect.Enter += new System.EventHandler(this.txtQuery_Enter);
            this.txtQuerySelect.Validated += new System.EventHandler(this.txtQuerySelect_Validated);
            // 
            // pnlQueryProcessing
            // 
            this.pnlQueryProcessing.Controls.Add(this.btnTestSqlQuery);
            this.pnlQueryProcessing.Controls.Add(this.txtQuerySelect);
            this.pnlQueryProcessing.Controls.Add(this.label12);
            this.pnlQueryProcessing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQueryProcessing.Location = new System.Drawing.Point(0, 148);
            this.pnlQueryProcessing.Name = "pnlQueryProcessing";
            this.pnlQueryProcessing.Size = new System.Drawing.Size(725, 99);
            this.pnlQueryProcessing.TabIndex = 48;
            // 
            // btnTestSqlQuery
            // 
            this.btnTestSqlQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestSqlQuery.Enabled = false;
            this.btnTestSqlQuery.Location = new System.Drawing.Point(15, 73);
            this.btnTestSqlQuery.Name = "btnTestSqlQuery";
            this.btnTestSqlQuery.Size = new System.Drawing.Size(73, 23);
            this.btnTestSqlQuery.TabIndex = 6;
            this.btnTestSqlQuery.Text = "&Test";
            this.btnTestSqlQuery.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Left;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 99);
            this.label12.TabIndex = 36;
            this.label12.Text = "Select Query\r\n(to fetch data for \'Processing\' - This is \'Original\' state)\r\n\r\n";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTestButton2
            // 
            this.pnlTestButton2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTestButton2.Location = new System.Drawing.Point(0, 247);
            this.pnlTestButton2.Name = "pnlTestButton2";
            this.pnlTestButton2.Size = new System.Drawing.Size(725, 6);
            this.pnlTestButton2.TabIndex = 49;
            // 
            // pnlQueryRecovery
            // 
            this.pnlQueryRecovery.Controls.Add(this.btnTestRecoveryQuery);
            this.pnlQueryRecovery.Controls.Add(this.txtQueryRecovery);
            this.pnlQueryRecovery.Controls.Add(this.label2);
            this.pnlQueryRecovery.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQueryRecovery.Location = new System.Drawing.Point(0, 253);
            this.pnlQueryRecovery.Name = "pnlQueryRecovery";
            this.pnlQueryRecovery.Size = new System.Drawing.Size(725, 99);
            this.pnlQueryRecovery.TabIndex = 50;
            // 
            // txtQueryRecovery
            // 
            this.txtQueryRecovery.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtQueryRecovery.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtQueryRecovery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQueryRecovery.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.errorProvider1.SetIconAlignment(this.txtQueryRecovery, System.Windows.Forms.ErrorIconAlignment.TopLeft);
            this.txtQueryRecovery.Location = new System.Drawing.Point(106, 0);
            this.txtQueryRecovery.Name = "txtQueryRecovery";
            this.txtQueryRecovery.Size = new System.Drawing.Size(619, 99);
            this.txtQueryRecovery.TabIndex = 9;
            this.txtQueryRecovery.Text = "";
            this.txtQueryRecovery.TextChanged += new System.EventHandler(this.txtQueryRecovery_TextChanged);
            this.txtQueryRecovery.Enter += new System.EventHandler(this.txtQuery_Enter);
            this.txtQueryRecovery.Validated += new System.EventHandler(this.txtQueryRecovery_Validated);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 99);
            this.label2.TabIndex = 47;
            this.label2.Text = "Recovery Query \r\n(to mark data from \'Processing\' state to \'Error\' state in case o" +
    "f errors)\r\n\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTestButton3
            // 
            this.pnlTestButton3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTestButton3.Location = new System.Drawing.Point(0, 352);
            this.pnlTestButton3.Name = "pnlTestButton3";
            this.pnlTestButton3.Size = new System.Drawing.Size(725, 6);
            this.pnlTestButton3.TabIndex = 51;
            // 
            // pnlOthers
            // 
            this.pnlOthers.Controls.Add(this.chkFirstRowIsHeader);
            this.pnlOthers.Controls.Add(this.txtInterfaceName);
            this.pnlOthers.Controls.Add(this.radInterface);
            this.pnlOthers.Controls.Add(this.label11);
            this.pnlOthers.Controls.Add(this.label15);
            this.pnlOthers.Controls.Add(this.btnInterface);
            this.pnlOthers.Controls.Add(this.radInputData);
            this.pnlOthers.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOthers.Location = new System.Drawing.Point(0, 358);
            this.pnlOthers.Name = "pnlOthers";
            this.pnlOthers.Size = new System.Drawing.Size(725, 78);
            this.pnlOthers.TabIndex = 52;
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // pnlTestButton1
            // 
            this.pnlTestButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTestButton1.Location = new System.Drawing.Point(0, 142);
            this.pnlTestButton1.Name = "pnlTestButton1";
            this.pnlTestButton1.Size = new System.Drawing.Size(725, 6);
            this.pnlTestButton1.TabIndex = 47;
            // 
            // SqlConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOthers);
            this.Controls.Add(this.pnlTestButton3);
            this.Controls.Add(this.pnlQueryRecovery);
            this.Controls.Add(this.pnlTestButton2);
            this.Controls.Add(this.pnlQueryProcessing);
            this.Controls.Add(this.pnlTestButton1);
            this.Controls.Add(this.pnlQuerySelect);
            this.Controls.Add(this.gbBottom);
            this.Controls.Add(this.gbTop);
            this.Name = "SqlConfiguration";
            this.Size = new System.Drawing.Size(725, 474);
            this.gbTop.ResumeLayout(false);
            this.gbTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDnIntervalSql)).EndInit();
            this.gbBottom.ResumeLayout(false);
            this.gbBottom.PerformLayout();
            this.pnlQuerySelect.ResumeLayout(false);
            this.pnlQueryProcessing.ResumeLayout(false);
            this.pnlQueryRecovery.ResumeLayout(false);
            this.pnlOthers.ResumeLayout(false);
            this.pnlOthers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTop;
        private System.Windows.Forms.GroupBox gbBottom;
        private System.Windows.Forms.Button btnCreateConnections;
        private System.Windows.Forms.CheckBox chkFirstRowIsHeader;
        private System.Windows.Forms.Button btnInterface;
        private System.Windows.Forms.TextBox txtInterfaceName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblMinutes;
        private System.Windows.Forms.NumericUpDown numUpDnIntervalSql;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.RadioButton radInterface;
        private System.Windows.Forms.RadioButton radInputData;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbConnectionString;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel pnlQuerySelect;
        private System.Windows.Forms.Panel pnlQueryProcessing;
        private System.Windows.Forms.Panel pnlTestButton2;
        private System.Windows.Forms.Panel pnlQueryRecovery;
        private System.Windows.Forms.Panel pnlTestButton3;
        private System.Windows.Forms.Button btnTestRecoveryQuery;
        private System.Windows.Forms.Panel pnlOthers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtQuerySelect;
        private System.Windows.Forms.RichTextBox txtQueryUpdate;
        private System.Windows.Forms.RichTextBox txtQueryRecovery;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.CheckBox chkTestQueries;
        private System.Windows.Forms.TextBox txtRunTimeCononectionStringName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlTestButton1;
        private System.Windows.Forms.Button btnTestUpdateQuery;
        private System.Windows.Forms.Label lblUpdateQuery;
        private System.Windows.Forms.Button btnTestSqlQuery;
        private System.Windows.Forms.Label label12;
    }
}
