namespace INTSTDK008.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.SyncCustomerJSONRequest", typeof(global::INTSTDK008.STIP.Schemas.SyncCustomerJSONRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.SyncCustomerRequestJSON", typeof(global::INTSTDK008.Ehandel.Schemas.SyncCustomerRequestJSON))]
    public sealed class STIP_SyncCustomer_To_SyncCustomerResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://INTSTDK008.Ehandel.Schemas.SyncCustomerRequestJSON"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008/SyncCustomerJSONRequest/20150701"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:SyncFromCrmtoEPiRequestParameters"" />
  </xsl:template>
  <xsl:template match=""/s0:SyncFromCrmtoEPiRequestParameters"">
    <ns0:SyncFromCrmtoEPiRequestParameters>
      <xsl:if test=""CustomerId"">
        <CustomerId>
          <xsl:value-of select=""CustomerId/text()"" />
        </CustomerId>
      </xsl:if>
    </ns0:SyncFromCrmtoEPiRequestParameters>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.STIP.Schemas.SyncCustomerJSONRequest";
        
        private const global::INTSTDK008.STIP.Schemas.SyncCustomerJSONRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Ehandel.Schemas.SyncCustomerRequestJSON";
        
        private const global::INTSTDK008.Ehandel.Schemas.SyncCustomerRequestJSON _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.STIP.Schemas.SyncCustomerJSONRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Ehandel.Schemas.SyncCustomerRequestJSON";
                return _TrgSchemas;
            }
        }
    }
}
