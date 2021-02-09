namespace INTSTDK003.EHandel.Schemas.Internal {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime",@"crmCardRequest")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK003.EHandel.Schemas.PropertySchema.operation), XPath = @"/*[local-name()='crmCardRequest' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime']/*[local-name()='operation' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"crmCardRequest"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.PropertySchema.PropertySchema", typeof(global::INTSTDK003.EHandel.Schemas.PropertySchema.PropertySchema))]
    public sealed class CompositeCardRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK003.EHandel.Schemas.PropertySchema"" xmlns:stdk=""http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime"" xmlns=""http://www.w3.org/2001/XMLSchema"">
  <annotation>
    <appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK003.EHandel.Schemas.PropertySchema"" location=""INTSTDK003.EHandel.Schemas.PropertySchema.PropertySchema"" />
      </b:imports>
    </appinfo>
  </annotation>
  <element name=""crmCardRequest"" type=""stdk:crmCardRequestType"">
    <annotation>
      <appinfo>
        <b:properties>
          <b:property name=""ns0:operation"" xpath=""/*[local-name()='crmCardRequest' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime']/*[local-name()='operation' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime']"" />
        </b:properties>
      </appinfo>
    </annotation>
  </element>
  <complexType name=""cardDetailsType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""cardNbr"" type=""long"">
        <annotation>
          <documentation>Unique identifier</documentation>
        </annotation>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""cardName"" type=""string"">
        <annotation>
          <documentation>
						Customers description of Card
					</documentation>
        </annotation>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""periodCardType"" type=""int"">
        <annotation>
          <documentation />
        </annotation>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""valCardType"" type=""int"">
        <annotation>
          <documentation />
        </annotation>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""periodCardTypeTitle"" type=""string"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""valCardTypeTitle"" type=""string"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""perValidFromDate"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""perValidToDate"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""blocked"" type=""boolean"" />
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
  <complexType name=""cardListType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""unbounded"" name=""cardList"" type=""stdk:cardDetailsType"" />
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
