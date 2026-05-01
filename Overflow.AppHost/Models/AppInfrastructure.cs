using Aspire.Hosting.Keycloak;
using Overflow.AppHost.Configuration;

namespace Overflow.AppHost.Models;

internal sealed record AppInfrastructure(
	IResourceBuilder<PostgresServerResource> Postgres,
	IResourceBuilder<PostgresDatabaseResource> QuestionDb,
	IResourceBuilder<RabbitMQServerResource> RabbitMq,
	IResourceBuilder<KeycloakResource> Keycloak,
	IResourceBuilder<ContainerResource> Typesense,
	IResourceBuilder<ParameterResource> TypesenseApiKey)
{
	public EndpointReference TypesenseEndpoint => Typesense.GetEndpoint(AppHostConstants.TypesenseEndpointName);
}
