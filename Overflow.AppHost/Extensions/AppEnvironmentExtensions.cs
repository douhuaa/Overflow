using Microsoft.Extensions.Hosting;
using Overflow.AppHost.Configuration;

namespace Overflow.AppHost.Extensions;

internal static class AppEnvironmentExtensions
{
	public static void AddAppEnvironment(this IDistributedApplicationBuilder builder)
	{
		var options = AppHostOptions.FromConfiguration(builder.Configuration);

		builder
			.AddDockerComposeEnvironment("production")
			.WithDashboard(dashboard => dashboard.WithHostPort(options.Ports.Dashboard));
	}
}
