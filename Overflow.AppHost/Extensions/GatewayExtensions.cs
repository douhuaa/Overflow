using Overflow.AppHost.Configuration;
using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class GatewayExtensions
{
	public static void AddGateway(
		this IDistributedApplicationBuilder builder,
		ApplicationSlices slices)
	{
		var aspNetCoreUrls = $"http://*:{AppHostConstants.GatewayPort}";
		builder
			.AddYarp("gateway")
			.WithConfiguration(yarp => MapGatewayRoutes(yarp, slices))
			.WithEnvironment(EnvironmentVariableNames.AspNetCoreUrls, aspNetCoreUrls)
			.WithEnvironment(EnvironmentVariableNames.VirtualHost, AppHostConstants.ApiVirtualHost)
			.WithEnvironment(EnvironmentVariableNames.VirtualPort, AppHostConstants.GatewayPort.ToString())
			.WithEndpoint(
				port: AppHostConstants.GatewayPort,
				targetPort: AppHostConstants.GatewayPort,
				scheme: "http",
				name: "gateway",
				isExternal: true);
	}

	private static void MapGatewayRoutes(
		IYarpConfigurationBuilder yarp,
		ApplicationSlices slices)
	{
		yarp.AddRoute("/questions/{**catch-all}", slices.QuestionsApi);
		yarp.AddRoute("/tags/{**catch-all}", slices.QuestionsApi);
		yarp.AddRoute("/test/{**catch-all}", slices.QuestionsApi);
		yarp.AddRoute("/search/{**catch-all}", slices.SearchApi);
	}
}
