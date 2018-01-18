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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Services;
using System.IO;

namespace Eyedia.IDPE.Interface
{
    public partial class EnvironmentsControl : UserControl
    {
        SreEnvironments Envs;
        public EnvironmentsControl()
        {
            InitializeComponent();

            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            if (designMode == false)
                Bind(null);
        }

        SreEnvironment SelectedEnvironment
        {
            get
            {
                return lbEnvs.SelectedIndex > -1 ? (SreEnvironment)lbEnvs.SelectedItem : null;
            }
        }

        private void Bind(SreEnvironment selected, SreEnvironments envs = null)
        {
            lbEnvs.DataSource = null;
            lbEnvs.SelectionMode = SelectionMode.None;
            lbEnvs.DisplayMember = "Name";
            Envs = (envs == null) ? EnvironmentServiceDispatcherFactory.GetEnvironments() : envs;
            lbEnvs.DataSource = Envs;
            lbEnvs.SelectionMode = SelectionMode.One;
            if ((selected == null) && lbEnvs.Items.Count > 0)
                lbEnvs.SelectedIndex = 0;
            else if (selected != null)
                lbEnvs.SelectedItem = selected;

        }

        private void BindEnvironment()
        {
            if (SelectedEnvironment != null)
            {
                txtName.Enabled = !SelectedEnvironment.IsLocal;
                txtName.Text = SelectedEnvironment.Name;
                txtRootFolder.Text = SelectedEnvironment.RootFolder;
                txtPullFolder.Text = SelectedEnvironment.PullFolder;
                radFileSystem.Checked = !SelectedEnvironment.WcfMode;
                radTcpIp.Checked = SelectedEnvironment.WcfMode;

                var instances = SelectedEnvironment.EnvironmentConfigs.Where(ecf => ecf.IsService).ToList();

                SreEnvironmentConfig sreEnvConfig = instances.Where(ecf => ecf.InstanceNumber == 0).SingleOrDefault();
                if (sreEnvConfig != null)
                {
                    txtRemoteServer1.Text = sreEnvConfig.RemoteUrl;
                    try
                    {
                        EnvironmentServiceDispatcherUsingWcf.GetWcfClient(sreEnvConfig);
                        btnEnsureInstances.Visible = true;
                    }
                    catch
                    {
                        btnEnsureInstances.Visible = false;
                    }
                }
                else
                {
                    btnEnsureInstances.Visible = false;
                }
                lblInstance1.Visible = sreEnvConfig != null ? true : false;
                txtRemoteServer1.Visible = lblInstance1.Visible;
               

                sreEnvConfig = instances.Where(ecf => ecf.InstanceNumber == 1).SingleOrDefault();
                if (sreEnvConfig != null)
                    txtRemoteServer2.Text = sreEnvConfig.RemoteUrl;
                lblInstance2.Visible = sreEnvConfig != null ? true : false;
                txtRemoteServer2.Visible = lblInstance2.Visible;

                sreEnvConfig = instances.Where(ecf => ecf.InstanceNumber == 2).SingleOrDefault();
                if (sreEnvConfig != null)
                    txtRemoteServer3.Text = sreEnvConfig.RemoteUrl;
                lblInstance3.Visible = sreEnvConfig != null ? true : false;
                txtRemoteServer3.Visible = lblInstance3.Visible;

                sreEnvConfig = instances.Where(ecf => ecf.InstanceNumber == 3).SingleOrDefault();
                if (sreEnvConfig != null)
                    txtRemoteServer4.Text = sreEnvConfig.RemoteUrl;
                lblInstance4.Visible = sreEnvConfig != null ? true : false;
                txtRemoteServer4.Visible = lblInstance4.Visible;

            }
        }

        private void lbEnvs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEnvironment();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedEnvironment == null)
                return;
                      
            SelectedEnvironment.Name = txtName.Text;
            SelectedEnvironment.RootFolder = txtRootFolder.Text;
            SelectedEnvironment.PullFolder = txtPullFolder.Text;
            SelectedEnvironment.WcfMode = radTcpIp.Checked;

            SelectedEnvironment.SetRemoteUrl(0, txtRemoteServer1.Text);
            SelectedEnvironment.SetRemoteUrl(1, txtRemoteServer2.Text);
            SelectedEnvironment.SetRemoteUrl(2, txtRemoteServer3.Text);
            SelectedEnvironment.SetRemoteUrl(3, txtRemoteServer4.Text);
            SelectedEnvironment.SetEnvironmentConfigsPath();
            if (!ValidateData())
                return;

            EnvironmentServiceDispatcherFactory.SaveEnvironment(Envs);
            Bind(SelectedEnvironment);
            
            lbEnvs.Focus();
        }

        private bool ValidateData()
        {
            string errorMessages = string.Empty;      
            foreach(SreEnvironmentConfig config in SelectedEnvironment.EnvironmentConfigsInstancesOnly)
            {
                //validating url
                try
                {
                    EnvironmentServiceDispatcherUsingWcf.GetWcfClient(config);
                }
                catch (Exception ex)
                {
                    errorMessages += ex.Message + " Valid url format is net.tcp://servername:7200/srees" + Environment.NewLine;                   
                }
            }
          
            if (!string.IsNullOrEmpty(errorMessages))
            {
                MessageBox.Show(errorMessages, "Environment - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);               
                return false;
            }
            return true;
        }

        private void lbEnvs_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && (lbEnvs.SelectedIndex > -1))
            {                
                EnvironmentServiceDispatcherFactory.SaveEnvironment(Envs);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "IDPE Patch Files (*.idpep)|*.idpep|All Files (*.*)|*.*";
            openFileDialog1.FileName = "sreenvs.idpep";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch(openFileDialog1.FileName);
                dsp.Import();
                Bind(null);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sreenvs.idpep";
            saveFileDialog1.Filter = "IDPE Patch Files (*.idpep)|*.idpep|All Files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSourcePatch dsp = new DataSourcePatch();
                dsp.Keys.Add(new Manager().GetKey(IdpeKeyTypes.Environments));
                dsp.Export(saveFileDialog1.FileName);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bind(null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtName.Enabled = true;
            txtName.Text = string.Empty;
            txtPullFolder.Text = string.Empty;
            txtRemoteServer1.Text = string.Empty;
            txtRemoteServer2.Text = string.Empty;
            txtRemoteServer3.Text = string.Empty;
            txtRemoteServer4.Text = string.Empty;
            txtRootFolder.Text = string.Empty;
            lblInstance1.Visible = false;
            txtRemoteServer1.Visible = false;
            lblInstance2.Visible = false;
            txtRemoteServer2.Visible = false;
            lblInstance3.Visible = false;
            txtRemoteServer3.Visible = false;
            lblInstance4.Visible = false;
            txtRemoteServer4.Visible = false;
            btnEnsureInstances.Visible = false;

            SreEnvironment env = new SreEnvironment();
            env.LoadDefaultConfigs();
            env.Name = "new";            
            Envs.Add(env);            
            Bind(env, Envs);           
            txtName.Focus();
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            if(radTcpIp.Checked)
            {
                if(SelectedEnvironment.EnvironmentConfigsInstancesOnly.Count == 0)
                {
                    lblInstance1.Visible = true;
                    txtRemoteServer1.Visible = true;
                    txtRemoteServer1.Text = "net.tcp://localhost:7200/srees";
                    txtRemoteServer1.Focus();
                }
            }
            else
            {

            }
        }

        private void btnEnsureInstances_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedEnvironment.CheckAdditionalInstances(txtRemoteServer1.Text);
                BindEnvironment();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Environment - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if ((SelectedEnvironment != null) && (string.IsNullOrEmpty(SelectedEnvironment.RootFolder)))
            {
                if ((lbEnvs.Items.Count == 1) && (((SreEnvironment)lbEnvs.Items[0]).Name == "<local>"))
                {
                    btnSave.Enabled = true;
                    return;
                }
                for (int i = 0; i < lbEnvs.Items.Count; i++)
                {
                    if (((SreEnvironment)lbEnvs.Items[i]).Name == txtName.Text)
                    {
                        btnSave.Enabled = false;
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(txtName.Text))
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }
    }
}


