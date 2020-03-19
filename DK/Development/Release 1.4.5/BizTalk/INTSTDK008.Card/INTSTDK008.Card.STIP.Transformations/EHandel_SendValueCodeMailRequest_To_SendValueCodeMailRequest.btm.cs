namespace INTSTDK008.Card.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailResponse", typeof(global::INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.SendValueCodeMail+SendValueCodeMailResponse", typeof(global::INTSTDK008.Card.STIP.Schemas.SendValueCodeMail.SendValueCodeMailResponse))]
    public sealed class EHandel_SendValueCodeMailRequest_To_SendValueCodeMailRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailResponse"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/SendValueCodeMail/20150310"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:SendValueCodeMailResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:SendValueCodeMailResponse"">
    <ns0:SendValueCodeMailResponse>
      <xsl:if test=""Success"">
        <Success>
          <xsl:value-of select=""Success/text()"" />
        </Success>
      </xsl:if>
      <xsl:if test=""Message"">
        <Message>
          <xsl:value-of select=""Message/text()"" />
        </Message>
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
    </ns0:SendValueCodeMailResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailResponse";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.SendValueCodeMail+SendValueCodeMailResponse";
        
        private const global::INTSTDK008.Card.STIP.Schemas.SendValueCodeMail.SendValueCodeMailResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.SendValueCodeMail+SendValueCodeMailResponse";
                return _TrgSchemas;
            }
        }
    }
}
