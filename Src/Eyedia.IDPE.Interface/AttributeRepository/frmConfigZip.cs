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
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Services;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Windows.Utilities;
using System.Windows.Forms.Design;

namespace Eyedia.IDPE.Interface
{
    public partial class frmConfigZip : Form
    {
        public int DataSourceId { get; private set; }
        public DataSource DataSource { get; private set; }
        public IWindowsFormsEditorService _WindowsFormsEditorService;

        public frmConfigZip(int dataSourceId, IWindowsFormsEditorService windowsFormsEditorService = null)
        {
            InitializeComponent();
            _WindowsFormsEditorService = windowsFormsEditorService;
            if (_WindowsFormsEditorService != null)
                TopLevel = false;

            ;
            this.DataSourceId = dataSourceId;
            Init();
            BindData();
        }

        DataFormatTypes SelectedDataFormatType
        {
            get
            {
                if (string.IsNullOrEmpty(cmbDataFormatTypes.Text))
                    return DataFormatTypes.Delimited;
                else
                    return (DataFormatTypes)Enum.Parse(typeof(DataFormatTypes), cmbDataFormatTypes.Text);

            }
        }

        private void Init()
        {
            RefreshDataSource();
            Array dataFormatTypes = Enum.GetValues(typeof(DataFormatTypes));
            foreach (DataFormatTypes dataFormatType in Enum.GetValues(typeof(DataFormatTypes)))
            {
                if ((dataFormatType != DataFormatTypes.Sql)
                    && (dataFormatType != DataFormatTypes.Zipped))
                    cmbDataFormatTypes.Items.Add(dataFormatType);
            }

            cmbDelmiter.Items.Add(",");
            cmbDelmiter.Items.Add("|");
            cmbDelmiter.Items.Add("Tab");

            //pnlCustomInterface.Top = lblDelimiter.Top - 7;
            pnlZipHandlerCustom.Location = pnlZipHandlerDefault.Location;
        }

        private void BindData()
        {
             SreKey key = DataSource.Keys.GetKey(SreKeyTypes.ZipFilesSortType.ToString());
             if (key != null)
             {
                 if (key.Value == "-1")
                 {
                     chkZipSort.Checked = false;
                 }
                 else if (key.Value == "0")
                 {
                     chkZipSort.Checked = true;
                     cmbZipSortType.SelectedIndex = 0;
                 }
                 else if (key.Value == "1")
                 {
                     chkZipSort.Checked = true;
                     cmbZipSortType.SelectedIndex = 1;
                 }
                 else
                 {
                     chkZipSort.Checked = true;
                     radZipSortCstm.Checked = true;
                     txtSortCustom.Text = key.Value;
                 }
             }

            key = DataSource.Keys.GetKey(SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder.ToString());
            if (key != null)
                chkZipDoNotCreateAcknoledgementInOutputFolder.Checked = key.Value.ParseBool();

            key = DataSource.Keys.GetKey(SreKeyTypes.ZipInterfaceName.ToString());
            if (key != null)
            {
                txtInterfaceNameZip.Text = key.Value;
                radZipHandlerCustom.Checked = true;
            }
            else
            {
                radZipHandlerDefault.Checked = true;
            }

            key = DataSource.Keys.GetKey(SreKeyTypes.ZipIgnoreFileList.ToString());
            if (key != null)
            {
                txtZipIgnoreFileList.Text = key.Value;
                if (!string.IsNullOrEmpty(txtZipIgnoreFileList.Text))
                    chkZipIgnoreFiles.Checked = true;
            }


            key = DataSource.Keys.GetKey(SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder.ToString());
            if (key != null)
            {
                txtZipIgnoreFileListButCopy.Text = key.Value;
                if (!string.IsNullOrEmpty(txtZipIgnoreFileListButCopy.Text))
                    chkZipIgnoreFilesButCopy.Checked = true;
            }

            key = DataSource.Keys.GetKey(SreKeyTypes.ZipDataFileDataFormatType.ToString());
            DataFormatTypes formatType = DataFormatTypes.Delimited;
            if (key != null)
            {
                int intDataFormat = (int)key.Value.ParseInt();
                formatType = (DataFormatTypes)intDataFormat;                
            }
            cmbDataFormatTypes.Text = formatType.ToString();

            cmbDelmiter.Text = DataSource.Delimiter == "\t" ? "Tab" : DataSource.Delimiter;

            key = DataSource.Keys.GetKey(SreKeyTypes.IsFirstRowHeader.ToString());
            if (key != null)
                chkFileHasHeader.Checked = key.Value.ParseBool();

            key = DataSource.Keys.GetKey(SreKeyTypes.RenameColumnHeader.ToString());
            if (key != null)
                chkRenameHeaders.Checked = key.Value.ParseBool();

            key = DataSource.Keys.GetKey(SreKeyTypes.FileInterfaceName.ToString());
            if (key != null)
                txtFileInterfaceName.Text = key.Value;
          

        }

        private void RefreshDataSource()
        {
            Cache.Instance.Bag.Remove(DataSourceId);
            DataSource = new Services.DataSource(DataSourceId, string.Empty);
        }

        private void ZipHandlerChanged(object sender, EventArgs e)
        {
            if (radZipHandlerDefault.Checked)
            {
                pnlZipHandlerDefault.Visible = true;
                pnlZipHandlerCustom.Visible = false;
            }
            else
            {
                pnlZipHandlerDefault.Visible = false;
                pnlZipHandlerCustom.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == false)
                return;

            Manager manager = new Manager();
            SreKey key = new SreKey();

            ////////////////////////////
            key.Name = SreKeyTypes.ZipFilesSortType.ToString();
            key.Type = (int)SreKeyTypes.ZipFilesSortType;
            if (chkZipSort.Checked)
            {
                if (radZipSortStd.Checked)
                {
                    key.Value = cmbZipSortType.SelectedIndex.ToString();
                }
                else
                {
                    key.Value = txtSortCustom.Text;
                }

                manager.Save(key, DataSource.Id);
            }
            else
            {
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.ZipFilesSortType.ToString(), true);
            }


            ////////////////////////////
            key = new SreKey();
            key.Name = SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder.ToString();
            key.Type = (int)SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder;
            if (chkZipDoNotCreateAcknoledgementInOutputFolder.Checked)
            {
                key.Value = "1";
                manager.Save(key, DataSource.Id);
            }
            else
            {
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.ZipDoNotCreateAcknoledgementInOutputFolder.ToString(), true);
            }

            ////////////////////////////

            if (radZipHandlerDefault.Checked)
            {
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.ZipInterfaceName.ToString(), true);

                if (chkZipIgnoreFilesButCopy.Checked)
                {
                    key = new SreKey();
                    key.Name = SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder.ToString();
                    key.Type = (int)SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder;
                    key.Value = txtZipIgnoreFileListButCopy.Text;
                    manager.Save(key, DataSource.Id);
                }
                else
                {
                    manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder.ToString(), true);
                }

                if (chkZipIgnoreFiles.Checked)
                {
                    key = new SreKey();
                    key.Name = SreKeyTypes.ZipIgnoreFileList.ToString();
                    key.Type = (int)SreKeyTypes.ZipIgnoreFileList;
                    key.Value = txtZipIgnoreFileList.Text;
                    manager.Save(key, DataSource.Id);
                }
                else
                {
                    manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.ZipIgnoreFileList.ToString(), true);
                }

            }
            else
            {
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.ZipIgnoreFileListButCopyToOutputFolder.ToString(), true);
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.ZipIgnoreFileList.ToString(), true);

                key = new SreKey();
                key.Name = SreKeyTypes.ZipInterfaceName.ToString();
                key.Type = (int)SreKeyTypes.ZipInterfaceName;
                key.Value = txtInterfaceNameZip.Text;
                manager.Save(key, DataSource.Id);
            }

            ////////////////////////////
            key = new SreKey();
            key.Name = SreKeyTypes.ZipDataFileDataFormatType.ToString();
            key.Type = (int)SreKeyTypes.ZipDataFileDataFormatType;
            key.Value = ((int)(DataFormatTypes)Enum.Parse(typeof(DataFormatTypes), cmbDataFormatTypes.Text)).ToString();
            manager.Save(key, DataSource.Id);


            ////////////////////////////
            if (chkFileHasHeader.Checked)
            {
                key = new SreKey();
                key.Name = SreKeyTypes.IsFirstRowHeader.ToString();
                key.Type = (int)SreKeyTypes.IsFirstRowHeader;
                key.Value = "1";
                manager.Save(key, DataSource.Id);
            }
            else
            {
                manager.DeleteKeyFromApplication(DataSource.Id, SreKeyTypes.IsFirstRowHeader.ToString(), true);
            }

            ////////////////////////////
            if (chkRenameHeaders.Checked)
            {
                key = new SreKey();
                key.Name = SreKeyTypes.RenameColumnHeader.ToString();
                key.Value = "True";
                key.Type = (int)SreKeyTypes.RenameColumnHeader;
                manager.Save(key, this.DataSource.Id);
            }
            else
            {
                manager.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.RenameColumnHeader.ToString(), true);
            }
            ////////////////////////////
            if (SelectedDataFormatType == DataFormatTypes.Delimited)
            {
                manager.UpdateDelimiter(DataSource.Id, cmbDelmiter.Text.ToLower() == "tab" ? "\t" : cmbDelmiter.Text);
            }
            else if (SelectedDataFormatType == DataFormatTypes.Custom)
            {
                key = new SreKey();
                key.Name = SreKeyTypes.FileInterfaceName.ToString();
                key.Value = txtFileInterfaceName.Text;
                key.Type = (int)SreKeyTypes.FileInterfaceName;
                manager.Save(key, DataSource.Id);
            }
            else
            {
                manager.DeleteKeyFromApplication(this.DataSource.Id, SreKeyTypes.FileInterfaceName.ToString(), true);
            }

            this.Close();
        }

        private bool ValidateData()
        {
            bool allGood = true;
            if ((radZipSortCstm.Checked) && (string.IsNullOrEmpty(txtSortCustom.Text)))
            {
                errorProvider1.SetError(txtSortCustom, "Can not be blank!");
                allGood = false;
            }
            if (radZipHandlerDefault.Checked)
            {
                if ((chkZipIgnoreFilesButCopy.Checked) && (string.IsNullOrEmpty(txtZipIgnoreFileListButCopy.Text)))
                {
                    errorProvider1.SetError(txtZipIgnoreFileListButCopy, "Can not be blank!");
                    allGood = false;
                }

                if ((chkZipIgnoreFiles.Checked) && (string.IsNullOrEmpty(txtZipIgnoreFileList.Text)))
                {
                    errorProvider1.SetError(txtZipIgnoreFileList, "Can not be blank!");
                    allGood = false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtInterfaceNameZip.Text))
                {                    
                    errorProvider1.SetError(txtInterfaceNameZip, "Can not be blank!");
                    allGood = false;
                }
            }

            if ((pnlCustomInterface.Visible) && (string.IsNullOrEmpty(txtFileInterfaceName.Text)))
            {
                errorProvider1.SetError(txtFileInterfaceName, "Can not be blank!");
                allGood = false;
            }

            return allGood;
        }

        private void cmbDataFormatTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SelectedDataFormatType)
            {
                case DataFormatTypes.Delimited:
                    lblDelimiter.Visible = true;
                    cmbDelmiter.Visible = true;
                    btnConfigureDataFormat.Visible = true;
                    chkFileHasHeader.Visible = true;
                    chkRenameHeaders.Visible = true;
                    pnlCustomInterface.Visible = false;
                    break;
                case DataFormatTypes.FixedLength:                
                    lblDelimiter.Visible = false;
                    cmbDelmiter.Visible = false;
                    chkFileHasHeader.Visible = false;
                    chkRenameHeaders.Visible = false;
                    btnConfigureDataFormat.Visible = true;
                    pnlCustomInterface.Visible = false;
                    break;

                case DataFormatTypes.SpreadSheet:
                    lblDelimiter.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = false;
                    chkFileHasHeader.Visible = true;
                    chkRenameHeaders.Visible = true;
                    pnlCustomInterface.Visible = false;
                    break;

                case DataFormatTypes.Xml:
                    lblDelimiter.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = true;
                    chkFileHasHeader.Visible = false;
                    chkRenameHeaders.Visible = false;
                    pnlCustomInterface.Visible = false;
                    break;
               
                case DataFormatTypes.EDIX12:
                    lblDelimiter.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = true;
                    chkFileHasHeader.Visible = false;
                    chkRenameHeaders.Visible = false;
                    pnlCustomInterface.Visible = false;
                    break;

                case DataFormatTypes.Custom:
                    lblDelimiter.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = false;
                    chkFileHasHeader.Visible = true;
                    chkRenameHeaders.Visible = false;
                    pnlCustomInterface.Visible = true;
                    SreKey key = DataSource.Keys.GetKey(SreKeyTypes.FileInterfaceName.ToString());
                    if (key != null)
                        txtFileInterfaceName.Text = key.Value;
                    break;

                default:
                    lblDelimiter.Visible = false;
                    cmbDelmiter.Visible = false;
                    btnConfigureDataFormat.Visible = false;
                    chkFileHasHeader.Visible = false;
                    break;
            }
        }

        private void chkFileHasHeader_CheckedChanged(object sender, EventArgs e)
        {
            chkRenameHeaders.Enabled = chkFileHasHeader.Checked;
        }

        private void btnConfigureDataFormat_Click(object sender, EventArgs e)
        {
            switch (SelectedDataFormatType)
            {
                case DataFormatTypes.Delimited:
                case DataFormatTypes.FixedLength:                    
                    frmConfig2 config = new frmConfig2(DataSource.Id, DataSource.Name, SelectedDataFormatType);
                    if (config.ShowDialog() == DialogResult.OK)
                        RefreshDataSource();
                    break;

                case DataFormatTypes.Xml:                    
                    this.Cursor = Cursors.WaitCursor;
                    frmConfigXml configXml = new frmConfigXml(DataSource.Id);
                    this.Cursor = Cursors.Default;
                    if (configXml.ShowDialog() == DialogResult.OK)
                        RefreshDataSource();
                    break;

                case DataFormatTypes.EDIX12:                    
                    frmConfigEDI ediConfig = new frmConfigEDI(this.DataSource.Id);
                    ediConfig.ShowDialog();
                    break;
                   
            }
        }

        private void chkZipIgnoreFilesButCopy_CheckedChanged(object sender, EventArgs e)
        {
            txtZipIgnoreFileListButCopy.Visible = chkZipIgnoreFilesButCopy.Checked;
            txtZipIgnoreFileListButCopy.Focus();
        }

        private void chkZipIgnoreFiles_CheckedChanged(object sender, EventArgs e)
        {
            txtZipIgnoreFileList.Visible = chkZipIgnoreFiles.Checked;
            txtZipIgnoreFileList.Focus();
        }

        private void chkZipSort_CheckedChanged(object sender, EventArgs e)
        {
            grpZipSort.Visible = chkZipSort.Checked;
            if (grpZipSort.Visible)
            {
                if (radZipSortStd.Checked)
                    cmbZipSortType.SelectedIndex = 0;
            }
        }

        private void zipSortTypeChanged(object sender, EventArgs e)
        {
            if (radZipSortStd.Checked)
            {
                cmbZipSortType.Enabled = true;
                txtSortCustom.Enabled = false;
            }
            else
            {
                cmbZipSortType.Enabled = false;
                txtSortCustom.Enabled = true;
                txtSortCustom.Focus();
            }
        }

        private void btnInterface_Click(object sender, EventArgs e)
        {
            try
            {
                TypeSelectorDialog activitySelector = new TypeSelectorDialog(typeof(ZipFileWatcher));
                if ((activitySelector.ShowDialog() == DialogResult.OK) && (!String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null))
                    txtInterfaceNameZip.Text = string.Format("{0}, {1}", activitySelector.Activity.FullName, Path.GetFileNameWithoutExtension(activitySelector.AssemblyPath));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInterface2_Click(object sender, EventArgs e)
        {
            try
            {
                TypeSelectorDialog activitySelector = new TypeSelectorDialog(typeof(InputFileGenerator));
                if ((activitySelector.ShowDialog() == DialogResult.OK) && (!String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null))
                    txtFileInterfaceName.Text = string.Format("{0}, {1}", activitySelector.Activity.FullName, Path.GetFileNameWithoutExtension(activitySelector.AssemblyPath));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmConfigZip_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_WindowsFormsEditorService != null)
                _WindowsFormsEditorService.CloseDropDown();
        }
      
    }
}


