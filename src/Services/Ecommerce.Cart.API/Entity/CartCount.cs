namespace Ecommerce.Cart.API.Entity
{
    public class CartCount
    {
        public Guid CartId { get; set; }
        public int cartCount { get; set; }
        public double cartCost { get; set; }
    }
}
