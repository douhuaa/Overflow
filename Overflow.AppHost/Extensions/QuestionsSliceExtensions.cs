using Overflow.AppHost.Models;
using Projects;

namespace Overflow.AppHost.Extensions;

internal static class QuestionsSliceExtensions
{
	public static IResourceBuilder<ProjectResource> AddQuestionsSlice(
		this IDistributedApplicationBuilder builder,
		AppInfrastructure infra)
	{
		return builder
			.AddProject<QuestionService>("question-service")
			.WithReference(infra.RabbitMq)
			.WaitFor(infra.RabbitMq)
			.WithReference(infra.QuestionDb)
			.WaitFor(infra.QuestionDb)
			.WithReference(infra.Keycloak)
			.WaitFor(infra.Keycloak);
	}
}
