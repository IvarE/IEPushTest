namespace INTSKDK._007.CMS.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK007.CMS.Schemas.Internal.GetJourneyType+GetJourney", typeof(global::INTSTDK007.CMS.Schemas.Internal.GetJourneyType.GetJourney))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK._007.Etis.Schemas.External.GetJourneyType+GetJourney", typeof(global::INTSTDK._007.Etis.Schemas.External.GetJourneyType.GetJourney))]
    public sealed class CMS_GetJourney_To_Eits_GetJourney : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK007/GetJourney/20141216"" xmlns:ns0=""http://www.etis.fskab.se/v1.0/ETISws"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetJourney"" />
  </xsl:template>
  <xsl:template match=""/s0:GetJourney"">
    <ns0:GetJourney>
      <ns0:FromPointId>
        <xsl:value-of select=""FromPointId/text()"" />
      </ns0:FromPointId>
      <ns0:FromPointType>
        <xsl:value-of select=""FromPointType/text()"" />
      </ns0:FromPointType>
      <ns0:ToPointId>
        <xsl:value-of select=""ToPointId/text()"" />
      </ns0:ToPointId>
      <ns0:ToPointType>
        <xsl:value-of select=""ToPointType/text()"" />
      </ns0:ToPointType>
      <xsl:if test=""ViaPointId"">
        <ns0:ViaPointId>
          <xsl:value-of select=""ViaPointId/text()"" />
        </ns0:ViaPointId>
      </xsl:if>
      <xsl:if test=""WaitingTime"">
        <ns0:WaitingTime>
          <xsl:value-of select=""WaitingTime/text()"" />
        </ns0:WaitingTime>
      </xsl:if>
      <xsl:if test=""JourneyDateTime"">
        <ns0:JourneyDateTime>
          <xsl:value-of select=""JourneyDateTime/text()"" />
        </ns0:JourneyDateTime>
      </xsl:if>
      <xsl:if test=""Direction"">
        <ns0:Direction>
          <xsl:value-of select=""Direction/text()"" />
        </ns0:Direction>
      </xsl:if>
      <xsl:if test=""NoOfJourneysBefore"">
        <ns0:NoOfJourneysBefore>
          <xsl:value-of select=""NoOfJourneysBefore/text()"" />
        </ns0:NoOfJourneysBefore>
      </xsl:if>
      <xsl:if test=""NoOfJourneysAfter"">
        <ns0:NoOfJourneysAfter>
          <xsl:value-of select=""NoOfJourneysAfter/text()"" />
        </ns0:NoOfJourneysAfter>
      </xsl:if>
      <xsl:if test=""ChangeTime"">
        <ns0:ChangeTime>
          <xsl:value-of select=""ChangeTime/text()"" />
        </ns0:ChangeTime>
      </xsl:if>
      <xsl:if test=""Priority"">
        <ns0:Priority>
          <xsl:value-of select=""Priority/text()"" />
        </ns0:Priority>
      </xsl:if>
      <xsl:if test=""SelectedMeansOfTransport"">
        <ns0:SelectedMeansOfTransport>
          <xsl:value-of select=""SelectedMeansOfTransport/text()"" />
        </ns0:SelectedMeansOfTransport>
      </xsl:if>
      <xsl:if test=""SelectionType"">
        <ns0:SelectionType>
          <xsl:value-of select=""SelectionType/text()"" />
        </ns0:SelectionType>
      </xsl:if>
      <xsl:if test=""Accessibility"">
        <ns0:Accessibility>
          <xsl:value-of select=""Accessibility/text()"" />
        </ns0:Accessibility>
      </xsl:if>
      <xsl:if test=""MaxWalkDistance"">
        <ns0:MaxWalkDistance>
          <xsl:value-of select=""MaxWalkDistance/text()"" />
        </ns0:MaxWalkDistance>
      </xsl:if>
      <xsl:if test=""DetailedResult"">
        <ns0:DetailedResult>
          <xsl:value-of select=""DetailedResult/text()"" />
        </ns0:DetailedResult>
      </xsl:if>
      <xsl:if test=""WalkSpeed"">
        <ns0:WalkSpeed>
          <xsl:value-of select=""WalkSpeed/text()"" />
        </ns0:WalkSpeed>
      </xsl:if>
      <xsl:if test=""VehicleAccessibility"">
        <ns0:VehicleAccessibility>
          <xsl:value-of select=""VehicleAccessibility/text()"" />
        </ns0:VehicleAccessibility>
      </xsl:if>
    </ns0:GetJourney>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK007.CMS.Schemas.Internal.GetJourneyType+GetJourney";
        
        private const global::INTSTDK007.CMS.Schemas.Internal.GetJourneyType.GetJourney _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK._007.Etis.Schemas.External.GetJourneyType+GetJourney";
        
        private const global::INTSTDK._007.Etis.Schemas.External.GetJourneyType.GetJourney _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK007.CMS.Schemas.Internal.GetJourneyType+GetJourney";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK._007.Etis.Schemas.External.GetJourneyType+GetJourney";
                return _TrgSchemas;
            }
        }
    }
}
