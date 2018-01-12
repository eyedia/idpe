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
    public partial class EnvironmentWindow
    {
        private void btnAction_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                switch ((EnvironmentServiceCommands)cbCommands.SelectedItem)
                {
                    case EnvironmentServiceCommands.Stop:
                        try
                        {
                            EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).StopService(SelectedEnvironmentConfig);
                        }
                        catch (System.ServiceModel.CommunicationException) { }
                        Log("Stop service command processed on " + SelectedEnvironment.Name + "! The service was stopped.");
                        DisableServiceStatus();
                        break;

                    case EnvironmentServiceCommands.Restart:
                        try
                        {
                            EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).RestartService(SelectedEnvironmentConfig);
                        }
                        catch (System.ServiceModel.CommunicationException) { }
                        Log("Restart command processed on " + SelectedEnvironment.Name + "! The service was restarted.");
                        DisableServiceStatus();
                        break;

                    case EnvironmentServiceCommands.DeploySdf:
                        try
                        {
                            EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).DeploySdf(SelectedEnvironmentConfig);
                        }
                        catch (System.ServiceModel.CommunicationException) { }
                        Log("SDF deployed on " + SelectedEnvironment.Name + "! The service was restarted.");
                        DisableServiceStatus();
                        break;

                    case EnvironmentServiceCommands.DeployArtifacts:
                        if (!chkAllEnvs.Checked)
                        {
                            DeployArtifacts(SelectedEnvironment, SelectedEnvironmentConfig);
                        }
                        else
                        {
                            foreach(SreEnvironment env in Envs)
                            {
                                if(!env.IsLocal)                                
                                    DeployArtifacts(env, env.EnvironmentConfigs[0]);                                
                            }
                        }
                        break;

                    case EnvironmentServiceCommands.GetLastDeploymentLog:
                        delayedCommand = EnvironmentServiceCommands.GetLastDeploymentLog;
                        delayedCommandSelectedEnvironment = SelectedEnvironment;
                        delayedCommandEnvironmentConfig = SelectedEnvironmentConfig;
                        timerDelayedCommand_Tick(sender, e);
                        break;

                    case EnvironmentServiceCommands.SetServiceLogonUser:
                        if (string.IsNullOrEmpty(SetLogOnUserBatchFileName))
                        {
                            Log("Please click on 'Set User' to set user name and password", true, Color.DarkRed);
                        }
                        else
                        {
                            EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).SetServiceLogonUser(SelectedEnvironmentConfig, SetLogOnUserBatchFileName);
                            Log("Service logon user was set on " + SelectedEnvironment.Name + "! The service(s) will be restarted.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Environment Manager - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnableAll(true);
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void btnDeployDataSource_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EnableAll(false);

            try
            {
                if (btnDeployDataSource.Text == "Deploy")
                {
                    if (chkIncludeSystemDS.Checked)
                    {
                        EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).DeployDataSource(SelectedEnvironmentConfig, (int)SelectedDataSource.SystemDataSourceId);
                        if (radFileSystem.Checked)
                        {
                            Log(string.Format("The system data source '{0}' being deployed on {1}. Waiting 30 seconds to finish deployment.",
                            new Manager().GetDataSourceDetails((int)SelectedDataSource.SystemDataSourceId).Name, SelectedEnvironment.Name));
                            this.Cursor = Cursors.Default;
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(30 * 1000);
                        }
                    }
                    this.Cursor = Cursors.WaitCursor;
                    Application.DoEvents();
                    EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).DeployDataSource(SelectedEnvironmentConfig, SelectedDataSource.Id);
                    Log(string.Format("The data source '{0}' is being deployed on {1}",
                        SelectedDataSource.Name, SelectedEnvironment.Name));
                }
                else if (btnDeployDataSource.Text == "Stop")
                {
                    EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).StopSqlPuller(SelectedEnvironmentConfig, SelectedDataSource.Id);
                    Log(string.Format("The sql puller data source '{0}' stopped on {1}",
                        SelectedDataSource.Name, SelectedEnvironment.Name));
                }
                else if (btnDeployDataSource.Text == "Start")
                {
                    EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).StartSqlPuller(SelectedEnvironmentConfig, SelectedDataSource.Id);
                    Log(string.Format("The sql puller data source '{0}' started on {1}",
                        SelectedDataSource.Name, SelectedEnvironment.Name));
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, true, Color.DarkRed);
                EnableAll(true);
                this.Cursor = Cursors.Default;
            }
            EnableAll(true);
            this.Cursor = Cursors.Default;
        }

        private void btnDeployRule_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EnableAll(false);
            EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).DeployRule(SelectedEnvironmentConfig, SelectedRule);
            Log(string.Format("The rule '{0}' deployed on {1}",
                SelectedRule.Name, SelectedEnvironment.Name));
            EnableAll(true);
            this.Cursor = Cursors.Default;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!ValidateExecuteParams())
            {
                this.Cursor = Cursors.Default;
                return;
            }

            EnableAll(false);
            if (btnExecute.Text == "Execute")
            {
                string result = EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).ExecuteCommand(SelectedEnvironmentConfig, cbParam1.Text);

                if (result == Constants.success)
                {
                    Log(string.Format("The command '{0}' executed on {1}",
                        cbParam1.Text, SelectedEnvironment.Name));
                }
                else
                {
                    Log(string.Format("Failed to process file on : '{0}': '{1}'",
                    SelectedEnvironment.Name, result));
                }
                SaveCookie();

            }
            else if (btnExecute.Text == "Process")
            {
                string result = EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).ProcessFile(SelectedEnvironmentConfig, SelectedDataSource.Id, cbParam2.Text);
                if (result == Constants.success)
                {
                    Log(string.Format("The file '{0}' is being processed on {1}",
                    cbParam2.Text, SelectedEnvironment.Name));
                }
                else
                {
                    Log(string.Format("Failed to process file on : '{0}'. Fail Reason: '{1}'",
                    SelectedEnvironment.Name, result));
                }
                SaveCookie();

            }
            else if (btnExecute.Text == "Artifacts")
            {
                DeploymentArtifacts deploymentArtifacts = new DeploymentArtifacts();
                deploymentArtifacts.ShowDialog();
                listOfdeploymentArtifacts = deploymentArtifacts.SelectedFiles;
            }
            else if (btnExecute.Text == "Set User")
            {
                WindowsServiceLogOnAsDialog logonUser = new WindowsServiceLogOnAsDialog(SelectedEnvironment.EnvironmentConfigsInstancesOnly.Count);
                if (logonUser.ShowDialog() == DialogResult.OK)
                    SetLogOnUserBatchFileName = logonUser.BatchFileName != null ? File.ReadAllText(logonUser.BatchFileName) : string.Empty;
            }
            else if (btnExecute.Text == "Deploy")
            {
                DeployableKeysWindow keyWindow = new DeployableKeysWindow();
                if(keyWindow.ShowDialog() == DialogResult.OK)
                {
                    Log(string.Format("Deploying '{0}' keys on {1}...",
                        keyWindow.SelectedKeys.Count, SelectedEnvironment.Name), false);
                    string result = EnvironmentServiceDispatcherFactory.GetInstance(radTcpIp.Checked).DeployKeys(SelectedEnvironmentConfig, keyWindow.SelectedKeys);

                    if (result == Constants.success)
                    {
                        Log("Deplyed.");
                    }
                    else
                    {
                        Log("Failed.");
                    }
                }
            }
                EnableAll(true);
            this.Cursor = Cursors.Default;
        }
    }
}


