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
using Symplus.Core;
using Symplus.Core.Data;
using Symplus.RuleEngine.Common;
using Symplus.RuleEngine.DataManager;
using System.Text.RegularExpressions;
using System.Reflection;


namespace Symplus.RuleEngine.Utilities
{
    public partial class frmWizMapping : Form
    {
        int DataSourceId;
        int SystemDataSourceId;
        string DataSourceName;
        List<SreAttribute> Attributes;
        List<SreAttribute> SystemAttributes;

        public frmWizMapping()
        {
            InitializeComponent();
            Attributes = new List<SreAttribute>();
            SystemAttributes = new List<SreAttribute>();

            if (!DesignMode)
                Init();
        }

        void Init()
        {
            cbDataSources.DataSource = new Manager().GetDataSources(1);
            cbDataSources.DisplayMember = "Name";

           
        }

        void BindData()
        {
            BindAttributes();

        }

        private void BindAttributes()
        {
            Manager manager = new Manager();
            Attributes = manager.GetAttributesSystem(DataSourceId);
            cbr1c1.DataSource = Attributes;
            cbr1c1.DisplayMember = "Name";

            SystemAttributes = manager.GetAttributes(SystemDataSourceId);
            SystemAttributes = SystemAttributes.Where(sa => sa.Name != "IsValid").ToList();

            if (SystemAttributes.Count > 0)
                BindAttributeFirst(SystemAttributes[0]);

            for (int i = 1; i < SystemAttributes.Count; i++)
            {
                BindAttribute(SystemAttributes[i]);
            }
           
        }

        private void BindAttributeFirst(SreAttribute attribute)
        {
            txtr1c3.Text = attribute.Name;
        }

        private void BindAttribute(SreAttribute attribute)
        {
           
        }


        private void cbDataSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            SreDataSource selectedDataSource = cbDataSources.SelectedItem as SreDataSource;
            DataSourceId = selectedDataSource.Id;
            SystemDataSourceId = (int)selectedDataSource.SystemDataSourceId;

            BindData();
        }

        private string GetSubStrings(string input, string start, string end)
        {
            Regex r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
            MatchCollection matches = r.Matches(input);
            return matches[0].Groups[1].Value;          
        }

        bool radChecked;
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

           
            string rowNumber = GetSubStrings(rb.Name, "rbr", "c");
            string leftRadioName = "rbr" + rowNumber + "c1";
            string leftCBName = "cbr" + rowNumber + "c1";

            string rightRadioName = "rbr" + rowNumber + "c2";
            string rightTextBoxName = "txtr" + rowNumber + "c2";

            Control[] controls1 = this.Controls.Find(leftRadioName, true);
            RadioButton radLeft = controls1[0] as RadioButton;

            Control[] controls2 = this.Controls.Find(leftCBName, true);
            ComboBox cbLeft = controls2[0] as ComboBox;

            Control[] controls3 = this.Controls.Find(rightRadioName, true);
            RadioButton radRight = controls3[0] as RadioButton;

            Control[] controls4 = this.Controls.Find(rightTextBoxName, true);
            TextBox txtRight = controls4[0] as TextBox;
            
            if (rb.Name == leftRadioName)
            {
                if(rb.Checked)
                    radRight.Checked = false;
                else
                    radRight.Checked = true;
            }
            else
            {
                if (rb.Checked)
                    radLeft.Checked = false;
                else
                    radLeft.Checked = true;
            }

            cbLeft.Enabled = radLeft.Checked;
            txtRight.Enabled = radRight.Checked;

            if (cbLeft.Enabled)
                cbLeft.Focus();

            if (txtRight.Enabled)
                txtRight.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(lblTips.Visible)
            {
                lblTips.Visible = false;
                toolTip1.SetToolTip(pictureBox1, "Click to show");
            }
            else
            {
                lblTips.Visible = true;
                toolTip1.SetToolTip(pictureBox1, "Click to hide");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateControls();
        }

        private void GenerateControls()
        {
            for (int i = 0; i < SystemAttributes.Count; i++)
            {
                GenerateControlsColumn1(i + 1);
            }
        }

        private void GenerateControlsColumn1(int position)
        {
            ComboBox cbrxc1 = new ComboBox();
            cbrxc1.Dock = System.Windows.Forms.DockStyle.Fill;
            cbrxc1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbrxc1.FormattingEnabled = true;
            //cbrxc1.Location = new System.Drawing.Point(94, 4);
            cbrxc1.Name = "cbr" + position +"c1";
            //cbrxc1.Size = new System.Drawing.Size(271, 21);
            cbrxc1.DataSource = cbr1c1.DataSource;
            cbrxc1.DisplayMember = "Name";

            RadioButton rbrxc1 = new RadioButton();
            rbrxc1.AutoSize = true;
            rbrxc1.Checked = true;
            rbrxc1.Dock = System.Windows.Forms.DockStyle.Left;
            //rbrxc1.Location = new System.Drawing.Point(4, 4);
            rbrxc1.Name = "rbr" + position +"c1";
            //rbrxc1.Size = new System.Drawing.Size(90, 18);            
            rbrxc1.TabStop = true;
            rbrxc1.Text = "From Attribute";
            rbrxc1.UseVisualStyleBackColor = true;
            rbrxc1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);

            Panel pnlrxc1 = new Panel();
            pnlrxc1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            pnlrxc1.Controls.Add(cbrxc1);
            pnlrxc1.Controls.Add(rbrxc1);
            pnlrxc1.Dock = System.Windows.Forms.DockStyle.Top;
            //pnlrxc1.Location = new System.Drawing.Point(0, 23);
            pnlrxc1.Name = "pnlr" + position + "xc1";
            pnlrxc1.Padding = new System.Windows.Forms.Padding(4);
            pnlrxc1.Size = new System.Drawing.Size(373, 30);

            splitContainerMain.Panel1.Controls.Add(pnlrxc1);
        }
    }
   
}


