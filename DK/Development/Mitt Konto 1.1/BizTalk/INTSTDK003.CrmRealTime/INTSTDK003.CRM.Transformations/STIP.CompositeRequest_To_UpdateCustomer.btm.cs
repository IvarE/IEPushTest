namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeRequest", typeof(global::INTSTDK003.STIP.Schemas.CompositeRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+UpdateCustomer", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.UpdateCustomer))]
    public sealed class STIP_CompositeRequest_To_UpdateCustomer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 ScriptNS0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s0=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeRequest/20141215"" xmlns:ns2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns3=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ScriptNS0=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:crmCustomerRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:crmCustomerRequest"">
    <ns0:UpdateCustomer>
      <xsl:if test=""s0:setAccount/s0:accountNbr/s0:nbrGUID"">
        <ns0:customerId>
          <xsl:value-of select=""s0:setAccount/s0:accountNbr/s0:nbrGUID/text()"" />
        </ns0:customerId>
      </xsl:if>
      <xsl:for-each select=""s0:setAccount"">
        <xsl:for-each select=""s0:account"">
          <ns0:customer>
            <xsl:if test=""s0:firstName"">
              <ns2:AccountFirstName>
                <xsl:value-of select=""s0:firstName/text()"" />
              </ns2:AccountFirstName>
            </xsl:if>
            <xsl:if test=""../s0:accountNbr/s0:nbrGUID"">
              <ns2:AccountId>
                <xsl:value-of select=""../s0:accountNbr/s0:nbrGUID/text()"" />
              </ns2:AccountId>
            </xsl:if>
            <xsl:if test=""s0:lastName"">
              <ns2:AccountLastName>
                <xsl:value-of select=""s0:lastName/text()"" />
              </ns2:AccountLastName>
            </xsl:if>
            <xsl:if test=""../s0:accountNbr/s0:nbr2"">
              <ns2:AccountNumber>
                <xsl:value-of select=""../s0:accountNbr/s0:nbr2/text()"" />
              </ns2:AccountNumber>
            </xsl:if>
            <ns2:Addresses>
              <xsl:for-each select=""../s0:adresses"">
                <xsl:for-each select=""s0:address"">
                  <ns2:Address>
                    <ns2:AddressId>
                      <xsl:value-of select=""s0:addressId/text()"" />
                    </ns2:AddressId>
                    <ns2:AddressType>
                      <xsl:value-of select=""s0:adrType/text()"" />
                    </ns2:AddressType>
                    <xsl:if test=""s0:careOff"">
                      <ns2:CareOff>
                        <xsl:value-of select=""s0:careOff/text()"" />
                      </ns2:CareOff>
                    </xsl:if>
                    <xsl:if test=""s0:city"">
                      <ns2:City>
                        <xsl:value-of select=""s0:city/text()"" />
                      </ns2:City>
                    </xsl:if>
                    <xsl:if test=""s0:companyName"">
                      <ns2:CompanyName>
                        <xsl:value-of select=""s0:companyName/text()"" />
                      </ns2:CompanyName>
                    </xsl:if>
                    <xsl:if test=""s0:contactPerson"">
                      <ns2:ContactPerson>
                        <xsl:value-of select=""s0:contactPerson/text()"" />
                      </ns2:ContactPerson>
                    </xsl:if>
                    <xsl:if test=""s0:contactPhnNbr"">
                      <ns2:ContactPhoneNumber>
                        <xsl:value-of select=""s0:contactPhnNbr/text()"" />
                      </ns2:ContactPhoneNumber>
                    </xsl:if>
                    <xsl:if test=""s0:country"">
                      <ns2:Country>
                        <xsl:value-of select=""s0:country/text()"" />
                      </ns2:Country>
                    </xsl:if>
                    <xsl:if test=""s0:county"">
                      <ns2:County>
                        <xsl:value-of select=""s0:county/text()"" />
                      </ns2:County>
                    </xsl:if>
                    <xsl:if test=""s0:emailNotifAdr"">
                      <ns2:EmailNotificationAddress>
                        <xsl:value-of select=""s0:emailNotifAdr/text()"" />
                      </ns2:EmailNotificationAddress>
                    </xsl:if>
                    <xsl:if test=""s0:zipPostalCode"">
                      <ns2:PostalCode>
                        <xsl:value-of select=""s0:zipPostalCode/text()"" />
                      </ns2:PostalCode>
                    </xsl:if>
                    <xsl:if test=""s0:smsNotifNbr"">
                      <ns2:SMSNotificationNumber>
                        <xsl:value-of select=""s0:smsNotifNbr/text()"" />
                      </ns2:SMSNotificationNumber>
                    </xsl:if>
                    <xsl:if test=""s0:street"">
                      <ns2:Street>
                        <xsl:value-of select=""s0:street/text()"" />
                      </ns2:Street>
                    </xsl:if>
                  </ns2:Address>
                </xsl:for-each>
              </xsl:for-each>
            </ns2:Addresses>
            <xsl:if test=""s0:allowAutoLoad"">
              <ns2:AllowAutoLoad>
                <xsl:value-of select=""s0:allowAutoLoad/text()"" />
              </ns2:AllowAutoLoad>
            </xsl:if>
            <xsl:if test=""s0:compName"">
              <ns2:CompanyName>
                <xsl:value-of select=""s0:compName/text()"" />
              </ns2:CompanyName>
            </xsl:if>
            <ns2:CustomerType>
              <xsl:value-of select=""s0:custType/text()"" />
            </ns2:CustomerType>
            <xsl:if test=""s0:deleted"">
              <ns2:Deleted>
                <xsl:value-of select=""s0:deleted/text()"" />
              </ns2:Deleted>
            </xsl:if>
            <ns2:Email>
              <xsl:value-of select=""s0:email/text()"" />
            </ns2:Email>
            <xsl:if test=""s0:inactive"">
              <ns2:InActive>
                <xsl:value-of select=""s0:inactive/text()"" />
              </ns2:InActive>
            </xsl:if>
            <xsl:if test=""s0:maxCardsAutoLoad"">
              <ns2:MaxCardsAutoLoad>
                <xsl:value-of select=""s0:maxCardsAutoLoad/text()"" />
              </ns2:MaxCardsAutoLoad>
            </xsl:if>
            <ns2:MobilePhone>
              <xsl:value-of select=""s0:mobPhone/text()"" />
            </ns2:MobilePhone>
            <xsl:if test=""s0:orgCreditApproved"">
              <ns2:OrganizationCreditApproved>
                <xsl:value-of select=""s0:orgCreditApproved/text()"" />
              </ns2:OrganizationCreditApproved>
            </xsl:if>
            <xsl:if test=""s0:orgNbr"">
              <ns2:OrganizationNumber>
                <xsl:value-of select=""s0:orgNbr/text()"" />
              </ns2:OrganizationNumber>
            </xsl:if>
            <xsl:if test=""s0:orgSubNbr"">
              <ns2:OrganizationSubNumber>
                <xsl:value-of select=""s0:orgSubNbr/text()"" />
              </ns2:OrganizationSubNumber>
            </xsl:if>
            <xsl:if test=""s0:phone"">
              <ns2:Phone>
                <xsl:value-of select=""s0:phone/text()"" />
              </ns2:Phone>
            </xsl:if>
            <xsl:if test=""s0:socialSecNbr"">
              <ns2:SocialSecurtiyNumber>
                <xsl:value-of select=""s0:socialSecNbr/text()"" />
              </ns2:SocialSecurtiyNumber>
            </xsl:if>
          </ns0:customer>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:variable name=""var:v1"" select=""ScriptNS0:ReadSSOValue(&quot;INTSTDK003&quot; , &quot;CallerId&quot;)"" />
      <ns0:callerId>
        <xsl:value-of select=""$var:v1"" />
      </ns0:callerId>
    </ns0:UpdateCustomer>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects>
  <ExtensionObject Namespace=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"" AssemblyName=""INTSTDK003.Helper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41"" ClassName=""INTSTDK003.Helper.INTSTDK003Helper"" />
</ExtensionObjects>";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.STIP.Schemas.CompositeRequest";
        
        private const global::INTSTDK003.STIP.Schemas.CompositeRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+UpdateCustomer";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.UpdateCustomer _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.STIP.Schemas.CompositeRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+UpdateCustomer";
                return _TrgSchemas;
            }
        }
    }
}
