namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsRequest", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.EAIConnectionService.EAIConnectorService+GetCustomerDetails", typeof(global::INTSTDK003.CRM.Schemas.EAIConnectionService.EAIConnectorService.GetCustomerDetails))]
    public sealed class STIP_GetCustomerDetailRequest_To_GetCustomerDetailRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGIXrmEAIConnectorService"" xmlns:ns2=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK003/GetCustomerDetailsRequest/20141029"" xmlns:ns3=""http://schemas.microsoft.com/2003/10/Serialization/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetCustomerDetails"" />
  </xsl:template>
  <xsl:template match=""/s0:GetCustomerDetails"">
    <ns0:GetCustomerDetails>
      <xsl:if test=""customerId"">
        <ns0:customerId>
          <xsl:value-of select=""customerId/text()"" />
        </ns0:customerId>
      </xsl:if>
      <xsl:if test=""callerId"">
        <ns0:callerId>
          <xsl:value-of select=""callerId/text()"" />
        </ns0:callerId>
      </xsl:if>
    </ns0:GetCustomerDetails>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsRequest";
        
        private const global::INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.EAIConnectionService.EAIConnectorService+GetCustomerDetails";
        
        private const global::INTSTDK003.CRM.Schemas.EAIConnectionService.EAIConnectorService.GetCustomerDetails _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.EAIConnectionService.EAIConnectorService+GetCustomerDetails";
                return _TrgSchemas;
            }
        }
    }
}
