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
using Eyedia.IDPE.DataManager;
using System.Reflection;

namespace Eyedia.IDPE.Interface
{
    public partial class RuleWizards : Form
    {
        public SreDataSource SelectedDataSource { get; private set; }
        bool FormActivated;        

        public RuleWizards(Icon icon = null)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            if (icon != null)
                this.Icon = icon;

            cbWizards.SelectedIndex = 0;
            cbDataSources.DataSource = new Manager().GetDataSources(1).OrderBy(ds => ds.Name).ToList();
            cbDataSources.DisplayMember = "Name";
            attributeMapperRuleWizard1.Dock = DockStyle.Fill;
        }
       
        void BindData()
        {
            switch (cbWizards.SelectedIndex)
            {
                case 0:
                    attributeMapperRuleWizard1.BindData(SelectedDataSource.Id, (int)SelectedDataSource.SystemDataSourceId);
                    break;
            }
        }       

        private void cbDataSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDataSource = cbDataSources.SelectedItem as SreDataSource;            
            if(FormActivated)
                BindData();
        }     
       
        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (cbWizards.SelectedIndex)
            {
                case 0:
                    if (attributeMapperRuleWizard1.Save(SelectedDataSource.Name))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmWizMapping_Activated(object sender, EventArgs e)
        {
            FormActivated = true;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            switch (cbWizards.SelectedIndex)
            {
                case 0:
                    attributeMapperRuleWizard1.ShowHelp();
                    break;
            }
        }    

        
    }
   
}


