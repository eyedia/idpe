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

namespace Eyedia.IDPE.Interface
{
    public class EditorPostEvents : UITypeEditor
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
                switch (dsProperty.PostEvent)
                {
                    case PusherTypes.DosCommands:
                        value = ShowDOSCommandEditor(wfes, dsProperty);
                        break;

                    case PusherTypes.Ftp:
                        return ShowFtpEditor(wfes, dsProperty);
                        break;

                    case PusherTypes.SqlQuery:
                        return ShowSqlEditor(wfes, dsProperty);
                        break;

                    case PusherTypes.Custom:
                        value = ShowCustomInterfaceEditor(wfes, dsProperty);
                        break;

                }
            }
            return value;
        }

        #region ShowDOSCommandEditor
        private string ShowDOSCommandEditor(IWindowsFormsEditorService wfes, SreDataSourceProperty dsProperty)
        {
            TextArea textArea = new TextArea("Configure Push DOS Commands");
            if ((dsProperty.PusherTypeFullName != null)
                && (!dsProperty.PusherTypeFullName.StartsWith("ftp://")))
                textArea.txtContent.Text = dsProperty.PusherTypeFullName;

            string sreDosHelpCommands = string.Empty;

            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.IDPE.Interface.EmbeddedResources.SreDosCommandsHelp.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                sreDosHelpCommands = reader.ReadToEnd();
            }

            textArea.HintContent1 = sreDosHelpCommands;
            textArea.HintContent1WindowTitle = "IDPE DOS Commands Help";

            if (textArea.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (dsProperty.PusherTypeFullName == null)
                {
                    dsProperty.PusherTypeFullName = textArea.txtContent.Text;
                    return dsProperty.PusherTypeFullName;
                }
                else if (dsProperty.PusherTypeFullName != textArea.txtContent.Text)
                {
                    dsProperty.PusherTypeFullName = textArea.txtContent.Text;
                    return dsProperty.PusherTypeFullName;
                }
            }
            return string.Empty;
        }

        #endregion ShowDOSCommandEditor

        #region ShowFtpEditor
        private string ShowFtpEditor(IWindowsFormsEditorService wfes, SreDataSourceProperty dsProperty)
        {
            frmFtpConfiguration ftpConfig = new frmFtpConfiguration();
            if (dsProperty.PusherTypeFullName.Contains("|"))
            {
                if (dsProperty.PusherTypeFullName.Split("|".ToCharArray()).Length == 3)
                {
                    ftpConfig.FtpRemoteLocation = dsProperty.PusherTypeFullName.Split("|".ToCharArray())[0];
                    ftpConfig.FtpUserName = dsProperty.PusherTypeFullName.Split("|".ToCharArray())[1];
                    ftpConfig.FtpPassword = dsProperty.PusherTypeFullName.Split("|".ToCharArray())[2];
                }
            }
            if (ftpConfig.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string updatedFtpDetails = string.Format("{0}|{1}|{2}",
                    ftpConfig.FtpRemoteLocation, ftpConfig.FtpUserName, ftpConfig.FtpPassword);

                if (dsProperty.PusherTypeFullName != updatedFtpDetails)
                {
                    dsProperty.PusherTypeFullName = updatedFtpDetails;

                }
                return dsProperty.PusherTypeFullName;
            }
            return string.Empty;
        }
        #endregion ShowFtpEditor

        #region ShowSqlEditor
        private string ShowSqlEditor(IWindowsFormsEditorService wfes, SreDataSourceProperty dsProperty)
        {
            frmConfigSql configSql = new frmConfigSql(dsProperty.Id, true, "Push Sql Configuration");
            if ((dsProperty.PusherTypeFullName != null) && 
                (dsProperty.PusherTypeFullName.Contains("|")))
            {
                string[] strInfo = dsProperty.PusherTypeFullName.Split("|".ToCharArray());
                configSql.pullSqlConfiguration1.ConnectionStringKeyNameDesign = strInfo[0];
                configSql.pullSqlConfiguration1.ConnectionStringKeyName = strInfo[1];
                configSql.pullSqlConfiguration1.UpdateQuery = strInfo[2];
            }
            if (configSql.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dsProperty.PusherTypeFullName = string.Format("{0}|{1}|{2}",
                    configSql.pullSqlConfiguration1.ConnectionStringKeyNameDesign,
                    configSql.pullSqlConfiguration1.ConnectionStringKeyName, 
                    configSql.pullSqlConfiguration1.UpdateQuery);

                return dsProperty.PusherTypeFullName;
            }
            return string.Empty;
        }
        #endregion ShowSqlEditor

        #region ShowCustomInterfaceEditor
        private string ShowCustomInterfaceEditor(IWindowsFormsEditorService wfes, SreDataSourceProperty dsProperty)
        {
            TypeSelectorDialog activitySelector = new TypeSelectorDialog(typeof(Pushers));

            if ((wfes.ShowDialog(activitySelector) == DialogResult.OK)
                && (!String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null))
            {
                string strPostEvent = string.Format("{0}, {1}", activitySelector.Activity.FullName, Path.GetFileNameWithoutExtension(activitySelector.AssemblyPath));
                dsProperty.PostEventCustomInterfaceName = strPostEvent;
                return strPostEvent;
            }

            return string.Empty;
            
        }
        #endregion ShowCustomInterfaceEditor


    }
}


