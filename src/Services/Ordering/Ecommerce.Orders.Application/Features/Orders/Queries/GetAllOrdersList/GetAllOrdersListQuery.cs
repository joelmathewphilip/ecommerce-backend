using MediatR;

namespace Ecommerce.Orders.Application.Features.Orders.Queries.GetAllOrdersList
{
    public class GetAllOrdersListQuery : IRequest<List<OrdersVm>>
    {

    }
}
