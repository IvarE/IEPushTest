namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.Internal.CompositeRequest", typeof(global::INTSTDK003.EHandel.Schemas.Internal.CompositeRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.External.PortalService+GetCustomer", typeof(global::INTSTDK003.CRM.Schemas.External.PortalService.GetCustomer))]
    public sealed class CRM_CompositeRequest_To_GetCustomer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns4=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:ns3=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns2=""http://schemas.microsoft.com/xrm/2011/Contracts"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:crmCustomerRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:crmCustomerRequest"">
    <ns0:GetCustomer>
      <ns0:customerId>
        <xsl:value-of select=""s0:getAccount/s0:nbrGUID/text()"" />
      </ns0:customerId>
    </ns0:GetCustomer>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.EHandel.Schemas.Internal.CompositeRequest";
        
        private const global::INTSTDK003.EHandel.Schemas.Internal.CompositeRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.External.PortalService+GetCustomer";
        
        private const global::INTSTDK003.CRM.Schemas.External.PortalService.GetCustomer _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.EHandel.Schemas.Internal.CompositeRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.External.PortalService+GetCustomer";
                return _TrgSchemas;
            }
        }
    }
}
