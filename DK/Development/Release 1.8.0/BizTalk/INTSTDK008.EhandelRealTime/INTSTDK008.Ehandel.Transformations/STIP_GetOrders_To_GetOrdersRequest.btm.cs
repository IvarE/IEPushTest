namespace INTSTDK008.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.GetOrders", typeof(global::INTSTDK008.STIP.Schemas.GetOrders))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.GetOrdersRequest", typeof(global::INTSTDK008.Ehandel.Schemas.GetOrdersRequest))]
    public sealed class STIP_GetOrders_To_GetOrdersRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008/GetOrders/20141031"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetOrdersRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:GetOrdersRequest"">
    <ns0:GetOrdersRequest>
      <CustomerId>
        <xsl:value-of select=""CustomerId/text()"" />
      </CustomerId>
      <OrderNumber>
        <xsl:value-of select=""OrderNumber/text()"" />
      </OrderNumber>
      <From>
        <xsl:value-of select=""From/text()"" />
      </From>
      <To>
        <xsl:value-of select=""To/text()"" />
      </To>
      <Email>
        <xsl:value-of select=""Email/text()"" />
      </Email>
      <CardNumber>
        <xsl:value-of select=""CardNumber/text()"" />
      </CardNumber>
      <xsl:value-of select=""./text()"" />
    </ns0:GetOrdersRequest>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.STIP.Schemas.GetOrders";
        
        private const global::INTSTDK008.STIP.Schemas.GetOrders _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Ehandel.Schemas.GetOrdersRequest";
        
        private const global::INTSTDK008.Ehandel.Schemas.GetOrdersRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.STIP.Schemas.GetOrders";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Ehandel.Schemas.GetOrdersRequest";
                return _TrgSchemas;
            }
        }
    }
}
