namespace INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://schemas.pubtrans.hogia.se/webservices/2008/11",@"FaultDetail")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"FaultDetail"})]
    public sealed class VehicleMonitoringService_1_2 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.pubtrans.hogia.se/webservices/2008/11"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:complexType name=""FaultDetail"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""Message"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""MethodName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Parameters"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Reason"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""FaultDetail"" nillable=""true"" type=""tns:FaultDetail"" />
</xs:schema>";
        
        public VehicleMonitoringService_1_2() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "FaultDetail";
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
