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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Eyedia.IDPE.Interface
{
    /// <summary>
    /// Range modification for direct edit override
    /// </summary>
    public class NumericUpDownTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            // Attempt to do them all
            return true;
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                string Value;
                if (!(value is string))
                {
                    Value = Convert.ChangeType(value, context.PropertyDescriptor.PropertyType).ToString();
                }
                else
                    Value = value as string;
                decimal decVal;
                if (!decimal.TryParse(Value, out decVal))
                    decVal = decimal.One;
                MinMaxAttribute attr = (MinMaxAttribute)context.PropertyDescriptor.Attributes[typeof(MinMaxAttribute)];
                if (attr != null)
                {
                    decVal = attr.PutInRange(decVal);
                }
                return Convert.ChangeType(decVal, context.PropertyDescriptor.PropertyType);
            }
            catch
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            try
            {
                return destinationType == typeof(string)
                   ? Convert.ChangeType(value, context.PropertyDescriptor.PropertyType).ToString()
                   : Convert.ChangeType(value, destinationType);
            }
            catch { }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    // ReSharper disable MemberCanBePrivate.Global
    /// <summary>
    /// Attribute to allow ranges to be added to the numeric updowner
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinMaxAttribute : Attribute
    {
        public decimal Min { get; private set; }
        public decimal Max { get; private set; }
        public decimal Increment { get; private set; }
        public int DecimalPlaces { get; private set; }

        /// <summary>
        /// Use to make a simple UInt16 max. Starts at 0, increment = 1
        /// </summary>
        /// <param name="max"></param>
        public MinMaxAttribute(UInt16 max)
            : this((decimal)UInt16.MinValue, max)
        {
        }

        /// <summary>
        /// Use to make a simple integer (or default conversion) based range.
        /// default inclrement is 1
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="increment"></param>
        public MinMaxAttribute(int min, int max, int increment = 1)
            : this((decimal)min, max, increment)
        {
        }

        /// <summary>
        /// Set the Min, Max, increment, and decimal places to be used.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="increment"></param>
        /// <param name="decimalPlaces"></param>
        public MinMaxAttribute(decimal min, decimal max, decimal increment = decimal.One, int decimalPlaces = 0)
        {
            Min = min;
            Max = max;
            Increment = increment;
            DecimalPlaces = decimalPlaces;
        }

        /// <summary>
        /// Validation function to check if the value is withtin the range (inclusive)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsInRange(object value)
        {
            decimal checkedValue = (decimal)Convert.ChangeType(value, typeof(decimal));
            return ((checkedValue <= Max)
               && (checkedValue >= Min)
               );
        }

        /// <summary>
        /// Takes the value and adjusts if it is out of bounds.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal PutInRange(object value)
        {
            decimal checkedValue = (decimal)Convert.ChangeType(value, typeof(decimal));
            if (checkedValue > Max)
                checkedValue = Max;
            else if (checkedValue < Min)
                checkedValue = Min;
            return checkedValue;
        }
    }
    // ReSharper restore MemberCanBePrivate.Global


    public class NumericUpDownTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null)
                return base.GetEditStyle(context);
            return context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                if (context == null || context.Instance == null || provider == null)
                    return value;

                //use IWindowsFormsEditorService object to display a control in the dropdown area  
                IWindowsFormsEditorService frmsvr = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (frmsvr == null)
                    return value;

                MinMaxAttribute attr = (MinMaxAttribute)context.PropertyDescriptor.Attributes[typeof(MinMaxAttribute)];
                if (attr != null)
                {
                    NumericUpDown nmr = new NumericUpDown
                    {
                        Size = new Size(60, 120),
                        Minimum = attr.Min,
                        Maximum = attr.Max,
                        Increment = attr.Increment,
                        DecimalPlaces = attr.DecimalPlaces,
                        Value = attr.PutInRange(value)
                    };
                    frmsvr.DropDownControl(nmr);
                    context.OnComponentChanged();
                    return Convert.ChangeType(nmr.Value, context.PropertyDescriptor.PropertyType);
                }
            }
            catch { }
            return value;
        }
    }
}


