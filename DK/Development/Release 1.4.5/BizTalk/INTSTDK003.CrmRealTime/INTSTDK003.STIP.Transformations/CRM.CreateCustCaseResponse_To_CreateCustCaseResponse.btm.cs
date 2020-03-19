namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase+RequestCreateCaseResponse", typeof(global::INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase.RequestCreateCaseResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase+RequestCreateCaseResponse", typeof(global::INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase.RequestCreateCaseResponse))]
    public sealed class CRM_CreateCustCaseResponse_To_CreateCustCaseResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s2 s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK003/CreateCase/20141201"" xmlns:s2=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK003/CreateCase_1_2/20141201"" xmlns:s1=""http://tempuri.org/"" xmlns:ns2=""http://www.skanetrafiken.com/DK/INTSTDK003/CreateCase_1/20141201"" xmlns:s0=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:RequestCreateCaseResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:RequestCreateCaseResponse"">
    <ns0:RequestCreateCaseResponse>
      <xsl:value-of select=""./text()"" />
    </ns0:RequestCreateCaseResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase+RequestCreateCaseResponse";
        
        private const global::INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase.RequestCreateCaseResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase+RequestCreateCaseResponse";
        
        private const global::INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase.RequestCreateCaseResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase+RequestCreateCaseResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase+RequestCreateCaseResponse";
                return _TrgSchemas;
            }
        }
    }
}
