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
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;

namespace Eyedia.IDPE.Interface
{
    public class DataFormatEnumTypeConverter : EnumConverter
    {
        private Type m_EnumType;
        public DataFormatEnumTypeConverter(Type type)
            : base(type)
        {
            m_EnumType = type;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            FieldInfo fi = m_EnumType.GetField(Enum.GetName(m_EnumType, value));
            DescriptionAttribute dna =
                (DescriptionAttribute)Attribute.GetCustomAttribute(
                fi, typeof(DescriptionAttribute));

            if (dna != null)
                return dna.Description;
            else
                return value.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
        {
            return srcType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            foreach (FieldInfo fi in m_EnumType.GetFields())
            {
                DescriptionAttribute dna =
                (DescriptionAttribute)Attribute.GetCustomAttribute(
                fi, typeof(DescriptionAttribute));

                if ((dna != null) && ((string)value == dna.Description))
                    return Enum.Parse(m_EnumType, fi.Name);
            }
            return Enum.Parse(m_EnumType, (string)value);
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            SreDataSourceProperty dsProperty = context.Instance as SreDataSourceProperty;
            if (dsProperty.DataFeederType == DataFeederTypes.Push)
                return new StandardValuesCollection(new DataFormatTypes[] { DataFormatTypes.Delimited, DataFormatTypes.FixedLength, DataFormatTypes.Xml, DataFormatTypes.Custom });
            else if (dsProperty.DataFeederType == DataFeederTypes.PullSql)
                return new StandardValuesCollection(new DataFormatTypes[] { DataFormatTypes.Delimited, DataFormatTypes.Custom });
            else if (dsProperty.DataFeederType == DataFeederTypes.PullFtp)
                return new StandardValuesCollection(new DataFormatTypes[] { DataFormatTypes.Delimited, DataFormatTypes.FixedLength, DataFormatTypes.Xml,  DataFormatTypes.SpreadSheet,
        DataFormatTypes.Zipped,DataFormatTypes.EDIX12,DataFormatTypes.MultiRecord,DataFormatTypes.CSharpCode,DataFormatTypes.Custom});            
            else
                return new StandardValuesCollection(new DataFormatTypes[] {DataFormatTypes.Delimited, DataFormatTypes.FixedLength, DataFormatTypes.Xml,  DataFormatTypes.SpreadSheet,
        DataFormatTypes.Sql,DataFormatTypes.Zipped,DataFormatTypes.EDIX12,DataFormatTypes.MultiRecord,DataFormatTypes.CSharpCode,DataFormatTypes.Custom});
        }
        
    }
}


