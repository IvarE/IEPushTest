namespace INTSTDK008.Ehandel.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest",@"GetOrdersRequest")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.customerId), XPath = @"/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='CustomerId' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.orderNumber), XPath = @"/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='OrderNumber' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.from), XPath = @"/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='From' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.to), XPath = @"/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='To' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.Email), XPath = @"/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='Email' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.CardNumber), XPath = @"/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='CardNumber' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetOrdersRequest"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.PropertySchema.PropertySchema", typeof(global::INTSTDK008.Ehandel.Schemas.PropertySchema.PropertySchema))]
    public sealed class GetOrdersRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK008.Ehandel.Schemas.PropertySchema"" targetNamespace=""http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK008.Ehandel.Schemas.PropertySchema"" location=""INTSTDK008.Ehandel.Schemas.PropertySchema.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetOrdersRequest"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:customerId"" xpath=""/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='CustomerId' and namespace-uri()='']"" />
          <b:property name=""ns0:orderNumber"" xpath=""/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='OrderNumber' and namespace-uri()='']"" />
          <b:property name=""ns0:from"" xpath=""/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='From' and namespace-uri()='']"" />
          <b:property name=""ns0:to"" xpath=""/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='To' and namespace-uri()='']"" />
          <b:property name=""ns0:Email"" xpath=""/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='Email' and namespace-uri()='']"" />
          <b:property name=""ns0:CardNumber"" xpath=""/*[local-name()='GetOrdersRequest' and namespace-uri()='http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest']/*[local-name()='CardNumber' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""CustomerId"" type=""xs:string"" />
        <xs:element name=""OrderNumber"" type=""xs:string"" />
        <xs:element name=""From"" type=""xs:string"" />
        <xs:element name=""To"" type=""xs:string"" />
        <xs:element name=""Email"" type=""xs:string"" />
        <xs:element name=""CardNumber"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetOrdersRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetOrdersRequest";
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
