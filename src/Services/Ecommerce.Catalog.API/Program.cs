using Ecommerce.Catalog.API.Interfaces;
using Ecommerce.Catalog.API.Repositories;
using Ecommerce.Catalog.API.Services;
using Ecommerce.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables().Build() ;

var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();

// Disables adaptive sampling.
aiOptions.EnableAdaptiveSampling = false;

// Disables QuickPulse (Live Metrics stream).
aiOptions.EnableQuickPulseMetricStream = false;

builder.Services.AddApplicationInsightsTelemetry(aiOptions);
builder.Services.AddLogging();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration) ;

builder.Services.AddSwaggerGen();
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoClient>(item =>
{
    return new MongoClient(connectionString: mongoDbSettings.connectionString);
});
builder.Services.AddSingleton<IDiscountService, DiscountService>();
builder.Logging.AddConsole();
builder.Services.AddSingleton<ICatalogItemRepository, MongoDbCatalogItemRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        //ValidateLifetime = true,
        ValidIssuer = "ecommerce.identity.api",
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = "ecommerce.catalog.api",
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("MIIBOQIBAAJAYRdI+qm2sNFdkYVcChA5zSJ7J2Zy5eCutkIZ0AP4rGytANxYiAuVi0CZtR10KfV4qSlCbnWlJhrBX9257/mHzwIDAQABAkAG/VQlp34dcJUZ2s3rc4uVtvvCtF9lKS2qtUuCbCbE0t9R5jdhBDXojCVHbE0TJ8U/rv99ijOYxZqdinYD5fdxAiEApAMCPPqpcxp1L0lMgVdmXsC/UFuZtxEuPXLcR47ydIUCIQCXi7apGs7Sa3liMm5+TQhnvymjQjDXBcY9pqA8iIe1QwIgKjBD8R+hWuRhZGp8bYDn6lO2YptNbRPUSyYyl42jvGkCIQCLm2PciRu68NNTyQ3NQH3bxVlAUvvXOjSUGupGmagbLQIgLxekN9xCUZ6zKLcTc+u5RycM3k51bTXp/VPH7H7IjvE="))
        //ClockSkew = TimeSpan.FromHours(3)
    };
});

builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "api/catalog/swagger/{documentname}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/catalog/swagger/v1/swagger.json", "Catalog API");
        //if runnng on IIS, the app will look for the swagger.json at
        //'https://localhost:<port>/<routeprefix>/api/catalog/swagger/{documentname}/swagger.json' url
        c.RoutePrefix = "api/catalog/swagger";
    });
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();