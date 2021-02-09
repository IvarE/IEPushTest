namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.GetCustomerDetails.GetCustomerDetailsResponse", typeof(global::INTSTDK003.CRM.Schemas.GetCustomerDetails.GetCustomerDetailsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsResponse", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsResponse))]
    public sealed class CRM_GetCustomerDetailResponse_To_GetCustomerDetailResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1"" version=""1.0"" xmlns:s1=""http://tempuri.org/"" xmlns:s0=""http://schemas.datacontract.org/2004/07/CGIXrmEAIConnectorService"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK003/GetCustomerDetailsResponse/20141029"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:GetCustomerDetailsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:GetCustomerDetailsResponse"">
    <ns0:GetCustomerDetailsResponse>
      <xsl:for-each select=""s1:GetCustomerDetailsResult"">
        <ns0:GetCustomerDetailsResult>
          <xsl:if test=""s0:Message"">
            <ns0:Message>
              <xsl:value-of select=""s0:Message/text()"" />
            </ns0:Message>
          </xsl:if>
          <xsl:if test=""s0:Status"">
            <ns0:Status>
              <xsl:value-of select=""s0:Status/text()"" />
            </ns0:Status>
          </xsl:if>
          <xsl:if test=""s0:Customer"">
            <ns0:Customer>
              <xsl:value-of select=""s0:Customer/text()"" />
            </ns0:Customer>
          </xsl:if>
          <xsl:value-of select=""./text()"" />
        </ns0:GetCustomerDetailsResult>
      </xsl:for-each>
    </ns0:GetCustomerDetailsResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.GetCustomerDetailsResponse";
        
        private const global::INTSTDK003.CRM.Schemas.GetCustomerDetails.GetCustomerDetailsResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsResponse";
        
        private const global::INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.GetCustomerDetailsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.GetCustomerDetailsResponse";
                return _TrgSchemas;
            }
        }
    }
}
