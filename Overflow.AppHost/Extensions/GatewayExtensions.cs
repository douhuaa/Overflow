using Overflow.AppHost.Configuration;
using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class GatewayExtensions
{
	public static void AddGateway(
		this IDistributedApplicationBuilder builder,
		AppSlices slices)
	{
		var options = AppHostOptions.FromConfiguration(builder.Configuration);
		var aspNetCoreUrls = $"http://*:{options.Ports.Gateway}";
		builder
			.AddYarp("gateway")
			.WithConfiguration(yarp => MapGatewayRoutes(yarp, slices))
			.WithEnvironment(EnvironmentVariableNames.AspNetCoreUrls, aspNetCoreUrls)
			.WithEnvironment(EnvironmentVariableNames.VirtualHost, options.VirtualHosts.Api)
			.WithEnvironment(EnvironmentVariableNames.VirtualPort, options.Ports.Gateway.ToString())
			.WithEndpoint(
				port: options.Ports.Gateway,
				targetPort: options.Ports.Gateway,
				scheme: "http",
				name: "gateway",
				isExternal: true);
	}

	private static void MapGatewayRoutes(
		IYarpConfigurationBuilder yarp,
		AppSlices slices)
	{
		yarp.AddRoute("/questions/{**catch-all}", slices.QuestionsApi);
		yarp.AddRoute("/tags/{**catch-all}", slices.QuestionsApi);
		yarp.AddRoute("/test/{**catch-all}", slices.QuestionsApi);
		yarp.AddRoute("/search/{**catch-all}", slices.SearchApi);
	}
}
