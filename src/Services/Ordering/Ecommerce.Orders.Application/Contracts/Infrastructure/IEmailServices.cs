using Ecommerce.Orders.Application.Models;

namespace Ecommerce.Orders.Application.Contracts.Infrastructure
{
    public interface IEmailServices

    {
        Task<bool> SendEmail(Email email);
    }
}
