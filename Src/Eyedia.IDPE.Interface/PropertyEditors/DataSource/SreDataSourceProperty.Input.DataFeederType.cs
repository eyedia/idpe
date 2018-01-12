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
        DataFeederTypes mDataFeederType;
        [Browsable(true)]
        [DefaultValue(DataFeederTypes.PullLocalFileSystem)]        
        [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(DataFeederEnumTypeConverter))]
        [DisplayName("Data Feeder")]
        [CategoryAttribute("Input.Feed"), Description("How to feed the data")]
        public DataFeederTypes DataFeederType
        {
            get { return mDataFeederType; }
            set
            {
                mDataFeederType = value;
                PropertyGridProperty prop_WriteStandardOutput = GetReadOnlyProperty("WriteStandardOutput");
                PropertyGridProperty prop_Configure = GetReadOnlyProperty("ConfigureDataFeeder");

                PropertyGridProperty propDelimiter = GetReadOnlyProperty("Delimiter");
                PropertyGridProperty propHasHeader = GetReadOnlyProperty("HasHeader");
                PropertyGridProperty propRenameHeader = GetReadOnlyProperty("RenameHeader");
                PropertyGridProperty propSpreadSheetNumber = GetReadOnlyProperty("SpreadSheetNumber");
                PropertyGridProperty propConfigure = GetReadOnlyProperty("ConfigureDataFormat");
                PropertyGridProperty propDataFormatType = GetReadOnlyProperty("DataFormatType");

                if (mDataFeederType == DataFeederTypes.Push)
                {
                    prop_WriteStandardOutput.FieldToChange.SetValue(prop_WriteStandardOutput.BrowsableAttribute, true);
                    prop_Configure.FieldToChange.SetValue(prop_Configure.BrowsableAttribute, false);

                    propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, true);
                    propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, true);
                    propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, true);
                    propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, true);
                    propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, true);
                    propDataFormatType.FieldToChange.SetValue(propDataFormatType.BrowsableAttribute, true);

                }
                else if (mDataFeederType == DataFeederTypes.PullSql)
                {
                    prop_WriteStandardOutput.FieldToChange.SetValue(prop_WriteStandardOutput.BrowsableAttribute, false);
                    prop_Configure.FieldToChange.SetValue(prop_Configure.BrowsableAttribute, true);

                    propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, false);
                    propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, false);
                    propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, false);
                    propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, false);
                    propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, false);
                    propDataFormatType.FieldToChange.SetValue(propDataFormatType.BrowsableAttribute, false);

                }
                else
                {
                    prop_WriteStandardOutput.FieldToChange.SetValue(prop_WriteStandardOutput.BrowsableAttribute, false);
                    prop_Configure.FieldToChange.SetValue(prop_Configure.BrowsableAttribute, true);

                    propDelimiter.FieldToChange.SetValue(propDelimiter.BrowsableAttribute, true);
                    propHasHeader.FieldToChange.SetValue(propHasHeader.BrowsableAttribute, true);
                    propRenameHeader.FieldToChange.SetValue(propRenameHeader.BrowsableAttribute, true);
                    propSpreadSheetNumber.FieldToChange.SetValue(propSpreadSheetNumber.BrowsableAttribute, true);
                    propConfigure.FieldToChange.SetValue(propConfigure.BrowsableAttribute, true);
                    propDataFormatType.FieldToChange.SetValue(propDataFormatType.BrowsableAttribute, true);
                }

              
            }


        }

        [Browsable(false)]
        [DefaultValue(true)]
        [DisplayName("Write Standard Output")]
        [CategoryAttribute("Input.Feed")]
        public bool WriteStandardOutput { get; set; }

        [Browsable(false)]
        [Editor(typeof(EditorDataFeeder), typeof(UITypeEditor))]
        [CategoryAttribute("Input.Feed")]
        [DisplayName("Configure")]
        public string ConfigureDataFeeder { get; set; }
        
    }

}


