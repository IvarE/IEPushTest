namespace INTSTDK008.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.CreateGiftCardRequest", typeof(global::INTSTDK008.STIP.Schemas.CreateGiftCardRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.CreateGiftCardRequestJSON", typeof(global::INTSTDK008.Ehandel.Schemas.CreateGiftCardRequestJSON))]
    public sealed class STIP_CreateGiftCard_To_CreateGiftCard : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008/CreateGiftCardRequest/2014-11-03"" xmlns:ns0=""http://INTSTDK008.Ehandel.Schemas.CreateGiftCardRequestJSON"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CreateGiftCardRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:CreateGiftCardRequest"">
    <ns0:CreateGiftCard>
      <xsl:if test=""s0:CampaignTrackingCode"">
        <ns0:CampaignTrackingCode>
          <xsl:value-of select=""s0:CampaignTrackingCode/text()"" />
        </ns0:CampaignTrackingCode>
      </xsl:if>
      <xsl:if test=""s0:Sum"">
        <ns0:Sum>
          <xsl:value-of select=""s0:Sum/text()"" />
        </ns0:Sum>
      </xsl:if>
      <xsl:if test=""s0:CustomerId"">
        <ns0:CustomerId>
          <xsl:value-of select=""s0:CustomerId/text()"" />
        </ns0:CustomerId>
      </xsl:if>
      <xsl:if test=""s0:Currency"">
        <ns0:Currency>
          <xsl:value-of select=""s0:Currency/text()"" />
        </ns0:Currency>
      </xsl:if>
      <xsl:if test=""s0:ValidTo"">
        <ns0:ValidTo>
          <xsl:value-of select=""s0:ValidTo/text()"" />
        </ns0:ValidTo>
      </xsl:if>
    </ns0:CreateGiftCard>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.STIP.Schemas.CreateGiftCardRequest";
        
        private const global::INTSTDK008.STIP.Schemas.CreateGiftCardRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Ehandel.Schemas.CreateGiftCardRequestJSON";
        
        private const global::INTSTDK008.Ehandel.Schemas.CreateGiftCardRequestJSON _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.STIP.Schemas.CreateGiftCardRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Ehandel.Schemas.CreateGiftCardRequestJSON";
                return _TrgSchemas;
            }
        }
    }
}
