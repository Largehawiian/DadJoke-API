using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Read the .pfx path from configuration
var certPath = builder.Configuration["Kestrel:Endpoints:Https:Certificate:Path"];

if (!string.IsNullOrEmpty(certPath) && !File.Exists(certPath))
{
    Console.Error.WriteLine($"ERROR: SSL certificate file not found: {certPath}");
    Environment.Exit(1); // Exit with error code
}

// Use Kestrel config from appsettings.json
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. X-API-KEY: {key}",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddResponseCompression();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IJokeService, JokeService>();
builder.Services.AddHttpClient();

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.ConfigObject.AdditionalItems["schemes"] = new[] { "https" };
});

app.UseHttpsRedirection();

app.UseResponseCompression();

app.Use(async (context, next) =>
{
    // Log request details
    var logger = app.Logger;
    logger.LogDebug("Incoming request: {method} {url}", context.Request.Method, context.Request.Path);

    await next();

    // Log response details
    logger.LogDebug("Response status: {statusCode}", context.Response.StatusCode);
});

app.Use(async (context, next) =>
{
    // Allow unauthenticated access to Swagger UI and OpenAPI endpoints
    var path = context.Request.Path.Value;
    if (path != null &&
        (path.StartsWith("/swagger") ||
         path.StartsWith("/favicon.ico") ||
         path.StartsWith("/index.html")))
    {
        await next();
        return;
    }

    var configApiKey = builder.Configuration["ApiKey"];
    if (string.IsNullOrEmpty(configApiKey))
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("API key not configured.");
        return;
    }

    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey) ||
        !string.Equals(extractedApiKey, configApiKey, StringComparison.Ordinal))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized: API key missing or invalid.");
        return;
    }

    await next();
});

app.MapHealthChecks("/health");

app.UseAuthorization();

app.Logger.LogDebug("Application started in {Environment} environment", app.Environment.EnvironmentName);

app.Run();