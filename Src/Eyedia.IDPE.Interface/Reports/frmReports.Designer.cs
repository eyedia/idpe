namespace Eyedia.IDPE.Interface
{
    partial class frmReports
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReports));
            this.SreAttributeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.SreDataSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewerAttributes = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportViewerDataSources = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportViewerAttributesParentChild = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.SreAttributeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SreDataSourceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // SreAttributeBindingSource
            // 
            this.SreAttributeBindingSource.DataSource = typeof(Eyedia.IDPE.DataManager.SreAttribute);
            // 
            // SreDataSourceBindingSource
            // 
            this.SreDataSourceBindingSource.DataSource = typeof(Eyedia.IDPE.DataManager.SreDataSource);
            // 
            // reportViewerAttributes
            // 
            reportDataSource1.Name = "dsAttributes";
            reportDataSource1.Value = this.SreAttributeBindingSource;
            this.reportViewerAttributes.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewerAttributes.LocalReport.ReportEmbeddedResource = "Eyedia.IDPE.Interface.Reports.rdlcAttributes.rdlc";
            this.reportViewerAttributes.Location = new System.Drawing.Point(22, 12);
            this.reportViewerAttributes.Name = "reportViewerAttributes";
            this.reportViewerAttributes.Size = new System.Drawing.Size(200, 80);
            this.reportViewerAttributes.TabIndex = 0;
            // 
            // reportViewerDataSources
            // 
            reportDataSource2.Name = "dsDataSource";
            reportDataSource2.Value = this.SreDataSourceBindingSource;
            this.reportViewerDataSources.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewerDataSources.LocalReport.ReportEmbeddedResource = "Eyedia.IDPE.Interface.Reports.rdlcDataSources.rdlc";
            this.reportViewerDataSources.Location = new System.Drawing.Point(253, 12);
            this.reportViewerDataSources.Name = "reportViewerDataSources";
            this.reportViewerDataSources.Size = new System.Drawing.Size(183, 80);
            this.reportViewerDataSources.TabIndex = 1;
            // 
            // reportViewerAttributesParentChild
            // 
            this.reportViewerAttributesParentChild.LocalReport.ReportEmbeddedResource = "Eyedia.IDPE.Interface.Reports.rdlcAttributesParentChild.rdlc";
            this.reportViewerAttributesParentChild.Location = new System.Drawing.Point(22, 116);
            this.reportViewerAttributesParentChild.Name = "reportViewerAttributesParentChild";
            this.reportViewerAttributesParentChild.Size = new System.Drawing.Size(200, 80);
            this.reportViewerAttributesParentChild.TabIndex = 2;
            // 
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 568);
            this.Controls.Add(this.reportViewerAttributesParentChild);
            this.Controls.Add(this.reportViewerDataSources);
            this.Controls.Add(this.reportViewerAttributes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReports";
            this.Text = "frmReports";
            this.Load += new System.EventHandler(this.frmReports_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SreAttributeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SreDataSourceBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewerAttributes;
        private System.Windows.Forms.BindingSource SreAttributeBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewerDataSources;
        private System.Windows.Forms.BindingSource SreDataSourceBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewerAttributesParentChild;
    }
}