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
using Symplus.Core;
using Symplus.RuleEngine.DataManager;
using Symplus.RuleEngine.Common;

namespace Symplus.RuleEngine.Utilities
{
    public partial class frmExportImportApplication : Form
    {
        bool _Working;
        bool _Exporting;
        SreDataSource _Application;
        Manager _DataManager;
        public frmExportImportApplication(int applicationId)
        {
            InitializeComponent();
            _Exporting = true;
            Init(applicationId);
        }

        public frmExportImportApplication()
        {
            InitializeComponent();
            _Exporting = false;
            Init(null);
        }

        void Init(int? applicationId)
        {
            _DataManager = new Manager();
            if (applicationId != null)
            {
                _Application = _DataManager.GetApplicationDetails((int)applicationId);
                lblFrom.Text = _Application.Name;
                txtExportFile.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _Application.Name + ".srex");
            }
            if (_Exporting)
            {
                this.Text = "Export Application";
                label1.Visible = true;
                label3.Text = "To";
                lblFrom.Visible = true;
            }
            else
            {
                this.Text = "Import Application";
                label1.Visible = false;
                label3.Text = "From";
                lblFrom.Visible = false;
                txtExportFile.Text = "";
            }
        }

        private void frmExportApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Working)
                e.Cancel = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (_Exporting)
                {
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
                    ExportApplication();
                }
                else
                {
                    ImportApplication();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
                toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
                toolStripProgressBar1.Visible = false;
                _Working = false;
            }

        }


        private void txtApplicationName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (btnOK.Enabled))
                btnOK_Click(null, null);
        }

        public string ApplicationName { get; set; }


        private void ExportApplication()
        {
            Manager utilDataManager = new Manager();
            int applicationId = _Application.Id;
            _Working = true;
            btnOK.Enabled = false;
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Increment(10);
            toolStripStatusLabel1.Text = "Exporting data source...";
            Application.DoEvents();
            DataSourceBundle exportApp = new DataSourceBundle();
            exportApp.DataSource = _Application;
            exportApp.DataSource.Id = 0;

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Exporting attributes...";
            Application.DoEvents();
            exportApp.Attributes = _DataManager.GetAttributes(applicationId);
            foreach (SreAttribute attrib in exportApp.Attributes)
            {
                attrib.AttributeId = 0;
            }
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            
            DataTable dt = utilDataManager.GetAttributes2(applicationId);
           // dt.Columns["Name"].ColumnName = "AttriButeName";
            dt.WriteXml(sw, XmlWriteMode.WriteSchema);
            sw.Close();
            exportApp.DataSourceAttributes = sb.ToString();

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Exporting rules...";
            Application.DoEvents();
            exportApp.RuleSets = _DataManager.GetApplicationRuleSets(applicationId);
            foreach (SreRule rs in exportApp.RuleSets)
            {
                rs.Id = 0;
            }

            sb = new StringBuilder();
            sw = new StringWriter(sb);

            List<SreRule> sr = utilDataManager.GetRules(applicationId);

            Type elementType = typeof(SreRule);


            DataTable dtrules = new DataTable("DataSourceAttributes");

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                if (propInfo.PropertyType.Name == "Nullable`1")
                    dtrules.Columns.Add(propInfo.Name);
                else
                    dtrules.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            
            foreach (SreRule item in sr)
            {
                DataRow row = dtrules.NewRow();
                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null);
                }

                dtrules.Rows.Add(row);
            }

            dtrules.WriteXml(sw, XmlWriteMode.WriteSchema);
            sw.Close();
            exportApp.DataSourceRuleSets = sb.ToString();

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Exporting keys...";
            Application.DoEvents();
            exportApp.Keys = _DataManager.GetApplicationKeys(applicationId, false);
            foreach (SreKey key in exportApp.Keys)
            {
                key.KeyId = 0;
            }

            sb = new StringBuilder();
            sw = new StringWriter(sb);
            dt = utilDataManager.GetKeys2(applicationId);
            dt.WriteXml(sw, XmlWriteMode.WriteSchema);
            sw.Close();
            exportApp.DataSourceKeys = sb.ToString();

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Creating file...";
            Application.DoEvents();
            StreamWriter sw1 = new StreamWriter(txtExportFile.Text);
            sw1.Write(exportApp.Serialize<DataSourceBundle>());
            sw1.Close();

            toolStripProgressBar1.Increment(10);
            toolStripStatusLabel1.Text = "Done";
            btnOK.Enabled = true;
            _Working = false;
            toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
            Application.DoEvents();


        }

        private void ImportApplication()
        {

            _Working = true;
            btnOK.Enabled = false;
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Increment(10);
            toolStripStatusLabel1.Text = "Reading file...";
            Application.DoEvents();
            StreamReader sr = new StreamReader(txtExportFile.Text);
            string fileContent = sr.ReadToEnd();
            sr.Close();
            DataSourceBundle importedApp = fileContent.Deserialize<DataSourceBundle>();
            if (importedApp == null)
            {
                toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
                Application.DoEvents();
                toolStripProgressBar1.Visible = false;
                btnOK.Enabled = true;
                throw new Exception("Could not read data from file. File might be corrupt.");
            }

            Manager utilDataManager = new Manager();

            ApplicationName = importedApp.DataSource.Name;
            if (utilDataManager.ApplicationExists(importedApp.DataSource.Name))
            {
                toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
                Application.DoEvents();
                toolStripProgressBar1.Visible = false;
                btnOK.Enabled = true;
                throw new Exception("Application already exists!");
            }


            toolStripProgressBar1.Increment(10);
            toolStripStatusLabel1.Text = "Importing application...";
            Application.DoEvents();
            List<SreRuleDataSource> appRules = new List<SreRuleDataSource>();
            utilDataManager.Save(importedApp.DataSource, appRules);
            //importedApp.Application.Id = 104;

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Importing attributes...";
            Application.DoEvents();

            DataTable dt = StringToDataTable(importedApp.DataSourceAttributes);

            List<SreAttributeDataSource> appAttribs = new List<SreAttributeDataSource>();
            int count = 1;
            foreach (SreAttribute attrib in importedApp.Attributes)
            {
                int attributeId = utilDataManager.Save(attrib);

                SreAttributeDataSource saa = new SreAttributeDataSource();
                saa.DataSourceId = importedApp.DataSource.Id;
                saa.AttributeId = attributeId;
                string str = GetApplicationAttribute(dt, attrib.Name);
                saa.Position = int.Parse(str.Split("|".ToCharArray())[0]);
                saa.IsAcceptable = bool.Parse(str.Split("|".ToCharArray())[1]);
                appAttribs.Add(saa);
                count++;
            }

            utilDataManager.SaveAssociations(importedApp.DataSource.Id, appAttribs);

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Importing rules...";
            Application.DoEvents();

            dt = StringToDataTable(importedApp.DataSourceRuleSets);

            foreach (SreRule ruleSet in importedApp.RuleSets)
            {
                int ruleSetId = utilDataManager.Save(ruleSet);

                SreRuleDataSource sra = new SreRuleDataSource();
                sra.DataSourceId = importedApp.DataSource.Id;
                sra.RuleId = ruleSetId;
                sra.IsActive = true;
                utilDataManager.Save(null, null, null, sra);

            }
            utilDataManager.Save(importedApp.DataSource, appRules);

            toolStripProgressBar1.Increment(20);
            toolStripStatusLabel1.Text = "Importing keys...";
            Application.DoEvents();

            dt = StringToDataTable(importedApp.DataSourceKeys);

            foreach (SreKey key in importedApp.Keys)
            {
                key.IsDefault = IsKeyIsDefault(dt, key.Name);
                utilDataManager.Save(importedApp.DataSource.Id, key);
            }

            toolStripProgressBar1.Increment(10);
            toolStripStatusLabel1.Text = "Done";
            btnOK.Enabled = true;
            _Working = false;
            toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
            Application.DoEvents();


        }

        string GetApplicationAttribute(DataTable applicationAttributes, string attributeName)
        {
            foreach (DataRow row in applicationAttributes.Rows)
            {
                if (row["name"].ToString() == attributeName)
                    return string.Format("{0}|{1}", row["Position"], row["IsAcceptable"]);
            }
            return "0|false"; ;
        }

        int GetRuleSetType(DataTable applicationRuleSets, string ruleSetName)
        {
            foreach (DataRow row in applicationRuleSets.Rows)
            {
                if (row["name"].ToString() == ruleSetName)
                    return int.Parse(row["RulesetType"].ToString());
            }
            return 0;
        }

        bool IsKeyIsDefault(DataTable applicationKeys, string keyName)
        {
            foreach (DataRow row in applicationKeys.Rows)
            {
                if (row["name"].ToString() == keyName)
                    return bool.Parse(row["IsDefault"].ToString());
            }
            return false;
        }


        public DataTable StringToDataTable(string xmlData)
        {
            StringReader theReader = new StringReader(xmlData);
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(theReader);
            return theDataSet.Tables[0];
        }

        private void frmCopyApplication_KeyUp(object sender, KeyEventArgs e)
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
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    txtExportFile.Text = saveFileDialog1.FileName;
            }
            else
            {
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog1.Filter = "SRE Exported files (*.srex)|*.srex|All files (*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    txtExportFile.Text = openFileDialog1.FileName;
            }
        }
    }

}





