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
using Eyedia.IDPE.Services;
using Eyedia.Core;
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface
{
    public partial class frmConfigXmlExtension : Form
    {
        public frmConfigXmlExtension(Icon icon = null)
        {
            InitializeComponent();
            ;
            if (icon != null)
                this.Icon = icon;

            BindData();
        }

        public DataSource DataSource { get; private set; }
        public XmlSampleInformation SampleInformation { get; private set; }
        public bool XsltSelected { get { return radXslt.Checked; } }
        public string CSharpCode { get { return rtbCSharpCode.Text; } }

        private void BindData()
        {
            string[] samples = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            samples = samples.Where(li => li.Contains("EmbeddedResources.XmlSamples")).ToArray();
            foreach (string sample in samples)
            {
                string sampleShortName = sample.Replace("Eyedia.IDPE.Interface.EmbeddedResources.XmlSamples.", "");
                ListViewItem item = new ListViewItem(sampleShortName);
                item.Tag = GetEmbeddedXmlSample(sample, sampleShortName);
                lvwSamples.Items.Add(item);

            }
        }

        private XmlSampleInformation GetEmbeddedXmlSample(string sample, string sampleShortName)
        {
             XmlSampleInformation xmlInfo = new XmlSampleInformation();

            xmlInfo.XmlFileName = Path.Combine(Information.TempDirectoryTempData, sampleShortName + ".xml");
            xmlInfo.XsltFileName = Path.Combine(Information.TempDirectoryTempData, sampleShortName + ".xslt");

            string sampleContent = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sample))
            using (StreamReader reader = new StreamReader(stream))
            {
                sampleContent = reader.ReadToEnd();
            }

             xmlInfo.Xml = sampleContent.Substring(0, sampleContent.IndexOf("==XMLEND=="));
             xmlInfo.CSharpCode = sampleContent.Substring(sampleContent.IndexOf("==XMLEND==") + 12, (sampleContent.IndexOf("==C#END==") - sampleContent.IndexOf("==XMLEND==")) - 12);
             xmlInfo.Xslt = sampleContent.Substring(sampleContent.IndexOf("==C#END==") + 11);            

             return xmlInfo;
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
                SampleInformation = (XmlSampleInformation)lvwSamples.SelectedItems[0].Tag;

                StreamWriter sw = new StreamWriter(SampleInformation.XmlFileName);
                sw.Write(SampleInformation.Xml);
                sw.Close();
                webBrowser2.Url = new Uri(SampleInformation.XmlFileName);

                sw = new StreamWriter(SampleInformation.XsltFileName);
                sw.Write(SampleInformation.Xslt);
                sw.Close();
                webBrowser1.Url = new Uri(SampleInformation.XsltFileName);

                rtbCSharpCode.Rtf = SampleInformation.CSharpCode;               

                XmlToDataTable xmlToDataTable = new XmlToDataTable();
                rtbFlatFile.Text = xmlToDataTable.Parse(SampleInformation.Xslt, SampleInformation.Xml).ToString();
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
            saveFileDialog.Filter = "XML files|*.xml|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                new FileUtility().FileCopy(SampleInformation.XmlFileName, saveFileDialog.FileName, false);
        }

        private void btnExport2_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "XSLT files|*.xslt|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                new FileUtility().FileCopy(SampleInformation.XsltFileName, saveFileDialog.FileName, false);

           
        }

        private void btnExport3_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "C# files|*.cs|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                rtbCSharpCode.SaveFile(saveFileDialog.FileName);
        }

        private void btnExport4_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV files|*.csv|Text files|*.txt|All files|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                rtbFlatFile.SaveFile(saveFileDialog.FileName);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportAll_Click(object sender, EventArgs e)
        {
            if (lvwSamples.SelectedItems.Count > 0)
            {
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new FileUtility().FileCopy(SampleInformation.XmlFileName, Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".xml"), false);
                    new FileUtility().FileCopy(SampleInformation.XsltFileName, Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".xslt"), false);
                    rtbCSharpCode.SaveFile(Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".cs"));                    
                    rtbFlatFile.SaveFile(Path.Combine(folderBrowserDialog.SelectedPath, lvwSamples.SelectedItems[0].Text + ".csv"));                    
                }
            }
        }        
    }

    public struct XmlSampleInformation
    {
        public string XmlFileName;
        public string Xml;
        public string Xslt;
        public string XsltFileName;
        public string CSharpCode;
    }
   
}


