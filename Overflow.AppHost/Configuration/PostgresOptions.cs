namespace Overflow.AppHost.Configuration;

internal sealed class PostgresOptions
{
	public const string SectionName = "Infrastructure:Postgres";

	public int Port { get; init; } = 5432;
	public int PgAdminPort { get; init; } = 5050;
	public string PgAdminImageTag { get; init; } = "9.9";
}
