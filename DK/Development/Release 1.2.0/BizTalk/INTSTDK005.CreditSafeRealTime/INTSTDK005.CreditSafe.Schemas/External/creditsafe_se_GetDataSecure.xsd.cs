namespace INTSTDK005.CreditSafe.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDataBySecure", @"GetDataBySecureResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.CreditSafe.Schemas.External.Diffgram", typeof(global::INTSTDK005.CreditSafe.Schemas.External.Diffgram))]
    public sealed class GetData_webservice_creditsafe_se_getdata : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:tns=""https://webservice.creditsafe.se/getdata/"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""https://webservice.creditsafe.se/getdata/"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK005.CreditSafe.Schemas.External.Diffgram"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetDataBySecure"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""GetData_Request"" type=""tns:GETDATA_REQUEST"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetDataBySecureResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""GetDataBySecureResult"" type=""tns:GETDATA_RESPONSE"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:complexType name=""GETDATA_REQUEST"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account"" type=""tns:Account"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Block_Name"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""SearchNumber"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""FormattedOutput"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""LODCustFreeText"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Mobile"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Email"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""Account"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""UserName"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Password"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TransactionId"" type=""xs:string"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Language"" type=""tns:LANGUAGE"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""LANGUAGE"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""EN"" />
      <xs:enumeration value=""SWE"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""GETDATA_RESPONSE"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""SearchNumber"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TransactionId"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Error"" type=""tns:ERROR"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Block_Name"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Parameters"">
        <xs:complexType>
          <xs:sequence>
            <xs:any processContents=""skip"" />
            <xs:element ref=""diffgr:diffgram"" />
          </xs:sequence>
        </xs:complexType>
      </xs:element>
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""ERROR"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Cause_of_Reject"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Reject_text"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Reject_comment"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
</xs:schema>";
        
        public GetData_webservice_creditsafe_se_getdata() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "GetDataBySecure";
                _RootElements[1] = "GetDataBySecureResponse";
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
        
        [Schema(@"https://webservice.creditsafe.se/getdata/",@"GetDataBySecure")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetDataBySecure"})]
        public sealed class GetDataBySecure : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetDataBySecure() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetDataBySecure";
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
        
        [Schema(@"https://webservice.creditsafe.se/getdata/",@"GetDataBySecureResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetDataBySecureResponse"})]
        public sealed class GetDataBySecureResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetDataBySecureResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetDataBySecureResponse";
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
