using Overflow.AppHost.Configuration;

namespace Overflow.AppHost.Extensions;

internal static class GatewayExtensions
{
	public static void AddGateway(
		this IDistributedApplicationBuilder builder,
		IResourceBuilder<ProjectResource> questionsApi,
		IResourceBuilder<ProjectResource> searchApi)
	{
		var aspNetCoreUrls = $"http://*:{AppHostConstants.GatewayPort}";
		builder
			.AddYarp("gateway")
			.WithConfiguration(yarp => MapGatewayRoutes(yarp, questionsApi, searchApi))
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
		IResourceBuilder<ProjectResource> questionsApi,
		IResourceBuilder<ProjectResource> searchApi)
	{
		yarp.AddRoute("/questions/{**catch-all}", questionsApi);
		yarp.AddRoute("/tags/{**catch-all}", questionsApi);
		yarp.AddRoute("/test/{**catch-all}", questionsApi);
		yarp.AddRoute("/search/{**catch-all}", searchApi);
	}
}
