using Ecommerce.Catalog.API.Models;

namespace Ecommerce.Catalog.API.Interfaces
{
    public interface IDiscountService
    {
        public  Task<dynamic> FetchDiscountedPrice(CatalogItem catalogItem);
    }
}
