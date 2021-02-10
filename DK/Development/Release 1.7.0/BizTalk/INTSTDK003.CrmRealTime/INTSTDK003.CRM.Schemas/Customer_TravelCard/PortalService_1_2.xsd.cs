namespace INTSTDK003.CRM.Schemas.Customer_TravelCard {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CheckCustomerExistResponse", @"Response", @"ProcessingStatus", @"AccountCategoryCode", @"Customer", @"ArrayOfAddress", @"Address", @"AddressTypeCode", @"CreateCustomerResponse", @"GetCustomerResponse", @"UpdateCustomerResponse", @"RegisterTravelCardResponse", @"UpdateTravelCardResponse", @"GetCardsForCustomerResponse", @"UnRegisterTravelCardResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2_3", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2_3))]
    public sealed class PortalService_1_2 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ser=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:tns=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1"" namespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2_3"" namespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
        <b:reference targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" />
        <b:reference targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""CheckCustomerExistResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence>
          <xs:element minOccurs=""0"" name=""AccountId"" type=""ser:guid"" />
          <xs:element minOccurs=""0"" name=""AccountNumber"" nillable=""true"" type=""xs:string"" />
          <xs:element minOccurs=""0"" name=""CustomerExists"" type=""xs:boolean"" />
          <xs:element minOccurs=""0"" name=""CustomerType"" nillable=""true"" type=""tns:AccountCategoryCode"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""CheckCustomerExistResponse"" nillable=""true"" type=""tns:CheckCustomerExistResponse"" />
  <xs:complexType name=""Response"">
    <xs:sequence>
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
  <xs:simpleType name=""AccountCategoryCode"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""Private"">
        <xs:annotation>
          <xs:appinfo>
            <EnumerationValue xmlns=""http://schemas.microsoft.com/2003/10/Serialization/"">1</EnumerationValue>
          </xs:appinfo>
        </xs:annotation>
      </xs:enumeration>
      <xs:enumeration value=""Company"">
        <xs:annotation>
          <xs:appinfo>
            <EnumerationValue xmlns=""http://schemas.microsoft.com/2003/10/Serialization/"">2</EnumerationValue>
          </xs:appinfo>
        </xs:annotation>
      </xs:enumeration>
    </xs:restriction>
  </xs:simpleType>
  <xs:element name=""AccountCategoryCode"" nillable=""true"" type=""tns:AccountCategoryCode"" />
  <xs:complexType name=""Customer"">
    <xs:sequence>
      <xs:element name=""AccountFirstName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""AccountId"" type=""ser:guid"" />
      <xs:element name=""AccountLastName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""AccountNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Addresses"" nillable=""true"" type=""tns:ArrayOfAddress"" />
      <xs:element minOccurs=""0"" name=""AllowAutoLoad"" type=""xs:boolean"" />
      <xs:element name=""CompanyName"" nillable=""true"" type=""xs:string"" />
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
      <xs:element minOccurs=""0"" name=""SocialSecurtiyNumber"" nillable=""true"" type=""xs:string"" />
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
      <xs:element minOccurs=""0"" name=""EmailNotificationAddress"" nillable=""true"" type=""xs:string"" />
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
  <xs:complexType name=""CreateCustomerResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence>
          <xs:element minOccurs=""0"" name=""AccountId"" type=""ser:guid"" />
          <xs:element minOccurs=""0"" name=""AccountNumber"" nillable=""true"" type=""xs:string"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""CreateCustomerResponse"" nillable=""true"" type=""tns:CreateCustomerResponse"" />
  <xs:complexType name=""GetCustomerResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence>
          <xs:element minOccurs=""0"" name=""Customer"" nillable=""true"" type=""tns:Customer"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""GetCustomerResponse"" nillable=""true"" type=""tns:GetCustomerResponse"" />
  <xs:complexType name=""UpdateCustomerResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""UpdateCustomerResponse"" nillable=""true"" type=""tns:UpdateCustomerResponse"" />
  <xs:complexType name=""RegisterTravelCardResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""RegisterTravelCardResponse"" nillable=""true"" type=""tns:RegisterTravelCardResponse"" />
  <xs:complexType name=""UpdateTravelCardResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""UpdateTravelCardResponse"" nillable=""true"" type=""tns:UpdateTravelCardResponse"" />
  <xs:complexType name=""GetCardsForCustomerResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence>
          <xs:element xmlns:q1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" minOccurs=""0"" name=""TravelCards"" nillable=""true"" type=""q1:ArrayOfTravelCard"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""GetCardsForCustomerResponse"" nillable=""true"" type=""tns:GetCardsForCustomerResponse"" />
  <xs:complexType name=""UnRegisterTravelCardResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:Response"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""UnRegisterTravelCardResponse"" nillable=""true"" type=""tns:UnRegisterTravelCardResponse"" />
</xs:schema>";
        
        public PortalService_1_2() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [15];
                _RootElements[0] = "CheckCustomerExistResponse";
                _RootElements[1] = "Response";
                _RootElements[2] = "ProcessingStatus";
                _RootElements[3] = "AccountCategoryCode";
                _RootElements[4] = "Customer";
                _RootElements[5] = "ArrayOfAddress";
                _RootElements[6] = "Address";
                _RootElements[7] = "AddressTypeCode";
                _RootElements[8] = "CreateCustomerResponse";
                _RootElements[9] = "GetCustomerResponse";
                _RootElements[10] = "UpdateCustomerResponse";
                _RootElements[11] = "RegisterTravelCardResponse";
                _RootElements[12] = "UpdateTravelCardResponse";
                _RootElements[13] = "GetCardsForCustomerResponse";
                _RootElements[14] = "UnRegisterTravelCardResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"CheckCustomerExistResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CheckCustomerExistResponse"})]
        public sealed class CheckCustomerExistResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CheckCustomerExistResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CheckCustomerExistResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"Response")]
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"ProcessingStatus")]
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"AccountCategoryCode")]
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"Customer")]
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"ArrayOfAddress")]
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"Address")]
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"AddressTypeCode")]
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"CreateCustomerResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateCustomerResponse"})]
        public sealed class CreateCustomerResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateCustomerResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateCustomerResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"GetCustomerResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomerResponse"})]
        public sealed class GetCustomerResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomerResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomerResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"UpdateCustomerResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UpdateCustomerResponse"})]
        public sealed class UpdateCustomerResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UpdateCustomerResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UpdateCustomerResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"RegisterTravelCardResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RegisterTravelCardResponse"})]
        public sealed class RegisterTravelCardResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RegisterTravelCardResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RegisterTravelCardResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"UpdateTravelCardResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UpdateTravelCardResponse"})]
        public sealed class UpdateTravelCardResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UpdateTravelCardResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UpdateTravelCardResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"GetCardsForCustomerResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCardsForCustomerResponse"})]
        public sealed class GetCardsForCustomerResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCardsForCustomerResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCardsForCustomerResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models",@"UnRegisterTravelCardResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UnRegisterTravelCardResponse"})]
        public sealed class UnRegisterTravelCardResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UnRegisterTravelCardResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UnRegisterTravelCardResponse";
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
