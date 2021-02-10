namespace INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCallsforServiceJourney", @"GetCallsforServiceJourneyResponse", @"GetCall", @"GetCallResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.Diffgram", typeof(global::INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.Diffgram))]
    public sealed class VehicleMonitoringService : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:tns=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.Diffgram"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
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
  <xs:element name=""GetCallsforServiceJourneyResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""GetCallsforServiceJourneyResult"" nillable=""true"">
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
  <xs:element name=""GetCall"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""forDatedServiceJourneyIdOrGid"" nillable=""true"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""atOperatingDayDate"" type=""xs:dateTime"" />
        <xs:element minOccurs=""0"" name=""atSequenceNumber"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCallResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""GetCallResult"" nillable=""true"">
          <xs:complexType>
            <xs:annotation>
              <xs:appinfo>
                <ActualType Name=""DataSet"" Namespace=""http://schemas.datacontract.org/2004/07/System.Data"" />
              </xs:appinfo>
            </xs:annotation>
            <xs:sequence>
              <xs:any />
              <xs:any />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public VehicleMonitoringService() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [4];
                _RootElements[0] = "GetCallsforServiceJourney";
                _RootElements[1] = "GetCallsforServiceJourneyResponse";
                _RootElements[2] = "GetCall";
                _RootElements[3] = "GetCallResponse";
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService",@"GetCallsforServiceJourney")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCallsforServiceJourney"})]
        public sealed class GetCallsforServiceJourney : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCallsforServiceJourney() {
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService",@"GetCallsforServiceJourneyResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCallsforServiceJourneyResponse"})]
        public sealed class GetCallsforServiceJourneyResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCallsforServiceJourneyResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCallsforServiceJourneyResponse";
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService",@"GetCall")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCall"})]
        public sealed class GetCall : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCall() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCall";
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
        
        [Schema(@"http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService",@"GetCallResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCallResponse"})]
        public sealed class GetCallResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCallResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCallResponse";
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
