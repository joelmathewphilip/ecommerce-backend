using AutoMapper;
using Ecommerce.Orders.Application.Contracts.Persistence;
using MediatR;

namespace Ecommerce.Orders.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var ordersList =  await _orderRepository.GetOrdersByUserName(request.Username);
            return _mapper.Map<List<OrdersVm>>(ordersList);

        }
    }
}
