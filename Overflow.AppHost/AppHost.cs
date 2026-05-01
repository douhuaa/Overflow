using Overflow.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAppEnvironment();

var infrastructure = builder.AddInfrastructure();

var questionsApi = builder.AddQuestionsSlice(infrastructure);
var searchApi = builder.AddSearchSlice(infrastructure);

builder.AddGateway(questionsApi, searchApi);
builder.AddFrontend(infrastructure.Keycloak);
builder.AddReverseProxyIfNeeded();

builder.Build().Run();
