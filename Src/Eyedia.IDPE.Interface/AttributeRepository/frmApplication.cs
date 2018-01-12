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
using System.Reflection;
using System.Windows.Forms;
using Symplus.RuleEngine.Common;
using Symplus.RuleEngine.Services;
using Symplus.RuleEngine.DataManager;
using Symplus.RuleEngine.Utilities.Data;
using Symplus.Core;
using Symplus.Core.Data;
using Symplus.Core.Windows.Utilities;


namespace Symplus.RuleEngine.Utilities
{
    internal partial class frmApplication : Form
    {
        UtilDataManager _UtilDataManager;
        Manager _Manager;
        const string NOTSET = "<not set>";
        const string DEFAULTSTRING = "<default>";
        internal SreDataSource DataSource;
        internal List<SreKey> DataSourceKeys;
        bool Initialized;

        internal frmApplication(SreDataSource application)
        {
            InitializeComponent();
            _UtilDataManager = new UtilDataManager();
            _Manager = new Manager();
            this.DataSource = application;
            this.DataSourceKeys = _Manager.GetKeys(this.DataSource.Id);

            Array dataFeederTypes = Enum.GetValues(typeof(DataFeederTypes));
            cmbDataFeederType.Items.Clear();

            foreach (DataFeederTypes dataFeederType in dataFeederTypes)
            {
                if (dataFeederType == DataFeederTypes.Push)
                    cmbDataFeederType.Items.Add("Push (SRE as Web Service)");
                else
                    cmbDataFeederType.Items.Add(dataFeederType);
            }

            if (this.DataSource.IsSystem == true)
            {
                tabControl1.Visible = false;
                this.Height = this.Height - tabControl1.Height;
                this.Text = "Parent Datasource";
                lblParent.Visible = false;
                cbParentApplications.Visible = false;
                btnKeyEditorParent.Visible = true;
            }
        }

        public frmApplication()
        {
            this.DataSource = new SreDataSource();
        }

        bool RenameColumnHeader;
        private void frmApplication_Load(object sender, EventArgs e)
        {
            cbParentApplications.DataSource = _UtilDataManager.GetAllExternalSystems(true);
            cbParentApplications.DisplayMember = "ExternalSystemName";

            if (DataSource.Id == 0)
            {
                tabControl1.Enabled = false;
            }

            if (cbParentApplications.Items.Count > 0)
                cbParentApplications.SelectedIndex = 0;

            if (DataSource.DataFeederType == null)
                DataSource.DataFeederType = (int)DataFeederTypes.Push;

            cmbDelmiter.Items.Add(",");
            cmbDelmiter.Items.Add("|");
            cmbDelmiter.Items.Add("Tab");

            cmbOutputFileExtension.Items.Add(".txt");
            cmbOutputFileExtension.Items.Add(".csv");
            cmbOutputFileExtension.Items.Add(".xml");

            if (DataSource != null)
                BindData();
        }

        private void BindData()
        {
            chkIsSystem.Enabled = DataSource.Id == 0 ? true : false;
            SetTextData(txtName, this.DataSource.Name);
            SetTextData(txtDescription, this.DataSource.Description);
            chkIsSystem.Checked = this.DataSource.IsSystem == null ? false : (bool)this.DataSource.IsSystem;
            if((this.DataSource.Delimiter != null)
                && (this.DataSource.Delimiter.ToLower() == "\t"))
                SetTextData(cmbDelmiter, "Tab");
            else
                SetTextData(cmbDelmiter, this.DataSource.Delimiter);

            SetTextData(txtDataContainerValidatorType, this.DataSource.DataContainerValidatorType);
            if (!string.IsNullOrEmpty(txtDataContainerValidatorType.Text))
                toolTip.SetToolTip(txtDataContainerValidatorType, "Press delete to set it to default");

            SetTextData(txtOutputWriterType, this.DataSource.OutputWriterType);
            if (!string.IsNullOrEmpty(txtOutputWriterType.Text))
                toolTip.SetToolTip(txtOutputWriterType, "Press delete to set it to default");

            SetTextData(txtPlugInsType, this.DataSource.PlugInsType);
            if (!string.IsNullOrEmpty(txtPlugInsType.Text))
                toolTip.SetToolTip(txtPlugInsType, "Press delete to set it to default");

            SetTextData(txtPusherType, this.DataSource.PusherType);
            if (!string.IsNullOrEmpty(txtPusherType.Text))
                toolTip.SetToolTip(txtPusherType, "Press delete to set it to default");

            if (DataSource.SystemDataSourceId != 0)
            {
                for (int i = 0; i < cbParentApplications.Items.Count; i++)
                {
                    if (((ExternalSystem)cbParentApplications.Items[i]).ExternalSystemId == DataSource.SystemDataSourceId)
                    {
                        cbParentApplications.SelectedIndex = i;
                        break;
                    }
                }
            }

            DataFeederTypes feederType = (DataFeederTypes)DataSource.DataFeederType;
            if(feederType == DataFeederTypes.Push)
                cmbDataFeederType.Text = "Push (SRE as Web Service)";
            else
                cmbDataFeederType.Text = feederType.ToString();

            if (DataSource.DataFormatType == null)
            {
                cmbDataFormatTypes.Text = DataFormatTypes.Delimited.ToString();
            }
            else
            {
                DataFormatTypes formatType = (DataFormatTypes)DataSource.DataFormatType;
                cmbDataFormatTypes.Text = formatType.ToString();
            }

            string strHeader = DataSourceKeys.GetKeyValue(SreKeyTypes.IsFirstRowHeader);
            if (!string.IsNullOrEmpty(strHeader))
            {
                bool boolVal = false;
                bool.TryParse(strHeader, out boolVal);
                chkFileHasHeader.Checked = boolVal;
            }

            string strRenCol = DataSourceKeys.GetKeyValue(SreKeyTypes.RenameColumnHeader);
            if (!string.IsNullOrEmpty(strRenCol))
            {
                bool boolVal = false;
                bool.TryParse(strRenCol, out boolVal);
                chkRenameHeaders.Checked = boolVal;
                RenameColumnHeader = boolVal;
            }

            string outputFileExt = DataSourceKeys.GetKeyValue(SreKeyTypes.OutputFileExtension);
            if (!string.IsNullOrEmpty(outputFileExt))
                cmbOutputFileExtension.Text = outputFileExt;
            else
                cmbOutputFileExtension.Text = ".xml";

            chkwcfCallsGenerateStandardOutput.Checked = DataSourceKeys.GetKeyValue(SreKeyTypes.WcfCallsGenerateStandardOutput).ParseBool();
            
            BindRules();
            Initialized = true;
        }
        bool succeeded = false;
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DataSource.Name = GetTextData(txtName);
            this.DataSource.Description = GetTextData(txtDescription);
            this.DataSource.IsSystem = chkIsSystem.Checked;
            if (!chkIsSystem.Checked)
            {
                this.DataSource.Delimiter = GetTextData(cmbDelmiter);
                if (this.DataSource.Delimiter.ToLower() == "tab")
                    this.DataSource.Delimiter = "\t";
                this.DataSource.DataContainerValidatorType = GetTextData(txtDataContainerValidatorType);
                this.DataSource.OutputWriterType = GetTextData(txtOutputWriterType);
                this.DataSource.PlugInsType = GetTextData(txtPlugInsType);
                this.DataSource.PusherType = GetTextData(txtPusherType);
                this.DataSource.SystemDataSourceId = ((ExternalSystem)cbParentApplications.SelectedItem).ExternalSystemId;
                this.DataSource.DataFeederType = (int)SelectedDataFeederType;
                if (SelectedDataFeederType == DataFeederTypes.PullSql)
                    this.DataSource.DataFormatType = (int)DataFormatTypes.Sql;
                else
                    this.DataSource.DataFormatType = (int)SelectedDataFormatType;
            }

            List<SreRuleDataSource> ruleSets = GetRuleSetDetails();

            try
            {
                Manager dm = new Manager();
                DataSource.Id = dm.Save(this.DataSource, ruleSets);
                succeeded = true;

                if (DataSource.IsSystem == false)
                {
                    if ((SelectedDataFormatType == DataFormatTypes.Delimited)
                        || (SelectedDataFormatType == DataFormatTypes.Excel))
                    {
                        SreKey key = new SreKey();
                        key.Name = SreKeyTypes.IsFirstRowHeader.ToString();
                        key.Value = chkFileHasHeader.Checked.ToString();
                        key.Type = (int)SreKeyTypes.IsFirstRowHeader;
                        dm.Save(this.DataSource.Id, key);

                    }

                    if (chkRenameHeaders.Checked)
                    {
                        SreKey key = new SreKey();
                        key.Name = SreKeyTypes.RenameColumnHeader.ToString();
                        key.Value = "True";
                        key.Type = (int)SreKeyTypes.RenameColumnHeader;
                        dm.Save(this.DataSource.Id, key);
                    }
                    else
                    {
                        dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.RenameColumnHeader.ToString(), true);
                    }

                    if (cmbOutputFileExtension.Text.ToLower() != ".xml")
                    {
                        SreKey key = new SreKey();
                        key.Name = SreKeyTypes.OutputFileExtension.ToString();
                        key.Value = cmbOutputFileExtension.Text;
                        key.Type = (int)SreKeyTypes.OutputFileExtension;
                        dm.Save(this.DataSource.Id, key);
                    }
                    else
                    {
                        dm.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.OutputFileExtension.ToString(), true);
                    }
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
            if (succeeded)
            {

                new Manager().SavePushConfig(this.DataSource.Id, chkwcfCallsGenerateStandardOutput.Checked);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            succeeded = true;
            this.Close();
        }

        string textBoxTypes1 = "txtDelmiter,txtDataContainerValidatorType,txtOutputWriterType,txtPlugInsType,txtPusherType";
        string textBoxTypes2 = "txtPreParse,txtPostParse,txtFinally,txtFinallyContainer";

        private string GetDefaultString(TextBox textBox)
        {
            if (textBoxTypes1.Contains(textBox.Name))
                return DEFAULTSTRING;
            else
                return NOTSET;
        }

        private void SetTextData(ComboBox cmbBox, string data)
        {
            cmbBox.Text = data;
        }

        private void SetTextData(TextBox textBox, string data)
        {
            //if (string.IsNullOrEmpty(data))
            //{
            //    textBox.Text = GetDefaultString(textBox);
            //    textBox.ForeColor = Color.DarkGray;
            //}
            //else
            //{
            textBox.Text = data;
            textBox.ForeColor = Color.Black;
            //}
            textBox.SelectionStart = textBox.Text.Length;
        }

        private string GetTextData(ComboBox cmBox)
        {            
            return cmBox.Text;
        }

        private string GetTextData(TextBox textBox)
        {
            if ((string.IsNullOrEmpty(textBox.Text)) || (textBox.Text == NOTSET) || (textBox.Text == DEFAULTSTRING))
                return null;
            else
                return textBox.Text;
        }

        private bool IsDataDirty()
        {

            if (cmbDataFormatTypes.Text.Contains(" - (Not Allowed)"))
                return false;

            if (string.IsNullOrEmpty(this.DataSource.Name))    //in case of new app
                return true;

            if ((GetTextData(txtName) != this.DataSource.Name))
                return true;

            if ((GetTextData(txtDescription) != this.DataSource.Description))
                return true;

            if ((GetTextData(cmbDelmiter) != this.DataSource.Delimiter))
                return true;

            if ((GetTextData(txtDataContainerValidatorType) != this.DataSource.DataContainerValidatorType))
                return true;

            if ((GetTextData(txtOutputWriterType) != this.DataSource.OutputWriterType))
                return true;

            if ((GetTextData(txtPlugInsType) != this.DataSource.PlugInsType))
                return true;

            if ((GetTextData(txtPusherType) != this.DataSource.PusherType))
                return true;

            if (this.DataSource.IsSystem != chkIsSystem.Checked)
                return true;

            if (this.RenameColumnHeader != chkRenameHeaders.Checked)
                return true;

            if (cmbOutputFileExtension.Text != ".xml")
                return true;

            DataFeederTypes feederType = DataFeederTypes.PullLocalFileSystem;
            if (cmbDataFeederType.Text.Contains("Push"))
                feederType = DataFeederTypes.Push;
            else
                feederType = (DataFeederTypes)Enum.Parse(typeof(DataFeederTypes), cmbDataFeederType.Text);

            if ((int)this.DataSource.DataFeederType != (int)feederType)
                return true;

            if (DataSourceKeys.GetKeyValue(SreKeyTypes.WcfCallsGenerateStandardOutput).ParseBool() != chkwcfCallsGenerateStandardOutput.Checked)
                return true;

            SreDataSource parentDs = _Manager.GetApplicationDetails((int)DataSource.SystemDataSourceId);
            if (cbParentApplications.Text != parentDs.Name)
                return true;

            if (!string.IsNullOrEmpty(cmbDataFormatTypes.Text))
            {

                DataFormatTypes formatType = (DataFormatTypes)Enum.Parse(typeof(DataFormatTypes), cmbDataFormatTypes.Text);
                if (this.DataSource.DataFormatType == null)
                {
                    if (cmbDataFormatTypes.Text != DataFormatTypes.Delimited.ToString())
                        return true;
                }
                else if ((int)this.DataSource.DataFormatType != (int)formatType)
                {
                    return true;
                }
                else if ((formatType == DataFormatTypes.Delimited) || (formatType == DataFormatTypes.Excel))
                {
                    return CheckIfHeaderInfoIsChanged();
                }
            }

            if (feederType == DataFeederTypes.PullSql)
                return CheckIfHeaderInfoIsChanged();

            return false;
        }

        bool CheckIfHeaderInfoIsChanged()
        {

            string isFWHeader = this.DataSourceKeys.GetKeyValue(SreKeyTypes.IsFirstRowHeader);
            if (!string.IsNullOrEmpty(isFWHeader))
            {
                bool boolVal = false;
                bool.TryParse(isFWHeader, out boolVal);
                if (chkFileHasHeader.Checked != boolVal)
                    return true;
            }
            else
            {
                if (chkFileHasHeader.Checked == true)
                    return true;
                else
                    return false;
            }
            return false;
        }

        private void CheckDirty(object sender, EventArgs e)
        {
            if (!Initialized) return;

            btnSave.Enabled = IsDataDirty();
            if ((chkIsSystem.Checked == false) 
                && ((cbParentApplications.Text == "") || (txtName.Text == "")))
            {
                btnSave.Enabled = false;
            }
            else if ((DataSource.Id == 0) && (_Manager.ApplicationExists(txtName.Text)))
            {
                toolStripStatusLabel1.Text = "Application already exists";
                btnSave.Enabled = false;
            }
            else if (btnSave.Enabled)
            {
                toolStripStatusLabel1.Text = "Ready";
            }

            if (sender is TextBox)
            {
                TextBox tbox = sender as TextBox;
                tbox.ForeColor = Color.Black;
                if ((tbox.Text.Contains(NOTSET)) && (tbox.Text.Length != NOTSET.Length))
                {
                    tbox.Text = tbox.Text.Replace(NOTSET, "");
                    tbox.SelectionStart = tbox.Text.Length;
                }
                else if ((tbox.Text.Contains(DEFAULTSTRING)) && (tbox.Text.Length != DEFAULTSTRING.Length))
                {
                    tbox.Text = tbox.Text.Replace(DEFAULTSTRING, "");
                    tbox.SelectionStart = tbox.Text.Length;
                }
                else if ((tbox.Text.Contains(NOTSET)) && (tbox.Text.Length == NOTSET.Length))
                {
                    tbox.ForeColor = Color.DarkGray;
                }
                else if ((tbox.Text.Contains(DEFAULTSTRING)) && (tbox.Text.Length == DEFAULTSTRING.Length))
                {
                    tbox.ForeColor = Color.DarkGray;
                }
            }
        }

        private void CheckEmptyTextBox(object sender, EventArgs e)
        {
            TextBox tbox = sender as TextBox;
            if (string.IsNullOrEmpty(tbox.Text))
            {
                tbox.Text = GetDefaultString(tbox);
                tbox.ForeColor = Color.DarkGray;
                tbox.SelectionStart = tbox.Text.Length;
            }


        }



        private void CheckCancel(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Escape) && (btnSave.Enabled))
            {
                if (sender is TextBox)
                {
                    TextBox tbox = sender as TextBox;
                    if (tbox == txtName)
                        SetTextData(tbox, this.DataSource.Name);
                    else if (tbox == txtDescription)
                        SetTextData(tbox, this.DataSource.Description);
                    else if (tbox == txtDataContainerValidatorType)
                        SetTextData(tbox, this.DataSource.DataContainerValidatorType);
                    else if (tbox == txtOutputWriterType)
                        SetTextData(tbox, this.DataSource.OutputWriterType);
                    else if (tbox == txtPlugInsType)
                        SetTextData(tbox, this.DataSource.PlugInsType);
                    else if (tbox == txtPusherType)
                        SetTextData(tbox, this.DataSource.PusherType);
                }
                else if (sender is ComboBox)
                {
                    ComboBox cBox = sender as ComboBox;
                    if (cBox == cmbDelmiter)
                        SetTextData(cBox, this.DataSource.Delimiter);
                }


                CheckDirty(sender, e);
            }
            else if ((e.KeyCode == Keys.Escape) && (!(btnSave.Enabled)))
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (sender is TextBox)
                {
                    TextBox tbox = sender as TextBox;
                    if ((tbox.Name == txtDataContainerValidatorType.Name)
                        || (tbox.Name == txtOutputWriterType.Name)
                        || (tbox.Name == txtPlugInsType.Name)
                        || (tbox.Name == txtPusherType.Name))
                        tbox.Text = "";
                }
            }
        }

        private void SelectAllText(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void SelectAllText(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void frmApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!succeeded)
                e.Cancel = true;
        }

        private void chkIsSystem_CheckedChanged(object sender, EventArgs e)
        {
            cbParentApplications.Enabled = !chkIsSystem.Checked;
            cmbDelmiter.Enabled = !chkIsSystem.Checked;
            txtDataContainerValidatorType.Enabled = !chkIsSystem.Checked;
            txtOutputWriterType.Enabled = !chkIsSystem.Checked;
            txtPlugInsType.Enabled = !chkIsSystem.Checked;
            txtPusherType.Enabled = !chkIsSystem.Checked;
            CheckDirty(sender, e);
        }


        private void SelectTypes_Click(object sender, EventArgs e)
        {
            
            Control ctlSender = sender as Control;
            if (ctlSender.Name == btnSetContainerValidator.Name)
            {
                TypeSelectorDialog typeSelectorDialog = new TypeSelectorDialog(typeof(DataContainerValidator));

                DialogResult dResult = typeSelectorDialog.ShowDialog();
                if ((dResult == DialogResult.OK) && (!String.IsNullOrEmpty(typeSelectorDialog.AssemblyPath) && typeSelectorDialog.Activity != null))
                    txtDataContainerValidatorType.Text = string.Format("{0}, {1}", typeSelectorDialog.Activity.FullName, Path.GetFileNameWithoutExtension(typeSelectorDialog.AssemblyPath));

            }
            else if (ctlSender.Name == btnSetOutputWriter.Name)
            {
                TypeSelectorDialog typeSelectorDialog = new TypeSelectorDialog(typeof(OutputWriter));

                DialogResult dResult = typeSelectorDialog.ShowDialog();
                if ((dResult == DialogResult.OK) && (!String.IsNullOrEmpty(typeSelectorDialog.AssemblyPath) && typeSelectorDialog.Activity != null))
                    txtOutputWriterType.Text = string.Format("{0}, {1}", typeSelectorDialog.Activity.FullName, Path.GetFileNameWithoutExtension(typeSelectorDialog.AssemblyPath));

            }
            else if (ctlSender.Name == btnSetPlugIns.Name)
            {
                TypeSelectorDialog typeSelectorDialog = new TypeSelectorDialog(typeof(PlugIns));

                DialogResult dResult = typeSelectorDialog.ShowDialog();
                if ((dResult == DialogResult.OK) && (!String.IsNullOrEmpty(typeSelectorDialog.AssemblyPath) && typeSelectorDialog.Activity != null))
                    txtPlugInsType.Text = string.Format("{0}, {1}", typeSelectorDialog.Activity.FullName, Path.GetFileNameWithoutExtension(typeSelectorDialog.AssemblyPath));

            }
            else if (ctlSender.Name == btnSetPusher.Name)
            {
                TypeSelectorDialog typeSelectorDialog = new TypeSelectorDialog(typeof(Pushers));

                DialogResult dResult = typeSelectorDialog.ShowDialog();
                if ((dResult == DialogResult.OK) && (!String.IsNullOrEmpty(typeSelectorDialog.AssemblyPath) && typeSelectorDialog.Activity != null))
                    txtPusherType.Text = string.Format("{0}, {1}", typeSelectorDialog.Activity.FullName, Path.GetFileNameWithoutExtension(typeSelectorDialog.AssemblyPath));

            }

        }

        List<SreRuleDataSource> GetRuleSetDetails()
        {
            return new List<SreRuleDataSource>();

        }

        private void btnKeyEditor_Click(object sender, EventArgs e)
        {
            frmKeys frmKey = new frmKeys(DataSource.Id, DataSource.Name);
            frmKey.ShowDialog();
        }

        private void btnConfigureDataFeeder_Click(object sender, EventArgs e)
        {
            //DataFeederTypes feederType = (DataFeederTypes)Enum.Parse(typeof(DataFeederTypes), cmbDataFeederType.Text);
            DataFormatTypes selType = SelectedDataFormatType;
            if ((SelectedDataFormatType == DataFormatTypes.Xml)
                || (SelectedDataFormatType == DataFormatTypes.Zipped))
                selType = DataFormatTypes.Delimited;  //to prevent showing xml/zip UI when clicked from this button.

            frmConfig configUI = new frmConfig(true, DataSource.Id, SelectedDataFeederType, selType);
            SreKey key = GetKey(DataSourceKeys, SreKeyTypes.FtpRemoteLocation.ToString());
            configUI.FtpRemoteLocation = key != null ? key.Value : string.Empty;
            key = GetKey(DataSourceKeys, SreKeyTypes.LocalLocation.ToString());
            key = GetKey(DataSourceKeys, SreKeyTypes.FtpUserName.ToString());
            configUI.FtpUserName = key != null ? key.Value : string.Empty;
            key = GetKey(DataSourceKeys, SreKeyTypes.FtpPassword.ToString());
            configUI.FtpPassword = key != null ? key.Value : string.Empty;
            key = GetKey(DataSourceKeys, SreKeyTypes.FtpWatchInterval.ToString());
            configUI.Interval = int.Parse(key != null ? key.Value : "2");
            key = GetKey(DataSourceKeys, SreKeyTypes.WatchFilter.ToString());
            configUI.WatchFilter = key != null ? key.Value : "All(No Filter)";


            key = GetKey(DataSourceKeys, SreKeyTypes.PullSqlConnectionString.ToString());
            configUI.ConnectionString = key != null ? key.Value : string.Empty;

            if (key != null)
            {
                SreKeyTypes connectionStringType = (SreKeyTypes)key.Type;

                if (connectionStringType == SreKeyTypes.ConnectionStringDB2iSeries)
                    configUI.DatabaseType = DatabaseTypes.DB2iSeries;
                else if (connectionStringType == SreKeyTypes.ConnectionStringOracle)
                    configUI.DatabaseType = DatabaseTypes.Oracle;
                else if (connectionStringType == SreKeyTypes.ConnectionStringSQLCe)
                    configUI.DatabaseType = DatabaseTypes.SqlCe;
                else if ((connectionStringType == SreKeyTypes.ConnectionStringSQLExpress)
                    || (connectionStringType == SreKeyTypes.ConnectionStringSQLServer))
                    configUI.DatabaseType = DatabaseTypes.SqlServer;
                else
                    configUI.DatabaseType = DatabaseTypes.SqlCe;
            }

            key = GetKey(DataSourceKeys, SreKeyTypes.PullSqlReturnType.ToString());
            configUI.PullSqlReturnType = key != null ? key.Value : string.Empty;

            key = GetKey(DataSourceKeys, SreKeyTypes.PullSqlInterfaceName.ToString());
            if (key != null)
                configUI.InterfaceName = key.Value;

            if (string.IsNullOrEmpty(configUI.InterfaceName))
            {
                key = GetKey(DataSourceKeys, SreKeyTypes.FileInterfaceName.ToString());
                if (key != null)
                    configUI.InterfaceName = key.Value;
            }

            key = GetKey(DataSourceKeys, SreKeyTypes.SQLQuery.ToString());
            if (key != null)
                configUI.Query = key.Value;

            key = GetKey(DataSourceKeys, SreKeyTypes.IsFirstRowHeader.ToString());
            if (key != null)
                configUI.IsFirstRowIsHeader = bool.Parse(key.Value);


            key = GetKey(DataSourceKeys, SreKeyTypes.LocalFileSystemFoldersOverriden.ToString());
            configUI.LocalFileSystemFoldersOverriden = key != null ? bool.Parse(key.Value) : false;

            key = GetKey(DataSourceKeys, SreKeyTypes.LocalFileSystemFolderArchiveAuto.ToString());
            configUI.LocalFileSystemFolderArchiveAuto = key != null ? bool.Parse(key.Value) : false;

            key = GetKey(DataSourceKeys, SreKeyTypes.LocalFileSystemFolderPull.ToString());
            configUI.LocalFileSystemFolderPullFolder = key != null ? key.Value : string.Empty;

            key = GetKey(DataSourceKeys, SreKeyTypes.LocalFileSystemFolderArchive.ToString());
            configUI.LocalFileSystemFolderArchiveFolder = key != null ? key.Value : string.Empty;

            key = GetKey(DataSourceKeys, SreKeyTypes.LocalFileSystemFolderOutput.ToString());
            configUI.LocalFileSystemFolderOutputFolder = key != null ? key.Value : string.Empty;


            if (configUI.ShowDialog() == DialogResult.OK)
            {
                this.DataSourceKeys = _Manager.GetKeys(DataSource.Id);
            }
        }

        private SreKey GetKey(List<SreKey> keys, string key)
        {

            SreKey srekey = (from k in keys
                             where k.Name == key
                             select k).SingleOrDefault();

            return srekey;
        }

        private void cmbDataFeederType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataFeederTypes selected = DataFeederTypes.PullLocalFileSystem;
            if (cmbDataFeederType.Text.Contains("Push"))
                selected = DataFeederTypes.Push;
            else
                selected = (DataFeederTypes)Enum.Parse(typeof(DataFeederTypes), cmbDataFeederType.Text);

            switch (selected)
            {
                case DataFeederTypes.PullFtp:
                    chkwcfCallsGenerateStandardOutput.Visible = false;
                    btnConfigureDataFeeder.Enabled = true;
                    break;
                case DataFeederTypes.PullLocalFileSystem:
                    chkwcfCallsGenerateStandardOutput.Visible = false;
                    label13.Visible = true;
                    cmbDataFormatTypes.Visible = true;
                    btnConfigureDataFormat.Visible = true;
                    label3.Visible = true;
                    cmbDelmiter.Visible = true;
                    chkFileHasHeader.Visible = true;
                    btnConfigureDataFeeder.Enabled = true;                    
                    break;
                case DataFeederTypes.PullSql:
                    chkwcfCallsGenerateStandardOutput.Visible = false;
                    label13.Visible = false;
                    cmbDataFormatTypes.Visible = false;
                    btnConfigureDataFormat.Visible = false;
                    label3.Visible = false;
                    cmbDelmiter.Visible = false;
                    chkFileHasHeader.Visible = false;
                    cmbDataFormatTypes.Text = DataFormatTypes.Sql.ToString();
                    btnConfigureDataFeeder.Enabled = true;
                    break;
                default:
                    chkwcfCallsGenerateStandardOutput.Visible = true;
                    label13.Visible = true;
                    cmbDataFormatTypes.Visible = true;
                    btnConfigureDataFormat.Visible = true;
                    label3.Visible = true;
                    cmbDelmiter.Visible = true;
                    chkFileHasHeader.Visible = true;
                    btnConfigureDataFeeder.Enabled = false;                    
                    break;
            }
            PopulateDataFormat();
            CheckDirty(sender, e);
        }

        private void PopulateDataFormat()
        {
            Array dataFormatTypes = Enum.GetValues(typeof(DataFormatTypes));
            cmbDataFormatTypes.Items.Clear();

            if (SelectedDataFeederType == DataFeederTypes.PullSql)
            {
                foreach (DataFormatTypes dataFormatType in dataFormatTypes)
                {
                    if ((dataFormatType.ToString() == DataFormatTypes.Delimited.ToString())
                        || (dataFormatType.ToString() == DataFormatTypes.Custom.ToString()))
                        cmbDataFormatTypes.Items.Add(dataFormatType);
                }
            }
            else if (SelectedDataFeederType == DataFeederTypes.Push)
            {
                foreach (DataFormatTypes dataFormatType in dataFormatTypes)
                {
                    if ((dataFormatType.ToString() == DataFormatTypes.Delimited.ToString())
                        || (dataFormatType.ToString() == DataFormatTypes.FixedLength.ToString())
                        || (dataFormatType.ToString() == DataFormatTypes.Xml.ToString())
                        || (dataFormatType.ToString() == DataFormatTypes.Custom.ToString()))
                        cmbDataFormatTypes.Items.Add(dataFormatType);
                }
            }
            else
            {
                foreach (DataFormatTypes dataFormatType in dataFormatTypes)
                {
                    if (dataFormatType.ToString() != DataFormatTypes.Sql.ToString())                        
                        cmbDataFormatTypes.Items.Add(dataFormatType);
                }
            }

        }

        private void cmbDataFormatTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SelectedDataFormatType)
            {
                case DataFormatTypes.Delimited:
                    label3.Visible = true;
                    cmbDelmiter.Visible = true;
                    btnConfigureDataFormat.Visible = false;
                    chkFileHasHeader.Visible = true;
                    break;
                case DataFormatTypes.FixedLength:
                case DataFormatTypes.Sql:
                    label3.Visible = false;
                    cmbDelmiter.Visible = false;
                    chkFileHasHeader.Visible = false;
                    if (!cmbDataFormatTypes.Text.Contains(" - (Not Allowed)"))
                        btnConfigureDataFormat.Visible = true;
                    break;

                case DataFormatTypes.Excel:
                    label3.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = false;
                    chkFileHasHeader.Visible = true;
                    break;

                case DataFormatTypes.Xml:
                    label3.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = true;
                    chkFileHasHeader.Visible = false;
                    break;

                case DataFormatTypes.Zipped:
                    label3.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = true;
                    chkFileHasHeader.Visible = false;
                    chkRenameHeaders.Visible = false;
                    break;

                case DataFormatTypes.Custom:
                    label3.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = true;
                    chkFileHasHeader.Visible = false;
                    chkRenameHeaders.Visible = false;
                    break;

                default:
                    label3.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = false;
                    chkFileHasHeader.Visible = false;
                    break;
            }

            CheckDirty(sender, e);
        }

        DataFormatTypes SelectedDataFormatType
        {
            get
            {
                string selectedText = cmbDataFormatTypes.Text.Replace(" - (Not Allowed)", string.Empty);
                if (string.IsNullOrEmpty(selectedText))
                    return DataFormatTypes.Delimited;
                else
                    return (DataFormatTypes)Enum.Parse(typeof(DataFormatTypes), selectedText);

            }
        }

        DataFeederTypes SelectedDataFeederType
        {
            get
            {
                DataFeederTypes selected = DataFeederTypes.PullLocalFileSystem;
                if (cmbDataFeederType.Text.Contains("Push"))
                    selected = DataFeederTypes.Push;
                else
                    selected = (DataFeederTypes)Enum.Parse(typeof(DataFeederTypes), cmbDataFeederType.Text);
                return selected;
            }
        }

        private void btnConfigureDataFormat_Click(object sender, EventArgs e)
        {
            switch (SelectedDataFormatType)
            {
                case DataFormatTypes.FixedLength:
                    frmFixedLengthConfig fsConfig = new frmFixedLengthConfig(DataSource.Id, DataSource.Name, SelectedDataFormatType);
                    SreKey key = GetKey(DataSourceKeys, SreKeyTypes.FixedLengthSchema.ToString());
                    fsConfig.Schema = key != null ? key.Value : string.Empty;

                    key = GetKey(DataSourceKeys, SreKeyTypes.FixedLengthHeaderAttribute.ToString());
                    fsConfig.FixedLegnthHeaderAttribute = key != null ? key.Value : string.Empty;

                    key = GetKey(DataSourceKeys, SreKeyTypes.FixedLengthFooterAttribute.ToString());
                    fsConfig.FixedLegnthFooterAttribute = key != null ? key.Value : string.Empty;

                    if (fsConfig.ShowDialog() == DialogResult.OK)
                    {
                        this.DataSourceKeys = _Manager.GetKeys(DataSource.Id);
                    }
                    break;
                case DataFormatTypes.Xml:
                case DataFormatTypes.Zipped:
                    frmConfig configUI = new frmConfig(false, DataSource.Id, SelectedDataFeederType, SelectedDataFormatType);

                    key = GetKey(DataSourceKeys, SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder.ToString());
                    if (key != null)
                        configUI.ZipDoNotCreateAcknoledgementInOutputFolder = ((key.Value == "1") || (key.Value.ToLower() == "true")) ? true : false;

                    key = GetKey(DataSourceKeys, SreKeyTypes.FileInterfaceName.ToString());
                    if (key != null)
                        configUI.InterfaceName = key.Value;

                    if (string.IsNullOrEmpty(configUI.InterfaceName))
                    {
                        key = GetKey(DataSourceKeys, SreKeyTypes.ZipInterfaceName.ToString());
                        if (key != null)
                            configUI.InterfaceName = key.Value;
                    }

                    key = GetKey(DataSourceKeys, SreKeyTypes.ZipInterfaceName.ToString());
                    if (key != null)
                        configUI.InterfaceNameZip = key.Value;

                    key = GetKey(DataSourceKeys, SreKeyTypes.RedirectOutput.ToString());
                    if (key != null)
                        configUI.OverriddenOutputFolder = key.Value;

                    configUI.PusherType = DataSource.PusherType;


                    key = GetKey(DataSourceKeys, SreKeyTypes.ZipFileConfigType.ToString());
                    if (key != null)
                        configUI.ZipFileConfigType = int.Parse(key.Value);


                    key = GetKey(DataSourceKeys, SreKeyTypes.ZipFilesSortedStandard.ToString());
                    if (key != null)
                    {
                        configUI.ZipFilesSortedType = SreKeyTypes.ZipFilesSortedStandard;
                        configUI.ZipFilesSortedStandard = int.Parse(key.Value);
                    }
                    else
                    {
                        key = GetKey(DataSourceKeys, SreKeyTypes.ZipFilesSortedCustom.ToString());
                        if (key != null)
                        {
                            configUI.ZipFilesSortedType = SreKeyTypes.ZipFilesSortedCustom;
                            configUI.ZipFilesSortedCustom = key.Value;
                        }
                        else
                        {
                            configUI.ZipFilesSortedType = SreKeyTypes.ZipFilesSortedNone;
                        }
                    }

                    key = GetKey(DataSourceKeys, SreKeyTypes.ZipIgnoreFileList.ToString());
                    if (key != null)
                        configUI.ZipIgnoreTypeList = key.Value;


                    key = GetKey(DataSourceKeys, SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder.ToString());
                    if (key != null)
                        configUI.ZipIgnoreFileListButCopyToOutputFolder = key.Value;

                    key = GetKey(DataSourceKeys, SreKeyTypes.IsFirstRowHeader.ToString());
                    if (key != null)
                        configUI.IsFirstRowIsHeaderInZipFile = string.IsNullOrEmpty(key.Value) ? false : bool.Parse(key.Value);


                    if (configUI.ShowDialog() == DialogResult.OK)
                    {
                        this.DataSource = _Manager.GetApplicationDetails(this.DataSource.Id);
                        this.DataSourceKeys = _Manager.GetKeys(DataSource.Id);
                    }

                    break;

                case DataFormatTypes.Custom:
                    configUI = new frmConfig(false, DataSource.Id, SelectedDataFeederType, SelectedDataFormatType);
                    key = GetKey(DataSourceKeys, SreKeyTypes.FileInterfaceName.ToString());
                    if (key != null)
                        configUI.InterfaceName = key.Value;

                    key = GetKey(DataSourceKeys, SreKeyTypes.IsFirstRowHeader.ToString());
                    if (key != null)
                        configUI.IsFirstRowIsHeader = string.IsNullOrEmpty(key.Value) ? false : bool.Parse(key.Value);

                    if (configUI.ShowDialog() == DialogResult.OK)
                    {
                        this.DataSourceKeys = _Manager.GetKeys(DataSource.Id);
                    }
                    break;
            }
        }

        private void chkFileHasHeader_CheckedChanged(object sender, EventArgs e)
        {
            chkRenameHeaders.Enabled = chkFileHasHeader.Checked;
            CheckDirty(sender, e);
        }

        private void AssociateRules(object sender, EventArgs e)
        {
            BindableListView selLvw = null;
            RuleSetTypes ruleSetTypes = RuleSetTypes.Unknown;

            if (((Control)sender).Name == btnSetPreContainer.Name)
            {
                selLvw = lvwPreContainer;
                ruleSetTypes = RuleSetTypes.PreValidate;
            }
            else if (((Control)sender).Name == btnSetPreParse.Name)
            {
                selLvw = lvwPreParse;
                ruleSetTypes = RuleSetTypes.RowPreparing;
            }
            else if (((Control)sender).Name == btnSetPostParse.Name)
            {
                selLvw = lvwPostParse;
                ruleSetTypes = RuleSetTypes.RowPrepared;
            }
            else if (((Control)sender).Name == btnSetFinally.Name)
            {
                selLvw = lvwFinally;
                ruleSetTypes = RuleSetTypes.RowValidate;
            }
            else if (((Control)sender).Name == btnSetPostContainer.Name)
            {
                selLvw = lvwPostContainer;
                ruleSetTypes = RuleSetTypes.PostValidate;
            }

            ListViewItem item = null;
            List<String> doNotIncludeList = selLvw.Items.Cast<ListViewItem>().Select(i => i.Text).ToList();

            frmSelectRuleSet frmSelectRuleSet = new frmSelectRuleSet(doNotIncludeList);
            if (frmSelectRuleSet.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                item = new ListViewItem(frmSelectRuleSet.SelectedRule.Name);
                item.SubItems.Add(frmSelectRuleSet.SelectedRule.Description);
            }

            if (item == null)
                return;

            int priority = selLvw.Items.Count + 1;
            item.SubItems.Add(priority.ToString());
            selLvw.Items.Add(item);

            SreRuleDataSource sreRuleDataSource = new SreRuleDataSource();
            sreRuleDataSource.DataSourceId = DataSource.Id;
            sreRuleDataSource.RuleId = frmSelectRuleSet.SelectedRule.Id;
            sreRuleDataSource.Priority = priority;
            sreRuleDataSource.RuleSetType = (int)ruleSetTypes;
            new DataManager.Manager().Save(sreRuleDataSource);
        }



        private void BindRules()
        {
            List<SreRule> rules = new Manager().GetRules(this.DataSource.Id);
            lvwPreContainer.Items.Clear();
            lvwPreParse.Items.Clear();
            lvwPostParse.Items.Clear();
            lvwFinally.Items.Clear();
            lvwPostContainer.Items.Clear();

            foreach (SreRule rule in rules)
            {
                ListViewItem item = new ListViewItem(rule.Name);
                item.SubItems.Add(rule.Priority.ToString());
                item.Tag = rule;

                if (rule.RuleSetType == (int)RuleSetTypes.PreValidate)
                    lvwPreContainer.Items.Add(item);
                else if (rule.RuleSetType == (int)RuleSetTypes.RowPreparing)
                    lvwPreParse.Items.Add(item);
                else if (rule.RuleSetType == (int)RuleSetTypes.RowPrepared)
                    lvwPostParse.Items.Add(item);
                else if (rule.RuleSetType == (int)RuleSetTypes.RowValidate)
                    lvwFinally.Items.Add(item);
                else if (rule.RuleSetType == (int)RuleSetTypes.PostValidate)
                    lvwPostContainer.Items.Add(item);
            }
        }

        private void PriorityChanged(object sender, DragEventArgs e)
        {
            if (timerPriorityUpdater.Enabled == true)
                return;

            ShowStatusText("Updating...");
            selectedListView = sender as BindableListView;
            selectedListView.Enabled = false;
            timerPriorityUpdater.Enabled = true;
        }

        BindableListView selectedListView;

        private void timerPriorityUpdater_Tick(object sender, EventArgs e)
        {
            timerPriorityUpdater.Enabled = false;
            Manager mgr = new Manager();
            for (int i = 0; i < selectedListView.Items.Count; i++)
            {
                SreRuleDataSource sreRuleDataSource = new SreRuleDataSource();
                SreRule rule = selectedListView.Items[i].Tag as SreRule;
                if (rule == null)
                    rule = mgr.GetRule(selectedListView.Items[i].Text);

                sreRuleDataSource.DataSourceId = this.DataSource.Id;
                sreRuleDataSource.RuleId = rule.Id;
                sreRuleDataSource.Priority = i + 1;
                mgr.Save(sreRuleDataSource);
            }
            ShowStatusText("Updated");
            selectedListView.Enabled = true;
            selectedListView = null;
            BindRules();

        }

        void ShowStatusText(string text)
        {
            toolStripStatusLabel1.Text = text;
            Application.DoEvents();
            timerStatusText.Enabled = true;
        }

        private void timerStatusText_Tick(object sender, EventArgs e)
        {
            timerStatusText.Enabled = false;
            toolStripStatusLabel1.Text = "";
            Application.DoEvents();
        }

        private void Rules_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                BindableListView lvw = sender as BindableListView;
                if (lvw.SelectedItems.Count > 0)
                {

                    if (MessageBox.Show(string.Format("Are you sure you want to disassociate rule '{0}' from '{1}'? ", lvw.SelectedItems[0].Text, DataSource.Name),
                        "Disassociation of Rule", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        SreRule rule = lvw.SelectedItems[0].Tag as SreRule;
                        new Manager().DeleteRuleFromDataSource(DataSource.Id, rule.Id, false);
                        BindRules();
                    }
                }
            }
        }

        private void cmbOutputFileExtension_TextChanged(object sender, EventArgs e)
        {
            CheckDirty(sender, e);
        }
       
    }

}






