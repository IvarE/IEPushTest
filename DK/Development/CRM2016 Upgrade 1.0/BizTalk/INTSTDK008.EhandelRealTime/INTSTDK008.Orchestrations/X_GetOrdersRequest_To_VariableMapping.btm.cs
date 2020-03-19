namespace INTSTDK008.Orchestrations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Ehandel.Schemas.GetOrdersRequest", typeof(global::INTSTDK008.Ehandel.Schemas.GetOrdersRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK008.Orchestrations.BtsHttpUrlMapping", typeof(global::INTSTDK008.Orchestrations.BtsHttpUrlMapping))]
    public sealed class X_GetOrdersRequest_To_VariableMapping : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 userCSharp"" version=""1.0"" xmlns:s0=""http://INTSTDK008.Ehandel.Schemas.GetOrdersRequest"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:GetOrdersRequest"" />
  </xsl:template>
  <xsl:template match=""/s0:GetOrdersRequest"">
    <xsl:variable name=""var:v1"" select=""userCSharp:StringConcat(&quot;GET&quot;)"" />
    <BtsHttpUrlMapping>
      <Operation>
        <xsl:attribute name=""Method"">
          <xsl:value-of select=""$var:v1"" />
        </xsl:attribute>
        <xsl:variable name=""var:v2"" select=""userCSharp:MakeUrl(string(CustomerId/text()) , string(OrderNumber/text()) , string(From/text()) , string(To/text()) , string(Email/text()) , string(CardNumber/text()))"" />
        <xsl:attribute name=""Url"">
          <xsl:value-of select=""$var:v2"" />
        </xsl:attribute>
      </Operation>
    </BtsHttpUrlMapping>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string MakeUrl(string custId, string orderNo, string from, string to, string email, string cardNumber)
{
      System.Collections.Generic.List<string> paramList = new System.Collections.Generic.List<string>();
      string url = string.Empty;
  
      if (!string.IsNullOrEmpty(custId))
            paramList.Add(""customerId=""+custId);
      if (!string.IsNullOrEmpty(orderNo))
            paramList.Add(""orderNumber=""+orderNo);
      if (!string.IsNullOrEmpty(from))
            paramList.Add(""from=""+from);
      if (!string.IsNullOrEmpty(to))
            paramList.Add(""to=""+to);
     if (!string.IsNullOrEmpty(email))
            paramList.Add(""email=""+email);
     if (!string.IsNullOrEmpty(cardNumber))
            paramList.Add(""cardNumber=""+cardNumber);

      int icount = paramList.Count;
      int it = 1;
      url += ""orders?"";
      foreach (var item in paramList)
      {
           if (it < icount)
               url += item + ""&"";
           else
               url += item;
           it++;
       }
       return url;
}


public string StringConcat(string param0)
{
   return param0;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK008.Ehandel.Schemas.GetOrdersRequest";
        
        private const global::INTSTDK008.Ehandel.Schemas.GetOrdersRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK008.Orchestrations.BtsHttpUrlMapping";
        
        private const global::INTSTDK008.Orchestrations.BtsHttpUrlMapping _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK008.Ehandel.Schemas.GetOrdersRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK008.Orchestrations.BtsHttpUrlMapping";
                return _TrgSchemas;
            }
        }
    }
}
