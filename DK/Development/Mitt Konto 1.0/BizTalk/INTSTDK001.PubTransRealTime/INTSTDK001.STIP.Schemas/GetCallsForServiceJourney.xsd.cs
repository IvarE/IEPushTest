namespace INTSTDK001.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanestrafiken.com/DK/INTSTDK001/GetCallsForServiceJourney/20141020",@"GetCallsforServiceJourney")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCallsforServiceJourney"})]
    public sealed class GetCallsForServiceJourney : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://www.skanestrafiken.com/DK/INTSTDK001/GetCallsForServiceJourney/20141020"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetCallsforServiceJourney"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""forServiceJourneyIdOrGid"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""atOperatingDate"" type=""xs:dateTime"" />
        <xs:element minOccurs=""0"" name=""atStopGid"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""includeArrivalsTable"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" name=""includeDeparturesTable"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" name=""includeDeviationTables"" type=""xs:boolean"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetCallsForServiceJourney() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetCallsforServiceJourney";
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
