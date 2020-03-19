namespace INTSTDK004.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse", typeof(global::INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK004.STIP.Schemas.GetCardTransactionsResponseType", typeof(global::INTSTDK004.STIP.Schemas.GetCardTransactionsResponseType))]
    public sealed class Biff_GetCardTransactionsRepsonse_To_GetCardTransactionsResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s2 s3 s1 s0"" version=""1.0"" xmlns:s0=""urn:schemas-microsoft-com:xml-msdata"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK004/CardTransactions/20141216"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK004/GetCardTransactionResponse/20141216"" xmlns:s1=""urn:schemas-microsoft-com:xml-diffgram-v1"" xmlns:s2=""http://Biff.com/CardTransactions.xsd"" xmlns:s3=""http://cubic.com"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetCardTransactionsResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:GetCardTransactionsResponse"">
    <ns1:GetCardTransactionsResponse>
      <GetCardTransactionsResult>
        <ns0:CardTransactions>
          <xsl:for-each select=""s3:GetCardTransactionsResult"">
            <xsl:for-each select=""s1:diffgram/s2:CardTransactions/s2:Transactions"">
              <ns0:Transactions>
                <xsl:if test=""s2:Date"">
                  <ns0:Date>
                    <xsl:value-of select=""s2:Date/text()"" />
                  </ns0:Date>
                </xsl:if>
                <xsl:if test=""s2:DeviceID"">
                  <ns0:DeviceID>
                    <xsl:value-of select=""s2:DeviceID/text()"" />
                  </ns0:DeviceID>
                </xsl:if>
                <xsl:if test=""s2:TxnNum"">
                  <ns0:TxnNum>
                    <xsl:value-of select=""s2:TxnNum/text()"" />
                  </ns0:TxnNum>
                </xsl:if>
                <xsl:if test=""s2:CardSect"">
                  <ns0:CardSect>
                    <xsl:value-of select=""s2:CardSect/text()"" />
                  </ns0:CardSect>
                </xsl:if>
                <xsl:if test=""s2:RecType"">
                  <ns0:RecType>
                    <xsl:value-of select=""s2:RecType/text()"" />
                  </ns0:RecType>
                </xsl:if>
                <xsl:if test=""s2:TxnType"">
                  <ns0:TxnType>
                    <xsl:value-of select=""s2:TxnType/text()"" />
                  </ns0:TxnType>
                </xsl:if>
                <xsl:if test=""s2:Route"">
                  <ns0:Route>
                    <xsl:value-of select=""s2:Route/text()"" />
                  </ns0:Route>
                </xsl:if>
                <xsl:if test=""s2:Currency"">
                  <ns0:Currency>
                    <xsl:value-of select=""s2:Currency/text()"" />
                  </ns0:Currency>
                </xsl:if>
                <xsl:if test=""s2:Amount"">
                  <ns0:Amount>
                    <xsl:value-of select=""s2:Amount/text()"" />
                  </ns0:Amount>
                </xsl:if>
                <xsl:if test=""s2:OrigZone"">
                  <ns0:OrigZone>
                    <xsl:value-of select=""s2:OrigZone/text()"" />
                  </ns0:OrigZone>
                </xsl:if>
              </ns0:Transactions>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""s3:GetCardTransactionsResult"">
            <xsl:for-each select=""s1:diffgram/s2:CardTransactions/s2:CardDetails"">
              <ns0:CardDetails>
                <xsl:if test=""s2:CardSerialNumber"">
                  <ns0:CardSerialNumber>
                    <xsl:value-of select=""s2:CardSerialNumber/text()"" />
                  </ns0:CardSerialNumber>
                </xsl:if>
                <xsl:if test=""s2:NumTxnsAvailable"">
                  <ns0:NumTxnsAvailable>
                    <xsl:value-of select=""s2:NumTxnsAvailable/text()"" />
                  </ns0:NumTxnsAvailable>
                </xsl:if>
              </ns0:CardDetails>
            </xsl:for-each>
          </xsl:for-each>
          <xsl:for-each select=""s3:GetCardTransactionsResult"">
            <xsl:for-each select=""s1:diffgram/s2:CardTransactions/s2:ZoneList"">
              <ns0:ZoneList>
                <xsl:if test=""s2:ZoneListID"">
                  <ns0:ZoneListID>
                    <xsl:value-of select=""s2:ZoneListID/text()"" />
                  </ns0:ZoneListID>
                </xsl:if>
                <xsl:if test=""s2:Zone"">
                  <ns0:Zone>
                    <xsl:value-of select=""s2:Zone/text()"" />
                  </ns0:Zone>
                </xsl:if>
              </ns0:ZoneList>
            </xsl:for-each>
          </xsl:for-each>
        </ns0:CardTransactions>
      </GetCardTransactionsResult>
    </ns1:GetCardTransactionsResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse";
        
        private const global::INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK004.STIP.Schemas.GetCardTransactionsResponseType";
        
        private const global::INTSTDK004.STIP.Schemas.GetCardTransactionsResponseType _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK004.Biff.Schemas.External.GetCardTransactionsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK004.STIP.Schemas.GetCardTransactionsResponseType";
                return _TrgSchemas;
            }
        }
    }
}
