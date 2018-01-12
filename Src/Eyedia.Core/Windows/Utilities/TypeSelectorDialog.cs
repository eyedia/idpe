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
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;

namespace Eyedia.Core.Windows.Utilities
{
    public partial class TypeSelectorDialog : Form
    {
        #region Variables and constructor

        public TypeSelectorDialog(Type filterType = null)
        {
            InitializeComponent();
            FilterType = filterType;

        }

        Type FilterType;
        private Type SelectedType;
        private string assemblyPath;
        private string initialDirectory;

        #endregion

        #region Properties
        
        public string AssemblyPath
        {
            get
            {
                return assemblyPath;
            }
            set
            {
                assemblyPath = value;
            }
        }

        public Type Activity
        {
            get
            {
                return SelectedType;
            }
            set
            {
                SelectedType = value;
            }
        }

        public string InitialDirectory
        {
            get { return initialDirectory; }
            set { initialDirectory = value; }
        }

        #endregion

        #region Form load and setup

        private void ActivitySelector_Load(object sender, EventArgs e)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(activitySelector_AssemblyResolve);

            if (!String.IsNullOrEmpty(assemblyPath))
            {
                assemblyBox.Text = assemblyPath;
                this.PopulateActivities();
            }

        }

        Assembly activitySelector_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                string assemblyPath = assemblyBox.Text;
                string failedAssemblyName = args.Name;
                string assemblyName;
                if (failedAssemblyName.Contains(",")) //strong name; need to strip off everything but the name
                {
                    assemblyName = failedAssemblyName.Substring(0, failedAssemblyName.IndexOf(",", StringComparison.Ordinal));
                }
                else
                {
                    assemblyName = failedAssemblyName;
                }
                string tempPath = Path.HasExtension(assemblyPath) ? Path.GetDirectoryName(assemblyPath) : assemblyPath;
                string assemblyPathToTry = tempPath + Path.DirectorySeparatorChar + assemblyName + ".dll";

                FileInfo assemblyFileInfo = new FileInfo(assemblyPathToTry);
                if (assemblyFileInfo.Exists)
                {
                    return Assembly.LoadFile(assemblyPathToTry);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception) //no luck in resolving the assembly
            {
                return null;
            }

        }

        #endregion

        #region Event handlers

        internal List<string> GetMembers(Type targetType)
        {
            List<string> members = new List<string>();

            if (targetType != null)
            {
                try
                {
                    PropertyInfo[] properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (PropertyInfo property in properties)
                    {
                        members.Add(string.Format(CultureInfo.InvariantCulture, "{0}   ({1})", property.Name, property.PropertyType));
                    }

                    FieldInfo[] fields = targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (FieldInfo field in fields)
                    {
                        members.Add(string.Format(CultureInfo.InvariantCulture, "{0}   ({1})", field.Name, field.FieldType));
                    }

                    members.Sort(); //sort all fields and properties as one, but exclude methods which will all be listed at the end

                    List<string> methodMembers = new List<string>();
                    MethodInfo[] methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (MethodInfo method in methods)
                    {
                        if (!method.Name.StartsWith("get_", StringComparison.Ordinal) && !method.Name.StartsWith("set_", StringComparison.Ordinal))
                        {
                            methodMembers.Add(method.ToString());
                        }
                    }

                    methodMembers.Sort();
                    members.AddRange(methodMembers);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading members for the target Type: \r\n\n{0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return members;
        }

        private void activitiesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            membersBox.Items.Clear();

            if (activitiesBox.SelectedItem != null)
            {
                membersBox.Items.AddRange(GetMembers(activitiesBox.SelectedItem as Type).ToArray());
            }
        }

        private void pickAssemblyButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Component Files (*.dll;*.exe)|*.dll;*.exe";
            if (!String.IsNullOrEmpty(assemblyPath))
                dialog.InitialDirectory = Path.GetDirectoryName(assemblyPath);
            else if (!String.IsNullOrEmpty(initialDirectory))
                dialog.InitialDirectory = initialDirectory;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK && dialog.FileName != null)
            {
                assemblyBox.Text = dialog.FileName;
                this.PopulateActivities();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SelectedType = activitiesBox.SelectedItem as Type;
            assemblyPath = assemblyBox.Text;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Other

        internal static Assembly LoadAssembly(string assemblyPath)
        {
            Assembly assembly = null;
            if (!String.IsNullOrEmpty(assemblyPath))
            {
                try
                {
                    FileInfo assemblyFileInfo = new FileInfo(assemblyPath);
                    if (assemblyFileInfo.Exists)
                    {
                        assembly = Assembly.LoadFile(assemblyPath);
                    }
                    else
                    {
                        //try to load from the application directory
                        AssemblyName assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(assemblyPath));
                        assembly = Assembly.Load(assemblyName);
                    }
                }
                catch (FileLoadException ex)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading assembly for the referenced Type at: \r\n\n'{0}' \r\n\n{1}", assemblyPath, ex.Message), "Assembly Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading assembly for the referenced Type at: \r\n\n'{0}'", assemblyPath), "Assembly Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return assembly;
        }

        private void PopulateActivities()
        {
            activitiesBox.Items.Clear();

            Assembly assembly = LoadAssembly(assemblyBox.Text);

            if (assembly != null)
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        // add a check here if you want to constrain the kinds of Types (e.g. Activity) that rulesets can be authored against
                        if (FilterType != null)
                        {
                            //if (type.ToString().Contains("Pushers"))
                            //    Debugger.Break();

                            if (type.IsSubclassOf(FilterType))
                            {
                                activitiesBox.Items.Add(type);
                            }
                        }
                        else
                        {
                            activitiesBox.Items.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Error loading types from assembly '{0}': \r\n\n{1}", assembly.FullName, ex.LoaderExceptions[0].Message), "Type Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (SelectedType != null && activitiesBox.Items.Contains(SelectedType))
            {
                activitiesBox.SelectedItem = SelectedType;
            }

            else if (activitiesBox.Items.Count > 0)
            {
                activitiesBox.SetSelected(0, true);
            }
        }



        #endregion
    }
}




