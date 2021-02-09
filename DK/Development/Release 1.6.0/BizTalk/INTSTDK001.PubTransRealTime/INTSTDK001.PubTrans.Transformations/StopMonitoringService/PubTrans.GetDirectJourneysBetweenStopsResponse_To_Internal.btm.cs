namespace INTSTDK001.PubTrans.Transformations.StopMonitoringService {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService+GetDirectJourneysBetweenStopsResponse", typeof(global::INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService.GetDirectJourneysBetweenStopsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.Internal.GetDirectJourneysBetweenStopsResponse", typeof(global::INTSTDK001.PubTrans.Schemas.Internal.GetDirectJourneysBetweenStopsResponse))]
    public sealed class PubTrans_GetDirectJourneysBetweenStopsResponse_To_Internal : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s1 s0 s2"" version=""1.0"" xmlns:s1=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/StopMonitoringService"" xmlns:s0=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:s2=""urn:schemas-microsoft-com:xml-msdata"" xmlns:ns0=""http://INTSTDK001.PubTrans.Schemas.GetDirectJourneysBetweenStopsResponse.20140710"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:GetDirectJourneysBetweenStopsResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:GetDirectJourneysBetweenStopsResponse"">
    <ns0:GetDirectJourneysBetweenStopsResponse>
      <xsl:for-each select=""s1:GetDirectJourneysBetweenStopsResult"">
        <xsl:for-each select=""s0:diffgram/GetDirectJourneysBetweenStopsMethod/DirectJourneysBetweenStops"">
          <DirectJourneysBetweenStops>
            <xsl:if test=""DatedVehicleJourneyId"">
              <DatedVehicleJourneyId>
                <xsl:value-of select=""DatedVehicleJourneyId/text()"" />
              </DatedVehicleJourneyId>
            </xsl:if>
            <xsl:if test=""ServiceJourneyGid"">
              <ServiceJourneyGid>
                <xsl:value-of select=""ServiceJourneyGid/text()"" />
              </ServiceJourneyGid>
            </xsl:if>
            <xsl:if test=""OperatingDayDate"">
              <OperatingDayDate>
                <xsl:value-of select=""OperatingDayDate/text()"" />
              </OperatingDayDate>
            </xsl:if>
            <xsl:if test=""ContractorGid"">
              <ContractorGid>
                <xsl:value-of select=""ContractorGid/text()"" />
              </ContractorGid>
            </xsl:if>
            <xsl:if test=""LineDesignation"">
              <LineDesignation>
                <xsl:value-of select=""LineDesignation/text()"" />
              </LineDesignation>
            </xsl:if>
            <xsl:if test=""JourneyNumber"">
              <JourneyNumber>
                <xsl:value-of select=""JourneyNumber/text()"" />
              </JourneyNumber>
            </xsl:if>
            <xsl:if test=""DirectionOfLineDescription"">
              <DirectionOfLineDescription>
                <xsl:value-of select=""DirectionOfLineDescription/text()"" />
              </DirectionOfLineDescription>
            </xsl:if>
            <xsl:if test=""OriginName"">
              <OriginName>
                <xsl:value-of select=""OriginName/text()"" />
              </OriginName>
            </xsl:if>
            <xsl:if test=""OriginShortName"">
              <OriginShortName>
                <xsl:value-of select=""OriginShortName/text()"" />
              </OriginShortName>
            </xsl:if>
            <xsl:if test=""OriginPlaceGid"">
              <OriginPlaceGid>
                <xsl:value-of select=""OriginPlaceGid/text()"" />
              </OriginPlaceGid>
            </xsl:if>
            <xsl:if test=""PrimaryDestinationName"">
              <PrimaryDestinationName>
                <xsl:value-of select=""PrimaryDestinationName/text()"" />
              </PrimaryDestinationName>
            </xsl:if>
            <xsl:if test=""PrimaryDestinationShortName"">
              <PrimaryDestinationShortName>
                <xsl:value-of select=""PrimaryDestinationShortName/text()"" />
              </PrimaryDestinationShortName>
            </xsl:if>
            <xsl:if test=""PrimaryDestinationGid"">
              <PrimaryDestinationGid>
                <xsl:value-of select=""PrimaryDestinationGid/text()"" />
              </PrimaryDestinationGid>
            </xsl:if>
            <xsl:if test=""SecondaryDestinationName"">
              <SecondaryDestinationName>
                <xsl:value-of select=""SecondaryDestinationName/text()"" />
              </SecondaryDestinationName>
            </xsl:if>
            <xsl:if test=""SecondaryDestinationShortName"">
              <SecondaryDestinationShortName>
                <xsl:value-of select=""SecondaryDestinationShortName/text()"" />
              </SecondaryDestinationShortName>
            </xsl:if>
            <xsl:if test=""SecondaryDestinationGid"">
              <SecondaryDestinationGid>
                <xsl:value-of select=""SecondaryDestinationGid/text()"" />
              </SecondaryDestinationGid>
            </xsl:if>
            <xsl:if test=""DepartureId"">
              <DepartureId>
                <xsl:value-of select=""DepartureId/text()"" />
              </DepartureId>
            </xsl:if>
            <xsl:if test=""DepartureStopPointGid"">
              <DepartureStopPointGid>
                <xsl:value-of select=""DepartureStopPointGid/text()"" />
              </DepartureStopPointGid>
            </xsl:if>
            <xsl:if test=""DepartureType"">
              <DepartureType>
                <xsl:value-of select=""DepartureType/text()"" />
              </DepartureType>
            </xsl:if>
            <xsl:if test=""DepartureSequenceNumber"">
              <DepartureSequenceNumber>
                <xsl:value-of select=""DepartureSequenceNumber/text()"" />
              </DepartureSequenceNumber>
            </xsl:if>
            <xsl:if test=""PlannedDepartureDateTime"">
              <PlannedDepartureDateTime>
                <xsl:value-of select=""PlannedDepartureDateTime/text()"" />
              </PlannedDepartureDateTime>
            </xsl:if>
            <xsl:if test=""ArrivalId"">
              <ArrivalId>
                <xsl:value-of select=""ArrivalId/text()"" />
              </ArrivalId>
            </xsl:if>
            <xsl:if test=""ArrivalStopPointGid"">
              <ArrivalStopPointGid>
                <xsl:value-of select=""ArrivalStopPointGid/text()"" />
              </ArrivalStopPointGid>
            </xsl:if>
            <xsl:if test=""ArrivalType"">
              <ArrivalType>
                <xsl:value-of select=""ArrivalType/text()"" />
              </ArrivalType>
            </xsl:if>
            <xsl:if test=""ArrivalSequenceNumber"">
              <ArrivalSequenceNumber>
                <xsl:value-of select=""ArrivalSequenceNumber/text()"" />
              </ArrivalSequenceNumber>
            </xsl:if>
            <xsl:if test=""PlannedArrivalDateTime"">
              <PlannedArrivalDateTime>
                <xsl:value-of select=""PlannedArrivalDateTime/text()"" />
              </PlannedArrivalDateTime>
            </xsl:if>
            <xsl:if test=""ExpectedToBeMonitored"">
              <ExpectedToBeMonitored>
                <xsl:value-of select=""ExpectedToBeMonitored/text()"" />
              </ExpectedToBeMonitored>
            </xsl:if>
            <xsl:if test=""TargetDepartureStopPointGid"">
              <TargetDepartureStopPointGid>
                <xsl:value-of select=""TargetDepartureStopPointGid/text()"" />
              </TargetDepartureStopPointGid>
            </xsl:if>
            <xsl:if test=""TargetDepartureDateTime"">
              <TargetDepartureDateTime>
                <xsl:value-of select=""TargetDepartureDateTime/text()"" />
              </TargetDepartureDateTime>
            </xsl:if>
            <xsl:if test=""EstimatedDepartureDateTime"">
              <EstimatedDepartureDateTime>
                <xsl:value-of select=""EstimatedDepartureDateTime/text()"" />
              </EstimatedDepartureDateTime>
            </xsl:if>
            <xsl:if test=""ObservedDepartureDateTime"">
              <ObservedDepartureDateTime>
                <xsl:value-of select=""ObservedDepartureDateTime/text()"" />
              </ObservedDepartureDateTime>
            </xsl:if>
            <xsl:if test=""ArrivalStopPointGid1"">
              <ArrivalStopPointGid1>
                <xsl:value-of select=""ArrivalStopPointGid1/text()"" />
              </ArrivalStopPointGid1>
            </xsl:if>
            <xsl:if test=""ObservedArrivalDateTime"">
              <ObservedArrivalDateTime>
                <xsl:value-of select=""ObservedArrivalDateTime/text()"" />
              </ObservedArrivalDateTime>
            </xsl:if>
            <xsl:value-of select=""./text()"" />
          </DirectJourneysBetweenStops>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetDirectJourneysBetweenStopsResult"">
        <xsl:for-each select=""s0:diffgram/GetDirectJourneysBetweenStopsMethod/DeviationMessageVersion"">
          <DeviationMessageVersion>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""PublicNote"">
              <PublicNote>
                <xsl:value-of select=""PublicNote/text()"" />
              </PublicNote>
            </xsl:if>
            <xsl:if test=""InternalNote"">
              <InternalNote>
                <xsl:value-of select=""InternalNote/text()"" />
              </InternalNote>
            </xsl:if>
            <xsl:if test=""PriorityImportanceLevel"">
              <PriorityImportanceLevel>
                <xsl:value-of select=""PriorityImportanceLevel/text()"" />
              </PriorityImportanceLevel>
            </xsl:if>
            <xsl:if test=""PriorityInfluenceLevel"">
              <PriorityInfluenceLevel>
                <xsl:value-of select=""PriorityInfluenceLevel/text()"" />
              </PriorityInfluenceLevel>
            </xsl:if>
            <xsl:if test=""PriorityUrgencyLevel"">
              <PriorityUrgencyLevel>
                <xsl:value-of select=""PriorityUrgencyLevel/text()"" />
              </PriorityUrgencyLevel>
            </xsl:if>
            <xsl:value-of select=""./text()"" />
          </DeviationMessageVersion>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetDirectJourneysBetweenStopsResult"">
        <xsl:for-each select=""s0:diffgram/GetDirectJourneysBetweenStopsMethod/DeviationMessageVariant"">
          <DeviationMessageVariant>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""IsPartOfDeviationMessageId"">
              <IsPartOfDeviationMessageId>
                <xsl:value-of select=""IsPartOfDeviationMessageId/text()"" />
              </IsPartOfDeviationMessageId>
            </xsl:if>
            <xsl:if test=""Content"">
              <Content>
                <xsl:value-of select=""Content/text()"" />
              </Content>
            </xsl:if>
            <xsl:if test=""ContentTypeLongCode"">
              <ContentTypeLongCode>
                <xsl:value-of select=""ContentTypeLongCode/text()"" />
              </ContentTypeLongCode>
            </xsl:if>
            <xsl:if test=""UsageTypeLongCode"">
              <UsageTypeLongCode>
                <xsl:value-of select=""UsageTypeLongCode/text()"" />
              </UsageTypeLongCode>
            </xsl:if>
            <xsl:if test=""LanguageCode"">
              <LanguageCode>
                <xsl:value-of select=""LanguageCode/text()"" />
              </LanguageCode>
            </xsl:if>
            <xsl:value-of select=""./text()"" />
          </DeviationMessageVariant>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetDirectJourneysBetweenStopsResult"">
        <xsl:for-each select=""s0:diffgram/GetDirectJourneysBetweenStopsMethod/ServiceJourneyDeviation"">
          <ServiceJourneyDeviation>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""IsOnDatedVehicleJourneyId"">
              <IsOnDatedVehicleJourneyId>
                <xsl:value-of select=""IsOnDatedVehicleJourneyId/text()"" />
              </IsOnDatedVehicleJourneyId>
            </xsl:if>
            <xsl:if test=""HasDeviationMessageVersionId"">
              <HasDeviationMessageVersionId>
                <xsl:value-of select=""HasDeviationMessageVersionId/text()"" />
              </HasDeviationMessageVersionId>
            </xsl:if>
            <xsl:if test=""ConsequenceLongCode"">
              <ConsequenceLongCode>
                <xsl:value-of select=""ConsequenceLongCode/text()"" />
              </ConsequenceLongCode>
            </xsl:if>
            <xsl:value-of select=""./text()"" />
          </ServiceJourneyDeviation>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetDirectJourneysBetweenStopsResult"">
        <xsl:for-each select=""s0:diffgram/GetDirectJourneysBetweenStopsMethod/DepartureDeviation"">
          <DepartureDeviation>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""IsOnDepartureId"">
              <IsOnDepartureId>
                <xsl:value-of select=""IsOnDepartureId/text()"" />
              </IsOnDepartureId>
            </xsl:if>
            <xsl:if test=""AffectsLaterArrivalsYesNo"">
              <AffectsLaterArrivalsYesNo>
                <xsl:value-of select=""AffectsLaterArrivalsYesNo/text()"" />
              </AffectsLaterArrivalsYesNo>
            </xsl:if>
            <xsl:if test=""HasDeviationMessageVersionId"">
              <HasDeviationMessageVersionId>
                <xsl:value-of select=""HasDeviationMessageVersionId/text()"" />
              </HasDeviationMessageVersionId>
            </xsl:if>
            <xsl:if test=""ConsequenceLongCode"">
              <ConsequenceLongCode>
                <xsl:value-of select=""ConsequenceLongCode/text()"" />
              </ConsequenceLongCode>
            </xsl:if>
            <xsl:value-of select=""./text()"" />
          </DepartureDeviation>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetDirectJourneysBetweenStopsResult"">
        <xsl:for-each select=""s0:diffgram/GetDirectJourneysBetweenStopsMethod/ArrivalDeviation"">
          <ArrivalDeviation>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""IsOnArrivalId"">
              <IsOnArrivalId>
                <xsl:value-of select=""IsOnArrivalId/text()"" />
              </IsOnArrivalId>
            </xsl:if>
            <xsl:if test=""AffectsPreviousDeparturesYesNo"">
              <AffectsPreviousDeparturesYesNo>
                <xsl:value-of select=""AffectsPreviousDeparturesYesNo/text()"" />
              </AffectsPreviousDeparturesYesNo>
            </xsl:if>
            <xsl:if test=""HasDeviationMessageVersionId"">
              <HasDeviationMessageVersionId>
                <xsl:value-of select=""HasDeviationMessageVersionId/text()"" />
              </HasDeviationMessageVersionId>
            </xsl:if>
            <xsl:if test=""ConsequenceLongCode"">
              <ConsequenceLongCode>
                <xsl:value-of select=""ConsequenceLongCode/text()"" />
              </ConsequenceLongCode>
            </xsl:if>
            <xsl:value-of select=""./text()"" />
          </ArrivalDeviation>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetDirectJourneysBetweenStopsResult"">
        <xsl:for-each select=""s0:diffgram/GetDirectJourneysBetweenStopsMethod/TargetAudience"">
          <TargetAudience>
            <xsl:if test=""IsForDeviationMessageVersionId"">
              <IsForDeviationMessageVersionId>
                <xsl:value-of select=""IsForDeviationMessageVersionId/text()"" />
              </IsForDeviationMessageVersionId>
            </xsl:if>
            <xsl:if test=""TypeCode"">
              <TypeCode>
                <xsl:value-of select=""TypeCode/text()"" />
              </TypeCode>
            </xsl:if>
            <xsl:value-of select=""./text()"" />
          </TargetAudience>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:GetDirectJourneysBetweenStopsResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService+GetDirectJourneysBetweenStopsResponse";
        
        private const global::INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService.GetDirectJourneysBetweenStopsResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK001.PubTrans.Schemas.Internal.GetDirectJourneysBetweenStopsResponse";
        
        private const global::INTSTDK001.PubTrans.Schemas.Internal.GetDirectJourneysBetweenStopsResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK001.PubTrans.Schemas.External.StopMonitoringService.StopMonitoringService+GetDirectJourneysBetweenStopsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK001.PubTrans.Schemas.Internal.GetDirectJourneysBetweenStopsResponse";
                return _TrgSchemas;
            }
        }
    }
}
