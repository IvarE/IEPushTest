namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest", typeof(global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.External.PortalService+RegisterTravelCard", typeof(global::INTSTDK003.CRM.Schemas.External.PortalService.RegisterTravelCard))]
    public sealed class CRM_CompositeCardRequest_To_CreateCardRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns4=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s0=""http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime"" xmlns:ns3=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns2=""http://schemas.microsoft.com/xrm/2011/Contracts"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:crmCardRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:crmCardRequest"">
    <ns0:RegisterTravelCard>
      <xsl:for-each select=""s0:card"">
        <ns0:travelCard>
          <ns1:Account>
            <ns2:Id>
              <xsl:value-of select=""../s0:accntNr/text()"" />
            </ns2:Id>
          </ns1:Account>
          <xsl:if test=""s0:blocked"">
            <ns1:Blocked>
              <xsl:value-of select=""s0:blocked/text()"" />
            </ns1:Blocked>
          </xsl:if>
          <xsl:if test=""s0:cardName"">
            <ns1:CardName>
              <xsl:value-of select=""s0:cardName/text()"" />
            </ns1:CardName>
          </xsl:if>
          <ns1:CardNumber>
            <xsl:value-of select=""s0:cardNbr/text()"" />
          </ns1:CardNumber>
          <xsl:if test=""s0:periodCardTypeTitle"">
            <ns1:PeriodCardTypeTitle>
              <xsl:value-of select=""s0:periodCardTypeTitle/text()"" />
            </ns1:PeriodCardTypeTitle>
          </xsl:if>
          <xsl:if test=""s0:perValidFromDate"">
            <ns1:PeriodValidFrom>
              <xsl:value-of select=""s0:perValidFromDate/text()"" />
            </ns1:PeriodValidFrom>
          </xsl:if>
          <xsl:if test=""s0:perValidToDate"">
            <ns1:PeriodValidTo>
              <xsl:value-of select=""s0:perValidToDate/text()"" />
            </ns1:PeriodValidTo>
          </xsl:if>
          <xsl:if test=""s0:valCardType"">
            <ns1:ValueCardTypeTitle>
              <xsl:value-of select=""s0:valCardType/text()"" />
            </ns1:ValueCardTypeTitle>
          </xsl:if>
        </ns0:travelCard>
      </xsl:for-each>
    </ns0:RegisterTravelCard>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest";
        
        private const global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.External.PortalService+RegisterTravelCard";
        
        private const global::INTSTDK003.CRM.Schemas.External.PortalService.RegisterTravelCard _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.External.PortalService+RegisterTravelCard";
                return _TrgSchemas;
            }
        }
    }
}
