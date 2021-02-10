namespace INTSTDK008.Card.Ehandel.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Card.Ehandel.Schemas.CardNumber), XPath = @"/*[local-name()='OutstandingChargesRequest' and namespace-uri()='http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges']/*[local-name()='CardNumber' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"OutstandingChargesRequest", @"OutstandingChargesResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.PropertySchema", typeof(global::INTSTDK008.Card.Ehandel.Schemas.PropertySchema))]
    public sealed class GetOutstandingCharges : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK008.Card.Ehandel.Schemas.PropertySchema"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" targetNamespace=""http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK008.Card.Ehandel.Schemas.PropertySchema"" location=""INTSTDK008.Card.Ehandel.Schemas.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""OutstandingChargesRequest"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:CardNumber"" xpath=""/*[local-name()='OutstandingChargesRequest' and namespace-uri()='http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges']/*[local-name()='CardNumber' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""CardNumber"">
          <xs:simpleType>
            <xs:restriction base=""xs:string"">
              <xs:length value=""50"" />
            </xs:restriction>
          </xs:simpleType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""OutstandingChargesResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""Message"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""HasOutstandingCharge"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" name=""HasExpiredCharge"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" name=""Amount"" type=""xs:unsignedShort"" />
        <xs:element minOccurs=""0"" name=""ErrorMessage"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetOutstandingCharges() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "OutstandingChargesRequest";
                _RootElements[1] = "OutstandingChargesResponse";
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
        
        [Schema(@"http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges",@"OutstandingChargesRequest")]
        [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.Card.Ehandel.Schemas.CardNumber), XPath = @"/*[local-name()='OutstandingChargesRequest' and namespace-uri()='http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges']/*[local-name()='CardNumber' and namespace-uri()='']", XsdType = @"string")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"OutstandingChargesRequest"})]
        public sealed class OutstandingChargesRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public OutstandingChargesRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "OutstandingChargesRequest";
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
        
        [Schema(@"http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges",@"OutstandingChargesResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"OutstandingChargesResponse"})]
        public sealed class OutstandingChargesResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public OutstandingChargesResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "OutstandingChargesResponse";
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
