using Microsoft.Extensions.Configuration;

namespace Overflow.AppHost.Configuration;

/// <summary>
/// Non-sensitive AppHost configuration for gateway, frontend, and edge infrastructure.
/// Infrastructure-specific options (ports, image tags) live in <see cref="InfrastructureOptions"/>.
/// </summary>
internal sealed class AppHostOptions
{
	public const string SectionName = "AppHost";

	public AppHostPortOptions Ports { get; init; } = new();
	public AppHostImageTagOptions ImageTags { get; init; } = new();
	public AppHostVirtualHostOptions VirtualHosts { get; init; } = new();

	public static AppHostOptions FromConfiguration(IConfiguration configuration) =>
		configuration.GetSection(SectionName).Get<AppHostOptions>() ?? new AppHostOptions();
}

internal sealed class AppHostPortOptions
{
	public int Dashboard { get; init; } = 8080;
	public int Gateway { get; init; } = 8001;
	public int WebApp { get; init; } = 3100;
	public int NginxProxy { get; init; } = 80;
}

internal sealed class AppHostImageTagOptions
{
	public string NginxProxy { get; init; } = "1.8";
}

internal sealed class AppHostVirtualHostOptions
{
	public string Api { get; init; } = "api.overflow.local";
}
