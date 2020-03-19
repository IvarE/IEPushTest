namespace INTSTDK001.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneysBetweenStopsResponse/20141023",@"GetDirectJourneysBetweenStopsMethod")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStopsMethod"})]
    public sealed class GetDirectJourneysBetweenStopsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK001.STIP.Schemas.GetDirectJourneysBetweenStopsResponse"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneysBetweenStopsResponse/20141023"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetDirectJourneysBetweenStopsMethod"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DirectJourneysBetweenStops"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" form=""unqualified"" name=""DatedVehicleJourneyId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ServiceJourneyGid"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""OperatingDayDate"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ContractorGid"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""LineDesignation"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""JourneyNumber"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""DirectionOfLineDescription"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""PrimaryDestinationName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""SecondaryDestinationName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""DepartureId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""DepartureStopPointGid"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""DepartureType"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""DepartureSequenceNumber"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""PlannedDepartureDateTime"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ArrivalId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ArrivalStopPointGid"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ArrivalType"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ArrivalSequenceNumber"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""PlannedArrivalDateTime"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ExpectedToBeMonitored"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""TargetDepartureStopPointGid"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""TargetDepartureDateTime"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""EstimatedDepartureDateTime"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ObservedDepartureDateTime"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ArrivalStopPointGid1"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ObservedArrivalDateTime"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DeviationMessageVersion"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" form=""unqualified"" name=""Id"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""PublicNote"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""InternalNote"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""PriorityImportanceLevel"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""PriorityInfluenceLevel"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""PriorityUrgencyLevel"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""DeviationMessageVariant"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" form=""unqualified"" name=""Id"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""IsPartOfDeviationMessageId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""Content"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""ContentTypeLongCode"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""UsageTypeLongCode"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" form=""unqualified"" name=""TargetAudience"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" form=""unqualified"" name=""IsForDeviationMessageVersionId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" form=""unqualified"" name=""TypeCode"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
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
                _RootElements[0] = "GetDirectJourneysBetweenStopsMethod";
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
