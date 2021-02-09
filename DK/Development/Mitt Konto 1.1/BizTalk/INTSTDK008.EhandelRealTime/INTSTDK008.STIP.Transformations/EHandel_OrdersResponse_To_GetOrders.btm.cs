namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.OrdersResponse", typeof(global::INTSTDK008.Ehandel.Schemas.OrdersResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.GetOrders+GetOrdersRequestResponse", typeof(global::INTSTDK008.STIP.Schemas.GetOrders.GetOrdersRequestResponse))]
    public sealed class EHandel_OrdersResponse_To_GetOrders : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.OrdersResponse"" xmlns:ns0=""http://www.skanetrafiken.se/DK/INTSTDK008/GetOrders/20141031"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Orders"" />
  </xsl:template>
  <xsl:template match=""/s0:Orders"">
    <ns0:GetOrdersRequestResponse>
      <Orders>
        <xsl:for-each select=""s0:Order"">
          <Order>
            <xsl:if test=""s0:OrderNumber"">
              <OrderNumber>
                <xsl:value-of select=""s0:OrderNumber/text()"" />
              </OrderNumber>
            </xsl:if>
            <xsl:if test=""s0:OrderStatus"">
              <OrderStatus>
                <xsl:value-of select=""s0:OrderStatus/text()"" />
              </OrderStatus>
            </xsl:if>
            <xsl:if test=""s0:Currency"">
              <Currency>
                <xsl:value-of select=""s0:Currency/text()"" />
              </Currency>
            </xsl:if>
            <xsl:if test=""s0:Language"">
              <Language>
                <xsl:value-of select=""s0:Language/text()"" />
              </Language>
            </xsl:if>
            <OrderDate>
              <xsl:value-of select=""s0:OrderDate/text()"" />
            </OrderDate>
            <OrderTotal>
              <xsl:value-of select=""s0:OrderTotal/text()"" />
            </OrderTotal>
            <OrderTotalVat>
              <xsl:value-of select=""s0:OrderTotalVat/text()"" />
            </OrderTotalVat>
            <xsl:if test=""s0:OrderDiscount"">
              <OrderDiscount>
                <xsl:value-of select=""s0:OrderDiscount/text()"" />
              </OrderDiscount>
            </xsl:if>
            <xsl:if test=""s0:OrderDiscountVat"">
              <OrderDiscountVat>
                <xsl:value-of select=""s0:OrderDiscountVat/text()"" />
              </OrderDiscountVat>
            </xsl:if>
            <xsl:if test=""s0:PaymentTotal"">
              <PaymentTotal>
                <xsl:value-of select=""s0:PaymentTotal/text()"" />
              </PaymentTotal>
            </xsl:if>
            <xsl:if test=""s0:PaymentTotalVat"">
              <PaymentTotalVat>
                <xsl:value-of select=""s0:PaymentTotalVat/text()"" />
              </PaymentTotalVat>
            </xsl:if>
            <xsl:if test=""s0:ShippingTotal"">
              <ShippingTotal>
                <xsl:value-of select=""s0:ShippingTotal/text()"" />
              </ShippingTotal>
            </xsl:if>
            <xsl:if test=""s0:ShippingTotalVat"">
              <ShippingTotalVat>
                <xsl:value-of select=""s0:ShippingTotalVat/text()"" />
              </ShippingTotalVat>
            </xsl:if>
            <xsl:if test=""s0:CustomerNumber"">
              <CustomerNumber>
                <xsl:value-of select=""s0:CustomerNumber/text()"" />
              </CustomerNumber>
            </xsl:if>
            <xsl:if test=""s0:CustomerNumber2"">
              <CustomerNumber2>
                <xsl:value-of select=""s0:CustomerNumber2/text()"" />
              </CustomerNumber2>
            </xsl:if>
            <xsl:if test=""s0:CustomerOrganizationNumber"">
              <CustomerOrganizationNumber>
                <xsl:value-of select=""s0:CustomerOrganizationNumber/text()"" />
              </CustomerOrganizationNumber>
            </xsl:if>
            <xsl:if test=""s0:CustomerName"">
              <CustomerName>
                <xsl:value-of select=""s0:CustomerName/text()"" />
              </CustomerName>
            </xsl:if>
            <xsl:for-each select=""s0:OrderItems"">
              <OrderItems>
                <xsl:for-each select=""s0:OrderItem"">
                  <OrderItem>
                    <xsl:if test=""s0:Code"">
                      <Code>
                        <xsl:value-of select=""s0:Code/text()"" />
                      </Code>
                    </xsl:if>
                    <xsl:if test=""s0:Name"">
                      <Name>
                        <xsl:value-of select=""s0:Name/text()"" />
                      </Name>
                    </xsl:if>
                    <xsl:if test=""s0:CardNumber"">
                      <CardNumber>
                        <xsl:value-of select=""s0:CardNumber/text()"" />
                      </CardNumber>
                    </xsl:if>
                    <xsl:if test=""s0:Amount"">
                      <Amount>
                        <xsl:value-of select=""s0:Amount/text()"" />
                      </Amount>
                    </xsl:if>
                    <Price>
                      <xsl:value-of select=""s0:Price/text()"" />
                    </Price>
                    <xsl:if test=""s0:Tax"">
                      <Tax>
                        <xsl:value-of select=""s0:Tax/text()"" />
                      </Tax>
                    </xsl:if>
                    <Discount>
                      <xsl:value-of select=""s0:Discount/text()"" />
                    </Discount>
                    <xsl:value-of select=""./text()"" />
                  </OrderItem>
                </xsl:for-each>
                <xsl:value-of select=""./text()"" />
              </OrderItems>
            </xsl:for-each>
            <xsl:for-each select=""s0:Payments"">
              <Payments>
                <xsl:for-each select=""s0:Payment"">
                  <Payment>
                    <xsl:if test=""s0:Code"">
                      <Code>
                        <xsl:value-of select=""s0:Code/text()"" />
                      </Code>
                    </xsl:if>
                    <xsl:if test=""s0:Name"">
                      <Name>
                        <xsl:value-of select=""s0:Name/text()"" />
                      </Name>
                    </xsl:if>
                    <xsl:if test=""s0:ReferenceNumber"">
                      <ReferenceNumber>
                        <xsl:value-of select=""s0:ReferenceNumber/text()"" />
                      </ReferenceNumber>
                    </xsl:if>
                    <Sum>
                      <xsl:value-of select=""s0:Sum/text()"" />
                    </Sum>
                    <xsl:value-of select=""./text()"" />
                  </Payment>
                </xsl:for-each>
                <xsl:value-of select=""./text()"" />
              </Payments>
            </xsl:for-each>
          </Order>
        </xsl:for-each>
      </Orders>
    </ns0:GetOrdersRequestResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.OrdersResponse";
        
        private const global::INTSTDK008.Ehandel.Schemas.OrdersResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.GetOrders+GetOrdersRequestResponse";
        
        private const global::INTSTDK008.STIP.Schemas.GetOrders.GetOrdersRequestResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.OrdersResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.GetOrders+GetOrdersRequestResponse";
                return _TrgSchemas;
            }
        }
    }
}
