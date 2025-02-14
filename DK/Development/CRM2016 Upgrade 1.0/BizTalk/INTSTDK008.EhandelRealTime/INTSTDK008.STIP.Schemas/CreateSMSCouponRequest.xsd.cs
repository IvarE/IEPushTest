namespace INTSTDK008.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/CreateSMSCouponRequest/20150811",@"CreateSMSCouponRequest")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreateSMSCouponRequest"})]
    public sealed class CreateSMSCouponRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK008/CreateSMSCouponRequest/20150811"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CreateSMSCouponRequest"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""Amount"" type=""xs:decimal"" />
        <xs:element minOccurs=""0"" name=""Currency"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""CustomerId"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""Email"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""MobilePhone"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""CampaignCode"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CreateSMSCouponRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CreateSMSCouponRequest";
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
