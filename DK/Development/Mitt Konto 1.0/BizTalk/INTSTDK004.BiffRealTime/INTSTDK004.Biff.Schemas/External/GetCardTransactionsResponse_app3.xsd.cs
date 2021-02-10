namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://Biff.com/CardTransactions.xsd",@"CardTransactions")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CardTransactions"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse_app1", typeof(global::INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse_app1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse_app2", typeof(global::INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse_app2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse", typeof(global::INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse))]
    public sealed class GetCardTransactionsResponse_app3 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:app2=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:mstns=""http://cubic.com"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns=""http://Biff.com/CardTransactions.xsd"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://Biff.com/CardTransactions.xsd"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse_app1"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse_app2"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse"" namespace=""http://cubic.com"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
        <reference targetNamespace=""http://cubic.com"" />
        <reference targetNamespace=""http://Biff.com/CardTransactions.xsd"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CardTransactions"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Transactions"">
          <xs:complexType>
            <xs:sequence>
              <xs:element app1:Ordinal=""0"" minOccurs=""0"" name=""Date"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""1"" minOccurs=""0"" name=""DeviceID"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""2"" minOccurs=""0"" name=""TxnNum"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""3"" minOccurs=""0"" name=""CardSect"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""4"" minOccurs=""0"" name=""RecType"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""5"" minOccurs=""0"" name=""TxnType"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""6"" minOccurs=""0"" name=""Route"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""7"" minOccurs=""0"" name=""Currency"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""8"" minOccurs=""0"" name=""Amount"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""9"" minOccurs=""0"" name=""OrigZone"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""10"" minOccurs=""0"" name=""DestZone"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""11"" minOccurs=""0"" name=""PeriodStart"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""12"" minOccurs=""0"" name=""PeriodEnd"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""13"" minOccurs=""0"" name=""WaitingPeriods"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""14"" minOccurs=""0"" name=""WaitingPeriodsAdded"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""15"" minOccurs=""0"" name=""TravelsInPeriod"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""16"" minOccurs=""0"" name=""ZoneListID"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
            </xs:sequence>
            <xs:attribute ref=""app2:id"" />
            <xs:attribute ref=""msdata:rowOrder"" />
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CardDetails"">
          <xs:complexType>
            <xs:sequence>
              <xs:element app1:Ordinal=""0"" minOccurs=""0"" name=""CardSerialNumber"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""1"" minOccurs=""0"" name=""NumTxnsAvailable"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
            </xs:sequence>
            <xs:attribute ref=""app2:id"" />
            <xs:attribute ref=""msdata:rowOrder"" />
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ZoneList"">
          <xs:complexType>
            <xs:sequence>
              <xs:element app1:Ordinal=""0"" minOccurs=""0"" name=""ZoneListID"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app1:Ordinal=""1"" minOccurs=""0"" name=""Zone"" type=""xs:string"" xmlns:app1=""urn:schemas-microsoft-com:xml-msdata"" />
            </xs:sequence>
            <xs:attribute ref=""app2:id"" />
            <xs:attribute ref=""msdata:rowOrder"" />
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetCardTransactionsResponse_app3() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CardTransactions";
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
