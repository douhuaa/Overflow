using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Overflow.AppHost.Configuration;

namespace Overflow.AppHost.Extensions;

internal static class AppEnvironmentExtensions
{
	public static void AddAppEnvironment(this IDistributedApplicationBuilder builder)
	{
		// Register typed options for non-sensitive AppHost configuration (ports, image tags, virtual hosts).
		// Defaults are defined on AppHostOptions; values can be overridden via appsettings.json or environment variables.
		builder.Services.AddOptions<AppHostOptions>()
			.BindConfiguration("AppHost");

		var options = AppHostOptions.FromConfiguration(builder.Configuration);

		builder
			.AddDockerComposeEnvironment("production")
			.WithDashboard(dashboard => dashboard.WithHostPort(options.Ports.Dashboard));
	}
}
