namespace INTSTDK004.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.RequestProductBlockType+RequestProductBlockResponse", typeof(global::INTSTDK004.Biff.Schemas.External.RequestProductBlockType.RequestProductBlockResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.STIP.Schemas.RequestProductBlockType+RequestProductBlockResponse", typeof(global::INTSTDK004.STIP.Schemas.RequestProductBlockType.RequestProductBlockResponse))]
    public sealed class Biff_RequestProductBlockResponse_To_RequestProductBlockResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://cubic.com"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK004/RequestProductBlock/20141216"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:RequestProductBlockResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:RequestProductBlockResponse"">
    <ns0:RequestProductBlockResponse>
      <RequestProductBlockResult>
        <xsl:value-of select=""s0:RequestProductBlockResult/text()"" />
      </RequestProductBlockResult>
      <xsl:value-of select=""./text()"" />
    </ns0:RequestProductBlockResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK004.Biff.Schemas.External.RequestProductBlockType+RequestProductBlockResponse";
        
        private const global::INTSTDK004.Biff.Schemas.External.RequestProductBlockType.RequestProductBlockResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK004.STIP.Schemas.RequestProductBlockType+RequestProductBlockResponse";
        
        private const global::INTSTDK004.STIP.Schemas.RequestProductBlockType.RequestProductBlockResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK004.Biff.Schemas.External.RequestProductBlockType+RequestProductBlockResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK004.STIP.Schemas.RequestProductBlockType+RequestProductBlockResponse";
                return _TrgSchemas;
            }
        }
    }
}
