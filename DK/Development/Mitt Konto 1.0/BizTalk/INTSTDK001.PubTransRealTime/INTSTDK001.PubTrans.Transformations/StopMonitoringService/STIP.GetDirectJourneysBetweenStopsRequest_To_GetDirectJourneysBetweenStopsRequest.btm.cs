namespace INTSTDK001.PubTrans.Transformations.StopMonitoringService {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.STIP.Schemas.GetDirectJourneyBetweenStopsRequest", typeof(global::INTSTDK001.STIP.Schemas.GetDirectJourneyBetweenStopsRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService+GetDirectJourneysBetweenStops", typeof(global::INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService.GetDirectJourneysBetweenStops))]
    public sealed class STIP_GetDirectJourneysBetweenStopsRequest_To_GetDirectJourneysBetweenStopsRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK001/GetDirectJourneyBetweenStopsRequest/20141023"" xmlns:ns0=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:ns1=""urn:schemas-microsoft-com:xml-msdata"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetDirectJourneysBetweenStops"" />
  </xsl:template>
  <xsl:template match=""/s0:GetDirectJourneysBetweenStops"">
    <ns0:GetDirectJourneysBetweenStops>
      <xsl:if test=""fromStopAreaGid"">
        <xsl:variable name=""var:v1"" select=""string(fromStopAreaGid/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v1)='true'"">
          <ns0:fromStopAreaGid>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:fromStopAreaGid>
        </xsl:if>
        <xsl:if test=""string($var:v1)='false'"">
          <ns0:fromStopAreaGid>
            <xsl:value-of select=""fromStopAreaGid/text()"" />
          </ns0:fromStopAreaGid>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""toStopAreaGid"">
        <xsl:variable name=""var:v2"" select=""string(toStopAreaGid/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v2)='true'"">
          <ns0:toStopAreaGid>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:toStopAreaGid>
        </xsl:if>
        <xsl:if test=""string($var:v2)='false'"">
          <ns0:toStopAreaGid>
            <xsl:value-of select=""toStopAreaGid/text()"" />
          </ns0:toStopAreaGid>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""forTimeWindowStartDateTime"">
        <ns0:forTimeWindowStartDateTime>
          <xsl:value-of select=""forTimeWindowStartDateTime/text()"" />
        </ns0:forTimeWindowStartDateTime>
      </xsl:if>
      <xsl:if test=""forTimeWindowDuration"">
        <xsl:variable name=""var:v3"" select=""string(forTimeWindowDuration/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v3)='true'"">
          <ns0:forTimeWindowDuration>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:forTimeWindowDuration>
        </xsl:if>
        <xsl:if test=""string($var:v3)='false'"">
          <ns0:forTimeWindowDuration>
            <xsl:value-of select=""forTimeWindowDuration/text()"" />
          </ns0:forTimeWindowDuration>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""withDepartureMaxCount"">
        <xsl:variable name=""var:v4"" select=""string(withDepartureMaxCount/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v4)='true'"">
          <ns0:withDepartureMaxCount>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:withDepartureMaxCount>
        </xsl:if>
        <xsl:if test=""string($var:v4)='false'"">
          <ns0:withDepartureMaxCount>
            <xsl:value-of select=""withDepartureMaxCount/text()"" />
          </ns0:withDepartureMaxCount>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""forLineGids"">
        <xsl:variable name=""var:v5"" select=""string(forLineGids/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v5)='true'"">
          <ns0:forLineGids>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:forLineGids>
        </xsl:if>
        <xsl:if test=""string($var:v5)='false'"">
          <ns0:forLineGids>
            <xsl:value-of select=""forLineGids/text()"" />
          </ns0:forLineGids>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""forProducts"">
        <xsl:variable name=""var:v6"" select=""string(forProducts/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v6)='true'"">
          <ns0:forProducts>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:forProducts>
        </xsl:if>
        <xsl:if test=""string($var:v6)='false'"">
          <ns0:forProducts>
            <xsl:value-of select=""forProducts/text()"" />
          </ns0:forProducts>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""purposeOfLineGroupingCode"">
        <xsl:variable name=""var:v7"" select=""string(purposeOfLineGroupingCode/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v7)='true'"">
          <ns0:purposeOfLineGroupingCode>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:purposeOfLineGroupingCode>
        </xsl:if>
        <xsl:if test=""string($var:v7)='false'"">
          <ns0:purposeOfLineGroupingCode>
            <xsl:value-of select=""purposeOfLineGroupingCode/text()"" />
          </ns0:purposeOfLineGroupingCode>
        </xsl:if>
      </xsl:if>
    </ns0:GetDirectJourneysBetweenStops>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK001.STIP.Schemas.GetDirectJourneyBetweenStopsRequest";
        
        private const global::INTSTDK001.STIP.Schemas.GetDirectJourneyBetweenStopsRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService+GetDirectJourneysBetweenStops";
        
        private const global::INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService.GetDirectJourneysBetweenStops _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK001.STIP.Schemas.GetDirectJourneyBetweenStopsRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService+GetDirectJourneysBetweenStops";
                return _TrgSchemas;
            }
        }
    }
}
