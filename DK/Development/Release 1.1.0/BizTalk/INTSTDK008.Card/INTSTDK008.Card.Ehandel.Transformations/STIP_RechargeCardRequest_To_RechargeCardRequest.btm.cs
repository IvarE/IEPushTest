namespace INTSTDK008.Card.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.RechargeCardRequest", typeof(global::INTSTDK008.Card.STIP.Schemas.RechargeCardRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.RechargeCardRequest", typeof(global::INTSTDK008.Card.Ehandel.Schemas.RechargeCardRequest))]
    public sealed class STIP_RechargeCardRequest_To_RechargeCardRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://INTSTDK008.Card.Ehandel.Schemas.RechargeCardRequest"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/RechargeCardRequest/20150310"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:RechargeCardRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:RechargeCardRequest"">
    <ns0:RechargeCardRequest>
      <xsl:if test=""CardNumber"">
        <CardNumber>
          <xsl:value-of select=""CardNumber/text()"" />
        </CardNumber>
      </xsl:if>
    </ns0:RechargeCardRequest>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.RechargeCardRequest";
        
        private const global::INTSTDK008.Card.STIP.Schemas.RechargeCardRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.RechargeCardRequest";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.RechargeCardRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.RechargeCardRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.RechargeCardRequest";
                return _TrgSchemas;
            }
        }
    }
}
