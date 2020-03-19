namespace INTSTDK003.Case.CRM.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreateCaseRequest", @"CustomerType", @"ArrayOfdocument", @"document", @"CreateAutoRGCaseRequest", @"CreateAutoRGCaseResponse", @"UpdateAutoRGCaseRequest", @"UpdateAutoRGCaseResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.Case.CRM.Schemas.CreateCase_1_2", typeof(global::INTSTDK003.Case.CRM.Schemas.CreateCase_1_2))]
    public sealed class CreateCase_1 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ser=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:tns=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK003.Case.CRM.Schemas.CreateCase_1_2"" namespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""CreateCaseRequest"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""CardNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""City"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""ContactCustomer"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""ControlFeeNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""County"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Customer"" nillable=""true"" type=""ser:guid"" />
      <xs:element minOccurs=""0"" name=""CustomerType"" type=""tns:CustomerType"" />
      <xs:element minOccurs=""0"" name=""CustomersCategory"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CustomersSubcategory"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Description"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""DocumentList"" nillable=""true"" type=""tns:ArrayOfdocument"" />
      <xs:element minOccurs=""0"" name=""EmailAddress"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""FirstName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""InvoiceNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LastName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Line"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""MobilePhoneNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Title"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Train"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""WayOfTravel"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""CreateCaseRequest"" nillable=""true"" type=""tns:CreateCaseRequest"" />
  <xs:simpleType name=""CustomerType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""Private"" />
      <xs:enumeration value=""Organisation"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:element name=""CustomerType"" nillable=""true"" type=""tns:CustomerType"" />
  <xs:complexType name=""ArrayOfdocument"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""document"" nillable=""true"" type=""tns:document"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfdocument"" nillable=""true"" type=""tns:ArrayOfdocument"" />
  <xs:complexType name=""document"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""DocumentBody"" nillable=""true"" type=""xs:base64Binary"" />
      <xs:element minOccurs=""0"" name=""FileName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""NoteText"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Subject"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""document"" nillable=""true"" type=""tns:document"" />
  <xs:complexType name=""CreateAutoRGCaseRequest"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""CardNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""City"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Customer"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CustomerType"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""CustomersCategory"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CustomersSubcategory"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""DepartureDateTime"" nillable=""true"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""Description"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""EmailAddress"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""ExperiencedDelay"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""FirstName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""LastName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Line"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""MobileNo"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""RGOLIssueID"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Title"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""WayOfTravel"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""CreateAutoRGCaseRequest"" nillable=""true"" type=""tns:CreateAutoRGCaseRequest"" />
  <xs:complexType name=""CreateAutoRGCaseResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""CaseID"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""ErrorMessage"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Success"" type=""xs:boolean"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""CreateAutoRGCaseResponse"" nillable=""true"" type=""tns:CreateAutoRGCaseResponse"" />
  <xs:complexType name=""UpdateAutoRGCaseRequest"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""Approved"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""CaseID"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Currency"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CustomerMessage"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""DelayType"" nillable=""true"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""InternalMessage"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""RefundType"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""TravelInformationArrivalActual"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationArrivalPlanned"" nillable=""true"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""TravelInformationCity"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationCompany"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationDeviationMessage"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationDirectionText"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationDisplayText"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationLine"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationStart"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationStartActual"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationStartPlanned"" nillable=""true"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""TravelInformationStop"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationTitle"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationTour"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""TravelInformationTransport"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Value"" type=""xs:decimal"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""UpdateAutoRGCaseRequest"" nillable=""true"" type=""tns:UpdateAutoRGCaseRequest"" />
  <xs:complexType name=""UpdateAutoRGCaseResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""ErrorMessage"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""RefundID"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Success"" type=""xs:boolean"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""UpdateAutoRGCaseResponse"" nillable=""true"" type=""tns:UpdateAutoRGCaseResponse"" />
</xs:schema>";
        
        public CreateCase_1() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [8];
                _RootElements[0] = "CreateCaseRequest";
                _RootElements[1] = "CustomerType";
                _RootElements[2] = "ArrayOfdocument";
                _RootElements[3] = "document";
                _RootElements[4] = "CreateAutoRGCaseRequest";
                _RootElements[5] = "CreateAutoRGCaseResponse";
                _RootElements[6] = "UpdateAutoRGCaseRequest";
                _RootElements[7] = "UpdateAutoRGCaseResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"CreateCaseRequest")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateCaseRequest"})]
        public sealed class CreateCaseRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateCaseRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateCaseRequest";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"CustomerType")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CustomerType"})]
        public sealed class CustomerType : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CustomerType() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CustomerType";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"ArrayOfdocument")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfdocument"})]
        public sealed class ArrayOfdocument : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfdocument() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfdocument";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"document")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"document"})]
        public sealed class document : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public document() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "document";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"CreateAutoRGCaseRequest")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateAutoRGCaseRequest"})]
        public sealed class CreateAutoRGCaseRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateAutoRGCaseRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateAutoRGCaseRequest";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"CreateAutoRGCaseResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreateAutoRGCaseResponse"})]
        public sealed class CreateAutoRGCaseResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreateAutoRGCaseResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreateAutoRGCaseResponse";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"UpdateAutoRGCaseRequest")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UpdateAutoRGCaseRequest"})]
        public sealed class UpdateAutoRGCaseRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UpdateAutoRGCaseRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UpdateAutoRGCaseRequest";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService",@"UpdateAutoRGCaseResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"UpdateAutoRGCaseResponse"})]
        public sealed class UpdateAutoRGCaseResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public UpdateAutoRGCaseResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "UpdateAutoRGCaseResponse";
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
