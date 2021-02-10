namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://cubic.com",@"GetAutoloadActivityReportResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetAutoloadActivityReportResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app3", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app3))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app1", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app2", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app2))]
    public sealed class GetAutoloadActivityReportResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:app2=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:mstns=""http://cubic.com"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:app3=""http://BIFF.org/AutoloadActivity.xsd"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://cubic.com"" id=""GetAutoloadActivityReportResponse"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app3"" namespace=""http://BIFF.org/AutoloadActivity.xsd"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app1"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app2"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://BIFF.org/AutoloadActivity.xsd"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
        <b:reference targetNamespace=""http://cubic.com"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" app1:IsDataSet=""true"" app1:Locale=""en-US"" name=""GetAutoloadActivityReportResponse"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""GetAutoloadActivityReportResult"">
          <xs:complexType>
            <xs:sequence>
              <xs:any processContents=""skip"" />
              <xs:element ref=""app2:diffgram"" />
            </xs:sequence>
            <xs:attribute ref=""app1:SchemaSerializationMode"" />
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetAutoloadActivityReportResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetAutoloadActivityReportResponse";
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
