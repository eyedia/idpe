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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Eyedia.Core.Data;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{
    public partial class GenerateDatabaseCode : Form
    {
        public GenerateDatabaseCode()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            cbDatabaseTypes.Items.AddRange(new string[] { "Microsoft SQL Server", "Oracle", "DB2", "Microsoft SQL Compact Edition" });
        }

        private void btnGenerateDuplicateCheck_Click(object sender, EventArgs e)
        {
            DatabaseTypes databaseType = DatabaseTypes.SqlServer;
            switch (cbDatabaseTypes.Text)
            {

                case "Oracle":
                    databaseType = DatabaseTypes.Oracle;
                    break;

                case "DB2":
                    databaseType = DatabaseTypes.DB2iSeries;
                    break;

                case "Microsoft SQL Compact Edition":
                    databaseType = DatabaseTypes.SqlCe;
                    break;

                case "Microsoft SQL Server":
                    databaseType = DatabaseTypes.SqlServer;
                    break;

                default:
                    return;
            }

           //this should be data source and not system
            SystemDataSources sds = new SystemDataSources(this.Icon);
            if (sds.ShowDialog() == DialogResult.OK)
            {
                IdpeKey uniquenessKey = new Manager().GetKey(sds.SelectedSystemDataSource.Id, "UniquenessCriteria");
                if ((uniquenessKey != null)
                    && (!string.IsNullOrEmpty(uniquenessKey.Value)))
                {
                    btnGenerateDuplicateCheck.Enabled = true;
                    toolTip1.SetToolTip(btnGenerateDuplicateCheck, "Generates store procedure code");
                    errorProvider1.SetError(btnGenerateDuplicateCheck, string.Empty);
                }
                else
                {
                    btnGenerateDuplicateCheck.Enabled = false;
                    string errorMsg = "The 'UniquenessCriteria' key is not defined";
                    toolTip1.SetToolTip(btnGenerateDuplicateCheck, errorMsg);
                    errorProvider1.SetError(btnGenerateDuplicateCheck, errorMsg);
                }

                string strCode = new Manager().GenerateDuplicateCheckStoreProcedureCode(sds.SelectedSystemDataSource.Id, databaseType);
                TextArea textArea = new TextArea("Duplicate Check Store Procedure Code");
                textArea.txtContent.Text = strCode;
                textArea.ShowDialog();
            }
        }



        private void btnShowInstructions1_Click(object sender, EventArgs e)
        {
            string strInstructions = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Interface.EmbeddedResources.DuplicateCheckInstructions.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                strInstructions = reader.ReadToEnd();
            }

            TextArea textArea = new TextArea("Duplicate Check Store Procedure Code");
            textArea.txtContent.Text = strInstructions;
            textArea.ShowDialog();
        }
    }
}


