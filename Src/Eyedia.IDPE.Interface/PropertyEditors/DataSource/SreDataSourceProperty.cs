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
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    //[DefaultPropertyAttribute("SaveOnClose")]
    public partial class SreDataSourceProperty:SrePropertyGrid
    {
        [Browsable(false)]
        public List<SreKey> DataSourceKeys { get; set; }

        [Browsable(true)]
        [CategoryAttribute("0-Global"), Description("System generated unique identifier of the data source")]
        public int Id { get { return DataSource.Id; } }

        [Browsable(true)]
        [CategoryAttribute("0-Global"), Description("Name of the data source")]
        public string Name { get { return DataSource.Name; } }

        [Browsable(false)]
        [DisplayName("Name")]
        [CategoryAttribute("0-Global"), Description("Name of the data source")]
        public string EditableName 
        { 
            get 
            { 
                return DataSource.Name; 
            } 
            set 
            { 
                DataSource.Name = value;
                if (new Manager().ApplicationExists(DataSource.Name))
                    ValidationError = string.Format("The '{0}' already exists! Please chose a different name", DataSource.Name);
                else
                    ValidationError = string.Empty;

            } 
        }
       

        [Browsable(true)]
        [DefaultValue(true)]
        [CategoryAttribute("0-Global"), Description("If this data source is enabled")]
        public bool Enabled { get { return DataSource.IsActive; } set { DataSource.IsActive = value; } }


        [Browsable(true)]
        [Editor(typeof(MultiLineTextEditor), typeof(UITypeEditor))]
        [CategoryAttribute("0-Global"), Description("Description of the data source")]
        public string Description { get { return DataSource.Description; } set { DataSource.Description = value; } }

        [Browsable(true)]
        [DisplayName("System Name")]
        [Editor(typeof(EditorSystemDataSource), typeof(UITypeEditor))]
        [CategoryAttribute("0-Global"), Description("System name")]
        public string SystemName { get; set; }

        [Browsable(true)]
        [DisplayName("System Id")]
        [CategoryAttribute("0-Global"), Description("System id")]
        public int SystemDataSourceId { get; internal set; }

        [Browsable(true)]
        [DisplayName("Test File Name")]
        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(UITypeEditor))]
        [CategoryAttribute("0-Global"), Description("Test file name to process the file instantly on test bed")]
        public string TestFileName { get; set; }

        [Browsable(false)]
        public string PusherTypeFullName { get; set; }

        public SreDataSourceProperty(SreDataSource dataSource)
            : base(dataSource)
        {
        }

        #region Assign

        public override void Assign()
        {
            PropertyGridProperty prop_Name = GetReadOnlyProperty("Name");
            PropertyGridProperty prop_EditableName = GetReadOnlyProperty("EditableName");

            if (DataSource == null)
            {
                //default values, just to show screen beautiful
                DataFormatType = DataFormatTypes.Delimited;
                OutputFileNameFormat = "Same as Input";
                OutputFileExtension = ".csv";
                HasHeader = true;
                OutputHasHeader = true;
                prop_Name.FieldToChange.SetValue(prop_Name.BrowsableAttribute, true);
                prop_EditableName.FieldToChange.SetValue(prop_EditableName.BrowsableAttribute, false);
            }
            else
            {
                if (DataSource.Id == 0)
                {
                    prop_Name.FieldToChange.SetValue(prop_Name.BrowsableAttribute, false);
                    prop_EditableName.FieldToChange.SetValue(prop_EditableName.BrowsableAttribute, true);
                    HasHeader = true;
                    OutputHasHeader = true;
                }
                else
                {
                    DataSourceKeys = new Manager().GetKeys(this.DataSource.Id);
                    prop_Name.FieldToChange.SetValue(prop_Name.BrowsableAttribute, true);
                    prop_EditableName.FieldToChange.SetValue(prop_EditableName.BrowsableAttribute, false);
                    HasHeader = DataSourceKeys.GetKeyValue(SreKeyTypes.IsFirstRowHeader).ParseBool();
                    OutputHasHeader = DataSourceKeys.GetKeyValue(SreKeyTypes.OutputIsFirstRowHeader).ParseBool();
                }

                DataFeederType = (DataFeederTypes)DataSource.DataFeederType;
                DataFormatType = (DataFormatTypes)DataSource.DataFormatType;
                Description = DataSource.Description;
                SystemDataSourceId = (int)DataSource.SystemDataSourceId;

                if(DataFormatType == DataFormatTypes.Custom)
                {
                    SreKey key = new Manager().GetKey(this.DataSource.Id, SreKeyTypes.FileInterfaceName.ToString());
                    if (key != null)
                        ConfigureDataFormat = key.Value;
                }

                if (SystemDataSourceId > 0)
                {
                    SreDataSource sysDs = new Manager().GetDataSourceDetails(SystemDataSourceId);
                    if (sysDs != null)
                        SystemName = sysDs.Name;
                }
                else
                {
                    SystemName = string.Empty;
                }
                OutputDataFormatType = (OutputTypes)DataSource.OutputType;
                OutputCustomInterfaceName = DataSource.OutputWriterTypeFullName;

                SreKey keyTestFileName = new Manager().GetKey(this.DataSource.Id, SreKeyTypes.TestFileName.ToString());
                if (keyTestFileName != null)
                    TestFileName = keyTestFileName.Value;

                #region Post Events

                PusherTypeFullName = DataSource.PusherTypeFullName;
                PostEvent = (PusherTypes)DataSource.PusherType;
                PostEventCustomInterfaceName = DataSource.PusherTypeFullName;

                #endregion Post Events

                if (DataSource.Id == 0)
                    return;//as keys are not available, its a new data source

                SpreadSheetNumber = (int)DataSourceKeys.GetKeyValue(SreKeyTypes.SpreadSheetNumber).ParseInt();
                WriteStandardOutput = DataSourceKeys.GetKeyValue(SreKeyTypes.WcfCallsGenerateStandardOutput).ParseBool();             

                if (string.IsNullOrEmpty(DataSource.Delimiter))
                    Delimiter = ",";
                else if ((DataSource.Delimiter != null) && (DataSource.Delimiter.ToLower() == "\t"))
                    Delimiter = "Tab";
                else
                    Delimiter = DataSource.Delimiter;
               
                RenameHeader = DataSourceKeys.GetKeyValue(SreKeyTypes.RenameColumnHeader).ParseBool();

                #region Output

                AllowPartial = DataSourceKeys.GetKeyValue(SreKeyTypes.OutputPartialRecordsAllowed).ParseBool();
               
                string strDelmiterOutput = DataSourceKeys.GetKeyValue(SreKeyTypes.OutputDelimiter);

                if (string.IsNullOrEmpty(strDelmiterOutput))
                    OutputDelimiter = ",";
                else if ((strDelmiterOutput != null) && (strDelmiterOutput.ToLower() == "\t"))
                    OutputDelimiter = "Tab";
                else
                    OutputDelimiter = strDelmiterOutput;

                string outputFileName = DataSourceKeys.GetKeyValue(SreKeyTypes.OutputFileName);
                if (!string.IsNullOrEmpty(outputFileName))
                    OutputFileNameFormat = outputFileName;
                else
                    OutputFileNameFormat = "Same as Input";

                string outputFileExt = DataSourceKeys.GetKeyValue(SreKeyTypes.OutputFileExtension);
                if (!string.IsNullOrEmpty(outputFileExt))
                    OutputFileExtension = outputFileExt;
                else
                    OutputFileExtension = ".xml";
                
                EncloseWithDoubleQuote = !DataSourceKeys.GetKeyValue(SreKeyTypes.OutputDelimiterDoNotEncloseWithDoubleQuote).ParseBool();
                #endregion Output
            }

        }

        #endregion Assign

        #region Save
        public override void Save()
        {
            KeepVersion(this.DataSource.Id);

            bool isInserted = false;
            if (this.DataSource.Id == 0)
            {
                this.DataSource.SystemDataSourceId = SystemDataSourceId;
                if (this.DataSource.SystemDataSourceId == 0)
                {
                    //sometimes user does not complete full add and navigate to different zone.
                    //for now, lets ignore such case
                    return;
                }
                int dsId = new Manager().Save(this.DataSource, ref isInserted);
                this.DataSourceId = dsId;
                this.DataSource.Id = dsId;
                Assign();

            }

            SaveBasic();          
            SaveOutputWriterKeys();
               
        }

        public static void KeepVersion(int dataSourceId)
        {
            new SreVersionManager().KeepVersion(VersionObjectTypes.DataSource, dataSourceId);
        }
        #region Save Basic
        private void SaveBasic()
        {
            if (this.DataSource.IsSystem == true)
                throw new NotImplementedException("System datasource save is not working!");

            Manager manager = new Manager();

            this.DataSource.Name = Name;
            this.DataSource.Description = Description;
            this.DataSource.IsActive = Enabled;
            this.DataSource.IsSystem = false;
            this.DataSource.Delimiter = Delimiter;
            if (this.DataSource.Delimiter.ToLower() == "tab")
                this.DataSource.Delimiter = "\t";

            this.DataSource.SystemDataSourceId = SystemDataSourceId;            
            this.DataSource.DataFeederType = (int)DataFeederType;
            if (DataFeederType == DataFeederTypes.PullSql)
                this.DataSource.DataFormatType = (int)DataFormatTypes.Sql;
            else
                this.DataSource.DataFormatType = (int)DataFormatType;

            this.DataSource.OutputType = (int)OutputDataFormatType;
            this.DataSource.OutputWriterTypeFullName = (OutputTypes)this.DataSource.OutputType == OutputTypes.Custom ? OutputCustomInterfaceName : null;

            this.DataSource.PusherType = (int)PostEvent;
            if (PostEvent == PusherTypes.None)
                this.DataSource.PusherTypeFullName = string.Empty;
            else if (PostEvent == PusherTypes.Custom)
                this.DataSource.PusherTypeFullName = PostEventCustomInterfaceName;
            else
                this.DataSource.PusherTypeFullName = PusherTypeFullName;

            manager.Save(this.DataSource);

            switch (DataFormatType)
            {
                case DataFormatTypes.FixedLength:
                    manager.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.IsFirstRowHeader.ToString(), true);
                    break;

                case DataFormatTypes.SpreadSheet:
                    SreKey key = new SreKey();
                    key.Name = SreKeyTypes.SpreadSheetNumber.ToString();
                    key.Value = SpreadSheetNumber.ToString();
                    manager.Save(key, DataSource.Id);
                    break;
            }

        
            if ((DataFormatType == DataFormatTypes.Delimited)
                || (DataFormatType == DataFormatTypes.SpreadSheet)
                || (DataFormatType == DataFormatTypes.CSharpCode)
                || (DataFormatType == DataFormatTypes.Custom))
            {
                if (HasHeader)
                {
                    SreKey key = new SreKey();
                    key.Name = SreKeyTypes.IsFirstRowHeader.ToString();
                    key.Value = "True";
                    key.Type = (int)SreKeyTypes.IsFirstRowHeader;
                    manager.Save(key, this.DataSource.Id);
                }
                else
                {
                    manager.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.IsFirstRowHeader.ToString(), false);
                }

            }

            if (RenameHeader)
            {
                SreKey key = new SreKey();
                key.Name = SreKeyTypes.RenameColumnHeader.ToString();
                key.Value = "True";
                key.Type = (int)SreKeyTypes.RenameColumnHeader;
                manager.Save(key, this.DataSource.Id);
            }
            else
            {
                manager.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.RenameColumnHeader.ToString(), true);
            }

            if (!string.IsNullOrEmpty(TestFileName))
            {
                SreKey keyTestFileName = new SreKey();
                keyTestFileName.Name = SreKeyTypes.TestFileName.ToString();
                keyTestFileName.Value = TestFileName;
                keyTestFileName.Type = (int)SreKeyTypes.TestFileName;
                manager.Save(keyTestFileName, this.DataSource.Id);
            }
            else
            {
                manager.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.TestFileName.ToString(), true);
            }

            manager.SavePushConfig(this.DataSource.Id, WriteStandardOutput);
        }

        #endregion Save Basic

        #region SaveOutputWriterKeys

        private void SaveOutputWriterKeys()
        {
            
            Manager dm = new Manager();
            SreKey key = new SreKey();
            key.Name = SreKeyTypes.OutputPartialRecordsAllowed.ToString();
            key.Value = AllowPartial.ToString();
            key.Type = (int)SreKeyTypes.OutputPartialRecordsAllowed;
            dm.Save(key, DataSource.Id);

            key = new SreKey();
            key.Name = SreKeyTypes.OutputIsFirstRowHeader.ToString();
            key.Value = OutputHasHeader.ToString();
            key.Type = (int)SreKeyTypes.OutputIsFirstRowHeader;
            dm.Save(key, DataSource.Id);


            switch (OutputDataFormatType)
            {
                case OutputTypes.Xml:
                    dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputDelimiter.ToString(), true);
                    dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputIsFirstRowHeader.ToString(), true);
                    dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputDelimiterDoNotEncloseWithDoubleQuote.ToString(), true);
                    break;

                case OutputTypes.Delimited:

                    key = new SreKey();
                    key.Name = SreKeyTypes.OutputDelimiter.ToString();
                    if (OutputDelimiter.ToLower() == "tab")
                        key.Value = "\t";
                    else if (string.IsNullOrEmpty(OutputDelimiter))
                        key.Value = ",";
                    else
                        key.Value = OutputDelimiter;
                    key.Type = (int)SreKeyTypes.OutputDelimiter;
                    dm.Save(key, DataSource.Id);


                    key = new SreKey();
                    key.Name = SreKeyTypes.OutputDelimiterDoNotEncloseWithDoubleQuote.ToString();
                    key.Value = (!EncloseWithDoubleQuote).ToString();
                    key.Type = (int)SreKeyTypes.OutputDelimiterDoNotEncloseWithDoubleQuote;
                    dm.Save(key, DataSource.Id);

                    break;

                case OutputTypes.FixedLength:
                    dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputDelimiter.ToString(), true);
                    dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputIsFirstRowHeader.ToString(), true);
                    dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputDelimiterDoNotEncloseWithDoubleQuote.ToString(), true);
                    break;

                case OutputTypes.Database:
                    OutputFileExtension = ".csv";
                    break;

                case OutputTypes.Custom:
                    break;
            }

            if (OutputFileNameFormat.ToLower() != "same as input")
            {
                key = new SreKey();
                key.Name = SreKeyTypes.OutputFileName.ToString();
                key.Value = OutputFileNameFormat;
                key.Type = (int)SreKeyTypes.OutputFileName;
                dm.Save(key, DataSource.Id);
            }
            else
            {
                dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputFileName.ToString(), true);
            }

            if (OutputFileExtension.ToLower() != ".xml")
            {
                key = new SreKey();
                key.Name = SreKeyTypes.OutputFileExtension.ToString();
                key.Value = OutputFileExtension;
                key.Type = (int)SreKeyTypes.OutputFileExtension;
                dm.Save(key, DataSource.Id);
            }
            else
            {
                dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputFileExtension.ToString(), true);
            }

        }
        #endregion SaveOutputWriterKeys
    

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

    public class PropertyGridProperty
    {
        public ReadOnlyAttribute ReadOnlyAttribute { get; private set; }
        public BrowsableAttribute BrowsableAttribute { get; private set; }
        public FieldInfo FieldToChange { get; private set; }

        public PropertyGridProperty(ReadOnlyAttribute attribute, FieldInfo fieldToChange)
        {
            this.ReadOnlyAttribute = attribute;
            this.FieldToChange = fieldToChange;
        }

        public PropertyGridProperty(BrowsableAttribute attribute, FieldInfo fieldToChange)
        {
            this.BrowsableAttribute = attribute;
            this.FieldToChange = fieldToChange;
        }
    }
}


