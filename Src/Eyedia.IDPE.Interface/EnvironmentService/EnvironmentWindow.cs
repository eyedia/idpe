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
using System.Threading.Tasks;
using System.Windows.Forms;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.Services;
using Eyedia.IDPE.DataManager;
using System.Reflection;
using System.IO;
using System.Configuration;

namespace Eyedia.IDPE.Interface
{
    public partial class EnvironmentWindow : Form
    {
        //Label lblLoading1;
        //Label lblLoading2;
        List<string> listOfdeploymentArtifacts;
        string SetLogOnUserBatchFileName;
        
        public EnvironmentWindow()
        {
            InitializeComponent();
            listOfdeploymentArtifacts = DeploymentService.DeploymentArtifacts;
            //lblLoading1 = new Label();
            //lblLoading1.AutoSize = false;
            //lblLoading1.Dock = DockStyle.Fill;
            //lblLoading1.Location = new System.Drawing.Point(0, 0);
            //lblLoading1.Name = "lblLoading1";            
            //lblLoading1.Size = new System.Drawing.Size(109, 38);            
            //lblLoading1.Text = "Loading";
            //tabPage1.Controls.Add(lblLoading1);

            //lblLoading2 = new Label();
            //lblLoading2.AutoSize = false;
            //lblLoading2.Dock = DockStyle.Fill;
            //lblLoading2.Location = new System.Drawing.Point(0, 0);
            //lblLoading2.Name = "lblLoading2";           
            //lblLoading2.Size = new System.Drawing.Size(109, 38);           
            //lblLoading2.Text = "Loading";
            //tabPage2.Controls.Add(lblLoading2);

            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            picServerStatus.Image = Properties.Resources.disabled;
            cbParam1.Dock = DockStyle.Fill;
            cbParam2.Dock = DockStyle.Fill;
            chkAllEnvs.Dock = DockStyle.Fill;
            cbServiceInstances.Dock = DockStyle.Fill;
            SreEnvironments_Resize(null, null);            
            Bind();           
        }

        #region Properties
        SreEnvironments Envs;
        private SreDataSource SelectedDataSource { get { return (SreDataSource)cbDataSources.SelectedItem; } }
        private SreRule SelectedRule { get { return (SreRule)cbRules.SelectedItem; } }
        SreEnvironment SelectedEnvironment
        {
            get
            {
                return (cbEnvs.SelectedIndex > -1) ? (SreEnvironment)cbEnvs.SelectedItem : null;
            }
        }
        private EnvironmentServiceCommands SelectedCommand { get { return (EnvironmentServiceCommands)cbCommands.SelectedItem; } }
        private SreEnvironmentConfig SelectedEnvironmentConfig { get { return (SreEnvironmentConfig)cbServiceInstances.SelectedItem; } }

        #endregion Properties

        #region Events
        
        
        #region UI Events
        private void cbCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch((EnvironmentServiceCommands)cbCommands.SelectedItem)
            {                
                case EnvironmentServiceCommands.Deploy:
                    BindDataSources();                    
                    lblParam1.Visible = true;
                    lblParam1.Text = "Keys";
                    cbParam1.Visible = false;
                    cbParam2.Visible = false;
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = false;
                    chkAllEnvs.Visible = false;
                    btnExecute.Visible = true;
                    btnExecute.Text = "Deploy";
                    btnAction.Enabled = false;
                    cbDataSources.Enabled = true;
                    GetTargetEnvironmentDataSources();
                    chkIncludeSystemDS.Enabled = true;
                    cbRules.Enabled = true;                   
                    btnDeployDataSource.Enabled = true;
                    btnDeployDataSource.Text = "Deploy";
                    btnDeployRule.Enabled = true;
                    break;

                case EnvironmentServiceCommands.DeployArtifacts:
                    lblParam1.Visible = false;
                    cbParam1.Visible = false;
                    cbParam2.Visible = false;
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = false;
                    chkAllEnvs.Visible = true;
                    btnExecute.Visible = true;
                    btnExecute.Text = "Artifacts";
                    btnAction.Enabled = true;                    
                    lblDataSources.ForeColor = SystemColors.ControlText;
                    cbDataSources.Enabled = false;
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = false;
                    btnDeployDataSource.Text = "Deploy";
                    btnDeployRule.Enabled = false;
                    break;

                case EnvironmentServiceCommands.SetServiceLogonUser:
                    lblParam1.Visible = false;
                    cbParam1.Visible = false;
                    cbParam2.Visible = false;
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = false;
                    chkAllEnvs.Visible = true;
                    btnExecute.Visible = true;
                    btnExecute.Text = "Set User";
                    btnAction.Enabled = true;
                    lblDataSources.ForeColor = SystemColors.ControlText;
                    cbDataSources.Enabled = false;
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = false;
                    btnDeployDataSource.Text = "Deploy";
                    btnDeployRule.Enabled = false;
                    break;

                case EnvironmentServiceCommands.StopSqlPuller:
                    BindDataSources(true);
                    lblParam1.Visible = false;
                    cbParam1.Visible = false;
                    cbParam2.Visible = false;
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = true;
                    chkAllEnvs.Visible = false;
                    btnExecute.Visible = false;
                    btnExecute.Text = "Execute";
                    btnAction.Enabled = false;
                    cbDataSources.Enabled = true;
                    GetTargetEnvironmentDataSources();
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = true;
                    btnDeployDataSource.Text = "Stop";
                    btnDeployRule.Enabled = false;
                    break;

                case EnvironmentServiceCommands.StartSqlPuller:
                    BindDataSources(true);
                    lblParam1.Visible = false;
                    cbParam1.Visible = false;
                    cbParam2.Visible = false;
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = true;
                    chkAllEnvs.Visible = false;
                    btnExecute.Visible = false;
                    btnExecute.Text = "Execute";
                    btnAction.Enabled = false;
                    cbDataSources.Enabled = true;
                    GetTargetEnvironmentDataSources();
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = true;
                    btnDeployDataSource.Text = "Start";
                    btnDeployRule.Enabled = false;
                    break;

                case EnvironmentServiceCommands.Unknown:
                    BindDataSources();
                    lblParam1.Visible = false;
                    cbParam1.Visible = false;
                    cbParam2.Visible = false;
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = false;
                    chkAllEnvs.Visible = false;
                    btnExecute.Visible = false;
                    btnExecute.Text = "Execute";
                    btnAction.Enabled = false;
                    cbDataSources.Enabled = false;
                    //GetTargetEnvironmentDataSources();
                    lblDataSources.ForeColor = SystemColors.ControlText;
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = false;
                    btnDeployDataSource.Text = "Deploy";
                    btnDeployRule.Enabled = false;                    
                    break;

                case EnvironmentServiceCommands.ExecuteDOSCommand:
                    lblParam1.Visible = true;
                    lblParam1.Text = "Command";
                    cbParam1.Visible = true;
                    cbParam2.Visible = false;
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = false;
                    chkAllEnvs.Visible = false;
                    btnExecute.Visible = true;
                    btnExecute.Text = "Execute";
                    btnAction.Enabled = false;
                    cbDataSources.Enabled = false;
                    GetTargetEnvironmentDataSources();
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = false;
                    btnDeployDataSource.Text = "Deploy";
                    btnDeployRule.Enabled = false;                    
                    GetCookie();
                    break;

                case EnvironmentServiceCommands.ProcessFile:
                    lblParam1.Visible = true;
                    lblParam1.Text = "File Name";
                    cbParam1.Visible = false;
                    cbParam2.Visible = true;
                    btnOpenFile.Visible = true;
                    cbServiceInstances.Visible = false;
                    chkAllEnvs.Visible = false;
                    btnExecute.Visible = true;
                    btnExecute.Text = "Process";
                    btnAction.Enabled = false;
                    cbDataSources.Enabled = true;
                    GetTargetEnvironmentDataSources();
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = false;
                    btnDeployDataSource.Text = "Deploy";
                    btnDeployRule.Enabled = false;                   
                    GetCookie();
                    break;

                default:
                    BindDataSources();
                    lblParam1.Visible = false;
                    cbParam1.Visible = false;
                    cbParam2.Visible = false;                    
                    btnOpenFile.Visible = false;
                    cbServiceInstances.Visible = false;
                    chkAllEnvs.Visible = false;
                    btnExecute.Visible = false;
                    btnAction.Enabled = true;
                    //GetTargetEnvironmentDataSources();
                    lblDataSources.ForeColor = SystemColors.ControlText;
                    cbDataSources.Enabled = false;
                    chkIncludeSystemDS.Enabled = false;
                    cbRules.Enabled = false;
                    btnDeployDataSource.Enabled = false;
                    btnDeployDataSource.Text = "Deploy";
                    btnDeployRule.Enabled = false;
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.splitContainer1.Visible = false;
            this.environmentsControl1.Visible = false;
            Application.DoEvents();
            Bind();
            this.splitContainer1.Visible = true;
            this.environmentsControl1.Visible = true;
            this.Cursor = Cursors.Default;
            Application.DoEvents();
        }

        private void lblParam1_Click(object sender, EventArgs e)
        {
            SaveCookie(true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SreEnvironments_Resize(object sender, EventArgs e)
        {
            cbDataSources.DropDownWidth = cbDataSources.Width + (cbDataSources.Width + (int)(cbDataSources.Width * .5));
            cbRules.DropDownWidth = cbRules.Width + (cbRules.Width + (int)(cbRules.Width * .5));
        }

        private void cbEnvs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(SelectedEnvironment != null )
            {
                BindConfigFiles();
                BindServiceInstances();
                BindCommands();
                lblInstances.Visible = SelectedEnvironment.IsLocal;
                nudInstances.Visible = SelectedEnvironment.IsLocal;
                radFileSystem.Checked = !SelectedEnvironment.WcfMode;
                radTcpIp.Checked = SelectedEnvironment.WcfMode;
                lblStatus.Visible = radTcpIp.Checked;
                if(tabControl1.SelectedIndex == 0)
                    timerServerStatus.Enabled = true;
            }
        }
                
        private void timerServerStatus_Tick(object sender, EventArgs e)
        {
            if (radTcpIp.Checked)
            {
                this.Cursor = Cursors.WaitCursor;
                Log(string.Format("Pinging server...[{0}] - {1}",
                SelectedEnvironment.Name,
                SelectedEnvironmentConfig.RemoteUrl), false);
                Application.DoEvents();
                CheckServerStatus();
                if (timerServerStatus.Interval > 1000)
                    timerServerStatus.Interval = 1000;
                timerServerStatus.Enabled = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void picServerStatus_Click(object sender, EventArgs e)
        {
            timerServerStatus.Enabled = true;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                cbParam2.Text = openFileDialog1.FileName;
        }

        
        private void btnOpenSreEnv_Click(object sender, EventArgs e)
        {
            try
            {                
                System.Diagnostics.Process.Start("notepad.exe", SelectedEnvironment.EnvXmlFile);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " Please ensure the root folder is set correctly and service has run at least once!",
                    "Environment - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string currentConfigFileName;
        bool currentConfigFileIsRemote;
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            
            this.Cursor = Cursors.WaitCursor;
            if(pgSymplus.SelectedObject is SreConfigurationSectionEditable)            
                ((SreConfigurationSectionEditable)pgSymplus.SelectedObject).
                   Save(ConfigurationManager.OpenExeConfiguration(GetExeNameFromConfigName(currentConfigFileName)));
            else
                ((EyediaCoreConfigurationSectionEditable)pgSymplus.SelectedObject).
                   Save(ConfigurationManager.OpenExeConfiguration(GetExeNameFromConfigName(currentConfigFileName)));            

            if (currentConfigFileIsRemote)
            {
                Log("Sending updated config file to " + SelectedEnvironment.Name + "...", false);
                FileTransferPacket packet = new FileTransferPacket();
                //this should be remote save path
                packet.FileName = Path.Combine(SelectedEnvironment.RootFolder, Path.GetFileName(currentConfigFileName));
                packet.Content = File.ReadAllBytes(currentConfigFileName);
                SreEnvironmentConfig selConfig = (SreEnvironmentConfig)cbEnvConfigs.SelectedItem;
                EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).SetConfigFile(SelectedEnvironmentConfig, packet);
                Log("Sent.");

                if (!currentConfigFileName.Contains("Configurator.exe"))
                {
                    Log("Sending restart signal to " + SelectedEnvironment.Name + "...", false);
                    try
                    {
                        EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).RestartService(selConfig);
                    }
                    catch (System.ServiceModel.CommunicationException commEx)
                    {
                        //we can eat this exception as the server must have been restarted
                    }
                    Log("Sent.");
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void cbEnvConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEnvConfigs.SelectedIndex > -1)
            {
                SreEnvironmentConfig selConfig = (SreEnvironmentConfig)cbEnvConfigs.SelectedItem;               
                this.Cursor = Cursors.WaitCursor;                
                //string configFileName = string.Empty;
                if (SelectedEnvironment.IsLocal)
                {
                    currentConfigFileName = selConfig.ConfigFileName;
                    lblConfigFileName.Text = selConfig.ConfigFileName;
                    currentConfigFileIsRemote = false;
                }
                else
                {
                    Log("Retrieving remote config file from " + SelectedEnvironment.Name + "...", false);
                    FileTransferPacket packet = null;
                    try
                    {
                        packet = EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).GetConfigFile(SelectedEnvironmentConfig, selConfig.ConfigFileName);
                    }
                    catch(Exception ex)
                    {
                        Log("Failed. " + ex.Message);                        
                    }
                    if ((packet == null) || (packet.Content == null))
                    {
                        Log(string.Format("The config file '{0}' was not found in the server", selConfig.ConfigFileName));
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else
                    {
                        Log("Retrieved.");

                        //make sure the exes are exist, else config exe won't open                       
                        if (!File.Exists(Path.Combine(Information.TempDirectorySre, "idpe.exe")))
                        {
                            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe"),
                                Path.Combine(Information.TempDirectorySre, "idpe.exe"), true);
                            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe"),
                                Path.Combine(Information.TempDirectorySre, "idpe1.exe"), true);
                            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe"),
                                Path.Combine(Information.TempDirectorySre, "idpe2.exe"), true);
                            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idpe.exe"),
                                Path.Combine(Information.TempDirectorySre, "idpe3.exe"), true);
                        }
                        
                        if (!File.Exists(Path.Combine(Information.TempDirectorySre, "idped.exe")))
                        {
                            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idped.exe"), 
                                Path.Combine(Information.TempDirectorySre, "idped.exe"), true);
                        }

                        lblConfigFileName.Text = SelectedEnvironment.Name + ":" + selConfig.ConfigFileName;
                        currentConfigFileName = Path.Combine(Information.TempDirectorySre, Path.GetFileName(packet.FileName));
                        this.SaveFileStream(currentConfigFileName, new MemoryStream(packet.Content));
                        currentConfigFileIsRemote = true;
                    }
                }
                if(!selConfig.IsService)
                    pgSymplus.SelectedObject = new EyediaCoreConfigurationSectionEditable(ConfigurationManager.OpenExeConfiguration(GetExeNameFromConfigName(currentConfigFileName)));
                else
                    pgSymplus.SelectedObject = new SreConfigurationSectionEditable(ConfigurationManager.OpenExeConfiguration(GetExeNameFromConfigName(currentConfigFileName)));


                this.Cursor = Cursors.Default;
            }
        }
        #endregion UI Events

        #endregion Events

        #region Helpers

        private string GetExeNameFromConfigName(string configName)
        {            
            if (configName.EndsWith(".config"))
                configName = configName.Replace(".config", string.Empty);
            return configName;
        }
        private void Bind()
        {
            Envs = EnvironmentServiceDispatcherFactory.GetEnvironments();
            cbCommands.Enabled = Envs.Count == 0 ? false : true;
            cbEnvs.DataSource = null;
            cbEnvs.DataSource = Envs;
            cbEnvs.DisplayMember = "Name";
            BindCommands();
            BindDataSources();
            cbRules.DataSource = null;
            cbRules.DataSource = new Manager().GetRules().OrderBy(r => r.Name).ToList();
            cbRules.DisplayMember = "Name";
        }

        private void BindDataSources(bool onlyPullSqlType = false)
        {
            cbDataSources.DataSource = null;
            List<SreDataSource> dataSources = new Manager().GetDataSources().OrderBy(ds => ds.Name).ToList();
            dataSources.Where(ds => ds.DataFeederType == null).ToList().ForEach(ds1 => ds1.DataFeederType = 0);

            if (onlyPullSqlType)
                dataSources = dataSources.Where(ds => (DataFeederTypes)ds.DataFeederType == DataFeederTypes.PullSql).ToList();

            cbDataSources.DataSource = dataSources;
            cbDataSources.DisplayMember = "Name";
        }

        private void BindConfigFiles()
        {
            cbEnvConfigs.DataSource = null;
            cbEnvConfigs.DisplayMember = "DisplayName";
            cbEnvConfigs.DataSource = SelectedEnvironment != null ? SelectedEnvironment.EnvironmentConfigs : null;            
        }

        private void BindServiceInstances()
        {
            cbServiceInstances.DataSource = null;
            cbServiceInstances.DataSource = SelectedEnvironment != null ? SelectedEnvironment.EnvironmentConfigs.Where(ecf => ecf.IsService).ToList() : null;
            cbServiceInstances.DisplayMember = "DisplayName";
            if (cbServiceInstances.Items.Count > 0)
                cbServiceInstances.SelectedIndex = 0;
        }

        private void DisableServiceStatus()
        {
            if(radTcpIp.Checked)
                Log("Click on status icon to refresh remote service status in few minutes.");
            picServerStatus.Image = Properties.Resources.disabled;
        }

        private void EnableAll(bool enable)
        {
            tabControl1.Enabled = enable;
            btnClose.Enabled = enable;          
        }

        private void GetCookie()
        {
            foreach (SreEnvironment env in Envs)
            {
                if (env.Name == SelectedEnvironment.Name)
                {
                    if (!string.IsNullOrEmpty(env.Param1))
                    {
                        if (env.Param1.Contains(","))
                        {
                            cbParam1.DataSource = new List<string>(env.Param1.Split(",".ToCharArray()));
                        }
                        else
                        {
                            cbParam1.Text = env.Param1;
                        }
                    }

                    if (!string.IsNullOrEmpty(env.Param2))
                    {
                        if (env.Param2.Contains(","))
                        {
                            cbParam2.DataSource = new List<string>(env.Param2.Split(",".ToCharArray()));
                        }
                        else
                        {
                            cbParam2.Text = env.Param2;
                        }
                    }
                }
            }
        }

        private void SaveCookie(bool clean = false)
        {
            foreach (SreEnvironment env in Envs)
            {
                if (env.Name == SelectedEnvironment.Name)
                {
                    if (clean == false)
                    {
                        if (string.IsNullOrEmpty(env.Param1))
                            env.Param1 = cbParam1.Text;
                        else
                            env.Param1 += "," + cbParam1.Text;

                        if (string.IsNullOrEmpty(env.Param2))
                            env.Param2 = cbParam2.Text;
                        else
                            env.Param2 += "," + cbParam2.Text;
                    }
                    else
                    {
                        env.Param1 = string.Empty;
                        cbParam1.DataSource = null;
                        cbParam1.Text = "";

                        env.Param2 = string.Empty;
                        cbParam2.DataSource = null;
                        cbParam2.Text = "";
                    }
                    //remove duplicates
                    List<string> items = new List<string>(env.Param1.Split(",".ToCharArray()));
                    items = items.Distinct().ToList();
                    env.Param1 = string.Join(",", items);

                    //remove duplicates
                    items = new List<string>(env.Param2.Split(",".ToCharArray()));
                    items = items.Distinct().ToList();
                    env.Param2 = string.Join(",", items);

                    EnvironmentServiceDispatcherFactory.SaveEnvironment(Envs);
                    //Bind();
                }
            }
        }

        private void Log(string[] lines, bool newLine = true, Color? color = null)
        {
            foreach(string line in lines)
            {
                Log(line, newLine, color);
            }
        }

        private void Log(string text, bool newLine = true, Color? color = null)
        {
            Color foreColor = color == null ? rtfLog.ForeColor : (Color)color;
            
            rtfLog.SelectionStart = rtfLog.TextLength;
            rtfLog.SelectionLength = 0;

            rtfLog.SelectionColor = foreColor;
            rtfLog.AppendText(text);
            rtfLog.SelectionColor = rtfLog.ForeColor;
            if (newLine)
                rtfLog.AppendText(Environment.NewLine);
        }
        private void GetTargetEnvironmentDataSources()
        {
            if (((SelectedCommand == EnvironmentServiceCommands.ProcessFile)
                || (SelectedCommand == EnvironmentServiceCommands.StopSqlPuller)
                || (SelectedCommand == EnvironmentServiceCommands.StartSqlPuller))
                && (SelectedEnvironment.WcfMode))
            {
                lblDataSources.ForeColor = SystemColors.HotTrack;
                Log(string.Format("Retrieving data sources from '{0}' environment...", SelectedEnvironment.Name), false);

                List<SreDataSource> dataSources = EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).GetDataSources(SelectedEnvironmentConfig);
                if (SelectedCommand == EnvironmentServiceCommands.ProcessFile)
                {
                    dataSources = dataSources.Where(ds => (DataFeederTypes)ds.DataFeederType != DataFeederTypes.PullSql).ToList();
                    dataSources = dataSources.Where(ds => ds.IsSystem == false).ToList();
                    Log(string.Format("{0} retrieved.", dataSources.Count));
                }
                else if ((SelectedCommand == EnvironmentServiceCommands.StopSqlPuller)
                    || (SelectedCommand == EnvironmentServiceCommands.StartSqlPuller))
                {
                    dataSources = dataSources.Where(ds => (DataFeederTypes)ds.DataFeederType == DataFeederTypes.PullSql).ToList();
                    Log(string.Format("{0} retrieved & filtered on sql pull type datasources.", dataSources.Count));
                }

                cbDataSources.DataSource = null;
                cbDataSources.DataSource = dataSources;
                cbDataSources.DisplayMember = "Name";

            }
            else
            {
                cbDataSources.DataSource = null;
                List<SreDataSource> dataSources = new Manager().GetDataSources().OrderBy(ds => ds.Name).ToList();
                dataSources.Where(ds => ds.DataFeederType == null).ToList().ForEach(ds1 => ds1.DataFeederType = 0);
                if ((SelectedCommand == EnvironmentServiceCommands.StopSqlPuller)
                || (SelectedCommand == EnvironmentServiceCommands.StartSqlPuller))
                    dataSources = dataSources.Where(ds => (DataFeederTypes)ds.DataFeederType == DataFeederTypes.PullSql).ToList();
                else
                    dataSources = dataSources.Where(ds => (DataFeederTypes)ds.DataFeederType != DataFeederTypes.PullSql).ToList();

                dataSources = dataSources.Where(ds => ds.IsSystem == false).ToList();
                cbDataSources.DataSource = dataSources;
                cbDataSources.DisplayMember = "Name";
                lblDataSources.ForeColor = SystemColors.ControlText;
                if ((((SelectedCommand == EnvironmentServiceCommands.ProcessFile)
                || (SelectedCommand == EnvironmentServiceCommands.StopSqlPuller)
                || (SelectedCommand == EnvironmentServiceCommands.StartSqlPuller)))
                    && (radFileSystem.Checked))
                    Log("As mode is filesystem, environment service will assume that target environment has same set of data sources!", true, Color.DarkBlue);
            }
        }

        private void BindCommands()
        {
            if (SelectedEnvironment.IsLocal)
            {
                cbCommands.DataSource = Enum.GetValues(typeof(EnvironmentServiceCommands)).Cast<EnvironmentServiceCommands>().
                Where(c => ((c != EnvironmentServiceCommands.Deploy) && (c != EnvironmentServiceCommands.DeploySdf) && (c != EnvironmentServiceCommands.DeployArtifacts))).OrderBy( c1 => c1.ToString()).ToList();                
            }
            else
            {
                cbCommands.DataSource = ((EnvironmentServiceCommands[])Enum.GetValues(typeof(EnvironmentServiceCommands))).OrderBy(c => c.ToString()).ToList();
            }
            cbCommands.SelectedItem = EnvironmentServiceCommands.Unknown;
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            if (cbCommands.Items.Count > 0)
                cbCommands.SelectedIndex = 0;
            cbCommands.Enabled = true;
            chkIncludeSystemDS.Enabled = true;

            if (radTcpIp.Checked)
            {
                if (string.IsNullOrEmpty(SelectedEnvironmentConfig.RemoteUrl))
                {
                    if (MessageBox.Show(string.Format("Remote URL is not set for the Environment {0}. Do you want to set now?"), "Environment - Question",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        lblStatus.Visible = false;
                        picServerStatus.Visible = false;
                        tabControl1.SelectedIndex = 1;
                        return;
                    }
                }
                lblStatus.Visible = radTcpIp.Checked;
                picServerStatus.Image = Properties.Resources.disabled;
                picServerStatus.Visible = true;
                timerServerStatus.Enabled = true;
            }
            else
            {
                lblStatus.Visible = false;
                picServerStatus.Visible = false;
            }
        }

        private void CheckServerStatus()
        {            
            bool up = false;
            try
            {
                EnvironmentServicePacket packet = new EnvironmentServicePacket();
                up = EnvironmentServiceDispatcherUsingWcf.GetWcfClient(SelectedEnvironmentConfig).Ping(packet) == "success" ? true : false;
            }
            catch { }
            if(up)
                Log(".....Alive", true, Color.DarkGreen);
            else
                Log(".....Down", true, Color.DarkRed);

            picServerStatus.Image = up ? Properties.Resources.status_anim : Properties.Resources.disabled;

            if (up)
            {
                cbCommands.Enabled = true;
            }
            else
            {
                cbCommands.Enabled = false;
            }
        }

        private bool ValidateExecuteParams()
        {
            if ((btnExecute.Text == "Execute") && (string.IsNullOrEmpty(cbParam1.Text)))
            {
                Log("Please provide command to be executed.", true, Color.DarkRed);
                return false;
            }
            else if ((btnExecute.Text == "Process") && (string.IsNullOrEmpty(cbParam1.Text)))
            {
                Log("Please provide file to be processed.", true, Color.DarkRed);
                return false;
            }

            return true;
        }

        private void SaveFileStream(string filePath, Stream stream)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Dispose();
        }

        private EnvironmentServiceDispatcher Dispatcher
        {
            get
            {
                EnvironmentServiceDispatcher dispatcher = EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked);
                dispatcher.Callback += Dispatcher_Callback;
                return dispatcher;
            }
        }

        private void Dispatcher_Callback(string info, bool isError)
        {
            if (isError)
                Log(info, true, Color.DarkRed);
            else
                Log(info);
        }

        private void DeployArtifacts(SreEnvironment toEnv, SreEnvironmentConfig config)
        {
           
            try
            {
                if (Dispatcher.DeployArtifacts(config) == Constants.success)
                {
                    Log("Artifacts deployed on " + toEnv.Name + "! The service was restarted.");
                    DisableServiceStatus();
                }
                else
                {
                    Log("Artifacts deployment failed!", true, Color.DarkRed);
                }
            }
            catch (System.ServiceModel.CommunicationException) { }
        }
        #endregion Helpers

        private void nudInstances_ValueChanged(object sender, EventArgs e)
        {
            SelectedEnvironment.SetNumberOfInstances((int)nudInstances.Value);
        }

        private void btnRefreshConfig_Click(object sender, EventArgs e)
        {
            cbEnvConfigs_SelectedIndexChanged(sender, e);
        }

        EnvironmentServiceCommands delayedCommand = EnvironmentServiceCommands.Unknown;
        SreEnvironmentConfig delayedCommandEnvironmentConfig = null;
        SreEnvironment delayedCommandSelectedEnvironment = null;
        private void timerDelayedCommand_Tick(object sender, EventArgs e)
        {
            if ((delayedCommandSelectedEnvironment == null)
                || (delayedCommandEnvironmentConfig == null))
            {
                timerDelayedCommand.Enabled = false;
                return;
            }
            switch(delayedCommand)
            {
                case EnvironmentServiceCommands.GetLastDeploymentLog:
                    FileTransferPacket packet = Dispatcher.GetLastDeploymentLog(delayedCommandEnvironmentConfig);
                    if ((packet != null) && (packet.Content != null))
                    {
                        string tempFile = Path.Combine(Information.TempDirectoryTempData, DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
                        Log("Last artifacts from " + delayedCommandSelectedEnvironment.Name + ":");
                        SaveFileStream(tempFile, new MemoryStream(packet.Content));
                        Log(File.ReadAllLines(tempFile), true, Color.DarkBlue);
                    }
                    else
                    {
                        Log("Could not find last artifacts deployment log, either nothing was deployed recently on " + delayedCommandSelectedEnvironment.Name
                            + ", or the log file has been deleted."
                            , true, Color.DarkRed);
                    }
                    break;
            }
        }
    }
}


