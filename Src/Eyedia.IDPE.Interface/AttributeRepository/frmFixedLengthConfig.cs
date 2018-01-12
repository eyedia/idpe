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
using Symplus.RuleEngine.Common;
using Data = Symplus.RuleEngine.Utilities.Data;
using Symplus.RuleEngine.DataManager;


namespace Symplus.RuleEngine.Utilities
{
    public partial class frmFixedLengthConfig : Form
    {
        int _ApplicationId;
        string _ApplicationName;
        DataFormatTypes _formatType;
        BindingList<Data.Attribute> _Attributes;

        public frmFixedLengthConfig(int applicationId, string applicationName, DataFormatTypes formatType)
        {
            _ApplicationId = applicationId;
            _ApplicationName = applicationName;
            _formatType = formatType;            
            InitializeComponent();

            switch (formatType)
            {
                case DataFormatTypes.Sql:
                    this.Text = "Paste SQL Query";
                    grpHeader.Visible = false;
                    grpFooter.Visible = false;
                    break;

                case DataFormatTypes.FixedLength:
                    this.Text = "Paste Fixed Length Schema";                    
                    grpHeader.Visible = true;
                    grpFooter.Visible = true;
                    break;

                default:
                    this.Text = "";                    
                    btnSave.Enabled = false;
                    grpHeader.Visible = false;
                    grpFooter.Visible = false;
                    break;
            }

            _Attributes = new Data.UtilDataManager().GetAttributes(_ApplicationId);
            cmbAttributeFooter.DataSource = _Attributes;
            cmbAttributeFooter.DisplayMember = "Name";

            Data.Attribute[] aa2 = new Data.Attribute[_Attributes.Count];
            _Attributes.CopyTo(aa2, 0);            
            cmbAttributeHeader.DataSource = aa2;
            cmbAttributeHeader.DisplayMember = "Name";

            this.fixedLengthSchemaGenerator1.Attributes = _Attributes.Where(aa => aa.IsAcceptable == true).Select(a => a.Name).ToArray();

        }

        public string Schema
        {
            get { return fixedLengthSchemaGenerator1.Schema; }
            set { fixedLengthSchemaGenerator1.Schema = value; }
        }

        public string FixedLegnthHeaderAttribute
        {
            get { return cmbAttributeHeader.Text; }
            set 
            { 
                cmbAttributeHeader.Text = value;
                if (string.IsNullOrEmpty(value))
                    chkHasHeader.Checked = false;
                else
                    chkHasHeader.Checked = true;
            }
        }

        public string FixedLegnthFooterAttribute
        {
            get { return cmbAttributeFooter.Text; }
            set 
            { 
                cmbAttributeFooter.Text = value;
                if (string.IsNullOrEmpty(value))
                    chkHasFooter.Checked = false;
                else
                    chkHasFooter.Checked = true;
            }
        }
       

        private void chkHasHeader_CheckedChanged(object sender, EventArgs e)
        {
            cmbAttributeHeader.Enabled = chkHasHeader.Checked;            
        }

        private void chkHasFooter_CheckedChanged(object sender, EventArgs e)
        {
            cmbAttributeFooter.Enabled = chkHasFooter.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            new Manager().SaveConfigFixedLength(_ApplicationId, Schema,
                chkHasHeader.Checked == false ? string.Empty : FixedLegnthHeaderAttribute, 
                chkHasFooter.Checked == false ? string.Empty : FixedLegnthFooterAttribute);
        }

       
    }
}





