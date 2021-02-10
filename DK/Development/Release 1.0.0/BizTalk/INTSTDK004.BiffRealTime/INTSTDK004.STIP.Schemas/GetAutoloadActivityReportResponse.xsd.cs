namespace INTSTDK004.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadActivityReportResponse/20141216",@"GetAutoloadActivityReportResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetAutoloadActivityReportResponse"})]
    public sealed class GetAutoloadActivityReportResponseType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadActivityReportResponse/20141216"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadActivityReportResponse/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetAutoloadActivityReportResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""GetAutoloadActivityReportResult"">
          <xs:complexType>
            <xs:sequence>
              <xs:element msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"" name=""AutoloadActivity"">
                <xs:complexType>
                  <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
                    <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Activity"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" name=""CardSerialNumber"" type=""xs:long"" />
                          <xs:element minOccurs=""0"" name=""RequestID"" type=""xs:long"" />
                          <xs:element minOccurs=""0"" name=""CardSection"" type=""xs:int"" />
                          <xs:element minOccurs=""0"" name=""Currency"">
                            <xs:simpleType>
                              <xs:restriction base=""xs:string"">
                                <xs:maxLength value=""3"" />
                              </xs:restriction>
                            </xs:simpleType>
                          </xs:element>
                          <xs:element minOccurs=""0"" name=""Price"" type=""xs:decimal"" />
                          <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:int"" />
                          <xs:element minOccurs=""0"" name=""DateLoaded"" type=""xs:dateTime"" />
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
  </xs:element>
</xs:schema>";
        
        public GetAutoloadActivityReportResponseType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetAutoloadActivityReportResponse";
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
