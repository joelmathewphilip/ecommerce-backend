﻿using Ecommerce.Catalog.API.Models;
namespace Ecommerce.Account.API.Model
{
    public class Cart
    {
        public double CartTotalCost { get; set; }   
        public long CartTotalQuantity { get; set; }
        public Guid CartId { get; set; }
        public Guid CartUserId { get; set; }
        public List<CatalogItem> CartCatalogItems { get; set; }
    }
}