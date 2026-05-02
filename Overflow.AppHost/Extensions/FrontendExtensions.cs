using Aspire.Hosting.Keycloak;
using Overflow.AppHost.Configuration;

namespace Overflow.AppHost.Extensions;

internal static class FrontendExtensions
{
	public static void AddFrontend(
		this IDistributedApplicationBuilder builder,
		IResourceBuilder<KeycloakResource> identity)
	{
		var options = AppHostOptions.FromConfiguration(builder.Configuration);
		builder
			.AddNpmApp("webapp", "../webapp", "dev")
			.WithReference(identity)
			.WithHttpEndpoint(env: EnvironmentVariableNames.Port, port: options.Ports.WebApp);
	}
}
