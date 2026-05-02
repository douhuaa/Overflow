using Microsoft.Extensions.Configuration;

namespace Overflow.AppHost.Configuration;

/// <summary>
/// Non-sensitive AppHost configuration for gateway, frontend, and edge infrastructure.
/// Infrastructure-specific options (ports, image tags) live in per-resource options classes
/// aggregated by <see cref="InfrastructureOptions"/>.
/// </summary>
internal sealed class AppHostOptions
{
	public const string SectionName = "AppHost";

	public AppHostPortOptions Ports { get; init; } = new();
	public AppHostImageTagOptions ImageTags { get; init; } = new();
	public AppHostVirtualHostOptions VirtualHosts { get; init; } = new();

	public static AppHostOptions FromConfiguration(IConfiguration configuration)
	{
		var opts = configuration.GetSection(SectionName).Get<AppHostOptions>() ?? new AppHostOptions();
		opts.Ports.Validate();
		opts.ImageTags.Validate();
		opts.VirtualHosts.Validate();
		return opts;
	}
}

internal sealed class AppHostPortOptions
{
	public int Dashboard { get; init; } = 8080;
	public int Gateway { get; init; } = 8001;
	public int WebApp { get; init; } = 3100;
	public int NginxProxy { get; init; } = 80;

	public void Validate()
	{
		if (Dashboard <= 0)
			throw new InvalidOperationException($"{AppHostOptions.SectionName}:Ports:{nameof(Dashboard)} must be > 0 (got {Dashboard}).");
		if (Gateway <= 0)
			throw new InvalidOperationException($"{AppHostOptions.SectionName}:Ports:{nameof(Gateway)} must be > 0 (got {Gateway}).");
		if (WebApp <= 0)
			throw new InvalidOperationException($"{AppHostOptions.SectionName}:Ports:{nameof(WebApp)} must be > 0 (got {WebApp}).");
		if (NginxProxy <= 0)
			throw new InvalidOperationException($"{AppHostOptions.SectionName}:Ports:{nameof(NginxProxy)} must be > 0 (got {NginxProxy}).");
	}
}

internal sealed class AppHostImageTagOptions
{
	public string NginxProxy { get; init; } = "1.8";

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(NginxProxy))
			throw new InvalidOperationException($"{AppHostOptions.SectionName}:ImageTags:{nameof(NginxProxy)} must not be empty.");
	}
}

internal sealed class AppHostVirtualHostOptions
{
	public string Api { get; init; } = "api.overflow.local";

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Api))
			throw new InvalidOperationException($"{AppHostOptions.SectionName}:VirtualHosts:{nameof(Api)} must not be empty.");
	}
}

