using Aspire.Hosting.Keycloak;
using Overflow.AppHost.Configuration;
using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class InfrastructureExtensions
{
	public static AppInfrastructure AddInfrastructure(this IDistributedApplicationBuilder builder)
	{
		var options = AppHostOptions.FromConfiguration(builder.Configuration);

		var identity = builder
			.AddKeycloak("keycloak", options.Ports.Keycloak)
			.WithEnvironment(EnvironmentVariableNames.KcHttpEnabled, "true")
			.WithEnvironment(EnvironmentVariableNames.KcHostnameStrict, "false")
			.WithEnvironment(EnvironmentVariableNames.VirtualHost, options.VirtualHosts.Identity)
			.WithEnvironment(EnvironmentVariableNames.VirtualPort, options.Ports.KeycloakInternal.ToString())
			.WithRealmImport("../infra/realms")
			.WithDataVolume("keycloak-data");

		var database = builder
			.AddPostgres("postgres", port: options.Ports.Postgres)
			.WithDataVolume("postgres-data")
			.WithPgAdmin(pgAdmin => pgAdmin
				.WithHostPort(options.Ports.PgAdmin)
				.WithImageTag(options.ImageTags.PgAdmin));

		var questionsDb = database.AddDatabase("question-db");

		var typesenseApiKey = builder.AddParameter(AppHostConstants.TypesenseApiKeyParameter, secret: true);

		var searchEngine = builder
			.AddContainer("typesense", "typesense/typesense")
			.WithImageTag(options.ImageTags.Typesense)
			.WithArgs("--data-dir", "/data", "--enable-cors")
			.WithEnvironment(EnvironmentVariableNames.TypesenseApiKey, typesenseApiKey)
			.WithVolume("typesense-data", "/data")
			.WithHttpEndpoint(options.Ports.Typesense, options.Ports.Typesense, name: AppHostConstants.TypesenseEndpointName);

		var messageBus = builder
			.AddRabbitMQ("messaging")
			.WithDataVolume("rabbitmq-data")
			.WithManagementPlugin(port: options.Ports.RabbitMqManagement);

		return new AppInfrastructure(
			database,
			questionsDb,
			messageBus,
			identity,
			searchEngine,
			typesenseApiKey);
	}
}
