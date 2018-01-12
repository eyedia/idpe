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
using Eyedia.IDPE.Common;
using System.Windows.Forms.Design;

namespace Eyedia.IDPE.Interface
{
    public partial class ConfigCSharpInputGenerator : Form
    {
        public int DataSourceId { get; private set; }
        public DataFormatTypes DataformatType { get; private set; }
        public IWindowsFormsEditorService _WindowsFormsEditorService;

        public ConfigCSharpInputGenerator(int dataSourceId, DataFormatTypes dataformatType = DataFormatTypes.CSharpCode, IWindowsFormsEditorService windowsFormsEditorService = null)
        {
            InitializeComponent();
            ;

            _WindowsFormsEditorService = windowsFormsEditorService;
            if (_WindowsFormsEditorService != null)
                TopLevel = false;

            DataSourceId = dataSourceId;
            DataformatType = dataformatType;
            if(DataformatType == DataFormatTypes.CSharpCode)
            {
                pnlTop.Visible =  true;
                this.Text = "C# Code Configuration";
            }
            else
            {
                pnlTop.Visible = false;
                this.Text = "Multi Record Configuration";
                cbGenerateDataTableFrom.SelectedIndex = 1;
            }
            BindData();
        }

        private void BindData()
        {
           IdpeKey key = new Manager().GetKey(DataSourceId, SreKeyTypes.CSharpCodeGenerateTable.ToString());
           if (key != null)
           {
               cSharpExpression1.RawString = key.Value;
               if (cSharpExpression1.CodeType == Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileName)
                   cbGenerateDataTableFrom.SelectedIndex = 0;
               else if (cSharpExpression1.CodeType == Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileContent)
                   cbGenerateDataTableFrom.SelectedIndex = 1;
           }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetCodeType();
            cSharpExpression1.Code = cSharpExpression1.Code;
            cSharpExpression1.AdditionalUsingNamespace = cSharpExpression1.AdditionalUsingNamespace;
            cSharpExpression1.AdditionalReferences = cSharpExpression1.AdditionalReferences;

            IdpeKey key = new IdpeKey();
            key.Name = SreKeyTypes.CSharpCodeGenerateTable .ToString();
            key.Type = (int)SreKeyTypes.CSharpCodeGenerateTable;
            key.Value = cSharpExpression1.RawString;
            new Manager().Save(key, DataSourceId);
        }

        private void cbGenerateDataTableFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGenerateDataTableFrom.SelectedIndex == 0)
                cSharpExpression1.CodeType = Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileName;
            else if (cbGenerateDataTableFrom.SelectedIndex == 1)
                cSharpExpression1.CodeType = Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileContent;
        }

        private void SetCodeType()
        {
            if (cbGenerateDataTableFrom.SelectedIndex == 0)
                cSharpExpression1.CodeType = Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileName;
            else if (cbGenerateDataTableFrom.SelectedIndex == 1)
                cSharpExpression1.CodeType = Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.DataTableGeneratorFromFileContent;
        }

        private void ConfigCSharpInputGenerator_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_WindowsFormsEditorService != null)
                _WindowsFormsEditorService.CloseDropDown();
        }
    }
}


