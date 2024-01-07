using Ecommerce.Orders.Domain.Entity;
using System.Collections;

namespace Ecommerce.Orders.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable> GetOrdersByUserName(string userName);
    }
}
