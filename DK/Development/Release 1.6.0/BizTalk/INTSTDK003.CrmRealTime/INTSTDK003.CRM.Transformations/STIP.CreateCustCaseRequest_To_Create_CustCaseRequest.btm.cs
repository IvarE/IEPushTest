namespace INTSTDK003.CRM.Transformations {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase+RequestCreateCase", typeof(global::INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase.RequestCreateCase))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase+RequestCreateCase", typeof(global::INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase.RequestCreateCase))]
    public sealed class STIP_CreateCustCaseRequest_To_Create_CustCaseRequest : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s2 s1 s0"" version=""1.0"" xmlns:s1=""http://www.skanetrafiken.com/DK/INTSTDK003/CreateCase/20141201"" xmlns:s2=""http://www.skanetrafiken.com/DK/INTSTDK003/CreateCase_1_2/20141201"" xmlns:ns2=""http://schemas.microsoft.com/2003/10/Serialization/"" xmlns:ns0=""http://tempuri.org/"" xmlns:s0=""http://www.skanetrafiken.com/DK/INTSTDK003/CreateCase_1/20141201"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/CGIXrmCreateCaseService"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:RequestCreateCase"" />
  </xsl:template>
  <xsl:template match=""/s1:RequestCreateCase"">
    <ns0:RequestCreateCase>
      <xsl:for-each select=""s1:request"">
        <ns0:request>
          <xsl:if test=""s0:CardNumber"">
            <xsl:variable name=""var:v1"" select=""string(s0:CardNumber/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v1)='true'"">
              <ns1:CardNumber>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:CardNumber>
            </xsl:if>
            <xsl:if test=""string($var:v1)='false'"">
              <ns1:CardNumber>
                <xsl:value-of select=""s0:CardNumber/text()"" />
              </ns1:CardNumber>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:City"">
            <xsl:variable name=""var:v2"" select=""string(s0:City/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v2)='true'"">
              <ns1:City>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:City>
            </xsl:if>
            <xsl:if test=""string($var:v2)='false'"">
              <ns1:City>
                <xsl:value-of select=""s0:City/text()"" />
              </ns1:City>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:ContactCustomer"">
            <ns1:ContactCustomer>
              <xsl:value-of select=""s0:ContactCustomer/text()"" />
            </ns1:ContactCustomer>
          </xsl:if>
          <xsl:if test=""s0:ControlFeeNumber"">
            <xsl:variable name=""var:v3"" select=""string(s0:ControlFeeNumber/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v3)='true'"">
              <ns1:ControlFeeNumber>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:ControlFeeNumber>
            </xsl:if>
            <xsl:if test=""string($var:v3)='false'"">
              <ns1:ControlFeeNumber>
                <xsl:value-of select=""s0:ControlFeeNumber/text()"" />
              </ns1:ControlFeeNumber>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:County"">
            <xsl:variable name=""var:v4"" select=""string(s0:County/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v4)='true'"">
              <ns1:County>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:County>
            </xsl:if>
            <xsl:if test=""string($var:v4)='false'"">
              <ns1:County>
                <xsl:value-of select=""s0:County/text()"" />
              </ns1:County>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:Customer"">
            <xsl:variable name=""var:v5"" select=""string(s0:Customer/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v5)='true'"">
              <ns1:Customer>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:Customer>
            </xsl:if>
            <xsl:if test=""string($var:v5)='false'"">
              <ns1:Customer>
                <xsl:value-of select=""s0:Customer/text()"" />
              </ns1:Customer>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:CustomerType"">
            <ns1:CustomerType>
              <xsl:value-of select=""s0:CustomerType/text()"" />
            </ns1:CustomerType>
          </xsl:if>
          <xsl:if test=""s0:CustomersCategory"">
            <xsl:variable name=""var:v6"" select=""string(s0:CustomersCategory/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v6)='true'"">
              <ns1:CustomersCategory>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:CustomersCategory>
            </xsl:if>
            <xsl:if test=""string($var:v6)='false'"">
              <ns1:CustomersCategory>
                <xsl:value-of select=""s0:CustomersCategory/text()"" />
              </ns1:CustomersCategory>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:CustomersSubcategory"">
            <xsl:variable name=""var:v7"" select=""string(s0:CustomersSubcategory/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v7)='true'"">
              <ns1:CustomersSubcategory>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:CustomersSubcategory>
            </xsl:if>
            <xsl:if test=""string($var:v7)='false'"">
              <ns1:CustomersSubcategory>
                <xsl:value-of select=""s0:CustomersSubcategory/text()"" />
              </ns1:CustomersSubcategory>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:Description"">
            <xsl:variable name=""var:v8"" select=""string(s0:Description/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v8)='true'"">
              <ns1:Description>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:Description>
            </xsl:if>
            <xsl:if test=""string($var:v8)='false'"">
              <ns1:Description>
                <xsl:value-of select=""s0:Description/text()"" />
              </ns1:Description>
            </xsl:if>
          </xsl:if>
          <xsl:for-each select=""s0:DocumentList"">
            <ns1:DocumentList>
              <xsl:for-each select=""s0:document"">
                <ns1:document>
                  <xsl:if test=""s0:DocumentBody"">
                    <xsl:variable name=""var:v9"" select=""string(s0:DocumentBody/@xsi:nil) = 'true'"" />
                    <xsl:if test=""string($var:v9)='true'"">
                      <ns1:DocumentBody>
                        <xsl:attribute name=""xsi:nil"">
                          <xsl:value-of select=""'true'"" />
                        </xsl:attribute>
                      </ns1:DocumentBody>
                    </xsl:if>
                    <xsl:if test=""string($var:v9)='false'"">
                      <ns1:DocumentBody>
                        <xsl:value-of select=""s0:DocumentBody/text()"" />
                      </ns1:DocumentBody>
                    </xsl:if>
                  </xsl:if>
                  <xsl:if test=""s0:FileName"">
                    <xsl:variable name=""var:v10"" select=""string(s0:FileName/@xsi:nil) = 'true'"" />
                    <xsl:if test=""string($var:v10)='true'"">
                      <ns1:FileName>
                        <xsl:attribute name=""xsi:nil"">
                          <xsl:value-of select=""'true'"" />
                        </xsl:attribute>
                      </ns1:FileName>
                    </xsl:if>
                    <xsl:if test=""string($var:v10)='false'"">
                      <ns1:FileName>
                        <xsl:value-of select=""s0:FileName/text()"" />
                      </ns1:FileName>
                    </xsl:if>
                  </xsl:if>
                  <xsl:if test=""s0:NoteText"">
                    <xsl:variable name=""var:v11"" select=""string(s0:NoteText/@xsi:nil) = 'true'"" />
                    <xsl:if test=""string($var:v11)='true'"">
                      <ns1:NoteText>
                        <xsl:attribute name=""xsi:nil"">
                          <xsl:value-of select=""'true'"" />
                        </xsl:attribute>
                      </ns1:NoteText>
                    </xsl:if>
                    <xsl:if test=""string($var:v11)='false'"">
                      <ns1:NoteText>
                        <xsl:value-of select=""s0:NoteText/text()"" />
                      </ns1:NoteText>
                    </xsl:if>
                  </xsl:if>
                  <xsl:if test=""s0:Subject"">
                    <xsl:variable name=""var:v12"" select=""string(s0:Subject/@xsi:nil) = 'true'"" />
                    <xsl:if test=""string($var:v12)='true'"">
                      <ns1:Subject>
                        <xsl:attribute name=""xsi:nil"">
                          <xsl:value-of select=""'true'"" />
                        </xsl:attribute>
                      </ns1:Subject>
                    </xsl:if>
                    <xsl:if test=""string($var:v12)='false'"">
                      <ns1:Subject>
                        <xsl:value-of select=""s0:Subject/text()"" />
                      </ns1:Subject>
                    </xsl:if>
                  </xsl:if>
                  <xsl:value-of select=""./text()"" />
                </ns1:document>
              </xsl:for-each>
              <xsl:value-of select=""./text()"" />
            </ns1:DocumentList>
          </xsl:for-each>
          <xsl:if test=""s0:EmailAddress"">
            <xsl:variable name=""var:v13"" select=""string(s0:EmailAddress/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v13)='true'"">
              <ns1:EmailAddress>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:EmailAddress>
            </xsl:if>
            <xsl:if test=""string($var:v13)='false'"">
              <ns1:EmailAddress>
                <xsl:value-of select=""s0:EmailAddress/text()"" />
              </ns1:EmailAddress>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:FirstName"">
            <xsl:variable name=""var:v14"" select=""string(s0:FirstName/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v14)='true'"">
              <ns1:FirstName>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:FirstName>
            </xsl:if>
            <xsl:if test=""string($var:v14)='false'"">
              <ns1:FirstName>
                <xsl:value-of select=""s0:FirstName/text()"" />
              </ns1:FirstName>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:InvoiceNumber"">
            <xsl:variable name=""var:v15"" select=""string(s0:InvoiceNumber/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v15)='true'"">
              <ns1:InvoiceNumber>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:InvoiceNumber>
            </xsl:if>
            <xsl:if test=""string($var:v15)='false'"">
              <ns1:InvoiceNumber>
                <xsl:value-of select=""s0:InvoiceNumber/text()"" />
              </ns1:InvoiceNumber>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:LastName"">
            <xsl:variable name=""var:v16"" select=""string(s0:LastName/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v16)='true'"">
              <ns1:LastName>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:LastName>
            </xsl:if>
            <xsl:if test=""string($var:v16)='false'"">
              <ns1:LastName>
                <xsl:value-of select=""s0:LastName/text()"" />
              </ns1:LastName>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:Line"">
            <xsl:variable name=""var:v17"" select=""string(s0:Line/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v17)='true'"">
              <ns1:Line>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:Line>
            </xsl:if>
            <xsl:if test=""string($var:v17)='false'"">
              <ns1:Line>
                <xsl:value-of select=""s0:Line/text()"" />
              </ns1:Line>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:MobilePhoneNumber"">
            <xsl:variable name=""var:v18"" select=""string(s0:MobilePhoneNumber/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v18)='true'"">
              <ns1:MobilePhoneNumber>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:MobilePhoneNumber>
            </xsl:if>
            <xsl:if test=""string($var:v18)='false'"">
              <ns1:MobilePhoneNumber>
                <xsl:value-of select=""s0:MobilePhoneNumber/text()"" />
              </ns1:MobilePhoneNumber>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:Title"">
            <xsl:variable name=""var:v19"" select=""string(s0:Title/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v19)='true'"">
              <ns1:Title>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:Title>
            </xsl:if>
            <xsl:if test=""string($var:v19)='false'"">
              <ns1:Title>
                <xsl:value-of select=""s0:Title/text()"" />
              </ns1:Title>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:Train"">
            <xsl:variable name=""var:v20"" select=""string(s0:Train/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v20)='true'"">
              <ns1:Train>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:Train>
            </xsl:if>
            <xsl:if test=""string($var:v20)='false'"">
              <ns1:Train>
                <xsl:value-of select=""s0:Train/text()"" />
              </ns1:Train>
            </xsl:if>
          </xsl:if>
          <xsl:if test=""s0:WayOfTravel"">
            <xsl:variable name=""var:v21"" select=""string(s0:WayOfTravel/@xsi:nil) = 'true'"" />
            <xsl:if test=""string($var:v21)='true'"">
              <ns1:WayOfTravel>
                <xsl:attribute name=""xsi:nil"">
                  <xsl:value-of select=""'true'"" />
                </xsl:attribute>
              </ns1:WayOfTravel>
            </xsl:if>
            <xsl:if test=""string($var:v21)='false'"">
              <ns1:WayOfTravel>
                <xsl:value-of select=""s0:WayOfTravel/text()"" />
              </ns1:WayOfTravel>
            </xsl:if>
          </xsl:if>
        </ns0:request>
      </xsl:for-each>
    </ns0:RequestCreateCase>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase+RequestCreateCase";
        
        private const global::INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase.RequestCreateCase _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase+RequestCreateCase";
        
        private const global::INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase.RequestCreateCase _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"INTSTDK003.STIP.Schemas.CreateCustCase.CreateCase+RequestCreateCase";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"INTSTDK003.CRM.Schemas.CreateCustCase.CreateCase+RequestCreateCase";
                return _TrgSchemas;
            }
        }
    }
}
