namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.CompanyOrdersResponseJSON", typeof(global::INTSTDK008.Ehandel.Schemas.CompanyOrdersResponseJSON))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.GetCompanyOrdersResponse", typeof(global::INTSTDK008.STIP.Schemas.GetCompanyOrdersResponse))]
    public sealed class EHandel_CompanyOrdersResponse_To_GetCompanyOrders : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.CompanyOrdersResponseJSON"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK008/GetCompanyOrdersResponse/20141117"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:CompanyOrders"" />
  </xsl:template>
  <xsl:template match=""/s0:CompanyOrders"">
    <ns0:CompanyOrders>
      <ErrorMessage>
        <xsl:value-of select=""ErrorMessage/text()"" />
      </ErrorMessage>
      <StatusCode>
        <xsl:value-of select=""StatusCode/text()"" />
      </StatusCode>
      <xsl:for-each select=""Orders"">
        <Orders>
          <xsl:if test=""OrderNumber"">
            <OrderNumber>
              <xsl:value-of select=""OrderNumber/text()"" />
            </OrderNumber>
          </xsl:if>
          <xsl:if test=""OrderStatus"">
            <OrderStatus>
              <xsl:value-of select=""OrderStatus/text()"" />
            </OrderStatus>
          </xsl:if>
          <xsl:if test=""Currency"">
            <Currency>
              <xsl:value-of select=""Currency/text()"" />
            </Currency>
          </xsl:if>
          <xsl:if test=""Language"">
            <Language>
              <xsl:value-of select=""Language/text()"" />
            </Language>
          </xsl:if>
          <xsl:if test=""OrderDate"">
            <OrderDate>
              <xsl:value-of select=""OrderDate/text()"" />
            </OrderDate>
          </xsl:if>
          <xsl:if test=""OrderTotal"">
            <OrderTotal>
              <xsl:value-of select=""OrderTotal/text()"" />
            </OrderTotal>
          </xsl:if>
          <xsl:if test=""OrderTotalVat"">
            <OrderTotalVat>
              <xsl:value-of select=""OrderTotalVat/text()"" />
            </OrderTotalVat>
          </xsl:if>
          <xsl:if test=""OrderDiscount"">
            <OrderDiscount>
              <xsl:value-of select=""OrderDiscount/text()"" />
            </OrderDiscount>
          </xsl:if>
          <xsl:if test=""OrderDiscountVat"">
            <OrderDiscountVat>
              <xsl:value-of select=""OrderDiscountVat/text()"" />
            </OrderDiscountVat>
          </xsl:if>
          <xsl:if test=""PaymentTotal"">
            <PaymentTotal>
              <xsl:value-of select=""PaymentTotal/text()"" />
            </PaymentTotal>
          </xsl:if>
          <xsl:if test=""PaymentTotalVat"">
            <PaymentTotalVat>
              <xsl:value-of select=""PaymentTotalVat/text()"" />
            </PaymentTotalVat>
          </xsl:if>
          <xsl:if test=""ShippingTotal"">
            <ShippingTotal>
              <xsl:value-of select=""ShippingTotal/text()"" />
            </ShippingTotal>
          </xsl:if>
          <xsl:if test=""ShippingTotalVat"">
            <ShippingTotalVat>
              <xsl:value-of select=""ShippingTotalVat/text()"" />
            </ShippingTotalVat>
          </xsl:if>
          <xsl:if test=""CustomerNumber"">
            <CustomerNumber>
              <xsl:value-of select=""CustomerNumber/text()"" />
            </CustomerNumber>
          </xsl:if>
          <xsl:if test=""CustomerNumber2"">
            <CustomerNumber2>
              <xsl:value-of select=""CustomerNumber2/text()"" />
            </CustomerNumber2>
          </xsl:if>
          <xsl:if test=""CustomerOrganizationNumber"">
            <CustomerOrganizationNumber>
              <xsl:value-of select=""CustomerOrganizationNumber/text()"" />
            </CustomerOrganizationNumber>
          </xsl:if>
          <xsl:if test=""CustomerName"">
            <CustomerName>
              <xsl:value-of select=""CustomerName/text()"" />
            </CustomerName>
          </xsl:if>
          <xsl:for-each select=""OrderItems"">
            <OrderItems>
              <xsl:if test=""Code"">
                <Code>
                  <xsl:value-of select=""Code/text()"" />
                </Code>
              </xsl:if>
              <xsl:if test=""Name"">
                <Name>
                  <xsl:value-of select=""Name/text()"" />
                </Name>
              </xsl:if>
              <xsl:if test=""CardNumber"">
                <CardNumber>
                  <xsl:value-of select=""CardNumber/text()"" />
                </CardNumber>
              </xsl:if>
              <xsl:if test=""Amount"">
                <Amount>
                  <xsl:value-of select=""Amount/text()"" />
                </Amount>
              </xsl:if>
              <xsl:if test=""Price"">
                <Price>
                  <xsl:value-of select=""Price/text()"" />
                </Price>
              </xsl:if>
              <xsl:if test=""Tax"">
                <Tax>
                  <xsl:value-of select=""Tax/text()"" />
                </Tax>
              </xsl:if>
              <xsl:if test=""Discount"">
                <Discount>
                  <xsl:value-of select=""Discount/text()"" />
                </Discount>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </OrderItems>
          </xsl:for-each>
          <xsl:for-each select=""PaymentTypes"">
            <PaymentTypes>
              <xsl:if test=""Code"">
                <Code>
                  <xsl:value-of select=""Code/text()"" />
                </Code>
              </xsl:if>
              <xsl:if test=""Name"">
                <Name>
                  <xsl:value-of select=""Name/text()"" />
                </Name>
              </xsl:if>
              <xsl:if test=""ReferenceNumber"">
                <ReferenceNumber>
                  <xsl:value-of select=""ReferenceNumber/text()"" />
                </ReferenceNumber>
              </xsl:if>
              <xsl:if test=""Sum"">
                <Sum>
                  <xsl:value-of select=""Sum/text()"" />
                </Sum>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </PaymentTypes>
          </xsl:for-each>
        </Orders>
      </xsl:for-each>
    </ns0:CompanyOrders>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.CompanyOrdersResponseJSON";
        
        private const global::INTSTDK008.Ehandel.Schemas.CompanyOrdersResponseJSON _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.GetCompanyOrdersResponse";
        
        private const global::INTSTDK008.STIP.Schemas.GetCompanyOrdersResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.CompanyOrdersResponseJSON";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.GetCompanyOrdersResponse";
                return _TrgSchemas;
            }
        }
    }
}
