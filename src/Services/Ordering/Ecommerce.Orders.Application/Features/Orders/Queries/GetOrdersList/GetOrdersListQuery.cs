using MediatR;

namespace Ecommerce.Orders.Application.Features.Orders.Queries.GetOrdersList
{
    //this class is for read operations in cqrs
    public class GetOrdersListQuery : IRequest<List<OrdersVm>>
    {
        public string Username { get; set; }

        public GetOrdersListQuery(string username)
        {
            Username = username;
        }
    }
}
