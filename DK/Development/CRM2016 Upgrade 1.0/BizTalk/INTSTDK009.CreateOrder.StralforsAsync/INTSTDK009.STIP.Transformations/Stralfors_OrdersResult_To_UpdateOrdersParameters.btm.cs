namespace INTSTDK009.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.Stralfors.Schemas.SingelOrderResult", typeof(global::INTSTDK009.Stralfors.Schemas.SingelOrderResult))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.STIP.Schemas.UpdateOrdersParametersJSON", typeof(global::INTSTDK009.STIP.Schemas.UpdateOrdersParametersJSON))]
    public sealed class Stralfors_OrdersResult_To_UpdateOrdersParameters : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var"" version=""1.0"" xmlns:ns0=""http://INTSTDK009.STIP.Schemas.UpdateOrdersParametersJSON"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/Order"" />
  </xsl:template>
  <xsl:template match=""/Order"">
    <ns0:UpdateOrderParameters>
      <xsl:for-each select=""Products"">
        <xsl:for-each select=""Product"">
          <Cards>
            <xsl:if test=""NameOnCard"">
              <CardName>
                <xsl:value-of select=""NameOnCard/text()"" />
              </CardName>
            </xsl:if>
            <xsl:if test=""Serial"">
              <CardNumber>
                <xsl:value-of select=""Serial/text()"" />
              </CardNumber>
            </xsl:if>
            <xsl:if test=""Reference"">
              <Reference>
                <xsl:value-of select=""Reference/text()"" />
              </Reference>
            </xsl:if>
          </Cards>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:UpdateOrderParameters>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK009.Stralfors.Schemas.SingelOrderResult";
        
        private const global::INTSTDK009.Stralfors.Schemas.SingelOrderResult _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK009.STIP.Schemas.UpdateOrdersParametersJSON";
        
        private const global::INTSTDK009.STIP.Schemas.UpdateOrdersParametersJSON _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK009.Stralfors.Schemas.SingelOrderResult";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK009.STIP.Schemas.UpdateOrdersParametersJSON";
                return _TrgSchemas;
            }
        }
    }
}
