namespace Overflow.AppHost.Configuration;

internal sealed class RabbitMqOptions
{
	public const string SectionName = "Infrastructure:RabbitMq";

	public int ManagementPort { get; init; } = 15672;

	public void Validate()
	{
		if (ManagementPort <= 0)
			throw new InvalidOperationException($"{SectionName}:{nameof(ManagementPort)} must be > 0 (got {ManagementPort}).");
	}
}
