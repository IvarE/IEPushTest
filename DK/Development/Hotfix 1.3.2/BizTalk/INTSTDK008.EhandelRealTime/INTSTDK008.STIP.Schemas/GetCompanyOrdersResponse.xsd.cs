namespace INTSTDK008.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrdersResponse/20141117",@"CompanyOrders")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CompanyOrders"})]
    public sealed class GetCompanyOrdersResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrdersResponse/20141117"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrdersResponse/20141117"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CompanyOrders"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""ErrorMessage"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:string"" />
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Orders"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""OrderNumber"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderStatus"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Currency"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Language"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderDate"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""OrderTotal"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""OrderTotalVat"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""OrderDiscount"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""OrderDiscountVat"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""PaymentTotal"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""PaymentTotalVat"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""ShippingTotal"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""ShippingTotalVat"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""CustomerNumber"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""CustomerNumber2"" type=""xs:unsignedShort"" />
              <xs:element minOccurs=""0"" name=""CustomerOrganizationNumber"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""CustomerName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderItems"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""Code"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""CardNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Amount"" type=""xs:unsignedByte"" />
                    <xs:element minOccurs=""0"" name=""Price"" type=""xs:unsignedShort"" />
                    <xs:element minOccurs=""0"" name=""Tax"" type=""xs:unsignedByte"" />
                    <xs:element minOccurs=""0"" name=""Discount"" type=""xs:unsignedByte"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""PaymentTypes"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""Code"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ReferenceNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Sum"" type=""xs:unsignedShort"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetCompanyOrdersResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CompanyOrders";
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
