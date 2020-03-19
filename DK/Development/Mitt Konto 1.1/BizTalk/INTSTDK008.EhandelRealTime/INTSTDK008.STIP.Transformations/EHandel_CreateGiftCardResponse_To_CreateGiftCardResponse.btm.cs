namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.CreateGiftCardResponseJSON", typeof(global::INTSTDK008.Ehandel.Schemas.CreateGiftCardResponseJSON))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.CreateGiftCardResponse", typeof(global::INTSTDK008.STIP.Schemas.CreateGiftCardResponse))]
    public sealed class EHandel_CreateGiftCardResponse_To_CreateGiftCardResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008/CreateGiftCardResponse/20141103"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.CreateGiftCardResponseJSON"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CreateGiftCardResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:CreateGiftCardResponse"">
    <ns0:CreateGiftCardResponse>
      <xsl:if test=""s0:Code"">
        <Code>
          <xsl:value-of select=""s0:Code/text()"" />
        </Code>
      </xsl:if>
      <xsl:if test=""s0:CampaignTrackingCode"">
        <CampaignTrackingCode>
          <xsl:value-of select=""s0:CampaignTrackingCode/text()"" />
        </CampaignTrackingCode>
      </xsl:if>
      <xsl:if test=""s0:Sum"">
        <Sum>
          <xsl:value-of select=""s0:Sum/text()"" />
        </Sum>
      </xsl:if>
      <xsl:if test=""s0:CustomerId"">
        <CustomerId>
          <xsl:value-of select=""s0:CustomerId/text()"" />
        </CustomerId>
      </xsl:if>
      <xsl:if test=""s0:Currency"">
        <Currency>
          <xsl:value-of select=""s0:Currency/text()"" />
        </Currency>
      </xsl:if>
      <xsl:if test=""s0:ValidTo"">
        <ValidTo>
          <xsl:value-of select=""s0:ValidTo/text()"" />
        </ValidTo>
      </xsl:if>
      <xsl:if test=""s0:ErrorMessage"">
        <ErrorMessage>
          <xsl:value-of select=""s0:ErrorMessage/text()"" />
        </ErrorMessage>
      </xsl:if>
      <xsl:if test=""s0:StatusCode"">
        <StatusCode>
          <xsl:value-of select=""s0:StatusCode/text()"" />
        </StatusCode>
      </xsl:if>
    </ns0:CreateGiftCardResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.CreateGiftCardResponseJSON";
        
        private const global::INTSTDK008.Ehandel.Schemas.CreateGiftCardResponseJSON _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.CreateGiftCardResponse";
        
        private const global::INTSTDK008.STIP.Schemas.CreateGiftCardResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.CreateGiftCardResponseJSON";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.CreateGiftCardResponse";
                return _TrgSchemas;
            }
        }
    }
}
