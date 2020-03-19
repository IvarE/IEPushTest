namespace INTSTDK004.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216",@"CardTransactions")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CardTransactions"})]
    public sealed class CardTransactions : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CardTransactions"" nillable=""true"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Transactions"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""Date"" type=""xs:string"" />
              <xs:element name=""DeviceID"" type=""xs:string"" />
              <xs:element name=""TxnNum"" type=""xs:string"" />
              <xs:element name=""CardSect"" type=""xs:string"" />
              <xs:element name=""RecType"" type=""xs:string"" />
              <xs:element name=""TxnType"" type=""xs:string"" />
              <xs:element name=""Route"" type=""xs:string"" />
              <xs:element name=""Currency"" type=""xs:string"" />
              <xs:element name=""Balance"" type=""xs:string"" />
              <xs:element name=""Amount"" type=""xs:string"" />
              <xs:element name=""OrigZone"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CardDetails"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""CardSerialNumber"" type=""xs:string"" />
              <xs:element name=""NumTxnsAvailable"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ZoneList"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ZoneListID"" type=""xs:string"" />
              <xs:element name=""Zone"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""RouteList"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""RouteListID"" type=""xs:string"" />
              <xs:element name=""Route"" type=""xs:string"" />
              <xs:element name=""FromZone"" type=""xs:string"" />
              <xs:element name=""ToZone"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CardTransactions() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CardTransactions";
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
