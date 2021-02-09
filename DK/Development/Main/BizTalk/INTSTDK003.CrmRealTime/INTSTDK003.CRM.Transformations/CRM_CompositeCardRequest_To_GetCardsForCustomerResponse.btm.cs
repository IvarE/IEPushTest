namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest", typeof(global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse", typeof(global::INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse))]
    public sealed class CRM_CompositeCardRequest_To_GetCardsForCustomerResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:app2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s0=""http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime"" xmlns:app1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:crmCardRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:crmCardRequest"">
    <ns0:GetCardsForCustomerResponse>
      <ns0:GetCardsForCustomerResult>
        <app1:TravelCards>
          <xsl:for-each select=""s0:card"">
            <app2:TravelCard>
              <xsl:if test=""s0:cardName"">
                <app2:CardName>
                  <xsl:value-of select=""s0:cardName/text()"" />
                </app2:CardName>
              </xsl:if>
              <app2:CardNumber>
                <xsl:value-of select=""s0:cardNbr/text()"" />
              </app2:CardNumber>
              <xsl:if test=""s0:periodCardTypeTitle"">
                <app2:PeriodCardTypeTitle>
                  <xsl:value-of select=""s0:periodCardTypeTitle/text()"" />
                </app2:PeriodCardTypeTitle>
              </xsl:if>
              <xsl:if test=""s0:perValidFromDate"">
                <app2:PeriodValidFrom>
                  <xsl:value-of select=""s0:perValidFromDate/text()"" />
                </app2:PeriodValidFrom>
              </xsl:if>
              <xsl:if test=""s0:perValidToDate"">
                <app2:PeriodValidTo>
                  <xsl:value-of select=""s0:perValidToDate/text()"" />
                </app2:PeriodValidTo>
              </xsl:if>
              <xsl:if test=""s0:valCardTypeTitle"">
                <app2:ValueCardTypeTitle>
                  <xsl:value-of select=""s0:valCardTypeTitle/text()"" />
                </app2:ValueCardTypeTitle>
              </xsl:if>
            </app2:TravelCard>
          </xsl:for-each>
        </app1:TravelCards>
      </ns0:GetCardsForCustomerResult>
    </ns0:GetCardsForCustomerResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest";
        
        private const global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse";
        
        private const global::INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.External.GetCardsForCustomerResponse";
                return _TrgSchemas;
            }
        }
    }
}
