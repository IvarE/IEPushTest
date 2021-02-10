namespace INTSTDK004.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216",@"CardDetails2")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CardDetails2"})]
    public sealed class CardDetails2Type : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:mstns=""http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"" id=""CardDetails2"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CardDetails2"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""CardInformation"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""CardNumber"" type=""xs:unsignedLong"" />
              <xs:element minOccurs=""0"" name=""CardIssuer"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""CardKind"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""CardHotlisted"" type=""xs:boolean"" />
              <xs:element minOccurs=""0"" name=""CardReplByCardNumber"" type=""xs:unsignedLong"" />
              <xs:element minOccurs=""0"" name=""CardReplaces"" type=""xs:unsignedLong"" />
              <xs:element minOccurs=""0"" name=""CardTypePeriod"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""CardTypeValue"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""CardValueProductType"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""PurseDetails"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""CardCategory"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""Balance"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""Currency"">
                <xs:simpleType>
                  <xs:restriction base=""xs:string"">
                    <xs:maxLength value=""3"" />
                  </xs:restriction>
                </xs:simpleType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""OutstandingDirectedAutoload"" type=""xs:boolean"" />
              <xs:element minOccurs=""0"" name=""OutstandingEnableThresholdAutoload"" type=""xs:boolean"" />
              <xs:element minOccurs=""0"" name=""Hotlisted"" type=""xs:boolean"" />
              <xs:element minOccurs=""0"" name=""HotlistReason"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""PeriodDetails"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""CardCategory"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""ProductType"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""PeriodStart"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""PeriodEnd"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""WaitingPeriods"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""TravelsInPeriod"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""ZoneListID"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""RouteListID"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""PricePaid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""Currency"">
                <xs:simpleType>
                  <xs:restriction base=""xs:string"">
                    <xs:maxLength value=""3"" />
                  </xs:restriction>
                </xs:simpleType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""OutstandingDirectedAutoload"" type=""xs:boolean"" />
              <xs:element minOccurs=""0"" name=""OutstandingEnableThresholdAutoload"" type=""xs:boolean"" />
              <xs:element minOccurs=""0"" name=""Hotlisted"" type=""xs:boolean"" />
              <xs:element minOccurs=""0"" name=""HotlistReason"" type=""xs:string"" />
              <xs:element name=""ContractSerialNumber"" type=""xs:unsignedLong"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""ZoneLists"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ZoneListID"" type=""xs:int"" />
              <xs:element name=""Zone"" type=""xs:int"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""RouteLists"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""RouteListID"" type=""xs:int"" />
              <xs:element name=""Route"" type=""xs:int"" />
              <xs:element name=""FromZone"" type=""xs:int"" />
              <xs:element name=""ToZone"" type=""xs:int"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CardDetails2Type() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CardDetails2";
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
