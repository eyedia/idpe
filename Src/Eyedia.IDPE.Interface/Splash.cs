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
using System.Reflection;
using System.Configuration;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using System.IO;
using System.Diagnostics;
using Eyedia.Core.Data;
using Eyedia.Core.Net;

namespace Eyedia.IDPE.Interface
{
    public partial class Splash : Form
    {        
        UserCookie _UserCookie;
        string _IniPath;
        public Splash()
        {
            try
            {
                InitializeComponent();                
                Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Initialize Error");                
            }
        }

        MainWindow MainWindow { get; set; }

        bool IgnoreDataBaseChangedEvent;
        bool RestartedSelf;
        public Splash(string[] args, MainWindow mainWindow = null)
        {
            MainWindow = mainWindow;
            RestartedSelf = true;            
            InitializeComponent();            

            Init();
            if (args.Length > 0)
            {                
                IgnoreDataBaseChangedEvent = true;                
                cbSdfs.Text = args[0];
                IgnoreDataBaseChangedEvent = false;
            }
        }

        void Init()
        {
            _IniPath = AppDomain.CurrentDomain.BaseDirectory + "sre.ini";
            if (File.Exists(_IniPath))
            {
                StreamReader sr = new StreamReader(_IniPath);
                string fileContent = sr.ReadToEnd();
                sr.Close();          
                _UserCookie = new UserCookie(fileContent);               
                foreach (string user in _UserCookie.UserNames)
                {
                    cbUserName.Items.Add(user);
                }
                cbUserName.Text = _UserCookie.UserName;
                cbSdfs.Text = _UserCookie.SdfName;
            }
            else
            {
                _UserCookie = new UserCookie();
            }

            lblVersion.Text = "Version - " + AssemblyVersion;

            if (EyediaCoreConfigurationSection.CurrentConfig.AuthenticationType == AuthenticationTypes.ActiveDirectory)
            {
                pnlLoginBox.Visible = false;
                Authenticator authenticator = new Authenticator(EyediaCoreConfigurationSection.CurrentConfig.AuthenticationType);
                Information.LoggedInUser = authenticator.Authenticate();
                if (authenticator.IsAuthenticated)
                {
                    timerLoad.Enabled = true;
                }
                else
                {
                    ShowMessage("Unauthorized: Active directory authentication failed. Please contact administrator for further information.", true);
                    timerExit.Enabled = true;
                    return;
                }
            }
            else if (EyediaCoreConfigurationSection.CurrentConfig.AuthenticationType == AuthenticationTypes.ActiveDirectoryGroup)
            {
                pnlLoginBox.Visible = false;
                Authenticator authenticator = new Authenticator(EyediaCoreConfigurationSection.CurrentConfig.AuthenticationType);
                Information.LoggedInUser = authenticator.Authenticate(EyediaCoreConfigurationSection.CurrentConfig.AuthorizedGroups);
                if (authenticator.IsAuthenticated)
                {                    
                    timerLoad.Enabled = true;
                }
                else
                {
                    ShowMessage("Unauthorized: Seems like you do not belong to correct AD group(s). Please contact administrator for further information.", true);
                    timerExit.Enabled = true;
                    return;
                }
                
            }
            else
            {
                List<string> sdfFiles = new List<string>(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.sdf"));
                foreach (string dbFile in sdfFiles)
                {
                    cbSdfs.Items.Add(Path.GetFileName(dbFile));
                }

                if (cbSdfs.Items.Count > 1)
                {
                    label3.Visible = true;
                    cbSdfs.Visible = true;
                    IgnoreDataBaseChangedEvent = true;
                    cbSdfs.Text = _UserCookie.SdfName;
                    IgnoreDataBaseChangedEvent = false;

                    if (cbSdfs.SelectedIndex == -1)
                    {
                        if (cbSdfs.Items.Count == 1)
                        {
                            cbSdfs.SelectedIndex = 0;
                        }
                        else if (cbSdfs.Items.Count > 0)
                        {
                            cbSdfs.Select();
                            return;
                        }
                    }

                }
                else
                {
                    label3.Visible = false;
                    cbSdfs.Visible = false;
                }
            }
        }        

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        int counter;
        public bool SplashCompleted;
        private void timerLoad_Tick(object sender, EventArgs e)
        {
            lblMessage.Visible = true;
            Application.DoEvents();
            switch (counter)
            {
                case 0:
                    lblMessage.Text = "Initializing...";
                    break;                
                case 1:
                    lblMessage.Text = "Initializing...Database.";
                    Application.DoEvents();
                    string anyError = TestDatabaseConnection();
                    if (anyError != string.Empty)
                    {
                        lblMessage.Text = "Initializing...Database. Failed!";                       
                        timerLoad.Enabled = false;
                        ShowMessage(anyError, true);
                    }
                    if(!ValidateSystemObject())
                    {
                        lblMessage.Text = "Initializing...Database. Failed!";
                        timerLoad.Enabled = false;
                        ShowMessage("System objects could not be created! Please contact system administrator.", true);
                    }
                    break;
                case 2:
                    lblMessage.Text = "Initializing...Menu.";
                    break;
                case 3:
                    lblMessage.Text = "Initializing...Plug Ins.";
                    string from = Assembly.GetAssembly(typeof(System.Activities.Core.Presentation.ConnectionPointType)).Location;
                    string to = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(from));
                    if(!File.Exists(to))
                        File.Copy(from, to, true);

                    break;
                case 4:
                    lblMessage.Text = "Initializing...Graphics.";                   
                    Application.DoEvents();
                    ExtractImages();

                    if (MainWindow != null)
                        MainWindow.OpenDataSourceWindow();
                    break;
                case 5:                   
                    this.DialogResult = DialogResult.OK;
                    //SplashCompleted = true;
                    timerLoad.Enabled = false;                   
                    break;
            }
            counter++;
        }

        private bool ValidateSystemObject()
        {
            DataManager.Manager manager = new DataManager.Manager();
            if (!manager.ValidateSystemObjects())
            {
                manager.InsertSystemObjects();
                return manager.ValidateSystemObjects();
            }
            return true;
        }

        private void ExtractImages()
        {
            string dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Utility.ExtractImages(dir, "Graphics.WorkflowActivities");                
            }
        }

        private string TestDatabaseConnection()
        {
            try
            {
                CoreDatabaseObjects.Instance.TestConnection();
                return string.Empty;
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }
       
        private void cbUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (btnOK.Enabled))
            {
                e.Handled = true;
                btnOK_Click(sender, e);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
            _UserCookie.Save(cbUserName.Text, cbSdfs.Text);
            if (File.Exists(_IniPath))
                File.Delete(_IniPath);
            StreamWriter sw = new StreamWriter(_IniPath);
            sw.Write(_UserCookie.Serialize());
            sw.Close();
            if ((EyediaCoreConfigurationSection.CurrentConfig.AuthenticationType == AuthenticationTypes.ActiveDirectoryGroup)
                && (string.IsNullOrEmpty(EyediaCoreConfigurationSection.CurrentConfig.AuthorizedGroups) ))
            {
                ShowMessage("Authorize group name is required when authentication type is 'ActiveDirectoryGroup'", true);
                return;
            }
            Authenticator authenticator = new Authenticator(EyediaCoreConfigurationSection.CurrentConfig.AuthenticationType);
            authenticator.Callback += Authenticator_Callback;
            Information.LoggedInUser = authenticator.Authenticate(cbUserName.Text, txtPassword.Text);
            Trace.TraceInformation("Auth result:" + authenticator.ToString());
            Trace.Flush();
            if (authenticator.IsAuthenticated)
            {
                ShowMessage(string.Empty, false);
                Information.LoggedInUser = CoreDatabaseObjects.Instance.GetUser(cbUserName.Text);
                Information.LoggedInUser.GroupId = authenticator.GroupId;
            }
            else
            {
                ShowMessage("Incorrect user name or password.", true);
                return;
            }
            pnlLoginBox.Enabled = false;
            timerLoad.Enabled = true;
        }

        private void Authenticator_Callback(string info)
        {
            lblErrorMessage.Text = info;
            Application.DoEvents();
        }

        void ShowMessage(string message, bool isError)
        {
            lblErrorMessage.Text = message;
            lblErrorMessage.ForeColor = isError ? Color.DarkRed : Color.Black;
            Application.DoEvents();         
        }


        private void Validate(object sender, EventArgs e)
        {
            if (cbSdfs.Visible == false)
                btnOK.Enabled = ((cbUserName.Text.Length > 0) && (txtPassword.Text.Length > 0));
            else
                btnOK.Enabled = ((cbUserName.Text.Length > 0) && (txtPassword.Text.Length > 0) && (cbSdfs.SelectedIndex > -1));

        }

        private void Splash_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.Close();
            }
        }

        private void timerReset_Tick(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            timerReset.Enabled = false;
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void cmbInstances_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (IgnoreDataBaseChangedEvent)
                return;
            if (cbSdfs.Items.Count > 1)
            {
                if (cbSdfs.SelectedItem != null)
                {
                    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.ConnectionStrings.ConnectionStrings["cs"].ConnectionString = FormatSQLCEConnectionString();
                    config.Save(ConfigurationSaveMode.Modified);
                    System.Diagnostics.Process.Start(Application.ExecutablePath, cbSdfs.Text);
                    Application.Exit();
                }
            }
        }

        private string FormatSQLCEConnectionString()
        {
            if ((cbSdfs.Text == string.Empty)
                && (cbSdfs.Items.Count > 0))
                cbSdfs.SelectedIndex = 0;

            return string.Format("Data Source='{0}';password=acc3s$", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cbSdfs.Text));
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            if (RestartedSelf)
            {
                if (cbUserName.Text == string.Empty)
                    cbUserName.Select();
                else
                    txtPassword.Select();
            }
            else
            {
                if (cbSdfs.SelectedIndex == -1)
                {
                    if (cbSdfs.Items.Count == 1)
                    {
                        cbSdfs.SelectedIndex = 0;
                    }
                    else if (cbSdfs.Items.Count > 0)
                    {
                        cbSdfs.Select();
                        return;
                    }
                }

                if (cbUserName.Text == string.Empty)
                    cbUserName.Select();
                else
                    txtPassword.Select();
            }       
        }

        private void picTitle_MouseUp(object sender, MouseEventArgs e)
        {
            if (((ModifierKeys & Keys.Control) != 0)
                && ((ModifierKeys & Keys.Alt) != 0)
                && ((ModifierKeys & Keys.Shift) != 0)
                && (e.Button == MouseButtons.Right))
            {
                CoreDatabaseObjects.Instance.ExecuteStatement("delete from User where UserName = 'root'");
                blinked = 0;
                timerBlink.Enabled = true;
            }
        }
        int blinked = 0;

        private void timerBlink_Tick(object sender, EventArgs e)
        {
            lblRecoveryMode.Visible = lblRecoveryMode.Visible ? false : true;
            blinked++;

            if (blinked > 11)
                timerBlink.Enabled = false;
        }

        int _ExitTimeOutCount = 10;
        string oldMessage;
        private void timerExit_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(oldMessage))
                oldMessage = lblErrorMessage.Text;
            lblErrorMessage.Text = oldMessage + ". Exiting in " + _ExitTimeOutCount.ToString("00") + " seconds. Press escape to exit now.";
            _ExitTimeOutCount = _ExitTimeOutCount - 1;

            if (_ExitTimeOutCount <= 0)
                Application.Exit();
        }
    }   
    
}





