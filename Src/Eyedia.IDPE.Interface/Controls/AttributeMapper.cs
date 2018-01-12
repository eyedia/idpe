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
using Eyedia.IDPE.DataManager;
using Eyedia.Core;
using System.Diagnostics;

namespace Eyedia.IDPE.Interface
{
    public partial class AttributeMapper : UserControl
    {
        const int __LevenshteinDistanceThreshold = 5;
        const string __fromAttribute = "<srs:AssignAttribute Data=\"[Data]\" FromValue=\"{x:Null}\" DisplayName=\"{0}\" FromAttribute=\"[Data.CurrentRow.Columns(&quot;{0}&quot;)]\" sap:VirtualizedContainerService.HintSize=\"242,22\" ToAttribute=\"[Data.CurrentRow.ColumnsSystem(&quot;{1}&quot;)]\" />";
        const string __fromValue = "<srs:AssignAttribute Data=\"[Data]\" FromValue=\"{0}\" DisplayName=\"{1}\" FromAttribute=\"{x:Null}\" sap:VirtualizedContainerService.HintSize=\"242,22\" ToAttribute=\"[Data.CurrentRow.ColumnsSystem(&quot;{2}&quot;)]\" />";
        const string __fromVB = "<Assign sap:VirtualizedContainerService.HintSize=\"242,57\">"
              + "<Assign.To>"
                + "<OutArgument x:TypeArguments=\"x:String\">[Data.CurrentRow.ColumnsSystem(\"{0}\").Value]</OutArgument>"
              + "</Assign.To>"
              + "<Assign.Value>"
                + "<InArgument x:TypeArguments=\"x:String\">[{1}]</InArgument>"
              + "</Assign.Value>"
            + "</Assign>";

        #region Properties

        List<SreAttribute> _Attributes;
        public List<SreAttribute> Attributes 
        { 
            get {return _Attributes;}
            set
            {
                SreAttribute firstItem = new SreAttribute();
                firstItem.Name = "";
                _Attributes.Add(firstItem);
                _Attributes.AddRange(value);
                cmbAttributes.DataSource = _Attributes;
                cmbAttributes.DisplayMember = "Name";     
            }
        }

        List<SreAttribute> _SystemAttributes;
        public List<SreAttribute> SystemAttributes
        {
            get { return _SystemAttributes; }
            set
            {
                SreAttribute firstItem = new SreAttribute();
                firstItem.Name = "";
                _SystemAttributes.Add(firstItem);
                _SystemAttributes.AddRange(value);
                cmbAttributesSys.DataSource = _SystemAttributes;
                cmbAttributesSys.DisplayMember = "Name";
            }
        }

        int _SystemAttributeSelectedIndex;
        public int SystemAttributeSelectedIndex
        {
            get { return _SystemAttributeSelectedIndex; }
            set
            {
                _SystemAttributeSelectedIndex = value;
                if (_SystemAttributeSelectedIndex < cmbAttributesSys.Items.Count)
                {
                    cmbAttributesSys.SelectedIndex = _SystemAttributeSelectedIndex;
                    SetAttributeNameUsingLevenshteinDistance();
                }
            }
        }
       
        public string XamlString
        {
            get
            {
                string myXamlString = string.Empty;

                if (radAttribute.Checked)
                    myXamlString = __fromAttribute.Replace("{0}", cmbAttributesSys.Text).Replace("{1}", cmbAttributes.Text);
                else if(radString.Checked)
                    myXamlString = __fromValue.Replace("{0}", txtVBExpression.Text).Replace("{1}", cmbAttributesSys.Text).Replace("{2}", cmbAttributes.Text);
                else
                    myXamlString = __fromVB.Replace("{0}", cmbAttributesSys.Text).Replace("{1}", txtVBExpression.Text);
                
                myXamlString = myXamlString.Replace("\"\"\"\"", "\"\"");
                return myXamlString;
            }
        }

        public Color BackColors
        {
            get { return cmbAttributes.BackColor; }
            set
            {
                cmbAttributes.BackColor = value;
                cmbAttributesSys.BackColor = value;             
                splitContainer1.BackColor = value;
                txtVBExpression.BackColor = value;
            }
        }
        #endregion Properties

        public AttributeMapper()
        {
            InitializeComponent();
            _Attributes = new List<SreAttribute>();
            _SystemAttributes = new List<SreAttribute>();            
        }

        private void AssignTypeChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (radAttribute.Checked)
            {
                cmbAttributes.Visible = true;
                txtVBExpression.Visible = false;
                cmbAttributes.Focus();
            }
            else
            {
                cmbAttributes.Visible = false;

                if (radString.Checked)
                    toolTip1.SetToolTip(txtVBExpression, "The constant string");
                else
                    toolTip1.SetToolTip(txtVBExpression, "VB Expression");
                txtVBExpression.Visible = true;
                txtVBExpression.Focus();
            }
        }
       
        public bool ValidateData()
        {
            bool isValid = true;
            if (cmbAttributesSys.SelectedIndex < 1)
            {
                errorProvider1.SetIconAlignment(cmbAttributesSys, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(cmbAttributesSys, "Please select a system attribute!");
                isValid = false;
            }

            if (radAttribute.Checked)
            {
                if (cmbAttributes.SelectedIndex < 1)
                {
                    errorProvider1.SetError(cmbAttributes, "Please select a attribute!");
                    isValid = false;
                }
            }
            else
            {
                if (radVBExpression.Checked)
                {
                    CSharpExpression cSharpExpression = new CSharpExpression();
                    cSharpExpression.CodeType = Eyedia.IDPE.Services.CSharpCodeInformation.CodeTypes.ExecuteAndReturnString;
                    cSharpExpression.Code = txtVBExpression.Text;                    
                    List<string>  errors = cSharpExpression.Compile();
                    if (errors.Count != 0)
                    {
                        errorProvider1.SetError(txtVBExpression, errors.ToLine());
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private void ClearError(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private void SetAttributeNameUsingLevenshteinDistance()
        {
            string source = ((SreAttribute)cmbAttributesSys.SelectedItem).Name;
            List<string> targets = new List<string>();
            foreach (SreAttribute item in cmbAttributes.Items) { targets.Add(item.Name); }
            string matchResult = source.TryMatchUsingLevenshteinDistance(targets, __LevenshteinDistanceThreshold);
            if (!string.IsNullOrEmpty(matchResult))
                cmbAttributes.Text = matchResult;
            else
                radString.Checked = true;
        }
    }
    
   
}


