namespace INTSTDK012.Kuponginlosen.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.STIP.Schemas.CouponService+createUniqueCouponBatch", typeof(global::INTSTDK012.STIP.Schemas.CouponService.createUniqueCouponBatch))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createUniqueCouponBatch", typeof(global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.createUniqueCouponBatch))]
    public sealed class STIP_CreateUniqueCouponBatch_To_CreateUniqueCouponBatch : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK012.STIP.CouponService.Schemas/20150316"" xmlns:ns0=""http://service.web.couponcreator.kuponginlosen.se/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:createUniqueCouponBatch"" />
  </xsl:template>
  <xsl:template match=""/s0:createUniqueCouponBatch"">
    <ns0:createUniqueCouponBatch>
      <xsl:for-each select=""campaignNumberBatchRequest"">
        <campaignNumberBatchRequest>
          <validityInDays>
            <xsl:value-of select=""validityInDays/text()"" />
          </validityInDays>
          <campaignNumber>
            <xsl:value-of select=""campaignNumber/text()"" />
          </campaignNumber>
          <amount>
            <xsl:value-of select=""amount/text()"" />
          </amount>
          <quantity>
            <xsl:value-of select=""quantity/text()"" />
          </quantity>
          <xsl:value-of select=""./text()"" />
        </campaignNumberBatchRequest>
      </xsl:for-each>
    </ns0:createUniqueCouponBatch>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK012.STIP.Schemas.CouponService+createUniqueCouponBatch";
        
        private const global::INTSTDK012.STIP.Schemas.CouponService.createUniqueCouponBatch _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createUniqueCouponBatch";
        
        private const global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.createUniqueCouponBatch _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK012.STIP.Schemas.CouponService+createUniqueCouponBatch";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createUniqueCouponBatch";
                return _TrgSchemas;
            }
        }
    }
}
