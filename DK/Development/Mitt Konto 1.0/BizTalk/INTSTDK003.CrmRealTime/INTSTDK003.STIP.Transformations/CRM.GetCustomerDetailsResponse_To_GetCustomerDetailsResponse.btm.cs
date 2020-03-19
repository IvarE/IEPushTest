namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerDetailsResponse", typeof(global::INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerDetailsResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerDetailsResponse", typeof(global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerDetailsResponse))]
    public sealed class CRM_GetCustomerDetailsResponse_To_GetCustomerDetailsResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s0 s2"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"" xmlns:s3=""http://tempuri.org/"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGIXrmEAIConnectorService"" xmlns:s2=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns2=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1/20141126"" xmlns:ns1=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2_3/20141126"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService/20141126"" xmlns:ns3=""http://www.skanetrafiken.com/DK/INTSTDK003/EAIConnectorService_1_2/20141126"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetCustomerDetailsResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:GetCustomerDetailsResponse"">
    <ns0:GetCustomerDetailsResponse>
      <xsl:for-each select=""s3:GetCustomerDetailsResult"">
        <ns0:GetCustomerDetailsResult>
          <xsl:if test=""s1:IsNull"">
            <ns3:IsNull>
              <xsl:value-of select=""s1:IsNull/text()"" />
            </ns3:IsNull>
          </xsl:if>
          <xsl:if test=""s1:Message"">
            <xsl:variable name=""var:v1"" select=""string(s1:Message/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v1)='true'"">
              <ns3:Message>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns3:Message>
            </xsl:if>
            <xsl:if test=""string($var:v1)='false'"">
              <ns3:Message>
                <xsl:value-of select=""s1:Message/text()"" />
              </ns3:Message>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s1:Status"">
            <ns3:Status>
              <xsl:value-of select=""s1:Status/text()"" />
            </ns3:Status>
          </xsl:if>
          <xsl:for-each select=""s1:Customer"">
            <ns3:Customer>
              <xsl:variable name=""var:v2"" select=""string(s1:AccountFirstName/@xsi:nil) = 'true'"" />
              <xsl:if test=""string($var:v2)='true'"">
                <ns3:AccountFirstName>
                  <xsl:attribute name=""xsi:nil"">
                    <xsl:value-of select=""'true'"" />
                  </xsl:attribute>
                </ns3:AccountFirstName>
              </xsl:if>
              <xsl:if test=""string($var:v2)='false'"">
                <ns3:AccountFirstName>
                  <xsl:value-of select=""s1:AccountFirstName/text()"" />
                </ns3:AccountFirstName>
              </xsl:if>
              <xsl:if test=""s1:AccountId"">
                <ns3:AccountId>
                  <xsl:value-of select=""s1:AccountId/text()"" />
                </ns3:AccountId>
              </xsl:if>
              <xsl:variable name=""var:v3"" select=""string(s1:AccountLastName/@xsi:nil) = 'true'"" />
              <xsl:if test=""string($var:v3)='true'"">
                <ns3:AccountLastName>
                  <xsl:attribute name=""xsi:nil"">
                    <xsl:value-of select=""'true'"" />
                  </xsl:attribute>
                </ns3:AccountLastName>
              </xsl:if>
              <xsl:if test=""string($var:v3)='false'"">
                <ns3:AccountLastName>
                  <xsl:value-of select=""s1:AccountLastName/text()"" />
                </ns3:AccountLastName>
              </xsl:if>
              <xsl:if test=""s1:AccountNumber"">
                <xsl:variable name=""var:v4"" select=""string(s1:AccountNumber/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v4)='true'"">
                  <ns3:AccountNumber>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:AccountNumber>
                </xsl:if>
                <xsl:if test=""string($var:v4)='false'"">
                  <ns3:AccountNumber>
                    <xsl:value-of select=""s1:AccountNumber/text()"" />
                  </ns3:AccountNumber>
                </xsl:if>
              </xsl:if>
              <xsl:for-each select=""s1:Addresses"">
                <ns3:Addresses>
                  <xsl:for-each select=""s1:Address"">
                    <ns3:Address>
                      <xsl:if test=""s1:AddressId"">
                        <xsl:variable name=""var:v5"" select=""string(s1:AddressId/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v5)='true'"">
                          <ns3:AddressId>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:AddressId>
                        </xsl:if>
                        <xsl:if test=""string($var:v5)='false'"">
                          <ns3:AddressId>
                            <xsl:value-of select=""s1:AddressId/text()"" />
                          </ns3:AddressId>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:AddressType"">
                        <ns3:AddressType>
                          <xsl:value-of select=""s1:AddressType/text()"" />
                        </ns3:AddressType>
                      </xsl:if>
                      <xsl:if test=""s1:CareOff"">
                        <xsl:variable name=""var:v6"" select=""string(s1:CareOff/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v6)='true'"">
                          <ns3:CareOff>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:CareOff>
                        </xsl:if>
                        <xsl:if test=""string($var:v6)='false'"">
                          <ns3:CareOff>
                            <xsl:value-of select=""s1:CareOff/text()"" />
                          </ns3:CareOff>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:City"">
                        <xsl:variable name=""var:v7"" select=""string(s1:City/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v7)='true'"">
                          <ns3:City>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:City>
                        </xsl:if>
                        <xsl:if test=""string($var:v7)='false'"">
                          <ns3:City>
                            <xsl:value-of select=""s1:City/text()"" />
                          </ns3:City>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:CompanyName"">
                        <xsl:variable name=""var:v8"" select=""string(s1:CompanyName/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v8)='true'"">
                          <ns3:CompanyName>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:CompanyName>
                        </xsl:if>
                        <xsl:if test=""string($var:v8)='false'"">
                          <ns3:CompanyName>
                            <xsl:value-of select=""s1:CompanyName/text()"" />
                          </ns3:CompanyName>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:ContactPerson"">
                        <xsl:variable name=""var:v9"" select=""string(s1:ContactPerson/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v9)='true'"">
                          <ns3:ContactPerson>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:ContactPerson>
                        </xsl:if>
                        <xsl:if test=""string($var:v9)='false'"">
                          <ns3:ContactPerson>
                            <xsl:value-of select=""s1:ContactPerson/text()"" />
                          </ns3:ContactPerson>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:ContactPhoneNumber"">
                        <xsl:variable name=""var:v10"" select=""string(s1:ContactPhoneNumber/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v10)='true'"">
                          <ns3:ContactPhoneNumber>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:ContactPhoneNumber>
                        </xsl:if>
                        <xsl:if test=""string($var:v10)='false'"">
                          <ns3:ContactPhoneNumber>
                            <xsl:value-of select=""s1:ContactPhoneNumber/text()"" />
                          </ns3:ContactPhoneNumber>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:Country"">
                        <xsl:variable name=""var:v11"" select=""string(s1:Country/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v11)='true'"">
                          <ns3:Country>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:Country>
                        </xsl:if>
                        <xsl:if test=""string($var:v11)='false'"">
                          <ns3:Country>
                            <xsl:value-of select=""s1:Country/text()"" />
                          </ns3:Country>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:County"">
                        <xsl:variable name=""var:v12"" select=""string(s1:County/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v12)='true'"">
                          <ns3:County>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:County>
                        </xsl:if>
                        <xsl:if test=""string($var:v12)='false'"">
                          <ns3:County>
                            <xsl:value-of select=""s1:County/text()"" />
                          </ns3:County>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:CustomerAddressId"">
                        <ns3:CustomerAddressId>
                          <xsl:value-of select=""s1:CustomerAddressId/text()"" />
                        </ns3:CustomerAddressId>
                      </xsl:if>
                      <xsl:if test=""s1:PostalCode"">
                        <xsl:variable name=""var:v13"" select=""string(s1:PostalCode/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v13)='true'"">
                          <ns3:PostalCode>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:PostalCode>
                        </xsl:if>
                        <xsl:if test=""string($var:v13)='false'"">
                          <ns3:PostalCode>
                            <xsl:value-of select=""s1:PostalCode/text()"" />
                          </ns3:PostalCode>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:SMSNotificationNumber"">
                        <xsl:variable name=""var:v14"" select=""string(s1:SMSNotificationNumber/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v14)='true'"">
                          <ns3:SMSNotificationNumber>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:SMSNotificationNumber>
                        </xsl:if>
                        <xsl:if test=""string($var:v14)='false'"">
                          <ns3:SMSNotificationNumber>
                            <xsl:value-of select=""s1:SMSNotificationNumber/text()"" />
                          </ns3:SMSNotificationNumber>
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test=""s1:Street"">
                        <xsl:variable name=""var:v15"" select=""string(s1:Street/@xsi:nil) = 'true'"" />
                        <xsl:if test=""string($var:v15)='true'"">
                          <ns3:Street>
                            <xsl:attribute name=""xsi:nil"">
                              <xsl:value-of select=""'true'"" />
                            </xsl:attribute>
                          </ns3:Street>
                        </xsl:if>
                        <xsl:if test=""string($var:v15)='false'"">
                          <ns3:Street>
                            <xsl:value-of select=""s1:Street/text()"" />
                          </ns3:Street>
                        </xsl:if>
                      </xsl:if>
                      <xsl:value-of select=""./text()"" />
                    </ns3:Address>
                  </xsl:for-each>
                  <xsl:value-of select=""./text()"" />
                </ns3:Addresses>
              </xsl:for-each>
              <xsl:if test=""s1:AllowAutoLoad"">
                <ns3:AllowAutoLoad>
                  <xsl:value-of select=""s1:AllowAutoLoad/text()"" />
                </ns3:AllowAutoLoad>
              </xsl:if>
              <xsl:variable name=""var:v16"" select=""string(s1:CompanyName/@xsi:nil) = 'true'"" />
              <xsl:if test=""string($var:v16)='true'"">
                <ns3:CompanyName>
                  <xsl:attribute name=""xsi:nil"">
                    <xsl:value-of select=""'true'"" />
                  </xsl:attribute>
                </ns3:CompanyName>
              </xsl:if>
              <xsl:if test=""string($var:v16)='false'"">
                <ns3:CompanyName>
                  <xsl:value-of select=""s1:CompanyName/text()"" />
                </ns3:CompanyName>
              </xsl:if>
              <xsl:if test=""s1:Counterpart"">
                <xsl:variable name=""var:v17"" select=""string(s1:Counterpart/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v17)='true'"">
                  <ns3:Counterpart>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:Counterpart>
                </xsl:if>
                <xsl:if test=""string($var:v17)='false'"">
                  <ns3:Counterpart>
                    <xsl:value-of select=""s1:Counterpart/text()"" />
                  </ns3:Counterpart>
                </xsl:if>
              </xsl:if>
              <ns3:CustomerType>
                <xsl:value-of select=""s1:CustomerType/text()"" />
              </ns3:CustomerType>
              <xsl:if test=""s1:Deleted"">
                <ns3:Deleted>
                  <xsl:value-of select=""s1:Deleted/text()"" />
                </ns3:Deleted>
              </xsl:if>
              <xsl:variable name=""var:v18"" select=""string(s1:Email/@xsi:nil) = 'true'"" />
              <xsl:if test=""string($var:v18)='true'"">
                <ns3:Email>
                  <xsl:attribute name=""xsi:nil"">
                    <xsl:value-of select=""'true'"" />
                  </xsl:attribute>
                </ns3:Email>
              </xsl:if>
              <xsl:if test=""string($var:v18)='false'"">
                <ns3:Email>
                  <xsl:value-of select=""s1:Email/text()"" />
                </ns3:Email>
              </xsl:if>
              <xsl:if test=""s1:InActive"">
                <ns3:InActive>
                  <xsl:value-of select=""s1:InActive/text()"" />
                </ns3:InActive>
              </xsl:if>
              <xsl:if test=""s1:MaxCardsAutoLoad"">
                <ns3:MaxCardsAutoLoad>
                  <xsl:value-of select=""s1:MaxCardsAutoLoad/text()"" />
                </ns3:MaxCardsAutoLoad>
              </xsl:if>
              <xsl:variable name=""var:v19"" select=""string(s1:MobilePhone/@xsi:nil) = 'true'"" />
              <xsl:if test=""string($var:v19)='true'"">
                <ns3:MobilePhone>
                  <xsl:attribute name=""xsi:nil"">
                    <xsl:value-of select=""'true'"" />
                  </xsl:attribute>
                </ns3:MobilePhone>
              </xsl:if>
              <xsl:if test=""string($var:v19)='false'"">
                <ns3:MobilePhone>
                  <xsl:value-of select=""s1:MobilePhone/text()"" />
                </ns3:MobilePhone>
              </xsl:if>
              <xsl:if test=""s1:OrganizationCreditApproved"">
                <ns3:OrganizationCreditApproved>
                  <xsl:value-of select=""s1:OrganizationCreditApproved/text()"" />
                </ns3:OrganizationCreditApproved>
              </xsl:if>
              <xsl:if test=""s1:OrganizationNumber"">
                <xsl:variable name=""var:v20"" select=""string(s1:OrganizationNumber/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v20)='true'"">
                  <ns3:OrganizationNumber>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:OrganizationNumber>
                </xsl:if>
                <xsl:if test=""string($var:v20)='false'"">
                  <ns3:OrganizationNumber>
                    <xsl:value-of select=""s1:OrganizationNumber/text()"" />
                  </ns3:OrganizationNumber>
                </xsl:if>
              </xsl:if>
              <xsl:if test=""s1:OrganizationSubNumber"">
                <xsl:variable name=""var:v21"" select=""string(s1:OrganizationSubNumber/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v21)='true'"">
                  <ns3:OrganizationSubNumber>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:OrganizationSubNumber>
                </xsl:if>
                <xsl:if test=""string($var:v21)='false'"">
                  <ns3:OrganizationSubNumber>
                    <xsl:value-of select=""s1:OrganizationSubNumber/text()"" />
                  </ns3:OrganizationSubNumber>
                </xsl:if>
              </xsl:if>
              <xsl:if test=""s1:Phone"">
                <xsl:variable name=""var:v22"" select=""string(s1:Phone/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v22)='true'"">
                  <ns3:Phone>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:Phone>
                </xsl:if>
                <xsl:if test=""string($var:v22)='false'"">
                  <ns3:Phone>
                    <xsl:value-of select=""s1:Phone/text()"" />
                  </ns3:Phone>
                </xsl:if>
              </xsl:if>
              <xsl:if test=""s1:Responsibility"">
                <xsl:variable name=""var:v23"" select=""string(s1:Responsibility/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v23)='true'"">
                  <ns3:Responsibility>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:Responsibility>
                </xsl:if>
                <xsl:if test=""string($var:v23)='false'"">
                  <ns3:Responsibility>
                    <xsl:value-of select=""s1:Responsibility/text()"" />
                  </ns3:Responsibility>
                </xsl:if>
              </xsl:if>
              <xsl:if test=""s1:Rsid"">
                <xsl:variable name=""var:v24"" select=""string(s1:Rsid/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v24)='true'"">
                  <ns3:Rsid>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:Rsid>
                </xsl:if>
                <xsl:if test=""string($var:v24)='false'"">
                  <ns3:Rsid>
                    <xsl:value-of select=""s1:Rsid/text()"" />
                  </ns3:Rsid>
                </xsl:if>
              </xsl:if>
              <xsl:if test=""s1:SocialSecurityNumber"">
                <xsl:variable name=""var:v25"" select=""string(s1:SocialSecurityNumber/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v25)='true'"">
                  <ns3:SocialSecurityNumber>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns3:SocialSecurityNumber>
                </xsl:if>
                <xsl:if test=""string($var:v25)='false'"">
                  <ns3:SocialSecurityNumber>
                    <xsl:value-of select=""s1:SocialSecurityNumber/text()"" />
                  </ns3:SocialSecurityNumber>
                </xsl:if>
              </xsl:if>
              <xsl:value-of select=""./text()"" />
            </ns3:Customer>
          </xsl:for-each>
          <xsl:if test=""s1:RequestAccountCategoryCode"">
            <ns3:RequestAccountCategoryCode>
              <xsl:value-of select=""s1:RequestAccountCategoryCode/text()"" />
            </ns3:RequestAccountCategoryCode>
          </xsl:if>
        </ns0:GetCustomerDetailsResult>
      </xsl:for-each>
    </ns0:GetCustomerDetailsResponse>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerDetailsResponse";
        
        private const global::INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerDetailsResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerDetailsResponse";
        
        private const global::INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService.GetCustomerDetailsResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerDetailsResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.STIP.Schemas.GetCustomerDetails.EAIConnectorService+GetCustomerDetailsResponse";
                return _TrgSchemas;
            }
        }
    }
}
