namespace INTSTDK003.STIP.Schemas.GetCustomerDetails {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetAgentUserId", @"GetAgentUserIdResponse", @"GetCustomerDetails", @"GetCustomerDetailsResponse", @"GetCustomerIdForTravelCard", @"GetCustomerIdForTravelCardResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1_2", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1_2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1_2_3", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1_2_3))]
    public sealed class EAIConnectorService : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" />
  <xs:import schemaLocation=""INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1_2"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" />
  <xs:import schemaLocation=""INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService_1_2_3"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2_3/20141126"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" />
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" />
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2_3/20141126"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetAgentUserId"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""crmuserName"" nillable=""true"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetAgentUserIdResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q1=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" minOccurs=""0"" name=""GetAgentUserIdResult"" type=""q1:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCustomerDetails"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q2=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" minOccurs=""0"" name=""customerId"" type=""q2:guid"" />
        <xs:element xmlns:q3=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" minOccurs=""0"" name=""customerType"" type=""q3:AccountCategoryCode"" />
        <xs:element xmlns:q4=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" minOccurs=""0"" name=""callerId"" type=""q4:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCustomerDetailsResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q5=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" minOccurs=""0"" name=""GetCustomerDetailsResult"" nillable=""true"" type=""q5:GetCustomerDetailsResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCustomerIdForTravelCard"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q1=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2_3/20141126"" minOccurs=""0"" name=""travelCard"" nillable=""true"" type=""q1:ArrayOfstring"" />
        <xs:element xmlns:q2=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" minOccurs=""0"" name=""callerId"" type=""q2:guid"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""GetCustomerIdForTravelCardResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q8=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" minOccurs=""0"" name=""GetCustomerIdForTravelCardResult"" nillable=""true"" type=""q8:GetCustomerIdForTravelCardResponse"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public EAIConnectorService() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [6];
                _RootElements[0] = "GetAgentUserId";
                _RootElements[1] = "GetAgentUserIdResponse";
                _RootElements[2] = "GetCustomerDetails";
                _RootElements[3] = "GetCustomerDetailsResponse";
                _RootElements[4] = "GetCustomerIdForTravelCard";
                _RootElements[5] = "GetCustomerIdForTravelCardResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126",@"GetAgentUserId")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetAgentUserId"})]
        public sealed class GetAgentUserId : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetAgentUserId() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetAgentUserId";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126",@"GetAgentUserIdResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetAgentUserIdResponse"})]
        public sealed class GetAgentUserIdResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetAgentUserIdResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetAgentUserIdResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126",@"GetCustomerDetails")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomerDetails"})]
        public sealed class GetCustomerDetails : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomerDetails() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomerDetails";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126",@"GetCustomerDetailsResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomerDetailsResponse"})]
        public sealed class GetCustomerDetailsResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomerDetailsResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomerDetailsResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126",@"GetCustomerIdForTravelCard")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomerIdForTravelCard"})]
        public sealed class GetCustomerIdForTravelCard : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomerIdForTravelCard() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomerIdForTravelCard";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126",@"GetCustomerIdForTravelCardResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"GetCustomerIdForTravelCardResponse"})]
        public sealed class GetCustomerIdForTravelCardResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public GetCustomerIdForTravelCardResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "GetCustomerIdForTravelCardResponse";
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
