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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Eyedia.IDPE.Interface.RuleEditor
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog()
        {
            IsSet = false;
            InitializeComponent();            
        }

        public string RuleName
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private bool _Isset;

        public bool IsSet
        {
            get { return _Isset; }
            set { _Isset = value; }
        }
        
        //public string RulePriority
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(PriorityTextBox.Text))
        //            return "0";
        //        else
        //            return PriorityTextBox.Text;
        //    }
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value))
        //            PriorityTextBox.Text = "0";
        //        else
        //            if (System.Text.RegularExpressions.Regex.IsMatch("[^0-9]*", PriorityTextBox.Text))
        //            {

        //            }
        //            else
        //            {
        //                MessageBox.Show("Valid input must consist of digits 0-9", "Warning: Input Error");

        //            }


        //        PriorityTextBox.Text = value;
        //    }
        //}
        //public string RuleSetType
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(SetTypeTextBox.Text))
        //            return "0";
        //        else
        //            return SetTypeTextBox.Text;
        //    }
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value))
        //            SetTypeTextBox.Text = "0";
        //        else
        //            SetTypeTextBox.Text = value;
        //    }
        //}

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            IsSet = true;
            this.Close();
        }

        bool ValidateNumericdata(string inputData)
        {
            bool IsValid = true;
            if (System.Text.RegularExpressions.Regex.IsMatch("[^0-9]*", inputData))
            {
                IsValid = true;
            }
            else
            {
                //MessageBox.Show("Valid input must consist of digits 0-9", "Warning: Input Error");
                IsValid = false;
            }
            return IsValid;
        
        }

        //private void PriorityTextBox_KeyUp(object sender, KeyEventArgs e)
        //{
        //    ValidateNumericdata(PriorityTextBox.Text);
        //    PriorityTextBox.Text = "0";
        //}

        //private void SetTypeTextBox_KeyUp(object sender, KeyEventArgs e)
        //{
        //    ValidateNumericdata(SetTypeTextBox.Text);
        //    SetTypeTextBox.Text = "0";
        //}

    }
}



