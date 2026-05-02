using Aspire.Hosting.Keycloak;

namespace Overflow.AppHost.Models;

internal sealed record AppInfrastructure(
	IResourceBuilder<PostgresServerResource> Postgres,
	IResourceBuilder<PostgresDatabaseResource> QuestionDb,
	IResourceBuilder<RabbitMQServerResource> RabbitMq,
	IResourceBuilder<KeycloakResource> Keycloak,
	IResourceBuilder<ContainerResource> Typesense,
	IResourceBuilder<ParameterResource> TypesenseApiKey,
	string TypesenseEndpointName)
{
	public EndpointReference TypesenseEndpoint => Typesense.GetEndpoint(TypesenseEndpointName);
}
