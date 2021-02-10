namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://cubic.com",@"GetCardDetails2Response")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCardDetails2Response"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.Diffgram", typeof(global::INTSTDK004.Biff.Schemas.External.Diffgram))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.ResponseSerializationMode", typeof(global::INTSTDK004.Biff.Schemas.External.ResponseSerializationMode))]
    public sealed class GetCardDetails2ResponseType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns=""http://cubic.com"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://cubic.com"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.Diffgram"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.ResponseSerializationMode"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://Biff.com/CardDetails2.xsd"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetCardDetails2Response"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" minOccurs=""0"" maxOccurs=""1"" name=""GetCardDetails2Result"">
          <xs:complexType>
            <xs:sequence>
              <xs:any processContents=""skip"" />
              <xs:element ref=""diffgr:diffgram"" />
            </xs:sequence>
            <xs:attribute ref=""msdata:SchemaSerializationMode"" />
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetCardDetails2ResponseType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetCardDetails2Response";
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
