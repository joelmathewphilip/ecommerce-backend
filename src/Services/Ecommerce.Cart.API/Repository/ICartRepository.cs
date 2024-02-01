
using Ecommerce.Cart.API.Entity;

namespace Ecommerce.Cart.API.Repository
{
    public interface ICartRepository
    {
       // public Task<IEnumerable<CartEntity>> GetAllCarts();
        Task<dynamic> AddItem(CartItem cartItem, string cartId);
        public Task<CartEntity> GetCart(string cartId);
        public Task<int> DeleteItem(string cartId, string itemId);
            
    }
}
