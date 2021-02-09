namespace INTSTDK001.PubTrans.Transformations.StopMonitoringService {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest", typeof(global::INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService+GetDirectJourneysBetweenStops", typeof(global::INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService.GetDirectJourneysBetweenStops))]
    public sealed class PubTrans_GetDirectJourneysBetweenStopsRequest_To_External : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:ns1=""urn:schemas-microsoft-com:xml-msdata"" xmlns:s0=""http://INTSTDK001.CRM.Schemas.GetDirectJourneysBetweenStopsRequest.20140710"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetDirectJourneysBetweenStops"" />
  </xsl:template>
  <xsl:template match=""/s0:GetDirectJourneysBetweenStops"">
    <ns0:GetDirectJourneysBetweenStops>
      <xsl:if test=""fromStopAreaGid"">
        <ns0:fromStopAreaGid>
          <xsl:value-of select=""fromStopAreaGid/text()"" />
        </ns0:fromStopAreaGid>
      </xsl:if>
      <xsl:if test=""toStopAreaGid"">
        <ns0:toStopAreaGid>
          <xsl:value-of select=""toStopAreaGid/text()"" />
        </ns0:toStopAreaGid>
      </xsl:if>
      <xsl:if test=""forTimeWindowStartDateTime"">
        <ns0:forTimeWindowStartDateTime>
          <xsl:value-of select=""forTimeWindowStartDateTime/text()"" />
        </ns0:forTimeWindowStartDateTime>
      </xsl:if>
      <xsl:if test=""forTimeWindowDuration"">
        <ns0:forTimeWindowDuration>
          <xsl:value-of select=""forTimeWindowDuration/text()"" />
        </ns0:forTimeWindowDuration>
      </xsl:if>
      <xsl:if test=""withDepartureMaxCount"">
        <ns0:withDepartureMaxCount>
          <xsl:value-of select=""withDepartureMaxCount/text()"" />
        </ns0:withDepartureMaxCount>
      </xsl:if>
      <xsl:if test=""forLineGids"">
        <ns0:forLineGids>
          <xsl:value-of select=""forLineGids/text()"" />
        </ns0:forLineGids>
      </xsl:if>
      <xsl:if test=""forProducts"">
        <ns0:forProducts>
          <xsl:value-of select=""forProducts/text()"" />
        </ns0:forProducts>
      </xsl:if>
      <xsl:if test=""purposeOfLineGroupingCode"">
        <ns0:purposeOfLineGroupingCode>
          <xsl:value-of select=""purposeOfLineGroupingCode/text()"" />
        </ns0:purposeOfLineGroupingCode>
      </xsl:if>
    </ns0:GetDirectJourneysBetweenStops>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest";
        
        private const global::INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest";
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
