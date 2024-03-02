using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Identity.API.Dto
{
    public class UserIdentityDto
    {
        [BsonId]
        public Guid userid { get; set; }
        public string? username { get; set; }
    }
}
