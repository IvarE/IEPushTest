namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+RegisterTravelCardResponse", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.RegisterTravelCardResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeCardResponse", typeof(global::INTSTDK003.STIP.Schemas.CompositeCardResponse))]
    public sealed class CRM_RegisterTravelCardResponse_To_CompositeCardResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s2 s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardResponse/20141215"" xmlns:s3=""http://tempuri.org/"" xmlns:s0=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:ns2=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2_3/20141128"" xmlns:s2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns3=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1/20141128"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2/20141128"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:RegisterTravelCardResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:RegisterTravelCardResponse"">
    <ns0:crmCardResponse>
      <xsl:for-each select=""s3:RegisterTravelCardResult"">
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
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+RegisterTravelCardResponse";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.RegisterTravelCardResponse _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+RegisterTravelCardResponse";
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
