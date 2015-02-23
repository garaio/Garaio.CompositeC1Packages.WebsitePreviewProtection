<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebsitePreviewProtection.aspx.cs" Inherits="Garaio.CompositeC1Packages.WebsitePreviewProtection.WebsitePreviewProtection" %>
<%@ Import Namespace="Garaio.CompositeC1Packages.WebsitePreviewProtection" %>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title><%= Garaio_WebsitePreviewProtection.Page_Title%></title>

		<meta name="robots" content="noindex, nofollow" />
		<meta http-equiv="X-UA-Compatible" content="IE=9" />

		<script type="text/javascript">
			window.onload = function() {
				var txtUsername = document.getElementById('<%= txtUsername.ClientID %>');
				var txtPassword = document.getElementById('<%= txtPassword.ClientID %>');
				var btnLogin = document.getElementById('<%= btnLogin.ClientID %>');

				txtPassword.onkeydown = function(e) {
					if (e.keyCode == 13) {
						submitForm();
					}
				};
				txtUsername.onkeydown = function(e) {
					if (e.keyCode == 13) {
						submitForm();
					}
				};
				btnLogin.onclick = function() {
					submitForm();
				};
				txtUsername.focus();
			};

			function submitForm() {
				document.getElementById('<%= btnLogin.ClientID %>').click();
			}
		</script>

		<style type="text/css">
			body {
				background-color: #F0F0F0;
				font-family: "Segoe UI", Tahoma, sans-serif;
				font-size: 12px;
			}

			div#splash {
				color: #B2B2B2;
				display: block;
				height: 360px;
				left: 50%;
				margin-left: -180px;
				margin-top: -157px;
				overflow-x: hidden;
				overflow-y: hidden;
				position: absolute;
				top: 45%;
				width: 315px;
				z-index: 5;
			}

			div#splashcontent {
				clear: both;
				display: block;
				height: 180px;
				height: 100%;
				margin-left: 58px;
				margin-top: 140px;
				overflow-x: hidden;
				overflow-y: hidden;
				position: relative;
				position: static;
				position: absolute;
				width: 200px;
				z-index: 2;
			}

			div#splashcontent div.splashtext {
				height: 95px;
			}

			div#backdrop {
				background-image: url("../../Composite/images/box.png");
				height: 100%;
				left: 0;
				position: absolute;
				top: 0;
				width: 100%;
			}

			/* Forms */

			div.field div.label {
				float: left;
				font-weight: bold;
				padding-top: 7px;
				width: 60px;
			}

			div.field div.value {
				float: right;
				height: 23px;
				padding-top: 4px;
			}

			div.field div.value input {
				border-color: rgb(127, 157, 185);
				border-style: solid;
				border-width: 1px;
				max-height: 25px;
				padding: 4px 0 4px 7px;
				width: 130px;
			}

			div.field div.value input:focus {
				outline-color: #B2B2B2;
				outline-style: solid;
				outline-width: 1px;
			}

			input.error {
				border-color: #CC071E !important;
			}

			div#LoginFailedContainer {
				color: #CC071E;
				font-weight: bold;
			}

			div.LoginButton {
				float: right;
				padding-top: 5px;
			}

			div.LoginButton a {
				background-color: #CC071E;
				background-image: url("../../Composite/skins/system/splash/button.png");
				background-position: 8px 7px;
				background-repeat: no-repeat;
				border-radius: 10px;
				color: white;
				display: inline-block;
				font-weight: bold;
				height: 16px;
				padding: 6px 10px 6px 30px;
				text-decoration: none;
				text-transform: uppercase;
				vertical-align: central;
			}

			div.LoginButton a:hover {
				background-color: #D63548;
			}

			div.clear {
				clear: both;
			}
		</style>
	</head>
	<body>
		<div id="splash">
			<div id="backdrop"></div>
			<div id="splashcontent">
				<form runat="server" id="frmLogin" method="post">
					<div class="splashtext">
						<%= Garaio_WebsitePreviewProtection.Splash_Text%>
						<div id="LoginFailedContainer" runat="server" visible="false">
							<p><%= ValidationMessage%></p>
						</div>
					</div>
					<div class="fields">
						<div class="field">
							<div class="label">
								<label for="txtUsername"><%= Garaio_WebsitePreviewProtection.Label_Username_Text%></label>
							</div>
							<div class="value">
								<input runat="server" tabindex="1" type="text" id="txtUsername" />
							</div>
							<div class="clear"></div>
						</div>
						<div class="field">
							<div class="label">
								<label for="txtPassword"><%= Garaio_WebsitePreviewProtection.Label_Password_Text%></label>
							</div>
							<div class="value">
								<input runat="server" tabindex="2" type="password" id="txtPassword" autocomplete="off" />
							</div>
							<div class="clear"></div>
						</div>
						<div class="LoginButton">
							<asp:LinkButton runat="server" ID="btnLogin" TabIndex="3" OnClick="LoginUser">
								<%= Garaio_WebsitePreviewProtection.Button_Login_Text%>
							</asp:LinkButton>
						</div>
					</div>
				</form>
			</div>
		</div>
	</body>
</html>