namespace INTSTDK003.CRM.Schemas.Customer_TravelCard {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"TravelCard", @"ArrayOfTravelCard"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2))]
    public sealed class PortalService_1_2_3 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:ser=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:tns=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1"" namespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
  <xs:import schemaLocation=""INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService_1_2"" namespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://schemas.microsoft.com/2003/10/Serialization/"" />
        <reference targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" />
        <reference targetNamespace=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:complexType name=""TravelCard"">
    <xs:sequence>
      <xs:element name=""AccountId"" type=""ser:guid"" />
      <xs:element minOccurs=""0"" name=""AutoloadConnectionDate"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""AutoloadDisconnectionDate"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""AutoloadStatus"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""Blocked"" type=""xs:boolean"" />
      <xs:element minOccurs=""0"" name=""CardCategory"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""CardName"" nillable=""true"" type=""xs:string"" />
      <xs:element name=""CardNumber"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""CreditCardMask"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""Currency"" nillable=""true"" type=""xs:string"" />
      <xs:element xmlns:q1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" name=""CustomerType"" type=""q1:AccountCategoryCode"" />
      <xs:element minOccurs=""0"" name=""FailedAttemptsToChargeMoney"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""LatestAutoloadAmount"" type=""xs:decimal"" />
      <xs:element minOccurs=""0"" name=""LatestChargeDate"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""LatestFailedAttempt"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""PeriodCardTypeId"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""PeriodCardTypeTitle"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""PeriodValidFrom"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""PeriodValidTo"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" name=""ValueCardTypeId"" type=""xs:int"" />
      <xs:element minOccurs=""0"" name=""ValueCardTypeTitle"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""VerifyId"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""TravelCard"" nillable=""true"" type=""tns:TravelCard"" />
  <xs:complexType name=""ArrayOfTravelCard"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""TravelCard"" nillable=""true"" type=""tns:TravelCard"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfTravelCard"" nillable=""true"" type=""tns:ArrayOfTravelCard"" />
</xs:schema>";
        
        public PortalService_1_2_3() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "TravelCard";
                _RootElements[1] = "ArrayOfTravelCard";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService",@"TravelCard")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"TravelCard"})]
        public sealed class TravelCard : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public TravelCard() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "TravelCard";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/CGICRMPortalService",@"ArrayOfTravelCard")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfTravelCard"})]
        public sealed class ArrayOfTravelCard : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfTravelCard() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfTravelCard";
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
}
