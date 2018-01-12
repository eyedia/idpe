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
using Eyedia.IDPE.Common;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Eyedia.IDPE.Interface
{
    //[DefaultPropertyAttribute("SaveOnClose")]
    public partial class SreDataSourceProperty
    {

        DataFormatTypes mDataFormatType;        

        [Browsable(false)]
        [DefaultValue(DataFormatTypes.Delimited)]
        [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(DataFormatEnumTypeConverter))]
        [DisplayName("Data Format")]
        [CategoryAttribute("Input.Format")]       
        public DataFormatTypes DataFormatType
        {
            get { return mDataFormatType; }
            set
            {
                mDataFormatType = value;
                PropertyGridProperty propDelimiter = GetReadOnlyProperty("Delimiter");
                PropertyGridProperty propHasHeader = GetReadOnlyProperty("HasHeader");
                PropertyGridProperty propRenameHeader = GetReadOnlyProperty("RenameHeader");
                PropertyGridProperty propSpreadSheetNumber = GetReadOnlyProperty("SpreadSheetNumber");
                PropertyGridProperty propConfigure = GetReadOnlyProperty("ConfigureDataFormat");

                switch (mDataFormatType)
                {
                    case DataFormatTypes.Delimited:
                        propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, true);
                        propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, true);
                        propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, true);
                        propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, false);
                        propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, true);
                        break;

                    case DataFormatTypes.FixedLength:
                    case DataFormatTypes.Sql:
                        propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, false);
                        propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, false);
                        propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, false);
                        propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, false);
                        propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, true);
                        break;

                    case DataFormatTypes.SpreadSheet:
                        propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, false);
                        propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, true);
                        propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, true);
                        propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, true);
                        propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, false);
                        break;

                    case DataFormatTypes.Xml:
                        propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, false);
                        propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, false);
                        propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, false);
                        propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, false);
                        propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, true);
                        break;

                    case DataFormatTypes.CSharpCode:
                        propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, true);
                        propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, true);
                        propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, false);
                        propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, false);
                        propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, true);
                        break;

                    case DataFormatTypes.Zipped:
                    case DataFormatTypes.EDIX12:
                    case DataFormatTypes.MultiRecord:
                    case DataFormatTypes.Custom:
                    default:
                        propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, true);
                        propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, true);
                        propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, false);
                        propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, false);
                        propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, true);
                        break;
                }
            }
        }

        [Browsable(false)]
        [CategoryAttribute("Input.Format")]
        [DefaultValue(",")]
        [TypeConverter(typeof(DelimiterConverter))]
        public string Delimiter { get; set; }

        [Browsable(false)]
        [DefaultValue(true)]
        [CategoryAttribute("Input.Format")]
        [DisplayName("Has Header")]
        public bool HasHeader { get; set; }

        [Browsable(false)]
        [DefaultValue(false)]
        [DisplayName("Rename Header")]
        [CategoryAttribute("Input.Format")]
        public bool RenameHeader { get; set; }

        [Browsable(false)]
        [CategoryAttribute("Input.Format")]
        [TypeConverter(typeof(NumericUpDownTypeConverter))]
        [DisplayName("Spread Sheet Number")]
        [Editor(typeof(NumericUpDownTypeEditor), typeof(UITypeEditor))] 
        public int SpreadSheetNumber { get; set; }

        [Browsable(false)]
        [Editor(typeof(EditorDataFormat), typeof(UITypeEditor))]
        [CategoryAttribute("Input.Format")]
        [DisplayName("Configure")]
        public string ConfigureDataFormat { get; set; }

    }

   
}


