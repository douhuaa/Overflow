using Microsoft.Extensions.Configuration;

namespace Overflow.AppHost.Configuration;

internal sealed class AppHostOptions
{
	public PortOptions Ports { get; init; } = new();
	public ImageTagOptions ImageTags { get; init; } = new();
	public VirtualHostOptions VirtualHosts { get; init; } = new();

	public static AppHostOptions FromConfiguration(IConfiguration configuration) =>
		configuration.GetSection("AppHost").Get<AppHostOptions>() ?? new AppHostOptions();
}

internal sealed class PortOptions
{
	public int Dashboard { get; init; } = 8080;
	public int Keycloak { get; init; } = 6001;
	public int KeycloakInternal { get; init; } = 8080;
	public int Postgres { get; init; } = 5432;
	public int Typesense { get; init; } = 8108;
	public int PgAdmin { get; init; } = 5050;
	public int RabbitMqManagement { get; init; } = 15672;
	public int Gateway { get; init; } = 8001;
	public int WebApp { get; init; } = 3100;
	public int NginxProxy { get; init; } = 80;
}

internal sealed class ImageTagOptions
{
	public string Typesense { get; init; } = "29.0";
	public string PgAdmin { get; init; } = "9.9";
	public string NginxProxy { get; init; } = "1.8";
}

internal sealed class VirtualHostOptions
{
	public string Api { get; init; } = "api.overflow.local";
	public string Identity { get; init; } = "id.overflow.local";
}
