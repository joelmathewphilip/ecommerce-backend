using Ecommerce.Orders.Domain.Entity;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Orders.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public OrderContextSeed()
        {

        }

        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if(context.Orders.Any()) 
            {
                context.Orders.AddRange(ReturnOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded data successfully with context {DbContext}",typeof(OrderContext));
            }
        }

        public static IEnumerable<Order> ReturnOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    UserName = "JohnDoe",
                    TotalPrice = 99.99M,
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    AddressLine = "123 Main St",
                    Country = "USA",
                    State = "California",
                    ZipCode = "12345",
                    CardName = "John Doe",
                    CardNumber = "1234-5678-9012-3456",
                    Expiration = "12/25",
                    CVV = "123",
                    PaymentMethod = 1 // Assuming 1 represents a specific payment method, adjust accordingly
                }
            };
        }
    }
}
