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
using System.Configuration;
using System.IO;
using Eyedia.Core;
using System.Collections;
using System.Diagnostics;
using System.ServiceProcess;
using System.Globalization;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using System.Threading;
using Eyedia.IDPE.DataManager;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using Eyedia.IDPE.Clients;

namespace Eyedia.IDPE.Interface
{
    public partial class Monitor : SREBaseFormNew
    {        
        string[] LogFiles;
        string LogFile;
        Image ServiceRunningImage;

        public Monitor()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            ServiceRunningImage = toolStripButton2.Image;       
            cbSearchOptions.SelectedIndex = 2;
            btnFind.Height = dateTimePicker1.Height + 5;
            _LogModifiedAt = DateTime.MinValue;
            ReadSearchHistory();         
            RefreshFiles();
            LoadLogFile();
        }

        private void RefreshFiles()
        {
            string exeName = "idpe.exe";
            if (ConfigurationManager.AppSettings["exeName"] != null)
                exeName = ConfigurationManager.AppSettings["exeName"];

            string confFileName = AppDomain.CurrentDomain.BaseDirectory + exeName;

            Configuration configuration = null;
            if (File.Exists(confFileName))
                configuration = ConfigurationManager.OpenExeConfiguration(confFileName);

            if (configuration == null)
                return;

            EyediaCoreConfigurationSection sreSection = (EyediaCoreConfigurationSection)configuration.GetSection("eyediaCoreConfigurationSection");

            if (string.IsNullOrEmpty(sreSection.Trace.File))
            {
                SetStatusText("Could not read log files", true);
                return;
            }

            LogFile = sreSection.Trace.File;

            string ext = Path.GetExtension(sreSection.Trace.File);
            string dir = Path.GetDirectoryName(sreSection.Trace.File);
            string onlyName = Path.GetFileNameWithoutExtension(sreSection.Trace.File);

            if (!Directory.Exists(dir))
            {
                MessageBox.Show(sreSection.Trace.File + " does not exist! Could not load the logs.", "Log File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LogFiles = Directory.GetFiles(dir, onlyName + "*" + ext);
        }

        private void Monitor_Shown(object sender, EventArgs e)
        {            
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void Monitor_Load(object sender, EventArgs e)
        {
            BindData();
        }
      
      
        private void BindData()
        {           
            SetStatusText("Trying to communicate with service...");
            CheckServiceStatus();
            SetStatusText("Loading logs...");           
            LoadLogFile();
            SetStatusText("Ready");           
        }
       
        void CheckServiceStatus(string status = null)
        {
            this.Cursor = Cursors.WaitCursor;
            if (status == null)
            {
                SetStatusText("Checking service status...");
                status = SreServiceCommunicator.GetPullersStatus();
            }

            toolStripButton2.Image = ServiceRunningImage;
            toolStripButton2.ToolTipText = "Service is running";
            SetStatusText("Ready");

            if (string.IsNullOrEmpty(status))
            {
                if (EyediaCoreConfigurationSection.CurrentConfig.HostingEnvironment == EyediaCoreConfigurationSection.HostingEnvironments.WindowsService)
                {
                    toolStripButton2.ToolTipText = "The service is not running. Click here to refresh once service is up and running!";
                    toolStripButton2.Image = null;
                }
                else if (EyediaCoreConfigurationSection.CurrentConfig.HostingEnvironment == EyediaCoreConfigurationSection.HostingEnvironments.WindowsService)
                {
                    toolStripButton2.ToolTipText = "The engine is not active. Make sure ASP.Net/IIS is configured properly.Double click here to refresh once web app is up and running!";
                }

            }

            this.Cursor = Cursors.Default;
        }

       
        //Configuration _Configuration;
        

       


        public static string GetCCFile(string fileName)
        {
            return Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName) + ".x.log");

        }

        DateTime _LogModifiedAt;
        private void LoadLogFile()
        {          
            if (!File.Exists(LogFile))
                return;

            DateTime dtLogModified = File.GetLastWriteTime(LogFile);
            if (_LogModifiedAt == dtLogModified)
                return;

            _LogModifiedAt = File.GetLastWriteTime(LogFile);

            try
            {
                using (FileStream fs = new FileStream(LogFile, FileMode.Open,
                                  FileAccess.Read, FileShare.ReadWrite))
                {
                    if (txtLog.InvokeRequired)
                    {
                        txtLog.Invoke(new MethodInvoker(delegate
                            {
                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    txtLog.Text = sr.ReadToEnd();
                                    txtLog.SelectionStart = txtLog.Text.Length;
                                    txtLog.ScrollToCaret();
                                }
                            }));
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            txtLog.Text = sr.ReadToEnd();
                            txtLog.SelectionStart = txtLog.Text.Length;
                            txtLog.ScrollToCaret();
                        }
                    }
                }

                toolStripProgressBar1.Visible = false;
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.ScrollToCaret();
            }
            catch { }
        }
       

      
        private string SearchHistoryFileName
        {
            get
            {
                if (!Directory.Exists(EyediaCoreConfigurationSection.CurrentConfig.TempDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(EyediaCoreConfigurationSection.CurrentConfig.TempDirectory);
                    }
                    catch
                    {
                        throw new Exception(string.Format("'{0}' - Drive does not exist. Please correct the 'tempDirectory' value under 'eyediaCoreConfigurationSection'"
                            , EyediaCoreConfigurationSection.CurrentConfig.TempDirectory));
                    }
                }
                string filePath = Path.Combine(EyediaCoreConfigurationSection.CurrentConfig.TempDirectory, "sre");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                return Path.Combine(filePath, "searchedStrings.txt");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            CheckServiceStatus();
        }        
      

        private void Monitor_Resize(object sender, EventArgs e)
        {
            txtLog.Refresh();
            lvSearchResult.Columns[2].Width = (lvSearchResult.Width - lvSearchResult.Columns[0].Width) - 200;
        }


        private void Monitor_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    txtFind.Focus();
                    break;
              
                case Keys.F6:
                    ClearCache_Click(sender, e);
                    break;
            }
        }        


        private void ClearCache_Click(object sender, EventArgs e)
        {
            try
            {
                new SreClient().ClearCache();
            }
            catch (Exception ex)
            {
                MainWindow.SetToolStripStatusLabel("Failed" + ex.ToString().Substring(0, 300), true);              
            }
        }
       

        private void picLocalWatcherStatus_Click(object sender, EventArgs e)
        {
            CheckServiceStatus();
        }       
      
        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = txtFind.Text.Length > 0 ? true : false;            
        }

      
        private void WriteSearchHistory()
        {

            if (File.Exists(SearchHistoryFileName))
            {
                string[] lines = File.ReadAllLines(SearchHistoryFileName);
                foreach (string line in lines)
                {
                    if (line == txtFind.Text)
                        return;
                }
            }
            foreach (string item in txtFind.Items)
            {
                if (item == txtFind.Text)
                    return;
            }

            txtFind.Items.Add(txtFind.Text);      
            StreamWriter sw = new StreamWriter(SearchHistoryFileName, true);
            sw.WriteLine(txtFind.Text);
            sw.Close();
        }

        private void ReadSearchHistory()
        {
             if (File.Exists(SearchHistoryFileName))
            {
                string[] lines = File.ReadAllLines(SearchHistoryFileName);
                foreach (string line in lines)
                {
                    txtFind.Items.Add(line);
                }
            }
        }
     
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            txtLog.Copy();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLogFile();           
        }

        private void contextMenuLog_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem.Enabled = !string.IsNullOrEmpty(txtLog.SelectedText);
        }

        private void txtFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (txtFind.Text)
            {
                case "------------------------":                    
                    btnFind.Enabled = false;
                    break;

                case "Go to start of a day":
                case "Go to end of a day":
                    btnFind.Enabled = true;
                    dateTimePicker1.Visible = true;
                    btnFind.Left = txtFind.Width + dateTimePicker1.Width + 10;
                    cbSearchOptions.Left = btnFind.Left + btnFind.Width + 5;
                    btnFind.Text = "Go";                    
                    cbSearchOptions.Visible = false;
                    break;

                default:
                    btnFind.Enabled = true;
                    dateTimePicker1.Visible = false;
                    btnFind.Text = "&Find";
                    btnFind.Left = txtFind.Width + 8;                    
                    cbSearchOptions.Visible = true;
                    cbSearchOptions.Left = btnFind.Left + btnFind.Width + 5;
                    break;
            }

        }

        private void txtLog_DoubleClick(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                Log log = new Log(txtLog.Text);
                log.Icon = this.Icon;
                log.ShowDialog();
                txtLog.Focus();
            }
        }

        #region Active Jobs
        void LoadActiveJobs()
        {
            /*
            lvActiveJobs.Columns.Clear();
            lvActiveJobs.Columns.Add("Job Id", 230);
            lvActiveJobs.Columns.Add("Started", 150);
            lvActiveJobs.Columns.Add("Requested", 100);
            lvActiveJobs.Columns.Add("Processed", 100);
            lvActiveJobs.Columns.Add("Time Elapsed", 100);
            lvActiveJobs.Columns.Add("Time Per Row", 100);

            lvActiveJobs.Items.Clear();
            string result = string.Empty;

            result = new SreClient().GetActiveJobs();
            if (string.IsNullOrEmpty(result))
            {
                SetStatusText("Failed to get active jobs", true);
            }

            if (string.IsNullOrEmpty(result))
                return;

            string[] jobs = result.Split("\r\n".ToCharArray());
            foreach (string job in jobs)
            {
                if (string.IsNullOrEmpty(job)) continue;

                string[] jobInfo = job.Split("|".ToCharArray());

                ListViewItem itm = new ListViewItem(jobInfo[0]);
                itm.SubItems.Add(jobInfo[1]);
                itm.SubItems.Add(jobInfo[2]);
                itm.SubItems.Add(jobInfo[3]);
                itm.SubItems.Add(jobInfo[4]);
                itm.SubItems.Add(jobInfo[5]);

                lvActiveJobs.Items.Add(itm);
            }

    */
        }
        #endregion Active Jobs

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            splitContainer3.Panel2Collapsed = false;

            foreach (string logFile in LogFiles)
            {
                if (!dateTimePicker1.Visible)
                {
                    SearchInFile(logFile, cbSearchOptions.SelectedIndex,
                        txtFind.Text, dateTimePicker1.Value);
                }
                else
                {
                    SearchInFile(logFile, cbSearchOptions.SelectedIndex,
                        string.Empty, dateTimePicker1.Value);
                }
            }
            this.Cursor = Cursors.Default;
           // MonitorLogSearchResult.DockState = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LoadLogFile();
        }

        private void miClear_Click(object sender, EventArgs e)
        {
            try
            {
                new SreClient().ClearLog();
            }
            catch
            {
                CheckServiceStatus(string.Empty);
            }

        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            //LoadLogFile();
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            timerRefresh.Enabled = chkAutoRefresh.Checked;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            List<IdpePersistentVariable> variables = new Manager().GetPersistentVariables();
            string data = string.Empty;
            foreach (IdpePersistentVariable variable in variables)
            {
                data += string.Format("{0},{1},{2},{3}{4}",
                    variable.DataSourceId,
                    variable.Name,
                    variable.Value,
                    variable.CreatedTS,
                    Environment.NewLine);
            }

            TextArea tArea = new TextArea("Persistent Variables");
            tArea.txtContent.Text = data;
            tArea.ShowDialog();

            this.Cursor = Cursors.Default;

        }

        private void showSearchResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = !splitContainer3.Panel2Collapsed;
        }
    }
}





