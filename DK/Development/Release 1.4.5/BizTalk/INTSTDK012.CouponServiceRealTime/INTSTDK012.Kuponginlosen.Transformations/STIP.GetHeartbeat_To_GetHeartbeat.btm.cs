namespace INTSTDK012.Kuponginlosen.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.STIP.Schemas.CouponService+getHeartbeat", typeof(global::INTSTDK012.STIP.Schemas.CouponService.getHeartbeat))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+getHeartbeat", typeof(global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.getHeartbeat))]
    public sealed class STIP_GetHeartbeat_To_GetHeartbeat : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK012.STIP.CouponService.Schemas/20150316"" xmlns:ns0=""http://service.web.couponcreator.kuponginlosen.se/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:getHeartbeat"" />
  </xsl:template>
  <xsl:template match=""/s0:getHeartbeat"">
    <ns0:getHeartbeat>
      <xsl:value-of select=""./text()"" />
    </ns0:getHeartbeat>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK012.STIP.Schemas.CouponService+getHeartbeat";
        
        private const global::INTSTDK012.STIP.Schemas.CouponService.getHeartbeat _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+getHeartbeat";
        
        private const global::INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService.getHeartbeat _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK012.STIP.Schemas.CouponService+getHeartbeat";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK012.Kuponginlosen.Schemas.CouponCreatorService+getHeartbeat";
                return _TrgSchemas;
            }
        }
    }
}
