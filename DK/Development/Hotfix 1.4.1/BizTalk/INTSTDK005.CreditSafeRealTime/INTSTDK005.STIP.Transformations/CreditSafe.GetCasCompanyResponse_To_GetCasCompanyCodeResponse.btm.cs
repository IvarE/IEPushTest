namespace INTSTDK005.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS+CasCompanyServiceResponse", typeof(global::INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS.CasCompanyServiceResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.Internal.GetCasCompany+CasCompanyServiceResponse", typeof(global::INTSTDK005.STIP.Schemas.Internal.GetCasCompany.CasCompanyServiceResponse))]
    public sealed class CreditSafe_GetCasCompanyResponse_To_GetCasCompanyCodeResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK005/GetCasCompany/20141216"" xmlns:s0=""https://webservice.creditsafe.se/CAS/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CasCompanyServiceResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:CasCompanyServiceResponse"">
    <ns0:CasCompanyServiceResponse>
      <xsl:for-each select=""s0:CasCompanyServiceResult"">
        <ns0:CasCompanyServiceResult>
          <xsl:if test=""s0:Org_nr"">
            <ns0:Org_nr>
              <xsl:value-of select=""s0:Org_nr/text()"" />
            </ns0:Org_nr>
          </xsl:if>
          <xsl:if test=""s0:TemplateNames"">
            <ns0:TemplateNames>
              <xsl:value-of select=""s0:TemplateNames/text()"" />
            </ns0:TemplateNames>
          </xsl:if>
          <xsl:if test=""s0:TransactionId"">
            <ns0:TransactionId>
              <xsl:value-of select=""s0:TransactionId/text()"" />
            </ns0:TransactionId>
          </xsl:if>
          <xsl:if test=""s0:Status"">
            <ns0:Status>
              <xsl:value-of select=""s0:Status/text()"" />
            </ns0:Status>
          </xsl:if>
          <xsl:if test=""s0:Status_Text"">
            <ns0:Status_Text>
              <xsl:value-of select=""s0:Status_Text/text()"" />
            </ns0:Status_Text>
          </xsl:if>
          <xsl:for-each select=""s0:ErrorList"">
            <ns0:ErrorList>
              <xsl:for-each select=""s0:ERROR"">
                <ns0:ERROR>
                  <xsl:if test=""s0:Cause_of_Reject"">
                    <ns0:Cause_of_Reject>
                      <xsl:value-of select=""s0:Cause_of_Reject/text()"" />
                    </ns0:Cause_of_Reject>
                  </xsl:if>
                  <xsl:if test=""s0:Reject_text"">
                    <ns0:Reject_text>
                      <xsl:value-of select=""s0:Reject_text/text()"" />
                    </ns0:Reject_text>
                  </xsl:if>
                  <xsl:if test=""s0:Reject_comment"">
                    <ns0:Reject_comment>
                      <xsl:value-of select=""s0:Reject_comment/text()"" />
                    </ns0:Reject_comment>
                  </xsl:if>
                  <xsl:value-of select=""./text()"" />
                </ns0:ERROR>
              </xsl:for-each>
              <xsl:value-of select=""./text()"" />
            </ns0:ErrorList>
          </xsl:for-each>
          <xsl:if test=""s0:Name"">
            <ns0:Name>
              <xsl:value-of select=""s0:Name/text()"" />
            </ns0:Name>
          </xsl:if>
          <xsl:if test=""s0:Address"">
            <ns0:Address>
              <xsl:value-of select=""s0:Address/text()"" />
            </ns0:Address>
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
          <xsl:value-of select=""./text()"" />
        </ns0:CasCompanyServiceResult>
      </xsl:for-each>
    </ns0:CasCompanyServiceResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS+CasCompanyServiceResponse";
        
        private const global::INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS.CasCompanyServiceResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK005.STIP.Schemas.Internal.GetCasCompany+CasCompanyServiceResponse";
        
        private const global::INTSTDK005.STIP.Schemas.Internal.GetCasCompany.CasCompanyServiceResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK005.CreditSafe.Schemas.Cas_Service_webservice_creditsafe_se_CAS+CasCompanyServiceResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK005.STIP.Schemas.Internal.GetCasCompany+CasCompanyServiceResponse";
                return _TrgSchemas;
            }
        }
    }
}
