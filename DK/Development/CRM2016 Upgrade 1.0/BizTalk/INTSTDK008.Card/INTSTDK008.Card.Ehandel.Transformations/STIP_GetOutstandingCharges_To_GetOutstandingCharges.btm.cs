namespace INTSTDK008.Card.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.GetOutstandingCharges+OutstandingChargesRequest", typeof(global::INTSTDK008.Card.STIP.Schemas.GetOutstandingCharges.OutstandingChargesRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges+OutstandingChargesRequest", typeof(global::INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges.OutstandingChargesRequest))]
    public sealed class STIP_GetOutstandingCharges_To_GetOutstandingCharges : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310"" xmlns:ns0=""http://INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges"">
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
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.GetOutstandingCharges+OutstandingChargesRequest";
        
        private const global::INTSTDK008.Card.STIP.Schemas.GetOutstandingCharges.OutstandingChargesRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges+OutstandingChargesRequest";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges.OutstandingChargesRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.GetOutstandingCharges+OutstandingChargesRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.GetOutstandingCharges+OutstandingChargesRequest";
                return _TrgSchemas;
            }
        }
    }
}
