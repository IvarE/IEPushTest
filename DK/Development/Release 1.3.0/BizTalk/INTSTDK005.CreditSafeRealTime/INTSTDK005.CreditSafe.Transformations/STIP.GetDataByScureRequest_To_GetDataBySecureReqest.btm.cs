namespace INTSTDK005.CreditSafe.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure", typeof(global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecure))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure", typeof(global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecure))]
    public sealed class STIP_GetDataByScureRequest_To_GetDataBySecureReqest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK005/GetDataSecure/20141216"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/ns0:GetDataBySecure"" />
  </xsl:template>
  <xsl:template match=""/ns0:GetDataBySecure"">
    <ns0:GetDataBySecure>
      <xsl:for-each select=""GetData_Request"">
        <GetData_Request>
          <xsl:for-each select=""account"">
            <account>
              <xsl:if test=""UserName"">
                <UserName>
                  <xsl:value-of select=""UserName/text()"" />
                </UserName>
              </xsl:if>
              <xsl:if test=""Password"">
                <Password>
                  <xsl:value-of select=""Password/text()"" />
                </Password>
              </xsl:if>
              <xsl:if test=""TransactionId"">
                <TransactionId>
                  <xsl:value-of select=""TransactionId/text()"" />
                </TransactionId>
              </xsl:if>
              <Language>
                <xsl:value-of select=""Language/text()"" />
              </Language>
              <xsl:value-of select=""./text()"" />
            </account>
          </xsl:for-each>
          <xsl:if test=""Block_Name"">
            <Block_Name>
              <xsl:value-of select=""Block_Name/text()"" />
            </Block_Name>
          </xsl:if>
          <xsl:if test=""SearchNumber"">
            <SearchNumber>
              <xsl:value-of select=""SearchNumber/text()"" />
            </SearchNumber>
          </xsl:if>
          <xsl:if test=""FormattedOutput"">
            <FormattedOutput>
              <xsl:value-of select=""FormattedOutput/text()"" />
            </FormattedOutput>
          </xsl:if>
          <xsl:if test=""LODCustFreeText"">
            <LODCustFreeText>
              <xsl:value-of select=""LODCustFreeText/text()"" />
            </LODCustFreeText>
          </xsl:if>
          <xsl:if test=""Mobile"">
            <Mobile>
              <xsl:value-of select=""Mobile/text()"" />
            </Mobile>
          </xsl:if>
          <xsl:if test=""Email"">
            <Email>
              <xsl:value-of select=""Email/text()"" />
            </Email>
          </xsl:if>
          <xsl:value-of select=""./text()"" />
        </GetData_Request>
      </xsl:for-each>
    </ns0:GetDataBySecure>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure";
        
        private const global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecure _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure";
        
        private const global::INTSTDK005.STIP.Schemas.Internal.GetDataSecure.GetDataBySecure _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"INTSTDK005.STIP.Schemas.Internal.GetDataSecure+GetDataBySecure";
                return _TrgSchemas;
            }
        }
    }
}
