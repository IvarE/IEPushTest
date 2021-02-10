namespace INTSTDK008.Card.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.CreateCouponResponse", typeof(global::INTSTDK008.Card.Ehandel.Schemas.CreateCouponResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.CreateCoupon+CreateCouponResponse", typeof(global::INTSTDK008.Card.STIP.Schemas.CreateCoupon.CreateCouponResponse))]
    public sealed class EHandel_CreateCouponResponse_To_CreateCouponResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Card.Ehandel.Schemas.CreateCouponResponse"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/CreateCoupon/20150310"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CreateCouponResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:CreateCouponResponse"">
    <ns0:CreateCouponResponse>
      <xsl:if test=""OrderNumber"">
        <OrderNumber>
          <xsl:value-of select=""OrderNumber/text()"" />
        </OrderNumber>
      </xsl:if>
      <xsl:if test=""OrderCreated"">
        <OrderCreated>
          <xsl:value-of select=""OrderCreated/text()"" />
        </OrderCreated>
      </xsl:if>
      <xsl:if test=""Message"">
        <Message>
          <xsl:value-of select=""Message/text()"" />
        </Message>
      </xsl:if>
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
    </ns0:CreateCouponResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.CreateCouponResponse";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.CreateCouponResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.CreateCoupon+CreateCouponResponse";
        
        private const global::INTSTDK008.Card.STIP.Schemas.CreateCoupon.CreateCouponResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.CreateCouponResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.CreateCoupon+CreateCouponResponse";
                return _TrgSchemas;
            }
        }
    }
}
