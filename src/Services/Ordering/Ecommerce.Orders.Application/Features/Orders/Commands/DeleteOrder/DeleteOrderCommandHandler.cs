using AutoMapper;
using Ecommerce.Orders.Application.Contracts.Infrastructure;
using Ecommerce.Orders.Application.Contracts.Persistence;
using Ecommerce.Orders.Application.Exceptions;
using Ecommerce.Orders.Application.Features.Orders.Commands.CheckoutOrder;
using Ecommerce.Orders.Application.Models;
using Ecommerce.Orders.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Orders.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailServices _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailServices emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        async Task<Unit> IRequestHandler<DeleteOrderCommand, Unit>.Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderItemToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderItemToDelete == null)
            {
                throw new NotFoundException(nameof(Order),request.Id);
            }
            var order = _mapper.Map<Order>(orderItemToDelete);
            await _orderRepository.DeleteAsync(order);
            await SendEmail(order);
            return Unit.Value;
        }

        private async Task SendEmail(Order order)
        {
            var email = new Email()
            {
                To = order.EmailAddress,
                Body = $"Order {order.Id} is successfully deleted",
                Subject = $"Order deleted"
            };
            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception exception)
            {
                _logger.LogError("An error occured while sending email");
            }
        }
    }
}
