namespace Overflow.AppHost.Configuration;

internal sealed class TypesenseInfraOptions
{
	public const string SectionName = "Infrastructure:Typesense";

	public int Port { get; init; } = 8108;
	public string ImageTag { get; init; } = "29.0";
}
