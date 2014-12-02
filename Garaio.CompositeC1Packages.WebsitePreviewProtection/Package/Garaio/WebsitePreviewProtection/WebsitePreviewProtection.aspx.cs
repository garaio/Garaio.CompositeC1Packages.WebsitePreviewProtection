using System;
using System.Web.Security;
using System.Web.UI;

namespace Garaio.CompositeC1Packages.WebsitePreviewProtection.Package.Garaio.WebsitePreviewProtection
{
	public partial class WebsitePreviewProtection : Page
	{
		private readonly WebsitePreviewProtectionManager _websitePreviewProtectionManager = new WebsitePreviewProtectionManager();

		public string ValidationMessage { get; private set; }

		protected override void OnLoad(EventArgs e)
		{
			if (Request.RequestType == "POST")
			{
				OnPost();
			}

			base.OnLoad(e);
		}

		private void OnPost()
		{
			if (_websitePreviewProtectionManager.CheckAccess(Request.Form[txtUsername.Name], Request.Form[txtPassword.Name]))
			{
				FormsAuthentication.RedirectFromLoginPage(Request.Form[txtUsername.Name], false);
			}
			else
			{
				ValidationMessage = _websitePreviewProtectionManager.LastLogMessage;
				LoginFailedContainer.Visible = true;
			}
		}
	}
}
