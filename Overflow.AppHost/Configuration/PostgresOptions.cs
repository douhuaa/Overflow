namespace Overflow.AppHost.Configuration;

internal sealed class PostgresOptions
{
	public const string SectionName = "Infrastructure:Postgres";

	public int Port { get; init; } = 5432;
	public int PgAdminPort { get; init; } = 5050;
	public string PgAdminImageTag { get; init; } = "9.9";

	public void Validate()
	{
		if (Port <= 0)
			throw new InvalidOperationException($"{SectionName}:{nameof(Port)} must be > 0 (got {Port}).");
		if (PgAdminPort <= 0)
			throw new InvalidOperationException($"{SectionName}:{nameof(PgAdminPort)} must be > 0 (got {PgAdminPort}).");
		if (string.IsNullOrWhiteSpace(PgAdminImageTag))
			throw new InvalidOperationException($"{SectionName}:{nameof(PgAdminImageTag)} must not be empty.");
	}
}
