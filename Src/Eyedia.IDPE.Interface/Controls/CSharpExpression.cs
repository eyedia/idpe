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
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Eyedia.IDPE.Common;
using System.IO;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class CSharpExpression : UserControl
    {
        public CSharpCodeInformation CSharpCodeInformation { get; private set; }

        public event EventHandler SourceCodeChanged;

        public CSharpExpression()
        {
            CSharpCodeInformation = new CSharpCodeInformation();
            InitializeComponent();
            
            lbReferences.Items.Add("System.dll");
            lbReferences.Items.Add("System.Data.dll");            
            lbReferences.Items.Add("System.Xml.dll");
            lbReferences.Items.Add("Eyedia.Core.dll");
            lbReferences.Items.Add("Eyedia.IDPE.Common.dll");
            lbReferences.Items.Add("Eyedia.IDPE.Services.dll");
            lbReferences.Items.Add("Eyedia.IDPE.DataManager.dll");

            
        }

        [DefaultValue(false)]
        public bool ShowHelperMethods
        {
            get
            {
                return (tabControl1.TabPages[1].Name == tabPage4.Name);
            }
            set
            {
                if (value == true)
                {
                    if (tabControl1.TabPages[1].Name != tabPage4.Name)
                        tabControl1.TabPages.Insert(1, tabPage4);
                }
                else
                {
                    if (tabControl1.TabPages[1].Name == tabPage4.Name)
                        tabControl1.TabPages.Remove(tabPage4);
                }
            }
        }

        [Browsable(false)]
        public string Code
        {
            get
            {
                return this.txtEditor != null ? this.txtEditor.Text : string.Empty;
            }
            set
            {              
                this.txtEditor.Text = value;
                if (!string.IsNullOrEmpty(this.txtEditor.Text))
                {
                     if (!this.DesignMode)
                         btnCompile_Click(null, null);
                    SyntaxHighLighter.HighLight(txtEditor);
                }
            }
        }

        [DefaultValue(Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileContent)]
        public Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes CodeType 
        {
            get { return CSharpCodeInformation.CodeType; }
            set { CSharpCodeInformation.CodeType = value; }
        }

        [Browsable(false)]
        public string RawString
        {
            get
            {
                return CSharpCodeInformation.RawString;
            }
            set
            {
                if (!this.DesignMode)
                {
                    CSharpCodeInformation.RawString = value;                    
                    Code = CSharpCodeInformation.Code;
                    HelperMethods = CSharpCodeInformation.HelperMethods;
                    AdditionalUsingNamespace = CSharpCodeInformation.AdditionalUsingNamespace;
                    AdditionalReferences = CSharpCodeInformation.AdditionalReferences;
                }
            }
        }

        [Browsable(false)]
        public string AdditionalReferences
        {
            get
            {
                string references = string.Empty;
                for (int i = 7; i < lbReferences.Items.Count; i++)
                {
                    references += lbReferences.Items[i] + ";";
                }

                return references;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] references = value.Split(";".ToCharArray());
                    foreach (string reference in references)
                    {
                        if (!string.IsNullOrEmpty(reference))
                            lbReferences.Items.Add(reference);
                    }
                    if (!this.DesignMode)
                        btnCompile_Click(null, null);
                }
            }
        }

        [Browsable(false)]
        public string HelperMethods
        {
            get
            {
                return this.txtHelperMethods != null ? this.txtHelperMethods.Text : string.Empty;
            }
            set
            {
                this.txtHelperMethods.Text = value;
                if (!string.IsNullOrEmpty(this.txtHelperMethods.Text))
                {
                    if (!this.DesignMode)
                        btnCompile_Click(null, null);
                    SyntaxHighLighter.HighLight(txtHelperMethods);
                }
            }
        }

        [Browsable(false)]
        public string AdditionalUsingNamespace
        {
            get
            {
                return this.txtUsing != null ? this.txtUsing.Text : string.Empty;
            }
            set
            {
                this.txtUsing.Text = value;
                if (!string.IsNullOrEmpty(this.txtUsing.Text))
                {
                    if (!this.DesignMode)
                        btnCompile_Click(null, null);
                    SyntaxHighLighter.HighLight(txtUsing);
                }
            }
        }

        private void CSharpExpression_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F6:
                    btnCompile_Click(sender, e);
                    break;
                case Keys.Escape:
                    this.ParentForm.Close();
                    break;
            }
        }
       
        private void ShowCompileResult(string message, bool error = false)
        {

            rtbCompileResult.Select(rtbCompileResult.TextLength, 0);
            rtbCompileResult.SelectionColor = error ? Color.Red : Color.Blue;
            rtbCompileResult.AppendText(message);

            if (error)
                btnOK.Enabled = false;
        }

        private void txtEditor_TextChanged(object sender, EventArgs e)
        {
            btnCompile.Enabled = true;
            SyntaxHighLighter.HighLight(txtEditor);
            if (SourceCodeChanged != null)
                SourceCodeChanged(this, e);

            CSharpCodeInformation.Code = txtEditor.Text;
        }

        private void txtHelperMethods_TextChanged(object sender, EventArgs e)
        {
            btnCompile.Enabled = true;
            SyntaxHighLighter.HighLight(txtHelperMethods);
            if (SourceCodeChanged != null)
                SourceCodeChanged(this, e);
            CSharpCodeInformation.HelperMethods = txtHelperMethods.Text;
        }

        private void txtUsing_TextChanged(object sender, EventArgs e)
        {
            btnCompile.Enabled = true;
            SyntaxHighLighter.HighLight(txtUsing);
            if (SourceCodeChanged != null)
                SourceCodeChanged(this, e);
            CSharpCodeInformation.AdditionalUsingNamespace = txtUsing.Text;
        }
       
        public List<string> Compile()
        {
            string language = "CSharp";
            if (CodeDomProvider.IsDefinedLanguage(language))
            {
                CSharpCodeProvider codeProvider = (CSharpCodeProvider)CodeDomProvider.CreateProvider(language);

                rtbCompileResult.Text = "";
                System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;

                foreach (string assemblyName in lbReferences.Items)
                {
                    parameters.ReferencedAssemblies.Add(assemblyName);
                }

                List<string> errors = new List<string>();
                CompilerResults results = null;

                try
                {
                    results = codeProvider.CompileAssemblyFromSource(parameters, CSharpCodeInformation.CompleteCode);
                }
                catch (Exception ex)
                {
                    errors.Add(ex.ToString());
                }

                if (results != null)
                {
                    foreach (CompilerError CompErr in results.Errors)
                    {
                        errors.Add("Line number " + CompErr.Line +
                                    ", Error Number: " + CompErr.ErrorNumber +
                                    ", '" + CompErr.ErrorText + ";");
                    }
                }
                return errors;
            }
            return new List<string>();
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            List<string> results = Compile();
            if (results.Count > 0)
            {

                foreach (string error in results)
                {
                    ShowCompileResult(error + Environment.NewLine + Environment.NewLine, true);
                }
            }
            else
            {
                //Successful Compile
                if (sender != null)
                    ShowCompileResult("Success!");

                if (Activated)
                    btnOK.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }
       
        private void btnAddReference_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lbReferences.Items.Add(Path.GetFileName(openFileDialog.FileName));
                CSharpCodeInformation.AdditionalReferences = GetAdditionalReferences();
            }
        }

        private string GetAdditionalReferences()
        {
            string additionalReferences = string.Empty;
            for (int i = 7; i < lbReferences.Items.Count; i++)
            {
                additionalReferences += lbReferences.Items[i] + ";";
            }
            if (additionalReferences.Length > 1)
                additionalReferences = additionalReferences.Substring(0, additionalReferences.Length - 1);

            return additionalReferences;
        }

        private void CSharpExpression_Load(object sender, EventArgs e)
        {
            if(this.ParentForm != null)
                this.ParentForm.Activated +=new EventHandler(ParentForm_Activated);
        }

        bool Activated;
        private void ParentForm_Activated(object sender, EventArgs e)
        {
            Activated = true;            
            
            txtEditor.Select(txtEditor.Text.Length, 0);
            txtEditor.Focus();

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbReferences.SelectedItems.Count > 0)
            {
                lbReferences.Items.Remove(lbReferences.SelectedItems[0]);
                CSharpCodeInformation.AdditionalReferences = GetAdditionalReferences();
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            removeToolStripMenuItem.Enabled = lbReferences.SelectedItems.Count > 0 ? true : false;
        }

        
    }
}


