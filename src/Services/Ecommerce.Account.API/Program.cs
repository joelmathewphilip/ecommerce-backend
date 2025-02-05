using Ecommerce.Account.API.Interfaces;
using Ecommerce.Account.API.Repository;
using Ecommerce.Shared;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>(true)
    .AddEnvironmentVariables();

var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
// Disables adaptive sampling.
aiOptions.EnableAdaptiveSampling = false;
// Disables QuickPulse (Live Metrics stream).
aiOptions.EnableQuickPulseMetricStream = false;
builder.Services.AddApplicationInsightsTelemetry(aiOptions);
builder.Services.AddLogging();

builder.Services.Configure<TelemetryConfiguration>(telemetryConfiguration =>
{
    var telemetryProcessorChainBuilder = telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
    // Using adaptive sampling
    telemetryProcessorChainBuilder.UseAdaptiveSampling(maxTelemetryItemsPerSecond: 5);
    telemetryProcessorChainBuilder.Build();
});


builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration[Constants.AccountIdentityIssuerSettingName],
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration[Constants.AccountIdentitySigningKeySettingName]))
    };
});

builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddAuthorization();
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoClient>(item =>
{
    return new MongoClient(connectionString: mongoDbSettings.connectionString);
});
builder.Services.AddSingleton<IUserRepository, MongoDbUserRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});
//mass transit configuration
builder.Services.AddMassTransit(bus =>
{
    //rabbit mq configuration
    bus.UsingRabbitMq((ctx, factoryConfigurator) =>
    {
        EndpointConvention.Map<AccountCreationEvent>(new Uri("queue:"+builder.Configuration.GetSection(Constants.CartServiceQueueName).Get<string>()));
        string connString = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>().connString;
        if(!string.IsNullOrWhiteSpace(connString))
        {
        factoryConfigurator.Host(new Uri(connString));
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger(options =>
{
    options.RouteTemplate = "api/account/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/account/swagger/v1/swagger.json", "Account API");
    //if runnng on IIS, the app will look for the swagger.json at
    //'https://localhost:<port>/<routeprefix>/api/account/swagger/{documentname}/swagger.json' url
    c.RoutePrefix = "api/account/swagger";
});

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
