
using Ecommerce.Cart.API.Entity;

namespace Ecommerce.Cart.API.Repository
{
    public interface ICartRepository
    {
       // public Task<IEnumerable<CartEntity>> GetAllCarts();
        Task<dynamic> AddItem(CartItem cartItem, string cartId);
        public Task<CartEntity> GetCartAll(string cartId);
        public Task<CartCount> GetCart(string cartId);

        public Task<int> DeleteItem(string cartId, string itemId);
        public Task<int> CartExists(string cartid);
        public Task<int> CreateCart(string cartId, string accountId);
            
    }
}
