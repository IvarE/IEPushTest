﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.skanetrafiken.com/DK/INTSTDK013/20161115", ConfigurationName="WCFService_Biztalk_Internal.Kunskapsartiklar")]
    public interface Kunskapsartiklar {
        
        // CODEGEN: Generating message contract since the operation KBArtiklar is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="KBArtiklar", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse1 KBArtiklar(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="KBArtiklar", ReplyAction="*")]
        System.Threading.Tasks.Task<CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse1> KBArtiklarAsync(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://wwww.skanetrafiken.se/DK/INTSTDK013/KBArtiklarRequest/20161114")]
    public partial class KBArtiklarRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string numberField;
        
        private string titleField;
        
        private string contentField;
        
        private string externalContentField;
        
        private System.DateTime modifiedonField;
        
        private bool modifiedonFieldSpecified;
        
        private string subjectField;
        
        private string keywordsField;
        
        private string createdByUserField;
        
        private string actionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Number {
            get {
                return this.numberField;
            }
            set {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
                this.RaisePropertyChanged("Title");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
                this.RaisePropertyChanged("content");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string ExternalContent {
            get {
                return this.externalContentField;
            }
            set {
                this.externalContentField = value;
                this.RaisePropertyChanged("ExternalContent");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="date", Order=4)]
        public System.DateTime modifiedon {
            get {
                return this.modifiedonField;
            }
            set {
                this.modifiedonField = value;
                this.RaisePropertyChanged("modifiedon");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool modifiedonSpecified {
            get {
                return this.modifiedonFieldSpecified;
            }
            set {
                this.modifiedonFieldSpecified = value;
                this.RaisePropertyChanged("modifiedonSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string subject {
            get {
                return this.subjectField;
            }
            set {
                this.subjectField = value;
                this.RaisePropertyChanged("subject");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string Keywords {
            get {
                return this.keywordsField;
            }
            set {
                this.keywordsField = value;
                this.RaisePropertyChanged("Keywords");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string CreatedByUser {
            get {
                return this.createdByUserField;
            }
            set {
                this.createdByUserField = value;
                this.RaisePropertyChanged("CreatedByUser");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string Action {
            get {
                return this.actionField;
            }
            set {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://wwww.skanetrafiken.se/DK/INTSTDK013/KBArtiklarResponse/20161114")]
    public partial class KBArtiklarResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string messageField;
        
        private string actionTypeField;
        
        private string kbArticleIDField;
        
        private KBArtiklarResponseError errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("Message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ActionType {
            get {
                return this.actionTypeField;
            }
            set {
                this.actionTypeField = value;
                this.RaisePropertyChanged("ActionType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string KbArticleID {
            get {
                return this.kbArticleIDField;
            }
            set {
                this.kbArticleIDField = value;
                this.RaisePropertyChanged("KbArticleID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public KBArtiklarResponseError Error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
                this.RaisePropertyChanged("Error");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://wwww.skanetrafiken.se/DK/INTSTDK013/KBArtiklarResponse/20161114")]
    public partial class KBArtiklarResponseError : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string typeField;
        
        private string errorMessageField;
        
        private string httpStatusCodeField;
        
        private string timeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
                this.RaisePropertyChanged("Type");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
                this.RaisePropertyChanged("ErrorMessage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string HttpStatusCode {
            get {
                return this.httpStatusCodeField;
            }
            set {
                this.httpStatusCodeField = value;
                this.RaisePropertyChanged("HttpStatusCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Time {
            get {
                return this.timeField;
            }
            set {
                this.timeField = value;
                this.RaisePropertyChanged("Time");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class KBArtiklarRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://wwww.skanetrafiken.se/DK/INTSTDK013/KBArtiklarRequest/20161114", Order=0)]
        public CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest KBArtiklarRequest;
        
        public KBArtiklarRequest1() {
        }
        
        public KBArtiklarRequest1(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest KBArtiklarRequest) {
            this.KBArtiklarRequest = KBArtiklarRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class KBArtiklarResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://wwww.skanetrafiken.se/DK/INTSTDK013/KBArtiklarResponse/20161114", Order=0)]
        public CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse KBArtiklarResponse;
        
        public KBArtiklarResponse1() {
        }
        
        public KBArtiklarResponse1(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse KBArtiklarResponse) {
            this.KBArtiklarResponse = KBArtiklarResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface KunskapsartiklarChannel : CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.Kunskapsartiklar, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KunskapsartiklarClient : System.ServiceModel.ClientBase<CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.Kunskapsartiklar>, CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.Kunskapsartiklar {
        
        public KunskapsartiklarClient() {
        }
        
        public KunskapsartiklarClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KunskapsartiklarClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KunskapsartiklarClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KunskapsartiklarClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse1 CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.Kunskapsartiklar.KBArtiklar(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1 request) {
            return base.Channel.KBArtiklar(request);
        }
        
        public CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse KBArtiklar(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest KBArtiklarRequest) {
            CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1 inValue = new CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1();
            inValue.KBArtiklarRequest = KBArtiklarRequest;
            CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse1 retVal = ((CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.Kunskapsartiklar)(this)).KBArtiklar(inValue);
            return retVal.KBArtiklarResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse1> CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.Kunskapsartiklar.KBArtiklarAsync(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1 request) {
            return base.Channel.KBArtiklarAsync(request);
        }
        
        public System.Threading.Tasks.Task<CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarResponse1> KBArtiklarAsync(CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest KBArtiklarRequest) {
            CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1 inValue = new CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.KBArtiklarRequest1();
            inValue.KBArtiklarRequest = KBArtiklarRequest;
            return ((CGI.CRM2013.Skanetrafiken.KBArticlePlugin.WCFService_Biztalk_Internal.Kunskapsartiklar)(this)).KBArtiklarAsync(inValue);
        }
    }
}
