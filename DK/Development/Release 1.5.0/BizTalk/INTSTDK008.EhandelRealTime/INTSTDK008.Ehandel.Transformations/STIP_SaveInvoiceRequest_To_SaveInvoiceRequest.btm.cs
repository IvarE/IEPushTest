namespace INTSTDK008.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.AddInoviceJSONRequest", typeof(global::INTSTDK008.STIP.Schemas.AddInoviceJSONRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.AddInvoiceJSONRequest", typeof(global::INTSTDK008.Ehandel.Schemas.AddInvoiceJSONRequest))]
    public sealed class STIP_SaveInvoiceRequest_To_SaveInvoiceRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008/AddInoviceJSONRequest/20141110"" xmlns:ns0=""http://INTSTDK008.Ehandel.Schemas.AddInvoiceJSON"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:AddInvoiceParameters"" />
  </xsl:template>
  <xsl:template match=""/s0:AddInvoiceParameters"">
    <ns0:AddInvoiceParameters>
      <xsl:if test=""InvoiceId"">
        <InvoiceId>
          <xsl:value-of select=""InvoiceId/text()"" />
        </InvoiceId>
      </xsl:if>
      <xsl:if test=""CustomerId"">
        <CustomerId>
          <xsl:value-of select=""CustomerId/text()"" />
        </CustomerId>
      </xsl:if>
      <xsl:if test=""Date"">
        <Date>
          <xsl:value-of select=""Date/text()"" />
        </Date>
      </xsl:if>
      <xsl:if test=""TotalAmount"">
        <TotalAmount>
          <xsl:value-of select=""TotalAmount/text()"" />
        </TotalAmount>
      </xsl:if>
    </ns0:AddInvoiceParameters>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.STIP.Schemas.AddInoviceJSONRequest";
        
        private const global::INTSTDK008.STIP.Schemas.AddInoviceJSONRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Ehandel.Schemas.AddInvoiceJSONRequest";
        
        private const global::INTSTDK008.Ehandel.Schemas.AddInvoiceJSONRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.STIP.Schemas.AddInoviceJSONRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Ehandel.Schemas.AddInvoiceJSONRequest";
                return _TrgSchemas;
            }
        }
    }
}
