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
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.Core.Windows.Utilities;
using Eyedia.IDPE.Services;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace Eyedia.IDPE.Interface
{
    public class EditorLovName : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        private IWindowsFormsEditorService _editorService;
        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            _editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;
            SreAttributeProperty aProperty = context.Instance as SreAttributeProperty;

            if (_editorService != null)
            {                
                ListBox lb = new ListBox();
                lb.SelectionMode = SelectionMode.One;
                lb.SelectedValueChanged += OnListBoxSelectedValueChanged;
                DataTable table = Core.Data.CoreDatabaseObjects.Instance.ExecuteCommand("select distinct code from CodeSet") ;
                foreach (DataRow row in table.Rows)
                {                    
                    int index = lb.Items.Add(row["code"].ToString());
                    if (row["code"].Equals(value))
                    {
                        lb.SelectedIndex = index;
                    }
                }

                _editorService.DropDownControl(lb);
                if (lb.SelectedItem == null)
                    return value;

                return lb.SelectedItem;

            }
            return value;
        }

        private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
        {            
            _editorService.CloseDropDown();
        }

    }
}


