namespace INTSTDK004.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadActivityReport/20141216",@"GetAutoloadActivityReport")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetAutoloadActivityReport"})]
    public sealed class GetAutoloadActivityReportType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadActivityReport/20141216"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadActivityReport/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetAutoloadActivityReport"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""StartDate"" type=""xs:dateTime"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""EndDate"" type=""xs:dateTime"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetAutoloadActivityReportType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetAutoloadActivityReport";
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
