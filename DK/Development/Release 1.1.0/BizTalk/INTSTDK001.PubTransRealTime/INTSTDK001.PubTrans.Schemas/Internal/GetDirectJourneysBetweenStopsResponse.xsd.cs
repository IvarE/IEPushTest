namespace INTSTDK001.PubTrans.Schemas.Internal {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK001.PubTrans.Schemas.GetDirectJourneysBetweenStopsResponse.20140710",@"GetDirectJourneysBetweenStopsResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStopsResponse"})]
    public sealed class GetDirectJourneysBetweenStopsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK001.PubTrans.Schemas.GetDirectJourneysBetweenStopsResponse.20140710"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://INTSTDK001.PubTrans.Schemas.GetDirectJourneysBetweenStopsResponse.20140710"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetDirectJourneysBetweenStopsResponse"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""DirectJourneysBetweenStops"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""DatedVehicleJourneyId"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""ServiceJourneyGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""OperatingDayDate"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""ContractorGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""LineDesignation"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""JourneyNumber"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""DirectionOfLineDescription"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OriginName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OriginShortName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OriginPlaceGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""PrimaryDestinationName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""PrimaryDestinationShortName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""PrimaryDestinationGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""SecondaryDestinationName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SecondaryDestinationShortName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SecondaryDestinationGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""DepartureId"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""DepartureStopPointGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""DepartureType"" type=""xs:short"" />
              <xs:element minOccurs=""0"" name=""DepartureSequenceNumber"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""PlannedDepartureDateTime"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""ArrivalId"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""ArrivalStopPointGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""ArrivalType"" type=""xs:short"" />
              <xs:element minOccurs=""0"" name=""ArrivalSequenceNumber"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""PlannedArrivalDateTime"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""ExpectedToBeMonitored"" type=""xs:int"" />
              <xs:element minOccurs=""0"" name=""TargetDepartureStopPointGid"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""TargetDepartureDateTime"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""EstimatedDepartureDateTime"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""ObservedDepartureDateTime"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""ArrivalStopPointGid1"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""ObservedArrivalDateTime"" type=""xs:dateTime"" />
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
</xs:schema>";
        
        public GetDirectJourneysBetweenStopsResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetDirectJourneysBetweenStopsResponse";
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
