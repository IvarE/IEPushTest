namespace INTSTDK005.CreditSafe.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.Internal.GetCasCompany+CasCompanyService", typeof(global::INTSTDK005.STIP.Schemas.Internal.GetCasCompany.CasCompanyService))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS+CasCompanyService", typeof(global::INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS.CasCompanyService))]
    public sealed class STIP_GetCasCompanyRequest_To_GetCasCompanyRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216"" xmlns:ns0=""https://webservice.creditsafe.se/CAS/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CasCompanyService"" />
  </xsl:template>
  <xsl:template match=""/s0:CasCompanyService"">
    <ns0:CasCompanyService>
      <xsl:for-each select=""s0:cas_company"">
        <ns0:cas_company>
          <xsl:for-each select=""s0:account"">
            <ns0:account>
              <xsl:if test=""s0:UserName"">
                <ns0:UserName>
                  <xsl:value-of select=""s0:UserName/text()"" />
                </ns0:UserName>
              </xsl:if>
              <xsl:if test=""s0:Password"">
                <ns0:Password>
                  <xsl:value-of select=""s0:Password/text()"" />
                </ns0:Password>
              </xsl:if>
              <xsl:if test=""s0:TransactionId"">
                <ns0:TransactionId>
                  <xsl:value-of select=""s0:TransactionId/text()"" />
                </ns0:TransactionId>
              </xsl:if>
              <ns0:Language>
                <xsl:value-of select=""s0:Language/text()"" />
              </ns0:Language>
              <xsl:value-of select=""./text()"" />
            </ns0:account>
          </xsl:for-each>
          <xsl:if test=""s0:SearchNumber"">
            <ns0:SearchNumber>
              <xsl:value-of select=""s0:SearchNumber/text()"" />
            </ns0:SearchNumber>
          </xsl:if>
          <xsl:if test=""s0:Name"">
            <ns0:Name>
              <xsl:value-of select=""s0:Name/text()"" />
            </ns0:Name>
          </xsl:if>
          <xsl:if test=""s0:Address1"">
            <ns0:Address1>
              <xsl:value-of select=""s0:Address1/text()"" />
            </ns0:Address1>
          </xsl:if>
          <xsl:if test=""s0:ZIP"">
            <ns0:ZIP>
              <xsl:value-of select=""s0:ZIP/text()"" />
            </ns0:ZIP>
          </xsl:if>
          <xsl:if test=""s0:Town"">
            <ns0:Town>
              <xsl:value-of select=""s0:Town/text()"" />
            </ns0:Town>
          </xsl:if>
          <xsl:if test=""s0:Templates"">
            <ns0:Templates>
              <xsl:value-of select=""s0:Templates/text()"" />
            </ns0:Templates>
          </xsl:if>
          <xsl:if test=""s0:LODCustFreeText"">
            <ns0:LODCustFreeText>
              <xsl:value-of select=""s0:LODCustFreeText/text()"" />
            </ns0:LODCustFreeText>
          </xsl:if>
          <xsl:if test=""s0:Mobile"">
            <ns0:Mobile>
              <xsl:value-of select=""s0:Mobile/text()"" />
            </ns0:Mobile>
          </xsl:if>
          <xsl:if test=""s0:Email"">
            <ns0:Email>
              <xsl:value-of select=""s0:Email/text()"" />
            </ns0:Email>
          </xsl:if>
          <xsl:value-of select=""./text()"" />
        </ns0:cas_company>
      </xsl:for-each>
    </ns0:CasCompanyService>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK005.STIP.Schemas.Internal.GetCasCompany+CasCompanyService";
        
        private const global::INTSTDK005.STIP.Schemas.Internal.GetCasCompany.CasCompanyService _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS+CasCompanyService";
        
        private const global::INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS.CasCompanyService _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK005.STIP.Schemas.Internal.GetCasCompany+CasCompanyService";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS+CasCompanyService";
                return _TrgSchemas;
            }
        }
    }
}
