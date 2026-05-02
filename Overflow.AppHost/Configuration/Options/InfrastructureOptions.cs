namespace Overflow.AppHost.Configuration.Options;

internal sealed class InfrastructureOptions
{
	public const string SectionName = "Infrastructure";

	public IdentityOptions Identity { get; set; } = new();
	public PostgresOptions Postgres { get; set; } = new();
	public TypesenseOptions Typesense { get; set; } = new();
	public RabbitMqOptions RabbitMq { get; set; } = new();
}
