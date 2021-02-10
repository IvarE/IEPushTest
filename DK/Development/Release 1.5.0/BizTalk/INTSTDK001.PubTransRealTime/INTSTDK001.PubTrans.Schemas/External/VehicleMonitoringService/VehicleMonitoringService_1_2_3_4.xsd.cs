namespace INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"www.skanetrafiken.se/DK/INTSTDK001/VechileMonotoringService_1_2_3_4",@"DataSet")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"DataSet"})]
    public sealed class VehicleMonitoringService_1_2_3_4 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""www.skanetrafiken.se/DK/INTSTDK001/VechileMonotoringService_1_2_3_4"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""www.skanetrafiken.se/DK/INTSTDK001/VechileMonotoringService_1_2_3_4"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""DataSet"" nillable=""true"">
    <xs:complexType>
      <xs:annotation>
        <xs:appinfo>
          <ActualType Name=""DataSet"" Namespace=""http://schemas.datacontract.org/2004/07/System.Data"" xmlns="""" />
        </xs:appinfo>
      </xs:annotation>
      <xs:sequence>
        <xs:any />
        <xs:any />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public VehicleMonitoringService_1_2_3_4() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "DataSet";
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
