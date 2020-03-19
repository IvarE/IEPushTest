namespace INTSTDK005.STIP.Schemas.Internal {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK005.STIP.Schemas.DataSecureTest), XPath = @"/*[local-name()='GetDataBySecure' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='GetData_Request' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='Test' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK005.STIP.Schemas.Test), XPath = @"/*[local-name()='GetDataBySecure' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='GetData_Request' and namespace-uri()='']/*[local-name()='Test' and namespace-uri()='']", XsdType = @"boolean")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDataBySecure", @"GetDataBySecureResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.PropertySchema", typeof(global::INTSTDK005.STIP.Schemas.PropertySchema))]
    public sealed class GetDataSecure : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216"" xmlns:ns0=""http://INTSTDK005.EHandel.Schemas.PropertySchema"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""http://INTSTDK005.EHandel.Schemas.PropertySchema"" location=""INTSTDK005.STIP.Schemas.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetDataBySecure"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:DataSecureTest"" xpath=""/*[local-name()='GetDataBySecure' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='GetData_Request' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='Test' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']"" />
          <b:property name=""ns0:Test"" xpath=""/*[local-name()='GetDataBySecure' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='GetData_Request' and namespace-uri()='']/*[local-name()='Test' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""GetData_Request"" type=""tns:GETDATA_REQUEST"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:complexType name=""GETDATA_REQUEST"">
    <xs:sequence>
      <xs:element name=""Test"" type=""xs:boolean"" />
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
  <xs:element name=""GetDataBySecureResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""GetDataBySecureResult"" type=""tns:GETDATA_RESPONSE"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:complexType name=""GETDATA_RESPONSE"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""SearchNumber"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TransactionId"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Error"" type=""tns:ERROR"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Block_Name"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Parameters"">
        <xs:complexType>
          <xs:sequence>
            <xs:element name=""NewDataSet"">
              <xs:complexType>
                <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
                  <xs:element name=""GETDATA_RESPONSE"">
                    <xs:complexType>
                      <xs:sequence>
                        <xs:element minOccurs=""0"" name=""PNR"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""FIRST_NAME"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""GIVEN_NAME"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""LAST_NAME"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""CO_ADDRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""REGISTERED_ADDRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""ADDRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""ZIPCODE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""TOWN"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""SPEC_CO_ADDRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""SPEC_ADDRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""SPEC_ZIPCODE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""SPEC_COUNTRY"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""SPEC_TOWN"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""SPEC_REGISTERED_ADDRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""SEARCH_DATE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""EMIGRATED"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""EMIGRATED_DATE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""PROTECTED"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""GENDER"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""AGE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""ORGNR"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""NAME"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""COMPANY_TYPE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""COMPANY_TYPE_TEXT"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""COMPANY_LEGAL_CODE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""OLD_NAME"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""TELEPHONE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""FAXNR"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""EMAIL_ADRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""WWW_ADRESS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""INCORPORATION_DATE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""CFAR_NR"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""COMPANY_STATUS"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""COMPANY_STATUS_DATE"" type=""xs:string"" />
                        <xs:element minOccurs=""0"" name=""COMPANY_ACTIVE"" type=""xs:string"" />
                      </xs:sequence>
                    </xs:complexType>
                  </xs:element>
                </xs:choice>
              </xs:complexType>
            </xs:element>
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
        
        public GetDataSecure() {
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216",@"GetDataBySecure")]
        [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK005.STIP.Schemas.DataSecureTest), XPath = @"/*[local-name()='GetDataBySecure' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='GetData_Request' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='Test' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']", XsdType = @"string")]
        [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK005.STIP.Schemas.Test), XPath = @"/*[local-name()='GetDataBySecure' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216']/*[local-name()='GetData_Request' and namespace-uri()='']/*[local-name()='Test' and namespace-uri()='']", XsdType = @"boolean")]
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216",@"GetDataBySecureResponse")]
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
