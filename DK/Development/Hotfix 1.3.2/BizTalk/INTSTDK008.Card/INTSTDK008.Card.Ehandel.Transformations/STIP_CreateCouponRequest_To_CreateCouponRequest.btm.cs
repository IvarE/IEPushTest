namespace INTSTDK008.Card.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.CreateCoupon+CreateCouponRequest", typeof(global::INTSTDK008.Card.STIP.Schemas.CreateCoupon.CreateCouponRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.CreateCouponRequest", typeof(global::INTSTDK008.Card.Ehandel.Schemas.CreateCouponRequest))]
    public sealed class STIP_CreateCouponRequest_To_CreateCouponRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://INTSTDK008.Card.Ehandel.Schemas.CreateCouponRequest"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/CreateCoupon/20150310"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CreateCouponRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:CreateCouponRequest"">
    <ns0:CreateCouponRequest>
      <xsl:if test=""Amount"">
        <Amount>
          <xsl:value-of select=""Amount/text()"" />
        </Amount>
      </xsl:if>
      <xsl:if test=""Currency"">
        <Currency>
          <xsl:value-of select=""Currency/text()"" />
        </Currency>
      </xsl:if>
      <xsl:if test=""CustomerId"">
        <CustomerId>
          <xsl:value-of select=""CustomerId/text()"" />
        </CustomerId>
      </xsl:if>
      <xsl:if test=""DistrubutionType"">
        <DistrubutionType>
          <xsl:value-of select=""DistrubutionType/text()"" />
        </DistrubutionType>
      </xsl:if>
      <xsl:if test=""Email"">
        <Email>
          <xsl:value-of select=""Email/text()"" />
        </Email>
      </xsl:if>
      <xsl:if test=""MobilePhone"">
        <MobilePhone>
          <xsl:value-of select=""MobilePhone/text()"" />
        </MobilePhone>
      </xsl:if>
    </ns0:CreateCouponRequest>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.CreateCoupon+CreateCouponRequest";
        
        private const global::INTSTDK008.Card.STIP.Schemas.CreateCoupon.CreateCouponRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.CreateCouponRequest";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.CreateCouponRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.CreateCoupon+CreateCouponRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.CreateCouponRequest";
                return _TrgSchemas;
            }
        }
    }
}
