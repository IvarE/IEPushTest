namespace INTSTDK004.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.STIP.Schemas.GetAutoloadActivityReportResponseType", typeof(global::INTSTDK004.STIP.Schemas.GetAutoloadActivityReportResponseType))]
    public sealed class Biff_GetAutoloadActivityReportResponse_To_GetAutoloadActivityReportResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s2 s0 s1"" version=""1.0"" xmlns:s3=""http://cubic.com"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadActivityReportResponse/20141216"" xmlns:s2=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:s0=""urn:schemas-microsoft-com:xml-msdata"" xmlns:s1=""http://BIFF.org/AutoloadActivity.xsd"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetAutoloadActivityReportResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:GetAutoloadActivityReportResponse"">
    <ns0:GetAutoloadActivityReportResponse>
      <ns0:GetAutoloadActivityReportResult>
        <ns0:AutoloadActivity>
          <xsl:for-each select=""s3:GetAutoloadActivityReportResult"">
            <xsl:for-each select=""s2:diffgram/s1:AutoloadActivity/s1:Activity"">
              <ns0:Activity>
                <xsl:if test=""s1:CardSerialNumber"">
                  <ns0:CardSerialNumber>
                    <xsl:value-of select=""s1:CardSerialNumber/text()"" />
                  </ns0:CardSerialNumber>
                </xsl:if>
                <xsl:if test=""s1:RequestID"">
                  <ns0:RequestID>
                    <xsl:value-of select=""s1:RequestID/text()"" />
                  </ns0:RequestID>
                </xsl:if>
                <xsl:if test=""s1:CardSection"">
                  <ns0:CardSection>
                    <xsl:value-of select=""s1:CardSection/text()"" />
                  </ns0:CardSection>
                </xsl:if>
                <xsl:if test=""s1:Currency"">
                  <ns0:Currency>
                    <xsl:value-of select=""s1:Currency/text()"" />
                  </ns0:Currency>
                </xsl:if>
                <xsl:if test=""s1:Price"">
                  <ns0:Price>
                    <xsl:value-of select=""s1:Price/text()"" />
                  </ns0:Price>
                </xsl:if>
                <xsl:if test=""s1:StatusCode"">
                  <ns0:StatusCode>
                    <xsl:value-of select=""s1:StatusCode/text()"" />
                  </ns0:StatusCode>
                </xsl:if>
                <xsl:if test=""s1:DateLoaded"">
                  <ns0:DateLoaded>
                    <xsl:value-of select=""s1:DateLoaded/text()"" />
                  </ns0:DateLoaded>
                </xsl:if>
              </ns0:Activity>
            </xsl:for-each>
          </xsl:for-each>
        </ns0:AutoloadActivity>
      </ns0:GetAutoloadActivityReportResult>
    </ns0:GetAutoloadActivityReportResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse";
        
        private const global::INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK004.STIP.Schemas.GetAutoloadActivityReportResponseType";
        
        private const global::INTSTDK004.STIP.Schemas.GetAutoloadActivityReportResponseType _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK004.Biff.Schemas.External.GetAutoloadActivityReportResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK004.STIP.Schemas.GetAutoloadActivityReportResponseType";
                return _TrgSchemas;
            }
        }
    }
}
