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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.Toolbox;
using System.Activities.Statements;
using System.ComponentModel;
using System.Activities.Presentation.Validation;
using System.IO;
using System.Windows.Markup;
using System.Activities.Hosting;
using System.Xaml;
using System.Activities.XamlIntegration;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.DataManager;
using System.Activities.Core.Presentation;
using System.Reflection;
using System.Drawing;
using Eyedia.Core.Windows.Utilities;
using System.Xml;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;
using System.Activities.Presentation.View;
using System.Activities.Presentation.Services;
using Eyedia.IDPE.Services;

namespace Eyedia.IDPE.Interface.RuleEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ValidationErrorInfo> Returnerrors = new List<ValidationErrorInfo>();
        //ViewStateService _ViewStateService;
        private WorkflowDesigner _WorkflowDesigner;
        private MainWindowViewModel _ViewModel;        
        private bool _IsWorkflowChanged = false;
        DebugValidationErrorService _DebugValidationErrorService;
        RuleSetTypes _ruleSetTemplate;
        ActivityBuilder _activitybuilder;
        SreRule _sreRule;
        string TransactionMode = "INSERT";        
        public MainWindow(SreRule sreRule, RuleSetTypes ruleSetType)
        {
            if (!String.IsNullOrEmpty(sreRule.Name))
            {
                string sname = sreRule.Name.Replace("-", "").Replace(" ", "");
                this.WindowName = sname;
            }
        
            this._TemplateWithJob = System.IO.Path.Combine(Information.TempDirectorySre, "WithJob.xml");
            this._TemplateWithData = System.IO.Path.Combine(Information.TempDirectorySre, "WithData.xml");

            if (!File.Exists(_TemplateWithJob))
            {
                using (Stream stream = Assembly.GetExecutingAssembly()
                               .GetManifestResourceStream("Eyedia.IDPE.Interface.RuleEditor.Templates.WithJob.xml"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (StreamWriter sw = new StreamWriter(_TemplateWithJob))
                        {
                            sw.Write(reader.ReadToEnd());
                            sw.Close();
                        }
                    }
                }
            }
            if (!File.Exists(_TemplateWithData))
            {
                using (Stream stream = Assembly.GetExecutingAssembly()
                              .GetManifestResourceStream("Eyedia.IDPE.Interface.RuleEditor.Templates.WithData.xml"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (StreamWriter sw = new StreamWriter(_TemplateWithData))
                        {
                            sw.Write(reader.ReadToEnd());
                            sw.Close();
                        }
                    }
                }
            }

            //when new reference added, .new file will be created
            if (File.Exists(_TemplateWithJob + ".New"))
            {
                if (File.Exists(_TemplateWithJob))
                    File.Delete(_TemplateWithJob);

                File.Copy(_TemplateWithJob + ".New", _TemplateWithJob);
                File.Delete(_TemplateWithJob + ".New");
            }

            if (File.Exists(_TemplateWithData + ".New"))
            {
                if (File.Exists(_TemplateWithData))
                    File.Delete(_TemplateWithData);

                File.Copy(_TemplateWithData + ".New", _TemplateWithData);
                File.Delete(_TemplateWithData + ".New");
            } 

            _sreRule = sreRule;
            _ruleSetTemplate = ruleSetType;         
            TransactionMode = sreRule.Id == 0 ? "INSERT" : "UPDATE";
            InitializeComponent();
            LoadDocument();           
        }

        void LoadDocument()
        {
            this._ViewModel = new MainWindowViewModel();
            this.DataContext = this._ViewModel;
            RegisterMetadata();
            AddDesigner();
            this.AddToolBox();
            this.AddPropertyInspector();
            this._WorkflowDesigner.ModelChanged += new EventHandler(ModelChanged);
            _DebugValidationErrorService = new DebugValidationErrorService();
            this._WorkflowDesigner.Context.Services.Publish<IValidationErrorService>(_DebugValidationErrorService);
            //ErrorsDataGrid.DataContext = _DebugValidationErrorService.Returnerrors;
            _IsWorkflowChanged = false;
        }

        void ModelChanged(object sender, EventArgs e)
        {
            _IsWorkflowChanged = true;
            if (!this._WorkflowDesigner.IsInErrorState())
            {
                this.ErrorCount = true;
                this.AddErrors();
            }


            if (ErrorCount == true)
                ErrorCount = false;
            else
                ErrorCount = true;



        }

        public enum LoadWorkFlowDesigner
        {
            Default,
            PreContainer
        };

        private void AddErrors()
        {

            //ErrorsDataGrid.DataContext = _DebugValidationErrorService.Returnerrors;
        }

        private bool _ErrorCount;
        public bool ErrorCount
        {
            get { return _ErrorCount; }
            set { _ErrorCount = value; this.AddErrors(); }
        }

        private void AddGridSpliter()
        {
            GridSplitter GS = new GridSplitter();
            grid1.Children.Add(GS);
        }

        public string _TemplateWithJob;
        public string _TemplateWithData;
        TabControl TabCtrl = new TabControl();
        TabItem t1 = new TabItem();
        TabItem t2 = new TabItem();
        RichTextBox xamlTextBox = new RichTextBox();
        private void AddDesigner()
        {
            string strXAML = "";

            this._WorkflowDesigner = new WorkflowDesigner();
            _WorkflowDesigner.Context.Services.GetService<DesignerConfigurationService>().AnnotationEnabled = true;            
            _WorkflowDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
           

            FlowDocument FD = new FlowDocument();
            Paragraph para = new Paragraph();
            xamlTextBox.Name = "txtXamlView";            
            //xamlTextBox.IsReadOnly = true;
            xamlTextBox.FontFamily = new System.Windows.Media.FontFamily("Consolas");

            if (TransactionMode == "INSERT")
            {
                strXAML = _ruleSetTemplate == RuleSetTypes.PreValidate ? ReadXAMLFile(_TemplateWithJob) : ReadXAMLFile(_TemplateWithData);
                _activitybuilder = XamlServices.Load(ActivityXamlServices.CreateBuilderReader(new XamlXmlReader(new StringReader(strXAML)))) as ActivityBuilder;
                _activitybuilder.Name = _sreRule.Name;
                this._WorkflowDesigner.Load(_activitybuilder);
                para.Inlines.Add(strXAML);
            }
            else
            {
                _activitybuilder = XamlServices.Load(ActivityXamlServices.CreateBuilderReader(new XamlXmlReader(new StringReader(_sreRule.Xaml)))) as ActivityBuilder;
                _activitybuilder.Name = _sreRule.Name;
                this._WorkflowDesigner.Load(_activitybuilder);
                para.Inlines.Add(_sreRule.Xaml);
            }

            FD.Blocks.Add(para);
            xamlTextBox.Document = FD;
            xamlTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(TextChangedEventHandler);
            
            //Place the designer canvas in the middle column of the grid.
            Grid.SetColumn(this._WorkflowDesigner.View, 1);
            t1.Header = "Designer";
            t1.Content = this._WorkflowDesigner.View;
            TabCtrl.Items.Add(t1);

            t2.Header = "XAML";
            t2.Content = xamlTextBox;
            TabCtrl.Items.Add(t2);

            TabCtrl.SelectionChanged += new SelectionChangedEventHandler(TabCtrl_SelectionChanged);            
            grdwb.Children.Add(TabCtrl);

        }
        private string _WindowAccessState;

        public string WindowName
        {
            get { return _WindowAccessState; }
            set { _WindowAccessState = value; }
        }
        
        
        void TabCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.Source is TabControl) //if this event fired from TabControl then enter
            {
                if (t2.IsSelected)
                {
                    SaveXaml();
                    FlowDocument FD = new FlowDocument();
                    Paragraph para = new Paragraph();
                    para.Inlines.Add(_sreRule.Xaml);
                    FD.Blocks.Add(para);
                    xamlTextBox.Document = FD;                   
                }
            }
        }     

        private string ReadXAMLFile(string TemplateFilePath)
        {
            StringBuilder sb = new StringBuilder();
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(TemplateFilePath);
            while ((line = file.ReadLine()) != null)
            {
                sb.Append(line);
            }
            file.Close();

            return sb.ToString();
        }
        private void RegisterMetadata()
        {
            DesignerMetadata dm = new DesignerMetadata();
            dm.Register();
        }


        private void AddToolBox()
        {
            ToolboxControl tc = this._ViewModel.ViewToolbox();
            Grid.SetColumn(tc, 0);       
            grdToolbox.Children.Add(tc);            
        }
        private void AddPropertyInspector()
        {            
            _WorkflowDesigner.PropertyInspectorView.IsEnabled = true;
            grdproperty.Children.Add(_WorkflowDesigner.PropertyInspectorView);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess = false;
            isSuccess = SaveWorkflow();

            if (isSuccess)
                this.Close();
        }
        private bool SaveWorkflow()
        {
            bool IsSuccess = false;
            SaveXaml();

            if (string.IsNullOrEmpty(_sreRule.Xaml))
            {
                MessageBox.Show("Can not save blank rule ", "Validation check", MessageBoxButton.OK, MessageBoxImage.Hand);
                return IsSuccess;
            }

            if (!this._WorkflowDesigner.IsInErrorState())
            {
                TabItem ti = TabCtrl.SelectedItem as TabItem;
                if (ti.Header == "Designer")
                {
                    if (string.IsNullOrEmpty(_sreRule.Name) || _sreRule.Name == "New Rule Designer")
                        _sreRule.Name = "new rule1";

                    _sreRule.Xaml = this._WorkflowDesigner.Text;
                    RulesExtraInformation rulesExtraInformation = new RulesExtraInformation(_sreRule);
                    _sreRule.Name = rulesExtraInformation.RuleName;
                    _sreRule.Description = rulesExtraInformation.RuleDescription;

                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        Save();
                    }
                    else
                    {
                        if (rulesExtraInformation.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            _sreRule.Name = rulesExtraInformation.RuleName;
                            _sreRule.Description = rulesExtraInformation.RuleDescription;
                            Save();
                            rulesExtraInformation.Close();
                        }
                        else
                        {
                            return false; //to avoid closing this form, we dont want to close and lose
                        }
                    }
                    if (RuleForm != null)
                        RuleForm.RefreshRules(RuleForm.txtSearch.Text);


                }
                else
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = _sreRule.Name + ".xml";
                    dlg.DefaultExt = ".xml";
                    dlg.Filter = "XML documents (.xml)|*.xml";

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        this._WorkflowDesigner.Save(dlg.FileName);
                    }
                }

                IsSuccess = true;
                _IsWorkflowChanged = false;
            }
            else
            {
                this.AddErrors();
                IsSuccess = false;
            }
            return IsSuccess;
        }

        private void SaveXaml()
        {
            _activitybuilder.Name = _sreRule.Name;
            this._WorkflowDesigner.Save(System.IO.Path.Combine(Information.TempDirectorySre, _sreRule.Name + ".xml"));           
            _sreRule.Xaml = this._WorkflowDesigner.Text;
        }

        private void Save()
        {            
            SaveXaml();
            new SreVersionManager().KeepVersion(VersionObjectTypes.Rule, _sreRule.Id);
            new Manager().Save(_sreRule);
            SreServiceCommunicator.ClearRule(_sreRule.Id, _sreRule.Name);            
        }

        public string RuleName { get; set; }

        private void GridSplitter_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeNS;
        }

        private void GridSplitter_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;

        }
        private void GridVertSplitter_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeWE;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            TypeSelectorDialog typeSelectorDialog = new TypeSelectorDialog();
            //string selectedNameSpace;
            System.Windows.Forms.DialogResult dResult = typeSelectorDialog.ShowDialog();
            if ((dResult == System.Windows.Forms.DialogResult.OK) && (!String.IsNullOrEmpty(typeSelectorDialog.AssemblyPath) && typeSelectorDialog.Activity != null))
            {
                //selectedNameSpace = string.Format("clr-namespace:{0};assembly={1}", typeSelectorDialog.Activity.Namespace, System.IO.Path.GetFileNameWithoutExtension(typeSelectorDialog.AssemblyPath));
                string newAttributeName = GenerateNewAttributeName(_TemplateWithJob, typeSelectorDialog.Activity.Namespace);
                string newAttributeValue = GenerateNewAttributeValue(typeSelectorDialog.Activity.Namespace, System.IO.Path.GetFileNameWithoutExtension(typeSelectorDialog.AssemblyPath));
                AddNewAttribute(_TemplateWithData, newAttributeName, newAttributeValue);
                AddNewAttribute(_TemplateWithJob, newAttributeName, newAttributeValue);
                if (MessageBox.Show("You need to close editor to reflect the changes. Do you want to do it now?", "New Reference Added", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    this.Close();
            }
        }

        #region Add reference
        private void AddNewAttribute(string templateFileName, string newAttributeName, string newAttributeValue)
        {
            //what am I missing in this code?
            /*
            XmlDocument doc = new XmlDocument();
            doc.Load(templateFileName);
            XmlNode node = doc.FirstChild;
            XmlAttribute attr = doc.CreateAttribute(newAttributeName);            
            attr.Value = newAttributeValue;         
            node.Attributes.Append(attr);
            doc.Save(templateFileName + "1");*/

            //stupid code starts
            StreamReader sr = new StreamReader(templateFileName);
            string xmlAsString = sr.ReadToEnd();
            sr.Close();
            int pos = xmlAsString.IndexOf(">");
            string newXmlAsString = xmlAsString.Substring(0, pos);
            newXmlAsString += string.Format(" xmlns:{0}=\"{1}\">", newAttributeName, newAttributeValue);
            newXmlAsString += xmlAsString.Substring(pos + 1, xmlAsString.Length - (pos + 1));

            using (StreamWriter sw = new StreamWriter(templateFileName + ".New"))
            {
                sw.Write(newXmlAsString);
                sw.Close();
            }
            //stupid code ends
        }

        private string GenerateNewAttributeName(string templateFileName, string namespaceName)
        {

            List<string> existingAttributeNames = GetActivityAttributes(templateFileName);

            string ns = string.Empty;
            if (namespaceName.Contains("."))
            {
                string[] splitted = namespaceName.Split(".".ToCharArray());
                foreach (string str in splitted)
                {
                    ns += str.Substring(0, 1);
                }
            }
            else
            {
                ns = namespaceName.Substring(0, 1);
            }
            ns = ns.ToLower();
            string actualns = ns;
            int counter = 1;
            while (AttributeNameAlreadyExist(ns, existingAttributeNames))
            {
                ns = actualns + counter;
                counter++;
            }

            return ns;
        }

        private string GenerateNewAttributeValue(string namespaceName, string assemblyNameWithOutExtension)
        {
            return string.Format("clr-namespace:{0};assembly={1}", namespaceName, assemblyNameWithOutExtension);
        }

        private bool AttributeNameAlreadyExist(string newAttributeName, List<string> existingAttributeNames)
        {
            newAttributeName = "xmlns:" + newAttributeName;
            foreach (string attributeName in existingAttributeNames)
            {
                if (attributeName == newAttributeName)
                    return true;
            }
            return false;
        }

        private List<string> GetActivityAttributes(string templateFileName)
        {
            List<string> attributes = new List<string>();
            //XmlTextReader reader = new XmlTextReader(templateFileName);

            //XmlDocument doc = new XmlDocument();
            //XmlNode node = doc.ReadNode(reader);
            XmlDocument doc = new XmlDocument();
            doc.Load(templateFileName);
            XmlNode node = doc.FirstChild;
            foreach (XmlAttribute attrb in node.Attributes)
            {
                attributes.Add(attrb.Name);
            }

            return attributes;
        }
        #endregion Add reference

        #region Properties
        public Rules RuleForm { get; set; }
        #endregion Properties

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_IsWorkflowChanged)
            {
                MessageBoxResult closingResult = MessageBox.Show(string.Format("Do you want to save?", this._sreRule.Name), "Unsaved Data", MessageBoxButton.YesNoCancel);
                switch (closingResult)
                {
                    case MessageBoxResult.No:
                        e.Cancel = false;
                        break;
                    case MessageBoxResult.Yes:
                        e.Cancel = !this.SaveWorkflow();
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

            if (!e.Cancel)
                Utilities.CheckOpenWindow.Helpers.DeleteWindowFromList(this.WindowName);
        }

        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            if (xamlTextBox.Document == null)
                return;

            SyntaxHighLighterWPF.Highlight(xamlTextBox);
        }


    }
}



