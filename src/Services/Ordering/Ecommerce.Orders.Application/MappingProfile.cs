using AutoMapper;
using Ecommerce.Orders.Application.Features.Orders.Commands.CheckoutOrder;
using Ecommerce.Orders.Application.Features.Orders.Commands.UpdateOrder;
using Ecommerce.Orders.Domain.Entity;

namespace Ecommerce.Orders.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, Features.Orders.Queries.GetAllOrdersList.OrdersVm>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order,UpdateOrderCommand>().ReverseMap();
            CreateMap<Order, Features.Orders.Queries.GetOrdersList.OrdersVm>().ReverseMap();
        }
    }
}
