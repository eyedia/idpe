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

        OutputTypes mOutputDataFormatType;        

        [Browsable(true)]
        [DefaultValue(OutputTypes.Delimited)]
        [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(OutputTypeEnumTypeConverter))]
        [DisplayName("Data Format")]
        [CategoryAttribute("Output.Format")]       
        public OutputTypes OutputDataFormatType
        {
            get { return mOutputDataFormatType; }
            set
            {
                mOutputDataFormatType = value;
                PropertyGridProperty prop_OutputDelimiter = GetReadOnlyProperty("OutputDelimiter");
                PropertyGridProperty prop_EncloseWithDoubleQuote = GetReadOnlyProperty("EncloseWithDoubleQuote");
                PropertyGridProperty prop_AllowPartial = GetReadOnlyProperty("AllowPartial");
                PropertyGridProperty prop_OutputHasHeader = GetReadOnlyProperty("OutputHasHeader");
                PropertyGridProperty prop_OutputFileNameFormat = GetReadOnlyProperty("OutputFileNameFormat");
                PropertyGridProperty prop_OutputFileExtension = GetReadOnlyProperty("OutputFileExtension");
                PropertyGridProperty prop_ConfigureOutputDataFormat = GetReadOnlyProperty("ConfigureOutputDataFormat");
                PropertyGridProperty prop_OutputCustomInterfaceName = GetReadOnlyProperty("OutputCustomInterfaceName");
                

                switch (mOutputDataFormatType)
                {
                    case OutputTypes.Xml:
                        prop_OutputDelimiter.FieldToChange.SetValue(prop_OutputDelimiter.BrowsableAttribute, false);
                        prop_EncloseWithDoubleQuote.FieldToChange.SetValue(prop_EncloseWithDoubleQuote.BrowsableAttribute, false);
                        prop_AllowPartial.FieldToChange.SetValue(prop_AllowPartial.BrowsableAttribute, false);
                        prop_OutputHasHeader.FieldToChange.SetValue(prop_OutputHasHeader.BrowsableAttribute, false);
                        prop_OutputFileNameFormat.FieldToChange.SetValue(prop_OutputFileNameFormat.BrowsableAttribute, true);
                        prop_OutputFileExtension.FieldToChange.SetValue(prop_OutputFileExtension.BrowsableAttribute, true);
                        prop_ConfigureOutputDataFormat.FieldToChange.SetValue(prop_ConfigureOutputDataFormat.BrowsableAttribute, false);
                        prop_OutputCustomInterfaceName.FieldToChange.SetValue(prop_OutputCustomInterfaceName.BrowsableAttribute, false);
                        break;

                    case OutputTypes.Delimited:
                        prop_OutputDelimiter.FieldToChange.SetValue(prop_OutputDelimiter.BrowsableAttribute, true);
                        prop_EncloseWithDoubleQuote.FieldToChange.SetValue(prop_EncloseWithDoubleQuote.BrowsableAttribute, true);
                        prop_AllowPartial.FieldToChange.SetValue(prop_AllowPartial.BrowsableAttribute, true);
                        prop_OutputHasHeader.FieldToChange.SetValue(prop_OutputHasHeader.BrowsableAttribute, true);
                        prop_OutputFileNameFormat.FieldToChange.SetValue(prop_OutputFileNameFormat.BrowsableAttribute, true);
                        prop_OutputFileExtension.FieldToChange.SetValue(prop_OutputFileExtension.BrowsableAttribute, true);
                        prop_ConfigureOutputDataFormat.FieldToChange.SetValue(prop_ConfigureOutputDataFormat.BrowsableAttribute, false);
                        prop_OutputCustomInterfaceName.FieldToChange.SetValue(prop_OutputCustomInterfaceName.BrowsableAttribute, false);
                        break;

                    case OutputTypes.FixedLength:
                        prop_OutputDelimiter.FieldToChange.SetValue(prop_OutputDelimiter.BrowsableAttribute, false);
                        prop_EncloseWithDoubleQuote.FieldToChange.SetValue(prop_EncloseWithDoubleQuote.BrowsableAttribute, false);
                        prop_AllowPartial.FieldToChange.SetValue(prop_AllowPartial.BrowsableAttribute, true);
                        prop_OutputHasHeader.FieldToChange.SetValue(prop_OutputHasHeader.BrowsableAttribute, false);
                        prop_OutputFileNameFormat.FieldToChange.SetValue(prop_OutputFileNameFormat.BrowsableAttribute, true);
                        prop_OutputFileExtension.FieldToChange.SetValue(prop_OutputFileExtension.BrowsableAttribute, true);
                        prop_ConfigureOutputDataFormat.FieldToChange.SetValue(prop_ConfigureOutputDataFormat.BrowsableAttribute, true);
                        prop_OutputCustomInterfaceName.FieldToChange.SetValue(prop_OutputCustomInterfaceName.BrowsableAttribute, false);
                        break;

                    case OutputTypes.CSharpCode:
                        prop_OutputDelimiter.FieldToChange.SetValue(prop_OutputDelimiter.BrowsableAttribute, true);
                        prop_EncloseWithDoubleQuote.FieldToChange.SetValue(prop_EncloseWithDoubleQuote.BrowsableAttribute, true);
                        prop_AllowPartial.FieldToChange.SetValue(prop_AllowPartial.BrowsableAttribute, true);
                        prop_OutputHasHeader.FieldToChange.SetValue(prop_OutputHasHeader.BrowsableAttribute, true);
                        prop_OutputFileNameFormat.FieldToChange.SetValue(prop_OutputFileNameFormat.BrowsableAttribute, true);
                        prop_OutputFileExtension.FieldToChange.SetValue(prop_OutputFileExtension.BrowsableAttribute, true);
                        prop_ConfigureOutputDataFormat.FieldToChange.SetValue(prop_ConfigureOutputDataFormat.BrowsableAttribute, true);
                        prop_OutputCustomInterfaceName.FieldToChange.SetValue(prop_OutputCustomInterfaceName.BrowsableAttribute, false);
                        break;

                    case OutputTypes.Database:
                        prop_OutputDelimiter.FieldToChange.SetValue(prop_OutputDelimiter.BrowsableAttribute, false);
                        prop_EncloseWithDoubleQuote.FieldToChange.SetValue(prop_EncloseWithDoubleQuote.BrowsableAttribute, false);
                        prop_AllowPartial.FieldToChange.SetValue(prop_AllowPartial.BrowsableAttribute, true);
                        prop_OutputHasHeader.FieldToChange.SetValue(prop_OutputHasHeader.BrowsableAttribute, false);
                        prop_OutputFileNameFormat.FieldToChange.SetValue(prop_OutputFileNameFormat.BrowsableAttribute, false);
                        prop_OutputFileExtension.FieldToChange.SetValue(prop_OutputFileExtension.BrowsableAttribute, false);
                        prop_ConfigureOutputDataFormat.FieldToChange.SetValue(prop_ConfigureOutputDataFormat.BrowsableAttribute, true);
                        prop_OutputCustomInterfaceName.FieldToChange.SetValue(prop_OutputCustomInterfaceName.BrowsableAttribute, false);
                        break;                  

                    case OutputTypes.Custom:
                        prop_OutputDelimiter.FieldToChange.SetValue(prop_OutputDelimiter.BrowsableAttribute, false);
                        prop_EncloseWithDoubleQuote.FieldToChange.SetValue(prop_EncloseWithDoubleQuote.BrowsableAttribute, true);
                        prop_AllowPartial.FieldToChange.SetValue(prop_AllowPartial.BrowsableAttribute, false);
                        prop_OutputHasHeader.FieldToChange.SetValue(prop_OutputHasHeader.BrowsableAttribute, false);
                        prop_OutputFileNameFormat.FieldToChange.SetValue(prop_OutputFileNameFormat.BrowsableAttribute, true);
                        prop_OutputFileExtension.FieldToChange.SetValue(prop_OutputFileExtension.BrowsableAttribute, true);
                        prop_ConfigureOutputDataFormat.FieldToChange.SetValue(prop_ConfigureOutputDataFormat.BrowsableAttribute, false);
                        prop_OutputCustomInterfaceName.FieldToChange.SetValue(prop_OutputCustomInterfaceName.BrowsableAttribute, true);
                        break;

                    default:
                        prop_OutputDelimiter.FieldToChange.SetValue(prop_OutputDelimiter.BrowsableAttribute, false);
                        prop_EncloseWithDoubleQuote.FieldToChange.SetValue(prop_EncloseWithDoubleQuote.BrowsableAttribute, false);
                        prop_AllowPartial.FieldToChange.SetValue(prop_AllowPartial.BrowsableAttribute, false);
                        prop_OutputHasHeader.FieldToChange.SetValue(prop_OutputHasHeader.BrowsableAttribute, true);
                        prop_OutputFileNameFormat.FieldToChange.SetValue(prop_OutputFileNameFormat.BrowsableAttribute, true);
                        prop_OutputFileExtension.FieldToChange.SetValue(prop_OutputFileExtension.BrowsableAttribute, true);
                        prop_ConfigureOutputDataFormat.FieldToChange.SetValue(prop_ConfigureOutputDataFormat.BrowsableAttribute, true);
                        prop_OutputCustomInterfaceName.FieldToChange.SetValue(prop_OutputCustomInterfaceName.BrowsableAttribute, false);
                        break;
                }
            }
        }

        [Browsable(false)]
        [CategoryAttribute("Output.Format")]
        [DefaultValue(",")]
        [DisplayName("Delimiter")]
        [TypeConverter(typeof(DelimiterConverter))]
        public string OutputDelimiter { get; set; }

        [Browsable(false)]
        [DefaultValue(false)]
        [DisplayName("Allow Partial")]
        [CategoryAttribute("Output.Format")]
        public bool AllowPartial { get; set; }

        [Browsable(false)]
        [DefaultValue(true)]
        [CategoryAttribute("Output.Format")]
        [DisplayName("Has Header")]
        public bool OutputHasHeader { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Output.Format")]
        [TypeConverter(typeof(OutputFileNameFormatConverter))]
        [DefaultValue("Same as Input")]
        [DisplayName("File Name Format")]
        public string OutputFileNameFormat { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Output.Format")]
        [TypeConverter(typeof(OutputFileExtensionConverter))]
        [DefaultValue(".csv")]
        [DisplayName("File Extension")]
        public string OutputFileExtension { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Output.Format")]
        [DefaultValue(true)]
        [DisplayName("Enclose Double Quote")]        
        [Description("If false then cell value will not be enclosed with double quote. NOTE - System will fail if delimiter found in cell")]
        public bool EncloseWithDoubleQuote { get; set; }


        [Browsable(false)]
        [CategoryAttribute("Output.Format")]
        [Editor(typeof(EditorOutputDataFormat), typeof(UITypeEditor))]
        public string OutputCustomInterfaceName { get; set; }

        [Browsable(false)]
        [Editor(typeof(EditorOutputDataFormat), typeof(UITypeEditor))]
        [CategoryAttribute("Output.Format")]
        [DisplayName("Configure")]
        public string ConfigureOutputDataFormat { get; set; }

    }

   
}


