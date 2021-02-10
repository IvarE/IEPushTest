namespace INTSTDK001.PubTrans.Transformations.VehicleMonitoringService {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.STIP.Schemas.GetCallsForServiceJourney", typeof(global::INTSTDK001.STIP.Schemas.GetCallsForServiceJourney))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService+GetCallsforServiceJourney", typeof(global::INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService.GetCallsforServiceJourney))]
    public sealed class STIP_GetCallsForServiceJourneyRequest_To_GetCallsForServiceJourneyRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanestrafiken.com/DK/INTSTDK001/GetCallsForServiceJourney/20141020"" xmlns:diffgr=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:ns1=""urn:schemas-microsoft-com:xml-msdata"" xmlns:ns0=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetCallsforServiceJourney"" />
  </xsl:template>
  <xsl:template match=""/s0:GetCallsforServiceJourney"">
    <ns0:GetCallsforServiceJourney>
      <xsl:if test=""forServiceJourneyIdOrGid"">
        <xsl:variable name=""var:v1"" select=""string(forServiceJourneyIdOrGid/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v1)='true'"">
          <ns0:forServiceJourneyIdOrGid>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:forServiceJourneyIdOrGid>
        </xsl:if>
        <xsl:if test=""string($var:v1)='false'"">
          <ns0:forServiceJourneyIdOrGid>
            <xsl:value-of select=""forServiceJourneyIdOrGid/text()"" />
          </ns0:forServiceJourneyIdOrGid>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""atOperatingDate"">
        <ns0:atOperatingDate>
          <xsl:value-of select=""atOperatingDate/text()"" />
        </ns0:atOperatingDate>
      </xsl:if>
      <xsl:if test=""atStopGid"">
        <xsl:variable name=""var:v2"" select=""string(atStopGid/@xsi:nil) = 'true'"" />
        <xsl:if test=""string($var:v2)='true'"">
          <ns0:atStopGid>
            <xsl:attribute name=""xsi:nil"">
              <xsl:value-of select=""'true'"" />
            </xsl:attribute>
          </ns0:atStopGid>
        </xsl:if>
        <xsl:if test=""string($var:v2)='false'"">
          <ns0:atStopGid>
            <xsl:value-of select=""atStopGid/text()"" />
          </ns0:atStopGid>
        </xsl:if>
      </xsl:if>
      <xsl:if test=""includeArrivalsTable"">
        <ns0:includeArrivalsTable>
          <xsl:value-of select=""includeArrivalsTable/text()"" />
        </ns0:includeArrivalsTable>
      </xsl:if>
      <xsl:if test=""includeDeparturesTable"">
        <ns0:includeDeparturesTable>
          <xsl:value-of select=""includeDeparturesTable/text()"" />
        </ns0:includeDeparturesTable>
      </xsl:if>
      <xsl:if test=""includeDeviationTables"">
        <ns0:includeDeviationTables>
          <xsl:value-of select=""includeDeviationTables/text()"" />
        </ns0:includeDeviationTables>
      </xsl:if>
    </ns0:GetCallsforServiceJourney>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK001.STIP.Schemas.GetCallsForServiceJourney";
        
        private const global::INTSTDK001.STIP.Schemas.GetCallsForServiceJourney _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService+GetCallsforServiceJourney";
        
        private const global::INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService.GetCallsforServiceJourney _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK001.STIP.Schemas.GetCallsForServiceJourney";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService+GetCallsforServiceJourney";
                return _TrgSchemas;
            }
        }
    }
}
