namespace INTSTDK008.Card.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges+OutstandingChargesResponse", typeof(global::INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges.OutstandingChargesResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges+OutstandingChargesResponse", typeof(global::INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges.OutstandingChargesResponse))]
    public sealed class Ehandel_CreateOutstandingCharges_To_CreateOutstandingCharges : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges"" xmlns:s0=""http://INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:OutstandingChargesResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:OutstandingChargesResponse"">
    <ns0:OutstandingChargesResponse>
      <xsl:if test=""Message"">
        <Message>
          <xsl:value-of select=""Message/text()"" />
        </Message>
      </xsl:if>
      <xsl:if test=""HasOutstandingCharge"">
        <HasOutstandingCharge>
          <xsl:value-of select=""HasOutstandingCharge/text()"" />
        </HasOutstandingCharge>
      </xsl:if>
      <xsl:if test=""HasExpiredCharge"">
        <HasExpiredCharge>
          <xsl:value-of select=""HasExpiredCharge/text()"" />
        </HasExpiredCharge>
      </xsl:if>
      <xsl:if test=""Amount"">
        <Amount>
          <xsl:value-of select=""Amount/text()"" />
        </Amount>
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
    </ns0:OutstandingChargesResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges+OutstandingChargesResponse";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges.OutstandingChargesResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges+OutstandingChargesResponse";
        
        private const global::INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges.OutstandingChargesResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.CreateOutstandingCharges+OutstandingChargesResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.CreateOutstandingCharges+OutstandingChargesResponse";
                return _TrgSchemas;
            }
        }
    }
}
