namespace INTSTDK008.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.GetOrdersResponse_JSON", typeof(global::INTSTDK008.Ehandel.Schemas.GetOrdersResponse_JSON))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.STIP.Schemas.GetOrdersResponse_JSON", typeof(global::INTSTDK008.STIP.Schemas.GetOrdersResponse_JSON))]
    public sealed class EHandel_OrdersResponse_To_GetOrdersResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.GetOrdersResponse_JSON"" xmlns:ns0=""http://www.skanetrafiken.se/DK/INTSTDK008/GetOrdersResponse_JSON/20150918"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetOrdersResponse"" />
  </xsl:template>
  <xsl:template match=""/s0:GetOrdersResponse"">
    <ns0:GetOrdersResponse>
      <xsl:for-each select=""Orders"">
        <Orders>
          <xsl:if test=""OrderConfirmationEmail"">
            <OrderConfirmationEmail>
              <xsl:value-of select=""OrderConfirmationEmail/text()"" />
            </OrderConfirmationEmail>
          </xsl:if>
          <xsl:if test=""OrderDate"">
            <OrderDate>
              <xsl:value-of select=""OrderDate/text()"" />
            </OrderDate>
          </xsl:if>
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
          <xsl:if test=""SystemName"">
            <SystemName>
              <xsl:value-of select=""SystemName/text()"" />
            </SystemName>
          </xsl:if>
          <xsl:if test=""SystemExtraInfo"">
            <SystemExtraInfo>
              <xsl:value-of select=""SystemExtraInfo/text()"" />
            </SystemExtraInfo>
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
          <xsl:if test=""OrderCreditedTotal"">
            <OrderCreditedTotal>
              <xsl:value-of select=""OrderCreditedTotal/text()"" />
            </OrderCreditedTotal>
          </xsl:if>
          <xsl:if test=""OrderType"">
            <OrderType>
              <xsl:value-of select=""OrderType/text()"" />
            </OrderType>
          </xsl:if>
          <xsl:if test=""CampaignCode"">
            <CampaignCode>
              <xsl:value-of select=""CampaignCode/text()"" />
            </CampaignCode>
          </xsl:if>
          <xsl:if test=""ExtraInfo"">
            <ExtraInfo>
              <xsl:value-of select=""ExtraInfo/text()"" />
            </ExtraInfo>
          </xsl:if>
          <xsl:if test=""Currency"">
            <Currency>
              <xsl:value-of select=""Currency/text()"" />
            </Currency>
          </xsl:if>
          <xsl:for-each select=""Customer"">
            <Customer>
              <xsl:if test=""Email"">
                <Email>
                  <xsl:value-of select=""Email/text()"" />
                </Email>
              </xsl:if>
              <xsl:if test=""AccountNumber"">
                <AccountNumber>
                  <xsl:value-of select=""AccountNumber/text()"" />
                </AccountNumber>
              </xsl:if>
              <xsl:if test=""AccountNumber2"">
                <AccountNumber2>
                  <xsl:value-of select=""AccountNumber2/text()"" />
                </AccountNumber2>
              </xsl:if>
              <xsl:if test=""IsCompany"">
                <IsCompany>
                  <xsl:value-of select=""IsCompany/text()"" />
                </IsCompany>
              </xsl:if>
              <xsl:if test=""ExtraInfo"">
                <ExtraInfo>
                  <xsl:value-of select=""ExtraInfo/text()"" />
                </ExtraInfo>
              </xsl:if>
              <xsl:if test=""CustomerName"">
                <CustomerName>
                  <xsl:value-of select=""CustomerName/text()"" />
                </CustomerName>
              </xsl:if>
              <xsl:if test=""IsAuthenticatedUser"">
                <IsAuthenticatedUser>
                  <xsl:value-of select=""IsAuthenticatedUser/text()"" />
                </IsAuthenticatedUser>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </Customer>
          </xsl:for-each>
          <xsl:for-each select=""ShippingAddress"">
            <ShippingAddress>
              <xsl:if test=""Address"">
                <Address>
                  <xsl:value-of select=""Address/text()"" />
                </Address>
              </xsl:if>
              <xsl:if test=""City"">
                <City>
                  <xsl:value-of select=""City/text()"" />
                </City>
              </xsl:if>
              <xsl:if test=""Co"">
                <Co>
                  <xsl:value-of select=""Co/text()"" />
                </Co>
              </xsl:if>
              <xsl:if test=""CompanyName"">
                <CompanyName>
                  <xsl:value-of select=""CompanyName/text()"" />
                </CompanyName>
              </xsl:if>
              <xsl:if test=""Country"">
                <Country>
                  <xsl:value-of select=""Country/text()"" />
                </Country>
              </xsl:if>
              <xsl:if test=""CellPhoneNumber"">
                <CellPhoneNumber>
                  <xsl:value-of select=""CellPhoneNumber/text()"" />
                </CellPhoneNumber>
              </xsl:if>
              <xsl:if test=""Email"">
                <Email>
                  <xsl:value-of select=""Email/text()"" />
                </Email>
              </xsl:if>
              <xsl:if test=""FirstName"">
                <FirstName>
                  <xsl:value-of select=""FirstName/text()"" />
                </FirstName>
              </xsl:if>
              <xsl:if test=""LastName"">
                <LastName>
                  <xsl:value-of select=""LastName/text()"" />
                </LastName>
              </xsl:if>
              <xsl:if test=""PostalCode"">
                <PostalCode>
                  <xsl:value-of select=""PostalCode/text()"" />
                </PostalCode>
              </xsl:if>
              <xsl:if test=""ExtraInfo"">
                <ExtraInfo>
                  <xsl:value-of select=""ExtraInfo/text()"" />
                </ExtraInfo>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </ShippingAddress>
          </xsl:for-each>
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
              <xsl:if test=""Price"">
                <Price>
                  <xsl:value-of select=""Price/text()"" />
                </Price>
              </xsl:if>
              <xsl:if test=""Discount"">
                <Discount>
                  <xsl:value-of select=""Discount/text()"" />
                </Discount>
              </xsl:if>
              <xsl:if test=""Quantity"">
                <Quantity>
                  <xsl:value-of select=""Quantity/text()"" />
                </Quantity>
              </xsl:if>
              <xsl:if test=""ExtraInfo"">
                <ExtraInfo>
                  <xsl:value-of select=""ExtraInfo/text()"" />
                </ExtraInfo>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </OrderItems>
          </xsl:for-each>
          <xsl:for-each select=""Payments"">
            <Payments>
              <xsl:if test=""PaymentMethodCode"">
                <PaymentMethodCode>
                  <xsl:value-of select=""PaymentMethodCode/text()"" />
                </PaymentMethodCode>
              </xsl:if>
              <xsl:if test=""PaymentMethodName"">
                <PaymentMethodName>
                  <xsl:value-of select=""PaymentMethodName/text()"" />
                </PaymentMethodName>
              </xsl:if>
              <xsl:if test=""TransactionType"">
                <TransactionType>
                  <xsl:value-of select=""TransactionType/text()"" />
                </TransactionType>
              </xsl:if>
              <xsl:if test=""Status"">
                <Status>
                  <xsl:value-of select=""Status/text()"" />
                </Status>
              </xsl:if>
              <xsl:if test=""GiftCardCode"">
                <GiftCardCode>
                  <xsl:value-of select=""GiftCardCode/text()"" />
                </GiftCardCode>
              </xsl:if>
              <xsl:if test=""Sum"">
                <Sum>
                  <xsl:value-of select=""Sum/text()"" />
                </Sum>
              </xsl:if>
              <xsl:if test=""TransactionId"">
                <TransactionId>
                  <xsl:value-of select=""TransactionId/text()"" />
                </TransactionId>
              </xsl:if>
              <xsl:if test=""ExtraInfo"">
                <ExtraInfo>
                  <xsl:value-of select=""ExtraInfo/text()"" />
                </ExtraInfo>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </Payments>
          </xsl:for-each>
          <xsl:for-each select=""OrderNotes"">
            <OrderNotes>
              <xsl:if test=""Created"">
                <Created>
                  <xsl:value-of select=""Created/text()"" />
                </Created>
              </xsl:if>
              <xsl:if test=""Message"">
                <Message>
                  <xsl:value-of select=""Message/text()"" />
                </Message>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </OrderNotes>
          </xsl:for-each>
          <xsl:for-each select=""Coupons"">
            <Coupons>
              <xsl:if test=""BlockedCardNumber"">
                <BlockedCardNumber>
                  <xsl:value-of select=""BlockedCardNumber/text()"" />
                </BlockedCardNumber>
              </xsl:if>
              <xsl:if test=""CouponCode"">
                <CouponCode>
                  <xsl:value-of select=""CouponCode/text()"" />
                </CouponCode>
              </xsl:if>
              <xsl:if test=""DateCreated"">
                <DateCreated>
                  <xsl:value-of select=""DateCreated/text()"" />
                </DateCreated>
              </xsl:if>
              <xsl:if test=""DateSent"">
                <DateSent>
                  <xsl:value-of select=""DateSent/text()"" />
                </DateSent>
              </xsl:if>
              <xsl:if test=""IsSent"">
                <IsSent>
                  <xsl:value-of select=""IsSent/text()"" />
                </IsSent>
              </xsl:if>
              <xsl:if test=""Receiver"">
                <Receiver>
                  <xsl:value-of select=""Receiver/text()"" />
                </Receiver>
              </xsl:if>
              <xsl:if test=""ShippingMethod"">
                <ShippingMethod>
                  <xsl:value-of select=""ShippingMethod/text()"" />
                </ShippingMethod>
              </xsl:if>
              <xsl:if test=""ExtraInfo"">
                <ExtraInfo>
                  <xsl:value-of select=""ExtraInfo/text()"" />
                </ExtraInfo>
              </xsl:if>
              <xsl:if test=""CouponSum"">
                <CouponSum>
                  <xsl:value-of select=""CouponSum/text()"" />
                </CouponSum>
              </xsl:if>
              <xsl:if test=""Attempts"">
                <Attempts>
                  <xsl:value-of select=""Attempts/text()"" />
                </Attempts>
              </xsl:if>
              <xsl:if test=""DateForLastAttempt"">
                <DateForLastAttempt>
                  <xsl:value-of select=""DateForLastAttempt/text()"" />
                </DateForLastAttempt>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </Coupons>
          </xsl:for-each>
          <xsl:for-each select=""Shipments"">
            <Shipments>
              <xsl:if test=""ShipmentMethodName"">
                <ShipmentMethodName>
                  <xsl:value-of select=""ShipmentMethodName/text()"" />
                </ShipmentMethodName>
              </xsl:if>
              <xsl:if test=""Status"">
                <Status>
                  <xsl:value-of select=""Status/text()"" />
                </Status>
              </xsl:if>
              <xsl:if test=""ExtraInfo"">
                <ExtraInfo>
                  <xsl:value-of select=""ExtraInfo/text()"" />
                </ExtraInfo>
              </xsl:if>
              <xsl:if test=""ShipmentMethodCode"">
                <ShipmentMethodCode>
                  <xsl:value-of select=""ShipmentMethodCode/text()"" />
                </ShipmentMethodCode>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </Shipments>
          </xsl:for-each>
          <xsl:value-of select=""./text()"" />
        </Orders>
      </xsl:for-each>
      <xsl:if test=""ErrorMessage"">
        <ErrorMessage>
          <xsl:value-of select=""ErrorMessage/text()"" />
        </ErrorMessage>
      </xsl:if>
      <xsl:if test=""StatusCode"">
        <StatusCode>
          <xsl:value-of select=""StatusCode/text()"" />
        </StatusCode>
      </xsl:if>
    </ns0:GetOrdersResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.GetOrdersResponse_JSON";
        
        private const global::INTSTDK008.Ehandel.Schemas.GetOrdersResponse_JSON _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.STIP.Schemas.GetOrdersResponse_JSON";
        
        private const global::INTSTDK008.STIP.Schemas.GetOrdersResponse_JSON _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.GetOrdersResponse_JSON";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.STIP.Schemas.GetOrdersResponse_JSON";
                return _TrgSchemas;
            }
        }
    }
}
