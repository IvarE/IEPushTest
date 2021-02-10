namespace INTSTDK003.STIP.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.com/DK/INSTDK003/CompositeResponse/20141215",@"crmCustomerResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"crmCustomerResponse"})]
    public sealed class CompositeResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:stdk=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeResponse/20141215"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeResponse/20141215"" xmlns=""http://www.w3.org/2001/XMLSchema"">
  <complexType name=""statusType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""status"">
        <simpleType>
          <restriction base=""string"">
            <enumeration value=""SUCCESS"" />
            <enumeration value=""FAILED"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""message"">
        <annotation>
          <documentation>0 = error 1 = success</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"" />
        </simpleType>
      </element>
    </sequence>
  </complexType>
  <complexType name=""accountExistsType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""nbrGUID"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""nbr2"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""contactValExists"" type=""boolean"">
        <annotation>
          <documentation>returned from CRM</documentation>
        </annotation>
      </element>
    </sequence>
  </complexType>
  <complexType name=""crmCustomerResponseType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""customerExists"" type=""stdk:accountExistsType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""customer"" nillable=""true"" type=""stdk:customerType"" />
      <element minOccurs=""1"" maxOccurs=""1"" name=""status"" type=""stdk:statusType"" />
    </sequence>
  </complexType>
  <element name=""crmCustomerResponse"" type=""stdk:crmCustomerResponseType"" />
  <complexType name=""addressesType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""unbounded"" name=""address"" type=""stdk:addressType"" />
    </sequence>
  </complexType>
  <complexType name=""addressType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""addressId"">
        <annotation>
          <documentation>Unique id for address entity</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""companyName"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""firstName"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""45"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""lastName"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""45"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""street"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""zipPostalCode"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""20"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""city"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""county"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""country"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""careOff"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""contactPerson"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""contactPhnNbr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""30"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""smsNotifNbr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""30"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""emailNotifAdr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
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
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""firstName"">
        <annotation>
          <documentation />
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""45"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""lastName"">
        <annotation>
          <documentation />
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""45"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""1"" maxOccurs=""1"" name=""mobPhone"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""30"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""phone"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""30"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""1"" maxOccurs=""1"" name=""email"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
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
      <element minOccurs=""1"" maxOccurs=""1"" name=""custType"" nillable=""true"">
        <annotation>
          <documentation>1: Private 2: Company</documentation>
        </annotation>
        <simpleType>
          <restriction base=""string"">
            <enumeration value=""Private"" />
            <enumeration value=""Company"" />
            <maxLength value=""50"" />
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
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""orgSubNbr"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""50"" />
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
            <maxLength value=""50"" />
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
  <complexType name=""customerType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""accountNbr"" type=""stdk:accountNbrType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""account"" type=""stdk:accountType"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""adresses"" type=""stdk:addressesType"" />
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
            <maxLength value=""50"" />
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
            <maxLength value=""50"" />
          </restriction>
        </simpleType>
      </element>
    </sequence>
  </complexType>
</schema>";
        
        public CompositeResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "crmCustomerResponse";
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
