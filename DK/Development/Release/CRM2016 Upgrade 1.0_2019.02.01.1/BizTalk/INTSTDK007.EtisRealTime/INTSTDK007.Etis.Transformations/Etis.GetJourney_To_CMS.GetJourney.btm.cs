namespace INTSTDK007.Etis.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK._007.Etis.Schemas.External.GetJourneyType+GetJourneyResponse", typeof(global::INTSTDK._007.Etis.Schemas.External.GetJourneyType.GetJourneyResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK007.CMS.Schemas.Internal.GetJourneyType+GetJourneyResult", typeof(global::INTSTDK007.CMS.Schemas.Internal.GetJourneyType.GetJourneyResult))]
    public sealed class Etis_GetJourney_To_CMS_GetJourney : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK007/GetJourney/20141216"" xmlns:s0=""http://www.etis.fskab.se/v1.0/ETISws"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetJourneyResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:GetJourneyResponse"">
    <ns0:GetJourneyResult>
      <Code>
        <xsl:value-of select=""s0:GetJourneyResult/s0:Code/text()"" />
      </Code>
      <Message>
        <xsl:value-of select=""s0:GetJourneyResult/s0:Message/text()"" />
      </Message>
      <JourneyResultKey>
        <xsl:value-of select=""s0:GetJourneyResult/s0:JourneyResultKey/text()"" />
      </JourneyResultKey>
      <Journeys>
        <xsl:for-each select=""s0:GetJourneyResult/s0:Journeys/s0:Journey"">
          <Journey>
            <SequenceNo>
              <xsl:value-of select=""s0:SequenceNo/text()"" />
            </SequenceNo>
            <DepDateTime>
              <xsl:value-of select=""s0:DepDateTime/text()"" />
            </DepDateTime>
            <ArrDateTime>
              <xsl:value-of select=""s0:ArrDateTime/text()"" />
            </ArrDateTime>
            <DepWalkDist>
              <xsl:value-of select=""s0:DepWalkDist/text()"" />
            </DepWalkDist>
            <ArrWalkDist>
              <xsl:value-of select=""s0:ArrWalkDist/text()"" />
            </ArrWalkDist>
            <NoOfChanges>
              <xsl:value-of select=""s0:NoOfChanges/text()"" />
            </NoOfChanges>
            <Guaranteed>
              <xsl:value-of select=""s0:Guaranteed/text()"" />
            </Guaranteed>
            <CO2factor>
              <xsl:value-of select=""s0:CO2factor/text()"" />
            </CO2factor>
            <NoOfZones>
              <xsl:value-of select=""s0:NoOfZones/text()"" />
            </NoOfZones>
            <PriceZoneList>
              <xsl:value-of select=""s0:PriceZoneList/text()"" />
            </PriceZoneList>
            <xsl:if test=""s0:FareType"">
              <FareType>
                <xsl:value-of select=""s0:FareType/text()"" />
              </FareType>
            </xsl:if>
            <Prices>
              <xsl:for-each select=""s0:Prices/s0:PriceInfo"">
                <PriceInfo>
                  <PriceType>
                    <xsl:value-of select=""s0:PriceType/text()"" />
                  </PriceType>
                  <Price>
                    <xsl:value-of select=""s0:Price/text()"" />
                  </Price>
                  <VAT>
                    <xsl:value-of select=""s0:VAT/text()"" />
                  </VAT>
                  <xsl:if test=""s0:JourneyTicketKey"">
                    <JourneyTicketKey>
                      <xsl:value-of select=""s0:JourneyTicketKey/text()"" />
                    </JourneyTicketKey>
                  </xsl:if>
                  <xsl:for-each select=""s0:Counties"">
                    <Counties>
                      <xsl:for-each select=""s0:CountyInfo"">
                        <CountyInfo>
                          <CountyCode>
                            <xsl:value-of select=""s0:CountyCode/text()"" />
                          </CountyCode>
                          <Price>
                            <xsl:value-of select=""s0:Price/text()"" />
                          </Price>
                          <xsl:value-of select=""./text()"" />
                        </CountyInfo>
                      </xsl:for-each>
                      <xsl:value-of select=""./text()"" />
                    </Counties>
                  </xsl:for-each>
                  <xsl:value-of select=""./text()"" />
                </PriceInfo>
              </xsl:for-each>
              <xsl:value-of select=""s0:Prices/text()"" />
            </Prices>
            <JourneyKey>
              <xsl:value-of select=""s0:JourneyKey/text()"" />
            </JourneyKey>
            <RouteLinks>
              <xsl:for-each select=""s0:RouteLinks/s0:RouteLink"">
                <RouteLink>
                  <RouteLinkKey>
                    <xsl:value-of select=""s0:RouteLinkKey/text()"" />
                  </RouteLinkKey>
                  <DepDateTime>
                    <xsl:value-of select=""s0:DepDateTime/text()"" />
                  </DepDateTime>
                  <DepIsTimingPoint>
                    <xsl:value-of select=""s0:DepIsTimingPoint/text()"" />
                  </DepIsTimingPoint>
                  <ArrDateTime>
                    <xsl:value-of select=""s0:ArrDateTime/text()"" />
                  </ArrDateTime>
                  <ArrIsTimingPoint>
                    <xsl:value-of select=""s0:ArrIsTimingPoint/text()"" />
                  </ArrIsTimingPoint>
                  <CallTrip>
                    <xsl:value-of select=""s0:CallTrip/text()"" />
                  </CallTrip>
                  <PriceZones>
                    <xsl:for-each select=""s0:PriceZones/s0:PriceZone"">
                      <PriceZone>
                        <Id>
                          <xsl:value-of select=""s0:Id/text()"" />
                        </Id>
                        <xsl:value-of select=""./text()"" />
                      </PriceZone>
                    </xsl:for-each>
                    <xsl:value-of select=""s0:PriceZones/text()"" />
                  </PriceZones>
                  <RealTime>
                    <xsl:for-each select=""s0:RealTime/s0:RealTimeInfo"">
                      <RealTimeInfo>
                        <xsl:if test=""s0:NewDepPoint"">
                          <NewDepPoint>
                            <xsl:value-of select=""s0:NewDepPoint/text()"" />
                          </NewDepPoint>
                        </xsl:if>
                        <xsl:if test=""s0:NewArrPoint"">
                          <NewArrPoint>
                            <xsl:value-of select=""s0:NewArrPoint/text()"" />
                          </NewArrPoint>
                        </xsl:if>
                        <xsl:if test=""s0:DepTimeDeviation"">
                          <DepTimeDeviation>
                            <xsl:value-of select=""s0:DepTimeDeviation/text()"" />
                          </DepTimeDeviation>
                        </xsl:if>
                        <xsl:if test=""s0:DepDeviationAffect"">
                          <DepDeviationAffect>
                            <xsl:value-of select=""s0:DepDeviationAffect/text()"" />
                          </DepDeviationAffect>
                        </xsl:if>
                        <xsl:if test=""s0:ArrTimeDeviation"">
                          <ArrTimeDeviation>
                            <xsl:value-of select=""s0:ArrTimeDeviation/text()"" />
                          </ArrTimeDeviation>
                        </xsl:if>
                        <xsl:if test=""s0:ArrDeviationAffect"">
                          <ArrDeviationAffect>
                            <xsl:value-of select=""s0:ArrDeviationAffect/text()"" />
                          </ArrDeviationAffect>
                        </xsl:if>
                        <xsl:if test=""s0:Canceled"">
                          <Canceled>
                            <xsl:value-of select=""s0:Canceled/text()"" />
                          </Canceled>
                        </xsl:if>
                        <xsl:value-of select=""./text()"" />
                      </RealTimeInfo>
                    </xsl:for-each>
                    <xsl:for-each select=""s0:RealTime/s0:RealTimeRoadInfo"">
                      <RealTimeRoadInfo>
                        <Road>
                          <xsl:value-of select=""s0:Road/text()"" />
                        </Road>
                        <Reason>
                          <xsl:value-of select=""s0:Reason/text()"" />
                        </Reason>
                        <Info>
                          <xsl:value-of select=""s0:Info/text()"" />
                        </Info>
                        <xsl:value-of select=""./text()"" />
                      </RealTimeRoadInfo>
                    </xsl:for-each>
                    <xsl:value-of select=""s0:RealTime/text()"" />
                  </RealTime>
                  <From>
                    <Id>
                      <xsl:value-of select=""s0:From/s0:Id/text()"" />
                    </Id>
                    <Name>
                      <xsl:value-of select=""s0:From/s0:Name/text()"" />
                    </Name>
                    <StopPoint>
                      <xsl:value-of select=""s0:From/s0:StopPoint/text()"" />
                    </StopPoint>
                    <xsl:value-of select=""s0:From/text()"" />
                  </From>
                  <To>
                    <Id>
                      <xsl:value-of select=""s0:To/s0:Id/text()"" />
                    </Id>
                    <Name>
                      <xsl:value-of select=""s0:To/s0:Name/text()"" />
                    </Name>
                    <StopPoint>
                      <xsl:value-of select=""s0:To/s0:StopPoint/text()"" />
                    </StopPoint>
                    <xsl:value-of select=""s0:To/text()"" />
                  </To>
                  <Line>
                    <Name>
                      <xsl:value-of select=""s0:Line/s0:Name/text()"" />
                    </Name>
                    <xsl:if test=""s0:Line/s0:No"">
                      <No>
                        <xsl:value-of select=""s0:Line/s0:No/text()"" />
                      </No>
                    </xsl:if>
                    <xsl:if test=""s0:Line/s0:RunNo"">
                      <RunNo>
                        <xsl:value-of select=""s0:Line/s0:RunNo/text()"" />
                      </RunNo>
                    </xsl:if>
                    <LineTypeId>
                      <xsl:value-of select=""s0:Line/s0:LineTypeId/text()"" />
                    </LineTypeId>
                    <LineTypeName>
                      <xsl:value-of select=""s0:Line/s0:LineTypeName/text()"" />
                    </LineTypeName>
                    <TransportModeId>
                      <xsl:value-of select=""s0:Line/s0:TransportModeId/text()"" />
                    </TransportModeId>
                    <TransportModeName>
                      <xsl:value-of select=""s0:Line/s0:TransportModeName/text()"" />
                    </TransportModeName>
                    <xsl:if test=""s0:Line/s0:TrainNo"">
                      <TrainNo>
                        <xsl:value-of select=""s0:Line/s0:TrainNo/text()"" />
                      </TrainNo>
                    </xsl:if>
                    <xsl:if test=""s0:Line/s0:Towards"">
                      <Towards>
                        <xsl:value-of select=""s0:Line/s0:Towards/text()"" />
                      </Towards>
                    </xsl:if>
                    <xsl:if test=""s0:Line/s0:OperatorId"">
                      <OperatorId>
                        <xsl:value-of select=""s0:Line/s0:OperatorId/text()"" />
                      </OperatorId>
                    </xsl:if>
                    <xsl:if test=""s0:Line/s0:OperatorName"">
                      <OperatorName>
                        <xsl:value-of select=""s0:Line/s0:OperatorName/text()"" />
                      </OperatorName>
                    </xsl:if>
                    <FootNotes>
                      <xsl:for-each select=""s0:Line/s0:FootNotes/s0:FootNote"">
                        <FootNote>
                          <Index>
                            <xsl:value-of select=""s0:Index/text()"" />
                          </Index>
                          <Text>
                            <xsl:value-of select=""s0:Text/text()"" />
                          </Text>
                          <xsl:value-of select=""./text()"" />
                        </FootNote>
                      </xsl:for-each>
                      <xsl:value-of select=""s0:Line/s0:FootNotes/text()"" />
                    </FootNotes>
                    <xsl:for-each select=""s0:Line/s0:PointsOnRouteLink"">
                      <PointsOnRouteLink>
                        <xsl:for-each select=""s0:PointOnRouteLink"">
                          <PointOnRouteLink>
                            <Id>
                              <xsl:value-of select=""s0:Id/text()"" />
                            </Id>
                            <Name>
                              <xsl:value-of select=""s0:Name/text()"" />
                            </Name>
                            <StopPoint>
                              <xsl:value-of select=""s0:StopPoint/text()"" />
                            </StopPoint>
                            <ArrDateTime>
                              <xsl:value-of select=""s0:ArrDateTime/text()"" />
                            </ArrDateTime>
                            <ArrIsTimingPoint>
                              <xsl:value-of select=""s0:ArrIsTimingPoint/text()"" />
                            </ArrIsTimingPoint>
                            <xsl:if test=""s0:Distance"">
                              <Distance>
                                <xsl:value-of select=""s0:Distance/text()"" />
                              </Distance>
                            </xsl:if>
                            <xsl:value-of select=""./text()"" />
                          </PointOnRouteLink>
                        </xsl:for-each>
                        <xsl:value-of select=""./text()"" />
                      </PointsOnRouteLink>
                    </xsl:for-each>
                    <xsl:if test=""s0:Line/s0:Provider"">
                      <Provider>
                        <xsl:value-of select=""s0:Line/s0:Provider/text()"" />
                      </Provider>
                    </xsl:if>
                    <xsl:value-of select=""s0:Line/text()"" />
                  </Line>
                  <Deviations>
                    <xsl:for-each select=""s0:Deviations/s0:Deviation"">
                      <Deviation>
                        <DeviationScopes>
                          <xsl:for-each select=""s0:DeviationScopes/s0:DeviationScope"">
                            <DeviationScope>
                              <xsl:for-each select=""s0:ScopeAttribute"">
                                <ScopeAttribute>
                                  <xsl:value-of select=""./text()"" />
                                </ScopeAttribute>
                              </xsl:for-each>
                              <FromDateTime>
                                <xsl:value-of select=""s0:FromDateTime/text()"" />
                              </FromDateTime>
                              <ToDateTime>
                                <xsl:value-of select=""s0:ToDateTime/text()"" />
                              </ToDateTime>
                              <xsl:value-of select=""./text()"" />
                            </DeviationScope>
                          </xsl:for-each>
                          <xsl:value-of select=""s0:DeviationScopes/text()"" />
                        </DeviationScopes>
                        <PublicNote>
                          <xsl:value-of select=""s0:PublicNote/text()"" />
                        </PublicNote>
                        <Header>
                          <xsl:value-of select=""s0:Header/text()"" />
                        </Header>
                        <Details>
                          <xsl:value-of select=""s0:Details/text()"" />
                        </Details>
                        <Summary>
                          <xsl:value-of select=""s0:Summary/text()"" />
                        </Summary>
                        <ShortText>
                          <xsl:value-of select=""s0:ShortText/text()"" />
                        </ShortText>
                        <Importance>
                          <xsl:value-of select=""s0:Importance/text()"" />
                        </Importance>
                        <Influence>
                          <xsl:value-of select=""s0:Influence/text()"" />
                        </Influence>
                        <Urgency>
                          <xsl:value-of select=""s0:Urgency/text()"" />
                        </Urgency>
                        <WebLinks>
                          <xsl:for-each select=""s0:WebLinks/s0:WebLink"">
                            <WebLink>
                              <URL>
                                <xsl:value-of select=""s0:URL/text()"" />
                              </URL>
                              <xsl:value-of select=""./text()"" />
                            </WebLink>
                          </xsl:for-each>
                          <xsl:value-of select=""s0:WebLinks/text()"" />
                        </WebLinks>
                        <xsl:value-of select=""./text()"" />
                      </Deviation>
                    </xsl:for-each>
                    <xsl:value-of select=""s0:Deviations/text()"" />
                  </Deviations>
                  <xsl:if test=""s0:Accessibility"">
                    <Accessibility>
                      <xsl:value-of select=""s0:Accessibility/text()"" />
                    </Accessibility>
                  </xsl:if>
                  <xsl:value-of select=""./text()"" />
                </RouteLink>
              </xsl:for-each>
              <xsl:value-of select=""s0:RouteLinks/text()"" />
            </RouteLinks>
            <Distance>
              <xsl:value-of select=""s0:Distance/text()"" />
            </Distance>
            <CO2value>
              <xsl:value-of select=""s0:CO2value/text()"" />
            </CO2value>
            <xsl:if test=""s0:Recalculated"">
              <Recalculated>
                <xsl:value-of select=""s0:Recalculated/text()"" />
              </Recalculated>
            </xsl:if>
            <xsl:if test=""s0:OriginalTime"">
              <OriginalTime>
                <xsl:value-of select=""s0:OriginalTime/text()"" />
              </OriginalTime>
            </xsl:if>
            <SalesRestriction>
              <xsl:value-of select=""s0:SalesRestriction/text()"" />
            </SalesRestriction>
            <PriceZoneNamesList>
              <xsl:value-of select=""s0:PriceZoneNamesList/text()"" />
            </PriceZoneNamesList>
            <StartEndBigZoneList>
              <xsl:value-of select=""s0:StartEndBigZoneList/text()"" />
            </StartEndBigZoneList>
            <xsl:value-of select=""./text()"" />
          </Journey>
        </xsl:for-each>
        <xsl:value-of select=""s0:GetJourneyResult/s0:Journeys/text()"" />
      </Journeys>
    </ns0:GetJourneyResult>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK._007.Etis.Schemas.External.GetJourneyType+GetJourneyResponse";
        
        private const global::INTSTDK._007.Etis.Schemas.External.GetJourneyType.GetJourneyResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK007.CMS.Schemas.Internal.GetJourneyType+GetJourneyResult";
        
        private const global::INTSTDK007.CMS.Schemas.Internal.GetJourneyType.GetJourneyResult _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK._007.Etis.Schemas.External.GetJourneyType+GetJourneyResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK007.CMS.Schemas.Internal.GetJourneyType+GetJourneyResult";
                return _TrgSchemas;
            }
        }
    }
}
