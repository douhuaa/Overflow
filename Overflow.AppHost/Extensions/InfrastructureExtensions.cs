using Aspire.Hosting.Keycloak;
using Overflow.AppHost.Configuration;
using Overflow.AppHost.Configuration.Options;
using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class InfrastructureExtensions
{
	public static AppInfrastructure AddInfrastructure(
		this IDistributedApplicationBuilder builder,
		InfrastructureOptions? options = null)
	{
		options ??= new InfrastructureOptions();

		var identity = builder
			.AddKeycloak(AppHostResourceNames.Identity, options.Identity.HostPort)
			.WithEnvironment(EnvironmentVariableNames.KcHttpEnabled, "true")
			.WithEnvironment(EnvironmentVariableNames.KcHostnameStrict, "false")
			.WithEnvironment(EnvironmentVariableNames.VirtualHost, options.Identity.VirtualHost)
			.WithEnvironment(EnvironmentVariableNames.VirtualPort, options.Identity.InternalHttpPort.ToString())
			.WithRealmImport("../infra/realms")
			.WithDataVolume("keycloak-data");

		var database = builder
			.AddPostgres(AppHostResourceNames.Postgres, port: options.Postgres.Port)
			.WithDataVolume("postgres-data")
			.WithPgAdmin(pgAdmin => pgAdmin
				.WithHostPort(options.Postgres.PgAdminPort)
				.WithImageTag(options.Postgres.PgAdminImageTag));

		var questionsDb = database.AddDatabase(AppHostResourceNames.QuestionDb);

		var typesenseApiKey = builder.AddParameter(SecretParameterNames.TypesenseApiKey, secret: true);

		var searchEngine = builder
			.AddContainer(AppHostResourceNames.Typesense, "typesense/typesense")
			.WithImageTag(options.Typesense.ImageTag)
			.WithArgs("--data-dir", "/data", "--enable-cors")
			.WithEnvironment(EnvironmentVariableNames.TypesenseApiKey, typesenseApiKey)
			.WithVolume("typesense-data", "/data")
			.WithHttpEndpoint(options.Typesense.Port, options.Typesense.Port, name: AppHostResourceNames.Typesense);

		var messageBus = builder
			.AddRabbitMQ(AppHostResourceNames.Messaging)
			.WithDataVolume("rabbitmq-data")
			.WithManagementPlugin(port: options.RabbitMq.ManagementPort);

		return new AppInfrastructure(
			database,
			questionsDb,
			messageBus,
			identity,
			searchEngine,
			typesenseApiKey);
	}
}
