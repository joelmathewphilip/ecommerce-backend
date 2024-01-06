using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Orders.Application.Behaviours
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch(Exception exception)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(exception,$"Unhandled exception for request {requestName}");
                throw;
            }
        }
    }
}
