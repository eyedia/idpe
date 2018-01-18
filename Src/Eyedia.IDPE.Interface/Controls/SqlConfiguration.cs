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
using System.Windows.Forms;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Windows.Utilities;
using Eyedia.IDPE.Services;
using System.IO;

namespace Eyedia.IDPE.Interface.Controls
{
    public partial class SqlConfiguration : UserControl
    {
        #region Properties
        bool DoNotClose;
        int _dataSourceId;       
        public int DataSourceId 
        { 
            get
            {
                return _dataSourceId;
            } 
            set 
            {
                _dataSourceId = value;
                BindData(); 
            } 
        }

        public string DataSourceName { get; private set; }

        string AcceptableAttributeNames { get; set; }

        string PullSqlReturnType
        {
            get { return radInputData.Checked ? "D" : "I"; }
            set
            {
                radInputData.Checked = ((value == "D") || (value == "")) ? true : false;
                radInterface.Checked = value == "I" ? true : false;
            }
        }

        bool _PusherMode;
        public bool PusherMode
        {
            get { return _PusherMode; }
            set
            {
                _PusherMode = value;

                lblInterval.Visible = !_PusherMode;
                numUpDnIntervalSql.Visible = !_PusherMode;
                lblMinutes.Visible = !_PusherMode;
                pnlQuerySelect.Visible = !_PusherMode;             
                pnlTestButton1.Visible = !_PusherMode;
                pnlQueryRecovery.Visible = !_PusherMode;
                pnlTestButton3.Visible = !_PusherMode;
                pnlOthers.Visible = !_PusherMode;

                if (_PusherMode)
                {                    
                    lblUpdateQuery.Text = "Query";
                    lblUpdateQuery.TextAlign = ContentAlignment.MiddleLeft;
                }
                else
                {                    
                    lblUpdateQuery.Text = "Update Query (to mark data to 'Processing' state)";
                    lblUpdateQuery.TextAlign = ContentAlignment.MiddleCenter;
                }
            }
        }

        public string ConnectionStringKeyNameDesign
        {
            get { return cmbConnectionString.Text; }
            set { try { cmbConnectionString.Text = value; } catch { } }
        }

        public string ConnectionStringKeyName
        {
            get { return txtRunTimeCononectionStringName.Text; }
            set { txtRunTimeCononectionStringName.Text = value; }
        }
        
        public string UpdateQuery
        {
            get { return txtQueryUpdate.Text; }
            set 
            {
                if (string.IsNullOrEmpty(txtQueryUpdate.Text))
                {
                    txtQueryUpdate.Text = value;
                    SyntaxHighLighter.HighLight(txtQueryUpdate);
                }
            }
        }
        
        #endregion Properties

        public SqlConfiguration()
        {
            InitializeComponent();           
        }      

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DataSourceId == 0)
                return;
            

            if (PusherMode == false)
            {
                if (!IsValid())
                    return;

                this.Cursor = Cursors.WaitCursor;
                if (chkTestQueries.Checked)
                {
                    ValidateSelectQuery();
                    ValidateUpdateQuery(txtQueryUpdate.Text);
                    ValidateUpdateQuery(txtQueryRecovery.Text);
                }
            
                if (radInputData.Checked)
                {
                    new Manager().SavePullSqlConfig(DataSourceId, cmbConnectionString.Text,
                        radInputData.Checked, txtQuerySelect.Text, txtQueryUpdate.Text, txtQueryRecovery.Text,
                        txtInterfaceName.Text, (int)numUpDnIntervalSql.Value, true, txtRunTimeCononectionStringName.Text);

                }
                else
                {
                    new Manager().SavePullSqlConfig(DataSourceId, cmbConnectionString.Text,
                        radInputData.Checked, txtQuerySelect.Text, txtQueryUpdate.Text, txtQueryRecovery.Text,
                        txtInterfaceName.Text, (int)numUpDnIntervalSql.Value, chkFirstRowIsHeader.Checked, txtRunTimeCononectionStringName.Text);
                }            
                this.Cursor = Cursors.Default;
                MessageBox.Show("Configuration saved successfully!", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UserPreferences userPreferences = new UserPreferences(Information.LoggedInUser.Preferences);
                userPreferences.InputFeedPullFromDatabaseTestQuery = chkTestQueries.Checked;
                Information.LoggedInUser.Preferences = userPreferences.Serialize();
                CoreDatabaseObjects.Instance.UpdateUserPreferences(Information.LoggedInUser);
                
            }
            else
            {
                //save will be taken care by caller
            }

            //if ((DoNotClose == false)
            //    && (ParentForm != null))
            //    ParentForm.Close();                
        }

        bool IsValid()
        {
            txtQuerySelect_Validated(null, null);
            txtQueryUpdate_Validated(null, null);
            txtQueryRecovery_Validated(null, null);
            if (errorProvider1.GetError(txtQuerySelect) != "")
                return false;

            if (errorProvider1.GetError(txtQueryUpdate) != "")
                return false;

            if (errorProvider1.GetError(txtQueryRecovery) != "")
                return false;

            return true;
        }

        private void btnCreateConnections_Click(object sender, EventArgs e)
        {
            frmSreConnections connectionStrings = new frmSreConnections(DataSourceId);
            connectionStrings.ShowDialog();
            cmbConnectionString.DataSource = new Manager().GetDataSourceKeysConnectionStrings(DataSourceId, false);
            cmbConnectionString.DisplayMember = "Name";
        }

        private void txtQuerySelect_DoubleClick(object sender, EventArgs e)
        {
            TextArea textArea = new TextArea();
            textArea.txtContent.Text = txtQuerySelect.Text;
            if (textArea.ShowDialog() == DialogResult.OK)
                txtQuerySelect.Text = textArea.txtContent.Text;
        }

        private void txtQuerySelect_TextChanged(object sender, EventArgs e)
        {            
            btnTestSqlQuery.Enabled = string.IsNullOrEmpty(txtQuerySelect.Text) ? false : true;
            SyntaxHighLighter.HighLight(sender as RichTextBox);
        }

        private void btnTestSqlQuery_Click(object sender, EventArgs e)
        {
            ValidateSelectQuery(true);
        }

        private void txtQueryUpdate_TextChanged(object sender, EventArgs e)
        {           
            btnTestUpdateQuery.Enabled = string.IsNullOrEmpty(txtQueryUpdate.Text) ? false : true;
            SyntaxHighLighter.HighLight(sender as RichTextBox);
        }


        private void btnTestUpdateQuery_Click(object sender, EventArgs e)
        {
            ValidateUpdateQuery(txtQueryUpdate.Text, true);
        }

        private void txtQueryRecovery_TextChanged(object sender, EventArgs e)
        {            
            btnTestRecoveryQuery.Enabled = string.IsNullOrEmpty(txtQueryRecovery.Text) ? false : true;
            SyntaxHighLighter.HighLight(sender as RichTextBox);
        }

        private void btnTestRecoveryQuery_Click(object sender, EventArgs e)
        {
            ValidateUpdateQuery(txtQueryRecovery.Text, true);
        }

        private void radInputData_CheckedChanged(object sender, EventArgs e)
        {
            if (radInterface.Checked)
            {
                label15.Visible = true;
                txtInterfaceName.Visible = true;
                btnInterface.Visible = true;
                chkFirstRowIsHeader.Visible = true;
            }
            else
            {
                label15.Visible = false;
                txtInterfaceName.Visible = false;
                btnInterface.Visible = false;
                chkFirstRowIsHeader.Visible = false;
            }
        }

        private void btnInterface_Click(object sender, EventArgs e)
        {
            try
            {
                TypeSelectorDialog activitySelector = new TypeSelectorDialog(typeof(InputFileGenerator));

                if ((activitySelector.ShowDialog() == DialogResult.OK) && (!String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null))
                {
                    txtInterfaceName.Text = string.Format("{0}, {1}", activitySelector.Activity.FullName, Path.GetFileNameWithoutExtension(activitySelector.AssemblyPath));                    

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (ParentForm != null)
                ParentForm.Close();
        }

        #region Private Methods

        private void BindData()
        {
            if ((this.DesignMode)
             || (DataSourceId == 0))
                return;

            UserPreferences userPreferences = new UserPreferences(Information.LoggedInUser.Preferences);
            chkTestQueries.Checked = userPreferences.InputFeedPullFromDatabaseTestQuery;

            cmbConnectionString.DataSource = new Manager().GetDataSourceKeysConnectionStrings(DataSourceId, false);
            cmbConnectionString.DisplayMember = "Name";

            BindingList<Eyedia.IDPE.Interface.Data.Attribute> allAttributes = new Data.UtilDataManager().GetAttributes(DataSourceId);
            if (allAttributes != null)
            {
                List<string> attributeNames = allAttributes.Select(a => a.Name).ToList();
                AcceptableAttributeNames = string.Join(",", attributeNames.ToArray());
            }

            if (PusherMode == false)
                BindDataPullSql();
         
        }

        private void BindDataPullSql()
        {

            List<IdpeKey> keys = new Manager().GetApplicationKeys(DataSourceId, false);

            cmbConnectionString.Text = keys.GetKeyValue(IdpeKeyTypes.PullSqlConnectionString);
            txtRunTimeCononectionStringName.Text = keys.GetKeyValue(IdpeKeyTypes.PullSqlConnectionStringRunTime);
            string strSqlWatchInterval = keys.GetKeyValue(IdpeKeyTypes.SqlWatchInterval);
            numUpDnIntervalSql.Value = (decimal)(!string.IsNullOrEmpty(strSqlWatchInterval) ? strSqlWatchInterval.ParseInt() : 2);
            PullSqlReturnType = keys.GetKeyValue(IdpeKeyTypes.PullSqlReturnType);
            txtQuerySelect.Text = keys.GetKeyValue(IdpeKeyTypes.SqlQuery);
            txtQueryUpdate.Text = keys.GetKeyValue(IdpeKeyTypes.SqlUpdateQueryProcessing);
            txtQueryRecovery.Text = keys.GetKeyValue(IdpeKeyTypes.SqlUpdateQueryRecovery);
            chkFirstRowIsHeader.Checked = bool.Parse(keys.GetKeyValue(IdpeKeyTypes.IsFirstRowHeader));
            txtInterfaceName.Text = keys.GetKeyValue(IdpeKeyTypes.PullSqlInterfaceName);
            toolTip.SetToolTip(txtInterfaceName, keys.GetKeyValue(IdpeKeyTypes.PullSqlInterfaceName));

            SyntaxHighLighter.HighLight(txtQuerySelect);
            SyntaxHighLighter.HighLight(txtQueryUpdate);
            SyntaxHighLighter.HighLight(txtQueryRecovery);
        }

        void ValidateSelectQuery(bool testing = false)
        {
            DataSource ds = new DataSource(DataSourceId, string.Empty);
            IdpeKey connectionStringKey = cmbConnectionString.SelectedItem as IdpeKey;
            DatabaseTypes databaseType = connectionStringKey.GetDatabaseType();
            string actualConnectionString = connectionStringKey.Value;

            IDal myDal = new DataAccessLayer(databaseType).Instance;
            IDbConnection conn = myDal.CreateConnection(actualConnectionString);
            IDbCommand command = myDal.CreateCommand();
            DataTable table = new DataTable();

            try
            {
                command.CommandText = new CommandParser(ds).Parse(txtQuerySelect.Text);

                conn.Open();
                command.Connection = conn;
                IDataReader reader = command.ExecuteReader();
                table.Load(reader);

                string columns = string.Join(",", table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray());

                if (!columns.Equals(AcceptableAttributeNames, StringComparison.OrdinalIgnoreCase))
                {
                    if (!testing)
                    {

                        MessageBox.Show("Invalid Query! The return columns should be exactly same as 'acceptable attributes'! (" + AcceptableAttributeNames + ")",
                                  "Invalid Select Query", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                        //DoNotClose = true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid query! The return columns should be exactly same as 'acceptable attributes'! (" + AcceptableAttributeNames + ")",
                                  "Invalid Select Query", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       // DoNotClose = true;
                    }

                }
                else
                {
                    if (testing)
                        MessageBox.Show("Successful!", "Test SQL Query", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid select query! " + ex.Message,
                               "Invalid Select Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //DoNotClose = true;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }

        }

        void ValidateUpdateQuery(string updateQuery, bool testing = false)
        {
            if (string.IsNullOrEmpty(updateQuery))
                return;

            DataSource ds = new DataSource(DataSourceId, string.Empty);
            updateQuery = new CommandParser(ds).Parse(updateQuery);
            //DoNotClose = false;
            IdpeKey connectionStringKey = cmbConnectionString.SelectedItem as IdpeKey;
            DatabaseTypes databaseType = connectionStringKey.GetDatabaseType();
            string actualConnectionString = connectionStringKey.Value;
            IDbConnection conn = null;
            IDbTransaction transaction = null;
            IDbCommand command = null;
            try
            {
                IDal myDal = new DataAccessLayer(databaseType).Instance;
                conn = myDal.CreateConnection(actualConnectionString);
                conn.Open();
                transaction = myDal.CreateTransaction(conn);
                command = myDal.CreateCommand();
                command.Connection = conn;
                command.Transaction = transaction;
                command.CommandText = updateQuery;

                if (conn.State != ConnectionState.Open) conn.Open();
                command.ExecuteNonQuery();
                transaction.Rollback();
                if (testing)
                    MessageBox.Show("Successful!", "Test SQL Update Query", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                if(transaction != null)
                    transaction.Rollback();
                //DoNotClose = true;
                MessageBox.Show("Invalid update query! " + ex.Message,
                              "Invalid Update or Recovery Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
                if (transaction != null)
                    transaction.Dispose();
                if (command != null)
                    command.Dispose();
            }
        }


        #endregion Private Methods

        private void txtQuerySelect_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtQuerySelect.Text))
                errorProvider1.SetError(this.txtQuerySelect, "Select query is required!");
            else
                errorProvider1.SetError(this.txtQuerySelect, string.Empty);
        }

        private void txtQueryUpdate_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtQueryUpdate.Text))
                errorProvider1.SetError(this.txtQueryUpdate, "Update query is required!");
            else
                errorProvider1.SetError(this.txtQueryUpdate, string.Empty);
        }

        private void txtQueryRecovery_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtQueryRecovery.Text))
                errorProvider1.SetError(this.txtQueryRecovery, "Recovery query is required!");
            else
                errorProvider1.SetError(this.txtQueryRecovery, string.Empty);
        }

        private void txtQuery_Enter(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}


