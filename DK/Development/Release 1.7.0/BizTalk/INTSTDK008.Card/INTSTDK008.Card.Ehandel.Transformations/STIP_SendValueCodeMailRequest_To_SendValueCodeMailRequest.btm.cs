namespace INTSTDK008.Card.Ehandel.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.STIP.Schemas.SendValueCodeMail+SendValueCodeMailRequest", typeof(global::INTSTDK008.Card.STIP.Schemas.SendValueCodeMail.SendValueCodeMailRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailRequest", typeof(global::INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailRequest))]
    public sealed class STIP_SendValueCodeMailRequest_To_SendValueCodeMailRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailRequest"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK008.Card/SendValueCodeMail/20150310"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:SendValueCodeMailRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:SendValueCodeMailRequest"">
    <ns0:SendValueCodeMailRequest>
      <xsl:if test=""ValueCode"">
        <ValueCode>
          <xsl:value-of select=""ValueCode/text()"" />
        </ValueCode>
      </xsl:if>
      <xsl:if test=""CustomerId"">
        <CustomerId>
          <xsl:value-of select=""CustomerId/text()"" />
        </CustomerId>
      </xsl:if>
      <xsl:if test=""Email"">
        <Email>
          <xsl:value-of select=""Email/text()"" />
        </Email>
      </xsl:if>
    </ns0:SendValueCodeMailRequest>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Card.STIP.Schemas.SendValueCodeMail+SendValueCodeMailRequest";
        
        private const global::INTSTDK008.Card.STIP.Schemas.SendValueCodeMail.SendValueCodeMailRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailRequest";
        
        private const global::INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Card.STIP.Schemas.SendValueCodeMail+SendValueCodeMailRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Card.Ehandel.Schemas.SendValueCodeMailRequest";
                return _TrgSchemas;
            }
        }
    }
}
