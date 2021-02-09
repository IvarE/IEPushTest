namespace INTSTDK005.CreditSafe.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure", typeof(global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecure))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata+GetDataBySecure", typeof(global::INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata.GetDataBySecure))]
    public sealed class STIP_GetDataBySecureRequest_To_GetDataBySecureRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:ns0=""https://webservice.creditsafe.se/getdata/"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216"" xmlns:ns1=""urn:schemas-microsoft-com:xml-msdata"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetDataBySecure"" />
  </xsl:template>
  <xsl:template match=""/s0:GetDataBySecure"">
    <ns0:GetDataBySecure>
      <xsl:for-each select=""GetData_Request"">
        <ns0:GetData_Request>
          <xsl:for-each select=""account"">
            <ns0:account>
              <xsl:if test=""UserName"">
                <ns0:UserName>
                  <xsl:value-of select=""UserName/text()"" />
                </ns0:UserName>
              </xsl:if>
              <xsl:if test=""Password"">
                <ns0:Password>
                  <xsl:value-of select=""Password/text()"" />
                </ns0:Password>
              </xsl:if>
              <xsl:if test=""TransactionId"">
                <ns0:TransactionId>
                  <xsl:value-of select=""TransactionId/text()"" />
                </ns0:TransactionId>
              </xsl:if>
              <ns0:Language>
                <xsl:value-of select=""Language/text()"" />
              </ns0:Language>
              <xsl:value-of select=""./text()"" />
            </ns0:account>
          </xsl:for-each>
          <xsl:if test=""Block_Name"">
            <ns0:Block_Name>
              <xsl:value-of select=""Block_Name/text()"" />
            </ns0:Block_Name>
          </xsl:if>
          <xsl:if test=""SearchNumber"">
            <ns0:SearchNumber>
              <xsl:value-of select=""SearchNumber/text()"" />
            </ns0:SearchNumber>
          </xsl:if>
          <xsl:if test=""FormattedOutput"">
            <ns0:FormattedOutput>
              <xsl:value-of select=""FormattedOutput/text()"" />
            </ns0:FormattedOutput>
          </xsl:if>
          <xsl:if test=""LODCustFreeText"">
            <ns0:LODCustFreeText>
              <xsl:value-of select=""LODCustFreeText/text()"" />
            </ns0:LODCustFreeText>
          </xsl:if>
          <xsl:if test=""Mobile"">
            <ns0:Mobile>
              <xsl:value-of select=""Mobile/text()"" />
            </ns0:Mobile>
          </xsl:if>
          <xsl:if test=""Email"">
            <ns0:Email>
              <xsl:value-of select=""Email/text()"" />
            </ns0:Email>
          </xsl:if>
          <xsl:value-of select=""./text()"" />
        </ns0:GetData_Request>
      </xsl:for-each>
    </ns0:GetDataBySecure>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure";
        
        private const global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecure _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata+GetDataBySecure";
        
        private const global::INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata.GetDataBySecure _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata+GetDataBySecure";
                return _TrgSchemas;
            }
        }
    }
}
