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
using Eyedia.IDPE.Services;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Data;

namespace Eyedia.IDPE.Interface
{
    public partial class ValidateDataSource : Form
    {
        public int DataSourceId { get; private set; }
        public int ParentDataSourceId { get; private set; }
        public string DataSourceName { get; private set; }
        public string ParentDataSourceName { get; private set; }

        int OrginialHeight;
        bool CancelRequested;

        public ValidateDataSource(int dataSourceId, Icon icon = null)
        {
            InitializeComponent();
            ;

            OrginialHeight = this.Height;
            this.DataSourceId = dataSourceId;
            
            int titleHeight = RectangleToScreen(this.ClientRectangle).Top - this.Top;
            this.Height = titleHeight + pnlTop.Height + statusStrip1.Height + 3;

            if (icon != null)
                this.Icon = icon;
        }

        private void ValidateDataSource_Load(object sender, EventArgs e)
        {
            timerStart.Enabled = true;
        }

        private void ValidateDataSource_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (btnOK.Text == "&OK")
                this.Close();
            //else
             //   CancelRequested = true;
        }
        private void timerStart_Tick(object sender, EventArgs e)
        {
            timerStart.Enabled = false;
            if (!Validate())
                ShowError();
        }

        private void ShowError()
        {
            this.Height = OrginialHeight;
            btnOK.Text = "&OK";
        }

        private void AddError(string errorMessage)
        {
            rtbErrors.AppendText(errorMessage + "\u2028");            
        }

        private void ShowMessage(string message)
        {
            toolStripStatusLabel1.Text = message;
            Application.DoEvents();
        }

        private bool Validate()
        {
            ShowMessage("Validating data source details...");
            Manager manager = new Manager();
            ParentDataSourceId = manager.GetDataSourceParentId(DataSourceId);
            DataSourceName = manager.GetApplicationName(DataSourceId);
            ParentDataSourceName = manager.GetApplicationName(ParentDataSourceId);
            this.Text = string.Format("Validating {0} and its parent {1}", DataSourceName, ParentDataSourceName);

            ShowMessage("Validating codesets...");
            ValidateCodeSets();
            ShowMessage("Codesets validated.");

            if (rtbErrors.Text == string.Empty)
            {
                ShowMessage("Validation completed.");
                btnOK.Text = "&OK";
                return true;
            }
            else
            {
                ShowMessage("Validation failed! Please study these errors and make neccessary changes.");
                return false;
            }
        }

        private void ValidateCodeSets()
        {
            Manager manager = new Manager();
            List<SreAttribute> codeSetAttributes = manager.GetAttributes(DataSourceId);
            codeSetAttributes = codeSetAttributes.Where(a => a.Type == "CODESET").ToList();
            List<SymplusCodeSet> systemCodeSets = CoreDatabaseObjects.Instance.GetCodeSets();
            foreach (SreAttribute codeSetAttribute in codeSetAttributes)
            {
                ValidateCodeSet(codeSetAttribute, systemCodeSets);
            }
            
            if (ParentDataSourceId != 0)
            {
                codeSetAttributes = manager.GetAttributes(ParentDataSourceId);
                codeSetAttributes = codeSetAttributes.Where(a => a.Type == "CODESET").ToList();

                foreach (SreAttribute codeSetAttribute in codeSetAttributes)
                {
                    ValidateCodeSet(codeSetAttribute, systemCodeSets);
                }
            }
        }

        private void ValidateCodeSet(SreAttribute codeSetAttribute, List<SymplusCodeSet> systemCodeSets)
        {
            ShowMessage(string.Format("Validating codesets({0})...", codeSetAttribute.Name));

            try
            {
                string code = SreCodeset.ParseFormulaGetCode(codeSetAttribute.Formula);

                List<SymplusCodeSet> thisCodeSet = (from cs in systemCodeSets
                                                    where (cs.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
                                                    select cs).ToList();

                if (thisCodeSet.Count == 0)
                    throw new Exception(string.Format("Codeset '{0}' of attribute '{1}' is not defined!", code, codeSetAttribute.Name));
                         
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("Validating codesets({0})...Failed", codeSetAttribute.Name));
                AddError(ex.Message);                
            }
        }


      
    }
}


