namespace INTSTDK008.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.se/DK/INTSTDK008/GetOrdersResponseJSON/20150918",@"GetOrdersResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetOrdersResponse"})]
    public sealed class GetOrdersResponseJSON : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" targetNamespace=""http://www.skanetrafiken.se/DK/INTSTDK008/GetOrdersResponseJSON/20150918"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""GetOrdersResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Orders"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""OrderConfirmationEmail"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderDate"" type=""xs:dateTime"" />
              <xs:element minOccurs=""0"" name=""OrderNumber"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderStatus"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""System"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SystemExtraInfo"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderTotal"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""OrderTotalVat"" type=""xs:decimal"" />
              <xs:element minOccurs=""0"" name=""OrderCreditedTotal"" type=""xs:unsignedByte"" />
              <xs:element minOccurs=""0"" name=""OrderType"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""CampaignCode"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ExtraInfo"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Customer"">
                <xs:complexType mixed=""true"">
                  <xs:sequence minOccurs=""0"">
                    <xs:element minOccurs=""0"" name=""Email"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""AccountNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""AccountNumber2"" type=""xs:unsignedShort"" />
                    <xs:element minOccurs=""0"" name=""IsCompany"" type=""xs:boolean"" />
                    <xs:element minOccurs=""0"" name=""ExtraInfo"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""CustomerName"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""ShippingAddress"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""Address"" type=""xs:string"" />
                    <xs:element name=""City"" type=""xs:string"" />
                    <xs:element name=""Co"" type=""xs:string"" />
                    <xs:element name=""CompanyName"" type=""xs:string"" />
                    <xs:element name=""Country"" type=""xs:string"" />
                    <xs:element name=""CellPhoneNumber"" type=""xs:string"" />
                    <xs:element name=""Email"" type=""xs:string"" />
                    <xs:element name=""FirstName"" type=""xs:string"" />
                    <xs:element name=""LastName"" type=""xs:string"" />
                    <xs:element name=""PostalCode"" type=""xs:string"" />
                    <xs:element name=""ExtraInfo"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""OrderItems"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""Code"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""CardNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Price"" type=""xs:decimal"" />
                    <xs:element minOccurs=""0"" name=""Discount"" type=""xs:unsignedByte"" />
                    <xs:element minOccurs=""0"" name=""Quantity"" type=""xs:unsignedByte"" />
                    <xs:element minOccurs=""0"" name=""ExtraInfo"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Payments"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""PaymentMethodCode"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""PaymentMethodName"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""TransactionType"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Status"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""GiftCardCode"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Sum"" type=""xs:decimal"" />
                    <xs:element minOccurs=""0"" name=""TransactionId"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ExtraInfo"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""OrderNotes"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""Created"" type=""xs:dateTime"" />
                    <xs:element minOccurs=""0"" name=""Message"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""Coupons"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""BlockedCardNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""CouponCode"" type=""xs:unsignedLong"" />
                    <xs:element minOccurs=""0"" name=""DateCreated"" type=""xs:dateTime"" />
                    <xs:element minOccurs=""0"" name=""DateSent"" type=""xs:dateTime"" />
                    <xs:element minOccurs=""0"" name=""IsSent"" type=""xs:boolean"" />
                    <xs:element minOccurs=""0"" name=""Receiver"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ShippingMethod"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ExtraInfo"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""CouponSum"" type=""xs:unsignedLong"" />
                    <xs:element minOccurs=""0"" name=""Attempts"" type=""xs:unsignedByte"" />
                    <xs:element minOccurs=""0"" name=""DateForLastAttempt"" type=""xs:dateTime"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" name=""Shipments"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""ShipmentMethodName"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""Status"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ExtraInfo"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""ShipmentMethodCode"" type=""xs:string"" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""ErrorMessage"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:unsignedByte"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetOrdersResponseJSON() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetOrdersResponse";
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
