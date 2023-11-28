using Ecommerce.Catalog.API.Models;

namespace Ecommerce.Catalog.API.Interfaces
{
    public interface ICatalogItemRepository
    {
        Task AddCatalogItemAsync(CatalogItem catalogItem);
        Task<CatalogItem?> GetCatalogItemAsync(Guid catalogId);
        Task<IEnumerable<CatalogItem>?> GetCatalogItemsAsync();
        Task DeleteCatalogItemAsync(Guid catalogId);
        Task<CatalogItem> UpdateCatalogItemAsync(CatalogItem catalogItem);
    }
}
