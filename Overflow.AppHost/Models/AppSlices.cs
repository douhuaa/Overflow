namespace Overflow.AppHost.Models;

internal sealed record AppSlices(
	IResourceBuilder<ProjectResource> QuestionsApi,
	IResourceBuilder<ProjectResource> SearchApi);
