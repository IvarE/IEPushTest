namespace INTSTDK009.Stralfors.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"Orders")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Orders"})]
    public sealed class OrdersType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" id=""Orders"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Orders"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""Order"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""OrderNo"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderTime"" type=""xs:string"" />
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ShippingAddress"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ContactPerson"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""CellPhone"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""EMail"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Address1"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Address2"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""PostalCode"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""City"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""CountryCode"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Products"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Product"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" name=""Reference"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""ProductCode"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""Qty"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" name=""NameOnCard"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public OrdersType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Orders";
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
