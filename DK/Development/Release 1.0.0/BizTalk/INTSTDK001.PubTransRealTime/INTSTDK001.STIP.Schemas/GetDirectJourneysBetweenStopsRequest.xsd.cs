namespace INTSTDK001.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneyBetweenStopsRequest/20141023",@"GetDirectJourneysBetweenStops")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStops"})]
    public sealed class GetDirectJourneyBetweenStopsRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneyBetweenStopsRequest/20141023"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneyBetweenStopsRequest/20141023"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetDirectJourneysBetweenStops"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""fromStopAreaGid"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""toStopAreaGid"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowStartDateTime"" type=""xs:dateTime"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowDuration"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""withDepartureMaxCount"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""forLineGids"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""forProducts"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""purposeOfLineGroupingCode"" nillable=""true"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetDirectJourneyBetweenStopsRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetDirectJourneysBetweenStops";
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
