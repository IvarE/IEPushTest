namespace INTSTDK003.CRM.Schemas.Customer_TravelCard {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CheckCustomerExist", @"CheckCustomerExistResponse", @"CreateCustomer", @"CreateCustomerResponse", @"GetCustomer", @"GetCustomerResponse", @"UpdateCustomer", @"UpdateCustomerResponse", @"RegisterTravelCard", @"RegisterTravelCardResponse", @"UpdateTravelCard", @"UpdateTravelCardResponse", @"GetCardsForCustomer", @"GetCardsForCustomerResponse", @"UnregisterTravelCard", @"UnregisterTravelCardResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2_3", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2_3))]
    public sealed class PortalService : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://tempuri.org/"" elementFormDefault=""qualified"" targetNamespace=""http://tempuri.org/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1"" namespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2"" namespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" />
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2_3"" namespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
        <reference targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" />
        <reference targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CheckCustomerExist"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""customerEmail"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q1=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q1:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CheckCustomerExistResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""CheckCustomerExistResult"" nillable=""true"" type=""q2:CheckCustomerExistResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CreateCustomer"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q3=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""customer"" nillable=""true"" type=""q3:Customer"" />
        <xs:element xmlns:q4=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q4:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CreateCustomerResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q5=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""CreateCustomerResult"" nillable=""true"" type=""q5:CreateCustomerResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCustomer"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q6=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""customerId"" type=""q6:guid"" />
        <xs:element xmlns:q7=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""accountType"" type=""q7:AccountCategoryCode"" />
        <xs:element xmlns:q8=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q8:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCustomerResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q9=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""GetCustomerResult"" nillable=""true"" type=""q9:GetCustomerResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""UpdateCustomer"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q10=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""customerId"" type=""q10:guid"" />
        <xs:element xmlns:q11=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""customer"" nillable=""true"" type=""q11:Customer"" />
        <xs:element xmlns:q12=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q12:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""UpdateCustomerResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q13=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""UpdateCustomerResult"" nillable=""true"" type=""q13:UpdateCustomerResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RegisterTravelCard"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q14=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" minOccurs=""0"" name=""travelCard"" nillable=""true"" type=""q14:TravelCard"" />
        <xs:element xmlns:q15=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q15:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RegisterTravelCardResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q16=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""RegisterTravelCardResult"" nillable=""true"" type=""q16:RegisterTravelCardResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""UpdateTravelCard"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q17=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" minOccurs=""0"" name=""travelCard"" nillable=""true"" type=""q17:TravelCard"" />
        <xs:element xmlns:q18=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q18:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""UpdateTravelCardResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q19=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""UpdateTravelCardResult"" nillable=""true"" type=""q19:UpdateTravelCardResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCardsForCustomer"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q20=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""customerId"" type=""q20:guid"" />
        <xs:element xmlns:q21=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""customerType"" type=""q21:AccountCategoryCode"" />
        <xs:element xmlns:q22=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q22:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCardsForCustomerResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q23=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""GetCardsForCustomerResult"" nillable=""true"" type=""q23:GetCardsForCustomerResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""UnregisterTravelCard"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q24=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""customerId"" type=""q24:guid"" />
        <xs:element xmlns:q25=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""customerType"" type=""q25:AccountCategoryCode"" />
        <xs:element minOccurs=""0"" name=""travelCardNumber"" nillable=""true"" type=""xs:string"" />
        <xs:element xmlns:q26=""http://schemas.microsoft.com/2003/10/Serialization/"" minOccurs=""0"" name=""callerId"" type=""q26:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""UnregisterTravelCardResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q27=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" minOccurs=""0"" name=""UnregisterTravelCardResult"" nillable=""true"" type=""q27:UnRegisterTravelCardResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public PortalService() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [16];
                _RootElements[0] = "CheckCustomerExist";
                _RootElements[1] = "CheckCustomerExistResponse";
                _RootElements[2] = "CreateCustomer";
                _RootElements[3] = "CreateCustomerResponse";
                _RootElements[4] = "GetCustomer";
                _RootElements[5] = "GetCustomerResponse";
                _RootElements[6] = "UpdateCustomer";
                _RootElements[7] = "UpdateCustomerResponse";
                _RootElements[8] = "RegisterTravelCard";
                _RootElements[9] = "RegisterTravelCardResponse";
                _RootElements[10] = "UpdateTravelCard";
                _RootElements[11] = "UpdateTravelCardResponse";
                _RootElements[12] = "GetCardsForCustomer";
                _RootElements[13] = "GetCardsForCustomerResponse";
                _RootElements[14] = "UnregisterTravelCard";
                _RootElements[15] = "UnregisterTravelCardResponse";
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
        
        [Schema(@"http://tempuri.org/",@"CheckCustomerExist")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CheckCustomerExist"})]
        public sealed class CheckCustomerExist : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CheckCustomerExist() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CheckCustomerExist";
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
        
        [Schema(@"http://tempuri.org/",@"CheckCustomerExistResponse")]
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
        
        [Schema(@"http://tempuri.org/",@"CreateCustomer")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateCustomer"})]
        public sealed class CreateCustomer : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateCustomer() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateCustomer";
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
        
        [Schema(@"http://tempuri.org/",@"CreateCustomerResponse")]
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
        
        [Schema(@"http://tempuri.org/",@"GetCustomer")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomer"})]
        public sealed class GetCustomer : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomer() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomer";
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
        
        [Schema(@"http://tempuri.org/",@"GetCustomerResponse")]
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
        
        [Schema(@"http://tempuri.org/",@"UpdateCustomer")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UpdateCustomer"})]
        public sealed class UpdateCustomer : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UpdateCustomer() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UpdateCustomer";
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
        
        [Schema(@"http://tempuri.org/",@"UpdateCustomerResponse")]
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
        
        [Schema(@"http://tempuri.org/",@"RegisterTravelCard")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RegisterTravelCard"})]
        public sealed class RegisterTravelCard : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RegisterTravelCard() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RegisterTravelCard";
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
        
        [Schema(@"http://tempuri.org/",@"RegisterTravelCardResponse")]
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
        
        [Schema(@"http://tempuri.org/",@"UpdateTravelCard")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UpdateTravelCard"})]
        public sealed class UpdateTravelCard : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UpdateTravelCard() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UpdateTravelCard";
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
        
        [Schema(@"http://tempuri.org/",@"UpdateTravelCardResponse")]
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
        
        [Schema(@"http://tempuri.org/",@"GetCardsForCustomer")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCardsForCustomer"})]
        public sealed class GetCardsForCustomer : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCardsForCustomer() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCardsForCustomer";
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
        
        [Schema(@"http://tempuri.org/",@"GetCardsForCustomerResponse")]
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
        
        [Schema(@"http://tempuri.org/",@"UnregisterTravelCard")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UnregisterTravelCard"})]
        public sealed class UnregisterTravelCard : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UnregisterTravelCard() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UnregisterTravelCard";
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
        
        [Schema(@"http://tempuri.org/",@"UnregisterTravelCardResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UnregisterTravelCardResponse"})]
        public sealed class UnregisterTravelCardResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UnregisterTravelCardResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UnregisterTravelCardResponse";
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
