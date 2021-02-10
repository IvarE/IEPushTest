namespace INTSTDK004.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadThresholdActivitiesResponse/20141216",@"GetAutoloadThresholdActivitiesReportResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetAutoloadThresholdActivitiesReportResponse"})]
    public sealed class GetAutoloadThresholdActivitiesResponseType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadThresholdActivitiesResponse/20141216"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadThresholdActivitiesResponse/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetAutoloadThresholdActivitiesReportResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""GetAutoloadThresholdActivitiesReportResult"">
          <xs:complexType>
            <xs:sequence>
              <xs:element msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"" name=""CardActivities"">
                <xs:complexType>
                  <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
                    <xs:element name=""Activities"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""TransactionDate"" type=""xs:dateTime"" />
                          <xs:element name=""DeviceId"" type=""xs:long"" />
                          <xs:element name=""TxnNum"" type=""xs:int"" />
                          <xs:element name=""RequestId"" type=""xs:long"" />
                          <xs:element name=""CardNumber"" type=""xs:unsignedLong"" />
                          <xs:element name=""CardSect"" type=""xs:unsignedByte"" />
                          <xs:element name=""Route"" type=""xs:string"" />
                          <xs:element name=""Price"" type=""xs:decimal"" />
                          <xs:element name=""Currency"">
                            <xs:simpleType>
                              <xs:restriction base=""xs:string"">
                                <xs:maxLength value=""3"" />
                              </xs:restriction>
                            </xs:simpleType>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""TransactionCount"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""Total"" type=""xs:int"" />
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
        
        public GetAutoloadThresholdActivitiesResponseType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetAutoloadThresholdActivitiesReportResponse";
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
