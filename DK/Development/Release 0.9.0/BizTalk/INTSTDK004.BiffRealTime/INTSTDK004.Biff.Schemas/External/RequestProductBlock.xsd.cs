namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"RequestProductBlock", @"RequestProductBlockResponse"})]
    public sealed class RequestProductBlockType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://cubic.com"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""RequestProductBlock"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CardSerialNumber"" type=""xs:unsignedLong"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ReasonCode"" type=""xs:long"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CardSection"" type=""xs:unsignedByte"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RequestProductBlockResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""RequestProductBlockResult"" type=""xs:long"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public RequestProductBlockType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "RequestProductBlock";
                _RootElements[1] = "RequestProductBlockResponse";
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
        
        [Schema(@"http://cubic.com",@"RequestProductBlock")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestProductBlock"})]
        public sealed class RequestProductBlock : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestProductBlock() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestProductBlock";
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
        
        [Schema(@"http://cubic.com",@"RequestProductBlockResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestProductBlockResponse"})]
        public sealed class RequestProductBlockResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestProductBlockResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestProductBlockResponse";
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
