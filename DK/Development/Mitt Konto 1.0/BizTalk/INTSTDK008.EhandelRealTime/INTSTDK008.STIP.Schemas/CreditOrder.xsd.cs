namespace INTSTDK008.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreditOrderRequest", @"CreditOrderRequestResponse"})]
    public sealed class CreditOrder : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK008/CreditOrder/20141031"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK008/CreditOrder/20141031"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CreditOrderRequest"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""orderNumber"" type=""xs:string"" />
        <xs:element name=""sum"" type=""xs:string"" />
        <xs:element name=""ProductNumber"" type=""xs:string"" />
        <xs:element name=""Quantity"" type=""xs:int"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""CreditOrderRequestResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ErrorMessage"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""StatusCode"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""OrderNumber"" type=""xs:string"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Sum"" type=""xs:decimal"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ReferenceNumber"" type=""xs:string"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Success"" type=""xs:boolean"" />
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Message"" type=""xs:string"" />
        <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Date"" type=""xs:dateTime"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CreditOrder() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "CreditOrderRequest";
                _RootElements[1] = "CreditOrderRequestResponse";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/CreditOrder/20141031",@"CreditOrderRequest")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreditOrderRequest"})]
        public sealed class CreditOrderRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreditOrderRequest() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreditOrderRequest";
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
        
        [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/CreditOrder/20141031",@"CreditOrderRequestResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CreditOrderRequestResponse"})]
        public sealed class CreditOrderRequestResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CreditOrderRequestResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CreditOrderRequestResponse";
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
