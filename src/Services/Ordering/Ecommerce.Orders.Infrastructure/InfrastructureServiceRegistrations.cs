using Ecommerce.Orders.Application.Contracts.Infrastructure;
using Ecommerce.Orders.Application.Contracts.Persistence;
using Ecommerce.Orders.Application.Models;
using Ecommerce.Orders.Infrastructure.Mail;
using Ecommerce.Orders.Infrastructure.Persistence;
using Ecommerce.Orders.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Orders.Infrastructure
{
    public static class InfrastructureServiceRegistrations
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServerConnString")));
            //we are writing typeof below because MediatR works using typeof
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailServices, EmailService>();
            return services;
        }
    }
}
