namespace INTSTDK001.PubTrans.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService",@"GetDirectJourneysBetweenStopsResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStopsResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app2", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app1", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app1))]
    public sealed class GetDirectJourneysBetweenStopsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:mstns=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:app1=""urn:schemas-microsoft-com:xml-diffgram-v1"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" id=""GetDirectJourneysBetweenStopsResponse"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app2"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app1"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
        <b:reference targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element xmlns:app2=""urn:schemas-microsoft-com:xml-msdata"" app2:IsDataSet=""true"" app2:Locale=""en-US"" name=""GetDirectJourneysBetweenStopsResponse"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""GetDirectJourneysBetweenStopsResult"">
          <xs:complexType>
            <xs:sequence>
              <xs:any processContents=""skip"" />
              <xs:element ref=""app1:diffgram"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetDirectJourneysBetweenStopsResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetDirectJourneysBetweenStopsResponse";
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
