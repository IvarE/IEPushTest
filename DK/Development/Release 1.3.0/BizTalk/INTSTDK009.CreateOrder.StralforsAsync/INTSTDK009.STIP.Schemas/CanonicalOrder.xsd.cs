namespace INTSTDK009.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK009.CreateOrder.STIP.Schemas.CanonicalOrder",@"Order")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK009.STIP.Schemas.RoutingCode), XPath = @"/*[local-name()='Order' and namespace-uri()='http://INTSTDK009.CreateOrder.STIP.Schemas.CanonicalOrder']/*[local-name()='RoutingCode' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Order"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.STIP.Schemas.PropertySchema", typeof(global::INTSTDK009.STIP.Schemas.PropertySchema))]
    public sealed class CanonicalOrder : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK009.CreateOrder.STIP.Schemas.CanonicalOrder"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK009.CreateOrder.STIP.Schemas.PropertySchema"" targetNamespace=""http://INTSTDK009.CreateOrder.STIP.Schemas.CanonicalOrder"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK009.CreateOrder.STIP.Schemas.PropertySchema"" location=""INTSTDK009.STIP.Schemas.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""Order"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo rootTypeName=""OrderType"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" />
        <b:properties>
          <b:property name=""ns0:RoutingCode"" xpath=""/*[local-name()='Order' and namespace-uri()='http://INTSTDK009.CreateOrder.STIP.Schemas.CanonicalOrder']/*[local-name()='RoutingCode' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""RoutingCode"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""OrderNo"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""OrderTime"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ShippingAddress"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ContactPerson"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""CellPhone"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""EMail"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Address1"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Address2"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""PostalCode"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""City"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""CountryCode"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Products"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Product"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""Reference"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ProductCode"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Qty"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""NameOnCard"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CanonicalOrder() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Order";
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
