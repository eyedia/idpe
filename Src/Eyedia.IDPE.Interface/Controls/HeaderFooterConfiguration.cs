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
using Eyedia.IDPE.Common;
using Data = Eyedia.IDPE.Interface.Data;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{
    public partial class HeaderFooterConfiguration : UserControl
    {
        public bool DoNotCloseAtSave { get; set; }

        public event EventHandler Saved;

        int _DataSourceId;
        public int DataSourceId
        {
            get { return _DataSourceId; }
            set
            {
                _DataSourceId = value;
                if(!DesignMode)
                    DataBind();
            }
        }

        //public string DataSourceName { get; set; }
        public DataFormatTypes DataFormatType { get; set; }

        BindingList<Data.Attribute> Attributes;
        List<IdpeKey> Keys;        

        public HeaderFooterConfiguration()
        {
            InitializeComponent();
            lblCaption.Dock = DockStyle.Fill;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool hasError = false;
            errorProvider1.Clear();
            List<string> headers = new List<string>();
            if (chkHasHeader1.Checked)
            {
                if (string.IsNullOrEmpty(txtHeader1.Text))
                {
                    errorProvider1.SetError(txtHeader1, "Header 1 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    headers.Add(txtHeader1.Text);
                }
            }
            if (chkHasHeader2.Checked)
            {
                if (string.IsNullOrEmpty(txtHeader2.Text))
                {
                    errorProvider1.SetError(txtHeader2, "Header 2 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    headers.Add(txtHeader2.Text);
                }
            }
            if (chkHasHeader3.Checked)
            {
                if (string.IsNullOrEmpty(txtHeader3.Text))
                {
                    errorProvider1.SetError(txtHeader3, "Header 3 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    headers.Add(txtHeader3.Text);
                }
            }
            if (chkHasHeader4.Checked)
            {
                if (string.IsNullOrEmpty(txtHeader4.Text))
                {
                    errorProvider1.SetError(txtHeader4, "Header 4 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    headers.Add(txtHeader4.Text);
                }
            }
            if (chkHasHeader5.Checked)
            {
                if (string.IsNullOrEmpty(txtHeader5.Text))
                {
                    errorProvider1.SetError(txtHeader5, "Header 5 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    headers.Add(txtHeader5.Text);
                }
            }
            if (chkHasHeader6.Checked)
            {
                if (string.IsNullOrEmpty(txtHeader6.Text))
                {
                    errorProvider1.SetError(txtHeader6, "Header 6 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    headers.Add(txtHeader6.Text);
                }
            }

            List<string> footers = new List<string>();
            if (chkHasFooter1.Checked)
            {
                if (string.IsNullOrEmpty(txtFooter1.Text))
                {
                    errorProvider1.SetError(txtFooter1, "Footer 1 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    footers.Add(txtFooter1.Text);
                }
            }
            if (chkHasFooter2.Checked)
            {
                if (string.IsNullOrEmpty(txtFooter2.Text))
                {
                    errorProvider1.SetError(txtFooter2, "Footer 2 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    footers.Add(txtFooter2.Text);
                }
            }
            if (chkHasFooter3.Checked)
            {
                if (string.IsNullOrEmpty(txtFooter3.Text))
                {
                    errorProvider1.SetError(txtFooter3, "Footer 3 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    footers.Add(txtFooter3.Text);
                }
            }
            if (chkHasFooter4.Checked)
            {
                if (string.IsNullOrEmpty(txtFooter4.Text))
                {
                    errorProvider1.SetError(txtFooter4, "Footer 4 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    footers.Add(txtFooter4.Text);
                }
            }
            if (chkHasFooter5.Checked)
            {
                if (string.IsNullOrEmpty(txtFooter5.Text))
                {
                    errorProvider1.SetError(txtFooter5, "Footer 5 cannot be empty string!");
                    hasError = true;
                }
                else
                {
                    footers.Add(txtFooter5.Text);
                }
            }
            if (chkHasFooter6.Checked)
            {
                if (string.IsNullOrEmpty(txtFooter6.Text))
                {
                    errorProvider1.SetError(txtFooter6, "Footer 6 cannot be empty string!");
                    hasError = true;
                }
                else
                {                    
                    footers.Add(txtFooter6.Text);
                }
            }


            if (hasError == false)
            {
                new Manager().SaveConfig(DataSourceId, headers, footers,
                    DataFormatType == DataFormatTypes.FixedLength ? fixedLengthSchemaGenerator1.Schema : null,
                    null);

                OnSaved(e);
            }

            if (!DoNotCloseAtSave)
            {
                if (ParentForm != null)
                    ParentForm.Close();
            }
        }

        protected virtual void OnSaved(EventArgs e)
        {
            EventHandler handler = this.Saved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        private void chkHasHeader1_CheckedChanged(object sender, EventArgs e)
        {
            txtHeader1.Enabled = chkHasHeader1.Checked;
            chkHasHeader2.Enabled = chkHasHeader1.Checked;
        }

        private void chkHasHeader2_CheckedChanged(object sender, EventArgs e)
        {
            txtHeader2.Enabled = chkHasHeader2.Checked;
            chkHasHeader3.Enabled = chkHasHeader2.Checked;
        }

        private void chkHasHeader3_CheckedChanged(object sender, EventArgs e)
        {
            txtHeader3.Enabled = chkHasHeader3.Checked;
            chkHasHeader4.Enabled = chkHasHeader3.Checked;
        }

        private void chkHasHeader4_CheckedChanged(object sender, EventArgs e)
        {
            txtHeader4.Enabled = chkHasHeader4.Checked;
            chkHasHeader5.Enabled = chkHasHeader4.Checked;
        }

        private void chkHasHeader5_CheckedChanged(object sender, EventArgs e)
        {
            txtHeader5.Enabled = chkHasHeader5.Checked;
            chkHasHeader6.Enabled = chkHasHeader5.Checked;
        }

        private void chkHasHeader6_CheckedChanged(object sender, EventArgs e)
        {
            txtHeader6.Enabled = chkHasHeader6.Checked;
        }

        private void chkHasFooter1_CheckedChanged(object sender, EventArgs e)
        {
            txtFooter1.Enabled = chkHasFooter1.Checked;
            chkHasFooter2.Enabled = chkHasFooter1.Checked;
        }

        private void chkHasFooter2_CheckedChanged(object sender, EventArgs e)
        {
            txtFooter2.Enabled = chkHasFooter2.Checked;
            chkHasFooter3.Enabled = chkHasFooter2.Checked;
        }

        private void chkHasFooter3_CheckedChanged(object sender, EventArgs e)
        {
            txtFooter3.Enabled = chkHasFooter3.Checked;
            chkHasFooter4.Enabled = chkHasFooter3.Checked;
        }

        private void chkHasFooter4_CheckedChanged(object sender, EventArgs e)
        {
            txtFooter4.Enabled = chkHasFooter4.Checked;
            chkHasFooter5.Enabled = chkHasFooter4.Checked;
        }

        private void chkHasFooter5_CheckedChanged(object sender, EventArgs e)
        {
            txtFooter5.Enabled = chkHasFooter5.Checked;
            chkHasFooter6.Enabled = chkHasFooter5.Checked;
        }

        private void chkHasFooter6_CheckedChanged(object sender, EventArgs e)
        {
            txtFooter6.Enabled = chkHasFooter6.Checked;
        }


        private void CheckVariableName(object sender, EventArgs e)
        {
            btnSave.Enabled = !CheckDuplicateOnUI(sender as TextBox);
        }



        public void DataBind()
        {
            if ((Keys == null)
                && (_DataSourceId != 0))
                Keys = new DataManager.Manager().GetKeys(_DataSourceId);

           
            switch (DataFormatType)
            {
                case DataFormatTypes.Delimited:
                    this.Text = "Delimited File Configuration";
                    grpHeader.Visible = true;
                    fixedLengthSchemaGenerator1.Visible = false;
                    lblCaption.Visible = true;
                    grpFooter.Visible = true;
                    break;

                case DataFormatTypes.Sql:
                    this.Text = "SQL Query";
                    fixedLengthSchemaGenerator1.Visible = false;
                    lblCaption.Visible = false;
                    grpHeader.Visible = false;
                    grpFooter.Visible = false;
                    break;

                case DataFormatTypes.FixedLength:
                    this.Text = "Fixed Length File Configuration";
                    fixedLengthSchemaGenerator1.Visible = true;
                    lblCaption.Visible = false;
                    grpHeader.Visible = true;
                    grpFooter.Visible = true;

                    Attributes = new Data.UtilDataManager().GetAttributes(DataSourceId);
                    string[] attributeNames = Attributes.Where(aa => aa.IsAcceptable == true).Select(a => a.Name).ToArray();
                    fixedLengthSchemaGenerator1.Attributes = attributeNames;

                    IdpeKey key =  Keys.GetKey(SreKeyTypes.FixedLengthSchema.ToString());
                    fixedLengthSchemaGenerator1.Schema = key != null ? key.Value : string.Empty;                    
                    break;

                default:
                    this.Text = "";
                    btnSave.Enabled = false;
                    grpHeader.Visible = false;
                    grpFooter.Visible = false;
                    lblCaption.Visible = false;
                    break;
            }

            if (Keys != null)
            {
                BindHeaders();
                BindFooters();
            }
        }

        private void BindHeaders()
        {
            IdpeKey key = Keys.GetKey(SreKeyTypes.HeaderLine1Attribute.ToString());            
            if (key != null)
            {
                txtHeader1.Text = key.Value;
                chkHasHeader1.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.HeaderLine2Attribute.ToString());
            if (key != null)
            {
                txtHeader2.Text = key.Value;
                chkHasHeader2.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.HeaderLine3Attribute.ToString());
            if (key != null)
            {
                txtHeader3.Text = key.Value;
                chkHasHeader3.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.HeaderLine4Attribute.ToString());
            if (key != null)
            {
                txtHeader4.Text = key.Value;
                chkHasHeader4.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.HeaderLine5Attribute.ToString());
            if (key != null)
            {
                txtHeader5.Text = key.Value;
                chkHasHeader5.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.HeaderLine6Attribute.ToString());
            if (key != null)
            {
                txtHeader6.Text = key.Value;
                chkHasHeader6.Checked = true;
            }

         
        }

        private void BindFooters()
        {
            IdpeKey key = Keys.GetKey(SreKeyTypes.FooterLine1Attribute.ToString());
            if (key != null)
            {
                txtFooter1.Text = key.Value;
                chkHasFooter1.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.FooterLine2Attribute.ToString());
            if (key != null)
            {
                txtFooter2.Text = key.Value;
                chkHasFooter2.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.FooterLine3Attribute.ToString());
            if (key != null)
            {
                txtFooter3.Text = key.Value;
                chkHasFooter3.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.FooterLine4Attribute.ToString());
            if (key != null)
            {
                txtFooter4.Text = key.Value;
                chkHasFooter4.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.FooterLine5Attribute.ToString());
            if (key != null)
            {
                txtFooter5.Text = key.Value;
                chkHasFooter5.Checked = true;
            }

            key = Keys.GetKey(SreKeyTypes.FooterLine6Attribute.ToString());
            if (key != null)
            {
                txtFooter6.Text = key.Value;
                chkHasFooter6.Checked = true;
            }
        }

        #region Private Methods
        private bool CheckDuplicateOnUI(TextBox txtBox)
        {
            if ((txtBox != txtHeader1)
                && (txtHeader1.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtHeader2)
                && (txtHeader2.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtHeader3)
                && (txtHeader3.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtHeader4)
                && (txtHeader4.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtHeader5)
                && (txtHeader5.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtHeader6)
                && (txtHeader6.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;

            if ((txtBox != txtFooter1)
                && (txtFooter1.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtFooter2)
                && (txtFooter2.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtFooter3)
                && (txtFooter3.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtFooter4)
                && (txtFooter4.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtFooter5)
               && (txtFooter5.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;
            if ((txtBox != txtFooter6)
                && (txtFooter6.Text.Equals(txtBox.Text, StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }

        #endregion Private Methods

    }
}


