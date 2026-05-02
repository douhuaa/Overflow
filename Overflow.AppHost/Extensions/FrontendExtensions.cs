using Aspire.Hosting.Keycloak;
using Overflow.AppHost.Configuration;
using Overflow.AppHost.Configuration.Options;

namespace Overflow.AppHost.Extensions;

internal static class FrontendExtensions
{
	public static void AddFrontend(
		this IDistributedApplicationBuilder builder,
		IResourceBuilder<KeycloakResource> identity,
		FrontendOptions options)
	{
		builder
			.AddNpmApp(AppHostResourceNames.WebApp, "../webapp", "dev")
			.WithReference(identity)
			.WithHttpEndpoint(env: EnvironmentVariableNames.Port, port: options.Port);
	}
}
