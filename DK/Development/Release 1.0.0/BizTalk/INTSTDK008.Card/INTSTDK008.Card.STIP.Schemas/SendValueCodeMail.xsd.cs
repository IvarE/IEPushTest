namespace INTSTDK008.Card.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"SendValueCodeMailRequest", @"SendValueCodeMailResponse"})]
    public sealed class SendValueCodeMail : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/SendValueCodeMail/20150310"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/SendValueCodeMail/20150310"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""SendValueCodeMailRequest"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""ValueCode"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""CustomerId"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""Email"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""SendValueCodeMailResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""Success"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" name=""Message"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""ErrorMessage"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public SendValueCodeMail() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "SendValueCodeMailRequest";
                _RootElements[1] = "SendValueCodeMailResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008.Card/SendValueCodeMail/20150310",@"SendValueCodeMailRequest")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"SendValueCodeMailRequest"})]
        public sealed class SendValueCodeMailRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public SendValueCodeMailRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "SendValueCodeMailRequest";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008.Card/SendValueCodeMail/20150310",@"SendValueCodeMailResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"SendValueCodeMailResponse"})]
        public sealed class SendValueCodeMailResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public SendValueCodeMailResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "SendValueCodeMailResponse";
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
}
