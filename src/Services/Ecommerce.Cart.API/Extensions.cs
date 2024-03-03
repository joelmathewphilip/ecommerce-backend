using Ecommerce.Cart.API.Entity;

namespace Ecommerce.Cart.API
{
    public static class CartExtensions
    {
        public static CartItem asCartItem(this CartItemDto cartItemDto)
        {
            return new CartItem()
            {
                itemid = cartItemDto.itemid,
                itemquantity = cartItemDto.itemquantity!= null ?  int.Parse(cartItemDto.itemquantity) : 0
            };
        }
    }
}
