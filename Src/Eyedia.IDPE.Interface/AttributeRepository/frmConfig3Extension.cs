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
using System.Reflection;
using System.IO;
using Symplus.RuleEngine.Services;
using Symplus.Core;

namespace Symplus.RuleEngine.Utilities
{
    public partial class frmConfig3Extension : Form
    {
        public frmConfig3Extension()
        {
            InitializeComponent();
            BindData();
        }

        public string EDIFileName
        {
            get
            {
                if (lvwSamples.SelectedItems.Count > 0)
                {
                    string fileName = Path.Combine(Path.GetTempPath(), lvwSamples.SelectedItems[0].Text + ".edi");
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.Write(rtbEDI.Text);
                    sw.Close();
                    return fileName;
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        public string Xslt
        {
            get
            {
                StreamReader sr = new StreamReader(XsltFile);
                string xslt = sr.ReadToEnd();
                sr.Close();
                return xslt;
            }
        }

        private string XmlFile;
        private string XsltFile;
        private void BindData()
        {
            string[] samples = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            samples = samples.Where(li => li.Contains("EmbeddedResources.EDIX12Samples")).ToArray();
            foreach (string sample in samples)
            {
                ListViewItem item = new ListViewItem(sample.Replace("Symplus.RuleEngine.Utilities.EmbeddedResources.EDIX12Samples.", ""));
                item.Tag = GetEmbeddedEDISample(sample);
                lvwSamples.Items.Add(item);

            }
        }

        private EDIInformation GetEmbeddedEDISample(string sample)
        {
            string fileName = Path.Combine("c:\\temp\\", "x.txt");

            string sampleContent = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sample))
            using (StreamReader reader = new StreamReader(stream))
            {
                sampleContent = reader.ReadToEnd();
            }

            string ediContent = sampleContent.Substring(0, sampleContent.IndexOf("==EDIEND=="));
            string xsltContent = sampleContent.Substring(sampleContent.IndexOf("==EDIEND==") + 11);

            EDIInformation ediInfo = new EDIInformation(string.Empty, xsltContent);
            ediInfo.Edi = ediContent;

            return ediInfo;
        }

        private void lvwSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwSamples.SelectedItems.Count > 0)
            {
                btnSelect.Enabled = true;
                btnExport1.Enabled = true;
                btnExport2.Enabled = true;
                btnExport3.Enabled = true;
                btnExport4.Enabled = true;
                btnExportAll.Enabled = true;


                this.Cursor = Cursors.WaitCursor;
                EDIInformation ediInfo = lvwSamples.SelectedItems[0].Tag as EDIInformation;
                rtbEDI.Text = ediInfo.Edi;

                XsltFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(lvwSamples.SelectedItems[0].Text) + ".xslt");
                StreamWriter sw = new StreamWriter(XsltFile);
                sw.Write(ediInfo.Xslt);
                sw.Close();


                EdiX12Parser ediParser = new EdiX12Parser(lvwSamples.SelectedItems[0].Text, rtbEDI.Text);
                XmlFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(lvwSamples.SelectedItems[0].Text) + ".xml");

                sw = new StreamWriter(XmlFile);
                sw.Write(ediParser.GenerateXml());
                sw.Close();

                webBrowser1.Url = new Uri(XmlFile);
                webBrowser2.Url = new Uri(XsltFile);
                rtbFlatFile.Text = ediParser.Parse(ediInfo.Xslt).ToString();

                this.Cursor = Cursors.Default;
            }
            else
            {
                btnSelect.Enabled = false;
                btnExport1.Enabled = false;
                btnExport2.Enabled = false;
                btnExport3.Enabled = false;
                btnExport4.Enabled = false;
                btnExportAll.Enabled = false;
            }
        }

        private void btnExport1_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "EDI files|*.edi|DAT files|*.dat|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.Write(rtbEDI.Text);
                sw.Close();
            }
        }

        private void btnExport2_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "XML files|*.xml|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                new FileUtility().FileCopy(XmlFile, saveFileDialog.FileName, false);
            }
        }

        private void btnExport3_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "XSLT files|*.xslt|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                new FileUtility().FileCopy(XsltFile, saveFileDialog.FileName, false);
            }
        }

        private void btnExport4_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV files|*.csv|Text files|*.txt|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.Write(rtbFlatFile.Text);
                sw.Close();
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {

        }

        private void btnExportAll_Click(object sender, EventArgs e)
        {
            if (lvwSamples.SelectedItems.Count > 0)
            {
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".edi"));
                    sw.Write(rtbEDI.Text);
                    sw.Close();

                    new FileUtility().FileCopy(XmlFile, Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".xml"), false);
                    new FileUtility().FileCopy(XsltFile, Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".xslt"), false);

                    sw = new StreamWriter(Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".csv"));
                    sw.Write(rtbFlatFile.Text);
                    sw.Close();
                }
            }
        }
    }
   
}


