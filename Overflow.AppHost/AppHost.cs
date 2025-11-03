using Projects;
var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder
	.AddKeycloak("keycloak", 6001)
	.WithDataVolume("keycloak-data");

var postgres = builder
	.AddPostgres("postgres", port: 5432)
	.WithDataVolume("postgres-data")
	.WithPgAdmin(pgAdmin => pgAdmin
		.WithHostPort(5050)
		.WithImageTag("9.9"));

var typesenseApiKey = builder.AddParameter("typesense-api-key", secret: true);
var typesense = builder
	.AddContainer("typesense", "typesense/typesense")
	.WithImageTag("29.0")
	.WithArgs("--data-dir",
	"/data",
	"--api-key",
	typesenseApiKey,
	"--enable-cors")
	.WithVolume("typesense-data", "/data")
	.WithHttpEndpoint(8108, 8108, name: "typesense");

var typesenseContainer = typesense.GetEndpoint("typesense");

var rabbitMq = builder
	.AddRabbitMQ("messaging")
	.WithDataVolume("rabbitmq-data")
	.WithManagementPlugin(port: 15672);

var questionDb = postgres.AddDatabase("question-db");

var questionService = builder
	.AddProject<QuestionService>("question-service")
	.WithReference(rabbitMq)
	.WaitFor(rabbitMq)
	.WithReference(questionDb)
	.WaitFor(questionDb)
	.WithReference(keycloak)
	.WaitFor(keycloak);

var searchService = builder
	.AddProject<SearchService>("search-service")
	.WithEnvironment("typesense-api-key", typesenseApiKey)
	.WithReference(rabbitMq)
	.WaitFor(rabbitMq)
	.WithReference(typesenseContainer)
	.WaitFor(typesense);

var yarp = builder
	.AddYarp("gateway")
	.WithConfiguration(yarpBuilder =>
	{
		yarpBuilder.AddRoute("/questions/{**catch-all}", questionService);
		yarpBuilder.AddRoute("/tags/{**catch-all}", questionService);
		yarpBuilder.AddRoute("/test/{**catch-all}", questionService);
		yarpBuilder.AddRoute("/search/{**catch-all}", searchService);
	})
	.WithEnvironment("ASPNETCORE_URLS", "http://*:8001")
	.WithEndpoint(port: 8001,
	targetPort: 8001,
	scheme: "http",
	name: "gateway",
	isExternal: true);

builder
	.Build()
	.Run();
