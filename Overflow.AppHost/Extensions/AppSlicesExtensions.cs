using Overflow.AppHost.Models;

namespace Overflow.AppHost.Extensions;

internal static class AppSlicesExtensions
{
	public static AppSlices AddAppSlices(
		this IDistributedApplicationBuilder builder,
		AppInfrastructure infra)
	{
		var questionsApi = builder.AddQuestionsSlice(infra);
		var searchApi = builder.AddSearchSlice(infra);

		return new AppSlices(questionsApi, searchApi);
	}
}
