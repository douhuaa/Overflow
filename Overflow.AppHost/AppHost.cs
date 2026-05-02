using Microsoft.Extensions.Configuration;
using Overflow.AppHost.Configuration.Options;
using Overflow.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var appHostOptions = new AppHostOptions();
builder.Configuration
	.GetSection(AppHostOptions.SectionName)
	.Bind(appHostOptions);

builder.AddAppEnvironment(appHostOptions);

var infraOptions = new InfrastructureOptions();
builder.Configuration
	.GetSection(InfrastructureOptions.SectionName)
	.Bind(infraOptions);

var infrastructure = builder.AddInfrastructure(infraOptions);
var slices = builder.AddAppSlices(infrastructure);

builder.AddGateway(slices, appHostOptions.Gateway);
builder.AddFrontend(infrastructure.Keycloak, appHostOptions.Frontend);
builder.AddReverseProxyIfNeeded(appHostOptions.ReverseProxy);

builder.Build().Run();
