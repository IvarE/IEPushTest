namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"RequestPurseDirectedAutoload", @"RequestPurseDirectedAutoloadResponse"})]
    public sealed class RequestPurseDirectedAutolaodType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://cubic.com"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""RequestPurseDirectedAutoload"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""CardSerialNumber"" type=""xs:unsignedLong"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Currency"" type=""xs:string"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""AmountToAdd"" type=""xs:float"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""RequestPurseDirectedAutoloadResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""RequestPurseDirectedAutoloadResult"" type=""xs:long"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public RequestPurseDirectedAutolaodType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "RequestPurseDirectedAutoload";
                _RootElements[1] = "RequestPurseDirectedAutoloadResponse";
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
        
        [Schema(@"http://cubic.com",@"RequestPurseDirectedAutoload")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestPurseDirectedAutoload"})]
        public sealed class RequestPurseDirectedAutoload : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestPurseDirectedAutoload() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestPurseDirectedAutoload";
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
        
        [Schema(@"http://cubic.com",@"RequestPurseDirectedAutoloadResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"RequestPurseDirectedAutoloadResponse"})]
        public sealed class RequestPurseDirectedAutoloadResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public RequestPurseDirectedAutoloadResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "RequestPurseDirectedAutoloadResponse";
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
