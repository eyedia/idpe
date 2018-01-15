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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using Eyedia.Core;
using Eyedia.Core.Data;
using System.Diagnostics;
using Eyedia.Core.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Eyedia.IDPE.Interface.Controls;
using Eyedia.IDPE.Clients;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class MainWindow : SREBaseFormNew
    {
        #region MainWindow Properties

        private IdpeDataSource SelectedDataSource { get; set; }     
        private DockPanelGlobalAttributes m_dm_GlobalAttributes;
        private DockPanelDataSources m_dm_DataSources;
        private DockPanelProperty m_dm_Property;
        private DockPanelDataSourceAttributes m_dm_Attributes;
        private DockPanelMapper m_dm_Mapper;
        private DockPanelDataSourceSystemAttributes m_dm_SystemAttributes;
        private RuleEditor.MainWindow m_Rules_Editor;
        private DockContent m_Rules;
        private DockContent m_Monitor;       
        private DockContent m_FileLog;
        private DockContent m_EmailLog;
        private DeserializeDockContent m_deserializeDockContent;

        #endregion MainWindow Properties

        #region MainWindow Events
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetSchema();
            EnvironmentFiles.KillTestBedService();
        }

        #endregion MainWindow Events

        #region Common


        void ShowHideAllDataSourcePanels(bool show)
        {
            m_dm_GlobalAttributes.Visible = show;
            m_dm_DataSources.Visible = show;
            m_dm_Property.Visible = show;
            m_dm_Attributes.Visible = show;
            m_dm_Mapper.Visible = show;
            m_dm_SystemAttributes.Visible = show;
        }

        private void SetSchema()
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.temp.config");
            dockPanel.SaveAsXml(configFile);
        }

        private bool GetSchema()
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.temp.config");
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            if (File.Exists(configFile))
            {
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
                return true;
            }
            return false;
        }

        public void OpenDataSourceWindow()
        {
            //if (GetSchema())
            //    return;

            if (m_dm_GlobalAttributes != null) //already open               
                return;

            dockPanel.Visible = false;
            dockPanel.SuspendLayout(true);

            CloseOtherWindows();

            m_dm_GlobalAttributes = new DockPanelGlobalAttributes();
            m_dm_GlobalAttributes.CloseButtonVisible = false;
            m_dm_GlobalAttributes.sreListView1.ItemSelectedIndexChanged += new EventHandler(OnAttributeChanged);
            m_dm_GlobalAttributes.sreListView1.AddButtonClick += new EventHandler(OnAttributeAddButtonClicked);
            m_dm_GlobalAttributes.sreListView1.PrintButtonClick += new EventHandler(OnAttributePrintButtonClicked);
            m_dm_GlobalAttributes.Show(dockPanel, DockState.DockLeftAutoHide);

            m_dm_DataSources = new DockPanelDataSources();
            m_dm_DataSources.sreListView1.ItemSelectedIndexChanged += new EventHandler(OnDataSourceChanged);
            m_dm_DataSources.sreListView1.AddButtonClick += new EventHandler(OnDataSourceAddButtonClicked);
            m_dm_DataSources.sreListView1.PrintButtonClick += new EventHandler(OnDataSourcePrintButtonClicked);
            m_dm_DataSources.CloseButtonVisible = false;
            m_dm_DataSources.Show(dockPanel, DockState.Document);
         
            m_Rules = new Rules();           
            m_Rules.CloseButtonVisible = false;
            m_Rules.Show(dockPanel, DockState.DockLeftAutoHide);
            m_Rules.AutoHidePortion = 0.50;
        
            m_Monitor = new Monitor();           
            m_Monitor.CloseButtonVisible = false;
            m_Monitor.Show(dockPanel, DockState.DockRightAutoHide);
            m_Monitor.AutoHidePortion = 0.5;         

            m_FileLog = new FileLog();            
            m_FileLog.CloseButtonVisible = false;
            m_FileLog.Show(dockPanel, DockState.DockBottomAutoHide);
            m_FileLog.AutoHidePortion = 0.965;


            m_EmailLog = new EmailLogs();           
            m_EmailLog.CloseButtonVisible = false;
            m_EmailLog.Show(dockPanel, DockState.DockRightAutoHide);
            m_EmailLog.AutoHidePortion = 0.5;

            m_Rules_Editor = new RuleEditor.MainWindow(new IdpeRule(), Common.RuleSetTypes.RowPreparing);            

            m_dm_Property = new DockPanelProperty();
            m_dm_Property.CloseButtonVisible = false;
            m_dm_Property.MainWindow = this;
            m_dm_Property.Show(m_dm_DataSources.Pane, DockAlignment.Right, 0.5);

            m_dm_Attributes = new DockPanelDataSourceAttributes();
            m_dm_Attributes.sreListView1.ItemSelectedIndexChanged += new EventHandler(OnAttributeChanged);
            m_dm_Attributes.sreListView1.Repositioned +=new EventHandler(OnAttributeRepositioned);
            m_dm_Attributes.sreListView1.DeleteKeyPressed += new EventHandler(OnAttributeDelete);
            m_dm_Attributes.CloseButtonVisible = false;
            m_dm_Attributes.MainWindow = this;

            m_dm_SystemAttributes = new DockPanelDataSourceSystemAttributes();
            m_dm_SystemAttributes.sreListView1.ItemSelectedIndexChanged += new EventHandler(OnAttributeChanged);
            m_dm_SystemAttributes.sreListView1.Repositioned += new EventHandler(OnAttributeRepositioned);
            m_dm_SystemAttributes.sreListView1.DeleteKeyPressed += new EventHandler(OnAttributeDelete);
            m_dm_SystemAttributes.CloseButtonVisible = false;
            m_dm_SystemAttributes.MainWindow = this;

            m_dm_Mapper = new DockPanelMapper();
            m_dm_Mapper.CloseButtonVisible = false;

            m_dm_Attributes.Show(m_dm_DataSources.Pane, DockAlignment.Bottom, 0.65);
            m_dm_SystemAttributes.Show(m_dm_Attributes.Pane, DockAlignment.Right, 0.45);
            m_dm_Mapper.Show(m_dm_Attributes.Pane, DockAlignment.Right, 0.2);

            if (m_dm_DataSources.sreListView1.ListView.Items.Count > 0)
                m_dm_DataSources.sreListView1.ListView.Items[0].Selected = true;

            m_dm_DataSources.Activate();
            dockPanel.ResumeLayout(true, true);
            dockPanel.Visible = true;
        }

        void OnAttributePrintButtonClicked(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmReports printReport = new frmReports(frmReports.ReportTypes.Attributes);
            printReport.ShowDialog();
            this.Cursor = Cursors.Default;
        }
            void OnAttributeAddButtonClicked(object sender, EventArgs e)
        {
            IdpeAttribute newAttribute = new IdpeAttribute();
            newAttribute.IsAcceptable = true;
            newAttribute.Type = "String";
            m_dm_Property.IsAssociatedWithSystemDataSource = false;
            m_dm_Property.Attribute = newAttribute;            
            m_dm_Property.propertyGrid.Focus();
        }

        void OnDataSourcePrintButtonClicked(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmReports printReport = new frmReports(frmReports.ReportTypes.DataSources);
            printReport.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        void OnDataSourceAddButtonClicked(object sender, EventArgs e)
        {
            IdpeDataSource newDs = new IdpeDataSource();
            newDs.IsSystem = false;
            newDs.IsActive = true;
            newDs.DataFeederType = (int)DataFeederTypes.PullLocalFileSystem;
            newDs.DataFormatType = (int)DataFormatTypes.Delimited;
            newDs.Delimiter = ",";           
            newDs.SystemDataSourceId = 0;
            newDs.OutputType = (int)OutputTypes.Delimited;
            newDs.PusherType = (int)PusherTypes.None;
            m_dm_Property.DataSource = newDs;
            m_dm_Property.ToggleDataSource(false);
            m_dm_Property.propertyGrid.Focus();
        }

        void OnAttributeDelete(object sender, EventArgs e)
        {
            SreListView listView = sender as SreListView;
            if (listView.Parent is DockPanelDataSourceAttributes)
            {
                DisassociateAttributeFromDataSource(m_dm_Attributes.sreListView1);
            }
            else if (listView.Parent is DockPanelDataSourceSystemAttributes)
            {
                DisassociateAttributeFromDataSource(m_dm_SystemAttributes.sreListView1, true);
            }
        }

        void OnAttributeRepositioned(object sender, EventArgs e)
        {
            SreListView listView = sender as SreListView;
            if (listView.Parent is DockPanelDataSourceAttributes)
            {
                SaveAssociations(m_dm_Attributes.sreListView1.ListView);
            }
            else if (listView.Parent is DockPanelDataSourceSystemAttributes)
            {
                SaveAssociations(m_dm_SystemAttributes.sreListView1.ListView, true);
            }

            if(SreServiceCommunicator.ClearDataSource(SelectedDataSource.Id, string.Empty))
                SetToolStripStatusLabel("Ready");
            else
                SetToolStripStatusLabel("Failed to clear cache", true);
        }
        public void DisassociateAttributeFromDataSource(SreListView sreListView1, bool isSystemDataSource = false)
        {
            int dataSourceId = isSystemDataSource ? (int)SelectedDataSource.SystemDataSourceId : SelectedDataSource.Id;
            bool saved = false;
            Manager manager = new Manager();
            SreDataSourceProperty.KeepVersion(dataSourceId);

            if (sreListView1.ListView.SelectedItems.Count > 0)
            {
                if ((sreListView1.ListView.SelectedItems.Count == 1)
                    && (sreListView1.ListView.SelectedItems[0].Text != "IsValid"))
                {
                    if (MessageBox.Show(string.Format("You are about to disassociate {0} from {1}. Are you sure?", sreListView1.ListView.SelectedItems[0].Text, SelectedDataSource.Name),
                   "Disassociate Attribute", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return;
                    
                    if (manager.DisassociateAttributeFromDataSource(dataSourceId, (sreListView1.ListView.SelectedItems[0].Tag as IdpeAttribute).AttributeId) == true)
                    {
                        sreListView1.ListView.Items.Remove(sreListView1.ListView.SelectedItems[0]);
                        saved = true;
                    }
                }
                else
                {
                    foreach (ListViewItem selectedItem in sreListView1.ListView.SelectedItems)
                    {
                        if (selectedItem.Text != "IsValid")
                        {
                            if (manager.DisassociateAttributeFromDataSource(dataSourceId, (selectedItem.Tag as IdpeAttribute).AttributeId) == true)
                                sreListView1.ListView.Items.Remove(sreListView1.ListView.SelectedItems[0]);
                            saved = true;
                        }
                    }
                }
            }

            if (saved)
                SaveAssociations(sreListView1.ListView, isSystemDataSource, false);

            if(SreServiceCommunicator.ClearDataSource(SelectedDataSource.Id, string.Empty))
                SetToolStripStatusLabel("Ready");
            else
                SetToolStripStatusLabel("Failed to clear cache", true);

        }
        void SaveAssociations(ListView listView, bool isSystemDataSource = false, bool takeCareOfOldVersion = true)
        {
            int dataSourceId = isSystemDataSource ? (int)SelectedDataSource.SystemDataSourceId : SelectedDataSource.Id;
            Manager manager = new Manager();
            if (takeCareOfOldVersion)
                SreDataSourceProperty.KeepVersion(dataSourceId);

            int position = 1;
            foreach (ListViewItem item in listView.Items)
            {
                IdpeAttribute attribute = item.Tag as IdpeAttribute;
                if (attribute.Name == "IsValid")
                    continue;
                IdpeAttributeDataSource sds = new IdpeAttributeDataSource();
                sds.DataSourceId = dataSourceId;
                sds.IsAcceptable = (bool)attribute.IsAcceptable;
                sds.AttributeId = attribute.AttributeId;
                sds.AttributePrintValueType = attribute.AttributePrintValueType;
                sds.AttributePrintValueCustom = attribute.AttributePrintValueCustom;
                sds.Position = position;
                toolStripStatusLabel1.Text = "Please wait...Saving..." + attribute.Name;
                Application.DoEvents();
                try
                {
                    manager.Save(sds);
                }
                catch
                {                
                    MessageBox.Show(string.Format("{0} could not be loaded!",attribute.Name), "Attribute Skipped",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                position++;
            }
        }

        private void CloseOtherWindows()
        {
            if (m_Rules_Editor != null)
            {
                m_Rules_Editor.Close();
                m_Rules_Editor = null;
            }
        }

        void OnAttributeChanged(object sender, EventArgs e)
        {
            //if ((SelectedDataSource != null) && (SelectedDataSource.Id == 100))
            //{
            //    m_dm_GlobalAttributes.btnAssociateBoth.Enabled = false;
            //    m_dm_GlobalAttributes.btnAssociateAttributeDataSource.Enabled = false;
            //    m_dm_GlobalAttributes.btnAssociateAttributeSystemDataSource.Enabled = false;
            //    this.Cursor = Cursors.Default;
            //    return;
            //}

            m_dm_GlobalAttributes.btnAssociateBoth.Enabled = true;
            m_dm_GlobalAttributes.btnAssociateAttributeDataSource.Enabled = true;
            m_dm_GlobalAttributes.btnAssociateAttributeSystemDataSource.Enabled = true;
            SreListView sreListView = sender as SreListView;
            if (sreListView != null)
            {
                if (sreListView.ListView.SelectedItems.Count > 0)
                {
                    IdpeAttribute selectedAttribute = sreListView.ListView.SelectedItems[0].Tag as IdpeAttribute;
                    if ((m_dm_Property.propertyGrid.SelectedObject != null)
                        && (((SrePropertyGrid)m_dm_Property.propertyGrid.SelectedObject).HasChanged))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        toolStripStatusLabel1.Text = "Please wait...Saving properties...";
                        Application.DoEvents();
                        ((SrePropertyGrid)m_dm_Property.propertyGrid.SelectedObject).Save();
                        toolStripStatusLabel1.Text = "Please wait...Clearing cache...";
                        Application.DoEvents();
                        new SreClient().ClearCache();
                        
                        toolStripStatusLabel1.Text = "Please wait...Refresing...";
                        Application.DoEvents();
                        m_dm_SystemAttributes.RefreshData();
                        if(MainWindowShown)
                            SetToolStripStatusLabel("Ready");
                        this.Cursor = Cursors.Default;
                    }
                  
                    m_dm_Property.IsAssociatedWithSystemDataSource = sreListView.Parent is DockPanelDataSourceSystemAttributes;
                    if (sreListView.Parent is DockPanelGlobalAttributes)
                        m_dm_Property.DataSourceId = 0;
                    else
                        m_dm_Property.DataSourceId = m_dm_Property.IsAssociatedWithSystemDataSource == false ? SelectedDataSource.Id : (int)SelectedDataSource.SystemDataSourceId;
                    m_dm_Property.Attribute = selectedAttribute;
                    sreListView.ListView.Focus();
                }
            }         
        }

        public void RefreshData(int selectedDataSourceId = 0)
        {
            m_dm_DataSources.RefreshData(selectedDataSourceId);
        }

        #endregion Common

        #region Global Attribute Events
        void OnAttributePrintClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void OnAttributeAddClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
      
        
        #endregion Global Attribute Events       

        #region DataSource Events
        void OnDataSourceChanged(object sender, EventArgs e)
        {            
           
            this.Cursor = Cursors.WaitCursor;
            dockPanel.SuspendLayout(true);
            SreListView sreListView = sender as SreListView;
            if (sreListView != null)
            {
                if (sreListView.ListView.SelectedItems.Count > 0)
                {
                    //if (((IdpeDataSource)sreListView.ListView.SelectedItems[0].Tag).Id == 100)
                    //{
                    //    m_dm_GlobalAttributes.btnAssociateBoth.Enabled = false;
                    //    m_dm_GlobalAttributes.btnAssociateAttributeDataSource.Enabled = false;
                    //    m_dm_GlobalAttributes.btnAssociateAttributeSystemDataSource.Enabled = false;
                    //    this.Cursor = Cursors.Default;
                    //    return;
                    //}
                    //else
                    //{
                    //    m_dm_GlobalAttributes.btnAssociateBoth.Enabled = true;
                    //    m_dm_GlobalAttributes.btnAssociateAttributeDataSource.Enabled = true;
                    //    m_dm_GlobalAttributes.btnAssociateAttributeSystemDataSource.Enabled = true;
                    //}

                    if ((m_dm_Property.propertyGrid.SelectedObject != null)
                        && (((SrePropertyGrid)m_dm_Property.propertyGrid.SelectedObject).HasChanged))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        toolStripStatusLabel1.Text = "Please wait...Saving properties...";
                        Application.DoEvents();
                        ((SrePropertyGrid)m_dm_Property.propertyGrid.SelectedObject).Save();

                        toolStripStatusLabel1.Text = "Please wait...Clearing cache...";
                        Application.DoEvents();
                        if(SreServiceCommunicator.ClearDataSource(SelectedDataSource.Id, string.Empty))
                            SetToolStripStatusLabel("Ready");
                        else
                            SetToolStripStatusLabel("Failed to clear cache", true);

                        toolStripStatusLabel1.Text = "Please wait...Refresing...";
                        Application.DoEvents();
                        RefreshData();
                        if (MainWindowShown)
                            SetToolStripStatusLabel("Ready");
                        this.Cursor = Cursors.Default;
                    }

                    if (sreListView.ListView.SelectedItems.Count > 0)
                    {
                        toolStripStatusLabel1.Text = "Please wait...Refresing...";
                        Application.DoEvents();
                        if ((sreListView.DefaultItemId > 0) && (!MainWindowShown))
                        {
                            SelectedDataSource = sreListView.ListView.Items.Cast<ListViewItem>().Select(x => x.Tag).ToList().Cast<IdpeDataSource>().Where(ds => ds.Id == sreListView.DefaultItemId).SingleOrDefault();
                            if (sreListView.DefaultItem != null)
                                sreListView.DefaultItem.EnsureVisible();

                            if (SelectedDataSource == null)
                                SelectedDataSource = sreListView.ListView.Items[0].Tag as IdpeDataSource;
                        }
                        else
                        {
                            SelectedDataSource = sreListView.ListView.SelectedItems[0].Tag as IdpeDataSource;
                        }

                        m_dm_DataSources.SelectedDataSource = SelectedDataSource;
                        m_dm_Attributes.SelectedDataSource = SelectedDataSource;
                        m_dm_SystemAttributes.SelectedDataSource = SelectedDataSource;
                        m_dm_GlobalAttributes.SelectedDataSource = SelectedDataSource;
                        if (MainWindowShown)
                            SetToolStripStatusLabel("Ready");
                        Application.DoEvents();
                    }
                    m_dm_Attributes.DataSourceId = SelectedDataSource.Id;
                    m_dm_SystemAttributes.SystemDataSourceId = (int)SelectedDataSource.SystemDataSourceId;
                    m_dm_Property.DataSource = SelectedDataSource;
                    SetToolStripStatusLabel("Ready");
                }
            }
            dockPanel.ResumeLayout(true, true);            
            this.Cursor = Cursors.Default;
            sreListView.ListView.Focus();
        }

        #endregion DataSource Events


        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(DockPanelGlobalAttributes).ToString())
                return m_dm_GlobalAttributes;
            else if (persistString == typeof(DockPanelDataSources).ToString())
                return m_dm_DataSources;
            else if (persistString == typeof(DockPanelProperty).ToString())
                return m_dm_Property;
            else if (persistString == typeof(DockPanelDataSourceAttributes).ToString())
                return m_dm_Attributes;
            else if (persistString == typeof(DockPanelMapper).ToString())
                return m_dm_Mapper;

            else if (persistString == typeof(DockPanelDataSourceSystemAttributes).ToString())
                return m_dm_SystemAttributes;          
            else
            {               
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

            }

            return m_dm_DataSources;
        }

        public void SetToolStripStatusLabel(string text, bool redFont = false, bool clear = false)
        {
            Invoke((MethodInvoker)delegate
                {
                    timerClearStatus.Enabled = clear;
                    toolStripStatusLabel1.Text = text;
                    toolStripStatusLabel1.ForeColor = redFont ? Color.DarkRed : Color.Black;
                });

            Application.DoEvents();
        }

        private void timerClearStatus_Tick(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
               {
                   timerClearStatus.Enabled = false;
                   toolStripStatusLabel1.ForeColor = Color.Black;
                   toolStripStatusLabel1.Text = "Ready";
               });
            Application.DoEvents();
        }

        private void miOnComplete_Click(object sender, EventArgs e)
        {
            GlobalEventsOnComplete gec = new GlobalEventsOnComplete();
            gec.ShowDialog();
        }

        private void mnItmlistOfValues_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ListOfValues listOfValues = new ListOfValues();
            listOfValues.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
      

        private void createNewRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> sdfFiles = new List<string>(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.sdf"));
            if (sdfFiles.Count == 0)
            {
                MessageBox.Show("No SDF file found!", "Maintenance - Create New Repository", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                InputBox iBox = new InputBox("new_sre", "New repository name", "New Repository", this.Icon);
                if (iBox.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(iBox.TheInput))
                    {
                        MessageBox.Show("Repository name cannot be blank!", "Maintenance - Create New Repository", 
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                string newFile = sdfFiles[0].Replace(Path.GetFileNameWithoutExtension(sdfFiles[0]), iBox.TheInput);
                if (File.Exists(newFile))
                {
                    if (MessageBox.Show("new_sre.sdf file already exists! Do you want to overwrite it?", "Maintenance - Create New Repository",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        new FileUtility().Delete(newFile);
                    else
                        return;
                }

                new Eyedia.Core.FileUtility().FileCopy(sdfFiles[0], newFile, false);
                string cleanScripts = string.Empty;

                using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Interface.EmbeddedResources.CleanTables.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    cleanScripts = reader.ReadToEnd();
                }

                string newcs = string.Format("Data Source='{0}';password=acc3s$", newFile);
                List<string> scripts = new List<string>(cleanScripts.Split(Environment.NewLine.ToCharArray())).RemoveEmptyStrings();

                SqlClientManager sqlClientManager = new SqlClientManager(newcs, SreKeyTypes.ConnectionStringSqlCe);
                foreach (string script in scripts)
                {
                    sqlClientManager.ExecuteNonQuery(script);
                }

                DataAccessLayer.CompactSqlCe(newcs);
                if (MessageBox.Show(newFile + " created successfully! Do you want to point to new repository?", "Maintenance - Create New Repository",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    new Manager(newcs).InsertSystemObjects();
                    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.ConnectionStrings.ConnectionStrings["cs"].ConnectionString = newcs;
                    config.Save(ConfigurationSaveMode.Modified);

                    string sre = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe");
                    string srewoa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpewoa.exe");

                    if (File.Exists(sre))
                    {
                        System.Configuration.Configuration sreConfig = ConfigurationManager.OpenExeConfiguration(sre);
                        config.ConnectionStrings.ConnectionStrings["cs"].ConnectionString = newcs;
                        config.Save(ConfigurationSaveMode.Modified);
                    }

                    if (File.Exists(srewoa))
                    {
                        System.Configuration.Configuration srewoaConfig = ConfigurationManager.OpenExeConfiguration(srewoa);
                        config.ConnectionStrings.ConnectionStrings["cs"].ConnectionString = newcs;
                        config.Save(ConfigurationSaveMode.Modified);
                    }

                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);
                }
            }
        }

        private void globalSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_dm_DataSources.globalSearchWidget1.Visible = globalSearchToolStripMenuItem.Checked;
        }

        private void envionmentManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnvironmentWindow envManager = new EnvironmentWindow();
            envManager.ShowDialog();
        }

        private void ensureSystemObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will check if system data sources, attributes and required rules exist, the process will regenerate these objects if required. Are you sure?",
              "System Objects", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {                    
                    new Manager().InsertSystemObjects();
                    m_dm_DataSources.RefreshData();
                    MessageBox.Show("Success!", "System Objects", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System Objects - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
             
        }

        private void runTestbedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string traceFileName = EnvironmentFiles.StartTestBed();
            if (string.IsNullOrEmpty(traceFileName))
                MessageBox.Show("Cannot start test bed, please contact support or reinstall IDPE!",
                    "IDPE - Test Bed", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            this.Cursor = Cursors.Default;
        }

        public void runTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (m_dm_Property.propertyGrid.SelectedObject is SreDataSourceProperty)
            {
                SreDataSourceProperty dsProp = m_dm_Property.propertyGrid.SelectedObject as SreDataSourceProperty;
                if (string.IsNullOrEmpty(dsProp.TestFileName))
                {
                    MessageBox.Show("Cannot start test bed, please set a test file on property window!",
                        "IDPE - Test Bed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    {                        
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }

                if (!File.Exists(dsProp.TestFileName))
                {
                    MessageBox.Show("Cannot start test bed, " + dsProp.TestFileName + "  does not exist!",
                        "IDPE - Test Bed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    {                       
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                string traceFileName = string.Empty;
                bool useWcf = false; //todo
                if (useWcf)
                {
                    traceFileName = EnvironmentFiles.StartTestBed(true);
                    SreEnvironment local = EnvironmentServiceDispatcherFactory.GetEnvironmentLocal();
                    string result = EnvironmentServiceDispatcherFactory.GetInstance(true).ProcessFile(local.EnvironmentConfigsInstancesOnly[0],
                        dsProp.DataSource.Id, dsProp.TestFileName);
                }
                else
                {
                    string pullFolder = DataSource.GetPullFolder(dsProp.DataSource.Id, dsProp.DataSourceKeys);
                    traceFileName = EnvironmentFiles.ProcessTestFile(pullFolder, dsProp.TestFileName);
                }

                if (!string.IsNullOrEmpty(traceFileName))
                {
                    System.Threading.Thread.Sleep(1000);//service needs time to start
                    if (Application.OpenForms.OfType<Log>().Count() == 1)
                    {
                        Application.OpenForms.OfType<Log>().First().Focus();
                    }
                    else
                    {
                        Log log = new Log("IDPE - Test Bed", traceFileName);
                        log.Show();
                    }
        
                }
                else
                {
                    MessageBox.Show("Cannot start test bed, please contact support or reinstall IDPE!",
                        "IDPE - Test Bed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            this.Cursor = Cursors.Default;
        }
    }
}


