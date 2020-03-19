namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.SyncCustomerResponseJSON", typeof(global::INTSTDK008.Ehandel.Schemas.SyncCustomerResponseJSON))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.SyncCustomerResponseJSON", typeof(global::INTSTDK008.STIP.Schemas.SyncCustomerResponseJSON))]
    public sealed class EHandel_SyncCustomerResponse_To_SyncCustomerResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008/SyncCustomerJSONResponse/2015701"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.SyncCustomerResponseJSON"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:SyncFromCrmtoEPiResponseParameters"" />
  </xsl:template>
  <xsl:template match=""/s0:SyncFromCrmtoEPiResponseParameters"">
    <ns0:SyncFromCrmtoEPiResponseParameters>
      <xsl:if test=""Message"">
        <Message>
          <xsl:value-of select=""Message/text()"" />
        </Message>
      </xsl:if>
      <xsl:if test=""Success"">
        <Success>
          <xsl:value-of select=""Success/text()"" />
        </Success>
      </xsl:if>
      <xsl:if test=""ErrorMessage"">
        <ErrorMessage>
          <xsl:value-of select=""ErrorMessage/text()"" />
        </ErrorMessage>
      </xsl:if>
      <xsl:if test=""StatusCode"">
        <StatusCode>
          <xsl:value-of select=""StatusCode/text()"" />
        </StatusCode>
      </xsl:if>
    </ns0:SyncFromCrmtoEPiResponseParameters>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.SyncCustomerResponseJSON";
        
        private const global::INTSTDK008.Ehandel.Schemas.SyncCustomerResponseJSON _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.SyncCustomerResponseJSON";
        
        private const global::INTSTDK008.STIP.Schemas.SyncCustomerResponseJSON _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.SyncCustomerResponseJSON";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.SyncCustomerResponseJSON";
                return _TrgSchemas;
            }
        }
    }
}
