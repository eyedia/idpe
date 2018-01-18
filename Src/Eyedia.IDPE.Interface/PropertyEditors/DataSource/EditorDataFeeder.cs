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
using Eyedia.Core;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;

namespace Eyedia.IDPE.Interface
{    
    public class EditorDataFeeder : UITypeEditor
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

                switch (dsProperty.DataFeederType)
                {
                    case DataFeederTypes.PullSql:

                        frmConfigSql configPullSql = new frmConfigSql(dsProperty.Id);
                        wfes.ShowDialog(configPullSql);
                        break;


                    default:

                        DataFormatTypes selType = dsProperty.DataFormatType;
                        if ((selType == DataFormatTypes.Xml)
                            || (selType == DataFormatTypes.Zipped))
                            selType = DataFormatTypes.Delimited;  //to prevent showing xml/zip UI when clicked from this button.

                        frmConfig configUI = new frmConfig(true, dsProperty.Id, dsProperty.DataFeederType, selType);

                        IdpeKey key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.FtpRemoteLocation.ToString());
                        configUI.FtpRemoteLocation = key != null ? key.Value : string.Empty;
                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.LocalLocation.ToString());
                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.FtpUserName.ToString());
                        configUI.FtpUserName = key != null ? key.Value : string.Empty;
                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.FtpPassword.ToString());
                        configUI.FtpPassword = key != null ? key.Value : string.Empty;
                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.FtpWatchInterval.ToString());
                        configUI.Interval = int.Parse(key != null ? key.Value : "2");
                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.WatchFilter.ToString());
                        configUI.WatchFilter = key != null ? key.Value : "All(No Filter)";

                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.FileInterfaceName.ToString());
                        if (key != null)
                            configUI.InterfaceName = key.Value;

                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.IsFirstRowHeader.ToString());
                        if (key != null)
                            configUI.IsFirstRowIsHeader = key.Value.ParseBool();


                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.LocalFileSystemFoldersOverriden.ToString());
                        configUI.LocalFileSystemFoldersOverriden = key != null ? key.Value.ParseBool() : false;

                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.LocalFileSystemFolderArchiveAuto.ToString());
                        configUI.LocalFileSystemFolderArchiveAuto = key != null ? key.Value.ParseBool() : false;

                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.LocalFileSystemFolderPull.ToString());
                        configUI.LocalFileSystemFolderPullFolder = key != null ? key.Value : string.Empty;

                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.LocalFileSystemFolderArchive.ToString());
                        configUI.LocalFileSystemFolderArchiveFolder = key != null ? key.Value : string.Empty;

                        key = dsProperty.DataSourceKeys.GetKey(IdpeKeyTypes.LocalFileSystemFolderOutput.ToString());
                        configUI.LocalFileSystemFolderOutputFolder = key != null ? key.Value : string.Empty;


                        wfes.ShowDialog(configUI);
                        //this.DataSourceKeys = _Manager.GetKeys(DataSource.Id);


                        break;
                }                          
            }
            return value;
        }
    }
}


