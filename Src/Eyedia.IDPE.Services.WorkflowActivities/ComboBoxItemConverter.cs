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
using System.Windows.Data;
using System.Activities.Presentation.Model;
using System.Activities;
using Microsoft.VisualBasic.Activities;
using System.Activities.Expressions;
using System.Windows.Controls;

namespace Eyedia.IDPE.Services.WorkflowActivities
{
    public class ComboBoxItemConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ModelItem modelItem = value as ModelItem;
            if (value != null)
            {
                if ((modelItem != null)
                    && (modelItem.GetCurrentValue() is InArgument<string>))
                {
                    InArgument<string> inArgument = modelItem.GetCurrentValue() as InArgument<string>;

                    if (inArgument != null)
                    {
                        Activity<string> expression = inArgument.Expression;
                        VisualBasicValue<string> vbexpression = expression as VisualBasicValue<string>;
                        Literal<string> literal = expression as Literal<string>;

                        if (literal != null)
                        {
                            return "\"" + literal.Value + "\"";
                        }
                        else if (vbexpression != null)
                        {
                            return vbexpression.ExpressionText;
                        }
                    }
                }
                else
                {
                    return value;
                }
                   
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Convert combo box value to InArgument<string>
            string itemContent = null;

            if (value is string)
            {
                return value.ToString();              
            }
            else
            {
                itemContent = (string)((ComboBoxItem)value).Content;
              
            }

            VisualBasicValue<string> vbArgument = new VisualBasicValue<string>(itemContent);
            InArgument<string> inArgument = new InArgument<string>(vbArgument);
            return inArgument;

        }
    }
}


