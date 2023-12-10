using Ecommerce.Identity.API;
using Ecommerce.Identity.API.Repository;
using Ecommerce.Shared;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.
    SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables().Build();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddLogging();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Logging.AddConsole();
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoClient>(item =>
{
    return new MongoClient(connectionString: mongoDbSettings.connectionString);
});
builder.Services.AddSingleton<IRepository, MongoDbRepository>();
var app = builder.Build();



// Configure the HTTP request pipeline.


app.UseSwagger(options =>
{
    options.RouteTemplate = "api/identity/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/identity/swagger/v1/swagger.json", "Identity API");
    //if runnng on IIS, the app will look for the swagger.json at
    //'https://localhost:<port>/<routeprefix>/api/identity/swagger/{documentname}/swagger.json' url
    c.RoutePrefix = "api/identity/swagger";
});


app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();