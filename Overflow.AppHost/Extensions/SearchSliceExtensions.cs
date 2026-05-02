using Overflow.AppHost.Configuration;
using Overflow.AppHost.Models;
using Projects;

namespace Overflow.AppHost.Extensions;

internal static class SearchSliceExtensions
{
	public static IResourceBuilder<ProjectResource> AddSearchSlice(
		this IDistributedApplicationBuilder builder,
		AppInfrastructure infra)
	{
		return builder
			.AddProject<SearchService>("search-service")
			// Inject the Typesense API key using the ASP.NET Core double-underscore convention
			// so it maps to TypesenseOptions.ApiKey (section "Typesense", property "ApiKey") in the service.
			.WithEnvironment("Typesense__ApiKey", infra.TypesenseApiKey)
			.WithReference(infra.RabbitMq)
			.WaitFor(infra.RabbitMq)
			.WithReference(infra.TypesenseEndpoint)
			.WaitFor(infra.Typesense);
	}
}
