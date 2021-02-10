namespace INTSTDK001.PubTrans.Schemas.External.StopMonitoringService {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetDeparturesAtStop", @"GetDeparturesAtStopResponse", @"GetDirectJourneysBetweenStops", @"GetDirectJourneysBetweenStopsResponse", @"GetCallsAtStop", @"GetCallsAtStopResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.Diffgram", typeof(global::INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.Diffgram))]
    public sealed class StopMonitoringService : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:tns=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.Diffgram"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <schemaInfo xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
      <b:references>
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetDeparturesAtStop"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""atStopGid"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""forDirectionOfLineGids"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowStartDateTime"" type=""xs:dateTime"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowDuration"" nillable=""true"" type=""xs:unsignedByte"" />
        <xs:element minOccurs=""0"" name=""withDepartureMaxCount"" nillable=""true"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetDeparturesAtStopResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""GetDeparturesAtStopResult"" nillable=""true"">
          <xs:complexType>
            <xs:annotation>
              <xs:appinfo>
                <ActualType Name=""DataSet"" Namespace=""http://schemas.datacontract.org/2004/07/System.Data"" />
              </xs:appinfo>
            </xs:annotation>
            <xs:sequence>
              <xs:any processContents=""skip"" />
              <xs:any processContents=""skip"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
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
  <xs:element name=""GetDirectJourneysBetweenStopsResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""GetDirectJourneysBetweenStopsResult"" nillable=""true"">
          <xs:complexType>
            <xs:annotation>
              <xs:appinfo>
                <ActualType Name=""DataSet"" Namespace=""http://schemas.datacontract.org/2004/07/System.Data"" />
              </xs:appinfo>
            </xs:annotation>
            <xs:sequence>
              <xs:any processContents=""skip"" />
              <xs:element ref=""diffgr:diffgram"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCallsAtStop"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""atStopGid"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowStartDateTime"" type=""xs:dateTime"" />
        <xs:element minOccurs=""0"" name=""forTimeWindowDurationMinutes"" type=""xs:int"" />
        <xs:element minOccurs=""0"" name=""includeArrivalsTable"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" name=""includeDeparturesTable"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" name=""includeDeviationTables"" type=""xs:boolean"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCallsAtStopResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""GetCallsAtStopResult"" nillable=""true"">
          <xs:complexType>
            <xs:annotation>
              <xs:appinfo>
                <ActualType Name=""DataSet"" Namespace=""http://schemas.datacontract.org/2004/07/System.Data"" />
              </xs:appinfo>
            </xs:annotation>
            <xs:sequence>
              <xs:any processContents=""skip"" />
              <xs:element ref=""diffgr:diffgram"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public StopMonitoringService() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [6];
                _RootElements[0] = "GetDeparturesAtStop";
                _RootElements[1] = "GetDeparturesAtStopResponse";
                _RootElements[2] = "GetDirectJourneysBetweenStops";
                _RootElements[3] = "GetDirectJourneysBetweenStopsResponse";
                _RootElements[4] = "GetCallsAtStop";
                _RootElements[5] = "GetCallsAtStopResponse";
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService",@"GetDeparturesAtStop")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetDeparturesAtStop"})]
        public sealed class GetDeparturesAtStop : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetDeparturesAtStop() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetDeparturesAtStop";
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService",@"GetDeparturesAtStopResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetDeparturesAtStopResponse"})]
        public sealed class GetDeparturesAtStopResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetDeparturesAtStopResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetDeparturesAtStopResponse";
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService",@"GetDirectJourneysBetweenStops")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStops"})]
        public sealed class GetDirectJourneysBetweenStops : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetDirectJourneysBetweenStops() {
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService",@"GetDirectJourneysBetweenStopsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetDirectJourneysBetweenStopsResponse"})]
        public sealed class GetDirectJourneysBetweenStopsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService",@"GetCallsAtStop")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCallsAtStop"})]
        public sealed class GetCallsAtStop : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCallsAtStop() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCallsAtStop";
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService",@"GetCallsAtStopResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCallsAtStopResponse"})]
        public sealed class GetCallsAtStopResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCallsAtStopResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCallsAtStopResponse";
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
}
