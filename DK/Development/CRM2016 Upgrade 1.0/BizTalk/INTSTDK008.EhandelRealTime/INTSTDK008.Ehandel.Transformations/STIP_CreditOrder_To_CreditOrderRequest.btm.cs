namespace INTSTDK008.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.CreditOrder+CreditOrderRequest", typeof(global::INTSTDK008.STIP.Schemas.CreditOrder.CreditOrderRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.CreditOrderRequest", typeof(global::INTSTDK008.Ehandel.Schemas.CreditOrderRequest))]
    public sealed class STIP_CreditOrder_To_CreditOrderRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008/CreditOrder/20141031"" xmlns:ns0=""http://INTSTDK008.Ehandel.Schemas.CreditOrderRequest"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CreditOrderRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:CreditOrderRequest"">
    <ns0:CreditOrderParameters>
      <orderNumber>
        <xsl:value-of select=""orderNumber/text()"" />
      </orderNumber>
      <Sum>
        <xsl:value-of select=""sum/text()"" />
      </Sum>
      <ProductNumber>
        <xsl:value-of select=""ProductNumber/text()"" />
      </ProductNumber>
      <Quantity>
        <xsl:value-of select=""Quantity/text()"" />
      </Quantity>
    </ns0:CreditOrderParameters>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.STIP.Schemas.CreditOrder+CreditOrderRequest";
        
        private const global::INTSTDK008.STIP.Schemas.CreditOrder.CreditOrderRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Ehandel.Schemas.CreditOrderRequest";
        
        private const global::INTSTDK008.Ehandel.Schemas.CreditOrderRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.STIP.Schemas.CreditOrder+CreditOrderRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Ehandel.Schemas.CreditOrderRequest";
                return _TrgSchemas;
            }
        }
    }
}
