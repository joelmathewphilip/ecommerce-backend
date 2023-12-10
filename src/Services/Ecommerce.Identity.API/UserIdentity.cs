using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Identity.API
{
    public class UserIdentity
    {
        [BsonId]
        public Guid userid { get; set; } 
        public string? username { get; set; }
        public byte[] passwordSalt { get; set; }
        public byte[] passwordHash { get; set; }
    }
}
