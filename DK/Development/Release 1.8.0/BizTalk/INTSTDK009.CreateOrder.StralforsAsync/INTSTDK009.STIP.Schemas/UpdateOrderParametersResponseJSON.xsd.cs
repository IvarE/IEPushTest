namespace INTSTDK009.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK009.STIP.Schemas.UpdateOrderParametersResponseJSON",@"UpdateOrderParametersResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"UpdateOrderParametersResponse"})]
    public sealed class UpdateOrderParametersResponseJSON : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://INTSTDK009.STIP.Schemas.UpdateOrderParametersResponseJSON"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""UpdateOrderParametersResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""ErrorMessage"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public UpdateOrderParametersResponseJSON() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "UpdateOrderParametersResponse";
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
