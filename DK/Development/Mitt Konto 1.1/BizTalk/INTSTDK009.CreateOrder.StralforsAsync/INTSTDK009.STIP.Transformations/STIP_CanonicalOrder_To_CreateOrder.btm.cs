namespace INTSTDK009.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.STIP.Schemas.CanonicalOrder", typeof(global::INTSTDK009.STIP.Schemas.CanonicalOrder))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.Stralfors.Schemas.OrdersType", typeof(global::INTSTDK009.Stralfors.Schemas.OrdersType))]
    public sealed class STIP_CanonicalOrder_To_CreateOrder : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK009.CreateOrder.STIP.Schemas.CanonicalOrder"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Order"" />
  </xsl:template>
  <xsl:template match=""/s0:Order"">
    <Orders>
      <Order>
        <xsl:if test=""OrderNo"">
          <OrderNo>
            <xsl:value-of select=""OrderNo/text()"" />
          </OrderNo>
        </xsl:if>
        <xsl:if test=""OrderTime"">
          <OrderTime>
            <xsl:value-of select=""OrderTime/text()"" />
          </OrderTime>
        </xsl:if>
        <xsl:for-each select=""ShippingAddress"">
          <ShippingAddress>
            <xsl:if test=""Name"">
              <Name>
                <xsl:value-of select=""Name/text()"" />
              </Name>
            </xsl:if>
            <xsl:if test=""ContactPerson"">
              <ContactPerson>
                <xsl:value-of select=""ContactPerson/text()"" />
              </ContactPerson>
            </xsl:if>
            <xsl:if test=""CellPhone"">
              <CellPhone>
                <xsl:value-of select=""CellPhone/text()"" />
              </CellPhone>
            </xsl:if>
            <xsl:if test=""EMail"">
              <EMail>
                <xsl:value-of select=""EMail/text()"" />
              </EMail>
            </xsl:if>
            <xsl:if test=""Address1"">
              <Address1>
                <xsl:value-of select=""Address1/text()"" />
              </Address1>
            </xsl:if>
            <xsl:if test=""Address2"">
              <Address2>
                <xsl:value-of select=""Address2/text()"" />
              </Address2>
            </xsl:if>
            <xsl:if test=""PostalCode"">
              <PostalCode>
                <xsl:value-of select=""PostalCode/text()"" />
              </PostalCode>
            </xsl:if>
            <xsl:if test=""City"">
              <City>
                <xsl:value-of select=""City/text()"" />
              </City>
            </xsl:if>
            <xsl:if test=""CountryCode"">
              <CountryCode>
                <xsl:value-of select=""CountryCode/text()"" />
              </CountryCode>
            </xsl:if>
          </ShippingAddress>
        </xsl:for-each>
        <Products>
          <xsl:for-each select=""Products"">
            <xsl:for-each select=""Product"">
              <Product>
                <xsl:if test=""Reference"">
                  <Reference>
                    <xsl:value-of select=""Reference/text()"" />
                  </Reference>
                </xsl:if>
                <xsl:if test=""ProductCode"">
                  <ProductCode>
                    <xsl:value-of select=""ProductCode/text()"" />
                  </ProductCode>
                </xsl:if>
                <xsl:if test=""Name"">
                  <Name>
                    <xsl:value-of select=""Name/text()"" />
                  </Name>
                </xsl:if>
                <xsl:if test=""Qty"">
                  <Qty>
                    <xsl:value-of select=""Qty/text()"" />
                  </Qty>
                </xsl:if>
                <xsl:if test=""NameOnCard"">
                  <NameOnCard>
                    <xsl:value-of select=""NameOnCard/text()"" />
                  </NameOnCard>
                </xsl:if>
                <xsl:value-of select=""./text()"" />
              </Product>
            </xsl:for-each>
          </xsl:for-each>
        </Products>
      </Order>
    </Orders>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK009.STIP.Schemas.CanonicalOrder";
        
        private const global::INTSTDK009.STIP.Schemas.CanonicalOrder _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK009.Stralfors.Schemas.OrdersType";
        
        private const global::INTSTDK009.Stralfors.Schemas.OrdersType _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK009.STIP.Schemas.CanonicalOrder";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK009.Stralfors.Schemas.OrdersType";
                return _TrgSchemas;
            }
        }
    }
}
