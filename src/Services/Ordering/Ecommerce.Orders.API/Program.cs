using Ecommerce.Orders.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "api/order/swagger/{documentname}/swagger.json";
    });
    
    app.UseSwaggerUI(c =>
        {
        c.SwaggerEndpoint("/api/order/swagger/v1/swagger.json", "Order API");
        //if runnng on IIS, the app will look for the swagger.json at
        //'https://localhost:<port>/<routeprefix>/api/order/swagger/{documentname}/swagger.json' url
        c.RoutePrefix = "api/order/swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
