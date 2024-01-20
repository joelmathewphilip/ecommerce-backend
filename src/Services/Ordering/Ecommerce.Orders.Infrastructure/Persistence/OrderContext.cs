using Ecommerce.Orders.Application.Contracts.Infrastructure;
using Ecommerce.Orders.Application.Models;
using Ecommerce.Orders.Domain.Common_Folder;
using Ecommerce.Orders.Domain.Entity;
using Ecommerce.Orders.Infrastructure.Mail;
using Ecommerce.Orders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Orders.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> dbContextOptions) : base(dbContextOptions)
        {

        }


        public DbSet<Order> Orders { get; set; }



        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken =  new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "swn";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "swn";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
