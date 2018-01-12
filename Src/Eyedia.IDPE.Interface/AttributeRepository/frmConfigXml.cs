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
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Services;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Windows.Utilities;
using System.Windows.Forms.Design;

namespace Eyedia.IDPE.Interface
{

    public partial class frmConfigXml : Form
    {
        public const string _blueKeyWords = "\\b(xsl:stylesheet|xsl:output|xsl:template|xsl:apply-templates|xsl:value-of|xsl:variable|xsl:text)\\b";
        public const string _redKeyWords = "\\b(xml|version|encoding|xmlns:xsl|xmlns:msxsl|exclude-result-prefixes|method|indent|match|select|name)\\b";

        public IWindowsFormsEditorService _WindowsFormsEditorService;
        public DataSource DataSource { get; private set; }

        public frmConfigXml(int dataSourceId, IWindowsFormsEditorService windowsFormsEditorService = null)
        {
            InitializeComponent();
            _WindowsFormsEditorService = windowsFormsEditorService;
            if (_WindowsFormsEditorService != null)
                TopLevel = false;

            ;
            Cache.Instance.Bag.Remove(dataSourceId + ".keys");
            DataSource = new Services.DataSource(dataSourceId, string.Empty);
            rtbSampleOutput.WordWrap = false;

            rtbXslt.Dock = DockStyle.Fill;
            cSharpExpression1.Dock = DockStyle.Fill;
            pnlXsltAndCSharpCode.Dock = DockStyle.Fill;            
            
            BindData();
            ViewModeChanged(null, null);
        
        }

        private void BindData()
        {
            string strfeedMechanism = DataSource.Keys.GetKeyValue(SreKeyTypes.XmlFeedMechanism);
            if (string.IsNullOrEmpty(strfeedMechanism))
                return;

            XmlFeedMechanism feedMechanism = (XmlFeedMechanism)Enum.Parse(typeof(XmlFeedMechanism), strfeedMechanism, true);
            if (feedMechanism == XmlFeedMechanism.Xslt)
            {
                radXslt.Checked = true;
                rtbXslt.Text = DataSource.Keys.GetKeyValue(SreKeyTypes.Xslt);
                SyntaxHighLighter.HighLight(rtbXslt, _blueKeyWords, _redKeyWords);
            }
            else if (feedMechanism == XmlFeedMechanism.CSharpCode)
            {
                radCSharpCode.Checked = true;
                cSharpExpression1.Code = DataSource.Keys.GetKeyValue(SreKeyTypes.CSharpCodeGenerateTable);
            }
            else if (feedMechanism == XmlFeedMechanism.Custom)
            {
                radCustomInterface.Checked = true;
                txtInterfaceName.Text = DataSource.Keys.GetKeyValue(SreKeyTypes.FileInterfaceName);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (radXslt.Checked)
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
            else
            {
                openFileDialog.Filter = "C# files|*.cs|All files|*.*";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        cSharpExpression1.Code = sr.ReadToEnd();
                        sr.Close();                        
                    }
                }
            }
        }

        private void rtbXslt_TextChanged(object sender, EventArgs e)
        {
            ValidateTransformReady(sender, e);
            btnExport.Enabled = rtbXslt.Text.Length > 5;

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

                if (radXslt.Checked)
                {
                    XmlToDataTable xmlToDataTable = new XmlToDataTable(DataSource);
                    rtbSampleOutput.Text = xmlToDataTable.Parse(rtbXslt.Text, fileContent).ToString();
                }
                else if (radCSharpCode.Checked)
                {
                    rtbSampleOutput.Text = "";
                    CSharpCodeToDataTable cSharpCodeToDataTable = new CSharpCodeToDataTable(DataSource);
                    DataTable table = cSharpCodeToDataTable.Parse(new StringBuilder(fileContent), cSharpExpression1.CSharpCodeInformation);
                    string tempFileName = Path.Combine(Information.TempDirectoryTempData, "temp.csv");
                    table.ToCsv(tempFileName);
                    StreamReader srTempFile = new StreamReader(tempFileName);
                    string oneLine = srTempFile.ReadLine(); //skip first line
                    while ((oneLine = srTempFile.ReadLine()) != null)
                    {
                        rtbSampleOutput.Text += oneLine + Environment.NewLine;
                    }
                    srTempFile.Close();
                }
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
            if (File.Exists(txtSampleFile.Text))
            {
                if (((radXslt.Checked) && (string.IsNullOrEmpty(rtbXslt.Text) == false))
                    || ((radCSharpCode.Checked) && (string.IsNullOrEmpty(cSharpExpression1.Code) == false)))
                    btnTransform.Enabled = true;
                else
                    btnTransform.Enabled = false;
            }
        }

        private void btnBrowseSampleFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "XML files|*.xml|All files|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSampleFile.Text = openFileDialog.FileName;                
                ValidateTransformReady(sender, e);
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

                XmlToDataTable xmlToDataTable = new XmlToDataTable(DataSource);
                e.Result = xmlToDataTable.Parse(par.Xslt, fileContent).ToString();
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
            Process.Start(txtSampleFile.Text);
        }
      
        private void rtbSampleOutput_TextChanged(object sender, EventArgs e)
        {
            btnExportSampleFile.Enabled = rtbSampleOutput.Text.Length > 5;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (radXslt.Checked)
            {
                saveFileDialog.Filter = "XSLT files|*.xslt|All files|*.*";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                    sw.Write(rtbXslt.Text);
                    sw.Close();
                }
            }
            else
            {
                saveFileDialog.Filter = "C# files|*.cs|All files|*.*";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                    sw.Write(cSharpExpression1.Code);
                    sw.Close();
                }
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
     
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (radXslt.Checked)
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
            else
            {
                cSharpExpression1.Code = "";
            }
        }

        private void frmConfigXml_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
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

        private void TransformMechanismChanged(object sender, EventArgs e)
        {
            if (radXslt.Checked)
            {
                pnlXsltAndCSharpCode.Visible = true;                
                rtbXslt.Visible = true;
                cSharpExpression1.Visible = false;
                pnlCustom.Visible = false;

                lblParseMechanism.Text = "XSLT";
                lblTips.Text = "Write the XSLT to transform XML to pipe(|) delimited data, if you have sample file, you can see transform result";
                btnTransform.Text = "&Transform";

                toolTip1.SetToolTip(btnImport, "Import XSLT file");
                toolTip1.SetToolTip(btnExport, "Saves the XSLT file into your local disk");
                toolTip1.SetToolTip(btnNew, "New XSLT");
            }
            else if (radCSharpCode.Checked)
            {
                pnlXsltAndCSharpCode.Visible = true;                
                rtbXslt.Visible = false;
                cSharpExpression1.Visible = true;
                pnlCustom.Visible = false;

                lblParseMechanism.Text = "C# Code";
                lblTips.Text = "String parameter 'fileContent' will contain the XML document, write code to return datatable";
                btnTransform.Text = "&Output";

                toolTip1.SetToolTip(btnImport, "Import C#(.cs) file");
                toolTip1.SetToolTip(btnExport, "Saves the C# code into your local disk");
                toolTip1.SetToolTip(btnNew, "New C# Code");
            }
            else if (radCustomInterface.Checked)
            {
                pnlXsltAndCSharpCode.Visible = false;               
                pnlCustom.Visible = true;
            }
        }

        private void btnInterface2_Click(object sender, EventArgs e)
        {
            try
            {
                TypeSelectorDialog activitySelector = new TypeSelectorDialog(typeof(InputFileGenerator));

                if ((activitySelector.ShowDialog() == DialogResult.OK) && (!String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null))
                {
                    txtInterfaceName.Text = string.Format("{0}, {1}", activitySelector.Activity.FullName, Path.GetFileNameWithoutExtension(activitySelector.AssemblyPath));                    
                    txtInterfaceName.Text = txtInterfaceName.Text;
                    toolStripStatusLabel1.Text = "";

                }

            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.ToString();
                toolTip1.SetToolTip(statusStrip1, ex.ToString());
            }
        }

        private void txtSampleFile1_TextChanged(object sender, EventArgs e)
        {
            btnShowXml1.Enabled = File.Exists(txtSampleFile.Text);
            btnTransform.Enabled = btnShowXml1.Enabled;
            ValidateTransformReady(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSamples_Click(object sender, EventArgs e)
        {
            frmConfigXmlExtension configXmlExt = new frmConfigXmlExtension(this.Icon);
            if (configXmlExt.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSampleFile.Text = configXmlExt.SampleInformation.XmlFileName;
                if (configXmlExt.XsltSelected)
                {
                    radXslt.Checked = true;
                    rtbXslt.Text = configXmlExt.SampleInformation.Xslt;
                    SyntaxHighLighter.HighLight(rtbXslt, _blueKeyWords, _redKeyWords);
                }
                else
                {
                    radCSharpCode.Checked = true;
                    cSharpExpression1.Code = configXmlExt.CSharpCode;
                }
            }
        }

        private void ViewModeChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            groupBoxTransformType.Visible = !radViewMode.Checked;
            btnSamples.Visible = !radViewMode.Checked;
            groupBoxXsltMainTop.Visible = !radViewMode.Checked;
            btnImport.Visible = !radViewMode.Checked;
            btnNew.Visible = !radViewMode.Checked;
            splitContainer1.Panel2Collapsed = radViewMode.Checked;

            this.Cursor = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Manager manager = new Manager();
            manager.UpdateDataFormatType(DataSource.Id, DataFormatTypes.Xml);

            SreKey key = new SreKey();            
            manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.IsFirstRowHeader.ToString(), false);

            key = new SreKey();
            key.Name = SreKeyTypes.XmlFeedMechanism.ToString();
            key.Type = (int)SreKeyTypes.XmlFeedMechanism;
            if (radXslt.Checked)
            {
                manager.UpdateDelimiter(DataSource.Id, "|");

                key.Value = XmlFeedMechanism.Xslt.ToString();                
                manager.Save(key, DataSource.Id);

                key = new SreKey();
                key.Name = SreKeyTypes.Xslt.ToString();
                key.Type = (int)SreKeyTypes.Xslt;
                key.Value = rtbXslt.Text;
                manager.Save(key, DataSource.Id);

                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.CSharpCodeGenerateTable.ToString(), true);
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.FileInterfaceName.ToString(), true);
            }
            else if (radCSharpCode.Checked)
            {
                key.Value = XmlFeedMechanism.CSharpCode.ToString();                
                manager.Save(key, DataSource.Id);

                key = new SreKey();
                key.Name = SreKeyTypes.CSharpCodeGenerateTable.ToString();
                key.Type = (int)SreKeyTypes.CSharpCodeGenerateTable;
                key.Value = cSharpExpression1.Code;
                manager.Save(key, DataSource.Id);

                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.Xslt.ToString(), true);
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.FileInterfaceName.ToString(), true);
            }
            else if (radCustomInterface.Checked)
            {
                key.Value = XmlFeedMechanism.Custom.ToString();                
                manager.Save(key, DataSource.Id);

                key.Name = SreKeyTypes.FileInterfaceName.ToString();
                key.Type = (int)SreKeyTypes.FileInterfaceName;
                key.Value = txtInterfaceName.Text;                
                manager.Save(key, DataSource.Id);

                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.Xslt.ToString(), true);
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.CSharpCodeGenerateTable.ToString(), true);
            }

           
        }

        private void frmConfigXml_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_WindowsFormsEditorService != null)
                _WindowsFormsEditorService.CloseDropDown();
        }

    }
    
}


