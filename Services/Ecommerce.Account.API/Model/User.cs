using Ecommerce.Account.API.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Account.API.Model
{
    public class User
    {
        public string Name { get; set; }
        [BsonId]
        public Guid Id { get; set; } 
        public string Email { get; set; }   
        public DateTime DateOfCreation { get; set; }    
        public Address Address { get; set; }
        public List<Guid> Orders { get; set; }
        public string Mobile { get; set; }
        public UserCart Cart { get;set; }
        public string DefaultPaymentMode { get; set; }
        
    }
    public class UserCart
    {
        public double CartTotalCost { get; set; }
        public long CartTotalQuantity { get; set; }
        public Guid CartId { get; set; }
    }
}
