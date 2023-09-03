using Ecommerce.Account.API.Model;
using Ecommerce.Catalog.API.Models;

namespace Ecommerce.Account.API.Interfaces
{
    public interface IUserRepository
    {
        Task  AddUserAsync(User user);
        Task<User?> GetUserAsync(Guid userId);
        Task<IEnumerable<User>?> GetUsersAsync();
        Task DeleteUserAsync(Guid userId);
        Task UpdateUserAsync(User user);
    }
}
