namespace Overflow.AppHost.Configuration.Options;

internal sealed class FrontendOptions
{
	public const string SectionName = "AppHost:Frontend";

	public int Port { get; set; } = 3100;
}
