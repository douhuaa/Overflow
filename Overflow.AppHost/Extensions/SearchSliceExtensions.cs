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
			.WithEnvironment("typesense-api-key", infra.TypesenseApiKey)
			.WithReference(infra.RabbitMq)
			.WaitFor(infra.RabbitMq)
			.WithReference(infra.TypesenseEndpoint)
			.WaitFor(infra.Typesense);
	}
}
