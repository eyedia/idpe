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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.Services;
using System.Diagnostics;
using Eyedia.Core;

namespace Eyedia.IDPE.Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConfigDatabaseWriter : Window
    {
        #region Properties

        public int DataSourceId { get; private set; }
        public int SystemDataSourceId { get; private set; }

        private List<SreAttribute> SystemAttributes;
        private readonly ColumnMapInfo DatabaseColumnMapInfo;
        private ColumnMap ExistingMap;
        private int TotalRecordsInBatch = 0;
        
        private readonly ObservableCollection<ColumnMapInfo> _ColumnMapInfos;
        public ObservableCollection<ColumnMapInfo> ColumnMapInfos { get { return _ColumnMapInfos; } }
        private ObservableCollection<string> TableNames;
        string _DatabaseTableName;
        const int __LevenshteinDistanceThreshold = 5;
        bool IsMappingLoaded = false;

        public string DatabaseTableName
        {
            get { return _DatabaseTableName; }
            set
            {
                _DatabaseTableName = value;
                if (!string.IsNullOrEmpty(_DatabaseTableName))
                {
                    if (TableNames.Count > 0)
                    {
                        for (int i = 0; i < cmbTableName.Items.Count; i++)
                        {
                            if (cmbTableName.Items[i] == DatabaseTableName)
                            {
                                cmbTableName.SelectedIndex = i;
                                return;
                            }
                        }
                    }
                }
            }
        }
        List<SreKey> ConnectionStringKeys;
        private SreKey OutputWriterDatabaseConfiguration { get; set; }
        private SreKey SelectedConnection { get; set; }

        public ColumnMapInfo SelectedColumnMap
        {
            get { return DatabaseColumnMapInfo; }
            set
            {
                if (value == null) return;
                DatabaseColumnMapInfo.InputColumn = value.InputColumn;
                DatabaseColumnMapInfo.OutputColumn = value.OutputColumn;
                DatabaseColumnMapInfo.OutputDataType = value.OutputDataType;

            }
        }

        #endregion Properties

        #region Constructor
        public ConfigDatabaseWriter(int dataSourceId, int systemDataSourceId)
        {
            InitializeComponent();
            this.FontFamily = SystemFonts.MessageFontFamily;
            this.FontSize = SystemFonts.MessageFontSize;
            this.FontStyle = SystemFonts.MessageFontStyle;
            this.FontWeight = SystemFonts.MessageFontWeight;

            this.DataSourceId = dataSourceId;
            this.SystemDataSourceId = systemDataSourceId;
            ConnectionStringKeys = new Manager().GetDataSourceKeysConnectionStrings(this.DataSourceId, false);
            SystemAttributes = new Manager().GetAttributes(this.SystemDataSourceId);
            DatabaseColumnMapInfo = new ColumnMapInfo(this.DataSourceId);
            _ColumnMapInfos = new ObservableCollection<ColumnMapInfo>();
            TableNames = new ObservableCollection<string>();
            BindData();
            this.DataContext = this;
        }
        #endregion Constructor

        void BindData()
        {
            PopulateConnections();
            PopulateTableNames();
            BindConfigExisting();
        }

        void PopulateConnections()
        {
            cmbConnection.ItemsSource = ConnectionStringKeys;
            cmbConnection.DisplayMemberPath = "Name";
            cmbConnection.SelectedValuePath = "Value";
        }

        void PopulateTableNames()
        {
            if (SelectedConnection != null)
            {
                TableNames = new ObservableCollection<string>(new SqlClientManager(SelectedConnection.Value, (SreKeyTypes)SelectedConnection.Type).GetTableNames());
                cmbTableName.ItemsSource = TableNames;
            }
        }

        void BindConfigExisting()
        {
            OutputWriterDatabaseConfiguration = new Manager().GetKey(DataSourceId, SreKeyTypes.OutputWriterDatabaseConfiguration.ToString());

            if ((OutputWriterDatabaseConfiguration != null)
                && (!string.IsNullOrEmpty(OutputWriterDatabaseConfiguration.Name)))
            {
                ExistingMap = new ColumnMap(DataSourceId, OutputWriterDatabaseConfiguration.Value);
                TableNames = new ObservableCollection<string>(new SqlClientManager(ExistingMap.ConnectionKey.Value, (SreKeyTypes)ExistingMap.ConnectionKey.Type).GetTableNames());
                cmbTableName.ItemsSource = TableNames;
            }
            if (ExistingMap != null)
            {
               
                DatabaseTableName = ExistingMap.TargetTableName;                
                cmbConnection.Text = ExistingMap.ConnectionKey.Name;
                txtCsProcessVariable.Text = ExistingMap.ConnectionKeyRunTime;
                cmbTableName.Text = DatabaseTableName;
                _ColumnMapInfos.Clear();
                foreach (ColumnMapInfo ci in ExistingMap)
                {
                    _ColumnMapInfos.Add(ci);
                }
                IsMappingLoaded = true;
            }
            else
            {
                IsMappingLoaded = false;
            }
        }

        void PopulateTableColumns()
        {
            if (SelectedConnection != null)
            {
                ColumnMap CM = new ColumnMap(this.DataSourceId, SelectedConnection, ConnectionStringKeys, DatabaseTableName, TotalRecordsInBatch);
                _ColumnMapInfos.Clear();
                foreach (ColumnMapInfo ci in CM)
                {
                    _ColumnMapInfos.Add(ci);
                }
            }
        }

        #region Events

        private void lbx1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(lbx1, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(lbx2, typeof(ScrollViewer)) as ScrollViewer;
            _listboxScrollViewer2.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
        }
        private void lbx2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(lbx1, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(lbx2, typeof(ScrollViewer)) as ScrollViewer;
            _listboxScrollViewer1.ScrollToVerticalOffset(_listboxScrollViewer2.VerticalOffset);
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Int32 itemIndex = lbx1.Items.IndexOf(comboBox.DataContext);
            lbx1.SelectedIndex = itemIndex;
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {            
            ComboBox cbBox = sender as ComboBox;
            cbBox.Items.Clear();
            ComboBoxItem cib = new ComboBoxItem();
            cib.Content ="";
            cbBox.Items.Add(cib);

            ComboBoxItem ci = new ComboBoxItem();
            ci.Content = ColumnMapInfo.DatabaseDefault;
            cbBox.Items.Add(ci);

            ci = new ComboBoxItem();
            ci.Content = ColumnMapInfo.CustomDefined;
            cbBox.Items.Add(ci);

            foreach (SreAttribute s in SystemAttributes)
            {
                ci = new ComboBoxItem();
                ci.Content = s.Name;
                //ci.Tag = s.Type;
                cbBox.Items.Add(ci);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null)
            {
                DatabaseColumnMapInfo.InputColumn = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();  //comboBox.SelectedValue.ToString();
                Int32 itemIndex = lbx1.Items.IndexOf(comboBox.DataContext);
                lbx1.SelectedIndex = itemIndex;
                _ColumnMapInfos[lbx1.SelectedIndex].InputColumn = DatabaseColumnMapInfo.InputColumn;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_Initialized(object sender, EventArgs e)
        {
            if (SelectedConnection != null)
            {
                ComboBox comboBox = sender as ComboBox;
                ComboBoxItem ci = new ComboBoxItem();
                if (IsMappingLoaded)
                {
                    Int32 itemIndex = lbx1.Items.IndexOf(comboBox.DataContext);
                    object itemdest = lbx1.Items[itemIndex];
                    ci.Content = ((ColumnMapInfo)itemdest).InputColumn;
                }
                else
                {
                    Int32 itemIndex = lbx1.Items.IndexOf(comboBox.DataContext);
                    object itemsource = lbx1.Items[itemIndex];
                    ci.Content = SetAttributeNameUsingLevenshteinDistance(((ColumnMapInfo)itemsource).OutputColumn);


                }
                comboBox.Items.Add(ci);
                comboBox.SelectedIndex = 0;
            }
        }

        private void cmbConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedConnection = cmbConnection.SelectedItem as SreKey;
            PopulateTableNames();
        }

        private void cmbTableName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTableName.SelectedItem != null)
            {
                DatabaseTableName = cmbTableName.SelectedItem.ToString();
                PopulateTableColumns();
                IsMappingLoaded = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int result = SaveConfiguration(this.DataSourceId, ColumnMapInfos.ToList<ColumnMapInfo>(), SelectedConnection.Name,txtCsProcessVariable.Text, cmbTableName.SelectedItem.ToString(), TotalRecordsInBatch);
            if (result == 0)
            {
                txtMessage.Text = "configuration saved successfully";
                this.Close();
            }
            else
                txtMessage.Text = "Failed to save configuration";
        }

        #endregion Events

        #region Helpers

        private string SetAttributeNameUsingLevenshteinDistance(string strSource)
        {
            string source = strSource;
            List<string> targets = new List<string>();
            foreach (SreAttribute item in SystemAttributes) { targets.Add(item.Name); }
            string matchResult = source.TryMatchUsingLevenshteinDistance(targets, __LevenshteinDistanceThreshold);
            return matchResult;           
        }

        public int SaveConfiguration(int dataSourceId, List<ColumnMapInfo> columnmaplist, string connectionStringKeyName, string connectionStringKeyNameRunTime, string TableName, int TotalRecordsInBatch)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("[" + connectionStringKeyName + "," + connectionStringKeyNameRunTime + "," + TableName + "," + TotalRecordsInBatch.ToString() + "]");
                foreach (ColumnMapInfo c in columnmaplist)
                {
                    sb.Append("[" + c.OutputColumn + "|" + (int)c.OutputDataType + "," + c.InputColumn + "]");
                }

                SreKey key = new SreKey();
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    key.Name = SreKeyTypes.OutputWriterDatabaseConfiguration.ToString();
                    key.Value = sb.ToString();
                    key.Type = (int)SreKeyTypes.OutputWriterDatabaseConfiguration;
                    new Manager().Save(key, dataSourceId);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Key value not saved for " + sb.ToString() + Environment.NewLine + ex.Message);
                return 1;
            }
        }

        public Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        #endregion Helpers

        private void txtBatch_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }
    }

}



