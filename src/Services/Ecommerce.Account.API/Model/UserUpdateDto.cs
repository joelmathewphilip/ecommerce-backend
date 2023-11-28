using Ecommerce.Account.API.Model;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ecommerce.Account.API.Model
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Address? Address { get; set; }
        public List<Guid>? Orders { get; set; }
        public string? Mobile { get; set; }
        public CartUserUpdateDto? Cart { get; set; }
        public string? DefaultPaymentMode { get; set; }
    }

    public class CartUserUpdateDto
    {
        public double CartTotalCost { get; set; }
        public long CartTotalQuantity { get; set; }
        public Guid CartId { get; set; }
    }
}
