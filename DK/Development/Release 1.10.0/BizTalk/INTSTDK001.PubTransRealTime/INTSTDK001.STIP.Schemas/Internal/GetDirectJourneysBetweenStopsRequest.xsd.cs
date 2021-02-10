namespace INTSTDK001.STIP.Schemas.Internal {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK001.CRM.Schemas.GetDirectJourneysBetweenStopsRequest.20140710",@"GetDirectJourneysBetweenStops")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStops"})]
    public sealed class GetDirectJourneysBetweenStopsRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK001.CRM.Schemas.GetDirectJourneysBetweenStopsRequest.20140710"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://INTSTDK001.CRM.Schemas.GetDirectJourneysBetweenStopsRequest.20140710"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetDirectJourneysBetweenStops"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""fromStopAreaGid"" type=""xs:anyType"" />
        <xs:element minOccurs=""0"" name=""toStopAreaGid"" type=""xs:anyType"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowStartDateTime"" type=""xs:anyType"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowDuration"" type=""xs:anyType"" />
        <xs:element minOccurs=""0"" name=""withDepartureMaxCount"" type=""xs:anyType"" />
        <xs:element minOccurs=""0"" name=""forLineGids"" type=""xs:anyType"" />
        <xs:element minOccurs=""0"" name=""forProducts"" type=""xs:anyType"" />
        <xs:element minOccurs=""0"" name=""purposeOfLineGroupingCode"" type=""xs:anyType"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetDirectJourneysBetweenStopsRequest() {
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
