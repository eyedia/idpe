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
using Eyedia.IDPE.Common;
using System.Reflection;
using System.IO;
using Eyedia.IDPE.Services;
using Eyedia.Core.Data;
using System.Diagnostics;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Windows.Utilities;
using System.Net;
using System.Windows.Forms.Design;

namespace Eyedia.IDPE.Interface
{
    public partial class frmConfig : Form
    {        
        bool _clickedFromDataFeeder;
        int _DataSourceId;
        DataFeederTypes _dataFeederType;
        DataFormatTypes _dataFormatType;
        public frmConfig(bool clickedFromDataFeeder, int applicationId, DataFeederTypes dataFeederType, DataFormatTypes dataFormatType)
        {            
            _clickedFromDataFeeder = clickedFromDataFeeder;
            _DataSourceId = applicationId;
            _dataFeederType = dataFeederType;
            _dataFormatType = dataFormatType;         

            InitializeComponent();           

            grpLocalFileSystem.Location = grpFTPFileSystem.Location;        
            grpCustom.Location = grpFTPFileSystem.Location;
            

            if (_clickedFromDataFeeder)
            {
                switch (_dataFeederType)
                {                   
                    case DataFeederTypes.PullFtp:
                        btnSave.Enabled = true;
                        this.Text = "Configure FTP";
                        grpFTPFileSystem.Visible = true;
                        grpLocalFileSystem.Visible = false;                     
                        grpCustom.Visible = false;
                        this.Height = grpFTPFileSystem.Height + btnSave.Height + toolStripStatusLabel1.Height + 40;
                        this.Width = grpFTPFileSystem.Width + 18;
                        this.btnSave.Top = grpFTPFileSystem.Top + grpFTPFileSystem.Height + 5;
                        this.btnCancel.Top = grpFTPFileSystem.Top + grpFTPFileSystem.Height + 5;
                        break;
                    case DataFeederTypes.PullLocalFileSystem:
                        btnSave.Enabled = true;
                        this.Text = "Configure Local File System";
                        grpFTPFileSystem.Visible = false;
                        grpLocalFileSystem.Visible = true;                    
                        grpCustom.Visible = false;

                        this.Height = grpLocalFileSystem.Height + btnSave.Height + toolStripStatusLabel1.Height + 40;
                        this.Width = grpLocalFileSystem.Width + 18;
                        this.btnSave.Top = grpLocalFileSystem.Top + grpLocalFileSystem.Height + 5;
                        this.btnCancel.Top = grpLocalFileSystem.Top + grpLocalFileSystem.Height + 5;
                        break;
                
                    default:
                        grpFTPFileSystem.Visible = false;
                        grpLocalFileSystem.Visible = false;                       
                        btnSave.Enabled = false;
                        break;
                }
            }
            else
            {
                SetDataFormatUI();
            }
           

            
            cbFilter1.Items.Add("All(No Filter)");
            cbFilter1.Items.Add(".txt");
            cbFilter1.Items.Add(".csv");
            cbFilter1.Items.Add(".txt|.csv");
            cbFilter1.Items.Add(".dat");
            cbFilter1.Items.Add(".zip");
            cbFilter1.Items.Add(".rar");
            cbFilter1.Items.Add(".tar");
            cbFilter1.Items.Add(".zip|.rar|.tar");


            cbFilter2.Items.Add("All(No Filter)");
            cbFilter2.Items.Add(".txt");
            cbFilter2.Items.Add(".csv");
            cbFilter2.Items.Add(".txt|.csv");
            cbFilter2.Items.Add(".dat");
            cbFilter2.Items.Add(".zip");
            cbFilter2.Items.Add(".rar");
            cbFilter1.Items.Add(".tar");
            cbFilter2.Items.Add(".zip|.rar|.tar");

            filterTypeErrorProvider.SetIconAlignment(this.cbFilter1, ErrorIconAlignment.TopLeft);
            filterTypeErrorProvider.SetIconAlignment(this.cbFilter2, ErrorIconAlignment.TopLeft);

         
            BindingList<Eyedia.IDPE.Interface.Data.Attribute> allAttributes = new Data.UtilDataManager().GetAttributes(this._DataSourceId);
            if (allAttributes != null)
            {
                List<string> attributeNames = allAttributes.Select(a => a.Name).ToList();
                AcceptableAttributeNames = string.Join(",", attributeNames.ToArray());
            }
        }

        void SetDataFormatUI()
        {
            switch (_dataFormatType)
            {               

                case DataFormatTypes.Custom:
                    btnSave.Enabled = true;
                    this.Text = "Configure Custom Interface";
                    grpFTPFileSystem.Visible = false;
                    grpLocalFileSystem.Visible = false;                
                    grpFTPFileSystem.Visible = false;
                    grpCustom.Visible = true;
                    chkFirstRowIsHeader2.Visible = true;
              
                    this.Height = grpCustom.Height + btnSave.Height + toolStripStatusLabel1.Height + 40;
                    this.Width = grpCustom.Width + 18;
                    this.btnSave.Top = grpCustom.Top + grpCustom.Height + 5;
                    this.btnCancel.Top = grpCustom.Top + grpCustom.Height + 5;
                    break;

                default:
                    btnSave.Enabled = true;
                    this.Text = "Configure Local File System";
                    grpFTPFileSystem.Visible = false;
                    grpLocalFileSystem.Visible = true;                
                    grpCustom.Visible = false;
                    this.Height = grpLocalFileSystem.Height + btnSave.Height + toolStripStatusLabel1.Height + 40;
                    this.Width = grpLocalFileSystem.Width + 18;
                    this.btnSave.Top = grpLocalFileSystem.Top + grpLocalFileSystem.Height + 5;
                    this.btnCancel.Top = grpLocalFileSystem.Top + grpLocalFileSystem.Height + 5;
                    break;
            }
        }

        public DataFeederTypes DataFeederType
        {
            get { return _dataFeederType; }
        }

        public string FtpRemoteLocation
        {
            get { return txtFtpRemoteLocation.Text; }
            set { txtFtpRemoteLocation.Text = value; }
        }

        public string FtpLocalLocation
        {
            get { return string.Format(SreConfigurationSection.CurrentConfig.LocalFileWatcher.DirectoryPull, _DataSourceId); }
        }

        public string FtpUserName
        {
            get { return txtFtpUserName.Text; }
            set { txtFtpUserName.Text = value; }
        }

        public string FtpPassword
        {
            get { return txtFtpPassword.Text; }
            set { txtFtpPassword.Text = value; }
        }

        public string WatchFilter
        {
            get 
            { 
                if(DataFeederType == DataFeederTypes.PullFtp)
                    return cbFilter1.Text;
                return cbFilter2.Text;
            }
            set 
            {
                if (DataFeederType == DataFeederTypes.PullFtp)
                    cbFilter1.Text = value;
                else
                    cbFilter2.Text = value;
            }
        }

        public int Interval
        {
            get { return int.Parse(numUpDnInterval.Value.ToString()); }
            set { numUpDnInterval.Value = value; }
        }       
       

        public string InterfaceName
        {
            get {
                        return txtInterfaceNameGeneric.Text;
                }
            
            set 
            {                
                txtInterfaceNameGeneric.Text = value;
                toolTip.SetToolTip(txtInterfaceNameGeneric, value); 
            }
        }


        public bool IsFirstRowIsHeader
        {
            get
            {
                return chkFirstRowIsHeader2.Checked;
            }
            set
            {

                chkFirstRowIsHeader2.Checked = value;
            }
        }
      
        public bool LocalFileSystemFoldersOverriden
        {
            get { return radUseSpecificPull.Checked; }
            set { radUseSpecificPull.Checked = value; }
        }

        public bool LocalFileSystemFolderArchiveAuto
        {
            get { return chkAutoArchive.Checked; }
            set
            {                
                chkAutoArchive.Checked = value;
                if (!LocalFileSystemFoldersOverriden)
                    chkAutoArchive.Checked = true;
                chkAutoArchive_CheckedChanged(null, null);

            }
        }

        public string LocalFileSystemFolderPullFolder
        {
            get { return txtSpecificPullFolder.Text; }
            set { txtSpecificPullFolder.Text = value; }
        }

        public string LocalFileSystemFolderArchiveFolder
        {
            get { return txtSpecificArchFolder.Text; }
            set { txtSpecificArchFolder.Text = value; }
        }

        public string LocalFileSystemFolderOutputFolder
        {
            get { return txtSpecificOutFolder.Text; }
            set { txtSpecificOutFolder.Text = value; }
        }

        string AcceptableAttributeNames { get; set; }
        bool DoNotClose;

        private void btnSave_Click(object sender, EventArgs e)
        {
            DoNotClose = false;
            if (_clickedFromDataFeeder)
            {
                switch (_dataFeederType)
                {
                    case DataFeederTypes.PullLocalFileSystem:
                      
                        string filterText = cbFilter2.Text;
                        if (filterText == "All(No Filter)")
                            filterText = string.Empty;

                        new Manager().SaveLocalFSConfig(_DataSourceId, filterText,LocalFileSystemFoldersOverriden,
                            LocalFileSystemFolderArchiveAuto,LocalFileSystemFolderPullFolder, LocalFileSystemFolderArchiveFolder, LocalFileSystemFolderOutputFolder);
                        break;

                    case DataFeederTypes.PullFtp:
                         filterText = cbFilter1.Text;
                        if (filterText == "All(No Filter)")
                            filterText = string.Empty;
                        new Manager().SaveFTPConfig(_DataSourceId, FtpRemoteLocation, FtpLocalLocation,
                                FtpUserName, FtpPassword, Interval, filterText);
                        break;
                }
            }
            else
            {
                switch (_dataFormatType)
                {
                    case DataFormatTypes.Xml:
                        new Manager().SaveXmlConfig(_DataSourceId, InterfaceName);
                        break;
                  
                    case DataFormatTypes.Custom:
                        new Manager().SaveCustomConfig(_DataSourceId, InterfaceName, chkFirstRowIsHeader2.Checked);
                        break;

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DoNotClose = false;
            this.Close();
        }

        void ValidateFilter(object sender, EventArgs e)
        {
            string errMsg = "Please include only extension like .txt, for multiple filters use |(pipe) sign, like .txt|.dat";
            if (cbFilter1.Text.Contains("*"))
            {                
                filterTypeErrorProvider.SetError(this.cbFilter1, errMsg);
                toolStripStatusLabel1.Text = errMsg;
                btnSave.Enabled = false;
            }
            else if (cbFilter2.Text.Contains("*"))
            {
                filterTypeErrorProvider.SetError(this.cbFilter2, errMsg);
                toolStripStatusLabel1.Text = errMsg;
                btnSave.Enabled = false;
            }
            else
            {
                filterTypeErrorProvider.SetError(this.cbFilter1, String.Empty);
                filterTypeErrorProvider.SetError(this.cbFilter1, String.Empty);
                toolStripStatusLabel1.Text = "Ready";
                btnSave.Enabled = true;
            }

        }
     
        

        private void btnInterface_Click(object sender, EventArgs e)
        {
            try
            {
                TypeSelectorDialog activitySelector = new TypeSelectorDialog(typeof(InputFileGenerator));

                if ((activitySelector.ShowDialog() == DialogResult.OK) && (!String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null))
                {
                    txtInterfaceNameGeneric.Text = string.Format("{0}, {1}", activitySelector.Activity.FullName, Path.GetFileNameWithoutExtension(activitySelector.AssemblyPath));                                        
                    toolStripStatusLabel1.Text = "";

                }
               
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.ToString();
                toolTip.SetToolTip(statusStrip1, ex.ToString());
            }
        }

        private void frmConfigFtp_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = DoNotClose;
        }

        private void txtInterfaceNameGeneric_TextChanged(object sender, EventArgs e)
        {
            InterfaceName = txtInterfaceNameGeneric.Text;
        }

        private void LocalPullTypeChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = radUseSpecificPull.Checked;
        }       

        private void chkAutoArchive_CheckedChanged(object sender, EventArgs e)
        {
            txtSpecificArchFolder.Enabled = !chkAutoArchive.Checked;
            if (chkAutoArchive.Checked)
            {
                txtSpecificArchFolder.Enabled = false;
                txtSpecificArchFolder.Text = "";
                btnFolderBrowser2.Enabled = false;
            }
            else
            {
                txtSpecificArchFolder.Enabled = true;                
                btnFolderBrowser2.Enabled = true;
            }
        }

        private void btnFolderBrowser1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtSpecificPullFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnFolderBrowser2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtSpecificArchFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnFolderBrowser3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtSpecificOutFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnFtpTest_Click(object sender, EventArgs e)
        {
            try
            {                
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(new Uri(txtFtpRemoteLocation.Text));
                Request.Credentials = new NetworkCredential(txtFtpUserName.Text, txtFtpPassword.Text);                
                Request.Method = WebRequestMethods.Ftp.ListDirectory;
                Request.Proxy = null;
                Request.UseBinary = true;
                FtpWebResponse Response = (FtpWebResponse)Request.GetResponse();              
                MessageBox.Show("Successful!", "FTP Connectivity Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Failed!", "FTP Connectivity Test", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }       
      
    }
}





