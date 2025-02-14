namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://cubic.com",@"GetCardDetails2")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCardDetails2"})]
    public sealed class GetCardDetails2RequestType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://cubic.com"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetCardDetails2"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CardSerialNumber"" type=""xs:unsignedLong"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetCardDetails2RequestType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetCardDetails2";
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
