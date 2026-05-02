namespace Overflow.AppHost.Configuration.Options;

internal sealed class PostgresOptions
{
	public const string SectionName = "Infrastructure:Postgres";

	public int Port { get; set; } = 5432;
	public int PgAdminPort { get; set; } = 5050;
	public string PgAdminImageTag { get; set; } = "9.9";
}
