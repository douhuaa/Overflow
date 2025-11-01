using Typesense.Setup;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

var typesenseUri = builder.Configuration["services:typesense:typesense:0"];
Console.WriteLine($"{typesenseUri}");

if (string.IsNullOrWhiteSpace(typesenseUri))
	throw new InvalidOperationException("typesense URI is not found in config");

var uri = new Uri(typesenseUri);
builder.Services.AddTypesenseClient(config =>
{
	config.ApiKey = "xyz";
	config.Nodes =
	[
		new Node(uri.Host, uri.Port.ToString(), uri.Scheme)
	];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}
app.MapDefaultEndpoints();

app.Run();
