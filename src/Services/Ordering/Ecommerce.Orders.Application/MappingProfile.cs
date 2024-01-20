using AutoMapper;
using Ecommerce.Orders.Application.Features.Orders.Commands.CheckoutOrder;
using Ecommerce.Orders.Application.Features.Orders.Commands.DeleteOrder;
using Ecommerce.Orders.Application.Features.Orders.Commands.UpdateOrder;
using Ecommerce.Orders.Application.Features.Orders.Queries.GetOrdersList;
using Ecommerce.Orders.Domain.Entity;

namespace Ecommerce.Orders.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVm>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order,UpdateOrderCommand>().ReverseMap();
        }
    }
}
