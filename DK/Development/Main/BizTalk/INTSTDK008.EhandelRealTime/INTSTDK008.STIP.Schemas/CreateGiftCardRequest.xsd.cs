namespace INTSTDK008.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/CreateGiftCardRequest/2014-11-03",@"CreateGiftCardRequest")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreateGiftCardRequest"})]
    public sealed class CreateGiftCardRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK008/CreateGiftCardRequest/2014-11-03"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK008/CreateGiftCardRequest/2014-11-03"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CreateGiftCardRequest"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""CampaignTrackingCode"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""Sum"" type=""xs:decimal"" />
        <xs:element minOccurs=""0"" name=""CustomerId"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""Currency"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""ValidTo"" type=""xs:dateTime"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CreateGiftCardRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CreateGiftCardRequest";
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
