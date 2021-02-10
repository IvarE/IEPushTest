namespace INTSTDK012.Kuponginlosen.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"createCouponForPrinting", @"createCouponForPrintingResponse", @"createMobileCoupon", @"createMobileCouponResponse", @"createUniqueCoupon", @"createUniqueCouponBatch", @"createUniqueCouponBatchResponse", @"createUniqueCouponResponse", @"createUniqueCouponWithEanCode", @"createUniqueCouponWithEanCodeResponse", @"getHeartbeat", @"getHeartbeatResponse", @"InvalidConfiguration", @"InternalError", @"InvalidAmount", @"InvalidCampaign", @"NoUniqueCodeCreated", @"InvalidField", @"InvalidEanCode"})]
    public sealed class CouponCreatorService : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://service.web.couponcreator.kuponginlosen.se/"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" targetNamespace=""http://service.web.couponcreator.kuponginlosen.se/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""createCouponForPrinting"" type=""tns:createCouponForPrinting"" />
  <xs:element name=""createCouponForPrintingResponse"" type=""tns:createCouponForPrintingResponse"" />
  <xs:element name=""createMobileCoupon"" type=""tns:createMobileCoupon"" />
  <xs:element name=""createMobileCouponResponse"" type=""tns:createMobileCouponResponse"" />
  <xs:element name=""createUniqueCoupon"" type=""tns:createUniqueCoupon"" />
  <xs:element name=""createUniqueCouponBatch"" type=""tns:createUniqueCouponBatch"" />
  <xs:element name=""createUniqueCouponBatchResponse"" type=""tns:createUniqueCouponBatchResponse"" />
  <xs:element name=""createUniqueCouponResponse"" type=""tns:createUniqueCouponResponse"" />
  <xs:element name=""createUniqueCouponWithEanCode"" type=""tns:createUniqueCouponWithEanCode"" />
  <xs:element name=""createUniqueCouponWithEanCodeResponse"" type=""tns:createUniqueCouponWithEanCodeResponse"" />
  <xs:element name=""getHeartbeat"" type=""tns:getHeartbeat"" />
  <xs:element name=""getHeartbeatResponse"" type=""tns:getHeartbeatResponse"" />
  <xs:complexType name=""createUniqueCouponBatch"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""campaignNumberBatchRequest"" type=""tns:campaignNumberBatchRequest"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""campaignNumberBatchRequest"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:campaignNumberRequest"">
        <xs:sequence>
          <xs:element name=""quantity"" type=""xs:int"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:complexType name=""campaignNumberRequest"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:abstractRequest"">
        <xs:sequence>
          <xs:element name=""campaignNumber"" type=""xs:int"" />
          <xs:element name=""amount"" type=""xs:decimal"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:complexType name=""abstractRequest"" abstract=""true"">
    <xs:sequence>
      <xs:element name=""validityInDays"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""createUniqueCouponBatchResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""return"" type=""tns:batchResponse"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""batchResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:abstractResponse"">
        <xs:sequence>
          <xs:element minOccurs=""0"" name=""fileName"" type=""xs:string"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:complexType name=""abstractResponse"" abstract=""true"">
    <xs:sequence>
      <xs:element name=""eanCode"" type=""xs:long"" />
      <xs:element minOccurs=""0"" name=""lastRedemptionDate"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""createUniqueCoupon"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""campaignNumberRequest"" type=""tns:campaignNumberRequest"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""createUniqueCouponResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""return"" type=""tns:singleResponse"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""singleResponse"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:abstractResponse"">
        <xs:sequence>
          <xs:element minOccurs=""0"" name=""uniqueCode"" type=""xs:string"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:complexType name=""createMobileCoupon"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""campaignNumberRequest"" type=""tns:campaignNumberRequest"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""createMobileCouponResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""return"" type=""tns:singleResponse"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""createCouponForPrinting"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""campaignNumberRequestWithAddress"" type=""tns:campaignNumberRequestWithAddress"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""campaignNumberRequestWithAddress"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:campaignNumberRequest"">
        <xs:sequence>
          <xs:element name=""firstName"" type=""xs:string"" />
          <xs:element name=""familyName"" type=""xs:string"" />
          <xs:element name=""socialSecurityNumber"" type=""xs:string"" />
          <xs:element name=""street"" type=""xs:string"" />
          <xs:element name=""streetNumber"" type=""xs:int"" />
          <xs:element minOccurs=""0"" name=""coAddress"" type=""xs:string"" />
          <xs:element name=""zipCode"" type=""xs:int"" />
          <xs:element name=""city"" type=""xs:string"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:complexType name=""createCouponForPrintingResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""return"" type=""tns:couponForPrintingResponse"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""couponForPrintingResponse"">
    <xs:sequence>
      <xs:element name=""referenceNumber"" type=""xs:long"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""getHeartbeat"">
    <xs:sequence />
  </xs:complexType>
  <xs:complexType name=""getHeartbeatResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""return"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""createUniqueCouponWithEanCode"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""eanCodeRequest"" type=""tns:eanCodeRequest"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""eanCodeRequest"">
    <xs:complexContent mixed=""false"">
      <xs:extension base=""tns:abstractRequest"">
        <xs:sequence>
          <xs:element name=""eanCode"" type=""xs:long"" />
        </xs:sequence>
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:complexType name=""createUniqueCouponWithEanCodeResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""return"" type=""tns:singleResponse"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""InvalidConfiguration"" type=""tns:InvalidConfiguration"" />
  <xs:complexType name=""InvalidConfiguration"">
    <xs:sequence>
      <xs:element name=""errorCode"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""InternalError"" type=""tns:InternalError"" />
  <xs:complexType name=""InternalError"">
    <xs:sequence>
      <xs:element name=""errorCode"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""InvalidAmount"" type=""tns:InvalidAmount"" />
  <xs:complexType name=""InvalidAmount"">
    <xs:sequence>
      <xs:element name=""errorCode"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""InvalidCampaign"" type=""tns:InvalidCampaign"" />
  <xs:complexType name=""InvalidCampaign"">
    <xs:sequence>
      <xs:element name=""errorCode"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""NoUniqueCodeCreated"" type=""tns:NoUniqueCodeCreated"" />
  <xs:complexType name=""NoUniqueCodeCreated"">
    <xs:sequence>
      <xs:element name=""errorCode"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""InvalidField"" type=""tns:InvalidField"" />
  <xs:complexType name=""InvalidField"">
    <xs:sequence>
      <xs:element name=""errorCode"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""InvalidEanCode"" type=""tns:InvalidEanCode"" />
  <xs:complexType name=""InvalidEanCode"">
    <xs:sequence>
      <xs:element name=""errorCode"" nillable=""true"" type=""xs:int"" />
    </xs:sequence>
  </xs:complexType>
</xs:schema>";
        
        public CouponCreatorService() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [19];
                _RootElements[0] = "createCouponForPrinting";
                _RootElements[1] = "createCouponForPrintingResponse";
                _RootElements[2] = "createMobileCoupon";
                _RootElements[3] = "createMobileCouponResponse";
                _RootElements[4] = "createUniqueCoupon";
                _RootElements[5] = "createUniqueCouponBatch";
                _RootElements[6] = "createUniqueCouponBatchResponse";
                _RootElements[7] = "createUniqueCouponResponse";
                _RootElements[8] = "createUniqueCouponWithEanCode";
                _RootElements[9] = "createUniqueCouponWithEanCodeResponse";
                _RootElements[10] = "getHeartbeat";
                _RootElements[11] = "getHeartbeatResponse";
                _RootElements[12] = "InvalidConfiguration";
                _RootElements[13] = "InternalError";
                _RootElements[14] = "InvalidAmount";
                _RootElements[15] = "InvalidCampaign";
                _RootElements[16] = "NoUniqueCodeCreated";
                _RootElements[17] = "InvalidField";
                _RootElements[18] = "InvalidEanCode";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createCouponForPrinting")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createCouponForPrinting"})]
        public sealed class createCouponForPrinting : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createCouponForPrinting() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createCouponForPrinting";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createCouponForPrintingResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createCouponForPrintingResponse"})]
        public sealed class createCouponForPrintingResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createCouponForPrintingResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createCouponForPrintingResponse";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createMobileCoupon")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createMobileCoupon"})]
        public sealed class createMobileCoupon : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createMobileCoupon() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createMobileCoupon";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createMobileCouponResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createMobileCouponResponse"})]
        public sealed class createMobileCouponResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createMobileCouponResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createMobileCouponResponse";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createUniqueCoupon")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createUniqueCoupon"})]
        public sealed class createUniqueCoupon : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createUniqueCoupon() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createUniqueCoupon";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createUniqueCouponBatch")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createUniqueCouponBatch"})]
        public sealed class createUniqueCouponBatch : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createUniqueCouponBatch() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createUniqueCouponBatch";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createUniqueCouponBatchResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createUniqueCouponBatchResponse"})]
        public sealed class createUniqueCouponBatchResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createUniqueCouponBatchResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createUniqueCouponBatchResponse";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createUniqueCouponResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createUniqueCouponResponse"})]
        public sealed class createUniqueCouponResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createUniqueCouponResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createUniqueCouponResponse";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createUniqueCouponWithEanCode")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createUniqueCouponWithEanCode"})]
        public sealed class createUniqueCouponWithEanCode : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createUniqueCouponWithEanCode() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createUniqueCouponWithEanCode";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"createUniqueCouponWithEanCodeResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"createUniqueCouponWithEanCodeResponse"})]
        public sealed class createUniqueCouponWithEanCodeResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public createUniqueCouponWithEanCodeResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "createUniqueCouponWithEanCodeResponse";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"getHeartbeat")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"getHeartbeat"})]
        public sealed class getHeartbeat : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public getHeartbeat() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "getHeartbeat";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"getHeartbeatResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"getHeartbeatResponse"})]
        public sealed class getHeartbeatResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public getHeartbeatResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "getHeartbeatResponse";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"InvalidConfiguration")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"InvalidConfiguration"})]
        public sealed class InvalidConfiguration : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public InvalidConfiguration() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "InvalidConfiguration";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"InternalError")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"InternalError"})]
        public sealed class InternalError : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public InternalError() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "InternalError";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"InvalidAmount")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"InvalidAmount"})]
        public sealed class InvalidAmount : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public InvalidAmount() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "InvalidAmount";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"InvalidCampaign")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"InvalidCampaign"})]
        public sealed class InvalidCampaign : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public InvalidCampaign() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "InvalidCampaign";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"NoUniqueCodeCreated")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"NoUniqueCodeCreated"})]
        public sealed class NoUniqueCodeCreated : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public NoUniqueCodeCreated() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "NoUniqueCodeCreated";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"InvalidField")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"InvalidField"})]
        public sealed class InvalidField : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public InvalidField() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "InvalidField";
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
        
        [Schema(@"http://service.web.couponcreator.kuponginlosen.se/",@"InvalidEanCode")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"InvalidEanCode"})]
        public sealed class InvalidEanCode : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public InvalidEanCode() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "InvalidEanCode";
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
