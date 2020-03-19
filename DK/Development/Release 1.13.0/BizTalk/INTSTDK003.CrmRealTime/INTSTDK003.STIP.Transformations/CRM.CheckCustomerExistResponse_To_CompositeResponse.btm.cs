namespace INTSTDK003.STIP.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+CheckCustomerExistResponse", typeof(global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.CheckCustomerExistResponse))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CompositeResponse", typeof(global::INTSTDK003.STIP.Schemas.CompositeResponse))]
    public sealed class CRM_CheckCustomerExistResponse_To_CompositeResponse : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s3 s1 s2 s0 userCSharp"" version=""1.0"" xmlns:ns0=""http://www.skanetrafiken.com/DK/INSTDK003/CompositeResponse/20141215"" xmlns:s1=""http://schemas.datacontract.org/2004/07/CGICRMPortalService"" xmlns:s3=""http://tempuri.org/"" xmlns:s2=""http://schemas.datacontract.org/2004/07/CGICRMPortalService.Models"" xmlns:s0=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s3:CheckCustomerExistResponse"" />
  </xsl:template>
  <xsl:template match=""/s3:CheckCustomerExistResponse"">
    <ns0:crmCustomerResponse>
      <xsl:for-each select=""s3:CheckCustomerExistResult"">
        <ns0:customerExists>
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
          <xsl:if test=""s2:CustomerExists"">
            <ns0:contactValExists>
              <xsl:value-of select=""s2:CustomerExists/text()"" />
            </ns0:contactValExists>
          </xsl:if>
        </ns0:customerExists>
      </xsl:for-each>
      <ns0:customer>
        <xsl:for-each select=""s3:CheckCustomerExistResult"">
          <xsl:variable name=""var:v1"" select=""string(s2:CustomerType/@xsi:nil) = 'true'"" />
          <xsl:variable name=""var:v2"" select=""userCSharp:LogicalNot(string($var:v1))"" />
          <xsl:if test=""$var:v2"">
            <ns0:account>
              <xsl:if test=""s2:CustomerType"">
                <xsl:variable name=""var:v3"" select=""string(s2:CustomerType/@xsi:nil) = 'true'"" />
                <xsl:if test=""string($var:v3)='true'"">
                  <ns0:custType>
                    <xsl:attribute name=""xsi:nil"">
                      <xsl:value-of select=""'true'"" />
                    </xsl:attribute>
                  </ns0:custType>
                </xsl:if>
                <xsl:if test=""string($var:v3)='false'"">
                  <ns0:custType>
                    <xsl:value-of select=""s2:CustomerType/text()"" />
                  </ns0:custType>
                </xsl:if>
              </xsl:if>
            </ns0:account>
          </xsl:if>
        </xsl:for-each>
      </ns0:customer>
      <xsl:for-each select=""s3:CheckCustomerExistResult"">
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
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+CheckCustomerExistResponse";
        
        private const global::INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService.CheckCustomerExistResponse _srcSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.CRM.Schemas.Customer_TravelCard.PortalService+CheckCustomerExistResponse";
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
