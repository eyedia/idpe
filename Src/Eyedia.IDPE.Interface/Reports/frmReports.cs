#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Deb'jyoti Das
Created - 3/19/2013 11:14:16 AM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eyedia.IDPE.DataManager;
using Microsoft.Reporting.WinForms;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface
{
    public partial class frmReports : Form
    {
        public enum ReportTypes
        {
            Attributes,
            AttributesParentChild,
            DataSources
        }

        int DataSourceId;
        ReportTypes ReportType;

        public frmReports(ReportTypes reportType, int dataSourceId = 0)
        {
            InitializeComponent();
           
            DataSourceId = dataSourceId;

            reportViewerAttributes.Dock = DockStyle.Fill;
            reportViewerDataSources.Dock = DockStyle.Fill;
            reportViewerAttributesParentChild.Dock = DockStyle.Fill;
            reportViewerAttributesParentChild.LocalReport.EnableExternalImages = true;

           
            ReportParameter p1 = new ReportParameter("UserName", Information.LoggedInUser.UserName);
            
            string image = "file:///" + GetImageFileName();
            reportViewerAttributes.LocalReport.SetParameters(p1);
            reportViewerDataSources.LocalReport.SetParameters(new ReportParameter[] { p1 });            
            reportViewerAttributesParentChild.LocalReport.SetParameters(p1);
            //reportViewerAttributesParentChild.LocalReport.SetParameters(new ReportParameter("MapImage", @"file:///C:\temp\Map_Arrow.png"));
            reportViewerAttributesParentChild.LocalReport.SetParameters(new ReportParameter("MapImage", image));

            ReportType = reportType;            
            BindData();
        }

        private string GetImageFileName()
        {
            string dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string fileName = System.IO.Path.Combine(dir, "Map_Arrow_Right.png");
            if(!System.IO.File.Exists(fileName))
                Properties.Resources.Map_Arrow_Right.Save(fileName);
            return fileName;
        }
        private void frmReports_Load(object sender, EventArgs e)
        {
            this.reportViewerAttributes.RefreshReport();
            this.reportViewerDataSources.RefreshReport();
            this.reportViewerAttributesParentChild.RefreshReport();
        }

        void BindData()
        {
            Manager manager = new Manager();
            string dsName = manager.GetApplicationName(DataSourceId);
            switch (ReportType)
            {
                case ReportTypes.AttributesParentChild:                    

                    int parentDataSourceId = manager.GetDataSourceParentId(DataSourceId);                    
                    string dsNameParent = manager.GetApplicationName(parentDataSourceId);

                    reportViewerAttributesParentChild.LocalReport.DisplayName = dsName + "_map_" + dsNameParent;
                    reportViewerAttributesParentChild.LocalReport.SetParameters(new ReportParameter("DSName", dsName));
                    reportViewerAttributesParentChild.LocalReport.SetParameters(new ReportParameter("DSNameParent", dsNameParent));
                    this.Text = "Mapping Details of " + dsName;
                    ReportDataSource rd1 = new ReportDataSource("dsAttributes", manager.GetAttributesAsDataTable(parentDataSourceId));
                    ReportDataSource rd2 = new ReportDataSource("dsAttributesChild", manager.GetAttributesAsDataTable(DataSourceId, true));

                    reportViewerAttributesParentChild.LocalReport.DataSources.Add(rd1);
                    reportViewerAttributesParentChild.LocalReport.DataSources.Add(rd2);
                    reportViewerAttributes.Visible = false;
                    reportViewerAttributesParentChild.Visible = true;
                    reportViewerDataSources.Visible = false;
                    break;

                case ReportTypes.Attributes:
                    this.Text = "Attributes";
                    SreAttributeBindingSource.DataSource = new Manager().GetAttributesAsDataTable(DataSourceId);
                    reportViewerAttributes.LocalReport.DisplayName = dsName;
                    reportViewerAttributes.Visible = true;
                    reportViewerAttributesParentChild.Visible = false;
                    reportViewerDataSources.Visible = false;
                    break;

                case ReportTypes.DataSources:
                    this.Text = "DataSources";
                    SreDataSourceBindingSource.DataSource = new Manager().GetDataSourcesAsDataTable();

                    reportViewerDataSources.LocalReport.DisplayName = dsName;
                    reportViewerAttributes.Visible = false;
                    reportViewerAttributesParentChild.Visible = false;
                    reportViewerDataSources.Visible = true;

                    break;
            }
        }
    }
}


