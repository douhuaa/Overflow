using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class ApplicationSlicesExtensions
{
	public static ApplicationSlices AddApplicationSlices(
		this IDistributedApplicationBuilder builder,
		AppInfrastructure infra)
	{
		var questionsApi = builder.AddQuestionsSlice(infra);
		var searchApi = builder.AddSearchSlice(infra);

		return new ApplicationSlices(questionsApi, searchApi);
	}
}
