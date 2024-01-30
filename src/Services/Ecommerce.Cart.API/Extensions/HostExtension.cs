using DbUp;
using System.Data.SqlClient;
using System.Reflection;

namespace Ecommerce.Cart.API.Extensions
{
    public static class DatabaseExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, string connString)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                logger.LogInformation("Migrating postresql database.");

                EnsureDatabase.For.PostgresqlDatabase(connString);

                var upgrader = DeployChanges.To
                    .PostgresqlDatabase(connString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                {
                    logger.LogError(result.Error, "An error occurred while migrating the postresql database");
                    return host;
                }

                logger.LogInformation("Migrated postresql database.");
            }

            return host;
        }
    }
}
