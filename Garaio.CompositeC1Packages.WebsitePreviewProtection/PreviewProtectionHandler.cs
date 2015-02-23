using System;
using System.Linq;
using System.Web;

namespace Garaio.CompositeC1Packages.WebsitePreviewProtection
{
	public class PreviewProtectionHandler : IHttpModule
	{
		public const string PreviewProtectionReturnUrlParameter = "ReturnUrl";

		private const string PreviewProtectionBaseUrl = "/Garaio/WebsitePreviewProtection/";
		private const string PreviewProtectionPage = "WebsitePreviewProtection.aspx";
		private readonly string[] _excludedDirectories = {"/Composite/", "/Frontend/"};

		public void Init(HttpApplication context)
		{
			context.BeginRequest += OnBeginRequest;
		}

		public void Dispose()
		{
		}

		private void OnBeginRequest(object sender, EventArgs eventArgs)
		{
			ProcessRequest(HttpContext.Current);
		}

		private void ProcessRequest(HttpContext context)
		{
			var previewProtectionManager = new WebsitePreviewProtectionManager();
			var originalUrl = context.Request.Url.OriginalString;

			if (!IsExcludedDirectory(context.Request.Url.PathAndQuery) && !previewProtectionManager.IsAuthenticated && !originalUrl.Contains(PreviewProtectionPage))
			{
				var previewProtectionPageUrl = string.Concat(PreviewProtectionBaseUrl, PreviewProtectionPage, "?", PreviewProtectionReturnUrlParameter, "=", HttpUtility.UrlEncode(originalUrl));
				context.Server.Transfer(previewProtectionPageUrl, true);
			}
		}

		private bool IsExcludedDirectory(string path)
		{
			return _excludedDirectories.Any(path.StartsWith);
		}
	}
}