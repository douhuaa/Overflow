using System.Text.RegularExpressions;
using SearchService.Data;
using SearchService.Models;
using Typesense;
using Typesense.Setup;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

var typesenseUri = builder.Configuration["services:typesense:typesense:0"];

if (string.IsNullOrWhiteSpace(typesenseUri))
	throw new InvalidOperationException("typesense URI is not found in config");

var typesenseApiKey = builder.Configuration["typesense-api-key"];

if (string.IsNullOrWhiteSpace(typesenseApiKey))
	throw new InvalidOperationException("typesense API key is not found in config");

var uri = new Uri(typesenseUri);
builder.Services.AddTypesenseClient(config =>
{
	config.ApiKey = typesenseApiKey;
	config.Nodes =
	[
		new Node(uri.Host, uri.Port.ToString(), uri.Scheme)
	];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}
app.MapDefaultEndpoints();

app.MapGet("/search",
async (string query, ITypesenseClient client) =>
{
	string? tag = null;
	var tagMatch = Regex.Match(query, @"\[(.*?)\]");
	if (tagMatch.Success)
	{
		tag = tagMatch.Groups[1].Value;
		query = query
			.Replace(tagMatch.Value, "")
			.Trim();
	}

	var searchParams = new SearchParameters(query, "title,content");
	if (!string.IsNullOrWhiteSpace(tag))
	{
		searchParams.FilterBy = $"tags:=[{tag}]";
	}

	try
	{
		var result = await client.Search<SearchQuestion>("questions", searchParams);
		return Results.Ok(result.Hits.Select(hit => hit.Document));
	}
	catch (Exception e)
	{
		return Results.Problem("Typesense search failed", e.Message);
	}
});

using var scope = app.Services.CreateScope();
var client = scope.ServiceProvider.GetRequiredService<ITypesenseClient>();
await SearchInitializer.EnsureIndexExists(client);

app.Run();
