namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://BIFF.org/AutoloadActivity.xsd",@"AutoloadActivity")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"AutoloadActivity"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.Diffgram_Id", typeof(global::INTSTDK004.Biff.Schemas.External.Diffgram_Id))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.Diffgram_RowOrder", typeof(global::INTSTDK004.Biff.Schemas.External.Diffgram_RowOrder))]
    public sealed class AutoloadActivityType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns=""http://BIFF.org/AutoloadActivity.xsd"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://BIFF.org/AutoloadActivity.xsd"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.Diffgram_Id"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.Diffgram_RowOrder"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://BIFF.org/AutoloadActivity.xsd"">
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </references>
      <b:references>
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""AutoloadActivity"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Activity"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""CardSerialNumber"" type=""xs:long"" />
              <xs:element minOccurs=""0"" name=""RequestID"" type=""xs:long"" />
              <xs:element minOccurs=""0"" name=""CardSection"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""Currency"">
                <xs:simpleType>
                  <xs:restriction base=""xs:string"">
                    <xs:maxLength value=""3"" />
                  </xs:restriction>
                </xs:simpleType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""Price"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""DateLoaded"" type=""xs:dateTime"" />
            </xs:sequence>
            <xs:attribute ref=""diffgr:id"" />
            <xs:attribute ref=""msdata:rowOrder"" />
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public AutoloadActivityType() {
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
