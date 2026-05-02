using System.Text.RegularExpressions;
using Common;
using Microsoft.Extensions.Options;
using SearchService.Configuration;
using SearchService.Data;
using SearchService.Models;
using Typesense;
using Typesense.Setup;
using Wolverine.RabbitMQ;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

// Register typed options for Typesense connection.
// ApiKey is a secret injected by Aspire at runtime via the "Typesense__ApiKey" environment variable
// (maps to TypesenseOptions.ApiKey through ASP.NET Core config binding convention).
builder.Services.AddOptions<TypesenseOptions>()
	.BindConfiguration(TypesenseOptions.SectionName)
	.ValidateDataAnnotations()
	.ValidateOnStart();

var typesenseUri = builder.Configuration["services:typesense:typesense:0"];

if (string.IsNullOrWhiteSpace(typesenseUri))
	throw new InvalidOperationException("Typesense URI is not found in config");

var typesenseOptions = builder.Configuration
	.GetSection(TypesenseOptions.SectionName)
	.Get<TypesenseOptions>()
	?? throw new InvalidOperationException("Typesense options are not configured");

var uri = new Uri(typesenseUri);
builder.Services.AddTypesenseClient(config =>
{
	config.ApiKey = typesenseOptions.ApiKey;
	config.Nodes =
	[
		new Node(uri.Host, uri.Port.ToString(), uri.Scheme)
	];
});

await builder.UseWolverineWithRabbitMqAsync(opts =>
{
	opts.ListenToRabbitQueue("questions.search",
	cfg =>
	{
		cfg.BindExchange("questions");
	});
	opts.ApplicationAssembly = typeof(Program).Assembly;
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

app.MapGet("/search/similar-titles",
async (string query, ITypesenseClient client) =>
{
	var searchParams = new SearchParameters(query, "title");

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

