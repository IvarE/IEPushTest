namespace INTSTDK001.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.STIP.Schemas.External.INTSTDK001GetDirectJourneysBetweenStops", typeof(global::INTSTDK001.STIP.Schemas.External.INTSTDK001GetDirectJourneysBetweenStops))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest", typeof(global::INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest))]
    public sealed class STIP_GetDirectJourneysBetweenStops_To_Internal : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.se/INTSTDK001GetDirectJourneysBetweenStops"" xmlns:ns0=""http://INTSTDK001.CRM.Schemas.GetDirectJourneysBetweenStopsRequest.20140710"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetDirectJourneysBetweenStops"" />
  </xsl:template>
  <xsl:template match=""/s0:GetDirectJourneysBetweenStops"">
    <ns0:GetDirectJourneysBetweenStops>
      <fromStopAreaGid>
        <xsl:value-of select=""s0:fromStopAreaGid/text()"" />
      </fromStopAreaGid>
      <toStopAreaGid>
        <xsl:value-of select=""s0:toStopAreaGid/text()"" />
      </toStopAreaGid>
      <forTimeWindowStartDateTime>
        <xsl:value-of select=""s0:forTimeWindowStartDateTime/text()"" />
      </forTimeWindowStartDateTime>
      <xsl:if test=""s0:forTimeWindowDuration"">
        <forTimeWindowDuration>
          <xsl:value-of select=""s0:forTimeWindowDuration/text()"" />
        </forTimeWindowDuration>
      </xsl:if>
      <xsl:if test=""s0:withDepartureMaxCount"">
        <withDepartureMaxCount>
          <xsl:value-of select=""s0:withDepartureMaxCount/text()"" />
        </withDepartureMaxCount>
      </xsl:if>
      <xsl:if test=""s0:forLineGids"">
        <forLineGids>
          <xsl:value-of select=""s0:forLineGids/text()"" />
        </forLineGids>
      </xsl:if>
      <xsl:if test=""s0:forProducts"">
        <forProducts>
          <xsl:value-of select=""s0:forProducts/text()"" />
        </forProducts>
      </xsl:if>
      <xsl:if test=""s0:purposeOfLineGroupingCode"">
        <purposeOfLineGroupingCode>
          <xsl:value-of select=""s0:purposeOfLineGroupingCode/text()"" />
        </purposeOfLineGroupingCode>
      </xsl:if>
    </ns0:GetDirectJourneysBetweenStops>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK001.STIP.Schemas.External.INTSTDK001GetDirectJourneysBetweenStops";
        
        private const global::INTSTDK001.STIP.Schemas.External.INTSTDK001GetDirectJourneysBetweenStops _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest";
        
        private const global::INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK001.STIP.Schemas.External.INTSTDK001GetDirectJourneysBetweenStops";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK001.STIP.Schemas.Internal.GetDirectJourneysBetweenStopsRequest";
                return _TrgSchemas;
            }
        }
    }
}
