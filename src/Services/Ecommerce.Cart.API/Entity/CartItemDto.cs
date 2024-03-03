using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Cart.API.Entity
{
    public class CartItemDto
    {
        public string itemid { get; set; }

        [Range(0, 1000)]
        public string itemquantity { get; set; }
    }
}
