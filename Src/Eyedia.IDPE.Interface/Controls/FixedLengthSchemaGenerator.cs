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

namespace Eyedia.IDPE.Interface
{
    public partial class FixedLengthSchemaGenerator : UserControl
    {
        public FixedLengthSchemaGenerator()
        {
            InitializeComponent();
            initValues = new List<int>();
            //GenerateControls();
        }

        void GenerateControls()
        {
            if (Attributes == null)
                return;

            Array.Reverse(Attributes);
            if (initValues.Count < Attributes.Length)
            {
                for (int i = initValues.Count; i < Attributes.Length; i++)
                {
                    initValues.Add(0);
                }
            }
            int counter = 1;
            foreach (string attribute in Attributes)
            {
                Label mylblAttributeName = new Label();
                mylblAttributeName.AutoSize = true;
                mylblAttributeName.Dock = DockStyle.Left;
                mylblAttributeName.Location = new Point(0, 0);
                mylblAttributeName.Name = "lblAttributeName" + counter;
                mylblAttributeName.Size = new Size(90, 13);
                mylblAttributeName.TabIndex = counter;
                mylblAttributeName.Text = attribute;

                NumericUpDown myColWidth = new NumericUpDown();
                myColWidth.Dock = DockStyle.Right;
                myColWidth.Location = new Point(0, 0);
                myColWidth.Name = "nmUd" + counter;
                myColWidth.Size = new Size(50, 20);
                myColWidth.TabIndex = counter;
                myColWidth.ValueChanged += new EventHandler(WidthChanged);
                if (initValues.Count > 0)
                    myColWidth.Value = initValues[counter - 1];

                GroupBox gp = new GroupBox();
                gp.Text = string.Empty;
                gp.Location = new Point(0, 0);
                gp.Name = "gp" + counter;
                gp.Size = new Size(300, 40);
                gp.TabIndex = counter;
                gp.TabStop = false;
                gp.Dock = DockStyle.Top;
                gp.Padding = new Padding(3, 0, 3, 0);
                gp.Controls.Add(myColWidth);
                gp.Controls.Add(mylblAttributeName);

                this.splitContainer1.Panel2.Controls.Add(gp);
                counter++;
            }
        }

        void WidthChanged(object sender, EventArgs e)
        {
            txtSchema.Text = GetSchema();
        }

        string GetAllValues()
        {
            List<string> all = new List<string>();

            foreach (Control gp in splitContainer1.Panel2.Controls)
            {
                if (gp is GroupBox)
                {
                    string oneName = string.Empty;
                    string oneValue = string.Empty;
                    foreach (Control ctl in gp.Controls)
                    {
                        if (ctl is Label)
                        {
                            oneName = ctl.Text;
                        }
                        else if (ctl is NumericUpDown)
                        {
                            oneValue = ((NumericUpDown)ctl).Value.ToString();
                        }
                    }
                    all.Add(string.Format("{0} Text Width {1}", oneName, oneValue));
                }
            }

            all.Reverse();

            string returnStr = string.Empty;

            int counter = 1;
            foreach (string info in all)
            {
                returnStr += string.Format("Col{0}={1}{2}", counter, info, Environment.NewLine);
                counter++;
            }
            return returnStr;
        }

        string GetSchema()
        {
            string schema = "[{0}]" + Environment.NewLine;
            schema += "ColNameHeader=false" + Environment.NewLine;
            schema += "Format=FixedLength" + Environment.NewLine;
            schema += "DateTimeFormat=yyyymmdd" + Environment.NewLine;
            schema += GetAllValues();

            return schema;
        }

        void Init()
        {
            if (string.IsNullOrEmpty(initialSchemaValue))
                return;

            initValues.Clear();
            string[] allLines = initialSchemaValue.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            for (int i = 4; i < allLines.Length; i++)
            {
                string[] val = allLines[i].Split(" ".ToCharArray());
                initValues.Add(int.Parse(val[val.Length - 1]));
            }

            initValues.Reverse();
        }

        string initialSchemaValue;
        List<int> initValues;

        public string[] Attributes;
        
        public string Schema 
        { 
            get 
            { 
                return GetSchema(); 
            }

            set
            {
                initialSchemaValue = value;
                Init();
                this.splitContainer1.Panel2.Controls.Clear();
                GenerateControls();
                txtSchema.Text = GetSchema();
            }
        }
        
    }
}



