using System;
using System.Web.UI;

namespace Garaio.CompositeC1Packages.WebsitePreviewProtection
{
	public partial class WebsitePreviewProtection : Page
	{
		public string ValidationMessage { get; private set; }

		protected void LoginUser(object sender, EventArgs e)
		{
			var ppManager = new WebsitePreviewProtectionManager();
			string validationMessage;

			if (ppManager.CheckAccess(txtUsername.Value, txtPassword.Value, out validationMessage))
			{
				var returnUrl = Request.QueryString[PreviewProtectionHandler.PreviewProtectionReturnUrlParameter];

				Response.Redirect(returnUrl);
			}
			else
			{
				ValidationMessage = validationMessage;
				LoginFailedContainer.Visible = true;
			}			
		}
	}
}
