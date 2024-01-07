using Ecommerce.Orders.Application.Contracts.Persistence;
using Ecommerce.Orders.Domain.Entity;
using Ecommerce.Orders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Ecommerce.Orders.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        protected readonly OrderContext _orderContext;
        public OrderRepository(OrderContext dbContext): base(dbContext)
        {
            
        }
        public async Task<IEnumerable> GetOrdersByUserName(string username)
        {
            var ordersList = await _orderContext.Orders.Where(a => a.UserName == username).ToListAsync();
            return ordersList;
        }
    }
}
