namespace INTSTDK004.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetCardDetails2ResponseType", typeof(global::INTSTDK004.Biff.Schemas.External.GetCardDetails2ResponseType))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.STIP.Schemas.GetCardDetails2ResponseType", typeof(global::INTSTDK004.STIP.Schemas.GetCardDetails2ResponseType))]
    public sealed class Biff_GetCardDetails2Response_To_GetCardDetails2Response : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s2 s1 s3 s0"" version=""1.0"" xmlns:s0=""urn:schemas-microsoft-com:xml-msdata"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"" xmlns:s1=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216"" xmlns:s3=""http://Biff.com/CardDetails2.xsd"" xmlns:s2=""http://cubic.com"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s2:GetCardDetails2Response"" />
  </xsl:template>
  <xsl:template match=""/s2:GetCardDetails2Response"">
    <ns1:GetCardDetails2Response>
      <ns1:GetCardDetails2Result>
        <ns0:CardDetails2>
          <xsl:for-each select=""s2:GetCardDetails2Result"">
            <xsl:for-each select=""s1:diffgram/s3:CardDetails2/s3:CardInformation"">
              <ns0:CardInformation>
                <xsl:if test=""s3:CardNumber"">
                  <ns0:CardNumber>
                    <xsl:value-of select=""s3:CardNumber/text()"" />
                  </ns0:CardNumber>
                </xsl:if>
                <xsl:if test=""s3:CardIssuer"">
                  <ns0:CardIssuer>
                    <xsl:value-of select=""s3:CardIssuer/text()"" />
                  </ns0:CardIssuer>
                </xsl:if>
                <xsl:if test=""s3:CardKind"">
                  <ns0:CardKind>
                    <xsl:value-of select=""s3:CardKind/text()"" />
                  </ns0:CardKind>
                </xsl:if>
                <xsl:if test=""s3:CardHotlisted"">
                  <ns0:CardHotlisted>
                    <xsl:value-of select=""s3:CardHotlisted/text()"" />
                  </ns0:CardHotlisted>
                </xsl:if>
                <xsl:if test=""s3:CardReplByCardNumber"">
                  <ns0:CardReplByCardNumber>
                    <xsl:value-of select=""s3:CardReplByCardNumber/text()"" />
                  </ns0:CardReplByCardNumber>
                </xsl:if>
                <xsl:if test=""s3:CardReplaces"">
                  <ns0:CardReplaces>
                    <xsl:value-of select=""s3:CardReplaces/text()"" />
                  </ns0:CardReplaces>
                </xsl:if>
                <xsl:if test=""s3:CardTypePeriod"">
                  <ns0:CardTypePeriod>
                    <xsl:value-of select=""s3:CardTypePeriod/text()"" />
                  </ns0:CardTypePeriod>
                </xsl:if>
                <xsl:if test=""s3:CardTypeValue"">
                  <ns0:CardTypeValue>
                    <xsl:value-of select=""s3:CardTypeValue/text()"" />
                  </ns0:CardTypeValue>
                </xsl:if>
                <xsl:if test=""s3:CardValueProductType"">
                  <ns0:CardValueProductType>
                    <xsl:value-of select=""s3:CardValueProductType/text()"" />
                  </ns0:CardValueProductType>
                </xsl:if>
                <xsl:value-of select=""./text()"" />
              </ns0:CardInformation>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""s2:GetCardDetails2Result"">
            <xsl:for-each select=""s1:diffgram/s3:CardDetails2/s3:PurseDetails"">
              <ns0:PurseDetails>
                <xsl:if test=""s3:CardCategory"">
                  <ns0:CardCategory>
                    <xsl:value-of select=""s3:CardCategory/text()"" />
                  </ns0:CardCategory>
                </xsl:if>
                <xsl:if test=""s3:Balance"">
                  <ns0:Balance>
                    <xsl:value-of select=""s3:Balance/text()"" />
                  </ns0:Balance>
                </xsl:if>
                <xsl:if test=""s3:Currency"">
                  <ns0:Currency>
                    <xsl:value-of select=""s3:Currency/text()"" />
                  </ns0:Currency>
                </xsl:if>
                <xsl:if test=""s3:OutstandingDirectedAutoload"">
                  <ns0:OutstandingDirectedAutoload>
                    <xsl:value-of select=""s3:OutstandingDirectedAutoload/text()"" />
                  </ns0:OutstandingDirectedAutoload>
                </xsl:if>
                <xsl:if test=""s3:OutstandingEnableThresholdAutoload"">
                  <ns0:OutstandingEnableThresholdAutoload>
                    <xsl:value-of select=""s3:OutstandingEnableThresholdAutoload/text()"" />
                  </ns0:OutstandingEnableThresholdAutoload>
                </xsl:if>
                <xsl:if test=""s3:Hotlisted"">
                  <ns0:Hotlisted>
                    <xsl:value-of select=""s3:Hotlisted/text()"" />
                  </ns0:Hotlisted>
                </xsl:if>
                <xsl:if test=""s3:HotlistReason"">
                  <ns0:HotlistReason>
                    <xsl:value-of select=""s3:HotlistReason/text()"" />
                  </ns0:HotlistReason>
                </xsl:if>
                <xsl:value-of select=""./text()"" />
              </ns0:PurseDetails>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""s2:GetCardDetails2Result"">
            <xsl:for-each select=""s1:diffgram/s3:CardDetails2/s3:PeriodDetails"">
              <ns0:PeriodDetails>
                <xsl:if test=""s3:CardCategory"">
                  <ns0:CardCategory>
                    <xsl:value-of select=""s3:CardCategory/text()"" />
                  </ns0:CardCategory>
                </xsl:if>
                <xsl:if test=""s3:ProductType"">
                  <ns0:ProductType>
                    <xsl:value-of select=""s3:ProductType/text()"" />
                  </ns0:ProductType>
                </xsl:if>
                <xsl:if test=""s3:PeriodStart"">
                  <ns0:PeriodStart>
                    <xsl:value-of select=""s3:PeriodStart/text()"" />
                  </ns0:PeriodStart>
                </xsl:if>
                <xsl:if test=""s3:PeriodEnd"">
                  <ns0:PeriodEnd>
                    <xsl:value-of select=""s3:PeriodEnd/text()"" />
                  </ns0:PeriodEnd>
                </xsl:if>
                <xsl:if test=""s3:WaitingPeriods"">
                  <ns0:WaitingPeriods>
                    <xsl:value-of select=""s3:WaitingPeriods/text()"" />
                  </ns0:WaitingPeriods>
                </xsl:if>
                <xsl:if test=""s3:TravelsInPeriod"">
                  <ns0:TravelsInPeriod>
                    <xsl:value-of select=""s3:TravelsInPeriod/text()"" />
                  </ns0:TravelsInPeriod>
                </xsl:if>
                <xsl:if test=""s3:ZoneListID"">
                  <ns0:ZoneListID>
                    <xsl:value-of select=""s3:ZoneListID/text()"" />
                  </ns0:ZoneListID>
                </xsl:if>
                <xsl:if test=""s3:RouteListID"">
                  <ns0:RouteListID>
                    <xsl:value-of select=""s3:RouteListID/text()"" />
                  </ns0:RouteListID>
                </xsl:if>
                <xsl:if test=""s3:PricePaid"">
                  <ns0:PricePaid>
                    <xsl:value-of select=""s3:PricePaid/text()"" />
                  </ns0:PricePaid>
                </xsl:if>
                <xsl:if test=""s3:Currency"">
                  <ns0:Currency>
                    <xsl:value-of select=""s3:Currency/text()"" />
                  </ns0:Currency>
                </xsl:if>
                <xsl:if test=""s3:OutstandingDirectedAutoload"">
                  <ns0:OutstandingDirectedAutoload>
                    <xsl:value-of select=""s3:OutstandingDirectedAutoload/text()"" />
                  </ns0:OutstandingDirectedAutoload>
                </xsl:if>
                <xsl:if test=""s3:OutstandingEnableThresholdAutoload"">
                  <ns0:OutstandingEnableThresholdAutoload>
                    <xsl:value-of select=""s3:OutstandingEnableThresholdAutoload/text()"" />
                  </ns0:OutstandingEnableThresholdAutoload>
                </xsl:if>
                <xsl:if test=""s3:Hotlisted"">
                  <ns0:Hotlisted>
                    <xsl:value-of select=""s3:Hotlisted/text()"" />
                  </ns0:Hotlisted>
                </xsl:if>
                <xsl:if test=""s3:HotlistReason"">
                  <ns0:HotlistReason>
                    <xsl:value-of select=""s3:HotlistReason/text()"" />
                  </ns0:HotlistReason>
                </xsl:if>
                <ns0:ContractSerialNumber>
                  <xsl:value-of select=""s3:ContractSerialNumber/text()"" />
                </ns0:ContractSerialNumber>
                <xsl:value-of select=""./text()"" />
              </ns0:PeriodDetails>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""s2:GetCardDetails2Result"">
            <xsl:for-each select=""s1:diffgram/s3:CardDetails2/s3:ZoneLists"">
              <ns0:ZoneLists>
                <ns0:ZoneListID>
                  <xsl:value-of select=""s3:ZoneListID/text()"" />
                </ns0:ZoneListID>
                <ns0:Zone>
                  <xsl:value-of select=""s3:Zone/text()"" />
                </ns0:Zone>
                <xsl:value-of select=""./text()"" />
              </ns0:ZoneLists>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""s2:GetCardDetails2Result"">
            <xsl:for-each select=""s1:diffgram/s3:CardDetails2/s3:RouteLists"">
              <ns0:RouteLists>
                <ns0:RouteListID>
                  <xsl:value-of select=""s3:RouteListID/text()"" />
                </ns0:RouteListID>
                <ns0:Route>
                  <xsl:value-of select=""s3:Route/text()"" />
                </ns0:Route>
                <ns0:FromZone>
                  <xsl:value-of select=""s3:FromZone/text()"" />
                </ns0:FromZone>
                <ns0:ToZone>
                  <xsl:value-of select=""s3:ToZone/text()"" />
                </ns0:ToZone>
                <xsl:value-of select=""./text()"" />
              </ns0:RouteLists>
            </xsl:for-each>
          </xsl:for-each>
        </ns0:CardDetails2>
      </ns1:GetCardDetails2Result>
    </ns1:GetCardDetails2Response>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK004.Biff.Schemas.External.GetCardDetails2ResponseType";
        
        private const global::INTSTDK004.Biff.Schemas.External.GetCardDetails2ResponseType _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK004.STIP.Schemas.GetCardDetails2ResponseType";
        
        private const global::INTSTDK004.STIP.Schemas.GetCardDetails2ResponseType _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK004.Biff.Schemas.External.GetCardDetails2ResponseType";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK004.STIP.Schemas.GetCardDetails2ResponseType";
                return _TrgSchemas;
            }
        }
    }
}
