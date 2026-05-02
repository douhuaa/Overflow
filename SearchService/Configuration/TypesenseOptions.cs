using System.ComponentModel.DataAnnotations;

namespace SearchService.Configuration;

internal sealed class TypesenseOptions
{
	public const string SectionName = "Typesense";

	/// <summary>
	/// The Typesense server API key.
	/// Injected at runtime by Aspire as a secret parameter — do not hard-code or store in plain-text config files.
	/// </summary>
	[Required]
	public string ApiKey { get; set; } = string.Empty;
}
