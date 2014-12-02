<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="configuration/system.web">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:if test="count(authentication) = 0">
				<authentication mode="Forms">
					<forms loginUrl="~/Garaio/WebsitePreviewProtection/WebsitePreviewProtection.aspx" name=".ASPXFORMSAUTH"  protection="All" timeout="30" path="/" />
				</authentication>
			</xsl:if>
			<xsl:if test="count(authorization) = 0">
				<authorization>
					<deny users="?" />
				</authorization>
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="configuration/appSettings">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<add key="ValidationSettings:UnobtrusiveValidationMode" value="None" />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="configuration">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:if test="count(appSettings) = 0">
				<appSettings>
					<add key="ValidationSettings:UnobtrusiveValidationMode" value="None" />
				</appSettings>
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
</xsl:stylesheet>
