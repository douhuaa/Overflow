namespace Overflow.AppHost.Configuration;

internal sealed class IdentityOptions
{
	public const string SectionName = "Infrastructure:Identity";

	public int HostPort { get; init; } = 6001;
	public int InternalHttpPort { get; init; } = 8080;
	public string VirtualHost { get; init; } = "id.overflow.local";
}
