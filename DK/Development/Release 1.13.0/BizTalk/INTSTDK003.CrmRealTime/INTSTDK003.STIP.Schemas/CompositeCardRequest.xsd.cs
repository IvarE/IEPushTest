namespace INTSTDK003.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215",@"crmCardRequest")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK003.STIP.Schemas.operation), XPath = @"/*[local-name()='crmCardRequest' and namespace-uri()='http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215']/*[local-name()='operation' and namespace-uri()='http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"crmCardRequest"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.PortalService_1", typeof(global::INTSTDK003.STIP.Schemas.PortalService_1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.PortalService_1_2", typeof(global::INTSTDK003.STIP.Schemas.PortalService_1_2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.PropertySchema", typeof(global::INTSTDK003.STIP.Schemas.PropertySchema))]
    public sealed class CompositeCardRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK003.EHandel.Schemas.PropertySchema"" xmlns:stdk=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215"" xmlns=""http://www.w3.org/2001/XMLSchema"">
  <import schemaLocation=""INTSTDK003.STIP.Schemas.PortalService_1"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1/20141128"" />
  <import schemaLocation=""INTSTDK003.STIP.Schemas.PortalService_1_2"" namespace=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2/20141128"" />
  <annotation>
    <appinfo>
      <b:references>
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1/20141128"" />
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2_3/20141128"" />
        <b:reference targetNamespace=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2/20141128"" />
      </b:references>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK003.EHandel.Schemas.PropertySchema"" location=""INTSTDK003.STIP.Schemas.PropertySchema"" />
      </b:imports>
    </appinfo>
  </annotation>
  <element name=""crmCardRequest"" type=""stdk:crmCardRequestType"">
    <annotation>
      <appinfo>
        <b:properties>
          <b:property name=""ns0:operation"" xpath=""/*[local-name()='crmCardRequest' and namespace-uri()='http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215']/*[local-name()='operation' and namespace-uri()='http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215']"" />
        </b:properties>
      </appinfo>
    </annotation>
  </element>
  <complexType name=""cardDetailsType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""AutoloadConnectionDate"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""AutoloadDisconnectionDate"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""AutoloadStatus"" type=""integer"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""Blocked"" type=""boolean"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""CardCategory"" type=""int"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""CardName"" nillable=""true"" type=""string"" />
      <element name=""CardNumber"" nillable=""true"" type=""string"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""CreditCardMask"" nillable=""true"" type=""string"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""Currency"" nillable=""true"" type=""string"" />
      <element xmlns:q1=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2/20141128"" name=""CustomerType"" type=""q1:AccountCategoryCode"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""FailedAttemptsToChargeMoney"" type=""int"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""LatestAutoloadAmount"" type=""decimal"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""LatestChargeDate"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""LatestFailedAttempt"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""PeriodCardTypeId"" type=""int"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""PeriodCardTypeTitle"" nillable=""true"" type=""string"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""PeriodValidFrom"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""PeriodValidTo"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""ValueCardTypeId"" type=""int"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""ValueCardTypeTitle"" nillable=""true"" type=""string"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""VerifyId"" nillable=""true"" type=""string"" />
    </sequence>
  </complexType>
  <complexType name=""crmCardRequestType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""operation"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""10"" />
            <enumeration value=""get"" />
            <enumeration value=""create"" />
            <enumeration value=""update"" />
            <enumeration value=""delete"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""1"" maxOccurs=""1"" name=""accntNr"" type=""stdk:accntNbrType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""card"" type=""stdk:cardDetailsType"" />
    </sequence>
  </complexType>
  <simpleType name=""accntNbrType"">
    <annotation>
      <documentation>Customer account number (Guid)</documentation>
    </annotation>
    <restriction base=""string"" />
  </simpleType>
</schema>";
        
        public CompositeCardRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "crmCardRequest";
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
