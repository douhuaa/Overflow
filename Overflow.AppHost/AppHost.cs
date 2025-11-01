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

var questionDb = postgres.AddDatabase("question-db");

var questionService = builder
	.AddProject<QuestionService>("question-service")
	.WithReference(questionDb)
	.WaitFor(questionDb)
	.WithReference(keycloak)
	.WaitFor(keycloak);

builder
	.Build()
	.Run();
