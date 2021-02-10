namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeCardRequest", typeof(global::INTSTDK003.STIP.Schemas.CompositeCardRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+RegisterTravelCard", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.RegisterTravelCard))]
    public sealed class STIP_CompositeCardRequest_To_RegisterTravelCard : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s3 s0 s2 ScriptNS0"" version=""1.0"" xmlns:s2=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns3=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s3=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2_3/20141128"" xmlns:ns2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1/20141128"" xmlns:s1=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2/20141128"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:ScriptNS0=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s2:crmCardRequest"" />
  </xsl:template>
  <xsl:template match=""/s2:crmCardRequest"">
    <ns0:RegisterTravelCard>
      <ns0:travelCard>
        <ns1:AccountId>
          <xsl:value-of select=""s2:accntNr/text()"" />
        </ns1:AccountId>
        <xsl:if test=""s2:card/s2:AutoloadConnectionDate"">
          <ns1:AutoloadConnectionDate>
            <xsl:value-of select=""s2:card/s2:AutoloadConnectionDate/text()"" />
          </ns1:AutoloadConnectionDate>
        </xsl:if>
        <xsl:if test=""s2:card/s2:AutoloadDisconnectionDate"">
          <ns1:AutoloadDisconnectionDate>
            <xsl:value-of select=""s2:card/s2:AutoloadDisconnectionDate/text()"" />
          </ns1:AutoloadDisconnectionDate>
        </xsl:if>
        <xsl:if test=""s2:card/s2:AutoloadStatus"">
          <ns1:AutoloadStatus>
            <xsl:value-of select=""s2:card/s2:AutoloadStatus/text()"" />
          </ns1:AutoloadStatus>
        </xsl:if>
        <xsl:if test=""s2:card/s2:Blocked"">
          <ns1:Blocked>
            <xsl:value-of select=""s2:card/s2:Blocked/text()"" />
          </ns1:Blocked>
        </xsl:if>
        <xsl:if test=""s2:card/s2:CardCategory"">
          <ns1:CardCategory>
            <xsl:value-of select=""s2:card/s2:CardCategory/text()"" />
          </ns1:CardCategory>
        </xsl:if>
        <xsl:if test=""s2:card/s2:CardName"">
          <xsl:variable name=""var:v1"" select=""string(s2:card/s2:CardName/@xsi:nil) = 'true'"" />
          <xsl:if test=""string($var:v1)='true'"">
            <ns1:CardName>
              <xsl:attribute name=""xsi:nil"">
                <xsl:value-of select=""'true'"" />
              </xsl:attribute>
            </ns1:CardName>
          </xsl:if>
          <xsl:if test=""string($var:v1)='false'"">
            <ns1:CardName>
              <xsl:value-of select=""s2:card/s2:CardName/text()"" />
            </ns1:CardName>
          </xsl:if>
        </xsl:if>
        <xsl:variable name=""var:v2"" select=""string(s2:card/s2:CardNumber/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v2)='true'"">
          <ns1:CardNumber>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns1:CardNumber>
        </xsl:if>
        <xsl:if test=""string($var:v2)='false'"">
          <ns1:CardNumber>
            <xsl:value-of select=""s2:card/s2:CardNumber/text()"" />
          </ns1:CardNumber>
        </xsl:if>
        <xsl:if test=""s2:card/s2:CreditCardMask"">
          <xsl:variable name=""var:v3"" select=""string(s2:card/s2:CreditCardMask/@xsi:nil) = 'true'"" />
          <xsl:if test=""string($var:v3)='true'"">
            <ns1:CreditCardMask>
              <xsl:attribute name=""xsi:nil"">
                <xsl:value-of select=""'true'"" />
              </xsl:attribute>
            </ns1:CreditCardMask>
          </xsl:if>
          <xsl:if test=""string($var:v3)='false'"">
            <ns1:CreditCardMask>
              <xsl:value-of select=""s2:card/s2:CreditCardMask/text()"" />
            </ns1:CreditCardMask>
          </xsl:if>
        </xsl:if>
        <xsl:if test=""s2:card/s2:Currency"">
          <xsl:variable name=""var:v4"" select=""string(s2:card/s2:Currency/@xsi:nil) = 'true'"" />
          <xsl:if test=""string($var:v4)='true'"">
            <ns1:Currency>
              <xsl:attribute name=""xsi:nil"">
                <xsl:value-of select=""'true'"" />
              </xsl:attribute>
            </ns1:Currency>
          </xsl:if>
          <xsl:if test=""string($var:v4)='false'"">
            <ns1:Currency>
              <xsl:value-of select=""s2:card/s2:Currency/text()"" />
            </ns1:Currency>
          </xsl:if>
        </xsl:if>
        <ns1:CustomerType>
          <xsl:value-of select=""s2:card/s2:CustomerType/text()"" />
        </ns1:CustomerType>
        <xsl:if test=""s2:card/s2:FailedAttemptsToChargeMoney"">
          <ns1:FailedAttemptsToChargeMoney>
            <xsl:value-of select=""s2:card/s2:FailedAttemptsToChargeMoney/text()"" />
          </ns1:FailedAttemptsToChargeMoney>
        </xsl:if>
        <xsl:if test=""s2:card/s2:LatestAutoloadAmount"">
          <ns1:LatestAutoloadAmount>
            <xsl:value-of select=""s2:card/s2:LatestAutoloadAmount/text()"" />
          </ns1:LatestAutoloadAmount>
        </xsl:if>
        <xsl:if test=""s2:card/s2:LatestChargeDate"">
          <ns1:LatestChargeDate>
            <xsl:value-of select=""s2:card/s2:LatestChargeDate/text()"" />
          </ns1:LatestChargeDate>
        </xsl:if>
        <xsl:if test=""s2:card/s2:LatestFailedAttempt"">
          <ns1:LatestFailedAttempt>
            <xsl:value-of select=""s2:card/s2:LatestFailedAttempt/text()"" />
          </ns1:LatestFailedAttempt>
        </xsl:if>
        <xsl:if test=""s2:card/s2:PeriodCardTypeId"">
          <ns1:PeriodCardTypeId>
            <xsl:value-of select=""s2:card/s2:PeriodCardTypeId/text()"" />
          </ns1:PeriodCardTypeId>
        </xsl:if>
        <xsl:if test=""s2:card/s2:PeriodCardTypeTitle"">
          <xsl:variable name=""var:v5"" select=""string(s2:card/s2:PeriodCardTypeTitle/@xsi:nil) = 'true'"" />
          <xsl:if test=""string($var:v5)='true'"">
            <ns1:PeriodCardTypeTitle>
              <xsl:attribute name=""xsi:nil"">
                <xsl:value-of select=""'true'"" />
              </xsl:attribute>
            </ns1:PeriodCardTypeTitle>
          </xsl:if>
          <xsl:if test=""string($var:v5)='false'"">
            <ns1:PeriodCardTypeTitle>
              <xsl:value-of select=""s2:card/s2:PeriodCardTypeTitle/text()"" />
            </ns1:PeriodCardTypeTitle>
          </xsl:if>
        </xsl:if>
        <xsl:if test=""s2:card/s2:PeriodValidFrom"">
          <ns1:PeriodValidFrom>
            <xsl:value-of select=""s2:card/s2:PeriodValidFrom/text()"" />
          </ns1:PeriodValidFrom>
        </xsl:if>
        <xsl:if test=""s2:card/s2:PeriodValidTo"">
          <ns1:PeriodValidTo>
            <xsl:value-of select=""s2:card/s2:PeriodValidTo/text()"" />
          </ns1:PeriodValidTo>
        </xsl:if>
        <xsl:if test=""s2:card/s2:ValueCardTypeId"">
          <ns1:ValueCardTypeId>
            <xsl:value-of select=""s2:card/s2:ValueCardTypeId/text()"" />
          </ns1:ValueCardTypeId>
        </xsl:if>
        <xsl:if test=""s2:card/s2:ValueCardTypeTitle"">
          <xsl:variable name=""var:v6"" select=""string(s2:card/s2:ValueCardTypeTitle/@xsi:nil) = 'true'"" />
          <xsl:if test=""string($var:v6)='true'"">
            <ns1:ValueCardTypeTitle>
              <xsl:attribute name=""xsi:nil"">
                <xsl:value-of select=""'true'"" />
              </xsl:attribute>
            </ns1:ValueCardTypeTitle>
          </xsl:if>
          <xsl:if test=""string($var:v6)='false'"">
            <ns1:ValueCardTypeTitle>
              <xsl:value-of select=""s2:card/s2:ValueCardTypeTitle/text()"" />
            </ns1:ValueCardTypeTitle>
          </xsl:if>
        </xsl:if>
        <xsl:if test=""s2:card/s2:VerifyId"">
          <xsl:variable name=""var:v7"" select=""string(s2:card/s2:VerifyId/@xsi:nil) = 'true'"" />
          <xsl:if test=""string($var:v7)='true'"">
            <ns1:VerifyId>
              <xsl:attribute name=""xsi:nil"">
                <xsl:value-of select=""'true'"" />
              </xsl:attribute>
            </ns1:VerifyId>
          </xsl:if>
          <xsl:if test=""string($var:v7)='false'"">
            <ns1:VerifyId>
              <xsl:value-of select=""s2:card/s2:VerifyId/text()"" />
            </ns1:VerifyId>
          </xsl:if>
        </xsl:if>
      </ns0:travelCard>
      <xsl:variable name=""var:v8"" select=""ScriptNS0:ReadSSOValue(&quot;INTSTDK003&quot; , &quot;CallerId&quot;)"" />
      <ns0:callerId>
        <xsl:value-of select=""$var:v8"" />
      </ns0:callerId>
    </ns0:RegisterTravelCard>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects>
  <ExtensionObject Namespace=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"" AssemblyName=""INTSTDK003.Helper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41"" ClassName=""INTSTDK003.Helper.INTSTDK003Helper"" />
</ExtensionObjects>";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.STIP.Schemas.CompositeCardRequest";
        
        private const global::INTSTDK003.STIP.Schemas.CompositeCardRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+RegisterTravelCard";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.RegisterTravelCard _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.STIP.Schemas.CompositeCardRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+RegisterTravelCard";
                return _TrgSchemas;
            }
        }
    }
}
