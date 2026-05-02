namespace Overflow.AppHost.Configuration.Options;

internal sealed class ReverseProxyOptions
{
	public const string SectionName = "AppHost:ReverseProxy";

	public int Port { get; set; } = 80;
	public string ImageTag { get; set; } = "1.8";
}
