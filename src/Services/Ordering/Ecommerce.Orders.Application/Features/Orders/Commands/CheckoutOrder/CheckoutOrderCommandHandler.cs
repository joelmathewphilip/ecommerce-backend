using AutoMapper;
using Ecommerce.Orders.Application.Contracts.Infrastructure;
using Ecommerce.Orders.Application.Contracts.Persistence;
using Ecommerce.Orders.Application.Models;
using Ecommerce.Orders.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Orders.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailServices _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailServices emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

       async Task<int> IRequestHandler<CheckoutOrderCommand, int>.Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} created successfully");
            await SendEmail(newOrder);
            return newOrder.Id;
        }

        public async Task SendEmail(Order newOrder)
        {
            try
            {
                var email = new Email()
                {
                    To = newOrder.EmailAddress,
                    Body = "Order was created",
                    Subject = "Order created sucessfully"
                };
                await _emailService.SendEmail(email);
            }
            catch(Exception exception)
            {
                _logger.LogError($"Email for Order {newOrder.Id} could not be sent");
            }
        }
    }
}
