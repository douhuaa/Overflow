using Microsoft.Extensions.Configuration;

namespace Overflow.AppHost.Configuration;

internal sealed class InfrastructureOptions
{
	/// <summary>Root configuration section for all infrastructure options.</summary>
	public const string SectionName = "Infrastructure";

	public IdentityOptions Identity { get; init; } = new();
	public PostgresOptions Postgres { get; init; } = new();
	public TypesenseInfraOptions Typesense { get; init; } = new();
	public RabbitMqOptions RabbitMq { get; init; } = new();

	public static InfrastructureOptions FromConfiguration(IConfiguration configuration)
	{
		var opts = new InfrastructureOptions
		{
			Identity = configuration.GetSection(IdentityOptions.SectionName).Get<IdentityOptions>() ?? new IdentityOptions(),
			Postgres = configuration.GetSection(PostgresOptions.SectionName).Get<PostgresOptions>() ?? new PostgresOptions(),
			Typesense = configuration.GetSection(TypesenseInfraOptions.SectionName).Get<TypesenseInfraOptions>() ?? new TypesenseInfraOptions(),
			RabbitMq = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>() ?? new RabbitMqOptions(),
		};

		opts.Identity.Validate();
		opts.Postgres.Validate();
		opts.Typesense.Validate();
		opts.RabbitMq.Validate();

		return opts;
	}
}
