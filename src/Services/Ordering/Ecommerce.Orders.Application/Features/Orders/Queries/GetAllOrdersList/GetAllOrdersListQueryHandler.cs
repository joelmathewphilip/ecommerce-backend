using AutoMapper;
using Ecommerce.Orders.Application.Contracts.Persistence;
using MediatR;

namespace Ecommerce.Orders.Application.Features.Orders.Queries.GetAllOrdersList
{
    public class GetAllOrdersListQueryHandler : IRequestHandler<GetAllOrdersListQuery,List<OrdersVm>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetAllOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrdersVm>> Handle(GetAllOrdersListQuery request, CancellationToken cancellationToken)
        {
            var ordersList = await _orderRepository.GetAllAsync();
            return _mapper.Map<List<OrdersVm>>(ordersList);
        }
    }
}
