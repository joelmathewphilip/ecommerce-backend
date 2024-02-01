using Ecommerce.Cart.API.Extensions;
using Ecommerce.Cart.API.Repository;
using Ecommerce.Shared;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(true)
    .Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
var postgresConfig = builder.Configuration.GetSection("PostgresDbSettings").Get<PostgresDbSettings>();
builder.Services.AddSingleton<IDbConnection>(item =>
{
    return new NpgsqlConnection(postgresConfig.connString);
});
builder.Services.AddSingleton<ICartRepository, PostgresRepo>();
builder.Logging.AddConsole();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CatalogAPI", Version = "v1" });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MigrateDatabase<Program>(postgresConfig.connString);

app.UseSwagger(options =>
{
    options.RouteTemplate = "api/cart/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/cart/swagger/v1/swagger.json", "Cart API");

    //if runnng on IIS, the app will look for the swagger.json at
    //'https://localhost:<port>/<routeprefix>/api/catalog/swagger/{documentname}/swagger.json' url
    c.RoutePrefix = "api/cart/swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();