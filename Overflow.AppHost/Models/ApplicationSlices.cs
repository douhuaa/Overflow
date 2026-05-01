namespace Overflow.AppHost.Models;

internal sealed record ApplicationSlices(
	IResourceBuilder<ProjectResource> QuestionsApi,
	IResourceBuilder<ProjectResource> SearchApi);
