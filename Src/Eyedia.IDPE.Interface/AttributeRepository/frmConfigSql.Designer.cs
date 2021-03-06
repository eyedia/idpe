﻿namespace Eyedia.IDPE.Interface
{
    partial class frmConfigSql
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigSql));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pullSqlConfiguration1 = new Eyedia.IDPE.Interface.Controls.SqlConfiguration();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.idpeRulesEditorControl1 = new Eyedia.IDPE.Interface.RulesEditorControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1111, 782);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pullSqlConfiguration1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1103, 749);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pullSqlConfiguration1
            // 
            this.pullSqlConfiguration1.ConnectionStringKeyName = "";
            this.pullSqlConfiguration1.DataSourceId = 0;
            this.pullSqlConfiguration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pullSqlConfiguration1.Location = new System.Drawing.Point(3, 3);
            this.pullSqlConfiguration1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.pullSqlConfiguration1.Name = "pullSqlConfiguration1";
            this.pullSqlConfiguration1.PusherMode = false;
            this.pullSqlConfiguration1.Size = new System.Drawing.Size(1097, 743);
            this.pullSqlConfiguration1.TabIndex = 0;
            this.pullSqlConfiguration1.UpdateQuery = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.idpeRulesEditorControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1103, 749);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // idpeRulesEditorControl1
            // 
            this.idpeRulesEditorControl1.DataSource = null;
            this.idpeRulesEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idpeRulesEditorControl1.Location = new System.Drawing.Point(3, 3);
            this.idpeRulesEditorControl1.Name = "idpeRulesEditorControl1";
            this.idpeRulesEditorControl1.ShowSqlInitRules = true;
            this.idpeRulesEditorControl1.Size = new System.Drawing.Size(1097, 743);
            this.idpeRulesEditorControl1.TabIndex = 0;
            // 
            // frmConfigSql
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 782);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmConfigSql";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Sql Pull";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmConfigSql_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public Controls.SqlConfiguration pullSqlConfiguration1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private RulesEditorControl idpeRulesEditorControl1;
    }
}