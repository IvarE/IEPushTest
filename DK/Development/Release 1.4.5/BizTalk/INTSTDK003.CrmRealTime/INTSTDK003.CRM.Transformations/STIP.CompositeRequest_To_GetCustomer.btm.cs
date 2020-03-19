namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeRequest", typeof(global::INTSTDK003.STIP.Schemas.CompositeRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCustomer", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCustomer))]
    public sealed class STIP_CompositeRequest_To_GetCustomer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 ScriptNS0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s0=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeRequest/20141215"" xmlns:ns2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns3=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ScriptNS0=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:crmCustomerRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:crmCustomerRequest"">
    <ns0:GetCustomer>
      <ns0:customerId>
        <xsl:value-of select=""s0:getAccount/s0:nbrGUID/text()"" />
      </ns0:customerId>
      <ns0:accountType>
        <xsl:value-of select=""s0:getAccount/s0:accountType/text()"" />
      </ns0:accountType>
      <xsl:variable name=""var:v1"" select=""ScriptNS0:ReadSSOValue(&quot;INTSTDK003&quot; , &quot;CallerId&quot;)"" />
      <ns0:callerId>
        <xsl:value-of select=""$var:v1"" />
      </ns0:callerId>
    </ns0:GetCustomer>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects>
  <ExtensionObject Namespace=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"" AssemblyName=""INTSTDK003.Helper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41"" ClassName=""INTSTDK003.Helper.INTSTDK003Helper"" />
</ExtensionObjects>";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.STIP.Schemas.CompositeRequest";
        
        private const global::INTSTDK003.STIP.Schemas.CompositeRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCustomer";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCustomer _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.STIP.Schemas.CompositeRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCustomer";
                return _TrgSchemas;
            }
        }
    }
}
