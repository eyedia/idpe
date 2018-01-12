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
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation;
using System.Activities;
using System.Activities.Statements;
using System.ServiceModel.Activities;
using System.ServiceModel.Activities.Presentation.Factories;
using System.Windows.Input;
using Eyedia.IDPE.Services;
using System.Reflection;
using System.IO;
using System.Activities.Presentation.Metadata;
using System.Windows.Media.Imaging;
using System.Drawing;


namespace Eyedia.IDPE.Interface.RuleEditor
{
    public class MainWindowViewModel
    {
        private ToolboxControl toolboxControl;
        private IDictionary<string, ToolboxCategory> toolboxCategoryMap;
        private IDictionary<ToolboxCategory, IList<string>> loadedToolboxActivities;
        private ICommand saveWorkflowCommand;
        static System.Resources.ResourceReader resourceReader;
        public MainWindowViewModel()
        {
           // LoadToolboxIconsForBuiltInActivities();
            this.toolboxControl = new ToolboxControl();
            this.InitialiseToolbox();
            
        }

        public ToolboxControl ViewToolbox()
        {
            return this.toolboxControl;
        }
        private bool IsValidToolboxActivity(Type activityType)
        {
            return activityType.IsPublic && !activityType.IsNested && !activityType.IsAbstract && !activityType.ContainsGenericParameters;
            //&& (typeof(Activity).IsAssignableFrom(activityType) || typeof(IActivityTemplateFactory).IsAssignableFrom(activityType));
        }
        private void InitialiseToolbox()
        {
            this.loadedToolboxActivities = new Dictionary<ToolboxCategory, IList<string>>();
            this.toolboxCategoryMap = new Dictionary<string, ToolboxCategory>();

            //this.AddCategoryToToolbox(
            //    Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
            //    new List<Type>
            //    {
            //        typeof(DoWhile),
            //        typeof(System.Activities.Core.Presentation.Factories.ForEachWithBodyFactory<Int32>),
            //        typeof(If),
            //        typeof(Parallel),
            //        typeof(System.Activities.Core.Presentation.Factories.ParallelForEachWithBodyFactory<Int32>),
            //        typeof(Pick),
            //        typeof(PickBranch),
            //        typeof(Sequence),
            //        typeof(Switch<Int32>),
            //        typeof(While)
            //    });

            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox, 
                typeof(DoWhile), "DoWhile");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(System.Activities.Core.Presentation.Factories.ForEachWithBodyFactory<Int32>), "ForEach");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(If), "If");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(Parallel), "Parallel");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(System.Activities.Core.Presentation.Factories.ParallelForEachWithBodyFactory<Int32>), "ParallelForEach");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
               typeof(Pick), "Pick");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(PickBranch), "PickBranch");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(Sequence), "Sequence");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(Switch<Int32>), "Switch");
            this.AddCategoryToToolbox(Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ControlFlowCategoryToToolbox,
                typeof(While), "While");


            this.AddCategoryToToolbox(
                Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.FlowchartCategoryToToolbox,
                new List<Type>
                {
                    typeof(Flowchart),
                    typeof(FlowDecision),
                    typeof(FlowSwitch<object>)
                });

            this.AddCategoryToToolbox(
                Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.MessagingCategoryToToolbox,
                new List<Type>
                {
                    typeof(CorrelationScope),
                    typeof(InitializeCorrelation),
                    typeof(Receive),
                    typeof(ReceiveAndSendReplyFactory),
                    typeof(Send),
                    typeof(SendAndReceiveReplyFactory),
                    typeof(TransactedReceiveScope)
                });

            this.AddCategoryToToolbox(
                Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.RuntimeCategoryToToolbox,
                new List<Type>
                {
                    typeof(Persist),
                    typeof(TerminateWorkflow),
                });

            this.AddCategoryToToolbox(
                Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.PrimitivesCategoryToToolbox,
                new List<Type>
                {
                    typeof(Assign),
                    typeof(Delay),
                    typeof(InvokeMethod),
                    typeof(WriteLine),
                });

            this.AddCategoryToToolbox(
                Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.TransactionCategoryToToolbox,
                new List<Type>
                {
                    typeof(CancellationScope),
                    typeof(CompensableActivity),
                    typeof(Compensate),
                    typeof(Confirm),
                    typeof(TransactionScope),
                });

            object T = new object();
            this.AddCategoryToToolbox(
                Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.CollectionCategoryToToolbox,
                new List<Type>
                {
                    typeof(AddToCollection<Int32>),
                    typeof(ClearCollection<Int32>),
                    typeof(ExistsInCollection<Int32>),
                    typeof(RemoveFromCollection<Int32>),
                });

            this.AddCategoryToToolbox(
                Eyedia.IDPE.Interface.RuleEditor.Properties.Resources.ErrorHandlingCategoryToToolbox,
                new List<Type>
                {
                    typeof(Rethrow),
                    typeof(Throw),
                    typeof(TryCatch),
                });

            string strFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Eyedia.IDPE.Services.dll");
            Assembly assembly = Assembly.LoadFile(strFilePath);
            List<Type> lstServices = new List<Type>();            
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Activity)))
                {
                    if (type.Name == "AddError" || type.Name == "AddOrUpdateProcessVariable" 
                        || type.Name == "AssignAttribute" || type.Name == "WriteTraceToCurrentRow"
                        || type.Name == "PreValidationFailed" || type.Name == "DuplicateCheck" 
                        || type.Name == "RestApiCall" || type.Name == "Lookup"
                        || type.Name == "LookupInCache" || type.Name == "InitCache" || type.Name == "ClearCache"
                        || type.Name == "PersistVariable" || type.Name == "GetPersistVariable" || type.Name == "DeletePersistVariable"
                        || type.Name == "ExecuteNonQuery" || type.Name == "ExecuteQuery" || type.Name == "WriteTrace"
                        || type.Name == "MarkValid" || type.Name == "BulkInsert" || type.Name == "LockedSequence"
                        || type.Name == "SetDefaultValue" || type.Name == "ExecuteRule" || type.Name == "SendEmail" 
                        || type.Name == "GetSreEnvironmentVariable" || type.Name == "ExecuteCSCode")
                    lstServices.Add(type);
                }
            }
            lstServices = lstServices.OrderBy(s => s.Name).ToList();
            this.AddCategoryToToolbox(
                 Properties.Resources.SymplusServicesCategoryToToolbox,
                 lstServices
               );

        }

        private void AddCategoryToToolbox(string categoryName, Type activityType, string displayName)
        {
            if (this.IsValidToolboxActivity(activityType))
            {
                ToolboxCategory category = this.GetToolboxCategory(categoryName);
                ToolboxItemWrapper toolbox = new ToolboxItemWrapper(activityType.FullName, activityType.Assembly.FullName, null, displayName);
                toolbox.BitmapName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", displayName.Replace("<>", "") + ".png");
                category.Add(toolbox);
            }
        }

        private void AddCategoryToToolbox(string categoryName, List<Type> activities)
        {
            List<string> lstignore = new List<string>();
            List<string> lstnew = new List<string>();


            bool Isvalid = true;
            foreach (Type activityType in activities)
            {
                lstnew.Clear();
                foreach (string str in lstignore)
                {
                    lstnew.Add(str);
                }
                if (this.IsValidToolboxActivity(activityType))
                {
                    ToolboxCategory category = this.GetToolboxCategory(categoryName);

                    if (!this.loadedToolboxActivities[category].Contains(activityType.FullName))
                    {
                        string displayName;
                        string[] splitName = activityType.Name.Split('`');
                        if (splitName.Length == 1)
                        {
                            displayName = activityType.Name;
                        }
                        else
                        {
                            displayName = string.Format("{0}<>", splitName[0]);
                        }

                        this.loadedToolboxActivities[category].Add(activityType.FullName);
                        Isvalid = true;
                    
                        if (Isvalid)
                        {
                            ToolboxItemWrapper toolbox = new ToolboxItemWrapper(activityType.FullName, activityType.Assembly.FullName, null, displayName);
                            toolbox.BitmapName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", displayName.Replace("<>", "") + ".png");
                            category.Add(toolbox);
                        }               

                    }
                }
        
            }


        }

        private ToolboxCategory GetToolboxCategory(string name)
        {
            if (this.toolboxCategoryMap.ContainsKey(name))
            {
                return this.toolboxCategoryMap[name];
            }
            else
            {
                ToolboxCategory category = new ToolboxCategory(name);
                this.toolboxCategoryMap[name] = category;
                this.loadedToolboxActivities.Add(category, new List<string>());
                this.toolboxControl.Categories.Add(category);
                return category;
            }
        }

        public ICommand SaveWorkflowCommand
        {
            get
            {
                if (this.saveWorkflowCommand == null)
                {
                    this.saveWorkflowCommand = new RelayCommand(
                        param => this.SaveWorkflow()//,
                        //param => this.CanSave
                        );
                }

                return this.saveWorkflowCommand;
            }
        }

        private void SaveWorkflow()
        {
            //WorkflowViewModel model = this.ActiveWorkflowViewModel;
            //if (model != null)
            //{

            //}
        }

        private void CanSave()
        {
            //WorkflowViewModel model = this.ActiveWorkflowViewModel;
            //if (model != null)
            //{

            //}
        }
        
        private static void LoadToolboxIconsForBuiltInActivities()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            Assembly sourceAssembly = Assembly.LoadFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Microsoft.VisualStudio.Activities.dll"));
            resourceReader = new System.Resources.ResourceReader(sourceAssembly.GetManifestResourceStream("Microsoft.VisualStudio.Activities.Resources.resources"));
            foreach (Type type in typeof(System.Activities.Activity).Assembly.GetTypes().Where(t => t.Namespace == "System.Activities.Statements"))
            { CreateToolboxBitmapAttributeForActivity(builder, resourceReader, type); }
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
        private static void CreateToolboxBitmapAttributeForActivity(AttributeTableBuilder builder, System.Resources.ResourceReader resourceReader, Type builtInActivityType)
        {
            System.Drawing.Bitmap bitmap = ExtractBitmapResource(resourceReader, builtInActivityType.IsGenericType ? builtInActivityType.Name.Split('`')[0] : builtInActivityType.Name);
            if (bitmap != null)
            {
                bitmap.Save(@"C:\temp\icons\Archive\" + builtInActivityType.Name + ".png",System.Drawing.Imaging.ImageFormat.Png);
                Type tbaType = typeof(System.Drawing.ToolboxBitmapAttribute);
                Type imageType = typeof(System.Drawing.Image);
                ConstructorInfo constructor = tbaType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { imageType, imageType }, null);
                System.Drawing.ToolboxBitmapAttribute tba = constructor.Invoke(new object[] { bitmap, bitmap }) as System.Drawing.ToolboxBitmapAttribute;
                builder.AddCustomAttributes(builtInActivityType, tba);
            }
        }
        private static System.Drawing.Bitmap ExtractBitmapResource(System.Resources.ResourceReader resourceReader, string bitmapName)
        {
            string Imagename = bitmapName.Replace("<>", "");
            System.Collections.IDictionaryEnumerator dictEnum = resourceReader.GetEnumerator();
            System.Drawing.Bitmap bitmap1 = null; 
            while (dictEnum.MoveNext())
            {
                System.Drawing.Bitmap bitmap = null;
                if (String.Equals(dictEnum.Key, "InitializeCorrelation"))
                {
                    bitmap = dictEnum.Value as System.Drawing.Bitmap;
                    System.Drawing.Color pixel = bitmap.GetPixel(bitmap.Width - 1, 0);
                    bitmap.MakeTransparent(pixel);
                    bitmap.Save(@"C:\temp\icons\Archive\" + dictEnum.Key + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    break;
                }
            }
            return bitmap1;
        }

    }




}



