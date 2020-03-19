namespace INTSTDK009.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK009.STIP.Schemas.OrderEnvelope",@"Orders")]
    [BodyXPath(@"/*[local-name()='Orders' and namespace-uri()='http://INTSTDK009.STIP.Schemas.OrderEnvelope']")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Orders"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.STIP.Schemas.Order", typeof(global::INTSTDK009.STIP.Schemas.Order))]
    public sealed class OrderEnvelope : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK009.STIP.Schemas.OrderEnvelope"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""http://INTSTDK009.CreateOrder.STIP.Schemas.Order"" targetNamespace=""http://INTSTDK009.STIP.Schemas.OrderEnvelope"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK009.STIP.Schemas.Order"" namespace=""http://INTSTDK009.CreateOrder.STIP.Schemas.Order"" />
  <xs:annotation>
    <xs:appinfo>
      <b:schemaInfo is_envelope=""yes"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" />
      <b:references>
        <b:reference targetNamespace=""http://INTSTDK009.CreateOrder.STIP.Schemas.Order"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""Orders"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo body_xpath=""/*[local-name()='Orders' and namespace-uri()='http://INTSTDK009.STIP.Schemas.OrderEnvelope']"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""unbounded"" ref=""ns0:Order"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public OrderEnvelope() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Orders";
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
