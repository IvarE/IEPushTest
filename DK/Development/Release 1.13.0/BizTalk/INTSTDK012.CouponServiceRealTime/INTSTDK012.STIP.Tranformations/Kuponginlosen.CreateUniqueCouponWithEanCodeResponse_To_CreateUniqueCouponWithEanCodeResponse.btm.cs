namespace INTSTDK012.STIP.Tranformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createUniqueCouponWithEanCodeResponse", typeof(global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.createUniqueCouponWithEanCodeResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.STIP.Schemas.CouponService+createUniqueCouponWithEanCodeResponse", typeof(global::INTSTDK012.STIP.Schemas.CouponService.createUniqueCouponWithEanCodeResponse))]
    public sealed class Kuponginlosen_CreateUniqueCouponWithEanCodeResponse_To_CreateUniqueCouponWithEanCodeResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://service.web.couponcreator.kuponginlosen.se/"" xmlns:ns0=""http://INTSTDK012.STIP.CouponService.Schemas/20150316"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:createUniqueCouponWithEanCodeResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:createUniqueCouponWithEanCodeResponse"">
    <ns0:createUniqueCouponWithEanCodeResponse>
      <xsl:for-each select=""return"">
        <return>
          <eanCode>
            <xsl:value-of select=""eanCode/text()"" />
          </eanCode>
          <xsl:if test=""lastRedemptionDate"">
            <lastRedemptionDate>
              <xsl:value-of select=""lastRedemptionDate/text()"" />
            </lastRedemptionDate>
          </xsl:if>
          <xsl:if test=""uniqueCode"">
            <uniqueCode>
              <xsl:value-of select=""uniqueCode/text()"" />
            </uniqueCode>
          </xsl:if>
          <xsl:value-of select=""./text()"" />
        </return>
      </xsl:for-each>
    </ns0:createUniqueCouponWithEanCodeResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createUniqueCouponWithEanCodeResponse";
        
        private const global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.createUniqueCouponWithEanCodeResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK012.STIP.Schemas.CouponService+createUniqueCouponWithEanCodeResponse";
        
        private const global::INTSTDK012.STIP.Schemas.CouponService.createUniqueCouponWithEanCodeResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createUniqueCouponWithEanCodeResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK012.STIP.Schemas.CouponService+createUniqueCouponWithEanCodeResponse";
                return _TrgSchemas;
            }
        }
    }
}
