using Aspire.Hosting.Keycloak;
using Overflow.AppHost.Configuration;
using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class InfrastructureExtensions
{
	public static AppInfrastructure AddInfrastructure(this IDistributedApplicationBuilder builder)
	{
		var identity = builder
			.AddKeycloak("keycloak", AppHostConstants.KeycloakPort)
			.WithEnvironment(EnvironmentVariableNames.KcHttpEnabled, "true")
			.WithEnvironment(EnvironmentVariableNames.KcHostnameStrict, "false")
			.WithEnvironment(EnvironmentVariableNames.VirtualHost, AppHostConstants.IdentityVirtualHost)
			.WithEnvironment(EnvironmentVariableNames.VirtualPort, AppHostConstants.DashboardPort.ToString())
			.WithRealmImport("../infra/realms")
			.WithDataVolume("keycloak-data");

		var database = builder
			.AddPostgres("postgres", port: AppHostConstants.PostgresPort)
			.WithDataVolume("postgres-data")
			.WithPgAdmin(pgAdmin => pgAdmin
				.WithHostPort(AppHostConstants.PgAdminPort)
				.WithImageTag(AppHostConstants.ImageTags.PgAdmin));

		var questionsDb = database.AddDatabase("question-db");

		var typesenseApiKey = builder.AddParameter("typesense-api-key", secret: true);

		var searchEngine = builder
			.AddContainer("typesense", "typesense/typesense")
			.WithImageTag(AppHostConstants.ImageTags.Typesense)
			.WithArgs("--data-dir", "/data", "--enable-cors")
			.WithEnvironment(EnvironmentVariableNames.TypesenseApiKey, typesenseApiKey)
			.WithVolume("typesense-data", "/data")
			.WithHttpEndpoint(AppHostConstants.TypesensePort, AppHostConstants.TypesensePort, name: "typesense");

		var messageBus = builder
			.AddRabbitMQ("messaging")
			.WithDataVolume("rabbitmq-data")
			.WithManagementPlugin(port: AppHostConstants.RabbitMqManagementPort);

		return new AppInfrastructure(
			database,
			questionsDb,
			messageBus,
			identity,
			searchEngine,
			typesenseApiKey);
	}
}
