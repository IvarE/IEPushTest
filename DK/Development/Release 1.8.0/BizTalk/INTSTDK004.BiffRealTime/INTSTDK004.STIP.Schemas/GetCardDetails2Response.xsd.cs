namespace INTSTDK004.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216",@"GetCardDetails2Response")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCardDetails2Response"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.STIP.Schemas.CardDetails2Type", typeof(global::INTSTDK004.STIP.Schemas.CardDetails2Type))]
    public sealed class GetCardDetails2ResponseType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.STIP.Schemas.CardDetails2Type"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetCardDetails2Response"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""GetCardDetails2Result"">
          <xs:complexType>
            <xs:sequence>
              <xs:element ref=""ns0:CardDetails2"" />
            </xs:sequence>
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
