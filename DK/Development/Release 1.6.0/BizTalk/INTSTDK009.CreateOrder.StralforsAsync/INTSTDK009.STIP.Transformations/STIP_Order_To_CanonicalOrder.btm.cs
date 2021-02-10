namespace INTSTDK009.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.STIP.Schemas.Order", typeof(global::INTSTDK009.STIP.Schemas.Order))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK009.STIP.Schemas.CanonicalOrder", typeof(global::INTSTDK009.STIP.Schemas.CanonicalOrder))]
    public sealed class STIP_Order_To_CanonicalOrder : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://INTSTDK009.CreateOrder.STIP.Schemas.CanonicalOrder"" xmlns:s0=""http://INTSTDK009.CreateOrder.STIP.Schemas.Order"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Order"" />
  </xsl:template>
  <xsl:template match=""/s0:Order"">
    <xsl:variable name=""var:v1"" select=""./Product[1]/ProductCode/text()"" />
    <ns0:Order>
      <RoutingCode>
        <xsl:value-of select=""$var:v1"" />
      </RoutingCode>
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
      <ShippingAddress>
        <xsl:if test=""ShippingAddress/Name"">
          <Name>
            <xsl:value-of select=""ShippingAddress/Name/text()"" />
          </Name>
        </xsl:if>
        <xsl:if test=""ShippingAddress/ContactPerson"">
          <ContactPerson>
            <xsl:value-of select=""ShippingAddress/ContactPerson/text()"" />
          </ContactPerson>
        </xsl:if>
        <xsl:if test=""ShippingAddress/CellPhone"">
          <CellPhone>
            <xsl:value-of select=""ShippingAddress/CellPhone/text()"" />
          </CellPhone>
        </xsl:if>
        <xsl:if test=""ShippingAddress/EMail"">
          <EMail>
            <xsl:value-of select=""ShippingAddress/EMail/text()"" />
          </EMail>
        </xsl:if>
        <xsl:if test=""ShippingAddress/Address1"">
          <Address1>
            <xsl:value-of select=""ShippingAddress/Address1/text()"" />
          </Address1>
        </xsl:if>
        <xsl:if test=""ShippingAddress/Address2"">
          <Address2>
            <xsl:value-of select=""ShippingAddress/Address2/text()"" />
          </Address2>
        </xsl:if>
        <xsl:if test=""ShippingAddress/PostalCode"">
          <PostalCode>
            <xsl:value-of select=""ShippingAddress/PostalCode/text()"" />
          </PostalCode>
        </xsl:if>
        <xsl:if test=""ShippingAddress/City"">
          <City>
            <xsl:value-of select=""ShippingAddress/City/text()"" />
          </City>
        </xsl:if>
        <xsl:if test=""ShippingAddress/CountryCode"">
          <CountryCode>
            <xsl:value-of select=""ShippingAddress/CountryCode/text()"" />
          </CountryCode>
        </xsl:if>
      </ShippingAddress>
      <Products>
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
      </Products>
      <xsl:value-of select=""./text()"" />
    </ns0:Order>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK009.STIP.Schemas.Order";
        
        private const global::INTSTDK009.STIP.Schemas.Order _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK009.STIP.Schemas.CanonicalOrder";
        
        private const global::INTSTDK009.STIP.Schemas.CanonicalOrder _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK009.STIP.Schemas.Order";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK009.STIP.Schemas.CanonicalOrder";
                return _TrgSchemas;
            }
        }
    }
}
