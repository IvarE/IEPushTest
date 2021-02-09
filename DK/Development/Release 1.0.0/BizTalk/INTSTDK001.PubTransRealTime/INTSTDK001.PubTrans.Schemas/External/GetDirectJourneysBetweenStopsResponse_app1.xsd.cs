namespace INTSTDK001.PubTrans.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"urn:schemas-microsoft-com:xml-diffgram-v1",@"diffgram")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"diffgram"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app2", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse))]
    public sealed class GetDirectJourneysBetweenStopsResponse_app1 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:mstns=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse_app2"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetDirectJourneysBetweenStopsResponse"" namespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
        <reference targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:attribute app2:Prefix=""diffgr"" name=""id"" type=""xs:string"" xmlns:app2=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:element app2:Prefix=""diffgr"" name=""diffgram"" xmlns:app2=""urn:schemas-microsoft-com:xml-msdata"">
    <xs:complexType>
      <xs:sequence>
        <xs:element form=""unqualified"" name=""GetDirectJourneysBetweenStopsMethod"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DirectJourneysBetweenStops"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element app2:Ordinal=""0"" minOccurs=""0"" form=""unqualified"" name=""DatedVehicleJourneyId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""1"" minOccurs=""0"" form=""unqualified"" name=""ServiceJourneyGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""2"" minOccurs=""0"" form=""unqualified"" name=""OperatingDayDate"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""3"" minOccurs=""0"" form=""unqualified"" name=""ContractorGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""4"" minOccurs=""0"" form=""unqualified"" name=""LineDesignation"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""5"" minOccurs=""0"" form=""unqualified"" name=""JourneyNumber"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""6"" minOccurs=""0"" form=""unqualified"" name=""DirectionOfLineDescription"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""7"" minOccurs=""0"" form=""unqualified"" name=""PrimaryDestinationName"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""8"" minOccurs=""0"" form=""unqualified"" name=""SecondaryDestinationName"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""9"" minOccurs=""0"" form=""unqualified"" name=""DepartureId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""10"" minOccurs=""0"" form=""unqualified"" name=""DepartureStopPointGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""11"" minOccurs=""0"" form=""unqualified"" name=""DepartureType"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""12"" minOccurs=""0"" form=""unqualified"" name=""DepartureSequenceNumber"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""13"" minOccurs=""0"" form=""unqualified"" name=""PlannedDepartureDateTime"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""14"" minOccurs=""0"" form=""unqualified"" name=""ArrivalId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""15"" minOccurs=""0"" form=""unqualified"" name=""ArrivalStopPointGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""16"" minOccurs=""0"" form=""unqualified"" name=""ArrivalType"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""17"" minOccurs=""0"" form=""unqualified"" name=""ArrivalSequenceNumber"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""18"" minOccurs=""0"" form=""unqualified"" name=""PlannedArrivalDateTime"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""19"" minOccurs=""0"" form=""unqualified"" name=""ExpectedToBeMonitored"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""20"" minOccurs=""0"" form=""unqualified"" name=""TargetDepartureStopPointGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""21"" minOccurs=""0"" form=""unqualified"" name=""TargetDepartureDateTime"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""22"" minOccurs=""0"" form=""unqualified"" name=""EstimatedDepartureDateTime"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""23"" minOccurs=""0"" form=""unqualified"" name=""ObservedDepartureDateTime"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""24"" minOccurs=""0"" form=""unqualified"" name=""ArrivalStopPointGid1"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""25"" minOccurs=""0"" form=""unqualified"" name=""ObservedArrivalDateTime"" type=""xs:string"" />
                  </xs:sequence>
                  <xs:attribute ref=""id"" />
                  <xs:attribute ref=""app2:rowOrder"" />
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DeviationMessageVersion"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element app2:Ordinal=""0"" minOccurs=""0"" form=""unqualified"" name=""Id"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""1"" minOccurs=""0"" form=""unqualified"" name=""PublicNote"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""2"" minOccurs=""0"" form=""unqualified"" name=""InternalNote"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""3"" minOccurs=""0"" form=""unqualified"" name=""PriorityImportanceLevel"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""4"" minOccurs=""0"" form=""unqualified"" name=""PriorityInfluenceLevel"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""5"" minOccurs=""0"" form=""unqualified"" name=""PriorityUrgencyLevel"" type=""xs:string"" />
                  </xs:sequence>
                  <xs:attribute ref=""id"" />
                  <xs:attribute ref=""app2:rowOrder"" />
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DeviationMessageVariant"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element app2:Ordinal=""0"" minOccurs=""0"" form=""unqualified"" name=""Id"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""1"" minOccurs=""0"" form=""unqualified"" name=""IsPartOfDeviationMessageId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""2"" minOccurs=""0"" form=""unqualified"" name=""Content"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""3"" minOccurs=""0"" form=""unqualified"" name=""ContentTypeLongCode"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""4"" minOccurs=""0"" form=""unqualified"" name=""UsageTypeLongCode"" type=""xs:string"" />
                  </xs:sequence>
                  <xs:attribute ref=""id"" />
                  <xs:attribute ref=""app2:rowOrder"" />
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""TargetAudience"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element app2:Ordinal=""0"" minOccurs=""0"" form=""unqualified"" name=""IsForDeviationMessageVersionId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""1"" minOccurs=""0"" form=""unqualified"" name=""TypeCode"" type=""xs:string"" />
                  </xs:sequence>
                  <xs:attribute ref=""id"" />
                  <xs:attribute ref=""app2:rowOrder"" />
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetDirectJourneysBetweenStopsResponse_app1() {
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
