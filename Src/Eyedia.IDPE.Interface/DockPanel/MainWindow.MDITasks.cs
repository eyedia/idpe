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
using Eyedia.Core.Data;
using System.Diagnostics;
using Eyedia.Core.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Eyedia.IDPE.Interface.Controls;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public partial class MainWindow : SREBaseFormNew
    {
        #region Menu

        #region Custom Actions
        private void mnItmManageActions_Click(object sender, EventArgs e)
        {
            ManageActions manageActions = new ManageActions(this.Icon);
            if (manageActions.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                InitCustomActionMenu();
        }

        private void OnCustomAction(object sender, EventArgs e)
        {
            ToolStripMenuItem mnuItem = sender as ToolStripMenuItem;
            if (mnuItem != null)
            {
                string output = DosCommands.Execute(mnuItem.Tag.ToString());
                SetToolStripStatusLabel("Executed " + mnuItem.Text, true);
            }
        }

        private void InitCustomActionMenu()
        {
            mnItmManageAction1.Visible = false;
            mnItmManageAction2.Visible = false;
            mnItmManageAction3.Visible = false;
            mnItmManageAction4.Visible = false;
            mnItmManageAction5.Visible = false;

            SreKey key = new Manager().GetKey(SreKeyTypes.CustomActions);
            if (key != null)
            {
                if (!key.Value.Contains("|"))
                {
                    mnItmManageAction1.Tag = key.Value;
                    mnItmManageAction1.Visible = true;
                }
                else
                {
                    string[] actions = key.Value.Split("|".ToCharArray());
                    int counter = 1;
                    foreach (string action in actions)
                    {

                        if (counter == 1)
                        {
                            mnItmManageAction1.Tag = action;
                            mnItmManageAction1.Visible = true;
                        }
                        else if (counter == 2)
                        {
                            mnItmManageAction2.Tag = action;
                            mnItmManageAction2.Visible = true;
                        }
                        else if (counter == 3)
                        {
                            mnItmManageAction3.Tag = action;
                            mnItmManageAction3.Visible = true;
                        }
                        else if (counter == 4)
                        {
                            mnItmManageAction4.Tag = action;
                            mnItmManageAction4.Visible = true;
                        }
                        else if (counter == 5)
                        {
                            mnItmManageAction5.Tag = action;
                            mnItmManageAction5.Visible = true;
                        }

                        counter++;
                    }
                }
            }
        }
        #endregion Custom Actions

        #region Tools

        private void mnItmSystemDataSources_Click(object sender, EventArgs e)
        {
            SystemDataSources systemDataSources = new SystemDataSources(this.Icon);
            systemDataSources.Show();          
        } 
       

        private void mnItmInstanceManager_Click(object sender, EventArgs e)
        {
          
        }

        private void mnItmUserManager_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            UserManager userManager = new UserManager();
            //userManager.MdiParent = this;
            userManager.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void mnItmChangePassword_Click(object sender, EventArgs e)
        {
            ChangePassword passwordForm = new ChangePassword(Information.LoggedInUser);
            passwordForm.ShowDialog();
        }
     
        private void mnItmImportDataSource_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "SRE Files (*.srex)|*.srex|All Files (*.*)|*.*";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Manager manager = new Manager();
                    foreach (String file in openFileDialog1.FileNames)
                    {
                        DataSourceBundle dsb = new DataSourceBundle(file);
                        if (manager.ApplicationExists(dsb.DataSource.Name))
                        {
                            if (MessageBox.Show("A data source with name '" + dsb.DataSource.Name + "' already exist! Do you want to overwrite?", "Import Data Source"
                                , MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                manager.DeleteDataSource(dsb.DataSource.Name);
                        }
                        else
                        {
                            continue;
                        }

                        dsb.Import();
                    }

                    MessageBox.Show("Import was successful!", "Import Data Sources", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Import was failed!", "Import Data Sources", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mnItmImportKeys_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "SRE Patch Files (*.srep)|*.srep|All Files (*.*)|*.*";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    foreach (String file in openFileDialog1.FileNames)
                    {
                        DataSourcePatch dsp = new DataSourcePatch(file);
                        dsp.Import();
                    }

                    MessageBox.Show("Import was successful!", "Import Keys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Import was failed!", "Import Keys", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mnItmClearCache_Click(object sender, EventArgs e)
        {
            ClearCache clearCache = new ClearCache();
            clearCache.ShowDialog();
        }

        private void mnItmOptions_Click(object sender, EventArgs e)
        {
            Preferences prefUI = new Preferences(this.Icon);
            prefUI.ShowDialog();
        }

        #endregion Tools

        #region Window
        private void mnItmWindowNew_Click(object sender, EventArgs e)
        {

        }

        private void mnItmWindowCascade_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void mnItmWindowTileVertical_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);         
        }

        private void mnItmWindowTileHorizontal_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mnItmWindowCloseAll_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }            
        }

        private void mnItmWindowArrangeIcons_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }
        #endregion Window

        #region Help
        private void mnItmHelpContents_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            path = "file://" + Path.Combine(path, "Help.chm");
            Help.ShowHelp(this, path);
        }

        private void mnItmHelpIndex_Click(object sender, EventArgs e)
        {

        }

        private void mnItmHelpSearch_Click(object sender, EventArgs e)
        {

        }

        private void mnItmHelpAbout_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
     
        }
        #endregion Help

        #endregion Menu       

    }
}


