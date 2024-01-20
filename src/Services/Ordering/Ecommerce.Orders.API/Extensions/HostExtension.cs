using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Ecommerce.Orders.API.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContext}", context);
                    //invoke seeder method to seed data into the database
                    InvokeSeeder(seeder, context, services);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occured during data migration to sql server");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability += 1;
                        Thread.Sleep(500);
                        MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                    }
                    throw;
                }
                return host;
            }


        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }

    }
}
