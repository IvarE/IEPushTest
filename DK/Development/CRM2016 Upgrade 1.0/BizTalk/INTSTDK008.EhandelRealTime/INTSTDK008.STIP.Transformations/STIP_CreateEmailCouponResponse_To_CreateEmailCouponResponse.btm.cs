namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.CreateEmailCouponResponse", typeof(global::INTSTDK008.Ehandel.Schemas.CreateEmailCouponResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.CreateEmailCouponResponse", typeof(global::INTSTDK008.STIP.Schemas.CreateEmailCouponResponse))]
    public sealed class STIP_CreateEmailCouponResponse_To_CreateEmailCouponResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008/CreateEmailCouponResponse/20150811"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.CreateEmailCouponResponse"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CreateEmailCouponResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:CreateEmailCouponResponse"">
    <ns0:CreateEmailCouponResponse>
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
    </ns0:CreateEmailCouponResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.CreateEmailCouponResponse";
        
        private const global::INTSTDK008.Ehandel.Schemas.CreateEmailCouponResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.CreateEmailCouponResponse";
        
        private const global::INTSTDK008.STIP.Schemas.CreateEmailCouponResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.CreateEmailCouponResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.CreateEmailCouponResponse";
                return _TrgSchemas;
            }
        }
    }
}
