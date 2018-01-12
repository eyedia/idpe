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
using Eyedia.IDPE.DataManager;
using Eyedia.Core;

namespace Eyedia.IDPE.Interface
{
    
    public partial class SreAttributeProperty:SrePropertyGrid
    {        

        [Browsable(false)]
        public List<SreKey> DataSourceKeys { get; private set; }

        [Browsable(true)]
        [CategoryAttribute("0-Global"), Description("System generated unique identifier of the attribute")]
        public int Id { get { return Attribute.AttributeId; } }

        [Browsable(true)]
        [CategoryAttribute("0-Global"), Description("Name of the attribute")]
        public string Name { get { return Attribute.Name; } }

        [Browsable(false)]
        [CategoryAttribute("0-Global"), Description("Name of the attribute")]
        [DisplayName("Name")]
        public string NameEditable { get { return Attribute.Name; } set { Attribute.Name = value; } }

        [Browsable(true)]
        [CategoryAttribute("0-Global"), Description("Type of the attribute")]
        [TypeConverter(typeof(AttributeTypeEnumTypeConverter))]
        public AttributeTypes Type
        {
            get
            {
                return (AttributeTypes)(Enum.Parse(typeof(AttributeTypes), Attribute.Type, true));
            }
           
        }


        [Browsable(false)]
        [CategoryAttribute("0-Global"), Description("Type of the attribute")]
        [DisplayName("Type")]
        [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [TypeConverter(typeof(AttributeTypeEnumTypeConverter))]
        public AttributeTypes TypeEditable { 
            get { return (AttributeTypes)(Enum.Parse(typeof(AttributeTypes), Attribute.Type)); }
            set
            {
                Attribute.Type = Enum.Parse(typeof(AttributeTypes), value.ToString()).ToString();

                PropertyGridProperty prop_LovName = GetReadOnlyProperty("LovName");
                if (TypeEditable == AttributeTypes.Codeset)
                {
                    prop_LovName.FieldToChange.SetValue(prop_LovName.BrowsableAttribute, true);
                }
                else
                {
                    prop_LovName.FieldToChange.SetValue(prop_LovName.BrowsableAttribute, false);
                }

            }
        }

        [Browsable(false)]
        //[CategoryAttribute("0-Global"), Description("Code set formula as [Connection String].[TableName].(\"CodeSet\"), for example: [defaultcs].[SymplusCodeSet].(\"Currency\") ")]
        [CategoryAttribute("0-Global"), Description("")]
        [DisplayName("Formula")]
        [Editor(typeof(MultiLineTextEditor), typeof(UITypeEditor))]       
        public string Formula
        {
            get
            {
                return Attribute.Formula;
            }

            set
            {
                Attribute.Formula = value;
            }
        }

        [Browsable(false)]
        [CategoryAttribute("0-Global"), Description("Name of the list of LOV(List of values)")]
        [DisplayName("LOV Name")]
        [Editor(typeof(EditorLovName), typeof(UITypeEditor))]        
        public string LovName
        {
            get; set;
        }

        [Browsable(false)]
        [DefaultValue(true)]
        [CategoryAttribute("Output.Format"), Description("If attribute value to be printed on the output")]
        [DisplayName("Is Printable")]
        public bool Printable { get; set; }


        AttributePrintValueTypes mPrintFormat;
        [Browsable(false)]
        [CategoryAttribute("Output.Format"), Description("Output format of the attribute")]
        [DisplayName("Print Format")]
        [TypeConverter(typeof(AttributePrintFormatEnumTypeConverter))]
        [DefaultValue(AttributePrintValueTypes.Value)]
        [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        public AttributePrintValueTypes PrintFormat
        {
            get { return mPrintFormat; }
            set
            {
                mPrintFormat = value;

                PropertyGridProperty prop_CSharpCode = GetReadOnlyProperty("CSharpCode");
                
                if (mPrintFormat == AttributePrintValueTypes.Custom)
                {
                    prop_CSharpCode.FieldToChange.SetValue(prop_CSharpCode.BrowsableAttribute, true);
                }
                else
                {
                    prop_CSharpCode.FieldToChange.SetValue(prop_CSharpCode.BrowsableAttribute, false);
                }
            }
        }


        [Browsable(false)]
        [CategoryAttribute("Output.Format"), Description("C# code to set output format of the attribute")]
        [DisplayName("C# Code")]
        [Editor(typeof(MultiLineTextEditor), typeof(UITypeEditor))]
        public string CSharpCode { get; set; }      

        [Browsable(false)]
        [CategoryAttribute("0-Global"), Description("If attribute is used by any data source")]
        [DisplayName("Is in Use")]
        public bool IsInUse { get; private set; }

        [Browsable(false)]
        public int? Position { get; private set; }


        public SreAttributeProperty(SreAttribute attribute, int dataSourceId = 0, bool isSystem = false)
            : base(attribute, dataSourceId, isSystem)
        {
        }

        #region Assign
        
        public override void Assign()
        {
            if (Attribute == null)
            {
                //default values, just to show screen beautiful
              
            }
            else
            {
                IsInUse = new Manager().IsAttributeInUse(this.Id);
                
                PropertyGridProperty prop_Name = GetReadOnlyProperty("Name");
                PropertyGridProperty prop_NameEditable = GetReadOnlyProperty("NameEditable");
                PropertyGridProperty prop_Type = GetReadOnlyProperty("Type");
                PropertyGridProperty prop_TypeEditable = GetReadOnlyProperty("TypeEditable");
                PropertyGridProperty prop_Printable = GetReadOnlyProperty("Printable");
                PropertyGridProperty prop_PrintFormat = GetReadOnlyProperty("PrintFormat");
                PropertyGridProperty prop_LovName = GetReadOnlyProperty("LovName");

                if (IsInUse)
                {
                    prop_Name.FieldToChange.SetValue(prop_Name.BrowsableAttribute, true);
                    prop_NameEditable.FieldToChange.SetValue(prop_NameEditable.BrowsableAttribute, false);
                    prop_Type.FieldToChange.SetValue(prop_Type.BrowsableAttribute, true);
                    prop_Printable.FieldToChange.SetValue(prop_Printable.BrowsableAttribute, false);
                    prop_TypeEditable.FieldToChange.SetValue(prop_TypeEditable.BrowsableAttribute, false);
                }
                else
                {
                    prop_Name.FieldToChange.SetValue(prop_Name.BrowsableAttribute, false);
                    prop_NameEditable.FieldToChange.SetValue(prop_NameEditable.BrowsableAttribute, true);
                    prop_Type.FieldToChange.SetValue(prop_Type.BrowsableAttribute, false);
                    prop_Printable.FieldToChange.SetValue(prop_Printable.BrowsableAttribute, false);
                    prop_TypeEditable.FieldToChange.SetValue(prop_TypeEditable.BrowsableAttribute, true);
                }

                if (IsAssociatedWithSystemDataSource)
                {
                    prop_Printable.FieldToChange.SetValue(prop_Printable.BrowsableAttribute, true);
                    prop_PrintFormat.FieldToChange.SetValue(prop_PrintFormat.BrowsableAttribute, true);                   
                }
                else
                {
                    prop_Printable.FieldToChange.SetValue(prop_Printable.BrowsableAttribute, false);
                    prop_PrintFormat.FieldToChange.SetValue(prop_PrintFormat.BrowsableAttribute, false);
                }

                if (Attribute.Type == AttributeTypes.Codeset.ToString())
                {
                    prop_LovName.FieldToChange.SetValue(prop_LovName.BrowsableAttribute, true);
                }
                else
                {
                    prop_LovName.FieldToChange.SetValue(prop_LovName.BrowsableAttribute, false);
                }
                Formula = Attribute.Formula;
                LovName = Services.SreCodeset.ParseFormulaGetCode(Attribute.Formula);
                Position = Attribute.Position;
                Printable = (bool)Attribute.IsAcceptable;
                PrintFormat = Attribute.AttributePrintValueType != null? (AttributePrintValueTypes) Attribute.AttributePrintValueType: AttributePrintValueTypes.Value;             
                Formula = Attribute.Formula;
                CSharpCode = Attribute.AttributePrintValueCustom;
            }

        }

        #endregion Assign

        #region Save
        public override void Save()
        {
            this.Attribute.Name = Name;
            if(this.Attribute.Type.ToString() == AttributeTypes.Codeset.ToString())
                this.Attribute.Formula = string.Format("[defaultcs].[SymplusCodeSet].(\"{0}\")", LovName);
            this.Attribute.IsAcceptable = true;
            this.Attribute.Type = Type.ToString();
            try
            {
                this.Attribute.AttributeId = new Manager().Save(this.Attribute);
            }
            catch
            {
                MessageBox.Show("Fail to save! Ensure that attribute name is unique!", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (IsAssociatedWithSystemDataSource)
                SaveSystemAttributeProperty();

        }

        public void SaveSystemAttributeProperty()
        {
            if (DataSourceId == 0)
                return;

            SreAttributeDataSource sads = new SreAttributeDataSource();
            sads.DataSourceId = DataSourceId;
            sads.AttributeId = Id;
            sads.Position = Position;
            sads.IsAcceptable = Printable;

            sads.AttributePrintValueType = (int)PrintFormat;
            if (PrintFormat == AttributePrintValueTypes.Custom)
                sads.AttributePrintValueCustom = CSharpCode;
            else
                sads.AttributePrintValueCustom = string.Empty;

            new Manager().Save(sads);
        }

        #endregion Save        

        #region Retrieve Property
        PropertyGridProperty GetReadOnlyProperty(string propertyName, bool browsable = true)
        {
            if (!browsable)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())[propertyName];
                ReadOnlyAttribute attribute = (ReadOnlyAttribute)
                                              descriptor.Attributes[typeof(ReadOnlyAttribute)];
                FieldInfo fieldToChange = attribute.GetType().GetField("isReadOnly",
                                                 System.Reflection.BindingFlags.NonPublic |
                                                 System.Reflection.BindingFlags.Instance);

                return new PropertyGridProperty(attribute, fieldToChange);
            }
            else
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())[propertyName];
                BrowsableAttribute attribute = (BrowsableAttribute)
                                              descriptor.Attributes[typeof(BrowsableAttribute)];
                FieldInfo fieldToChange = attribute.GetType().GetField("browsable",
                                                 System.Reflection.BindingFlags.NonPublic |
                                                 System.Reflection.BindingFlags.Instance);

                return new PropertyGridProperty(attribute, fieldToChange);
            }
        }

        #endregion Retrieve Property
    }

   
}


