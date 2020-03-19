namespace INTSTDK001.STIP.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.se/INTSTDK001GetDirectJourneysBetweenStops",@"GetDirectJourneysBetweenStops")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStops"})]
    public sealed class INTSTDK001GetDirectJourneysBetweenStops : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.se/INTSTDK001GetDirectJourneysBetweenStops"" xmlns=""http://www.w3.org/2001/XMLSchema"">
  <element name=""GetDirectJourneysBetweenStops"">
    <complexType>
      <sequence>
        <element name=""fromStopAreaGid"" type=""string"" />
        <element name=""toStopAreaGid"" type=""string"" />
        <element name=""forTimeWindowStartDateTime"" type=""dateTime"" />
        <element minOccurs=""0"" name=""forTimeWindowDuration"" nillable=""true"" type=""string"" />
        <element minOccurs=""0"" name=""withDepartureMaxCount"" nillable=""true"" type=""string"" />
        <element minOccurs=""0"" name=""forLineGids"" nillable=""true"" type=""string"" />
        <element minOccurs=""0"" name=""forProducts"" nillable=""true"" type=""string"" />
        <element minOccurs=""0"" name=""purposeOfLineGroupingCode"" nillable=""true"" type=""string"" />
      </sequence>
    </complexType>
  </element>
</schema>";
        
        public INTSTDK001GetDirectJourneysBetweenStops() {
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
