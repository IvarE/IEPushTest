namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCardsForCustomerResponse", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCardsForCustomerResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeCardResponse", typeof(global::INTSTDK003.STIP.Schemas.CompositeCardResponse))]
    public sealed class CRM_GetCardsForCustomerResponse_To_CompositeCardResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s2 s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardResponse/20141215"" xmlns:s3=""http://tempuri.org/"" xmlns:s0=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:ns2=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2_3/20141128"" xmlns:s2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns3=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1/20141128"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2/20141128"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetCardsForCustomerResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:GetCardsForCustomerResponse"">
    <ns0:crmCardResponse>
      <ns0:cards>
        <xsl:for-each select=""s3:GetCardsForCustomerResult"">
          <xsl:for-each select=""s2:TravelCards"">
            <xsl:for-each select=""s1:TravelCard"">
              <ns0:card>
                <xsl:if test=""s1:AutoloadConnectionDate"">
                  <ns0:AutoloadConnectionDate>
                    <xsl:value-of select=""s1:AutoloadConnectionDate/text()"" />
                  </ns0:AutoloadConnectionDate>
                </xsl:if>
                <xsl:if test=""s1:AutoloadDisconnectionDate"">
                  <ns0:AutoloadDisconnectionDate>
                    <xsl:value-of select=""s1:AutoloadDisconnectionDate/text()"" />
                  </ns0:AutoloadDisconnectionDate>
                </xsl:if>
                <xsl:if test=""s1:AutoloadStatus"">
                  <ns0:AutoloadStatus>
                    <xsl:value-of select=""s1:AutoloadStatus/text()"" />
                  </ns0:AutoloadStatus>
                </xsl:if>
                <xsl:if test=""s1:Blocked"">
                  <ns0:Blocked>
                    <xsl:value-of select=""s1:Blocked/text()"" />
                  </ns0:Blocked>
                </xsl:if>
                <xsl:if test=""s1:CardCategory"">
                  <ns0:CardCategory>
                    <xsl:value-of select=""s1:CardCategory/text()"" />
                  </ns0:CardCategory>
                </xsl:if>
                <xsl:if test=""s1:CardName"">
                  <xsl:variable name=""var:v1"" select=""string(s1:CardName/@xsi:nil) = 'true'"" />
                  <xsl:if test=""string($var:v1)='true'"">
                    <ns0:CardName>
                      <xsl:attribute name=""xsi:nil"">
                        <xsl:value-of select=""'true'"" />
                      </xsl:attribute>
                    </ns0:CardName>
                  </xsl:if>
                  <xsl:if test=""string($var:v1)='false'"">
                    <ns0:CardName>
                      <xsl:value-of select=""s1:CardName/text()"" />
                    </ns0:CardName>
                  </xsl:if>
                </xsl:if>
                <xsl:variable name=""var:v2"" select=""string(s1:CardNumber/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v2)='true'"">
                  <ns0:CardNumber>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns0:CardNumber>
                </xsl:if>
                <xsl:if test=""string($var:v2)='false'"">
                  <ns0:CardNumber>
                    <xsl:value-of select=""s1:CardNumber/text()"" />
                  </ns0:CardNumber>
                </xsl:if>
                <xsl:if test=""s1:CreditCardMask"">
                  <xsl:variable name=""var:v3"" select=""string(s1:CreditCardMask/@xsi:nil) = 'true'"" />
                  <xsl:if test=""string($var:v3)='true'"">
                    <ns0:CreditCardMask>
                      <xsl:attribute name=""xsi:nil"">
                        <xsl:value-of select=""'true'"" />
                      </xsl:attribute>
                    </ns0:CreditCardMask>
                  </xsl:if>
                  <xsl:if test=""string($var:v3)='false'"">
                    <ns0:CreditCardMask>
                      <xsl:value-of select=""s1:CreditCardMask/text()"" />
                    </ns0:CreditCardMask>
                  </xsl:if>
                </xsl:if>
                <xsl:if test=""s1:Currency"">
                  <xsl:variable name=""var:v4"" select=""string(s1:Currency/@xsi:nil) = 'true'"" />
                  <xsl:if test=""string($var:v4)='true'"">
                    <ns0:Currency>
                      <xsl:attribute name=""xsi:nil"">
                        <xsl:value-of select=""'true'"" />
                      </xsl:attribute>
                    </ns0:Currency>
                  </xsl:if>
                  <xsl:if test=""string($var:v4)='false'"">
                    <ns0:Currency>
                      <xsl:value-of select=""s1:Currency/text()"" />
                    </ns0:Currency>
                  </xsl:if>
                </xsl:if>
                <ns0:CustomerType>
                  <xsl:value-of select=""s1:CustomerType/text()"" />
                </ns0:CustomerType>
                <xsl:if test=""s1:FailedAttemptsToChargeMoney"">
                  <ns0:FailedAttemptsToChargeMoney>
                    <xsl:value-of select=""s1:FailedAttemptsToChargeMoney/text()"" />
                  </ns0:FailedAttemptsToChargeMoney>
                </xsl:if>
                <xsl:if test=""s1:LatestAutoloadAmount"">
                  <ns0:LatestAutoloadAmount>
                    <xsl:value-of select=""s1:LatestAutoloadAmount/text()"" />
                  </ns0:LatestAutoloadAmount>
                </xsl:if>
                <xsl:if test=""s1:LatestChargeDate"">
                  <ns0:LatestChargeDate>
                    <xsl:value-of select=""s1:LatestChargeDate/text()"" />
                  </ns0:LatestChargeDate>
                </xsl:if>
                <xsl:if test=""s1:LatestFailedAttempt"">
                  <ns0:LatestFailedAttempt>
                    <xsl:value-of select=""s1:LatestFailedAttempt/text()"" />
                  </ns0:LatestFailedAttempt>
                </xsl:if>
                <xsl:if test=""s1:PeriodCardTypeId"">
                  <ns0:PeriodCardTypeId>
                    <xsl:value-of select=""s1:PeriodCardTypeId/text()"" />
                  </ns0:PeriodCardTypeId>
                </xsl:if>
                <xsl:if test=""s1:PeriodCardTypeTitle"">
                  <xsl:variable name=""var:v5"" select=""string(s1:PeriodCardTypeTitle/@xsi:nil) = 'true'"" />
                  <xsl:if test=""string($var:v5)='true'"">
                    <ns0:PeriodCardTypeTitle>
                      <xsl:attribute name=""xsi:nil"">
                        <xsl:value-of select=""'true'"" />
                      </xsl:attribute>
                    </ns0:PeriodCardTypeTitle>
                  </xsl:if>
                  <xsl:if test=""string($var:v5)='false'"">
                    <ns0:PeriodCardTypeTitle>
                      <xsl:value-of select=""s1:PeriodCardTypeTitle/text()"" />
                    </ns0:PeriodCardTypeTitle>
                  </xsl:if>
                </xsl:if>
                <xsl:if test=""s1:PeriodValidFrom"">
                  <ns0:PeriodValidFrom>
                    <xsl:value-of select=""s1:PeriodValidFrom/text()"" />
                  </ns0:PeriodValidFrom>
                </xsl:if>
                <xsl:if test=""s1:PeriodValidTo"">
                  <ns0:PeriodValidTo>
                    <xsl:value-of select=""s1:PeriodValidTo/text()"" />
                  </ns0:PeriodValidTo>
                </xsl:if>
                <xsl:if test=""s1:ValueCardTypeId"">
                  <ns0:ValueCardTypeId>
                    <xsl:value-of select=""s1:ValueCardTypeId/text()"" />
                  </ns0:ValueCardTypeId>
                </xsl:if>
                <xsl:if test=""s1:ValueCardTypeTitle"">
                  <xsl:variable name=""var:v6"" select=""string(s1:ValueCardTypeTitle/@xsi:nil) = 'true'"" />
                  <xsl:if test=""string($var:v6)='true'"">
                    <ns0:ValueCardTypeTitle>
                      <xsl:attribute name=""xsi:nil"">
                        <xsl:value-of select=""'true'"" />
                      </xsl:attribute>
                    </ns0:ValueCardTypeTitle>
                  </xsl:if>
                  <xsl:if test=""string($var:v6)='false'"">
                    <ns0:ValueCardTypeTitle>
                      <xsl:value-of select=""s1:ValueCardTypeTitle/text()"" />
                    </ns0:ValueCardTypeTitle>
                  </xsl:if>
                </xsl:if>
                <xsl:if test=""s1:VerifyId"">
                  <xsl:variable name=""var:v7"" select=""string(s1:VerifyId/@xsi:nil) = 'true'"" />
                  <xsl:if test=""string($var:v7)='true'"">
                    <ns0:VerifyId>
                      <xsl:attribute name=""xsi:nil"">
                        <xsl:value-of select=""'true'"" />
                      </xsl:attribute>
                    </ns0:VerifyId>
                  </xsl:if>
                  <xsl:if test=""string($var:v7)='false'"">
                    <ns0:VerifyId>
                      <xsl:value-of select=""s1:VerifyId/text()"" />
                    </ns0:VerifyId>
                  </xsl:if>
                </xsl:if>
              </ns0:card>
            </xsl:for-each>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:cards>
      <xsl:for-each select=""s3:GetCardsForCustomerResult"">
        <ns0:status>
          <xsl:if test=""s2:Status"">
            <ns0:status>
              <xsl:value-of select=""s2:Status/text()"" />
            </ns0:status>
          </xsl:if>
          <xsl:if test=""s2:Message"">
            <ns0:message>
              <xsl:value-of select=""s2:Message/text()"" />
            </ns0:message>
          </xsl:if>
        </ns0:status>
      </xsl:for-each>
    </ns0:crmCardResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCardsForCustomerResponse";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCardsForCustomerResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.STIP.Schemas.CompositeCardResponse";
        
        private const global::INTSTDK003.STIP.Schemas.CompositeCardResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCardsForCustomerResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.STIP.Schemas.CompositeCardResponse";
                return _TrgSchemas;
            }
        }
    }
}
