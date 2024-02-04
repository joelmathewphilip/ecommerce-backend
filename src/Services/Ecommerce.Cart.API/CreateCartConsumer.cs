using Ecommerce.Cart.API.Repository;
using EventBus.Messages.Events;
using MassTransit;

namespace Ecommerce.Cart.API
{
    public class CreateCartConsumer : IConsumer<AccountCreationEvent>
    {
        private readonly ILogger<CreateCartConsumer> _logger;
        private readonly ICartRepository cartRepository;
        public CreateCartConsumer(ICartRepository cartRepository, ILogger<CreateCartConsumer> logger)
        {
            this.cartRepository = cartRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<AccountCreationEvent> context)
        {
            try
            {
                _logger.LogInformation("Inside CreateCartConsumer");
                await cartRepository.CreateCart(context.Message.CartId.ToString(), context.Message.AccountId.ToString());
                _logger.LogInformation("Successfully processed CreateCartConsumer message");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while adding data to UserCart", ex);
                throw;
            }
        }
    }
}
