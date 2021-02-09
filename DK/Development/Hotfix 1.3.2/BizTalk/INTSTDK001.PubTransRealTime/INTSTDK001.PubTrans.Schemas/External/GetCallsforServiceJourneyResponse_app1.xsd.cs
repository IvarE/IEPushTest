namespace INTSTDK001.PubTrans.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"urn:schemas-microsoft-com:xml-diffgram-v1",@"diffgram")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"diffgram"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse_app2", typeof(global::INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse_app2))]
    public sealed class GetCallsforServiceJourneyResponse_app1 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:mstns=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse"" namespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" />
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.GetCallsforServiceJourneyResponse_app2"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:attribute app2:Prefix=""diffgr"" name=""id"" type=""xs:string"" xmlns:app2=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:element app2:Prefix=""diffgr"" name=""diffgram"" xmlns:app2=""urn:schemas-microsoft-com:xml-msdata"">
    <xs:complexType>
      <xs:sequence>
        <xs:element form=""unqualified"" name=""GetCallsforServiceJourneyMethod"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DatedServiceJourney"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element app2:Ordinal=""0"" minOccurs=""0"" form=""unqualified"" name=""Id"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""1"" minOccurs=""0"" form=""unqualified"" name=""DatedVehicleJourneyId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""2"" minOccurs=""0"" form=""unqualified"" name=""IsDatedVehicleJourneyId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""3"" minOccurs=""0"" form=""unqualified"" name=""OperatingDayDate"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""4"" minOccurs=""0"" form=""unqualified"" name=""Gid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""5"" minOccurs=""0"" form=""unqualified"" name=""IsWorkedOnDirectionOfLineGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""6"" minOccurs=""0"" form=""unqualified"" name=""TransportModeCode"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""7"" minOccurs=""0"" form=""unqualified"" name=""LineDesignation"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""8"" minOccurs=""0"" form=""unqualified"" name=""TransportAuthorityCode"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""9"" minOccurs=""0"" form=""unqualified"" name=""TransportAuthorityName"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""10"" minOccurs=""0"" form=""unqualified"" name=""ContractorCode"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""11"" minOccurs=""0"" form=""unqualified"" name=""ContractorName"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""12"" minOccurs=""0"" form=""unqualified"" name=""ExpectedToBeMonitored"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""13"" minOccurs=""0"" form=""unqualified"" name=""State"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""14"" minOccurs=""0"" form=""unqualified"" name=""OriginName"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""15"" minOccurs=""0"" form=""unqualified"" name=""OriginShortName"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""16"" minOccurs=""0"" form=""unqualified"" name=""ProductCode"" type=""xs:string"" />
                  </xs:sequence>
                  <xs:attribute ref=""id"" />
                  <xs:attribute ref=""app2:rowOrder"" />
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DatedArrival"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element app2:Ordinal=""0"" minOccurs=""0"" form=""unqualified"" name=""Id"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""1"" minOccurs=""0"" form=""unqualified"" name=""IsOnDatedServiceJourneyId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""2"" minOccurs=""0"" form=""unqualified"" name=""IsOnServiceJourneyId"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""3"" minOccurs=""0"" form=""unqualified"" name=""JourneyPatternSequenceNumber"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""4"" minOccurs=""0"" form=""unqualified"" name=""IsTimetabledAtJourneyPatternPointGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""5"" minOccurs=""0"" form=""unqualified"" name=""IsTargetedAtJourneyPatternPointGid"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""6"" minOccurs=""0"" form=""unqualified"" name=""TimetabledLatestDateTime"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""7"" minOccurs=""0"" form=""unqualified"" name=""TargetDateTime"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""8"" minOccurs=""0"" form=""unqualified"" name=""State"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""9"" minOccurs=""0"" form=""unqualified"" name=""Type"" type=""xs:string"" />
                    <xs:element app2:Ordinal=""10"" minOccurs=""0"" form=""unqualified"" name=""PresentationType"" type=""xs:string"" />
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
        
        public GetCallsforServiceJourneyResponse_app1() {
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
