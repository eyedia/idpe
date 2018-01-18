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
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace Eyedia.IDPE.Interface
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
           
            InitAuthorsLabel();            
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.textBoxDescription.Text = AssemblyDescription + Environment.NewLine + AssemblyCopyright;

            this.textBoxDescription.Text += Environment.NewLine;
            this.textBoxDescription.Text += Environment.NewLine + "Eyedia.Core.dll - " + typeof(Eyedia.Core.Cache).Assembly.GetName().Version;
            this.textBoxDescription.Text += Environment.NewLine + "Eyedia.IDPE.Common.dll - " + typeof(Eyedia.IDPE.Common.IdpeKeyTypes).Assembly.GetName().Version;
            this.textBoxDescription.Text += Environment.NewLine + "Eyedia.IDPE.DataManager.dll - " + typeof(Eyedia.IDPE.DataManager.Manager).Assembly.GetName().Version;
            this.textBoxDescription.Text += Environment.NewLine + "Eyedia.IDPE.Services.dll - " + typeof(Eyedia.IDPE.Services.Attribute).Assembly.GetName().Version;
            this.textBoxDescription.Text += Environment.NewLine + "IDE - " + typeof(AboutBox).Assembly.GetName().Version;

            this.textBoxDescription.Text += Environment.NewLine + "-----------------------------------------------------------";

            AddAssembly("Excel.dll", "http://exceldatareader.codeplex.com/license");
            AddAssembly("FileHelpers.dll", "http://filehelpers.sourceforge.net");
            AddAssembly("ICSharpCode.SharpZipLib.dll", "http://www.icsharpcode.net/opensource/sharpziplib");
            AddAssembly("OopFactory.X12.dll", "http://x12parser.codeplex.com/license");
            AddAssembly("SharpCompress.dll", "https://sharpcompress.codeplex.com/license");

            AddAssembly("ErikEJ.SqlCe40.dll", "https://sqlcebulkcopy.codeplex.com/license");
            AddAssembly("Salient.Data.dll", "https://sqlcebulkcopy.codeplex.com/license");
         
        }

        private void AddAssembly(string assemblyName, string licenseUrl)
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName)))
            {
                Assembly assembly = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName));
                this.textBoxDescription.Text += Environment.NewLine + Environment.NewLine + assemblyName + " - " + assembly.GetName().Version;
                this.textBoxDescription.Text += Environment.NewLine + licenseUrl;
            }          
        }

        private void InitAuthorsLabel()
        {
            lblAuthorNames.Top = 0;
            lblAuthorNames.Left = 0;
            lblAuthorNames.Parent = logoPictureBox;
            lblAuthorNames.BackColor = Color.Transparent;
            lblAuthorNames.Visible = false;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        string _WhatIsBeingTyped;
        private void AboutBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                okButton_Click(sender, e);
            else
                _WhatIsBeingTyped += (char)e.KeyValue;

            if (_WhatIsBeingTyped == null)
                return;

            if ((_WhatIsBeingTyped.Equals("idpe", StringComparison.OrdinalIgnoreCase))
                || (_WhatIsBeingTyped.Equals("asre", StringComparison.OrdinalIgnoreCase)))      //when opened the form from mainform using keyboard shortcut, an 'A' is already typed
            {
                _WhatIsBeingTyped = string.Empty;
                lblAuthorNames.Visible = true;
                lblAuthorNames.Top = lblAuthorNames.Height * -1;
                timer1.Enabled = true;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
           this.Close();           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lblAuthorNames.Top > logoPictureBox.Height)
                lblAuthorNames.Top = lblAuthorNames.Height * -1;

            lblAuthorNames.Top += 1;
        }

        private void textBoxDescription_DoubleClick(object sender, EventArgs e)
        {
            if (IsValidUrl(textBoxDescription.SelectedText))
                System.Diagnostics.Process.Start(textBoxDescription.SelectedText);
        }

        private bool IsValidUrl(string uri)
        {
            Uri uriResult;
            return Uri.TryCreate(uri, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp
                              || uriResult.Scheme == Uri.UriSchemeHttps);
        }
       
    }
}





