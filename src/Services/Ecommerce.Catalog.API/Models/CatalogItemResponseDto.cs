namespace Ecommerce.Catalog.API.Models
{
    public class CatalogItemResponseDto
    {
        public Guid CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string CatalogDescription { get; set; }
        public DateTime CatalogDateOfCreation { get; set; }
        public double DiscountedPrice { get; set; }
        public double CatalogMrp { get; set; }
        public string CatalogType { get; set; }
        public int CatalogQuantity { get; set; }
        public List<string> CatalogImages { get; set; }
    }
}
