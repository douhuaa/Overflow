using Microsoft.Extensions.Configuration;
using Overflow.AppHost.Configuration.Options;
using Overflow.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAppEnvironment();

var infraOptions = new InfrastructureOptions();
builder.Configuration
	.GetSection(InfrastructureOptions.SectionName)
	.Bind(infraOptions);

var infrastructure = builder.AddInfrastructure(infraOptions);
var slices = builder.AddAppSlices(infrastructure);

builder.AddGateway(slices);
builder.AddFrontend(infrastructure.Keycloak);
builder.AddReverseProxyIfNeeded();

builder.Build().Run();
