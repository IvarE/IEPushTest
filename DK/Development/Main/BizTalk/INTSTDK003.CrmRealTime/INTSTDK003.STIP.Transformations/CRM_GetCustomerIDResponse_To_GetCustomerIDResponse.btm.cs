namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCardResponse", typeof(global::INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCardResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCardResponse", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCardResponse))]
    public sealed class CRM_GetCustomerIDResponse_To_GetCustomerIDResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s0 s2"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:s3=""http://tempuri.org/"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGIXrmEAIConnectorService"" xmlns:s2=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns2=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2_3/20141126"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126"" xmlns:ns3=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetCustomerIdForTravelCardResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:GetCustomerIdForTravelCardResponse"">
    <ns0:GetCustomerIdForTravelCardResponse>
      <xsl:for-each select=""s3:GetCustomerIdForTravelCardResult"">
        <ns0:GetCustomerIdForTravelCardResult>
          <xsl:if test=""s1:IsNull"">
            <ns3:IsNull>
              <xsl:value-of select=""s1:IsNull/text()"" />
            </ns3:IsNull>
          </xsl:if>
          <xsl:if test=""s1:Message"">
            <xsl:variable name=""var:v1"" select=""string(s1:Message/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v1)='true'"">
              <ns3:Message>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns3:Message>
            </xsl:if>
            <xsl:if test=""string($var:v1)='false'"">
              <ns3:Message>
                <xsl:value-of select=""s1:Message/text()"" />
              </ns3:Message>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s1:Status"">
            <ns3:Status>
              <xsl:value-of select=""s1:Status/text()"" />
            </ns3:Status>
          </xsl:if>
          <xsl:for-each select=""s1:Details"">
            <ns3:Details>
              <xsl:for-each select=""s1:Detail"">
                <ns3:Detail>
                  <xsl:if test=""s1:CustomerId"">
                    <ns3:CustomerId>
                      <xsl:value-of select=""s1:CustomerId/text()"" />
                    </ns3:CustomerId>
                  </xsl:if>
                  <xsl:if test=""s1:CustomerType"">
                    <ns3:CustomerType>
                      <xsl:value-of select=""s1:CustomerType/text()"" />
                    </ns3:CustomerType>
                  </xsl:if>
                  <xsl:if test=""s1:TravelCardName"">
                    <xsl:variable name=""var:v2"" select=""string(s1:TravelCardName/@xsi:nil) = 'true'"" />
                    <xsl:if test=""string($var:v2)='true'"">
                      <ns3:TravelCardName>
                        <xsl:attribute name=""xsi:nil"">
                          <xsl:value-of select=""'true'"" />
                        </xsl:attribute>
                      </ns3:TravelCardName>
                    </xsl:if>
                    <xsl:if test=""string($var:v2)='false'"">
                      <ns3:TravelCardName>
                        <xsl:value-of select=""s1:TravelCardName/text()"" />
                      </ns3:TravelCardName>
                    </xsl:if>
                  </xsl:if>
                  <xsl:if test=""s1:TravelCardNumber"">
                    <xsl:variable name=""var:v3"" select=""string(s1:TravelCardNumber/@xsi:nil) = 'true'"" />
                    <xsl:if test=""string($var:v3)='true'"">
                      <ns3:TravelCardNumber>
                        <xsl:attribute name=""xsi:nil"">
                          <xsl:value-of select=""'true'"" />
                        </xsl:attribute>
                      </ns3:TravelCardNumber>
                    </xsl:if>
                    <xsl:if test=""string($var:v3)='false'"">
                      <ns3:TravelCardNumber>
                        <xsl:value-of select=""s1:TravelCardNumber/text()"" />
                      </ns3:TravelCardNumber>
                    </xsl:if>
                  </xsl:if>
                  <xsl:value-of select=""./text()"" />
                </ns3:Detail>
              </xsl:for-each>
              <xsl:value-of select=""./text()"" />
            </ns3:Details>
          </xsl:for-each>
        </ns0:GetCustomerIdForTravelCardResult>
      </xsl:for-each>
    </ns0:GetCustomerIdForTravelCardResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCardResponse";
        
        private const global::INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCardResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCardResponse";
        
        private const global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerIdForTravelCardResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCardResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerIdForTravelCardResponse";
                return _TrgSchemas;
            }
        }
    }
}
