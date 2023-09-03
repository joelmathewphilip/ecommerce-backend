using Ecommerce.Account.API.Interfaces;
using Ecommerce.Account.API;
using Ecommerce.Shared;
using MongoDB.Driver;
using Ecommerce.Account.API.Model;

namespace Ecommerce.Account.API.Repository
{
    public class MongoDbUserRepository : IUserRepository
    {
        private readonly ILogger<MongoDbUserRepository> _logger;
        private readonly IMongoCollection<User> _mongoCollection;
        FilterDefinitionBuilder<User> _filter = Builders<User>.Filter;

        public MongoDbUserRepository(IMongoClient mongoClient, ILogger<MongoDbUserRepository> iLogger)
        {
            _mongoCollection = mongoClient.GetDatabase(Constants.MongoDbUserDatabaseName).GetCollection<User>(Constants.MongoDbUserCollectionName);
            _logger = iLogger;
        }

        public async Task<User?> GetUserAsync(Guid userId)
        {
            try
            {
                _logger.LogInformation($"Started executing {nameof(GetUserAsync)} in MongoDbRepository");
                var filter = _filter.Eq(item => item.Id, userId);
                _logger.LogInformation($"Finished executing {nameof(GetUserAsync)}  in MongoDbRepository");
                return await _mongoCollection.Find(filter).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0} for id {1}", nameof(GetUserAsync), userId));
                return null;
            }
        }
        public async Task<IEnumerable<User>?> GetUsersAsync()
        {
            try
            {
                var filter = _filter.Empty;
                _logger.LogInformation($"Started executing {nameof(GetUsersAsync)} in MongoDbRepository");
                _logger.LogInformation($"Finished executing {nameof(GetUsersAsync)} in MongoDbRepository");
                return await _mongoCollection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(GetUsersAsync)));
                throw;
            }
        }
        public async Task AddUserAsync(User user)
        {
            _logger.LogInformation($"Started executing {nameof(AddUserAsync)} in MongoDbRepository");
            try
            {
                await _mongoCollection.InsertOneAsync(user);
                _logger.LogInformation($"Finished executing {nameof(AddUserAsync)} in MongoDbRepository");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0} for id {1}", nameof(AddUserAsync), user.Id));
                throw;
            }
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            _logger.LogInformation($"Started executing {nameof(DeleteUserAsync)} in MongoDbRepository");
            try
            {
                var filter = _filter.Eq(item => item.Id, userId);
                await _mongoCollection.DeleteOneAsync(filter);
                _logger.LogInformation($"Finished executing {nameof(DeleteUserAsync)} in MongoDbRepository");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0} for id {1}", nameof(DeleteUserAsync), userId));
                throw;
            }
        }



        public async Task UpdateUserAsync(User user)
        {
            try
            {
                var filter = _filter.Eq(item => item.Id, user.Id);
                await _mongoCollection.ReplaceOneAsync(filter, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute ", nameof(UpdateUserAsync));
                throw;
            }
        }
    }
}
