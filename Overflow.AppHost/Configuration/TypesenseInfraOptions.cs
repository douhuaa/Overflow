namespace Overflow.AppHost.Configuration;

internal sealed class TypesenseInfraOptions
{
	public const string SectionName = "Infrastructure:Typesense";

	public int Port { get; init; } = 8108;
	public string ImageTag { get; init; } = "29.0";

	public void Validate()
	{
		if (Port <= 0)
			throw new InvalidOperationException($"{SectionName}:{nameof(Port)} must be > 0 (got {Port}).");
		if (string.IsNullOrWhiteSpace(ImageTag))
			throw new InvalidOperationException($"{SectionName}:{nameof(ImageTag)} must not be empty.");
	}
}
