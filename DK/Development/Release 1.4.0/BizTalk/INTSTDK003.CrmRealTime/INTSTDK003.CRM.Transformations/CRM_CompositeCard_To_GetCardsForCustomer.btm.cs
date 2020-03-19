namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse", typeof(global::INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.Internal.CompositeCardResponse", typeof(global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardResponse))]
    public sealed class CRM_CompositeCard_To_GetCardsForCustomer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s2 s0 s1"" version=""1.0"" xmlns:s2=""http://tempuri.org/"" xmlns:s0=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns0=""http://www.skanetrafiken.se/INSTDK003.22.CrmRealTime"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s2:GetCardsForCustomerResponse"" />
  </xsl:template>
  <xsl:template match=""/s2:GetCardsForCustomerResponse"">
    <ns0:crmCardResponse>
      <ns0:cards>
        <xsl:for-each select=""s2:GetCardsForCustomerResult"">
          <xsl:for-each select=""s1:TravelCards/s0:TravelCard"">
            <ns0:card>
              <xsl:if test=""s0:CardNumber"">
                <ns0:cardNbr>
                  <xsl:value-of select=""s0:CardNumber/text()"" />
                </ns0:cardNbr>
              </xsl:if>
              <xsl:if test=""s0:CardName"">
                <ns0:cardName>
                  <xsl:value-of select=""s0:CardName/text()"" />
                </ns0:cardName>
              </xsl:if>
              <xsl:if test=""s0:PeriodCardTypeTitle"">
                <ns0:periodCardTypeTitle>
                  <xsl:value-of select=""s0:PeriodCardTypeTitle/text()"" />
                </ns0:periodCardTypeTitle>
              </xsl:if>
              <xsl:if test=""s0:PeriodValidTo"">
                <ns0:perValidToDate>
                  <xsl:value-of select=""s0:PeriodValidTo/text()"" />
                </ns0:perValidToDate>
              </xsl:if>
              <xsl:if test=""s0:ValueCardTypeTitle"">
                <ns0:valCardTypeTitle>
                  <xsl:value-of select=""s0:ValueCardTypeTitle/text()"" />
                </ns0:valCardTypeTitle>
              </xsl:if>
              <xsl:if test=""s0:PeriodValidFrom"">
                <ns0:perValidFromDate>
                  <xsl:value-of select=""s0:PeriodValidFrom/text()"" />
                </ns0:perValidFromDate>
              </xsl:if>
              <xsl:if test=""s0:Blocked"">
                <ns0:blocked>
                  <xsl:value-of select=""s0:Blocked/text()"" />
                </ns0:blocked>
              </xsl:if>
            </ns0:card>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:cards>
      <xsl:for-each select=""s2:GetCardsForCustomerResult"">
        <ns0:status>
          <xsl:if test=""s1:Status"">
            <ns0:status>
              <xsl:value-of select=""s1:Status/text()"" />
            </ns0:status>
          </xsl:if>
          <xsl:if test=""s1:Message"">
            <ns0:message>
              <xsl:value-of select=""s1:Message/text()"" />
            </ns0:message>
          </xsl:if>
        </ns0:status>
      </xsl:for-each>
    </ns0:crmCardResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse";
        
        private const global::INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.EHandel.Schemas.Internal.CompositeCardResponse";
        
        private const global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.EHandel.Schemas.Internal.CompositeCardResponse";
                return _TrgSchemas;
            }
        }
    }
}
