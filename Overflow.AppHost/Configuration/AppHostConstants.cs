namespace Overflow.AppHost.Configuration;

internal static class AppHostConstants
{
	public const int DashboardPort = 8080;
	public const int GatewayPort = 8001;
	public const int WebAppPort = 3100;
	public const int NginxProxyPort = 80;

	public const string ApiVirtualHost = "api.overflow.local";

	public static class ImageTags
	{
		public const string NginxProxy = "1.8";
	}
}
