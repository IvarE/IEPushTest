namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+CreateCustomerResponse", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.CreateCustomerResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeResponse", typeof(global::INTSTDK003.STIP.Schemas.CompositeResponse))]
    public sealed class CRM_CreateCustomerResponse_To_CompositeResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s2 s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeResponse/20141215"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s3=""http://tempuri.org/"" xmlns:s2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:s0=""http://schemas.microsoft.com/2003/10/Serialization/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:CreateCustomerResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:CreateCustomerResponse"">
    <ns0:crmCustomerResponse>
      <ns0:customer>
        <xsl:for-each select=""s3:CreateCustomerResult"">
          <ns0:accountNbr>
            <xsl:if test=""s2:AccountId"">
              <ns0:nbrGUID>
                <xsl:value-of select=""s2:AccountId/text()"" />
              </ns0:nbrGUID>
            </xsl:if>
            <xsl:if test=""s2:AccountNumber"">
              <ns0:nbr2>
                <xsl:value-of select=""s2:AccountNumber/text()"" />
              </ns0:nbr2>
            </xsl:if>
          </ns0:accountNbr>
        </xsl:for-each>
      </ns0:customer>
      <xsl:for-each select=""s3:CreateCustomerResult"">
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
    </ns0:crmCustomerResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+CreateCustomerResponse";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.CreateCustomerResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.STIP.Schemas.CompositeResponse";
        
        private const global::INTSTDK003.STIP.Schemas.CompositeResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+CreateCustomerResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.STIP.Schemas.CompositeResponse";
                return _TrgSchemas;
            }
        }
    }
}
