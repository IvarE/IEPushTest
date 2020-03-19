namespace INTSTDK003.EHandel.Schemas.Internal {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime",@"crmCustomerRequest")]
    [Microsoft.XLANGs.BaseTypes.PropertyAttribute(typeof(global::INTSTDK003.EHandel.Schemas.PropertySchema.operation), XPath = @"/*[local-name()='crmCustomerRequest' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime']/*[local-name()='operation' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"crmCustomerRequest"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.PropertySchema.PropertySchema", typeof(global::INTSTDK003.EHandel.Schemas.PropertySchema.PropertySchema))]
    public sealed class CompositeRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ns0=""https://INTSTDK003.EHandel.Schemas.PropertySchema"" xmlns:stdk=""http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime"" xmlns=""http://www.w3.org/2001/XMLSchema"">
  <annotation>
    <appinfo>
      <b:imports>
        <b:namespace prefix=""ns0"" uri=""https://INTSTDK003.EHandel.Schemas.PropertySchema"" location=""INTSTDK003.EHandel.Schemas.PropertySchema.PropertySchema"" />
      </b:imports>
    </appinfo>
  </annotation>
  <element name=""crmCustomerRequest"" type=""stdk:crmCustomerRequestType"">
    <annotation>
      <appinfo>
        <b:properties>
          <b:property name=""ns0:operation"" xpath=""/*[local-name()='crmCustomerRequest' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime']/*[local-name()='operation' and namespace-uri()='http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime']"" />
        </b:properties>
      </appinfo>
    </annotation>
  </element>
  <complexType name=""addressType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""addressId"">
        <annotation>
          <documentation>Unique id for address entity</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""companyName"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""firstName"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""lastName"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""street"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""zipPostalCode"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""city"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""county"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""country"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""careOff"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""contactPerson"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""250"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""contactPhnNbr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""smsNotifNbr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""emailNotifAdr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""isPrimaryAdr"" type=""boolean"" />
      <element minOccurs=""1"" maxOccurs=""1"" name=""adrType"">
        <annotation>
          <documentation>
						1: Delivery 2: Invoice 3: None
					</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <enumeration value=""None"" />
            <enumeration value=""Invoice"" />
            <enumeration value=""Delivery"" />
          </restriction>
        </simpleType>
      </element>
    </sequence>
  </complexType>
  <complexType name=""accountType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""compName"">
        <annotation>
          <documentation>Mandatory for company customer</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""firstName"">
        <annotation>
          <documentation />
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""lastName"">
        <annotation>
          <documentation />
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""1"" maxOccurs=""1"" name=""mobPhone"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""phone"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""1"" maxOccurs=""1"" name=""email"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""allowBulkMail"" type=""boolean"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""allowPhone"" type=""boolean"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""allowEmail"" type=""boolean"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""allowMail"" type=""boolean"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""allowAutoLoad"" type=""boolean"">
        <annotation>
          <documentation>
						Possible to turn off autoload for customer
					</documentation>
        </annotation>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""maxCardsAutoLoad"" type=""int"">
        <annotation>
          <documentation>
						Max nbr of cards allowed for autoload
					</documentation>
        </annotation>
      </element>
      <element minOccurs=""1"" maxOccurs=""1"" name=""custType"">
        <annotation>
          <documentation>1: Private 2: Company</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <enumeration value=""Private"" />
            <enumeration value=""Company"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""orgCreditApproved"" type=""boolean"">
        <annotation>
          <documentation>
						Mandatory for companies
					</documentation>
        </annotation>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""orgNbr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""orgSubNbr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""socialSecNbr"">
        <annotation>
          <documentation>
						PASS - for private customers
					</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""inactive"" type=""boolean"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""deleted"" type=""boolean"">
        <annotation>
          <documentation>
						when customer unregistered on home page
					</documentation>
        </annotation>
      </element>
    </sequence>
  </complexType>
  <complexType name=""setAccountType"">
    <annotation>
      <documentation>Create and Update</documentation>
    </annotation>
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""accountNbr"" type=""stdk:accountNbrType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""account"" type=""stdk:accountType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""adresses"" type=""stdk:addressesType"" />
    </sequence>
  </complexType>
  <complexType name=""getAccountType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""nbrGUID"">
        <annotation>
          <documentation>GUID for specific account in CRM</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
    </sequence>
  </complexType>
  <complexType name=""accountExistsType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""email"">
        <annotation>
          <documentation>Email address for Customer</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
    </sequence>
  </complexType>
  <complexType name=""crmCustomerRequestType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""operation"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""10"" />
            <enumeration value=""exists"" />
            <enumeration value=""get"" />
            <enumeration value=""create"" />
            <enumeration value=""update"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""customerExists"" type=""stdk:accountExistsType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""getAccount"" type=""stdk:getAccountType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""setAccount"" type=""stdk:setAccountType"">
        <annotation>
          <documentation>Create and Update</documentation>
        </annotation>
      </element>
      <element minOccurs=""0"" name=""callerId"" type=""string"" />
    </sequence>
  </complexType>
  <complexType name=""addressesType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""unbounded"" name=""address"" type=""stdk:addressType"" />
    </sequence>
  </complexType>
  <complexType name=""accountNbrType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""nbrGUID"">
        <annotation>
          <documentation>
						Guid for specific account: Empty for Create,
						mandatory for Update. Created in CRM upon create
						customer and sent
						back
					</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""nbr2"">
        <annotation>
          <documentation>
						Account Number, unique sequence
					</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
    </sequence>
  </complexType>
</schema>";
        
        public CompositeRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "crmCustomerRequest";
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
