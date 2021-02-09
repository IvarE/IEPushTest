namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCustomerResponse", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCustomerResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeResponse", typeof(global::INTSTDK003.STIP.Schemas.CompositeResponse))]
    public sealed class CRM_GetCustomerResponse_To_CompositeResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s2 s0 userCSharp"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeResponse/20141215"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s3=""http://tempuri.org/"" xmlns:s2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:s0=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:GetCustomerResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:GetCustomerResponse"">
    <ns0:crmCustomerResponse>
      <xsl:for-each select=""s3:GetCustomerResult"">
        <xsl:for-each select=""s2:Customer"">
          <xsl:variable name=""var:v1"" select=""string(./@xsi:nil) = 'true'"" />
          <xsl:variable name=""var:v2"" select=""userCSharp:LogicalNot(string($var:v1))"" />
          <ns0:customer>
            <xsl:if test=""$var:v2"">
              <ns0:accountNbr>
                <xsl:if test=""s2:AccountId"">
                  <ns0:nbrGUID>
                    <xsl:value-of select=""s2:AccountId/text()"" />
                  </ns0:nbrGUID>
                </xsl:if>
                <xsl:if test=""s2:AccountNumber"">
                  <ns0:nbr2>
                    <xsl:value-of select=""s2:AccountNumber/text()"" />
                  </ns0:nbr2>
                </xsl:if>
              </ns0:accountNbr>
            </xsl:if>
            <xsl:if test=""$var:v2"">
              <ns0:account>
                <ns0:compName>
                  <xsl:value-of select=""s2:CompanyName/text()"" />
                </ns0:compName>
                <ns0:firstName>
                  <xsl:value-of select=""s2:AccountFirstName/text()"" />
                </ns0:firstName>
                <ns0:lastName>
                  <xsl:value-of select=""s2:AccountLastName/text()"" />
                </ns0:lastName>
                <ns0:mobPhone>
                  <xsl:value-of select=""s2:MobilePhone/text()"" />
                </ns0:mobPhone>
                <xsl:if test=""s2:Phone"">
                  <ns0:phone>
                    <xsl:value-of select=""s2:Phone/text()"" />
                  </ns0:phone>
                </xsl:if>
                <ns0:email>
                  <xsl:value-of select=""s2:Email/text()"" />
                </ns0:email>
                <xsl:if test=""s2:AllowAutoLoad"">
                  <ns0:allowAutoLoad>
                    <xsl:value-of select=""s2:AllowAutoLoad/text()"" />
                  </ns0:allowAutoLoad>
                </xsl:if>
                <xsl:if test=""s2:MaxCardsAutoLoad"">
                  <ns0:maxCardsAutoLoad>
                    <xsl:value-of select=""s2:MaxCardsAutoLoad/text()"" />
                  </ns0:maxCardsAutoLoad>
                </xsl:if>
                <ns0:custType>
                  <xsl:value-of select=""s2:CustomerType/text()"" />
                </ns0:custType>
                <xsl:if test=""s2:OrganizationCreditApproved"">
                  <ns0:orgCreditApproved>
                    <xsl:value-of select=""s2:OrganizationCreditApproved/text()"" />
                  </ns0:orgCreditApproved>
                </xsl:if>
                <xsl:if test=""s2:OrganizationNumber"">
                  <ns0:orgNbr>
                    <xsl:value-of select=""s2:OrganizationNumber/text()"" />
                  </ns0:orgNbr>
                </xsl:if>
                <xsl:if test=""s2:OrganizationSubNumber"">
                  <ns0:orgSubNbr>
                    <xsl:value-of select=""s2:OrganizationSubNumber/text()"" />
                  </ns0:orgSubNbr>
                </xsl:if>
                <xsl:if test=""s2:SocialSecurtiyNumber"">
                  <ns0:socialSecNbr>
                    <xsl:value-of select=""s2:SocialSecurtiyNumber/text()"" />
                  </ns0:socialSecNbr>
                </xsl:if>
                <xsl:if test=""s2:InActive"">
                  <ns0:inactive>
                    <xsl:value-of select=""s2:InActive/text()"" />
                  </ns0:inactive>
                </xsl:if>
                <xsl:if test=""s2:Deleted"">
                  <ns0:deleted>
                    <xsl:value-of select=""s2:Deleted/text()"" />
                  </ns0:deleted>
                </xsl:if>
              </ns0:account>
            </xsl:if>
            <xsl:if test=""$var:v2"">
              <ns0:adresses>
                <xsl:for-each select=""s2:Addresses"">
                  <xsl:for-each select=""s2:Address"">
                    <ns0:address>
                      <xsl:if test=""s2:AddressId"">
                        <ns0:addressId>
                          <xsl:value-of select=""s2:AddressId/text()"" />
                        </ns0:addressId>
                      </xsl:if>
                      <xsl:if test=""s2:CompanyName"">
                        <ns0:companyName>
                          <xsl:value-of select=""s2:CompanyName/text()"" />
                        </ns0:companyName>
                      </xsl:if>
                      <xsl:if test=""s2:Street"">
                        <ns0:street>
                          <xsl:value-of select=""s2:Street/text()"" />
                        </ns0:street>
                      </xsl:if>
                      <xsl:if test=""s2:PostalCode"">
                        <ns0:zipPostalCode>
                          <xsl:value-of select=""s2:PostalCode/text()"" />
                        </ns0:zipPostalCode>
                      </xsl:if>
                      <xsl:if test=""s2:City"">
                        <ns0:city>
                          <xsl:value-of select=""s2:City/text()"" />
                        </ns0:city>
                      </xsl:if>
                      <xsl:if test=""s2:County"">
                        <ns0:county>
                          <xsl:value-of select=""s2:County/text()"" />
                        </ns0:county>
                      </xsl:if>
                      <xsl:if test=""s2:Country"">
                        <ns0:country>
                          <xsl:value-of select=""s2:Country/text()"" />
                        </ns0:country>
                      </xsl:if>
                      <xsl:if test=""s2:CareOff"">
                        <ns0:careOff>
                          <xsl:value-of select=""s2:CareOff/text()"" />
                        </ns0:careOff>
                      </xsl:if>
                      <xsl:if test=""s2:ContactPerson"">
                        <ns0:contactPerson>
                          <xsl:value-of select=""s2:ContactPerson/text()"" />
                        </ns0:contactPerson>
                      </xsl:if>
                      <xsl:if test=""s2:ContactPhoneNumber"">
                        <ns0:contactPhnNbr>
                          <xsl:value-of select=""s2:ContactPhoneNumber/text()"" />
                        </ns0:contactPhnNbr>
                      </xsl:if>
                      <xsl:if test=""s2:SMSNotificationNumber"">
                        <ns0:smsNotifNbr>
                          <xsl:value-of select=""s2:SMSNotificationNumber/text()"" />
                        </ns0:smsNotifNbr>
                      </xsl:if>
                      <xsl:if test=""s2:EmailNotificationAddress"">
                        <ns0:emailNotifAdr>
                          <xsl:value-of select=""s2:EmailNotificationAddress/text()"" />
                        </ns0:emailNotifAdr>
                      </xsl:if>
                      <xsl:if test=""s2:AddressType"">
                        <ns0:adrType>
                          <xsl:value-of select=""s2:AddressType/text()"" />
                        </ns0:adrType>
                      </xsl:if>
                    </ns0:address>
                  </xsl:for-each>
                </xsl:for-each>
              </ns0:adresses>
            </xsl:if>
            <xsl:if test=""string($var:v1)='true'"">
              <xsl:attribute name=""xsi:nil"">
                <xsl:value-of select=""'true'"" />
              </xsl:attribute>
            </xsl:if>
          </ns0:customer>
        </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select=""s3:GetCustomerResult"">
        <ns0:status>
          <xsl:if test=""s2:Status"">
            <ns0:status>
              <xsl:value-of select=""s2:Status/text()"" />
            </ns0:status>
          </xsl:if>
          <xsl:if test=""s2:Message"">
            <ns0:message>
              <xsl:value-of select=""s2:Message/text()"" />
            </ns0:message>
          </xsl:if>
        </ns0:status>
      </xsl:for-each>
    </ns0:crmCustomerResponse>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public bool LogicalNot(string val)
{
	return !ValToBool(val);
}


public bool IsNumeric(string val)
{
	if (val == null)
	{
		return false;
	}
	double d = 0;
	return Double.TryParse(val, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d);
}

public bool IsNumeric(string val, ref double d)
{
	if (val == null)
	{
		return false;
	}
	return Double.TryParse(val, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d);
}

public bool ValToBool(string val)
{
	if (val != null)
	{
		if (string.Compare(val, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return true;
		}
		if (string.Compare(val, bool.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return false;
		}
		val = val.Trim();
		if (string.Compare(val, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return true;
		}
		if (string.Compare(val, bool.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return false;
		}
		double d = 0;
		if (IsNumeric(val, ref d))
		{
			return (d > 0);
		}
	}
	return false;
}


]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCustomerResponse";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.GetCustomerResponse _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.STIP.Schemas.CompositeResponse";
        
        private const global::INTSTDK003.STIP.Schemas.CompositeResponse _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+GetCustomerResponse";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.STIP.Schemas.CompositeResponse";
                return _TrgSchemas;
            }
        }
    }
}
