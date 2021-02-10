namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeCardRequest", typeof(global::INTSTDK003.STIP.Schemas.CompositeCardRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCardsForCustomer", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCardsForCustomer))]
    public sealed class STIP_CompositeCardRequest_To_GetCardsForCustomer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s3 s0 s2 ScriptNS0"" version=""1.0"" xmlns:s2=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeCardRequest/20141215"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns3=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s3=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2_3/20141128"" xmlns:ns2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1/20141128"" xmlns:s1=""http://www.skanetrafiken.com/DK/INTSTDK003/PortalService_1_2/20141128"" xmlns:ScriptNS0=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s2:crmCardRequest"" />
  </xsl:template>
  <xsl:template match=""/s2:crmCardRequest"">
    <ns0:GetCardsForCustomer>
      <ns0:customerId>
        <xsl:value-of select=""s2:accntNr/text()"" />
      </ns0:customerId>
      <ns0:customerType>
        <xsl:value-of select=""s2:card/s2:CustomerType/text()"" />
      </ns0:customerType>
      <xsl:variable name=""var:v1"" select=""ScriptNS0:ReadSSOValue(&quot;INTSTDK003&quot; , &quot;CallerId&quot;)"" />
      <ns0:callerId>
        <xsl:value-of select=""$var:v1"" />
      </ns0:callerId>
    </ns0:GetCardsForCustomer>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects>
  <ExtensionObject Namespace=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"" AssemblyName=""INTSTDK003.Helper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41"" ClassName=""INTSTDK003.Helper.INTSTDK003Helper"" />
</ExtensionObjects>";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.STIP.Schemas.CompositeCardRequest";
        
        private const global::INTSTDK003.STIP.Schemas.CompositeCardRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCardsForCustomer";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCardsForCustomer _trgSchemaTypeReference0 = null;
        
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
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCardsForCustomer";
                return _TrgSchemas;
            }
        }
    }
}
