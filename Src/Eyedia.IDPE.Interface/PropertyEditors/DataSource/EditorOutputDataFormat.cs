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
using System.Windows.Forms.Integration;

namespace Eyedia.IDPE.Interface
{
    public class EditorOutputDataFormat : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = provider.GetService(typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;
            SreDataSourceProperty dsProperty = context.Instance as SreDataSourceProperty;

            if (wfes != null)
            {
                switch (dsProperty.OutputDataFormatType)
                {
                    case OutputTypes.CSharpCode:
                        IdpeKey key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.CSharpCodeOutputWriter.ToString());
                        string code = key != null ? key.Value : string.Empty;
                        CSharpCodeEditor cSharpCodeEditor = new CSharpCodeEditor(code);
                        if (cSharpCodeEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            new Manager().SaveOutputWriterCSharpCode(dsProperty.Id, cSharpCodeEditor.CSharpCodeInformation.RawString);
                            dsProperty.DataSourceKeys = new Manager().GetKeys(dsProperty.Id);
                        }
                        break;

                    case OutputTypes.FixedLength:
                        frmOutputWriterFixedLength outputWriterFixedLength = new frmOutputWriterFixedLength(
                            dsProperty.Id, (int)dsProperty.SystemDataSourceId);
                        outputWriterFixedLength.ShowDialog();
                        break;

                    case OutputTypes.Database:
                        ConfigDatabaseWriter configDatabaseWriter = new ConfigDatabaseWriter(dsProperty.Id, (int)dsProperty.SystemDataSourceId);
                        ElementHost.EnableModelessKeyboardInterop(configDatabaseWriter);
                        configDatabaseWriter.Show();
                        break;

                    case OutputTypes.Custom:
                        return ShowCustomInterfaceEditor(wfes, dsProperty);
                        break;

                }

            }
            return value;
        }

        #region ShowCustomInterfaceEditor
        private string ShowCustomInterfaceEditor(IWindowsFormsEditorService wfes, SreDataSourceProperty dsProperty)
        {
            TypeSelectorDialog activitySelector = new TypeSelectorDialog(typeof(OutputWriter));

            if ((wfes.ShowDialog(activitySelector) == DialogResult.OK)
                && (!String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null))
            {
                string strInterface = string.Format("{0}, {1}", activitySelector.Activity.FullName, Path.GetFileNameWithoutExtension(activitySelector.AssemblyPath));
                dsProperty.OutputCustomInterfaceName = strInterface;
                return strInterface;
            }

            return string.Empty;

        }
        #endregion ShowCustomInterfaceEditor
    }
}


