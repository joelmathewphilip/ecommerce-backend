using Ecommerce.Shared;
using MongoDB.Driver;

namespace Ecommerce.Identity.API.Repository
{
    public class MongoDbRepository : IRepository
    {
        private readonly ILogger<MongoDbRepository> _logger;
        private readonly IMongoCollection<UserIdentity> _mongoCollection;
        private readonly FilterDefinitionBuilder<UserIdentity> _filterDefinition = Builders<UserIdentity>.Filter;
        public MongoDbRepository(IMongoClient mongoClient, ILogger<MongoDbRepository> logger)
        {
            _logger = logger;
            _mongoCollection = mongoClient.GetDatabase(Constants.MongoDbUserDatabaseName).GetCollection<UserIdentity>(Constants.MongoDbUserIdentityCollectionName);
        }
        public async Task AddData(UserIdentity userIdentity)
        {
            _logger.LogInformation("Started executing " + nameof(AddData));
            try
            {
                 await _mongoCollection.InsertOneAsync(userIdentity);
                _logger.LogInformation("Finished executing " + nameof(AddData));

            }
            catch(Exception exception)
            {
                _logger.LogError("Error occured while running " + nameof(AddData)+":", exception);
                throw;
            }
        }

        public async Task<UserIdentity> FetchRegisteredUsers(string username)
        {
            try
            {
                var filter = _filterDefinition.Eq(item => item.username,username);
                var value  =  await _mongoCollection.Find(filter).FirstOrDefaultAsync();
                return value;
            }
            catch(Exception exception)
            {
                _logger.LogError("An error occured while running" + nameof(FetchRegisteredUsers)+"," + exception);
                throw;
            }
        }
    }
}
