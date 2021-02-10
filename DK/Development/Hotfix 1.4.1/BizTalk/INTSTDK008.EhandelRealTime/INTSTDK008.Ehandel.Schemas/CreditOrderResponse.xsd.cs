namespace INTSTDK008.Ehandel.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://INTSTDK008.Ehandel.Schemas.CreditOrderResponse",@"CreditOrderResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreditOrderResponse"})]
    public sealed class CreditOrderResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://INTSTDK008.Ehandel.Schemas.CreditOrderResponse"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://INTSTDK008.Ehandel.Schemas.CreditOrderResponse"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CreditOrderResponse"" nillable=""true"" type=""CreditOrderResponse"" />
  <xs:complexType name=""CreditOrderResponse"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ErrorMessage"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""StatusCode"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""OrderNumber"" type=""xs:string"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" form=""unqualified"" name=""Sum"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""ReferenceNumber"" type=""xs:string"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" form=""unqualified"" name=""Success"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" form=""unqualified"" name=""Message"" type=""xs:string"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" form=""unqualified"" name=""Date"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
</xs:schema>";
        
        public CreditOrderResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CreditOrderResponse";
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
