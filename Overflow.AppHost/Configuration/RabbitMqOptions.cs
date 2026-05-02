namespace Overflow.AppHost.Configuration;

internal sealed class RabbitMqOptions
{
	public const string SectionName = "Infrastructure:RabbitMq";

	public int ManagementPort { get; init; } = 15672;
}
