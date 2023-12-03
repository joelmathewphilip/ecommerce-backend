using MongoDB.Bson.Serialization.Attributes;

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
        //DiscountedPrice should be calculated using the CouponService
        public double DiscountedPrice { get; set; }
        public string CatalogType { get; set; }
        public List<string>? CatalogImages { get; set; }  

    }
}
