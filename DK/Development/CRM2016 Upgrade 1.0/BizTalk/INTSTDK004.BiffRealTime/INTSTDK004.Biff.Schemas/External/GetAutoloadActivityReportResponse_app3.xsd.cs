namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://BIFF.org/AutoloadActivity.xsd",@"AutoloadActivity")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"AutoloadActivity"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app1", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app2", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse))]
    public sealed class GetAutoloadActivityReportResponse_app3 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:app2=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:mstns=""http://cubic.com"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns=""http://BIFF.org/AutoloadActivity.xsd"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://BIFF.org/AutoloadActivity.xsd"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app1"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse_app2"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse"" namespace=""http://cubic.com"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://BIFF.org/AutoloadActivity.xsd"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
        <reference targetNamespace=""http://cubic.com"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""AutoloadActivity"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Activity"">
          <xs:complexType>
            <xs:sequence>
              <xs:element app1:Ordinal=""0"" minOccurs=""0"" name=""CardSerialNumber"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""1"" minOccurs=""0"" name=""RequestID"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""2"" minOccurs=""0"" name=""CardSection"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""3"" minOccurs=""0"" name=""Currency"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""4"" minOccurs=""0"" name=""Price"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""5"" minOccurs=""0"" name=""StatusCode"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""6"" minOccurs=""0"" name=""DateLoaded"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
            </xs:sequence>
            <xs:attribute ref=""app2:id"" />
            <xs:attribute ref=""msdata:rowOrder"" />
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetAutoloadActivityReportResponse_app3() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "AutoloadActivity";
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
