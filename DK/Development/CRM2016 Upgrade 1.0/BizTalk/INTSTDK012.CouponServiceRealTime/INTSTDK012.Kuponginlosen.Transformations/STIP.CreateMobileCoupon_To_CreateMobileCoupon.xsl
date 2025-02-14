<?xml version="1.0" encoding="UTF-16"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:var="http://schemas.microsoft.com/BizTalk/2003/var" exclude-result-prefixes="msxsl var s0" version="1.0" xmlns:s0="http://INTSTDK012.STIP.CouponService.Schemas/20150316" xmlns:ns0="http://service.web.couponcreator.kuponginlosen.se/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output omit-xml-declaration="yes" method="xml" version="1.0" />
  <xsl:template match="/">
    <xsl:apply-templates select="/s0:createMobileCoupon" />
  </xsl:template>
  <xsl:template match="/s0:createMobileCoupon">
    <ns0:createMobileCoupon>
      <xsl:for-each select="campaignNumberRequest">
        <campaignNumberRequest>
          <validityInDays>
            <xsl:value-of select="validityInDays/text()" />
          </validityInDays>
          <campaignNumber>
            <xsl:value-of select="campaignNumber/text()" />
          </campaignNumber>
          <amount>
            <xsl:value-of select="amount/text()" />
          </amount>
          <xsl:if test="quantity">
            <quantity>
              <xsl:value-of select="quantity/text()" />
            </quantity>
          </xsl:if>
          <xsl:if test="firstName">
            <firstName>
              <xsl:value-of select="firstName/text()" />
            </firstName>
          </xsl:if>
          <xsl:if test="familyName">
            <familyName>
              <xsl:value-of select="familyName/text()" />
            </familyName>
          </xsl:if>
          <xsl:if test="socialSecurityNumber">
            <socialSecurityNumber>
              <xsl:value-of select="socialSecurityNumber/text()" />
            </socialSecurityNumber>
          </xsl:if>
          <xsl:if test="street">
            <street>
              <xsl:value-of select="street/text()" />
            </street>
          </xsl:if>
          <xsl:if test="streetNumber">
            <streetNumber>
              <xsl:value-of select="streetNumber/text()" />
            </streetNumber>
          </xsl:if>
          <xsl:if test="coAddress">
            <coAddress>
              <xsl:value-of select="coAddress/text()" />
            </coAddress>
          </xsl:if>
          <xsl:if test="zipCode">
            <zipCode>
              <xsl:value-of select="zipCode/text()" />
            </zipCode>
          </xsl:if>
          <xsl:if test="city">
            <city>
              <xsl:value-of select="city/text()" />
            </city>
          </xsl:if>
          <xsl:value-of select="./text()" />
        </campaignNumberRequest>
      </xsl:for-each>
    </ns0:createMobileCoupon>
  </xsl:template>
</xsl:stylesheet>