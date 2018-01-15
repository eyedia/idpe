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
using Eyedia.Core;
using System.Threading.Tasks;

namespace Eyedia.IDPE.Interface
{
    public partial class MainWindow : SREBaseFormNew
    {
        
        public MainWindow(string[] args)
        {
            InitializeComponent();

            #region Splash

            Splash splash = new Splash(args, this);
            if (splash.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                Environment.Exit(101);

            //Task.Run(async () =>
            //{
            //    var MyValue = await splash.SplashCompleted;

            //});
            #endregion Splash

            Init(args);
            InitCustomActionMenu();
            //GetSchema();
        }

        void Init(string[] args)
        {
            if (Information.LoggedInUser != null)            
                maintenanceToolStripMenuItem.Visible = Information.LoggedInUser.IsAdmin();            
            else            
                maintenanceToolStripMenuItem.Visible = true;
            
            if(EyediaCoreConfigurationSection.CurrentConfig.Debug)
                maintenanceToolStripMenuItem.Visible = true;

            #region When a file double clicked

            if (args.Length > 0)
            {
                //a file has been double clicked
                string fileExtension = Path.GetExtension(args[0]).ToLower();
                try
                {
                    switch (fileExtension)
                    {

                        case ".srex":
                            if (MessageBox.Show("Do you want to import " + args[0] + " ?", "Import Datasource",
                                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                            {
                                DataSourceBundle dsb = new DataSourceBundle(args[0]);
                                dsb.Import();
                            }
                            break;

                        case ".srep":
                            if (MessageBox.Show("Do you want to import " + args[0] + " ?", "Patch",
                                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                            {
                                DataSourcePatch dsp = new DataSourcePatch(args[0]);
                                dsp.Import();
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    switch (fileExtension)
                    {

                        case ".srex":
                            MessageBox.Show("An error occurred while importing data source from " + args[0] + Environment.NewLine + ex.ToString(), "Import Datasource",
                                 MessageBoxButtons.OK, MessageBoxIcon.Question);
                            break;

                        case ".srep":
                            MessageBox.Show("An error occurred while importing from " + args[0] + Environment.NewLine + ex.ToString(), "Patch",
                                        MessageBoxButtons.OK, MessageBoxIcon.Question);
                            break;
                    }
                }
            }

            #endregion When a file double clicked

        }

        #region Properties

        string ConnectedTo
        {
            get
            {
                try
                {
                    DatabaseTypes dataType = DatabaseTypes.SqlCe;
                    string dbName = string.Empty;
                    if (dataType == DatabaseTypes.SqlCe)
                    {
                        dbName = ConfigurationManager.ConnectionStrings["cs"].ConnectionString.Split(";".ToCharArray())[0].Split("=".ToCharArray())[1];
                        dbName = string.Format("Connected to:{0}", dbName);
                    }
                    else if (dataType == DatabaseTypes.SqlServer)
                    {
                        dbName = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
                        dbName = dbName.Substring(dbName.IndexOf("Initial Catalog=") + 16);
                        dbName = dbName.Substring(0, dbName.IndexOf(";"));
                        dbName = string.Format("Connected to:{0}", dbName);
                    }
                    else
                    {
                        dbName = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
                    }
                    return dbName;

                }
                catch { return string.Empty; }
            }
        }

        public bool CancelPressed;

        #endregion Properties

        #region MainWindow Events

        bool MainWindowShown;
        private void MainWindow_Shown(object sender, EventArgs e)
        {
            MainWindowShown = true;
            //toolStripStatusLabel1.Text = "Ready";
            toolStripStatusLabel3.Text = ConnectedTo;
            if (Information.LoggedInUser != null)
            {
                if (!string.IsNullOrEmpty(Information.LoggedInUser.FullName))
                    toolStripStatusLabel2.Text = Information.LoggedInUser.FullName;
                else
                    toolStripStatusLabel2.Text = Information.LoggedInUser.UserName;
            }
            
            m_dm_DataSources.Activate();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    CancelPressed = true;
                    break;

                case Keys.F4:
                    MessageBox.Show("F4");
                    break;
            }
        }       
       
        #endregion MainWindow Events

        #region Static Methods
        public static Control FindFocusedControl(Control control)
        {
            var container = control as ContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as ContainerControl;
            }
            return control;
        }

        #endregion Static Methods

        internal void ImportAttributes(SreListView sreListView, bool isSytem = false)
        {

            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Comma Delimited Files (*.csv)|*.csv|All Files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable table = m_dm_GlobalAttributes.ReadData(openFileDialog1.FileName);
                List<IdpeAttribute> attributes = new List<IdpeAttribute>();
                string result = m_dm_GlobalAttributes.ImportAttributes(table, ref attributes);
                if (!isSytem)
                {
                    m_dm_Attributes.sreListView1.Attributes = attributes;
                    SaveAssociations(m_dm_Attributes.sreListView1.ListView, isSytem);
                }
                else
                {
                    m_dm_SystemAttributes.sreListView1.Attributes = attributes;
                    SaveAssociations(m_dm_SystemAttributes.sreListView1.ListView, isSytem);
                }
                this.Cursor = Cursors.Default;
                MessageBox.Show(result, "Bulk Insert", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

        internal void ExportAttributes(SreListView sreListView, bool isSytem = false)
        {
            saveFileDialog1.FileName = "attributes.csv";
            saveFileDialog1.Filter = "Comma Delimited Files (*.csv)|*.csv|All Files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                sw.WriteLine("\"Name\",\"Type\",\"Formula\"");

                foreach (ListViewItem item in sreListView.ListView.Items)
                {
                    IdpeAttribute attrib = item.Tag as IdpeAttribute;
                    sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"", attrib.Name, attrib.Type, attrib.Formula));
                }
                sw.Close();
            }
        }
        
    }
}


