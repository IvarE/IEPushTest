namespace INTSTDK008.Orchestrations {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"BtsHttpUrlMapping")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"BtsHttpUrlMapping"})]
    public sealed class BtsHttpUrlMapping : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" id=""BtsHttpUrlMapping"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element msdata:IsDataSet=""true"" msdata:Locale=""en-US"" name=""BtsHttpUrlMapping"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""Operation"">
          <xs:complexType>
            <xs:attribute name=""Method"" type=""xs:string"" />
            <xs:attribute name=""Url"" type=""xs:string"" />
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public BtsHttpUrlMapping() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "BtsHttpUrlMapping";
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
