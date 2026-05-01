namespace Overflow.AppHost.Configuration;

internal static class AppHostConstants
{
	public const int DashboardPort = 8080;
	public const int KeycloakPort = 6001;
	public const int PostgresPort = 5432;
	public const int TypesensePort = 8108;
	public const int PgAdminPort = 5050;
	public const int RabbitMqManagementPort = 15672;
	public const int GatewayPort = 8001;
	public const int WebAppPort = 3100;
	public const int NginxProxyPort = 80;

	public const string ApiVirtualHost = "api.overflow.local";
	public const string IdentityVirtualHost = "id.overflow.local";

	public static class ImageTags
	{
		public const string Typesense = "29.0";
		public const string PgAdmin = "9.9";
		public const string NginxProxy = "1.8";
	}
}
