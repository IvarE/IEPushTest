﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRM2013.SkanetrafikenPlugins.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://v-dkbiz-tst.int.skanetrafiken.com/INTSTDK004/INTSTDK004_13.svc")]
        public string CRM2013_SkanetrafikenPlugins_WebServiceReference_BizTalkServiceInstance {
            get {
                return ((string)(this["CRM2013_SkanetrafikenPlugins_WebServiceReference_BizTalkServiceInstance"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://v-dkbiz-tst.int.skanetrafiken.com/INTSTDK008/CreateGiftCard.svc")]
        public string CRM2013_SkanetrafikenPlugins_CreateGiftcardService_BizTalkServiceInstance {
            get {
                return ((string)(this["CRM2013_SkanetrafikenPlugins_CreateGiftcardService_BizTalkServiceInstance"]));
            }
        }
    }
}
