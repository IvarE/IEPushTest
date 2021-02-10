namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.GetGiftCardResponse", typeof(global::INTSTDK008.Ehandel.Schemas.GetGiftCardResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.GetGiftCardResponseJSON", typeof(global::INTSTDK008.STIP.Schemas.GetGiftCardResponseJSON))]
    public sealed class Ehandel_GetGiftCardResponse_To_GetGiftCardResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.GetGiftCardResponse"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008/GetGiftCardResponseJSON/20150915"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetGiftCardResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:GetGiftCardResponse"">
    <ns0:GetGiftCardResponse>
      <xsl:for-each select=""GiftCards"">
        <GiftCards>
          <xsl:if test=""Currency"">
            <Currency>
              <xsl:value-of select=""Currency/text()"" />
            </Currency>
          </xsl:if>
          <xsl:if test=""Amount"">
            <Amount>
              <xsl:value-of select=""Amount/text()"" />
            </Amount>
          </xsl:if>
          <xsl:if test=""ValidTo"">
            <ValidTo>
              <xsl:value-of select=""ValidTo/text()"" />
            </ValidTo>
          </xsl:if>
          <xsl:if test=""GiftCardCode"">
            <GiftCardCode>
              <xsl:value-of select=""GiftCardCode/text()"" />
            </GiftCardCode>
          </xsl:if>
          <xsl:if test=""OrderTrackingNumber"">
            <OrderTrackingNumber>
              <xsl:value-of select=""OrderTrackingNumber/text()"" />
            </OrderTrackingNumber>
          </xsl:if>
          <xsl:value-of select=""./text()"" />
        </GiftCards>
      </xsl:for-each>
      <xsl:if test=""ErrorMessage"">
        <ErrorMessage>
          <xsl:value-of select=""ErrorMessage/text()"" />
        </ErrorMessage>
      </xsl:if>
      <xsl:if test=""StatusCode"">
        <StatusCode>
          <xsl:value-of select=""StatusCode/text()"" />
        </StatusCode>
      </xsl:if>
    </ns0:GetGiftCardResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.GetGiftCardResponse";
        
        private const global::INTSTDK008.Ehandel.Schemas.GetGiftCardResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.GetGiftCardResponseJSON";
        
        private const global::INTSTDK008.STIP.Schemas.GetGiftCardResponseJSON _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.GetGiftCardResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.GetGiftCardResponseJSON";
                return _TrgSchemas;
            }
        }
    }
}
