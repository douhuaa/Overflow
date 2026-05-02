using Overflow.AppHost.Configuration.Options;

namespace Overflow.AppHost.Extensions;

internal static class AppEnvironmentExtensions
{
	public static void AddAppEnvironment(
		this IDistributedApplicationBuilder builder,
		AppHostOptions options)
	{
		builder
			.AddDockerComposeEnvironment("production")
			.WithDashboard(dashboard => dashboard.WithHostPort(options.DashboardPort));
	}
}
