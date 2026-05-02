namespace Overflow.AppHost.Configuration.Options;

internal sealed class AppHostOptions
{
	public const string SectionName = "AppHost";

	public int DashboardPort { get; set; } = 8080;
	public GatewayOptions Gateway { get; set; } = new();
	public FrontendOptions Frontend { get; set; } = new();
	public ReverseProxyOptions ReverseProxy { get; set; } = new();
}
