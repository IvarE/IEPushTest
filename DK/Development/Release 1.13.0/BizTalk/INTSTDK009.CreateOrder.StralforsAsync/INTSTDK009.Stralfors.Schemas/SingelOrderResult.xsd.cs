namespace INTSTDK009.Stralfors.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"Order")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK009.Stralfors.Schemas.PropertySchema.OrderNo), XPath = @"/*[local-name()='Order' and namespace-uri()='']/*[local-name()='OrderNo' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Order"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.Stralfors.Schemas.PropertySchema.PropertySchema", typeof(global::INTSTDK009.Stralfors.Schemas.PropertySchema.PropertySchema))]
    public sealed class SingelOrderResult : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK009.Stralfors.Schemas.PropertySchema"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK009.Stralfors.Schemas.PropertySchema"" location=""INTSTDK009.Stralfors.Schemas.PropertySchema.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""Order"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:OrderNo"" xpath=""/*[local-name()='Order' and namespace-uri()='']/*[local-name()='OrderNo' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""OrderNo"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""OrderTime"" type=""xs:string"" />
        <xs:choice minOccurs=""0"">
          <xs:element minOccurs=""0"" name=""ShippingAdress"">
            <xs:complexType>
              <xs:sequence>
                <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""ContactPerson"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""CallPhone"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""EMail"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""Adress1"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""Adress2"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""PostalCode"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""City"" type=""xs:string"" />
                <xs:element minOccurs=""0"" name=""CountryCode"" type=""xs:string"" />
              </xs:sequence>
            </xs:complexType>
          </xs:element>
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
        </xs:choice>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PacketDesc"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""PacketId"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Products"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""1"" maxOccurs=""unbounded"" name=""Product"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""Reference"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ProductCode"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Serial"" type=""xs:string"" />
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
        
        public SingelOrderResult() {
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
