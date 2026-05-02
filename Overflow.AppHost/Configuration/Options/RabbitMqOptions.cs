namespace Overflow.AppHost.Configuration.Options;

internal sealed class RabbitMqOptions
{
	public const string SectionName = "Infrastructure:RabbitMq";

	public int ManagementPort { get; set; } = 15672;
}
