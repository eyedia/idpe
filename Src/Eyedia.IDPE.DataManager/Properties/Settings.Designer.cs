﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eyedia.IDPE.DataManager.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.5.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=DTCNXPLTSMSSD05\\SQLEX2005;Initial Catalog=IDPE;Integrated Security=Tr" +
            "ue")]
        public string cs {
            get {
                return ((string)(this["cs"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=DTCNXPLTSMSSD05\\SQLEX2005;Initial Catalog=SRE;Integrated Security=Tru" +
            "e")]
        public string SREConnectionString {
            get {
                return ((string)(this["SREConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=DTCBE4D9DAFF028\\SQLEXPRESS;Initial Catalog=sre;Integrated Security=Tr" +
            "ue")]
        public string sreConnectionString1 {
            get {
                return ((string)(this["sreConnectionString1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=MCIB3ESXT001V13\\SQLEXPRESS;Initial Catalog=sre;Integrated Security=Tr" +
            "ue")]
        public string sreConnectionString2 {
            get {
                return ((string)(this["sreConnectionString2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=MCIB3ESXT001V13\\SQLEXPRESS;Initial Catalog=sre2.0;Integrated Security" +
            "=True")]
        public string sre2_0ConnectionString {
            get {
                return ((string)(this["sre2_0ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=SYNLPTMP1WWH7\\SQLExpress;Initial Catalog=sre;Integrated Security=True" +
            "")]
        public string sreConnectionString3 {
            get {
                return ((string)(this["sreConnectionString3"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=MCIB3ESXT001V13\\SQLEXPRESS;Initial Catalog=sresample;Integrated Secur" +
            "ity=True")]
        public string sresampleConnectionString {
            get {
                return ((string)(this["sresampleConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ROSETTA\\SQLEXPRESS;Initial Catalog=SymplusRuleEngine;Integrated Secur" +
            "ity=True")]
        public string SymplusRuleEngineConnectionString {
            get {
                return ((string)(this["SymplusRuleEngineConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=CHARLOTTE-CORE\\SQLEXPRESS;Initial Catalog=SymplusRuleEngine;Integrate" +
            "d Security=True")]
        public string SymplusRuleEngineConnectionString1 {
            get {
                return ((string)(this["SymplusRuleEngineConnectionString1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=LPT-03084856325\\SQLEXPRESS;Initial Catalog=idpe;Integrated Security=T" +
            "rue")]
        public string idpeConnectionString {
            get {
                return ((string)(this["idpeConnectionString"]));
            }
        }
    }
}