namespace INTSTDK004.Biff.Schemas.External {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app2", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app2))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app1))]
    public sealed class GetAutoloadThresholdActivitiesResponse_app3 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:app2=""http://tempuri.org/CardActivities.xsd"" xmlns:mstns=""http://cubic.com"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns=""urn:schemas-microsoft-com:xml-msdata"" xmlns:app1=""urn:schemas-microsoft-com:xml-diffgram-v1"" attributeFormDefault=""qualified"" elementFormDefault=""qualified"" targetNamespace=""urn:schemas-microsoft-com:xml-msdata"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType"" namespace=""http://cubic.com"" />
  <xs:import schemaLocation=""INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponse_app2"" namespace=""http://tempuri.org/CardActivities.xsd"" />
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
  <xs:attribute app3:Prefix=""msdata"" name=""rowOrder"" type=""xs:string"" xmlns:app3=""urn:schemas-microsoft-com:xml-msdata"" />
</xs:schema>";
        
        public GetAutoloadThresholdActivitiesResponse_app3() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [0];
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
