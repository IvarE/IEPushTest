namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"urn:schemas-microsoft-com:xml-diffgram-v1",@"diffgram")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"diffgram"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.AutoloadActivityType", typeof(global::INTSTDK004.Biff.Schemas.External.AutoloadActivityType))]
    public sealed class Diffgram_GetActivityReport : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://BIFF.org/AutoloadActivity.xsd"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.AutoloadActivityType"" namespace=""http://BIFF.org/AutoloadActivity.xsd"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://BIFF.org/AutoloadActivity.xsd"">
        <reference targetNamespace=""http://BIFF.org/AutoloadActivity.xsd"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </references>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://BIFF.org/AutoloadActivity.xsd"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" name=""diffgram"">
    <xs:complexType>
      <xs:sequence>
        <xs:element ref=""AutoloadActivity"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public Diffgram_GetActivityReport() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "diffgram";
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
