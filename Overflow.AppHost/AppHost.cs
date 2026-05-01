using Overflow.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAppEnvironment();

var infrastructure = builder.AddInfrastructure();
var slices = builder.AddApplicationSlices(infrastructure);

builder.AddGateway(slices);
builder.AddFrontend(infrastructure.Keycloak);
builder.AddReverseProxyIfNeeded();

builder.Build().Run();
