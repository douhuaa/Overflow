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

var questionDb = postgres.AddDatabase("question-db");

var questionService = builder
	.AddProject<QuestionService>("question-service")
	.WithReference(questionDb)
	.WaitFor(questionDb)
	.WithReference(keycloak)
	.WaitFor(keycloak);

var searchService = builder
	.AddProject<SearchService>("search-service")
	.WithEnvironment("typesense-api-key", typesenseApiKey)
	.WithReference(typesenseContainer)
	.WaitFor(typesense);

builder
	.Build()
	.Run();
