namespace Overflow.AppHost.Configuration.Options;

internal sealed class TypesenseOptions
{
	public const string SectionName = "Infrastructure:Typesense";

	public int Port { get; set; } = 8108;
	public string ImageTag { get; set; } = "29.0";
	public string EndpointName { get; set; } = "typesense";
}
