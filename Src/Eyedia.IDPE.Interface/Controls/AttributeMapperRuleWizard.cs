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
using System.Reflection;
using System.IO;

namespace Eyedia.IDPE.Interface
{
    public partial class AttributeMapperRuleWizard : UserControl
    {
        private string XamlBody
        {
            get
            {
                string strInstructions = string.Empty;

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Interface.EmbeddedResources.AttributeMapper.xml"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    strInstructions = reader.ReadToEnd();
                }
                return strInstructions;
            }
        }

        public AttributeMapperRuleWizard()
        {
            InitializeComponent();
            pnlMain.Dock = DockStyle.Fill;
            pnlPreparing.Dock = DockStyle.Fill;
            pnlHelp.Dock = DockStyle.Fill;
        }

        public void ShowHelp()
        {
            pnlHelp.Visible = !pnlHelp.Visible;
        }    
        public void BindData(int dataSourceId, int systemDataSourceId)
        {
            pnlMain.Visible = false;
            lblMessage.Text = "Preparing...";
            pnlPreparing.Visible = true;
            this.ParentForm.Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            List<IdpeAttribute> attributes = new Manager().GetAttributes(dataSourceId).ToList();
            List<IdpeAttribute> sysAttributes = new Manager().GetAttributes(systemDataSourceId).Where(a => a.Name != "IsValid").ToList();
            if (sysAttributes.Count == 0)
            {
                this.ParentForm.Cursor = Cursors.Default;
                Application.DoEvents();
                ShowMessage("No system(internal) attribute found!");
                return;
            }

            List<Control> controls = new List<Control>();
            int count = 1;
            bool alternateRow = false;
            foreach (IdpeAttribute attribute in sysAttributes)
            {
                AttributeMapper attributeMapper = new AttributeMapper();
                attributeMapper.Attributes = attributes;
                attributeMapper.SystemAttributes = sysAttributes;
                attributeMapper.Dock = System.Windows.Forms.DockStyle.Top;
                attributeMapper.Name = "attributeMapper" + count;
                attributeMapper.Padding = new System.Windows.Forms.Padding(0, 8, 0, 5);
                attributeMapper.SystemAttributeSelectedIndex = count;               
                //if (alternateRow)
                //    attributeMapper.BackColors = SystemColors.ControlDark;
                controls.Add(attributeMapper);
                alternateRow = !alternateRow;
                count++;
            }

            controls.Reverse();
            pnlMain.Controls.AddRange(controls.ToArray());

            pnlMain.Visible = true;
            pnlPreparing.Visible = false;
            this.ParentForm.Cursor = Cursors.Default;
            Application.DoEvents();
        }

        public bool Save(string dataSourceName)
        {
            string dynamicXamlBody = string.Empty;
            bool allGood = true;

            List<Control> mappers = new List<Control>(pnlMain.Controls.Cast<Control>());
            mappers.Reverse();

            foreach (Control ctl in mappers)
            {
                if (ctl is AttributeMapper)
                {
                    AttributeMapper attributeMapper = (AttributeMapper)ctl;
                    if (attributeMapper.ValidateData() == false)
                    {
                        attributeMapper.Focus();
                        allGood = false;
                    }
                    else
                    {
                        dynamicXamlBody += attributeMapper.XamlString + Environment.NewLine;
                    }

                }

            }

            if (allGood)
            {
                InputBox inputBox = new InputBox(dataSourceName + " - Map", "Enter Rule Name", "Save Rule", this.ParentForm.Icon);
                if (inputBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    IdpeRule rule = new IdpeRule();
                    rule.Name = inputBox.TheInput;
                    rule.Description = inputBox.TheInput + " was created by wizard";
                    rule.Xaml = string.Format(XamlBody, rule.Name, dynamicXamlBody);
                    new Manager().Save(rule);
                }
            }
            return allGood;
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            pnlPreparing.Visible = true;
        }
    }
}


