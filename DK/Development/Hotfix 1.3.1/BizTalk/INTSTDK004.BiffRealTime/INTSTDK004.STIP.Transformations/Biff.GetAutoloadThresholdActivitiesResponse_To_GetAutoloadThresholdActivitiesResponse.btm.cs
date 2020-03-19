namespace INTSTDK004.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType", typeof(global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.STIP.Schemas.GetAutoloadThresholdActivitiesResponseType", typeof(global::INTSTDK004.STIP.Schemas.GetAutoloadThresholdActivitiesResponseType))]
    public sealed class Biff_GetAutoloadThresholdActivitiesResponse_To_GetAutoloadThresholdActivitiesResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s2 s0"" version=""1.0"" xmlns:s3=""http://cubic.com"" xmlns:s1=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK004/GetAutoloadThresholdActivitiesResponse/20141216"" xmlns:s2=""http://tempuri.org/CardActivities.xsd"" xmlns:s0=""urn:schemas-microsoft-com:xml-msdata"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetAutoloadThresholdActivitiesResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:GetAutoloadThresholdActivitiesResponse"">
    <ns0:GetAutoloadThresholdActivitiesReportResponse>
      <ns0:GetAutoloadThresholdActivitiesReportResult>
        <ns0:CardActivities>
          <xsl:for-each select=""s3:GetAutoloadThresholdActivitiesResult"">
            <xsl:for-each select=""s1:diffgram/s2:CardActivities/s2:Activities"">
              <ns0:Activities>
                <xsl:if test=""s2:TransactionDate"">
                  <ns0:TransactionDate>
                    <xsl:value-of select=""s2:TransactionDate/text()"" />
                  </ns0:TransactionDate>
                </xsl:if>
                <xsl:if test=""s2:DeviceId"">
                  <ns0:DeviceId>
                    <xsl:value-of select=""s2:DeviceId/text()"" />
                  </ns0:DeviceId>
                </xsl:if>
                <xsl:if test=""s2:TxnNum"">
                  <ns0:TxnNum>
                    <xsl:value-of select=""s2:TxnNum/text()"" />
                  </ns0:TxnNum>
                </xsl:if>
                <xsl:if test=""s2:RequestId"">
                  <ns0:RequestId>
                    <xsl:value-of select=""s2:RequestId/text()"" />
                  </ns0:RequestId>
                </xsl:if>
                <xsl:if test=""s2:CardNumber"">
                  <ns0:CardNumber>
                    <xsl:value-of select=""s2:CardNumber/text()"" />
                  </ns0:CardNumber>
                </xsl:if>
                <xsl:if test=""s2:CardSect"">
                  <ns0:CardSect>
                    <xsl:value-of select=""s2:CardSect/text()"" />
                  </ns0:CardSect>
                </xsl:if>
                <xsl:if test=""s2:Route"">
                  <ns0:Route>
                    <xsl:value-of select=""s2:Route/text()"" />
                  </ns0:Route>
                </xsl:if>
                <xsl:if test=""s2:Price"">
                  <ns0:Price>
                    <xsl:value-of select=""s2:Price/text()"" />
                  </ns0:Price>
                </xsl:if>
                <xsl:if test=""s2:Currency"">
                  <ns0:Currency>
                    <xsl:value-of select=""s2:Currency/text()"" />
                  </ns0:Currency>
                </xsl:if>
              </ns0:Activities>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""s3:GetAutoloadThresholdActivitiesResult"">
            <xsl:for-each select=""s1:diffgram/s2:CardActivities/s2:TransactionCount"">
              <ns0:TransactionCount>
                <xsl:if test=""s2:Total"">
                  <ns0:Total>
                    <xsl:value-of select=""s2:Total/text()"" />
                  </ns0:Total>
                </xsl:if>
              </ns0:TransactionCount>
            </xsl:for-each>
          </xsl:for-each>
        </ns0:CardActivities>
      </ns0:GetAutoloadThresholdActivitiesReportResult>
    </ns0:GetAutoloadThresholdActivitiesReportResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType";
        
        private const global::INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK004.STIP.Schemas.GetAutoloadThresholdActivitiesResponseType";
        
        private const global::INTSTDK004.STIP.Schemas.GetAutoloadThresholdActivitiesResponseType _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK004.Biff.Schemas.External.GetAutoloadThresholdActivitiesResponseType";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK004.STIP.Schemas.GetAutoloadThresholdActivitiesResponseType";
                return _TrgSchemas;
            }
        }
    }
}
