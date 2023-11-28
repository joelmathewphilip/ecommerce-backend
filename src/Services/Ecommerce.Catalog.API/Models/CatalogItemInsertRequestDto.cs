namespace Ecommerce.Catalog.API.Models
{
    public class CatalogItemInsertRequestDto
    {
        public string CatalogName { get; set; }
        public string CatalogDescription { get; set; }
        public double CatalogMrp { get; set; }
        public string CatalogType { get; set; }
        public List<string>? CatalogImages { get; set; }
    }
}
