namespace INTSTDK004.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK004/GetCardTransactionResponse/20141216",@"GetCardTransactionsResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetCardTransactionsResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.STIP.Schemas.CardTransactions", typeof(global::INTSTDK004.STIP.Schemas.CardTransactions))]
    public sealed class GetCardTransactionsResponseType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK004/GetCardTransactionResponse/20141216"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/GetCardTransactionResponse/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.STIP.Schemas.CardTransactions"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""GetCardTransactionsResponse"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""GetCardTransactionsResult"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""1"" ref=""ns0:CardTransactions"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetCardTransactionsResponseType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetCardTransactionsResponse";
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
