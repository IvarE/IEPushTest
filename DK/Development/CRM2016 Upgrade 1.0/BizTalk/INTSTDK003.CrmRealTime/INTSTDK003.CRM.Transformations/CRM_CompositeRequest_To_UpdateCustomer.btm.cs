namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.EHandel.Schemas.Internal.CompositeRequest", typeof(global::INTSTDK003.EHandel.Schemas.Internal.CompositeRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.External.PortalService+UpdateCustomer", typeof(global::INTSTDK003.CRM.Schemas.External.PortalService.UpdateCustomer))]
    public sealed class CRM_CompositeRequest_To_UpdateCustomer : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://www.skanetrafiken.se/INSTDK003.03.CrmRealTime"" xmlns:ns0=""http://tempuri.org/"" xmlns:ns4=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:ns3=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:ns2=""http://schemas.microsoft.com/xrm/2011/Contracts"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:crmCustomerRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:crmCustomerRequest"">
    <ns0:UpdateCustomer>
      <xsl:for-each select=""s0:setAccount"">
        <xsl:for-each select=""s0:account"">
          <ns0:customer>
            <xsl:if test=""s0:firstName"">
              <ns3:AccountFirstName>
                <xsl:value-of select=""s0:firstName/text()"" />
              </ns3:AccountFirstName>
            </xsl:if>
            <xsl:if test=""../s0:accountNbr/s0:nbrGUID"">
              <ns3:AccountId>
                <xsl:value-of select=""../s0:accountNbr/s0:nbrGUID/text()"" />
              </ns3:AccountId>
            </xsl:if>
            <xsl:if test=""s0:lastName"">
              <ns3:AccountLastName>
                <xsl:value-of select=""s0:lastName/text()"" />
              </ns3:AccountLastName>
            </xsl:if>
            <ns3:Addresses>
              <xsl:for-each select=""../s0:adresses"">
                <xsl:for-each select=""s0:address"">
                  <ns3:Address>
                    <ns3:AddressId>
                      <xsl:value-of select=""s0:addressId/text()"" />
                    </ns3:AddressId>
                    <ns3:AddressType>
                      <xsl:value-of select=""s0:adrType/text()"" />
                    </ns3:AddressType>
                    <xsl:if test=""s0:careOff"">
                      <ns3:CareOff>
                        <xsl:value-of select=""s0:careOff/text()"" />
                      </ns3:CareOff>
                    </xsl:if>
                    <xsl:if test=""s0:city"">
                      <ns3:City>
                        <xsl:value-of select=""s0:city/text()"" />
                      </ns3:City>
                    </xsl:if>
                    <xsl:if test=""s0:companyName"">
                      <ns3:CompanyName>
                        <xsl:value-of select=""s0:companyName/text()"" />
                      </ns3:CompanyName>
                    </xsl:if>
                    <xsl:if test=""s0:contactPerson"">
                      <ns3:ContactPerson>
                        <xsl:value-of select=""s0:contactPerson/text()"" />
                      </ns3:ContactPerson>
                    </xsl:if>
                    <xsl:if test=""s0:contactPhnNbr"">
                      <ns3:ContactPhoneNumber>
                        <xsl:value-of select=""s0:contactPhnNbr/text()"" />
                      </ns3:ContactPhoneNumber>
                    </xsl:if>
                    <xsl:if test=""s0:country"">
                      <ns3:Country>
                        <xsl:value-of select=""s0:country/text()"" />
                      </ns3:Country>
                    </xsl:if>
                    <xsl:if test=""s0:county"">
                      <ns3:County>
                        <xsl:value-of select=""s0:county/text()"" />
                      </ns3:County>
                    </xsl:if>
                    <xsl:if test=""s0:zipPostalCode"">
                      <ns3:PostalCode>
                        <xsl:value-of select=""s0:zipPostalCode/text()"" />
                      </ns3:PostalCode>
                    </xsl:if>
                    <xsl:if test=""s0:smsNotifNbr"">
                      <ns3:SMSNotificationNumber>
                        <xsl:value-of select=""s0:smsNotifNbr/text()"" />
                      </ns3:SMSNotificationNumber>
                    </xsl:if>
                    <xsl:if test=""s0:street"">
                      <ns3:Street>
                        <xsl:value-of select=""s0:street/text()"" />
                      </ns3:Street>
                    </xsl:if>
                  </ns3:Address>
                </xsl:for-each>
              </xsl:for-each>
            </ns3:Addresses>
            <xsl:if test=""s0:allowAutoLoad"">
              <ns3:AllowAutoLoad>
                <xsl:value-of select=""s0:allowAutoLoad/text()"" />
              </ns3:AllowAutoLoad>
            </xsl:if>
            <xsl:if test=""s0:compName"">
              <ns3:CompanyName>
                <xsl:value-of select=""s0:compName/text()"" />
              </ns3:CompanyName>
            </xsl:if>
            <ns3:CustomerType>
              <xsl:value-of select=""s0:custType/text()"" />
            </ns3:CustomerType>
            <xsl:if test=""s0:deleted"">
              <ns3:Deleted>
                <xsl:value-of select=""s0:deleted/text()"" />
              </ns3:Deleted>
            </xsl:if>
            <ns3:Email>
              <xsl:value-of select=""s0:email/text()"" />
            </ns3:Email>
            <xsl:if test=""s0:inactive"">
              <ns3:InActive>
                <xsl:value-of select=""s0:inactive/text()"" />
              </ns3:InActive>
            </xsl:if>
            <xsl:if test=""s0:maxCardsAutoLoad"">
              <ns3:MaxCardsAutoLoad>
                <xsl:value-of select=""s0:maxCardsAutoLoad/text()"" />
              </ns3:MaxCardsAutoLoad>
            </xsl:if>
            <ns3:MobilePhone>
              <xsl:value-of select=""s0:mobPhone/text()"" />
            </ns3:MobilePhone>
            <xsl:if test=""s0:orgCreditApproved"">
              <ns3:OrganizationCreditApproved>
                <xsl:value-of select=""s0:orgCreditApproved/text()"" />
              </ns3:OrganizationCreditApproved>
            </xsl:if>
            <xsl:if test=""s0:orgNbr"">
              <ns3:OrganizationNumber>
                <xsl:value-of select=""s0:orgNbr/text()"" />
              </ns3:OrganizationNumber>
            </xsl:if>
            <xsl:if test=""s0:orgSubNbr"">
              <ns3:OrganizationSubNumber>
                <xsl:value-of select=""s0:orgSubNbr/text()"" />
              </ns3:OrganizationSubNumber>
            </xsl:if>
            <xsl:if test=""s0:phone"">
              <ns3:Phone>
                <xsl:value-of select=""s0:phone/text()"" />
              </ns3:Phone>
            </xsl:if>
            <xsl:if test=""s0:socialSecNbr"">
              <ns3:SocialSecurtiyNumber>
                <xsl:value-of select=""s0:socialSecNbr/text()"" />
              </ns3:SocialSecurtiyNumber>
            </xsl:if>
          </ns0:customer>
        </xsl:for-each>
      </xsl:for-each>
    </ns0:UpdateCustomer>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.EHandel.Schemas.Internal.CompositeRequest";
        
        private const global::INTSTDK003.EHandel.Schemas.Internal.CompositeRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.External.PortalService+UpdateCustomer";
        
        private const global::INTSTDK003.CRM.Schemas.External.PortalService.UpdateCustomer _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.EHandel.Schemas.Internal.CompositeRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.External.PortalService+UpdateCustomer";
                return _TrgSchemas;
            }
        }
    }
}
