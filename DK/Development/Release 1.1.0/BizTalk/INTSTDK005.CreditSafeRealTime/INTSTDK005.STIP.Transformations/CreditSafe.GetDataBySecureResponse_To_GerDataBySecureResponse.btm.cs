namespace INTSTDK005.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata+GetDataBySecureResponse", typeof(global::INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata.GetDataBySecureResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecureResponse", typeof(global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecureResponse))]
    public sealed class CreditSafe_GetDataBySecureResponse_To_GerDataBySecureResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1 s2"" version=""1.0"" xmlns:s1=""https://webservice.creditsafe.se/getdata/"" xmlns:s0=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216"" xmlns:s2=""urn:schemas-microsoft-com:xml-msdata"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:GetDataBySecureResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:GetDataBySecureResponse"">
    <ns0:GetDataBySecureResponse>
      <xsl:for-each select=""s1:GetDataBySecureResult"">
        <GetDataBySecureResult>
          <xsl:if test=""s1:SearchNumber"">
            <SearchNumber>
              <xsl:value-of select=""s1:SearchNumber/text()"" />
            </SearchNumber>
          </xsl:if>
          <xsl:if test=""s1:TransactionId"">
            <TransactionId>
              <xsl:value-of select=""s1:TransactionId/text()"" />
            </TransactionId>
          </xsl:if>
          <xsl:for-each select=""s1:Error"">
            <Error>
              <xsl:if test=""s1:Cause_of_Reject"">
                <Cause_of_Reject>
                  <xsl:value-of select=""s1:Cause_of_Reject/text()"" />
                </Cause_of_Reject>
              </xsl:if>
              <xsl:if test=""s1:Reject_text"">
                <Reject_text>
                  <xsl:value-of select=""s1:Reject_text/text()"" />
                </Reject_text>
              </xsl:if>
              <xsl:if test=""s1:Reject_comment"">
                <Reject_comment>
                  <xsl:value-of select=""s1:Reject_comment/text()"" />
                </Reject_comment>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </Error>
          </xsl:for-each>
          <xsl:if test=""s1:Block_Name"">
            <Block_Name>
              <xsl:value-of select=""s1:Block_Name/text()"" />
            </Block_Name>
          </xsl:if>
          <Parameters>
            <NewDataSet>
              <xsl:for-each select=""s1:Parameters"">
                <xsl:for-each select=""s0:diffgram/NewDataSet/GETDATA_RESPONSE"">
                  <GETDATA_RESPONSE>
                    <xsl:if test=""PNR"">
                      <PNR>
                        <xsl:value-of select=""PNR/text()"" />
                      </PNR>
                    </xsl:if>
                    <xsl:if test=""FIRST_NAME"">
                      <FIRST_NAME>
                        <xsl:value-of select=""FIRST_NAME/text()"" />
                      </FIRST_NAME>
                    </xsl:if>
                    <xsl:if test=""GIVEN_NAME"">
                      <GIVEN_NAME>
                        <xsl:value-of select=""GIVEN_NAME/text()"" />
                      </GIVEN_NAME>
                    </xsl:if>
                    <xsl:if test=""LAST_NAME"">
                      <LAST_NAME>
                        <xsl:value-of select=""LAST_NAME/text()"" />
                      </LAST_NAME>
                    </xsl:if>
                    <xsl:if test=""CO_ADDRESS"">
                      <CO_ADDRESS>
                        <xsl:value-of select=""CO_ADDRESS/text()"" />
                      </CO_ADDRESS>
                    </xsl:if>
                    <xsl:if test=""REGISTERED_ADDRESS"">
                      <REGISTERED_ADDRESS>
                        <xsl:value-of select=""REGISTERED_ADDRESS/text()"" />
                      </REGISTERED_ADDRESS>
                    </xsl:if>
                    <xsl:if test=""ADDRESS"">
                      <ADDRESS>
                        <xsl:value-of select=""ADDRESS/text()"" />
                      </ADDRESS>
                    </xsl:if>
                    <xsl:if test=""ZIPCODE"">
                      <ZIPCODE>
                        <xsl:value-of select=""ZIPCODE/text()"" />
                      </ZIPCODE>
                    </xsl:if>
                    <xsl:if test=""TOWN"">
                      <TOWN>
                        <xsl:value-of select=""TOWN/text()"" />
                      </TOWN>
                    </xsl:if>
                    <xsl:if test=""SPEC_CO_ADDRESS"">
                      <SPEC_CO_ADDRESS>
                        <xsl:value-of select=""SPEC_CO_ADDRESS/text()"" />
                      </SPEC_CO_ADDRESS>
                    </xsl:if>
                    <xsl:if test=""SPEC_ADDRESS"">
                      <SPEC_ADDRESS>
                        <xsl:value-of select=""SPEC_ADDRESS/text()"" />
                      </SPEC_ADDRESS>
                    </xsl:if>
                    <xsl:if test=""SPEC_ZIPCODE"">
                      <SPEC_ZIPCODE>
                        <xsl:value-of select=""SPEC_ZIPCODE/text()"" />
                      </SPEC_ZIPCODE>
                    </xsl:if>
                    <xsl:if test=""SPEC_COUNTRY"">
                      <SPEC_COUNTRY>
                        <xsl:value-of select=""SPEC_COUNTRY/text()"" />
                      </SPEC_COUNTRY>
                    </xsl:if>
                    <xsl:if test=""SPEC_TOWN"">
                      <SPEC_TOWN>
                        <xsl:value-of select=""SPEC_TOWN/text()"" />
                      </SPEC_TOWN>
                    </xsl:if>
                    <xsl:if test=""SPEC_REGISTERED_ADDRESS"">
                      <SPEC_REGISTERED_ADDRESS>
                        <xsl:value-of select=""SPEC_REGISTERED_ADDRESS/text()"" />
                      </SPEC_REGISTERED_ADDRESS>
                    </xsl:if>
                    <xsl:if test=""SEARCH_DATE"">
                      <SEARCH_DATE>
                        <xsl:value-of select=""SEARCH_DATE/text()"" />
                      </SEARCH_DATE>
                    </xsl:if>
                    <xsl:if test=""EMIGRATED"">
                      <EMIGRATED>
                        <xsl:value-of select=""EMIGRATED/text()"" />
                      </EMIGRATED>
                    </xsl:if>
                    <xsl:if test=""EMIGRATED_DATE"">
                      <EMIGRATED_DATE>
                        <xsl:value-of select=""EMIGRATED_DATE/text()"" />
                      </EMIGRATED_DATE>
                    </xsl:if>
                    <xsl:if test=""PROTECTED"">
                      <PROTECTED>
                        <xsl:value-of select=""PROTECTED/text()"" />
                      </PROTECTED>
                    </xsl:if>
                    <xsl:if test=""GENDER"">
                      <GENDER>
                        <xsl:value-of select=""GENDER/text()"" />
                      </GENDER>
                    </xsl:if>
                    <xsl:if test=""AGE"">
                      <AGE>
                        <xsl:value-of select=""AGE/text()"" />
                      </AGE>
                    </xsl:if>
                  </GETDATA_RESPONSE>
                </xsl:for-each>
              </xsl:for-each>
            </NewDataSet>
          </Parameters>
        </GetDataBySecureResult>
      </xsl:for-each>
    </ns0:GetDataBySecureResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata+GetDataBySecureResponse";
        
        private const global::INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata.GetDataBySecureResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecureResponse";
        
        private const global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecureResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK005.CreditSafe.Schemas.GetData_webservice_creditsafe_se_getdata+GetDataBySecureResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecureResponse";
                return _TrgSchemas;
            }
        }
    }
}
