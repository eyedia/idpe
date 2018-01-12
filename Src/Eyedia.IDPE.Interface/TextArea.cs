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

namespace Eyedia.IDPE.Interface
{
    public partial class TextArea : Form
    {
        public TextArea(string title = null)
        {
            InitializeComponent();
            ;

            if (title != null)
                this.Text = title;
        }
        bool isSizeDoubled = false;
        int thisTop = 0;
        int thisLeft = 0;

        string _HintContent1;
        public string HintContent1
        {
            get
            {
                return _HintContent1;
            }

            set
            {
                _HintContent1 = value;
                if (!string.IsNullOrEmpty(value))
                    btnHelp.Visible = true;
            }
        }
        public string HintContent1WindowTitle { get; set; }

        private void TextArea_Load(object sender, EventArgs e)
        {
            thisTop = this.Top;
            thisLeft = this.Left;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextArea_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void txtContent_DoubleClick(object sender, EventArgs e)
        {
            if (!isSizeDoubled)
            {
                this.Height = this.Height * 2;
                this.Width = this.Width * 2;

                this.Top = thisTop - 100;
                this.Left = thisLeft  - 200;
                isSizeDoubled = true;
                this.toolStripStatusLabel1.Text = "Escape to return |OK to save | Double click on content to minimize";
            }
            else
            {
                this.Height = this.Height / 2;
                this.Width = this.Width / 2;
                this.Top = thisTop;
                this.Left = thisLeft;
                isSizeDoubled = false;
                this.toolStripStatusLabel1.Text = "Escape to return |OK to save | Double click on content to maximize";
            }
            
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            TextArea textAreaHintContent1 = new TextArea(HintContent1WindowTitle);
            textAreaHintContent1.Size = new System.Drawing.Size(691, 363);
            textAreaHintContent1.toolStripStatusLabel1.Text = "";
            textAreaHintContent1.txtContent.Text = HintContent1;
            textAreaHintContent1.ShowDialog();
            
        }

        
        
    }
}





