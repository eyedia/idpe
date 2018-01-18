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
using Eyedia.IDPE.Common;

namespace Eyedia.IDPE.Interface
{
    public partial class frmOutputWriterFixedLength : Form
    {
        public int DataSourceId { get; private set; }
        public int SystemDataSourceId { get; private set; }

        public frmOutputWriterFixedLength(int dataSourceId, int systemDataSourceId, Icon icon = null)
        {
            InitializeComponent();
            ;

            DataSourceId = dataSourceId;
            SystemDataSourceId = systemDataSourceId;
            if (icon != null)
                this.Icon = icon;

            Manager manager = new Manager();
            List<IdpeAttribute> attributes = manager.GetAttributes(SystemDataSourceId);
            string[] attributeNames = attributes.Where(aa => aa.IsAcceptable == true).Select(a => a.Name).ToArray();
            fixedLengthSchemaGenerator1.Attributes = attributeNames;
            IdpeKey key = manager.GetKey(DataSourceId, IdpeKeyTypes.FixedLengthSchemaOutputWriter.ToString());
            fixedLengthSchemaGenerator1.Schema = (key != null) ? key.Value : string.Empty;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IdpeKey key = new IdpeKey();
                key.Name = IdpeKeyTypes.FixedLengthSchemaOutputWriter.ToString();
                key.Value = fixedLengthSchemaGenerator1.Schema;
                key.Type = (int)IdpeKeyTypes.FixedLengthSchemaOutputWriter;
                new Manager().Save(key, DataSourceId);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


