namespace INTSTDK008.Ehandel.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK008.Ehandel.Schemas.CreditOrderRequest",@"CreditOrderParameters")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.orderNumber), XPath = @"/*[local-name()='CreditOrderParameters' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.CreditOrderRequest']/*[local-name()='orderNumber' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreditOrderParameters"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.PropertySchema.PropertySchema", typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.PropertySchema))]
    public sealed class CreditOrderRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK008.Ehandel.Schemas.CreditOrderRequest"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK008.Ehandel.Schemas.PropertySchema"" targetNamespace=""http://INTSTDK008.Ehandel.Schemas.CreditOrderRequest"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK008.Ehandel.Schemas.PropertySchema"" location=""INTSTDK008.Ehandel.Schemas.PropertySchema.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CreditOrderParameters"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:orderNumber"" xpath=""/*[local-name()='CreditOrderParameters' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.CreditOrderRequest']/*[local-name()='orderNumber' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""orderNumber"" type=""xs:string"" />
        <xs:element name=""Sum"" type=""xs:string"" />
        <xs:element name=""ProductNumber"" type=""xs:string"" />
        <xs:element name=""Quantity"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CreditOrderRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CreditOrderParameters";
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
