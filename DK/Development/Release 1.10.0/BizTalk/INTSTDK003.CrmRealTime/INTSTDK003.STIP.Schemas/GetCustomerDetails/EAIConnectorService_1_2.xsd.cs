namespace INTSTDK003.STIP.Schemas.GetCustomerDetails {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"AccountCategoryCode", @"GetCustomerDetailsResponse", @"Response", @"ProcessingStatus", @"Customer", @"ArrayOfAddress", @"Address", @"AddressTypeCode", @"GetCustomerIdForTravelCardResponse", @"ArrayOfDetail", @"Detail"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1))]
    public sealed class EAIConnectorService_1_2 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ser=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" xmlns:tns=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:simpleType name=""AccountCategoryCode"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""Private"">
        <xs:annotation>
          <xs:appinfo>
            <EnumerationValue xmlns=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"">1</EnumerationValue>
          </xs:appinfo>
        </xs:annotation>
      </xs:enumeration>
      <xs:enumeration value=""Company"">
        <xs:annotation>
          <xs:appinfo>
            <EnumerationValue xmlns=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"">2</EnumerationValue>
          </xs:appinfo>
        </xs:annotation>
      </xs:enumeration>
    </xs:restriction>
  </xs:simpleType>
  <xs:element name=""AccountCategoryCode"" nillable=""true"" type=""tns:AccountCategoryCode"" />
  <xs:complexType name=""GetCustomerDetailsResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence>
          <xs:element minOccurs=""0"" name=""Customer"" nillable=""true"" type=""tns:Customer"" />
          <xs:element minOccurs=""0"" name=""RequestAccountCategoryCode"" type=""tns:AccountCategoryCode"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""GetCustomerDetailsResponse"" nillable=""true"" type=""tns:GetCustomerDetailsResponse"" />
  <xs:complexType name=""Response"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""IsNull"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""Message"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Status"" type=""tns:ProcessingStatus"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""Response"" nillable=""true"" type=""tns:Response"" />
  <xs:simpleType name=""ProcessingStatus"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""SUCCESS"" />
      <xs:enumeration value=""FAILED"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:element name=""ProcessingStatus"" nillable=""true"" type=""tns:ProcessingStatus"" />
  <xs:complexType name=""Customer"">
    <xs:sequence>
      <xs:element name=""AccountFirstName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""AccountId"" type=""ser:guid"" />
      <xs:element name=""AccountLastName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""AccountNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Addresses"" nillable=""true"" type=""tns:ArrayOfAddress"" />
      <xs:element minOccurs=""0"" name=""AllowAutoLoad"" type=""xs:boolean"" />
      <xs:element name=""CompanyName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Counterpart"" nillable=""true"" type=""xs:string"" />
      <xs:element name=""CustomerType"" type=""tns:AccountCategoryCode"" />
      <xs:element minOccurs=""0"" name=""Deleted"" type=""xs:boolean"" />
      <xs:element name=""Email"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""InActive"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""MaxCardsAutoLoad"" type=""xs:int"" />
      <xs:element name=""MobilePhone"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""OrganizationCreditApproved"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""OrganizationNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""OrganizationSubNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Phone"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Responsibility"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Rsid"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""SocialSecurityNumber"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""Customer"" nillable=""true"" type=""tns:Customer"" />
  <xs:complexType name=""ArrayOfAddress"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Address"" nillable=""true"" type=""tns:Address"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfAddress"" nillable=""true"" type=""tns:ArrayOfAddress"" />
  <xs:complexType name=""Address"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""AddressId"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""AddressType"" type=""tns:AddressTypeCode"" />
      <xs:element minOccurs=""0"" name=""CareOff"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""City"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CompanyName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""ContactPerson"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""ContactPhoneNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Country"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""County"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CustomerAddressId"" type=""ser:guid"" />
      <xs:element minOccurs=""0"" name=""PostalCode"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""SMSNotificationNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Street"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""Address"" nillable=""true"" type=""tns:Address"" />
  <xs:simpleType name=""AddressTypeCode"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""None"" />
      <xs:enumeration value=""Invoice"" />
      <xs:enumeration value=""Delivery"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:element name=""AddressTypeCode"" nillable=""true"" type=""tns:AddressTypeCode"" />
  <xs:complexType name=""GetCustomerIdForTravelCardResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence>
          <xs:element minOccurs=""0"" name=""Details"" nillable=""true"" type=""tns:ArrayOfDetail"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""GetCustomerIdForTravelCardResponse"" nillable=""true"" type=""tns:GetCustomerIdForTravelCardResponse"" />
  <xs:complexType name=""ArrayOfDetail"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Detail"" nillable=""true"" type=""tns:Detail"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfDetail"" nillable=""true"" type=""tns:ArrayOfDetail"" />
  <xs:complexType name=""Detail"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""CustomerId"" type=""ser:guid"" />
      <xs:element minOccurs=""0"" name=""CustomerType"" type=""tns:AccountCategoryCode"" />
      <xs:element minOccurs=""0"" name=""TravelCardName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelCardNumber"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""Detail"" nillable=""true"" type=""tns:Detail"" />
</xs:schema>";
        
        public EAIConnectorService_1_2() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [11];
                _RootElements[0] = "AccountCategoryCode";
                _RootElements[1] = "GetCustomerDetailsResponse";
                _RootElements[2] = "Response";
                _RootElements[3] = "ProcessingStatus";
                _RootElements[4] = "Customer";
                _RootElements[5] = "ArrayOfAddress";
                _RootElements[6] = "Address";
                _RootElements[7] = "AddressTypeCode";
                _RootElements[8] = "GetCustomerIdForTravelCardResponse";
                _RootElements[9] = "ArrayOfDetail";
                _RootElements[10] = "Detail";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"AccountCategoryCode")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"AccountCategoryCode"})]
        public sealed class AccountCategoryCode : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public AccountCategoryCode() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "AccountCategoryCode";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"GetCustomerDetailsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomerDetailsResponse"})]
        public sealed class GetCustomerDetailsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomerDetailsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomerDetailsResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"Response")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"Response"})]
        public sealed class Response : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Response() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "Response";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"ProcessingStatus")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ProcessingStatus"})]
        public sealed class ProcessingStatus : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ProcessingStatus() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ProcessingStatus";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"Customer")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"Customer"})]
        public sealed class Customer : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Customer() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "Customer";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"ArrayOfAddress")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfAddress"})]
        public sealed class ArrayOfAddress : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfAddress() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfAddress";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"Address")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"Address"})]
        public sealed class Address : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Address() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "Address";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"AddressTypeCode")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"AddressTypeCode"})]
        public sealed class AddressTypeCode : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public AddressTypeCode() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "AddressTypeCode";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"GetCustomerIdForTravelCardResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomerIdForTravelCardResponse"})]
        public sealed class GetCustomerIdForTravelCardResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomerIdForTravelCardResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomerIdForTravelCardResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"ArrayOfDetail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfDetail"})]
        public sealed class ArrayOfDetail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfDetail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfDetail";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126",@"Detail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"Detail"})]
        public sealed class Detail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Detail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "Detail";
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
