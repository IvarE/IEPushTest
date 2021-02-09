namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.AddInoviceResponseJSON", typeof(global::INTSTDK008.Ehandel.Schemas.AddInoviceResponseJSON))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.AddInvoiceResponseJSON", typeof(global::INTSTDK008.STIP.Schemas.AddInvoiceResponseJSON))]
    public sealed class EHandel_SaveInvoiceResponse_To_SaveInvoiceResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.AddInoviceResponseJSON"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008/AddInvoiceResponseJSON/20141110"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:AddInvoiceResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:AddInvoiceResponse"">
    <ns0:AddInvoiceResponse>
      <xsl:if test=""s0:ErrorMessage"">
        <ns0:ErrorMessage>
          <xsl:value-of select=""s0:ErrorMessage/text()"" />
        </ns0:ErrorMessage>
      </xsl:if>
      <xsl:if test=""s0:StatusCode"">
        <ns0:StatusCode>
          <xsl:value-of select=""s0:StatusCode/text()"" />
        </ns0:StatusCode>
      </xsl:if>
    </ns0:AddInvoiceResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.AddInoviceResponseJSON";
        
        private const global::INTSTDK008.Ehandel.Schemas.AddInoviceResponseJSON _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.AddInvoiceResponseJSON";
        
        private const global::INTSTDK008.STIP.Schemas.AddInvoiceResponseJSON _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.AddInoviceResponseJSON";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.AddInvoiceResponseJSON";
                return _TrgSchemas;
            }
        }
    }
}
