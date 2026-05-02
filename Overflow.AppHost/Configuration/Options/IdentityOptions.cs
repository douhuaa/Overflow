namespace Overflow.AppHost.Configuration.Options;

internal sealed class IdentityOptions
{
	public const string SectionName = "Infrastructure:Identity";

	public int HostPort { get; set; } = 6001;
	public int InternalHttpPort { get; set; } = 8080;
	public string VirtualHost { get; set; } = "id.overflow.local";
}
