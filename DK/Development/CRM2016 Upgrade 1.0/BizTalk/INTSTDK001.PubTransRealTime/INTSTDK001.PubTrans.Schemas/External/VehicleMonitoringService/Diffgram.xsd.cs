namespace INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"urn:schemas-microsoft-com:xml-diffgram-v1",@"diffgram")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"diffgram"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.Diffgram_RowOrder", typeof(global::INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.Diffgram_RowOrder))]
    public sealed class Diffgram : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.Diffgram_RowOrder"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
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
        <xs:choice>
          <xs:element msdata:IsDataSet=""true"" msdata:Locale=""sv-SE"" name=""GetCallsforServiceJourneyMethod"">
            <xs:complexType>
              <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
                <xs:element name=""DatedServiceJourney"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""DatedVehicleJourneyId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsDatedVehicleJourneyId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""OperatingDayDate"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""Gid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsWorkedOnDirectionOfLineGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""TransportModeCode"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""LineDesignation"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""TransportAuthorityCode"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""TransportAuthorityName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""ContractorCode"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""ContractorName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""ExpectedToBeMonitored"" type=""xs:boolean"" />
                      <xs:element minOccurs=""0"" name=""IsAssignedToVehicleGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""State"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""PredictionState"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""OriginName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""OriginShortName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""ProductCode"" type=""xs:string"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""DatedDeparture"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsOnDatedServiceJourneyId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsOnServiceJourneyId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""JourneyPatternSequenceNumber"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsTimetabledAtJourneyPatternPointGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsTargetedAtJourneyPatternPointGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""WasObservedAtJourneyPatternPointGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""TimetabledEarliestDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""TargetDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""EstimatedDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""ObservedDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""State"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""Type"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""ProductName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""LineDesignation"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""PrimaryDestinationName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""PrimaryDestinationShortName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""SecondaryDestinationName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""SecondaryDestinationShortName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""SecondaryDestinationType"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""SymbolName"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""PresentationType"" type=""xs:int"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""DatedArrival"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsOnDatedServiceJourneyId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsOnServiceJourneyId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""JourneyPatternSequenceNumber"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsTimetabledAtJourneyPatternPointGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsTargetedAtJourneyPatternPointGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""WasObservedAtJourneyPatternPointGid"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""TimetabledLatestDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""TargetDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""EstimatedDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""ObservedDateTime"" type=""xs:dateTime"" />
                      <xs:element minOccurs=""0"" name=""State"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""Type"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""PresentationType"" type=""xs:int"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""DeviationMessageVersion"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""PublicNote"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""InternalNote"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""PriorityImportanceLevel"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""PriorityInfluenceLevel"" type=""xs:short"" />
                      <xs:element minOccurs=""0"" name=""PriorityUrgencyLevel"" type=""xs:short"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""DeviationMessageVariant"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsPartOfDeviationMessageId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""Content"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""ContentTypeLongCode"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""UsageTypeLongCode"" type=""xs:string"" />
                      <xs:element minOccurs=""0"" name=""LanguageCode"" type=""xs:string"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""ServiceJourneyDeviation"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsOnDatedVehicleJourneyId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""HasDeviationMessageVersionId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""ConsequenceLongCode"" type=""xs:string"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""DepartureDeviation"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsOnDepartureId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""AffectsLaterArrivalsYesNo"" type=""xs:boolean"" />
                      <xs:element minOccurs=""0"" name=""HasDeviationMessageVersionId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""ConsequenceLongCode"" type=""xs:string"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""ArrivalDeviation"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""Id"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""IsOnArrivalId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""AffectsPreviousDeparturesYesNo"" type=""xs:boolean"" />
                      <xs:element minOccurs=""0"" name=""HasDeviationMessageVersionId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""ConsequenceLongCode"" type=""xs:string"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""TargetAudience"">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element minOccurs=""0"" name=""IsForDeviationMessageVersionId"" type=""xs:decimal"" />
                      <xs:element minOccurs=""0"" name=""TypeCode"" type=""xs:string"" />
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
              </xs:choice>
            </xs:complexType>
          </xs:element>
        </xs:choice>
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
