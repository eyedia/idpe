﻿#pragma checksum "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E382656DD804710B806116DCD0DCB309"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Eyedia.IDPE.Interface;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Eyedia.IDPE.Interface {
    
    
    /// <summary>
    /// ConfigDatabaseWriter
    /// </summary>
    public partial class ConfigDatabaseWriter : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 42 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbConnection;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnConn;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbTableName;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCsProcessVariable;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbx2;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbx1;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtMessage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/idped;component/configuis/configdatabasewriter.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cmbConnection = ((System.Windows.Controls.ComboBox)(target));
            
            #line 43 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            this.cmbConnection.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbConnection_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnConn = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.cmbTableName = ((System.Windows.Controls.ComboBox)(target));
            
            #line 49 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            this.cmbTableName.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbTableName_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.txtCsProcessVariable = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.lbx2 = ((System.Windows.Controls.ListBox)(target));
            
            #line 62 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            this.lbx2.AddHandler(System.Windows.Controls.ScrollViewer.ScrollChangedEvent, new System.Windows.Controls.ScrollChangedEventHandler(this.lbx2_ScrollChanged));
            
            #line default
            #line hidden
            return;
            case 7:
            this.lbx1 = ((System.Windows.Controls.ListBox)(target));
            
            #line 70 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            this.lbx1.AddHandler(System.Windows.Controls.ScrollViewer.ScrollChangedEvent, new System.Windows.Controls.ScrollChangedEventHandler(this.lbx1_ScrollChanged));
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            
            #line 86 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            this.btnSave.Click += new System.Windows.RoutedEventHandler(this.btnSave_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 87 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.txtMessage = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 8:
            
            #line 77 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            ((System.Windows.Controls.ComboBox)(target)).DropDownOpened += new System.EventHandler(this.ComboBox_DropDownOpened);
            
            #line default
            #line hidden
            
            #line 77 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            ((System.Windows.Controls.ComboBox)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.ComboBox_GotFocus);
            
            #line default
            #line hidden
            
            #line 77 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            ((System.Windows.Controls.ComboBox)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 77 "..\..\..\ConfigUIs\ConfigDatabaseWriter.xaml"
            ((System.Windows.Controls.ComboBox)(target)).Initialized += new System.EventHandler(this.ComboBox_Initialized);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

