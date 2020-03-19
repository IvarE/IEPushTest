namespace INTSTDK003.CRM.Schemas.CreateCustCase {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"RequestCreateCase", @"RequestCreateCaseResponse", @"RequestCreateAutoRGCase", @"RequestCreateAutoRGCaseResponse", @"RequestUpdateAutoRGCase", @"RequestUpdateAutoRGCaseResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase_1", typeof(global::INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase_1))]
    public sealed class CreateCase : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://tempuri.org/"" elementFormDefault=""qualified"" targetNamespace=""http://tempuri.org/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase_1"" namespace=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
        <reference targetNamespace=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""RequestCreateCase"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q1=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" minOccurs=""0"" name=""request"" nillable=""true"" type=""q1:CreateCaseRequest"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RequestCreateCaseResponse"">
    <xs:complexType>
      <xs:sequence />
    </xs:complexType>
  </xs:element>
  <xs:element name=""RequestCreateAutoRGCase"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q2=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" minOccurs=""0"" name=""request"" nillable=""true"" type=""q2:CreateAutoRGCaseRequest"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RequestCreateAutoRGCaseResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q3=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" minOccurs=""0"" name=""RequestCreateAutoRGCaseResult"" nillable=""true"" type=""q3:CreateAutoRGCaseResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RequestUpdateAutoRGCase"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q4=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" minOccurs=""0"" name=""request"" nillable=""true"" type=""q4:UpdateAutoRGCaseRequest"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RequestUpdateAutoRGCaseResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q5=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" minOccurs=""0"" name=""RequestUpdateAutoRGCaseResult"" nillable=""true"" type=""q5:UpdateAutoRGCaseResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CreateCase() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [6];
                _RootElements[0] = "RequestCreateCase";
                _RootElements[1] = "RequestCreateCaseResponse";
                _RootElements[2] = "RequestCreateAutoRGCase";
                _RootElements[3] = "RequestCreateAutoRGCaseResponse";
                _RootElements[4] = "RequestUpdateAutoRGCase";
                _RootElements[5] = "RequestUpdateAutoRGCaseResponse";
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
        
        [Schema(@"http://tempuri.org/",@"RequestCreateCase")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestCreateCase"})]
        public sealed class RequestCreateCase : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestCreateCase() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestCreateCase";
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
        
        [Schema(@"http://tempuri.org/",@"RequestCreateCaseResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestCreateCaseResponse"})]
        public sealed class RequestCreateCaseResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestCreateCaseResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestCreateCaseResponse";
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
        
        [Schema(@"http://tempuri.org/",@"RequestCreateAutoRGCase")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestCreateAutoRGCase"})]
        public sealed class RequestCreateAutoRGCase : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestCreateAutoRGCase() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestCreateAutoRGCase";
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
        
        [Schema(@"http://tempuri.org/",@"RequestCreateAutoRGCaseResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestCreateAutoRGCaseResponse"})]
        public sealed class RequestCreateAutoRGCaseResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestCreateAutoRGCaseResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestCreateAutoRGCaseResponse";
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
        
        [Schema(@"http://tempuri.org/",@"RequestUpdateAutoRGCase")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestUpdateAutoRGCase"})]
        public sealed class RequestUpdateAutoRGCase : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestUpdateAutoRGCase() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestUpdateAutoRGCase";
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
        
        [Schema(@"http://tempuri.org/",@"RequestUpdateAutoRGCaseResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestUpdateAutoRGCaseResponse"})]
        public sealed class RequestUpdateAutoRGCaseResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestUpdateAutoRGCaseResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestUpdateAutoRGCaseResponse";
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
