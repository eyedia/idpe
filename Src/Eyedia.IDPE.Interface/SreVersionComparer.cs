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
using System.IO;
using System.Windows.Forms;
using Eyedia.Core;
using Eyedia.Core.Data;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface
{
    public class SreVersionComparer
    {
        public static void Compare(VersionObjectTypes versionObjectType, string objectName, IdpeVersion v1, IdpeVersion v2)
        {
            var toolPath = Information.LoggedInUser.GetUserPreferences().ComparisonToolExecutablePath;
            if (!File.Exists(toolPath))
            {
                if (MessageBox.Show("Comparison tool was not set! Do you want to set it now? You can later change it from Tools->Preferences", "Comparison Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1)
                    == DialogResult.Yes)
                {
                    if (SetComparisonTool())
                        toolPath = Information.LoggedInUser.GetUserPreferences().ComparisonToolExecutablePath;
                    else
                        return;
                }
                else
                {
                    return;
                }
            }

            
            string v1file = Path.Combine(Information.TempDirectorySre, objectName + ".v" + (v1.Version == 0? "Current":v1.Version.ToString()));
            new PrintObject().PrintProperties(ConvertToSreVersionObject(versionObjectType, v1), v1file);
            if (v1.Version == 0)
                PatchVerionZero(v1file);


            string v2file = Path.Combine(Information.TempDirectorySre, objectName + ".v" + (v2.Version == 0 ? "Current" : v2.Version.ToString()));
            new PrintObject().PrintProperties(ConvertToSreVersionObject(versionObjectType, v2), v2file);
            if (v2.Version == 0)
                PatchVerionZero(v1file);

            System.Diagnostics.Process p = System.Diagnostics.Process.Start(toolPath, "/r \"" + v1file + "\" \"" + v2file + "\"");
            p.EnableRaisingEvents = false;
            p.WaitForExit();
            p.Close();

            if (File.Exists(v1file))
                File.Delete(v1file);

            if (File.Exists(v2file))
                File.Delete(v2file);
        }

        private static void PatchVerionZero(string fileName)
        {
            string str = File.ReadAllText(fileName);
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(str.Replace("Version: 0", "Version: Current"));
                sw.Close();
            }

        }
        private static bool SetComparisonTool()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.FileName = string.Empty;
            openFileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UserPreferences userPreferences = Information.LoggedInUser.GetUserPreferences();
                userPreferences.ComparisonToolExecutablePath = openFileDialog.FileName;
                Information.LoggedInUser.Preferences = userPreferences.Serialize();
                CoreDatabaseObjects.Instance.UpdateUserPreferences(Information.LoggedInUser);
                return true;
            }
            return false;
        }

        public static object ConvertToSreVersionObject(VersionObjectTypes versionObjectType, IdpeVersion version)
        {
            object returnObject = null;
            switch (versionObjectType)
            {
                case VersionObjectTypes.Attribute:
                    returnObject = new object();
                    break;

                case VersionObjectTypes.DataSource:
                    returnObject = new DataSourceBundle(null, GZipArchive.Decompress(version.Data.ToArray()).GetString(), version.Version);
                    break;

                case VersionObjectTypes.Rule:
                    returnObject = new DataSourcePatch(null, GZipArchive.Decompress(version.Data.ToArray()).GetString(), version.Version);
                    break;
            }
            return returnObject;
        }
    }
}


