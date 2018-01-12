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
using System.IO;
using System.Windows.Forms;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class frmExportImport : Form
    {
        bool _Working;
        bool _Exporting;
        IdpeDataSource _DataSource;       
        public frmExportImport(int dataSourceId)
        {
            InitializeComponent();           
            _Exporting = true;
            Init(dataSourceId);
            btnBrowse.Height = txtExportFile.Height;
        }

        public frmExportImport()
        {
            InitializeComponent();
            ;

            _Exporting = false;
            Init(null);
        }

        void Init(int? dataSourceId)
        {            
            if ((dataSourceId != null)
                && (dataSourceId != 0))
            {
                _DataSource = new Manager().GetDataSourceDetails((int)dataSourceId);
                lblFrom.Text = _DataSource.Name;
                txtExportFile.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _DataSource.Name + ".srex");
            }
            if (_Exporting)
            {
                this.Text = "Export Data Source";
                label1.Visible = true;
                label3.Text = "To";
                lblFrom.Visible = true;
            }
            else
            {
                this.Text = "Import Data Source";
                label1.Visible = false;
                label3.Text = "From";
                lblFrom.Visible = false;
                txtExportFile.Text = "";

                label3.Top = label1.Top;
                txtExportFile.Top = label1.Top;
                btnBrowse.Top = label1.Top;
                label1.Visible = false;
                lblFrom.Visible = false;
                pnlText.Height = pnlText.Height / 2;
                this.Height = this.Height - pnlText.Height;
            }
        }

        private void frmExportApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Working)
                e.Cancel = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string operation = string.Empty;
            try
            {
                if (_Exporting)
                {
                    operation = " export ";
                    if (File.Exists(txtExportFile.Text))
                    {
                        if (MessageBox.Show("The file already exists. Do you want to overwrite the file?", "File Already Exists",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            File.Delete(txtExportFile.Text);
                        }
                        else
                        {
                            return;
                        }
                    }
                    DataSourceBundle dsb = new DataSourceBundle();
                    dsb.Export(this._DataSource.Id, txtExportFile.Text);

                }
                else
                {
                    operation = " import ";
                    DataSourceBundle dsb = new DataSourceBundle(txtExportFile.Text);
                    Manager manager = new Manager();
                    if (manager.ApplicationExists(dsb.DataSource.Name))
                    {
                        if (MessageBox.Show("A data source with name '" + dsb.DataSource.Name + "' already exist! Do you want to overwrite?", "Import Data Source"
                            , MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            manager.DeleteDataSource(dsb.DataSource.Name);
                            dsb.Import();
                        }
                    }
                    else
                    {
                        dsb.Import();
                    }
                    
                    
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
                toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
                toolStripProgressBar1.Visible = false;

                string message = string.Empty;
                if(ex is BusinessException)
                    message = "Failed to" + operation + "!" + Environment.NewLine + ex.Message;
                else
                    message = "Failed to" + operation + "!" + Environment.NewLine + ex.ToString();

                MessageBox.Show(message,operation + " falied" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                _Working = false;
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            }

        }


        private void txtApplicationName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (btnOK.Enabled))
                btnOK_Click(null, null);
        }

        public string ApplicationName { get; set; }
              

        string GetDataSourceAttribute(DataTable DataSourceAttributes, string attributeName)
        {
            foreach (DataRow row in DataSourceAttributes.Rows)
            {
                if (row["name"].ToString() == attributeName)
                    return string.Format("{0}|{1}", row["Position"], row["IsAcceptable"]);
            }
            return "0|false"; ;
        }

        int GetRuleSetType(DataTable DataSourceRuleSets, string ruleSetName)
        {
            foreach (DataRow row in DataSourceRuleSets.Rows)
            {
                if (row["name"].ToString() == ruleSetName)
                    return int.Parse(row["RulesetType"].ToString());
            }
            return 0;
        }

        public DataTable StringToDataTable(string xmlData)
        {
            StringReader theReader = new StringReader(xmlData);
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(theReader);
            return theDataSet.Tables[0];
        }

        private void frmCopyDataSource_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Escape)
                && (_Working == false))
                this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (_Exporting)
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (!(string.IsNullOrEmpty(txtExportFile.Text)))
                    dir = Path.GetDirectoryName(txtExportFile.Text);

                saveFileDialog1.InitialDirectory = dir;
                saveFileDialog1.Filter = "SRE Exported files (*.srex)|*.srex|All files (*.*)|*.*";
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    txtExportFile.Text = saveFileDialog1.FileName;
            }
            else
            {
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog1.Filter = "SRE Exported files (*.srex)|*.srex|All files (*.*)|*.*";
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    txtExportFile.Text = openFileDialog1.FileName;
            }
        }
    }

}





