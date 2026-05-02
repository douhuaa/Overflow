namespace Overflow.AppHost.Configuration;

internal sealed class IdentityOptions
{
	public const string SectionName = "Infrastructure:Identity";

	public int HostPort { get; init; } = 6001;
	public int InternalHttpPort { get; init; } = 8080;
	public string VirtualHost { get; init; } = "id.overflow.local";

	public void Validate()
	{
		if (HostPort <= 0)
			throw new InvalidOperationException($"{SectionName}:{nameof(HostPort)} must be > 0 (got {HostPort}).");
		if (InternalHttpPort <= 0)
			throw new InvalidOperationException($"{SectionName}:{nameof(InternalHttpPort)} must be > 0 (got {InternalHttpPort}).");
		if (string.IsNullOrWhiteSpace(VirtualHost))
			throw new InvalidOperationException($"{SectionName}:{nameof(VirtualHost)} must not be empty.");
	}
}
