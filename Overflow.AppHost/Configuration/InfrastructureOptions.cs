using Microsoft.Extensions.Configuration;

namespace Overflow.AppHost.Configuration;

internal sealed class InfrastructureOptions
{
	public const string SectionName = "Infrastructure";

	public IdentityOptions Identity { get; init; } = new();
	public PostgresOptions Postgres { get; init; } = new();
	public TypesenseInfraOptions Typesense { get; init; } = new();
	public RabbitMqOptions RabbitMq { get; init; } = new();

	public static InfrastructureOptions FromConfiguration(IConfiguration configuration) =>
		configuration.GetSection(SectionName).Get<InfrastructureOptions>() ?? new InfrastructureOptions();
}
