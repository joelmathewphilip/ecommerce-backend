﻿namespace Ecommerce.Catalog.API.Models
{
    public class CatalogItemUpdateDto
    {
        public Guid? CatalogId { get; set; }
        public string? CatalogName { get; set; }
        public string? CatalogDescription { get; set; }
        public string? CatalogMrp { get; set; }
        public string? DiscountedPrice { get; set; }
        public string? CatalogType { get; set; }
        public List<string>? CatalogImages { get; set; }
    }
}
