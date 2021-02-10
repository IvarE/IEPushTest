namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://cubic.com",@"GetAutoloadThresholdActivitiesResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"GetAutoloadThresholdActivitiesResponse"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app2", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app3", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app3))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1))]
    public sealed class GetAutoloadThresholdActivitiesResponseType : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:app2=""http://tempuri.org/CardActivities.xsd"" xmlns:app1=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:mstns=""http://cubic.com"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""http://cubic.com"" id=""GetAutoloadThresholdActivitiesResponse"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app2"" namespace=""http://tempuri.org/CardActivities.xsd"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app3"" namespace=""urn:schemas-microsoft-com:xml-msdata"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1"" namespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://cubic.com"" />
        <b:reference targetNamespace=""http://tempuri.org/CardActivities.xsd"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" />
        <b:reference targetNamespace=""urn:schemas-microsoft-com:xml-diffgram-v1"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" app3:IsDataSet=""true"" app3:Locale=""en-US"" name=""GetAutoloadThresholdActivitiesResponse"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""GetAutoloadThresholdActivitiesResult"">
          <xs:complexType>
            <xs:sequence>
              <xs:any processContents=""skip"" />
              <xs:element ref=""app1:diffgram"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public GetAutoloadThresholdActivitiesResponseType() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "GetAutoloadThresholdActivitiesResponse";
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
