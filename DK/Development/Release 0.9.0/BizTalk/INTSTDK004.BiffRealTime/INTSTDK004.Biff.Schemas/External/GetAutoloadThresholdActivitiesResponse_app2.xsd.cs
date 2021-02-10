namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://tempuri.org/CardActivities.xsd",@"CardActivities")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CardActivities"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app3", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app3))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1))]
    public sealed class GetAutoloadThresholdActivitiesResponse_app2 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:mstns=""http://cubic.com"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns=""http://tempuri.org/CardActivities.xsd"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:app1=""urn:schemas-microsoft-com:xml-diffgram-v1"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://tempuri.org/CardActivities.xsd"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType"" namespace=""http://cubic.com"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app3"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <references xmlns=""http://schemas.microsoft.com/BizTalk/2003"">
        <reference targetNamespace=""http://cubic.com"" />
        <reference targetNamespace=""http://tempuri.org/CardActivities.xsd"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CardActivities"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Activities"">
          <xs:complexType>
            <xs:sequence>
              <xs:element app3:Ordinal=""0"" minOccurs=""0"" name=""TransactionDate"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""1"" minOccurs=""0"" name=""DeviceId"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""2"" minOccurs=""0"" name=""TxnNum"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""3"" minOccurs=""0"" name=""RequestId"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""4"" minOccurs=""0"" name=""CardNumber"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""5"" minOccurs=""0"" name=""CardSect"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""6"" minOccurs=""0"" name=""Route"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""7"" minOccurs=""0"" name=""Price"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
              <xs:element app3:Ordinal=""8"" minOccurs=""0"" name=""Currency"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
            </xs:sequence>
            <xs:attribute ref=""app1:id"" />
            <xs:attribute ref=""msdata:rowOrder"" />
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""TransactionCount"">
          <xs:complexType>
            <xs:sequence>
              <xs:element app3:Ordinal=""0"" minOccurs=""0"" name=""Total"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
            </xs:sequence>
            <xs:attribute ref=""app1:id"" />
            <xs:attribute ref=""msdata:rowOrder"" />
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetAutoloadThresholdActivitiesResponse_app2() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CardActivities";
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
