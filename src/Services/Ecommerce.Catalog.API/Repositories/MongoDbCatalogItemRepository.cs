using Ecommerce.Catalog.API.Interfaces;
using Ecommerce.Catalog.API.Models;
using Ecommerce.Shared;
using MongoDB.Driver;

namespace Ecommerce.Catalog.API.Repositories
{
    public class MongoDbCatalogItemRepository : ICatalogItemRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly ILogger _logger;
        private readonly IMongoCollection<CatalogItem> _mongoCollection;
        private static FilterDefinitionBuilder<CatalogItem> _filter = Builders<CatalogItem>.Filter;
        private readonly string MongoCatalogDatabase = Constants.MongoDbCatalogDatabaseName;
        private readonly string MongoCatalogCollection = Constants.MongoDbCatalogCollectionName;
        public MongoDbCatalogItemRepository(IMongoClient _mongoClient, ILogger<MongoDbCatalogItemRepository> logger)
        {
            _mongoDatabase = _mongoClient.GetDatabase(MongoCatalogDatabase);
            _mongoCollection = _mongoDatabase.GetCollection<CatalogItem>(MongoCatalogCollection);
            _logger = logger;
        }
        public async Task<CatalogItem?> GetCatalogItemAsync(Guid catalogId)
        {
            try
            {
                _logger.LogInformation($"Started executing {nameof(GetCatalogItemAsync)} in MongoDbRepository");
                var filter = _filter.Eq(item => item.CatalogId, catalogId);
                _logger.LogInformation($"Finished executing {nameof(GetCatalogItemAsync)}  in MongoDbRepository");
                return await _mongoCollection.Find(filter).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0} for id {1}", nameof(GetCatalogItemAsync),catalogId));
                return null;
            }
        }
        public async Task<IEnumerable<CatalogItem>?> GetCatalogItemsAsync()
        {
            try
            {
                var filter = _filter.Empty;
                _logger.LogInformation($"Started executing {nameof(GetCatalogItemsAsync)} in MongoDbRepository");
                _logger.LogInformation($"Finished executing {nameof(GetCatalogItemsAsync)} in MongoDbRepository");
                return await _mongoCollection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(GetCatalogItemsAsync)));
                throw;
            }
        }
        public async Task AddCatalogItemAsync(CatalogItem catalogItem)
        {
            _logger.LogInformation($"Started executing {nameof(AddCatalogItemAsync)} in MongoDbRepository");
            try
            {
                await _mongoCollection.InsertOneAsync(catalogItem);
                _logger.LogInformation($"Finished executing {nameof(AddCatalogItemAsync)} in MongoDbRepository");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0} for id {1}", nameof(AddCatalogItemAsync), catalogItem.CatalogId));
                throw;
            }
        }

        public async Task DeleteCatalogItemAsync(Guid catalogId)
        {
            _logger.LogInformation($"Started executing {nameof(DeleteCatalogItemAsync)} in MongoDbRepository");
            try
            {
                var filter = _filter.Eq(item => item.CatalogId, catalogId);
                await _mongoCollection.DeleteOneAsync(filter);
                _logger.LogInformation($"Finished executing {nameof(DeleteCatalogItemAsync)} in MongoDbRepository");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0} for id {1}", nameof(DeleteCatalogItemAsync), catalogId));
                throw;
            }
        }



        public async Task<CatalogItem> UpdateCatalogItemAsync(CatalogItem catalogItem)
        {
            try
            {
                var filter = _filter.Eq(item => item.CatalogId, catalogItem.CatalogId);
                //_mongoCollection.FindOneAndUpdate<CatalogItem>(filter, catalogItem);
                await _mongoCollection.FindOneAndReplaceAsync(filter, catalogItem);
                return await GetCatalogItemAsync(catalogItem.CatalogId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute ",nameof(UpdateCatalogItemAsync));
            throw;
            }
        }
    }
}
