namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCard", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCard))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCard", typeof(global::INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCard))]
    public sealed class STIP_GetCustomerIDRequest_To_GetCustomerIDRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1 s2 s3 ScriptNS0"" version=""1.0"" xmlns:ns2=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGIXrmEAIConnectorService"" xmlns:ns3=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" xmlns:s1=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2_3/20141126"" xmlns:s3=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126"" xmlns:s2=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:ScriptNS0=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetCustomerIdForTravelCard"" />
  </xsl:template>
  <xsl:template match=""/s3:GetCustomerIdForTravelCard"">
    <ns0:GetCustomerIdForTravelCard>
      <ns0:travelCard>
        <xsl:for-each select=""s3:travelCard"">
          <xsl:for-each select=""s1:string"">
            <xsl:variable name=""var:v1"" select=""string(./@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v1)='true'"">
              <ns2:string>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns2:string>
            </xsl:if>
            <xsl:if test=""string($var:v1)='false'"">
              <ns2:string>
                <xsl:value-of select=""./text()"" />
              </ns2:string>
            </xsl:if>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:travelCard>
      <xsl:variable name=""var:v2"" select=""ScriptNS0:ReadSSOValue(&quot;INTSTDK003&quot; , &quot;CallerId&quot;)"" />
      <ns0:callerId>
        <xsl:value-of select=""$var:v2"" />
      </ns0:callerId>
    </ns0:GetCustomerIdForTravelCard>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects>
  <ExtensionObject Namespace=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"" AssemblyName=""INTSTDK003.Helper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41"" ClassName=""INTSTDK003.Helper.INTSTDK003Helper"" />
</ExtensionObjects>";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCard";
        
        private const global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCard _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCard";
        
        private const global::INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCard _trgSchemaTypeReference0 = null;
        
        public override string XmlContent {
            get {
                return _strMap;
            }
        }
        
        public override string XsltArgumentListContent {
            get {
                return _strArgList;
            }
        }
        
        public override string[] SourceSchemas {
            get {
                string[] _SrcSchemas = new string [1];
                _SrcSchemas[0] = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCard";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCard";
                return _TrgSchemas;
            }
        }
    }
}
