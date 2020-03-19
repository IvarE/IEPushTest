namespace INTSTDK012.Kuponginlosen.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.STIP.Schemas.CouponService+createCouponForPrinting", typeof(global::INTSTDK012.STIP.Schemas.CouponService.createCouponForPrinting))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createCouponForPrinting", typeof(global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.createCouponForPrinting))]
    public sealed class STIP_CreateCouponForPrinting_To_CreateCouponForPrinting : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK012.STIP.CouponService.Schemas/20150316"" xmlns:ns0=""http://service.web.couponcreator.kuponginlosen.se/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:createCouponForPrinting"" />
  </xsl:template>
  <xsl:template match=""/s0:createCouponForPrinting"">
    <ns0:createCouponForPrinting>
      <xsl:for-each select=""campaignNumberRequestWithAddress"">
        <campaignNumberRequestWithAddress>
          <validityInDays>
            <xsl:value-of select=""validityInDays/text()"" />
          </validityInDays>
          <campaignNumber>
            <xsl:value-of select=""campaignNumber/text()"" />
          </campaignNumber>
          <amount>
            <xsl:value-of select=""amount/text()"" />
          </amount>
          <firstName>
            <xsl:value-of select=""firstName/text()"" />
          </firstName>
          <familyName>
            <xsl:value-of select=""familyName/text()"" />
          </familyName>
          <socialSecurityNumber>
            <xsl:value-of select=""socialSecurityNumber/text()"" />
          </socialSecurityNumber>
          <street>
            <xsl:value-of select=""street/text()"" />
          </street>
          <streetNumber>
            <xsl:value-of select=""streetNumber/text()"" />
          </streetNumber>
          <xsl:if test=""coAddress"">
            <coAddress>
              <xsl:value-of select=""coAddress/text()"" />
            </coAddress>
          </xsl:if>
          <zipCode>
            <xsl:value-of select=""zipCode/text()"" />
          </zipCode>
          <city>
            <xsl:value-of select=""city/text()"" />
          </city>
          <xsl:value-of select=""./text()"" />
        </campaignNumberRequestWithAddress>
      </xsl:for-each>
    </ns0:createCouponForPrinting>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK012.STIP.Schemas.CouponService+createCouponForPrinting";
        
        private const global::INTSTDK012.STIP.Schemas.CouponService.createCouponForPrinting _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createCouponForPrinting";
        
        private const global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.createCouponForPrinting _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK012.STIP.Schemas.CouponService+createCouponForPrinting";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+createCouponForPrinting";
                return _TrgSchemas;
            }
        }
    }
}
