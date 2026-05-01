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

		builder
			.AddContainer("nginx-proxy", "nginxproxy/nginx-proxy", AppHostConstants.ImageTags.NginxProxy)
			.WithEndpoint(
				port: AppHostConstants.NginxProxyPort,
				targetPort: AppHostConstants.NginxProxyPort,
				scheme: "http",
				name: "nginx",
				isExternal: true)
			.WithBindMount("/var/run/docker.sock", "/tmp/docker.sock", isReadOnly: true);
	}
}
