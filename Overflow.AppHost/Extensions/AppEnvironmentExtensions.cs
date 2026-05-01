using Overflow.AppHost.Configuration;

namespace Overflow.AppHost.Extensions;

internal static class AppEnvironmentExtensions
{
	public static void AddAppEnvironment(this IDistributedApplicationBuilder builder)
	{
		builder
			.AddDockerComposeEnvironment("production")
			.WithDashboard(dashboard => dashboard.WithHostPort(AppHostConstants.DashboardPort));
	}
}
