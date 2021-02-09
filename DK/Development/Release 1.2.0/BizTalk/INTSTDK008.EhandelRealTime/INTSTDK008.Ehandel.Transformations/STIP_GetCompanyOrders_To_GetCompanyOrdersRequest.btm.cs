namespace INTSTDK008.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.GetCompanyOrders+GetCompanyOrdersRequest", typeof(global::INTSTDK008.STIP.Schemas.GetCompanyOrders.GetCompanyOrdersRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.GetCompanyOrdersRequest", typeof(global::INTSTDK008.Ehandel.Schemas.GetCompanyOrdersRequest))]
    public sealed class STIP_GetCompanyOrders_To_GetCompanyOrdersRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrders/20141031"" xmlns:ns0=""http://INTSTDK008.Ehandel.Schemas.GetCompanyOrdersRequest"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetCompanyOrdersRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:GetCompanyOrdersRequest"">
    <ns0:GetCompanyOrdersRequest>
      <createdFrom>
        <xsl:value-of select=""createdFrom/text()"" />
      </createdFrom>
    </ns0:GetCompanyOrdersRequest>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.STIP.Schemas.GetCompanyOrders+GetCompanyOrdersRequest";
        
        private const global::INTSTDK008.STIP.Schemas.GetCompanyOrders.GetCompanyOrdersRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Ehandel.Schemas.GetCompanyOrdersRequest";
        
        private const global::INTSTDK008.Ehandel.Schemas.GetCompanyOrdersRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.STIP.Schemas.GetCompanyOrders+GetCompanyOrdersRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Ehandel.Schemas.GetCompanyOrdersRequest";
                return _TrgSchemas;
            }
        }
    }
}
