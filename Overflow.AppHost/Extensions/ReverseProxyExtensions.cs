using Microsoft.Extensions.Hosting;
using Overflow.AppHost.Configuration;

namespace Overflow.AppHost.Extensions;

internal static class ReverseProxyExtensions
{
	public static void AddReverseProxyIfNeeded(this IDistributedApplicationBuilder builder)
	{
		if (builder.Environment.IsDevelopment())
		{
			return;
		}

		var options = AppHostOptions.FromConfiguration(builder.Configuration);
		builder
			.AddContainer("nginx-proxy", "nginxproxy/nginx-proxy", options.ImageTags.NginxProxy)
			.WithEndpoint(
				port: options.Ports.NginxProxy,
				targetPort: options.Ports.NginxProxy,
				scheme: "http",
				name: "nginx",
				isExternal: true)
			.WithBindMount("/var/run/docker.sock", "/tmp/docker.sock", isReadOnly: true);
	}
}
