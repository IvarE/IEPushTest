namespace INTSTDK008.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/CancelGiftCardRequestJSON/20150915",@"CancelGiftCardRequest")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK008.STIP.Schemas.GiftCardCode), XPath = @"/*[local-name()='CancelGiftCardRequest' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK008/CancelGiftCardRequestJSON/20150915']/*[local-name()='GiftCardCode' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CancelGiftCardRequest"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.PropertySchema", typeof(global::INTSTDK008.STIP.Schemas.PropertySchema))]
    public sealed class CancelGiftCardRequestJSON : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK008/CancelGiftCardRequestJSON/20150915"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""http://INTSTDK008.STIP.Schemas.PropertySchema"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK008/CancelGiftCardRequestJSON/20150915"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""http://INTSTDK008.STIP.Schemas.PropertySchema"" location=""INTSTDK008.STIP.Schemas.PropertySchema"" />
      </b:imports>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CancelGiftCardRequest"">
    <xs:annotation>
      <xs:appinfo>
        <b:properties>
          <b:property name=""ns0:GiftCardCode"" xpath=""/*[local-name()='CancelGiftCardRequest' and namespace-uri()='http://www.skanetrafiken.com/DK/INTSTDK008/CancelGiftCardRequestJSON/20150915']/*[local-name()='GiftCardCode' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""GiftCardCode"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CancelGiftCardRequestJSON() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CancelGiftCardRequest";
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
