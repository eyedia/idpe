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
using System.IO;
using System.Diagnostics;
using Eyedia.IDPE.Services;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Windows.Forms.Design;

namespace Eyedia.IDPE.Interface
{

    public partial class frmConfigEDI : Form
    {
        public const string _blueKeyWords = "\\b(xsl:stylesheet|xsl:output|xsl:template|xsl:apply-templates|xsl:value-of|xsl:variable|xsl:text)\\b";
        public const string _redKeyWords = "\\b(xml|version|encoding|xmlns:xsl|xmlns:msxsl|exclude-result-prefixes|method|indent|match|select|name)\\b";

        public int DataSourceId { get; private set; }
        public IWindowsFormsEditorService _WindowsFormsEditorService;
        int pnlTopHeightOriginal;
        const int pnlTopHeightShrink = 24;

        public frmConfigEDI(int dataSourceId, IWindowsFormsEditorService windowsFormsEditorService = null)
        {
            InitializeComponent();
            ;

            _WindowsFormsEditorService = windowsFormsEditorService;
            if (_WindowsFormsEditorService != null)
                TopLevel = false;

            DataSourceId = dataSourceId;

            rtbSampleOutput.WordWrap = false;
            cmbDelmiter.Items.Add(",");
            cmbDelmiter.Items.Add("|");
            cmbDelmiter.Items.Add("Tab");

            pnlTopHeightOriginal = pnlMainTop.Height;
            pnlMainTop.Height = pnlTopHeightShrink;
            
            BindData();
        }

        private void BindData()
        {
            Manager manager = new Manager();
            SreDataSource ds = manager.GetDataSourceDetails(DataSourceId);
            if ((ds.Delimiter != null)
                && (ds.Delimiter.ToLower() == "\t"))
                cmbDelmiter.Text = "Tab";
            else
                cmbDelmiter.Text = ds.Delimiter;

            List<SreKey> keys = manager.GetKeys(DataSourceId);
            rtbXslt.Text = keys.GetKeyValue(SreKeyTypes.EDIX12Xslt);
            SyntaxHighLighter.HighLight(rtbXslt, _blueKeyWords, _redKeyWords);

            string strHeader = keys.GetKeyValue(SreKeyTypes.IsFirstRowHeader);
            if (!string.IsNullOrEmpty(strHeader))
            {
                bool boolVal = false;
                bool.TryParse(strHeader, out boolVal);
                chkFileHasHeader.Checked = boolVal;
            }
            chkRenameHeaders.Enabled = chkFileHasHeader.Checked;

            string strRenCol = keys.GetKeyValue(SreKeyTypes.RenameColumnHeader);
            if (!string.IsNullOrEmpty(strRenCol))
            {
                bool boolVal = false;
                bool.TryParse(strRenCol, out boolVal);
                chkRenameHeaders.Checked = boolVal;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "XSLT files|*.xslt|All files|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    rtbXslt.Text = sr.ReadToEnd();
                    sr.Close();
                    SyntaxHighLighter.HighLight(rtbXslt, _blueKeyWords, _redKeyWords);
                }
            }
        }

        private void rtbXslt_TextChanged(object sender, EventArgs e)
        {
            ValidateTransformReady(sender, e);
            btnExportXslt.Enabled = rtbXslt.Text.Length > 5;

            if ((backgroundWorker1.IsBusy == false)
                && (btnTransform.Enabled == true))
                StartbackgroundWorker();
        }

        private void btnTransform_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                StreamReader sr = new StreamReader(txtSampleFile.Text);
                string fileContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();

                EdiX12Parser ediParser = new EdiX12Parser(txtSampleFile.Text, fileContent);
                rtbSampleOutput.Text = ediParser.Parse(rtbXslt.Text).ToString();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            this.Cursor = Cursors.Default;
        }


        private void ValidateTransformReady(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(txtSampleFile.Text) == false)
                && (File.Exists(txtSampleFile.Text)))
                btnShowXml.Enabled = true;
            else
                btnShowXml.Enabled = false;

            if ((string.IsNullOrEmpty(txtSampleFile.Text) == false)
                && (string.IsNullOrEmpty(rtbXslt.Text) == false)
                && (File.Exists(txtSampleFile.Text)))
                btnTransform.Enabled = true;
            else
                btnTransform.Enabled = false;
        }

        private void btnBrowseSampleFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "EDI files|*.edi|DAT files|*.dat|All files|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSampleFile.Text = openFileDialog.FileName;
            }
        }

        private void btnLoadSamples_Click(object sender, EventArgs e)
        {
            frmConfigEDIExtension ediSamples = new frmConfigEDIExtension(this.Icon);
            if (ediSamples.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSampleFile.Text = ediSamples.EDIFileName;
                rtbXslt.Text = ediSamples.Xslt;
                SyntaxHighLighter.HighLight(rtbXslt, _blueKeyWords, _redKeyWords);
            }
        }

        private void MainTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainTabs.SelectedIndex == 1)
            {
                headerFooterConfiguration1.DataFormatType = DataFormatTypes.Delimited;
                headerFooterConfiguration1.DataSourceId = DataSourceId;

            }
        }

        private void StartbackgroundWorker()
        {
            rtbSampleOutput.Text = "";
            EDIInformation para = new EDIInformation(txtSampleFile.Text, rtbXslt.Text);
            backgroundWorker1.RunWorkerAsync(para);
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                EDIInformation par = e.Argument as EDIInformation;

                StreamReader sr = new StreamReader(par.EDIFileName);
                string fileContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();

                EdiX12Parser ediTransformer = new EdiX12Parser(par.EDIFileName, fileContent);
                e.Result = ediTransformer.Parse(par.Xslt).ToString();
            }
            catch (Exception ex)
            {
                e.Result = "Error:" + ex.Message;

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = e.Result.ToString();
            if (result.StartsWith("Error:"))
            {
                toolStripStatusLabel1.Text = result.Substring(result.IndexOf("Error:"));
                toolStripStatusLabel1.ForeColor = Color.Red;
            }
            else
            {
                rtbSampleOutput.Text = result;
                toolStripStatusLabel1.Text = string.Empty;
                toolStripStatusLabel1.ForeColor = Color.Red;
            }

        }


        private void timerIdle_Tick(object sender, EventArgs e)
        {

            SyntaxHighLighter.HighLight(rtbXslt, _blueKeyWords, _redKeyWords);
        }

        private void btnShowXml_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(txtSampleFile.Text);
            string fileContent = sr.ReadToEnd();
            sr.Close();

            EdiX12Parser ediParser = new EdiX12Parser(txtSampleFile.Text, fileContent);
            string xmlFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(txtSampleFile.Text) + ".xml");

            StreamWriter sw = new StreamWriter(xmlFile);
            sw.Write(ediParser.GenerateXml());
            sw.Close();

            Process.Start(xmlFile);

        }

        private void headerFooterConfiguration1_Saved(object sender, EventArgs e)
        {
            Manager manager = new Manager();

            string delimiter = cmbDelmiter.Text;
            if (delimiter.ToLower() == "tab")
                delimiter = "\t";
            manager.UpdateDelimiter(DataSourceId, delimiter);

            SreKey key = new SreKey();
            key.Name = SreKeyTypes.IsFirstRowHeader.ToString();
            key.Value = chkFileHasHeader.Checked.ToString();
            key.Type = (int)SreKeyTypes.IsFirstRowHeader;
            manager.Save(key, DataSourceId);

            if (chkRenameHeaders.Checked)
            {
                key = new SreKey();
                key.Name = SreKeyTypes.RenameColumnHeader.ToString();
                key.Value = "True";
                key.Type = (int)SreKeyTypes.RenameColumnHeader;
                manager.Save(key, DataSourceId);
            }
            else
            {
                manager.DeleteKeyFromApplication(DataSourceId, SreKeyTypes.RenameColumnHeader.ToString(), true);
            }

            key = new SreKey();
            key.Name = SreKeyTypes.EDIX12Xslt.ToString();
            key.Value = rtbXslt.Text;
            key.Type = (int)SreKeyTypes.EDIX12Xslt;
            manager.Save(key, DataSourceId);

        }

        private void chkFileHasHeader_CheckedChanged(object sender, EventArgs e)
        {
            chkRenameHeaders.Enabled = chkFileHasHeader.Checked;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            pnlTips.Visible = !pnlTips.Visible;
            toolTip1.SetToolTip(btnHelp, pnlTips.Visible ? "Hides Tips [F1]" : "Shows Tips [F1]");
        }

        private void rtbSampleOutput_TextChanged(object sender, EventArgs e)
        {
            btnExportSampleFile.Enabled = rtbSampleOutput.Text.Length > 5;
        }

        private void btnExportXslt_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "XSLT files|*.xslt|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.Write(rtbXslt.Text);
                sw.Close();
            }
        }

        private void btnExportSampleFile_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV files|*.csv|Text files|*.txt|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.Write(rtbSampleOutput.Text);
                sw.Close();
            }
        }

        private void ViewModeChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            groupBoxTop.Visible = !radViewMode.Checked;
            btnImport.Visible = !radViewMode.Checked;
            btnDefaultXslt.Visible = !radViewMode.Checked;
            splitContainer1.Panel2Collapsed = radViewMode.Checked;

            if (!radViewMode.Checked)
            {
                pnlMainTop.Height = pnlTopHeightOriginal;
            }
            else
            {
                pnlMainTop.Height = pnlTopHeightShrink;
            }
            this.Cursor = Cursors.Default;
        }

        private void btnDefaultXslt_Click(object sender, EventArgs e)
        {
            rtbXslt.Text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
            rtbXslt.Text += "<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" exclude-result-prefixes=\"msxsl\">" + Environment.NewLine;
            rtbXslt.Text += "  <xsl:output method=\"text\" indent=\"yes\"/>" + Environment.NewLine;

            rtbXslt.Text += "  <xsl:template match=\"Interchange\">" + Environment.NewLine;
            rtbXslt.Text += "  <xsl:apply-templates select=\"XPATH1\"/>" + Environment.NewLine;
            rtbXslt.Text += "  <xsl:apply-templates select=\"XPATH2\"/>" + Environment.NewLine;


            rtbXslt.Text += Environment.NewLine;

            rtbXslt.Text += "    <xsl:template match=\"XPATH1\">" + Environment.NewLine;
            rtbXslt.Text += "      ..." + Environment.NewLine;
            rtbXslt.Text += "      ..." + Environment.NewLine;
            rtbXslt.Text += "    </xsl:template>" + Environment.NewLine;

            rtbXslt.Text += "    <xsl:template match=\"XPATH2\">" + Environment.NewLine;
            rtbXslt.Text += "      ..." + Environment.NewLine;
            rtbXslt.Text += "      ..." + Environment.NewLine;
            rtbXslt.Text += "    </xsl:template>" + Environment.NewLine;
            rtbXslt.Text += "</xsl:stylesheet>" + Environment.NewLine;
            SyntaxHighLighter.HighLight(rtbXslt, _blueKeyWords, _redKeyWords);
        }

        private void frmConfig3_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    btnHelp_Click(sender, e);
                    break;

                case Keys.F2:
                    if (radViewMode.Checked)
                    {
                        radViewMode.Checked = false;
                        radConfigureMode.Checked = true;
                    }
                    else
                    {
                        radViewMode.Checked = true;
                        radConfigureMode.Checked = false;
                    }                    
                    break;

                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void frmConfigEDI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_WindowsFormsEditorService != null)
                _WindowsFormsEditorService.CloseDropDown();
        }

    }

    public class EDIInformation
    {
        public string EDIFileName { get; set; }
        public string Xslt { get; set; }
        public string Edi { get; set; }

        public EDIInformation(string ediFileName, string xslt)
        {
            this.EDIFileName = ediFileName;
            this.Xslt = xslt;           
        }
    }
}


