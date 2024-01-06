using MediatR;

namespace Ecommerce.Orders.Application.Features.Orders.Commands.DeleteOrdder
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}
