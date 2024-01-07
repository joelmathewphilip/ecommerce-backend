using Ecommerce.Orders.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                new Order() {UserName = "swn", FirstName = "Mehmet", LastName = "Ozkaya", EmailAddress = "ezozkme@gmail.com", AddressLine = "Bahcelievler", Country = "Turkey", TotalPrice = 350 }
            };
        }
    }
}
