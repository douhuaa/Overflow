using Microsoft.Extensions.Hosting;
using Overflow.AppHost.Configuration;
using Overflow.AppHost.Configuration.Options;

namespace Overflow.AppHost.Extensions;

internal static class ReverseProxyExtensions
{
	public static void AddReverseProxyIfNeeded(
		this IDistributedApplicationBuilder builder,
		ReverseProxyOptions options)
	{
		if (builder.Environment.IsDevelopment())
		{
			return;
		}

		builder
			.AddContainer(AppHostResourceNames.NginxProxy, "nginxproxy/nginx-proxy", options.ImageTag)
			.WithEndpoint(
				port: options.Port,
				targetPort: options.Port,
				scheme: "http",
				name: "nginx",
				isExternal: true)
			.WithBindMount("/var/run/docker.sock", "/tmp/docker.sock", isReadOnly: true);
	}
}
