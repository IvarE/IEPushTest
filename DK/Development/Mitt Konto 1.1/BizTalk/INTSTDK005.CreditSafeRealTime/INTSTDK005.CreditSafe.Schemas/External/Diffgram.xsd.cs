namespace INTSTDK005.CreditSafe.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"urn:schemas-microsoft-com:xml-diffgram-v1",@"diffgram")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"diffgram"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.CreditSafe.Schemas.External.Diffgram_Msdata", typeof(global::INTSTDK005.CreditSafe.Schemas.External.Diffgram_Msdata))]
    public sealed class Diffgram : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK005.CreditSafe.Schemas.External.Diffgram_Msdata"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""diffgram"">
    <xs:complexType>
      <xs:sequence>
        <xs:element msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"" name=""NewDataSet"">
          <xs:complexType>
            <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
              <xs:element name=""GETDATA_RESPONSE"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
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
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""ORGNR"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""NAME"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""COMPANY_TYPE"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""COMPANY_TYPE_TEXT"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""COMPANY_LEGAL_CODE"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""OLD_NAME"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""TELEPHONE"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""FAXNR"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""EMAIL_ADRESS"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""WWW_ADRESS"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""INCORPORATION_DATE"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""CFAR_NR"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""COMPANY_STATUS"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""COMPANY_STATUS_DATE"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" form=""unqualified"" name=""COMPANY_ACTIVE"" type=""xs:string"" />
                    </xs:choice>
                  </xs:sequence>
                  <xs:attribute form=""qualified"" name=""id"" type=""xs:string"" />
                  <xs:attribute ref=""msdata:rowOrder"" />
                  <xs:attribute form=""qualified"" name=""hasChanges"" type=""xs:string"" />
                </xs:complexType>
              </xs:element>
            </xs:choice>
            <xs:attribute ref=""msdata:IsDataSet"" />
            <xs:attribute ref=""msdata:UseCurrentLocale"" />
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public Diffgram() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "diffgram";
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
