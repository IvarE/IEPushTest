namespace INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"RuntimeFaultDetail", @"NotSupportedFaultDetail", @"ImplementationFaultDetail", @"ConfigurationFaultDetail", @"ClientFaultDetail", @"LicenseFaultDetail"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService_1_2", typeof(global::INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService_1_2))]
    public sealed class VehicleMonitoringService_1 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService_1_2"" namespace=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""RuntimeFaultDetail"">
    <xs:complexContent mixed=""false"">
      <xs:extension xmlns:q1=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" base=""q1:FaultDetail"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""RuntimeFaultDetail"" nillable=""true"" type=""tns:RuntimeFaultDetail"" />
  <xs:complexType name=""NotSupportedFaultDetail"">
    <xs:complexContent mixed=""false"">
      <xs:extension xmlns:q2=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" base=""q2:FaultDetail"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""NotSupportedFaultDetail"" nillable=""true"" type=""tns:NotSupportedFaultDetail"" />
  <xs:complexType name=""ImplementationFaultDetail"">
    <xs:complexContent mixed=""false"">
      <xs:extension xmlns:q3=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" base=""q3:FaultDetail"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""ImplementationFaultDetail"" nillable=""true"" type=""tns:ImplementationFaultDetail"" />
  <xs:complexType name=""ConfigurationFaultDetail"">
    <xs:complexContent mixed=""false"">
      <xs:extension xmlns:q4=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" base=""q4:FaultDetail"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""ConfigurationFaultDetail"" nillable=""true"" type=""tns:ConfigurationFaultDetail"" />
  <xs:complexType name=""ClientFaultDetail"">
    <xs:complexContent mixed=""false"">
      <xs:extension xmlns:q5=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" base=""q5:FaultDetail"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""ClientFaultDetail"" nillable=""true"" type=""tns:ClientFaultDetail"" />
  <xs:complexType name=""LicenseFaultDetail"">
    <xs:complexContent mixed=""false"">
      <xs:extension xmlns:q6=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" base=""q6:FaultDetail"">
        <xs:sequence />
      </xs:extension>
    </xs:complexContent>
  </xs:complexType>
  <xs:element name=""LicenseFaultDetail"" nillable=""true"" type=""tns:LicenseFaultDetail"" />
</xs:schema>";
        
        public VehicleMonitoringService_1() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [6];
                _RootElements[0] = "RuntimeFaultDetail";
                _RootElements[1] = "NotSupportedFaultDetail";
                _RootElements[2] = "ImplementationFaultDetail";
                _RootElements[3] = "ConfigurationFaultDetail";
                _RootElements[4] = "ClientFaultDetail";
                _RootElements[5] = "LicenseFaultDetail";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices",@"RuntimeFaultDetail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RuntimeFaultDetail"})]
        public sealed class RuntimeFaultDetail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RuntimeFaultDetail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RuntimeFaultDetail";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices",@"NotSupportedFaultDetail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"NotSupportedFaultDetail"})]
        public sealed class NotSupportedFaultDetail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public NotSupportedFaultDetail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "NotSupportedFaultDetail";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices",@"ImplementationFaultDetail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ImplementationFaultDetail"})]
        public sealed class ImplementationFaultDetail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ImplementationFaultDetail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ImplementationFaultDetail";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices",@"ConfigurationFaultDetail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ConfigurationFaultDetail"})]
        public sealed class ConfigurationFaultDetail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ConfigurationFaultDetail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ConfigurationFaultDetail";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices",@"ClientFaultDetail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ClientFaultDetail"})]
        public sealed class ClientFaultDetail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ClientFaultDetail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ClientFaultDetail";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/Hogia.PubTrans.WebServices",@"LicenseFaultDetail")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"LicenseFaultDetail"})]
        public sealed class LicenseFaultDetail : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public LicenseFaultDetail() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "LicenseFaultDetail";
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
