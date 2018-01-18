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
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Eyedia.IDPE.Services;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Clients;

namespace Eyedia.IDPE.Interface
{
    public partial class DockPanelProperty : DockContent
    {
        public DockPanelProperty()
        {
            //data source color - #AE3C00
            InitializeComponent();
            this.TabText = "Property";
            this.Text = TabText;            
        }

        IdpeAttribute _Attribute;
        public IdpeAttribute Attribute
        {
            get { return _Attribute; }
            set
            {
                _Attribute = value;
                //btnSave.Visible = _Attribute.AttributeId == 0 ? true : false;

                SetTitle(_Attribute.AttributeId, _Attribute.Name);
                errorProvider1.Clear();
                propertyGrid.SelectedObject = new SreAttributeProperty(_Attribute, DataSourceId, IsAssociatedWithSystemDataSource);
                if ((IsAssociatedWithSystemDataSource == false) && (DataSourceId != 0))
                    this.picIcon.Image = global::Eyedia.IDPE.Interface.Properties.Resources.attributeds;
                else if ((IsAssociatedWithSystemDataSource == true) && (DataSourceId != 0))
                    this.picIcon.Image = global::Eyedia.IDPE.Interface.Properties.Resources.attributesds;
                else
                    this.picIcon.Image = global::Eyedia.IDPE.Interface.Properties.Resources.attribute;
                SetTitle(_Attribute.AttributeId, _Attribute.Name);
                ToggleDataSource(false);

            }
        }

        public DockPanelGlobalAttributes DockPanelGlobalAttributes
        {
            get
            {
                foreach (var c in this.DockPanel.Contents)
                {
                    if (c is DockPanelGlobalAttributes) return c as DockPanelGlobalAttributes;
                }
                return null;
            }
        }

        public DockPanelDataSources DockPanelDataSources
        {
            get
            {
                foreach (var c in this.DockPanel.Contents)
                {
                    if (c is DockPanelDataSources) return c as DockPanelDataSources;
                }
                return null;
            }
        }
        public MainWindow MainWindow { get; set; }
       
        public int DataSourceId {get; set; }
        public bool IsAssociatedWithSystemDataSource { get; set; }

        IdpeDataSource _SreDataSource;
        public IdpeDataSource DataSource
        {
            get { return _SreDataSource; }
            set
            {
                _SreDataSource = value;
                //btnSave.Visible = _SreDataSource.Id == 0? true: false;             

                idpeRulesEditorControl1.DataSource = _SreDataSource;
                idpeKeys1.DataSourceId = _SreDataSource.Id;
                idpeKeys1.DataSourceName = _SreDataSource.Name;
                sreEmails1.DataSourceId = _SreDataSource.Id;
                sreDatabases1.DataSourceId = _SreDataSource.Id;
                errorProvider1.Clear();
                propertyGrid.SelectedObject = new SreDataSourceProperty(_SreDataSource);
                this.picIcon.Image = global::Eyedia.IDPE.Interface.Properties.Resources.DataSource;
                SetTitle(_SreDataSource.Id, _SreDataSource.Name);                
                ToggleDataSource();
                DoWithSqlPuller();
            }
        }

        private void DoWithSqlPuller()
        {

            if ((_SreDataSource.DataFeederType != null)
               && ((DataFeederTypes)_SreDataSource.DataFeederType == DataFeederTypes.PullSql))
            {
                tsbSqlStopRequest.Visible = true;
                string currentStatus = "0";
                try
                {
                    currentStatus = new IdpeClient().IsTemporarilyStopped(_SreDataSource.Id);
                    if(currentStatus == "1")
                    {
                        timerBlank.Enabled = true;
                        tsbSqlStopRequest.ToolTipText = "Request Start";
                    }
                    else
                    {
                        timerBlank.Enabled = false;
                        tsbSqlStopRequest.ToolTipText = "Request Stop";
                    }
                }
                catch
                {

                }
            }
            else
            {
                tsbSqlStopRequest.Visible = false;
            }
           
        }

        private void SetTitle(int id, string name)
        {
            this.TabText = name;
            this.Text = TabText;
            this.lblCaption.Text = string.Format("{0} - {1}",id, name);
        }

        public void ToggleDataSource(bool showDataSource = true)
        {
            tabControl1.SuspendLayout();
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage1);
            if (!showDataSource)
            {
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
                tabControl1.TabPages.Remove(tabPage6);
                
                toolStrip1.Items[1].Visible = false;
                toolStrip1.Items[2].Visible = true;
                toolStrip1.Items[3].Visible = false;
                toolStrip1.Items[4].Visible = false;
                toolStrip1.Items[5].Visible = false;
            }
            else
            {
                tabControl1.TabPages.Add(tabPage2);
                tabControl1.TabPages.Add(tabPage3);
                tabControl1.TabPages.Add(tabPage4);
                tabControl1.TabPages.Add(tabPage5);
                tabControl1.TabPages.Add(tabPage6);
                
                toolStrip1.Items[1].Visible = true;
                toolStrip1.Items[2].Visible = true;
                toolStrip1.Items[3].Visible = true;
                toolStrip1.Items[4].Visible = true;
                toolStrip1.Items[5].Visible = true;
            }
            tabControl1.ResumeLayout(true);
            if(DataSource != null)
                globalSearchWidget1.DataSourceId = DataSource.Id;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (propertyGrid.SelectedObject != null)
            {
                ((SrePropertyGrid)propertyGrid.SelectedObject).HasChanged = true;

                int id = 0;
                SreAttributeProperty aProp = null;
                SreDataSourceProperty dsProp = null;
                if (propertyGrid.SelectedObject is SreAttributeProperty)
                {
                    aProp = ((SreAttributeProperty)propertyGrid.SelectedObject);
                    id = aProp.Id;
                }
                else if (propertyGrid.SelectedObject is SreDataSourceProperty)
                {
                    dsProp = ((SreDataSourceProperty)propertyGrid.SelectedObject);
                    id = dsProp.Id;
                }

                if (id != 0)
                    errorProvider1.SetError(picIcon, "Property has been changed, will be auto saved when the property if unloaded! Click save button to save now.");
                else if (dsProp != null)
                    ValidateNewDataSource(dsProp);

            }
        }

        private void picIcon_Click(object sender, EventArgs e)
        {
            if (propertyGrid.SelectedObject != null)
            {
                ((SrePropertyGrid)propertyGrid.SelectedObject).HasChanged = false;
                //((SrePropertyGrid)propertyGrid.SelectedObject).RevertChanges();
                //errorProvider1.Clear();
            }
        }
        

        private void ValidateNewDataSource(SreDataSourceProperty dsProp)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(dsProp.Name))
                errorMessage += "Name field is mandatory, it has to be unique!" + Environment.NewLine;
            if (dsProp.SystemDataSourceId == 0)
                errorMessage += "Select system name of the data source!" + Environment.NewLine;
            if (!string.IsNullOrEmpty(dsProp.ValidationError))
                errorMessage += dsProp.ValidationError;

            errorProvider1.Clear();
            errorProvider1.SetError(picIcon, errorMessage);

        }

        public void SelectTabs(int tabIndex, RuleSetTypes ruleType = RuleSetTypes.PreValidate)
        {
            switch (tabIndex)
            {
                case 0:
                    tabControl1.SelectedTab = tabPage1;
                    break;
                case 1:
                    tabControl1.SelectedTab = tabPage2;
                    idpeRulesEditorControl1.SelectTabs(ruleType);
                    break;
                case 2:
                    tabControl1.SelectedTab = tabPage3;
                    break;
                case 3:
                    tabControl1.SelectedTab = tabPage4;
                    break;
                case 4:
                    tabControl1.SelectedTab = tabPage5;
                    break;
            }
        }
      

        private void tsbShowDataSource_Click(object sender, EventArgs e)
        {
            if (_SreDataSource != null)
                DataSource = _SreDataSource;
        }

        private void tsbClearCache_Click(object sender, EventArgs e)
        {
            MainWindow.SetToolStripStatusLabel("Please wait...Clearing cache...");
            Application.DoEvents();
            if (ServiceCommunicator.ClearDataSource(_SreDataSource.Id, string.Empty))
                MainWindow.SetToolStripStatusLabel("Ready");
            else
                MainWindow.SetToolStripStatusLabel("Failed to clear cache", true);
            this.Cursor = Cursors.Default;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            switch (tabControl1.SelectedIndex)
            {
                case 2: SreDataSourceProperty.KeepVersion(this.DataSource.Id); this.idpeKeys1.Save(); break;
                case 3: SreDataSourceProperty.KeepVersion(this.DataSource.Id); this.sreEmails1.Save(); break;
                case 4: SreDataSourceProperty.KeepVersion(this.DataSource.Id); this.sreDatabases1.Save(); break;
            }

            if (propertyGrid.SelectedObject is SreAttributeProperty)
            {
                SreAttributeProperty aProp = propertyGrid.SelectedObject as SreAttributeProperty;
                if ((aProp != null) && (aProp.HasChanged == true))
                {
                    MainWindow.SetToolStripStatusLabel("Please wait...Saving...");
                    Application.DoEvents();
                    aProp.Save();
                    aProp.HasChanged = false;
                    errorProvider1.Clear();
                    DockPanelGlobalAttributes.RefreshData(aProp.Attribute.AttributeId);                    
                }
            }
            else if (propertyGrid.SelectedObject is SreDataSourceProperty)
            {
                SreDataSourceProperty dsProp = propertyGrid.SelectedObject as SreDataSourceProperty;

                if ((dsProp != null) && (dsProp.HasChanged == true))
                {
                    string errorMsg = errorProvider1.GetError(picIcon);
                    if ((errorMsg.StartsWith("Property has been changed, will be auto saved"))
                     || (string.IsNullOrEmpty(errorProvider1.GetError(picIcon))))
                    {
                        MainWindow.SetToolStripStatusLabel("Please wait...Saving...");
                        Application.DoEvents();
                        dsProp.Save();
                        dsProp.HasChanged = false;
                        errorProvider1.Clear();
                        DockPanelDataSources.RefreshData(dsProp.DataSource.Id);
                    }
                }
            }

            tsbClearCache_Click(sender, e);
        }

        private void tsbSqlStopRequest_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                IdpeClient client = new IdpeClient();
                string currentStatus = client.IsTemporarilyStopped(_SreDataSource.Id);
                if (currentStatus == "0")
                {
                    client.StopSqlPuller(_SreDataSource.Id);
                    timerBlank.Enabled = true;
                    tsbSqlStopRequest.ToolTipText = "Request Start";
                }
                else
                {
                    client.StartSqlPuller(_SreDataSource.Id);
                    tsbSqlStopRequest.Image = tsbSqlStopRequestImage;
                    timerBlank.Enabled = false;
                    tsbSqlStopRequest.ToolTipText = "Request Stop";
                }
            }
            catch { }
            this.Cursor = Cursors.Default;
        }

        Image tsbSqlStopRequestImage;
        private void timerBlank_Tick(object sender, EventArgs e)
        {
            if (tsbSqlStopRequest.Image == null)
            {
                tsbSqlStopRequest.Image = tsbSqlStopRequestImage;
            }
            else
            {
                tsbSqlStopRequestImage = tsbSqlStopRequest.Image;
                tsbSqlStopRequest.Image = null;
            }
        }

        private void tbPrint_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            frmReports printDs = new frmReports(frmReports.ReportTypes.AttributesParentChild, _SreDataSource.Id);
            printDs.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void tsbTest_Click(object sender, EventArgs e)
        {
            MainWindow.runTestToolStripMenuItem_Click(sender, e);
        }
    }
}


