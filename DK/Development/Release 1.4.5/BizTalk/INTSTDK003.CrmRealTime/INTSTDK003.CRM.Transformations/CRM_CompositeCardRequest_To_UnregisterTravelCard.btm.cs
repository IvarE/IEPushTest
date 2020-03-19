namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest", typeof(global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.External.PortalService+UnregisterTravelCard", typeof(global::INTSTDK003.CRM.Schemas.External.PortalService.UnregisterTravelCard))]
    public sealed class CRM_CompositeCardRequest_To_UnregisterTravelCard : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns4=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s0=""http://www.skanetrafiken.se/INSTDK003.19.CrmRealTime"" xmlns:ns3=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns2=""http://schemas.microsoft.com/xrm/2011/Contracts"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:crmCardRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:crmCardRequest"">
    <ns0:UnregisterTravelCard>
      <ns0:travelCardNumber>
        <xsl:value-of select=""s0:card/s0:cardNbr/text()"" />
      </ns0:travelCardNumber>
    </ns0:UnregisterTravelCard>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest";
        
        private const global::INTSTDK003.EHandel.Schemas.Internal.CompositeCardRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.External.PortalService+UnregisterTravelCard";
        
        private const global::INTSTDK003.CRM.Schemas.External.PortalService.UnregisterTravelCard _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.External.PortalService+UnregisterTravelCard";
                return _TrgSchemas;
            }
        }
    }
}
