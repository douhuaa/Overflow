using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using QuestionService.Data;
using QuestionService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Wolverine;
using Wolverine.RabbitMQ;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<TagService>();

builder.AddServiceDefaults();
builder
	.Services
	.AddAuthentication()
	.AddKeycloakJwtBearer(serviceName: "keycloak",
	realm: "overflow",
	options =>
	{
		options.RequireHttpsMetadata = false;
		options.Audience = "overflow";
	});

builder.AddNpgsqlDbContext<QuestionDbContext>("question-db");

builder
	.Services
	.AddOpenTelemetry()
	.WithTracing(providerBuilder =>
	{
		providerBuilder
			.SetResourceBuilder(ResourceBuilder
				.CreateDefault()
				.AddService(builder.Environment.ApplicationName))
			.AddSource("Wolverine");
	});

var retryPolicy = Policy
	.Handle<BrokerUnreachableException>()
	.Or<SocketException>()
	.WaitAndRetryAsync(retryCount: 5,
	retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
	(exception, timeSpan, retryCount) =>
	{
		Console.WriteLine($"Retry attempt {retryCount} failed. Retrying in {timeSpan.Seconds}  seconds...");
	});

await retryPolicy.ExecuteAsync(async () =>
{
	var endpoint = builder.Configuration.GetConnectionString("messaging") ?? throw new InvalidOperationException("messaging connection string not found");
	var factory = new ConnectionFactory
	{
		Uri = new Uri(endpoint)
	};
	await using var connection = await factory.CreateConnectionAsync();
});

builder.Host.UseWolverine(options =>
{
	options
		.UseRabbitMqUsingNamedConnection("messaging")
		.AutoProvision();
	options
		.PublishAllMessages()
		.ToRabbitExchange("questions");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapControllers();

app.MapDefaultEndpoints();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
	var context = services.GetRequiredService<QuestionDbContext>();
	await context.Database.MigrateAsync();
}
catch (Exception e)
{
	var logger = services.GetRequiredService<ILogger<Program>>();
	logger.LogError(e, "An error occurred seeding the DB.");
}

app.Run();
