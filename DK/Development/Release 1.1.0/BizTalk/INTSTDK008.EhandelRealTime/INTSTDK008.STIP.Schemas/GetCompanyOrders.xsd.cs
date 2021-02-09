namespace INTSTDK008.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.STIP.Schemas.createdFrom), XPath = @"/*[local-name()='GetCompanyOrdersRequest' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031']/*[local-name()='createdFrom' and namespace-uri()='']", XsdType = @"dateTime")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCompanyOrdersRequest", @"GetCompanyOrdersRequestResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.PropertySchema", typeof(global::INTSTDK008.STIP.Schemas.PropertySchema))]
    public sealed class GetCompanyOrders : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:ns2=""https://INTSTDK008.STIP.Schemas.PropertySchema"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008/PropertySchema/20141119"" xmlns=""http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031"" xmlns:ns1=""http://INTSTDK008.STIP.Schemas.PropertySchema"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns1"" uri=""http://INTSTDK008.STIP.Schemas.PropertySchema"" location=""INTSTDK008.STIP.Schemas.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetCompanyOrdersRequest"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns1:createdFrom"" xpath=""/*[local-name()='GetCompanyOrdersRequest' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031']/*[local-name()='createdFrom' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""createdFrom"" type=""xs:dateTime"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCompanyOrdersRequestResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Orders"" nillable=""true"" type=""ArrayOfOrder"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:complexType name=""ArrayOfOrder"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Order"" nillable=""true"" type=""Order"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""Order"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OrderNumber"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OrderStatus"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Currency"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Language"" type=""xs:string"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""OrderDate"" type=""xs:dateTime"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""OrderTotal"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""OrderTotalVat"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""OrderDiscount"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""OrderDiscountVat"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PaymentTotal"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PaymentTotalVat"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ShippingTotal"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ShippingTotalVat"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CustomerNumber"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CustomerNumber2"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CustomerOrganizationNumber"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CustomerName"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OrderItems"" type=""ArrayOfOrderItem"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""PaymentTypes"" type=""ArrayOfPaymentType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""ArrayOfOrderItem"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""OrderItem"" nillable=""true"" type=""OrderItem"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""OrderItem"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Code"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Name"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CardNumber"" type=""xs:string"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Amount"" type=""xs:int"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Price"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Tax"" type=""xs:decimal"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Discount"" type=""xs:decimal"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""ArrayOfPaymentType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PaymentType"" nillable=""true"" type=""PaymentType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""PaymentType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Code"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Name"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ReferenceNumber"" type=""xs:string"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Sum"" type=""xs:decimal"" />
    </xs:sequence>
  </xs:complexType>
</xs:schema>";
        
        public GetCompanyOrders() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "GetCompanyOrdersRequest";
                _RootElements[1] = "GetCompanyOrdersRequestResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031",@"GetCompanyOrdersRequest")]
        [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.STIP.Schemas.createdFrom), XPath = @"/*[local-name()='GetCompanyOrdersRequest' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031']/*[local-name()='createdFrom' and namespace-uri()='']", XsdType = @"dateTime")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCompanyOrdersRequest"})]
        public sealed class GetCompanyOrdersRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCompanyOrdersRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCompanyOrdersRequest";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031",@"GetCompanyOrdersRequestResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCompanyOrdersRequestResponse"})]
        public sealed class GetCompanyOrdersRequestResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCompanyOrdersRequestResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCompanyOrdersRequestResponse";
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
}
