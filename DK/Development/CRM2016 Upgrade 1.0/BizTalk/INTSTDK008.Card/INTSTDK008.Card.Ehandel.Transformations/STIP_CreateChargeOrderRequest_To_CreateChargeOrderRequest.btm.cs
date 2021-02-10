namespace INTSTDK008.Card.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.CreateChargeOrderRequest", typeof(global::INTSTDK008.Card.STIP.Schemas.CreateChargeOrderRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.CreateChargeOrderRequest", typeof(global::INTSTDK008.Card.Ehandel.Schemas.CreateChargeOrderRequest))]
    public sealed class STIP_CreateChargeOrderRequest_To_CreateChargeOrderRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/CreateChargeOrderRequest/20150310"" xmlns:ns0=""http://INTSTDK008.Card.Ehandel.Schemas.CreateChargeOrderRequest"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CreateChargeOrderRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:CreateChargeOrderRequest"">
    <ns0:CreateChargeOrder>
      <xsl:if test=""Amount"">
        <Amount>
          <xsl:value-of select=""Amount/text()"" />
        </Amount>
      </xsl:if>
      <xsl:if test=""CardNumber"">
        <CardNumber>
          <xsl:value-of select=""CardNumber/text()"" />
        </CardNumber>
      </xsl:if>
      <xsl:if test=""Currency"">
        <Currency>
          <xsl:value-of select=""Currency/text()"" />
        </Currency>
      </xsl:if>
      <xsl:if test=""CustomerId"">
        <xsl:variable name=""var:v1"" select=""string(CustomerId/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v1)='true'"">
          <CustomerId>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </CustomerId>
        </xsl:if>
        <xsl:if test=""string($var:v1)='false'"">
          <CustomerId>
            <xsl:value-of select=""CustomerId/text()"" />
          </CustomerId>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""Email"">
        <Email>
          <xsl:value-of select=""Email/text()"" />
        </Email>
      </xsl:if>
    </ns0:CreateChargeOrder>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.CreateChargeOrderRequest";
        
        private const global::INTSTDK008.Card.STIP.Schemas.CreateChargeOrderRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.CreateChargeOrderRequest";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.CreateChargeOrderRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.CreateChargeOrderRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.CreateChargeOrderRequest";
                return _TrgSchemas;
            }
        }
    }
}
