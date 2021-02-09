namespace INTSTDK003.Case.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.Case.STIP.Schemas.CreateCase+RequestUpdateAutoRGCase", typeof(global::INTSTDK003.Case.STIP.Schemas.CreateCase.RequestUpdateAutoRGCase))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.Case.CRM.Schemas.CreateCase+RequestUpdateAutoRGCase", typeof(global::INTSTDK003.Case.CRM.Schemas.CreateCase.RequestUpdateAutoRGCase))]
    public sealed class STIP_UpdateAutoRgCaseRequest_To_UpdateAutoRgCaseRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s2 s0 s1"" version=""1.0"" xmlns:ns2=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK003.Case/CreateCase_1/20150626"" xmlns:s1=""http://www.skanetrafiken.com/DK/INTSTDK003.Case/CreateCase/20150626"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" xmlns:s2=""http://www.skanetrafiken.com/DK/INTSTDK003.Case/CreateCase_1_2/20150626"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:RequestUpdateAutoRGCase"" />
  </xsl:template>
  <xsl:template match=""/s1:RequestUpdateAutoRGCase"">
    <ns0:RequestUpdateAutoRGCase>
      <xsl:for-each select=""s1:request"">
        <ns0:request>
          <xsl:if test=""s0:Approved"">
            <ns1:Approved>
              <xsl:value-of select=""s0:Approved/text()"" />
            </ns1:Approved>
          </xsl:if>
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
          <xsl:if test=""s0:Currency"">
            <xsl:variable name=""var:v2"" select=""string(s0:Currency/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v2)='true'"">
              <ns1:Currency>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:Currency>
            </xsl:if>
            <xsl:if test=""string($var:v2)='false'"">
              <ns1:Currency>
                <xsl:value-of select=""s0:Currency/text()"" />
              </ns1:Currency>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:CustomerMessage"">
            <xsl:variable name=""var:v3"" select=""string(s0:CustomerMessage/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v3)='true'"">
              <ns1:CustomerMessage>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:CustomerMessage>
            </xsl:if>
            <xsl:if test=""string($var:v3)='false'"">
              <ns1:CustomerMessage>
                <xsl:value-of select=""s0:CustomerMessage/text()"" />
              </ns1:CustomerMessage>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:DelayType"">
            <xsl:variable name=""var:v4"" select=""string(s0:DelayType/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v4)='true'"">
              <ns1:DelayType>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:DelayType>
            </xsl:if>
            <xsl:if test=""string($var:v4)='false'"">
              <ns1:DelayType>
                <xsl:value-of select=""s0:DelayType/text()"" />
              </ns1:DelayType>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:InternalMessage"">
            <xsl:variable name=""var:v5"" select=""string(s0:InternalMessage/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v5)='true'"">
              <ns1:InternalMessage>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:InternalMessage>
            </xsl:if>
            <xsl:if test=""string($var:v5)='false'"">
              <ns1:InternalMessage>
                <xsl:value-of select=""s0:InternalMessage/text()"" />
              </ns1:InternalMessage>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:RefundType"">
            <ns1:RefundType>
              <xsl:value-of select=""s0:RefundType/text()"" />
            </ns1:RefundType>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationArrivalActual"">
            <xsl:variable name=""var:v6"" select=""string(s0:TravelInformationArrivalActual/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v6)='true'"">
              <ns1:TravelInformationArrivalActual>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationArrivalActual>
            </xsl:if>
            <xsl:if test=""string($var:v6)='false'"">
              <ns1:TravelInformationArrivalActual>
                <xsl:value-of select=""s0:TravelInformationArrivalActual/text()"" />
              </ns1:TravelInformationArrivalActual>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationArrivalPlanned"">
            <xsl:variable name=""var:v7"" select=""string(s0:TravelInformationArrivalPlanned/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v7)='true'"">
              <ns1:TravelInformationArrivalPlanned>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationArrivalPlanned>
            </xsl:if>
            <xsl:if test=""string($var:v7)='false'"">
              <ns1:TravelInformationArrivalPlanned>
                <xsl:value-of select=""s0:TravelInformationArrivalPlanned/text()"" />
              </ns1:TravelInformationArrivalPlanned>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationCity"">
            <xsl:variable name=""var:v8"" select=""string(s0:TravelInformationCity/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v8)='true'"">
              <ns1:TravelInformationCity>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationCity>
            </xsl:if>
            <xsl:if test=""string($var:v8)='false'"">
              <ns1:TravelInformationCity>
                <xsl:value-of select=""s0:TravelInformationCity/text()"" />
              </ns1:TravelInformationCity>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationCompany"">
            <xsl:variable name=""var:v9"" select=""string(s0:TravelInformationCompany/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v9)='true'"">
              <ns1:TravelInformationCompany>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationCompany>
            </xsl:if>
            <xsl:if test=""string($var:v9)='false'"">
              <ns1:TravelInformationCompany>
                <xsl:value-of select=""s0:TravelInformationCompany/text()"" />
              </ns1:TravelInformationCompany>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationDeviationMessage"">
            <xsl:variable name=""var:v10"" select=""string(s0:TravelInformationDeviationMessage/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v10)='true'"">
              <ns1:TravelInformationDeviationMessage>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationDeviationMessage>
            </xsl:if>
            <xsl:if test=""string($var:v10)='false'"">
              <ns1:TravelInformationDeviationMessage>
                <xsl:value-of select=""s0:TravelInformationDeviationMessage/text()"" />
              </ns1:TravelInformationDeviationMessage>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationDirectionText"">
            <xsl:variable name=""var:v11"" select=""string(s0:TravelInformationDirectionText/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v11)='true'"">
              <ns1:TravelInformationDirectionText>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationDirectionText>
            </xsl:if>
            <xsl:if test=""string($var:v11)='false'"">
              <ns1:TravelInformationDirectionText>
                <xsl:value-of select=""s0:TravelInformationDirectionText/text()"" />
              </ns1:TravelInformationDirectionText>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationDisplayText"">
            <xsl:variable name=""var:v12"" select=""string(s0:TravelInformationDisplayText/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v12)='true'"">
              <ns1:TravelInformationDisplayText>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationDisplayText>
            </xsl:if>
            <xsl:if test=""string($var:v12)='false'"">
              <ns1:TravelInformationDisplayText>
                <xsl:value-of select=""s0:TravelInformationDisplayText/text()"" />
              </ns1:TravelInformationDisplayText>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationLine"">
            <xsl:variable name=""var:v13"" select=""string(s0:TravelInformationLine/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v13)='true'"">
              <ns1:TravelInformationLine>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationLine>
            </xsl:if>
            <xsl:if test=""string($var:v13)='false'"">
              <ns1:TravelInformationLine>
                <xsl:value-of select=""s0:TravelInformationLine/text()"" />
              </ns1:TravelInformationLine>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationStart"">
            <xsl:variable name=""var:v14"" select=""string(s0:TravelInformationStart/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v14)='true'"">
              <ns1:TravelInformationStart>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationStart>
            </xsl:if>
            <xsl:if test=""string($var:v14)='false'"">
              <ns1:TravelInformationStart>
                <xsl:value-of select=""s0:TravelInformationStart/text()"" />
              </ns1:TravelInformationStart>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationStartActual"">
            <xsl:variable name=""var:v15"" select=""string(s0:TravelInformationStartActual/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v15)='true'"">
              <ns1:TravelInformationStartActual>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationStartActual>
            </xsl:if>
            <xsl:if test=""string($var:v15)='false'"">
              <ns1:TravelInformationStartActual>
                <xsl:value-of select=""s0:TravelInformationStartActual/text()"" />
              </ns1:TravelInformationStartActual>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationStartPlanned"">
            <xsl:variable name=""var:v16"" select=""string(s0:TravelInformationStartPlanned/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v16)='true'"">
              <ns1:TravelInformationStartPlanned>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationStartPlanned>
            </xsl:if>
            <xsl:if test=""string($var:v16)='false'"">
              <ns1:TravelInformationStartPlanned>
                <xsl:value-of select=""s0:TravelInformationStartPlanned/text()"" />
              </ns1:TravelInformationStartPlanned>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationStop"">
            <xsl:variable name=""var:v17"" select=""string(s0:TravelInformationStop/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v17)='true'"">
              <ns1:TravelInformationStop>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationStop>
            </xsl:if>
            <xsl:if test=""string($var:v17)='false'"">
              <ns1:TravelInformationStop>
                <xsl:value-of select=""s0:TravelInformationStop/text()"" />
              </ns1:TravelInformationStop>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationTitle"">
            <xsl:variable name=""var:v18"" select=""string(s0:TravelInformationTitle/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v18)='true'"">
              <ns1:TravelInformationTitle>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationTitle>
            </xsl:if>
            <xsl:if test=""string($var:v18)='false'"">
              <ns1:TravelInformationTitle>
                <xsl:value-of select=""s0:TravelInformationTitle/text()"" />
              </ns1:TravelInformationTitle>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationTour"">
            <xsl:variable name=""var:v19"" select=""string(s0:TravelInformationTour/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v19)='true'"">
              <ns1:TravelInformationTour>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationTour>
            </xsl:if>
            <xsl:if test=""string($var:v19)='false'"">
              <ns1:TravelInformationTour>
                <xsl:value-of select=""s0:TravelInformationTour/text()"" />
              </ns1:TravelInformationTour>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:TravelInformationTransport"">
            <xsl:variable name=""var:v20"" select=""string(s0:TravelInformationTransport/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v20)='true'"">
              <ns1:TravelInformationTransport>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:TravelInformationTransport>
            </xsl:if>
            <xsl:if test=""string($var:v20)='false'"">
              <ns1:TravelInformationTransport>
                <xsl:value-of select=""s0:TravelInformationTransport/text()"" />
              </ns1:TravelInformationTransport>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:Value"">
            <ns1:Value>
              <xsl:value-of select=""s0:Value/text()"" />
            </ns1:Value>
          </xsl:if>
          <xsl:value-of select=""./text()"" />
        </ns0:request>
      </xsl:for-each>
    </ns0:RequestUpdateAutoRGCase>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.Case.STIP.Schemas.CreateCase+RequestUpdateAutoRGCase";
        
        private const global::INTSTDK003.Case.STIP.Schemas.CreateCase.RequestUpdateAutoRGCase _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.Case.CRM.Schemas.CreateCase+RequestUpdateAutoRGCase";
        
        private const global::INTSTDK003.Case.CRM.Schemas.CreateCase.RequestUpdateAutoRGCase _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.Case.STIP.Schemas.CreateCase+RequestUpdateAutoRGCase";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.Case.CRM.Schemas.CreateCase+RequestUpdateAutoRGCase";
                return _TrgSchemas;
            }
        }
    }
}
