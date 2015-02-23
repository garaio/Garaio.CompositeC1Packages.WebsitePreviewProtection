<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:output method="xml" indent="yes" />
		<xsl:template match="@* | node()">
			<xsl:copy>
				<xsl:apply-templates select="@* | node()" />
		</xsl:copy>
	</xsl:template>
	<xsl:template match="configuration/system.web/httpModules/add[@name='WebsitePreviewProtectionModule']" />
	<xsl:template match="configuration/system.webServer/modules/remove[@name='WebsitePreviewProtectionModule']" />
	<xsl:template match="configuration/system.webServer/modules/add[@name='WebsitePreviewProtectionModule']" />
	<xsl:template match="configuration/appSettings/add[@key='ValidationSettings:UnobtrusiveValidationMode']" />
</xsl:stylesheet>