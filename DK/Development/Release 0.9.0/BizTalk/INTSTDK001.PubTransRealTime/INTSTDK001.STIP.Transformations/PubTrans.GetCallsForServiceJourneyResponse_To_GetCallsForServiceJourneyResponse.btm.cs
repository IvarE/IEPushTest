namespace INTSTDK001.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService+GetCallsforServiceJourneyResponse", typeof(global::INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService.GetCallsforServiceJourneyResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK001.STIP.Schemas.GetCallsForServiceJourneyResponse", typeof(global::INTSTDK001.STIP.Schemas.GetCallsForServiceJourneyResponse))]
    public sealed class PubTrans_GetCallsForServiceJourneyResponse_To_GetCallsForServiceJourneyResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s2 s1"" version=""1.0"" xmlns:ns0=""http://wwww.skanetrafiken.com/DK/INTSTDK001/GetCallsForServiceJourneyResponse/20141020"" xmlns:s0=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:s2=""urn:schemas-microsoft-com:xml-msdata"" xmlns:s1=""http://schemas.pubtrans.hogia.se/webservices/dataset/2008/11/VehicleMonitoringService"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:GetCallsforServiceJourneyResponse"" />
  </xsl:template>
  <xsl:template match=""/s1:GetCallsforServiceJourneyResponse"">
    <ns0:GetCallsforServiceJourneyMethod>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/DatedServiceJourney"">
          <DatedServiceJourney>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""DatedVehicleJourneyId"">
              <DatedVehicleJourneyId>
                <xsl:value-of select=""DatedVehicleJourneyId/text()"" />
              </DatedVehicleJourneyId>
            </xsl:if>
            <xsl:if test=""IsDatedVehicleJourneyId"">
              <IsDatedVehicleJourneyId>
                <xsl:value-of select=""IsDatedVehicleJourneyId/text()"" />
              </IsDatedVehicleJourneyId>
            </xsl:if>
            <xsl:if test=""OperatingDayDate"">
              <OperatingDayDate>
                <xsl:value-of select=""OperatingDayDate/text()"" />
              </OperatingDayDate>
            </xsl:if>
            <xsl:if test=""Gid"">
              <Gid>
                <xsl:value-of select=""Gid/text()"" />
              </Gid>
            </xsl:if>
            <xsl:if test=""IsWorkedOnDirectionOfLineGid"">
              <IsWorkedOnDirectionOfLineGid>
                <xsl:value-of select=""IsWorkedOnDirectionOfLineGid/text()"" />
              </IsWorkedOnDirectionOfLineGid>
            </xsl:if>
            <xsl:if test=""TransportModeCode"">
              <TransportModeCode>
                <xsl:value-of select=""TransportModeCode/text()"" />
              </TransportModeCode>
            </xsl:if>
            <xsl:if test=""LineDesignation"">
              <LineDesignation>
                <xsl:value-of select=""LineDesignation/text()"" />
              </LineDesignation>
            </xsl:if>
            <xsl:if test=""TransportAuthorityCode"">
              <TransportAuthorityCode>
                <xsl:value-of select=""TransportAuthorityCode/text()"" />
              </TransportAuthorityCode>
            </xsl:if>
            <xsl:if test=""TransportAuthorityName"">
              <TransportAuthorityName>
                <xsl:value-of select=""TransportAuthorityName/text()"" />
              </TransportAuthorityName>
            </xsl:if>
            <xsl:if test=""ContractorCode"">
              <ContractorCode>
                <xsl:value-of select=""ContractorCode/text()"" />
              </ContractorCode>
            </xsl:if>
            <xsl:if test=""ContractorName"">
              <ContractorName>
                <xsl:value-of select=""ContractorName/text()"" />
              </ContractorName>
            </xsl:if>
            <xsl:if test=""ExpectedToBeMonitored"">
              <ExpectedToBeMonitored>
                <xsl:value-of select=""ExpectedToBeMonitored/text()"" />
              </ExpectedToBeMonitored>
            </xsl:if>
            <xsl:if test=""State"">
              <State>
                <xsl:value-of select=""State/text()"" />
              </State>
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
            <xsl:if test=""ProductCode"">
              <ProductCode>
                <xsl:value-of select=""ProductCode/text()"" />
              </ProductCode>
            </xsl:if>
          </DatedServiceJourney>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/DatedDeparture"">
          <DatedDeparture>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""IsOnDatedServiceJourneyId"">
              <IsOnDatedServiceJourneyId>
                <xsl:value-of select=""IsOnDatedServiceJourneyId/text()"" />
              </IsOnDatedServiceJourneyId>
            </xsl:if>
            <xsl:if test=""IsOnServiceJourneyId"">
              <IsOnServiceJourneyId>
                <xsl:value-of select=""IsOnServiceJourneyId/text()"" />
              </IsOnServiceJourneyId>
            </xsl:if>
            <xsl:if test=""JourneyPatternSequenceNumber"">
              <JourneyPatternSequenceNumber>
                <xsl:value-of select=""JourneyPatternSequenceNumber/text()"" />
              </JourneyPatternSequenceNumber>
            </xsl:if>
            <xsl:if test=""IsTimetabledAtJourneyPatternPointGid"">
              <IsTimetabledAtJourneyPatternPointGid>
                <xsl:value-of select=""IsTimetabledAtJourneyPatternPointGid/text()"" />
              </IsTimetabledAtJourneyPatternPointGid>
            </xsl:if>
            <xsl:if test=""IsTargetedAtJourneyPatternPointGid"">
              <IsTargetedAtJourneyPatternPointGid>
                <xsl:value-of select=""IsTargetedAtJourneyPatternPointGid/text()"" />
              </IsTargetedAtJourneyPatternPointGid>
            </xsl:if>
            <xsl:if test=""WasObservedAtJourneyPatternPointGid"">
              <WasObservedAtJourneyPatternPointGid>
                <xsl:value-of select=""WasObservedAtJourneyPatternPointGid/text()"" />
              </WasObservedAtJourneyPatternPointGid>
            </xsl:if>
            <xsl:if test=""TimetabledEarliestDateTime"">
              <TimetabledEarliestDateTime>
                <xsl:value-of select=""TimetabledEarliestDateTime/text()"" />
              </TimetabledEarliestDateTime>
            </xsl:if>
            <xsl:if test=""TargetDateTime"">
              <TargetDateTime>
                <xsl:value-of select=""TargetDateTime/text()"" />
              </TargetDateTime>
            </xsl:if>
            <xsl:if test=""EstimatedDateTime"">
              <EstimatedDateTime>
                <xsl:value-of select=""EstimatedDateTime/text()"" />
              </EstimatedDateTime>
            </xsl:if>
            <xsl:if test=""ObservedDateTime"">
              <ObservedDateTime>
                <xsl:value-of select=""ObservedDateTime/text()"" />
              </ObservedDateTime>
            </xsl:if>
            <xsl:if test=""State"">
              <State>
                <xsl:value-of select=""State/text()"" />
              </State>
            </xsl:if>
            <xsl:if test=""Type"">
              <Type>
                <xsl:value-of select=""Type/text()"" />
              </Type>
            </xsl:if>
            <xsl:if test=""ProductName"">
              <ProductName>
                <xsl:value-of select=""ProductName/text()"" />
              </ProductName>
            </xsl:if>
            <xsl:if test=""LineDesignation"">
              <LineDesignation>
                <xsl:value-of select=""LineDesignation/text()"" />
              </LineDesignation>
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
            <xsl:if test=""SecondaryDestinationType"">
              <SecondaryDestinationType>
                <xsl:value-of select=""SecondaryDestinationType/text()"" />
              </SecondaryDestinationType>
            </xsl:if>
            <xsl:if test=""SymbolName"">
              <SymbolName>
                <xsl:value-of select=""SymbolName/text()"" />
              </SymbolName>
            </xsl:if>
            <xsl:if test=""PresentationType"">
              <PresentationType>
                <xsl:value-of select=""PresentationType/text()"" />
              </PresentationType>
            </xsl:if>
          </DatedDeparture>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/DatedArrival"">
          <DatedArrival>
            <xsl:if test=""Id"">
              <Id>
                <xsl:value-of select=""Id/text()"" />
              </Id>
            </xsl:if>
            <xsl:if test=""IsOnDatedServiceJourneyId"">
              <IsOnDatedServiceJourneyId>
                <xsl:value-of select=""IsOnDatedServiceJourneyId/text()"" />
              </IsOnDatedServiceJourneyId>
            </xsl:if>
            <xsl:if test=""IsOnServiceJourneyId"">
              <IsOnServiceJourneyId>
                <xsl:value-of select=""IsOnServiceJourneyId/text()"" />
              </IsOnServiceJourneyId>
            </xsl:if>
            <xsl:if test=""JourneyPatternSequenceNumber"">
              <JourneyPatternSequenceNumber>
                <xsl:value-of select=""JourneyPatternSequenceNumber/text()"" />
              </JourneyPatternSequenceNumber>
            </xsl:if>
            <xsl:if test=""IsTimetabledAtJourneyPatternPointGid"">
              <IsTimetabledAtJourneyPatternPointGid>
                <xsl:value-of select=""IsTimetabledAtJourneyPatternPointGid/text()"" />
              </IsTimetabledAtJourneyPatternPointGid>
            </xsl:if>
            <xsl:if test=""IsTargetedAtJourneyPatternPointGid"">
              <IsTargetedAtJourneyPatternPointGid>
                <xsl:value-of select=""IsTargetedAtJourneyPatternPointGid/text()"" />
              </IsTargetedAtJourneyPatternPointGid>
            </xsl:if>
            <xsl:if test=""WasObservedAtJourneyPatternPointGid"">
              <WasObservedAtJourneyPatternPointGid>
                <xsl:value-of select=""WasObservedAtJourneyPatternPointGid/text()"" />
              </WasObservedAtJourneyPatternPointGid>
            </xsl:if>
            <xsl:if test=""TimetabledLatestDateTime"">
              <TimetabledLatestDateTime>
                <xsl:value-of select=""TimetabledLatestDateTime/text()"" />
              </TimetabledLatestDateTime>
            </xsl:if>
            <xsl:if test=""TargetDateTime"">
              <TargetDateTime>
                <xsl:value-of select=""TargetDateTime/text()"" />
              </TargetDateTime>
            </xsl:if>
            <xsl:if test=""EstimatedDateTime"">
              <EstimatedDateTime>
                <xsl:value-of select=""EstimatedDateTime/text()"" />
              </EstimatedDateTime>
            </xsl:if>
            <xsl:if test=""ObservedDateTime"">
              <ObservedDateTime>
                <xsl:value-of select=""ObservedDateTime/text()"" />
              </ObservedDateTime>
            </xsl:if>
            <xsl:if test=""State"">
              <State>
                <xsl:value-of select=""State/text()"" />
              </State>
            </xsl:if>
            <xsl:if test=""Type"">
              <Type>
                <xsl:value-of select=""Type/text()"" />
              </Type>
            </xsl:if>
            <xsl:if test=""PresentationType"">
              <PresentationType>
                <xsl:value-of select=""PresentationType/text()"" />
              </PresentationType>
            </xsl:if>
          </DatedArrival>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/DeviationMessageVersion"">
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
          </DeviationMessageVersion>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/DeviationMessageVariant"">
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
          </DeviationMessageVariant>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/ServiceJourneyDeviation"">
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
          </ServiceJourneyDeviation>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/DepartureDeviation"">
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
          </DepartureDeviation>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/ArrivalDeviation"">
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
          </ArrivalDeviation>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s1:GetCallsforServiceJourneyResult"">
        <xsl:for-each select=""s0:diffgram/GetCallsforServiceJourneyMethod/TargetAudience"">
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
          </TargetAudience>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:GetCallsforServiceJourneyMethod>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService+GetCallsforServiceJourneyResponse";
        
        private const global::INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService.GetCallsforServiceJourneyResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK001.STIP.Schemas.GetCallsForServiceJourneyResponse";
        
        private const global::INTSTDK001.STIP.Schemas.GetCallsForServiceJourneyResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK001.PubTrans.Schemas.External.VehicleMonitoringService.VehicleMonitoringService+GetCallsforServiceJourneyResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK001.STIP.Schemas.GetCallsForServiceJourneyResponse";
                return _TrgSchemas;
            }
        }
    }
}
