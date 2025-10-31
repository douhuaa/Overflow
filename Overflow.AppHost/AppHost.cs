using Projects;
var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder
	.AddKeycloak("keycloak", 6001)
	.WithDataVolume("keycloak-data");

var questionService = builder
	.AddProject<QuestionService>("question-service")
	.WithReference(keycloak)
	.WaitFor(keycloak);

builder
	.Build()
	.Run();
