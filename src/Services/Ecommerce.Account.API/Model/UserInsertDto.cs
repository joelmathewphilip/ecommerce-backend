using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Account.API.Model
{
    public class UserInsertDto
    {
        [Required]
        public string Name { get; set; }
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Address Address { get; set; }

        public List<Guid>? Orders { get; set; }

        [Required] 
        public string Mobile { get; set; }
        public CartUserInsertDto? Cart { get; set; }
        [Required]
        public string DefaultPaymentMode { get; set; }
    }

    public class CartUserInsertDto
    {
        public double CartTotalCost { get; set; }
        public long CartTotalQuantity { get; set; }
        public Guid CartId { get; set; }
    }

}
