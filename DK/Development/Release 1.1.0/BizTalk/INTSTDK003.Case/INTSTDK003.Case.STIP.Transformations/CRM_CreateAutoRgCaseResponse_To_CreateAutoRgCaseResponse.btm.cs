namespace INTSTDK003.Case.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.Case.CRM.Schemas.CreateCase+RequestCreateAutoRGCaseResponse", typeof(global::INTSTDK003.Case.CRM.Schemas.CreateCase.RequestCreateAutoRGCaseResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.Case.STIP.Schemas.CreateCase+RequestCreateAutoRGCaseResponse", typeof(global::INTSTDK003.Case.STIP.Schemas.CreateCase.RequestCreateAutoRGCaseResponse))]
    public sealed class CRM_CreateAutoRgCaseResponse_To_CreateAutoRgCaseResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s2 s0"" version=""1.0"" xmlns:s2=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK003.Case/CreateCase_1/20150626"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK003.Case/CreateCase/20150626"" xmlns:s1=""http://tempuri.org/"" xmlns:s0=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" xmlns:ns2=""http://www.skanetrafiken.com/DK/INTSTDK003.Case/CreateCase_1_2/20150626"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:RequestCreateAutoRGCaseResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:RequestCreateAutoRGCaseResponse"">
    <ns0:RequestCreateAutoRGCaseResponse>
      <xsl:for-each select=""s1:RequestCreateAutoRGCaseResult"">
        <ns0:RequestCreateAutoRGCaseResult>
          <xsl:if test=""s0:CaseID"">
            <xsl:variable name=""var:v1"" select=""string(s0:CaseID/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v1)='true'"">
              <ns1:CaseID>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:CaseID>
            </xsl:if>
            <xsl:if test=""string($var:v1)='false'"">
              <ns1:CaseID>
                <xsl:value-of select=""s0:CaseID/text()"" />
              </ns1:CaseID>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:ErrorMessage"">
            <xsl:variable name=""var:v2"" select=""string(s0:ErrorMessage/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v2)='true'"">
              <ns1:ErrorMessage>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:ErrorMessage>
            </xsl:if>
            <xsl:if test=""string($var:v2)='false'"">
              <ns1:ErrorMessage>
                <xsl:value-of select=""s0:ErrorMessage/text()"" />
              </ns1:ErrorMessage>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:Success"">
            <ns1:Success>
              <xsl:value-of select=""s0:Success/text()"" />
            </ns1:Success>
          </xsl:if>
        </ns0:RequestCreateAutoRGCaseResult>
      </xsl:for-each>
    </ns0:RequestCreateAutoRGCaseResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.Case.CRM.Schemas.CreateCase+RequestCreateAutoRGCaseResponse";
        
        private const global::INTSTDK003.Case.CRM.Schemas.CreateCase.RequestCreateAutoRGCaseResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.Case.STIP.Schemas.CreateCase+RequestCreateAutoRGCaseResponse";
        
        private const global::INTSTDK003.Case.STIP.Schemas.CreateCase.RequestCreateAutoRGCaseResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.Case.CRM.Schemas.CreateCase+RequestCreateAutoRGCaseResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.Case.STIP.Schemas.CreateCase+RequestCreateAutoRGCaseResponse";
                return _TrgSchemas;
            }
        }
    }
}
