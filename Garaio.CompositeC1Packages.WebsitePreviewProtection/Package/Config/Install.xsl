<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="@* | node()">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="configuration/system.web/httpModules">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:if test="count(add[@name='WebsitePreviewProtectionModule']) = 0">
				<add name="WebsitePreviewProtectionModule" type="Garaio.CompositeC1Packages.WebsitePreviewProtection.PreviewProtectionHandler, Garaio.CompositeC1Packages.WebsitePreviewProtection" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="configuration/system.webServer/modules">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:if test="count(add[@name='WebsitePreviewProtectionModule']) = 0">
				<remove name="WebsitePreviewProtectionModule" />
				<add name="WebsitePreviewProtectionModule" type="Garaio.CompositeC1Packages.WebsitePreviewProtection.PreviewProtectionHandler, Garaio.CompositeC1Packages.WebsitePreviewProtection" />
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="configuration/appSettings">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" />
			<xsl:if test="count(add[@key='ValidationSettings:UnobtrusiveValidationMode']) = 0">
				<add key="ValidationSettings:UnobtrusiveValidationMode" value="None" />
			</xsl:if>
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
