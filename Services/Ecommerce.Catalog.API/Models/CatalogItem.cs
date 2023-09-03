using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ecommerce.Catalog.API.Models
{
    public record CatalogItem
    {
        [BsonId]
        public Guid CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string CatalogDescription { get; set; }
        public DateTime CatalogDateOfCreation { get; set; }
        public double CatalogMrp { get; set; }
        public double DiscountedPrice { get; set; }
        public string CatalogType { get; set; }
        public List<string>? CatalogImages { get; set; }  

    }
}
