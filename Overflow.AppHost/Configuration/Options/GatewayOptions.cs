namespace Overflow.AppHost.Configuration.Options;

internal sealed class GatewayOptions
{
	public const string SectionName = "AppHost:Gateway";

	public int Port { get; set; } = 8001;
	public string VirtualHost { get; set; } = "api.overflow.local";
}
