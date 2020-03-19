namespace INTSTDK008.Card.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges+OutstandingChargesRequest", typeof(global::INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges.OutstandingChargesRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges+OutstandingChargesRequest", typeof(global::INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges.OutstandingChargesRequest))]
    public sealed class STIP_CreateOutstandingCharges_To_CreateOutstandingCharges : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges"" xmlns:ns0=""http://INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:OutstandingChargesRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:OutstandingChargesRequest"">
    <ns0:OutstandingChargesRequest>
      <CardNumber>
        <xsl:value-of select=""CardNumber/text()"" />
      </CardNumber>
    </ns0:OutstandingChargesRequest>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges+OutstandingChargesRequest";
        
        private const global::INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges.OutstandingChargesRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges+OutstandingChargesRequest";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges.OutstandingChargesRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges+OutstandingChargesRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges+OutstandingChargesRequest";
                return _TrgSchemas;
            }
        }
    }
}
