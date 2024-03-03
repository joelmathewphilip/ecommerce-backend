namespace Ecommerce.Cart.API.Entity
{
    public class CartEntity
    {
        public Guid Id { get; set; }

        public List<CartItem> cartItems { get; set; } 

        public double totalCost { get; set; }
        
        public long noOfItems { get; set; }

    }

    public class CartItem
    {
        public string itemid { get; set; }

        public int itemquantity { get; set; }
        
        public double itemcost { get; set; }
    }
}
