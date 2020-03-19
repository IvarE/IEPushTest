namespace INTSTDK005.STIP.Schemas.Internal {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK005.STIP.Schemas.CasCompanyTest), XPath = @"/*[local-name()='CasCompanyService' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']/*[local-name()='cas_company' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']/*[local-name()='Test' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']", XsdType = @"boolean")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CasCompanyService", @"CasCompanyServiceResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.PropertySchema", typeof(global::INTSTDK005.STIP.Schemas.PropertySchema))]
    public sealed class GetCasCompany : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216"" xmlns:ns0=""http://INTSTDK005.EHandel.Schemas.PropertySchema"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""http://INTSTDK005.EHandel.Schemas.PropertySchema"" location=""INTSTDK005.STIP.Schemas.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
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
  <xs:complexType name=""ArrayOfERROR"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ERROR"" nillable=""true"" type=""tns:ERROR"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""ERROR"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Cause_of_Reject"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Reject_text"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Reject_comment"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""CasCompanyService"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:CasCompanyTest"" xpath=""/*[local-name()='CasCompanyService' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']/*[local-name()='cas_company' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']/*[local-name()='Test' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cas_company"" type=""tns:CAS_COMPANY_REQUEST"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:complexType name=""CAS_COMPANY_REQUEST"">
    <xs:sequence>
      <xs:element name=""Test"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account"" type=""tns:Account"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""SearchNumber"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Name"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Address1"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ZIP"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Town"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Templates"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""LODCustFreeText"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Mobile"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Email"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""CasCompanyServiceResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CasCompanyServiceResult"" type=""tns:CAS_COMPANY_RESPONSE"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:complexType name=""CAS_COMPANY_RESPONSE"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Org_nr"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TemplateNames"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""TransactionId"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Status"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Status_Text"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ErrorList"" type=""tns:ArrayOfERROR"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Name"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Address"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ZIP"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Town"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
</xs:schema>";
        
        public GetCasCompany() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "CasCompanyService";
                _RootElements[1] = "CasCompanyServiceResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216",@"CasCompanyService")]
        [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK005.STIP.Schemas.CasCompanyTest), XPath = @"/*[local-name()='CasCompanyService' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']/*[local-name()='cas_company' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']/*[local-name()='Test' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216']", XsdType = @"boolean")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CasCompanyService"})]
        public sealed class CasCompanyService : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CasCompanyService() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CasCompanyService";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216",@"CasCompanyServiceResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CasCompanyServiceResponse"})]
        public sealed class CasCompanyServiceResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CasCompanyServiceResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CasCompanyServiceResponse";
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
