namespace INTSTDK001.PubTrans.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse_app1", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse_app1))]
    public sealed class GetCallsforServiceJourneyResponse_app2 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:app1=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns=""urn:schemas-microsoft-com:xml-msdata"" xmlns:mstns=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse"" namespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" />
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse_app1"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:attribute app2:Prefix=""msdata"" name=""rowOrder"" type=""xs:string"" xmlns:app2=""urn:schemas-microsoft-com:xml-msdata"" />
</xs:schema>";
        
        public GetCallsforServiceJourneyResponse_app2() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [0];
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
    }
}
