using Overflow.AppHost.Configuration;
using Overflow.AppHost.Configuration.Options;
using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class GatewayExtensions
{
	public static void AddGateway(
		this IDistributedApplicationBuilder builder,
		AppSlices slices,
		GatewayOptions options)
	{
		var aspNetCoreUrls = $"http://*:{options.Port}";
		builder
			.AddYarp(AppHostResourceNames.Gateway)
			.WithConfiguration(yarp => MapGatewayRoutes(yarp, slices))
			.WithEnvironment(EnvironmentVariableNames.AspNetCoreUrls, aspNetCoreUrls)
			.WithEnvironment(EnvironmentVariableNames.VirtualHost, options.VirtualHost)
			.WithEnvironment(EnvironmentVariableNames.VirtualPort, options.Port.ToString())
			.WithEndpoint(
				port: options.Port,
				targetPort: options.Port,
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
