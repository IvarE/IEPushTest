namespace INTSTDK003.EHandel.Schemas.Internal {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://www.skanetrafiken.se/INSTDK003.22.CrmRealTime",@"crmCardResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"crmCardResponse"})]
    public sealed class CompositeCardResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:stdk=""http://www.skanetrafiken.se/INSTDK003.22.CrmRealTime"" elementFormDefault=""qualified"" targetNamespace=""http://www.skanetrafiken.se/INSTDK003.22.CrmRealTime"" xmlns=""http://www.w3.org/2001/XMLSchema"">
  <element name=""crmCardResponse"" type=""stdk:crmCardResponseType"" />
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
      <element minOccurs=""0"" name=""perValidToDate"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""valCardTypeTitle"" type=""string"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""perValidFromDate"" type=""dateTime"" />
      <element minOccurs=""0"" maxOccurs=""1"" name=""blocked"" type=""boolean"" />
    </sequence>
  </complexType>
  <complexType name=""crmCardResponseType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""1"" name=""cards"" type=""stdk:cardListType"" />
      <element minOccurs=""1"" maxOccurs=""1"" name=""status"" type=""stdk:statusType"" />
    </sequence>
  </complexType>
  <complexType name=""cardListType"">
    <sequence>
      <element minOccurs=""0"" maxOccurs=""unbounded"" name=""card"" type=""stdk:cardDetailsType"" />
    </sequence>
  </complexType>
  <complexType name=""statusType"">
    <sequence>
      <element minOccurs=""1"" maxOccurs=""1"" name=""status"" type=""int"">
        <annotation>
          <documentation>0 = error 1 = success</documentation>
        </annotation>
      </element>
      <element minOccurs=""0"" maxOccurs=""1"" name=""message"">
        <simpleType>
          <restriction base=""string"">
            <maxLength value=""100"" />
          </restriction>
        </simpleType>
      </element>
    </sequence>
  </complexType>
</schema>";
        
        public CompositeCardResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "crmCardResponse";
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
